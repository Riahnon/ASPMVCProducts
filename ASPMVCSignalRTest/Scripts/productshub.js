function endsWith(str, suffix) {
    return str.indexOf(suffix, str.length - suffix.length) !== -1;
}

$(function () {
    var lEditingProductEntry = false;


    var lEditProductEntryForm = document.getElementById("form-productentryEdit");

    if (lEditProductEntryForm) {
        lEditProductEntryForm.onsubmit = function () {
            lEditingProductEntry = true;
        }
    }

    $(".div-productlist").hover(
      function () { $(this).toggleClass("view-candidate"); }
    );
    $(".div-productlist-delete").hover(
      function () { $(this).parent("div").toggleClass("delete-candidate"); }
    );
    $(".div-productentry").hover(
     function () { $(this).toggleClass("view-candidate"); }
   );
    $(".div-productentry-delete").hover(
      function () { $(this).parent("div").toggleClass("delete-candidate"); }
    );
    //Detect internet explorer and customize styles
    if (window.navigator.userAgent.indexOf("MSIE ") > 0)
    {
        $(".div-productlist").css("height", "28px");
        $(".div-productlist-name").css("width", "calc(100% - 30px)");

        $(".div-productentries").css("height", "28px");
        $(".div-productentry-name").css("width", "calc(100% - 30px)");
    }

    // Reference the auto-generated proxy for the hub.  
    var productsHub = $.connection.productsHub;
    // Create a function that the hub can call back to display messages.
    productsHub.client.onServerEvent = function (aEventName, aEventData) {
        switch (aEventName) {
            case "ProductListCreated":
                {
                    var lListsURL = "/ProductLists";
                    var lAntiForgeryToken = $("form[id='form-productlistAdd'] input[name='__RequestVerificationToken']").val();
                    var lListId = "div-productlist-" + aEventData.Id;
                    if (endsWith(document.URL, lListsURL)) {
                        $('#div-productlists').append(
                            '<div id="' + lListId + '" class="div-productlist">' +
                                '<div class="div-productlist-name">' +
                                    '<a href="/ProductLists/' + aEventData.Id + '">' + 
                                        aEventData.Name +
                                    '</a>' +
                                '</div>' +
                                '<div class="div-productlist-delete">' +
                                    '<form id="form-productlistDelete" action="/ProductLists/Delete/' + aEventData.Id + '" method="post">' + 
                                        '<input name="__RequestVerificationToken" type="hidden" value="' + lAntiForgeryToken + '" />' + 
                                        '<input type="image" src="/Images/delete.png"/>' + 
                                    '</form>' + 
                                '</div>' +
                            '</div>');
                        //Extra CSS needed for IE
                        if (window.navigator.userAgent.indexOf("MSIE ") > 0) {
                            $("#" + lListId ).css("height", "28px");
                            $("#" + lListId + " .div-productlist-name").css("width", "calc(100% - 30px)");
                        }
                        //hover events for newly generated list
                        $("#" + lListId).hover(
                             function () { $(this).toggleClass("view-candidate"); }
                        );
                        $("#" + lListId + " .div-productlist-delete").hover(
                            function () { $(this).parent("div").toggleClass("delete-candidate"); }
                        );

                    }
                }
                break;
            case "ProductListDeleted":
                {
                    var lListsURL = "/ProductLists";
                    var lDeleteURL = lListsURL + "/Delete/" + aEventData.Id;
                    var lListOperationURL = lListsURL + "/" + aEventData.Id;
                    //Vieweing product lists
                    if (endsWith(document.URL, lListsURL)) {
                        var lElement = document.getElementById("div-productlist-" + aEventData.Id);
                        lElement.parentNode.removeChild(lElement);
                    }//Working with the deleted list
                    else if (document.URL.lastIndexOf(lListOperationURL) != -1) {
                        alert("The product list was deleted. You will be redirected to product lists page");
                        window.location = document.URL.substring(0, document.URL.lastIndexOf(lListOperationURL)) + lListsURL;
                    }
                }
                break;
            case "ProductEntryCreated":
                {
                    var lListURL = "/ProductLists/" + aEventData.ListId;
                    var lAntiForgeryToken = $("form[id='form-productentryAdd'] input[name='__RequestVerificationToken']").val();
                    var lEntryId = "div-productentry-" + aEventData.Id;
                    if (endsWith(document.URL, lListURL)) {
                        $('#div-productentries').append(
                            '<div id="' + lEntryId + '" class="div-productentry">' +
                                '<div class="div-productentry-name">' +
                                    '<a href="/ProductLists/' + aEventData.ListId  + '/Edit/' + aEventData.Id + '">' +
                                        aEventData.Name +
                                    '</a>' +
                                '</div>' +
                                '<div class="div-productentry-delete">' +
                                    '<form id="form-productentryDelete" action="/ProductLists/' + aEventData.ListId + '/Delete/' + aEventData.Id + '" method="post">' +
                                        '<input name="__RequestVerificationToken" type="hidden" value="' + lAntiForgeryToken + '" />' +
                                        '<input type="image" src="/Images/delete.png"/>' +
                                    '</form>' +
                                '</div>' +
                            '</div>');
                        //Extra CSS needed for IE
                        if (window.navigator.userAgent.indexOf("MSIE ") > 0) {
                            $("#" + lEntryId).css("height", "28px");
                            $("#" + lEntryId + " .div-productentry-name").css("width", "calc(100% - 30px)");
                        }
                        //hover events for newly generated entry
                        $("#" + lEntryId).hover(
                             function () { $(this).toggleClass("view-candidate"); }
                        );
                        $("#" + lEntryId + " .div-productentry-delete").hover(
                            function () { $(this).parent("div").toggleClass("delete-candidate"); }
                        );
                    }
                }
                break;
            case "ProductEntryDeleted":
                {
                    var lListURL = "/ProductLists/" + aEventData.ListId;
                    var lEditURL = lListURL + "/Edit/" + aEventData.Id;
                    var lDeleteURL = lListURL + "/Delete/" + aEventData.Id;
                    //Vieweing the list from which a product entry was deleted
                    if (endsWith(document.URL, lListURL)) {
                        var lElement = document.getElementById("div-productentry-" + aEventData.Id);
                        lElement.parentNode.removeChild(lElement);
                    } //Editing the deleted product entry
                    else if (endsWith(document.URL, lEditURL)) {
                        alert("The product entry was deleted. You will be redirected to product list");
                        window.location = document.URL.substring(0, document.URL.length - lEditURL.length) + lListURL;
                    }
                }
                break;
            case "ProductEntryEdited":
                {
                    var lListURL = "/ProductLists/" + aEventData.ListId;
                    var lEditURL = lListURL + "/Edit/" + aEventData.Id;
                    if (endsWith(document.URL, lEditURL)) {
                        if (lEditingProductEntry == false) {
                            $("input[id='Amount']").val(aEventData.Amount);
                            $("textarea[id='Comments']").val(aEventData.Comments);
                        }
                    }
                }
                break;
        }
        // Add the message to the page. 
        
    };
    // Start the connection.
    $.connection.hub.start().done(function () {
        //productsHub.server.NotifyEvent);
    });
});
// This optional function html-encodes messages for display in the page.
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}