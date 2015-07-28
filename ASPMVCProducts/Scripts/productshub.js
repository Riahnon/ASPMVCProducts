function endsWith(str, suffix) {
    return str.indexOf(suffix, str.length - suffix.length) !== -1;
}

$(function () {
    var lEditingProductListEntry = false;
    var lEditingProductEntry = false;


    var lEditProductListEntryForm = document.getElementById("form-productlistentryEdit");
    if (lEditProductListEntryForm) {
        lEditProductListEntryForm.onsubmit = function () {
            lEditingProductListEntry = true;
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
    if (window.navigator.userAgent.indexOf("MSIE ") > 0)      // If Internet Explorer, return version number
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
                    if (endsWith(document.URL, lListsURL)) {
                        $('#div-productlists').append(
                            '<div id="div-productlist-' + aEventData.Id + '" class="div-productlist">' +
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
            case "ProductListEntryCreated":
                {
                    var lListURL = "/ProductLists/" + aEventData.ListId;
                    var lAntiForgeryToken = $("form[id='form-productentryAdd'] input[name='__RequestVerificationToken']").val();
                    if (endsWith(document.URL, lListURL)) {
                        $('#div-productentries').append(
                            '<div id="div-productentry-' + aEventData.Id + '" class="div-productentry">' +
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
                    }
                }
                break;
            case "ProductListEntryDeleted":
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
            case "ProductListEntryEdited":
                {
                    var lListURL = "/ProductLists/" + aEventData.ListId;
                    var lEditURL = lListURL + "/Edit/" + aEventData.Id;
                    if (endsWith(document.URL, lEditURL)) {
                        if (lEditingProductListEntry == false) {
                            $("input[id='Amount']").val(aEventData.Amount);
                            $("textarea[id='Comments']").val(aEventData.Comments);
                            /*alert("The product entry was modified. Page will be refreshed");
                            location.reload();*/
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