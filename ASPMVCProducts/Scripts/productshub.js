function endsWith(str, suffix) {
    return str.indexOf(suffix, str.length - suffix.length) !== -1;
}

$(function () {
    var lDeletingProductList = false;
    var lEditingProductListEntry = false;
    var lDeletingProductListEntry = false;
    var lEditingProductEntry = false;

    var lDeleteProductListForm = document.getElementById("form-productlistDelete");
    if (lDeleteProductListForm) {
        lDeleteProductListForm.onsubmit = function () {
            lDeletingProductList = true;
        }
    }

    var lEditProductListEntryForm = document.getElementById("form-productlistentryEdit");
    if (lEditProductListEntryForm) {
        lEditProductListEntryForm.onsubmit = function () {
            lEditingProductListEntry = true;
        }
    }

    var lDeleteProductListEntryForm = document.getElementById("form-productlistentryDelete");
    if (lDeleteProductListEntryForm) {
        lDeleteProductListEntryForm.onsubmit = function () {
            lDeletingProductListEntry = true;
        }
    }

    $(".div-productlist").hover(
      function () { $(this).toggleClass("view-candidate"); }
    );
    $(".div-productlist-delete").hover(
      function () { $(this).parent("div").toggleClass("delete-candidate"); }
    );

    // Reference the auto-generated proxy for the hub.  
    var productsHub = $.connection.productsHub;
    // Create a function that the hub can call back to display messages.
    productsHub.client.onServerEvent = function (aEventName, aEventData) {
        switch (aEventName) {
            case "ProductListCreated":
                {
                    var lListsURL = "/ProductLists";
                    var lAntiForgeryToken = $("form[id='form-productlistAdd'] input[name='__RequestVerificationToken']").val();
                    if (document.URL.endsWith(lListsURL)) {
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
                                        '<input type="image" src="Images/delete.png"/>' + 
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
                    if (document.URL.endsWith(lListsURL)) {
                        var lElement = document.getElementById("div-productlist-" + aEventData.Id);
                        lElement.parentNode.removeChild(lElement);
                    }//viewing the delete page of the list whit it is deleted 
                    else if (document.URL.endsWith(lDeleteURL)) {
                        if (lDeletingProductList == false) {
                            alert("The list was deleted. You will be redirected to product lists page");
                            window.location = document.URL.substring(0, document.URL.length - lDeleteURL.length) + lListsURL;
                        }
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
                    if (document.URL.endsWith(lListURL)) {
                        $('#table-productlistentries').append(
                            '<tr id="tr-productlistentry-' + aEventData.Id + '">' +
                                '<td>' +
                                    aEventData.Name +
                                '</td>' +
                            '<td>' +
                                '<a href="/ProductLists/' + aEventData.ListId + '/Edit/' + aEventData.Id + '">Edit</a>&nbsp;' +
                                '<a href="/ProductLists/' + aEventData.ListId + '/Delete/' + aEventData.Id + '">Delete</a>' +
                            '</td>' +
                        '</tr>');
                    }
                }
                break;
            case "ProductListEntryDeleted":
                {
                    var lListURL = "/ProductLists/" + aEventData.ListId;
                    var lEditURL = lListURL + "/Edit/" + aEventData.Id;
                    var lDeleteURL = lListURL + "/Delete/" + aEventData.Id;
                    //Vieweing the list from which a product entry was deleted
                    if (document.URL.endsWith(lListURL)) {
                        var lElement = document.getElementById("tr-productlistentry-" + aEventData.Id);
                        lElement.parentNode.removeChild(lElement);
                    } //Editing the deleted product entry
                    else if (document.URL.endsWith(lEditURL)) {
                        alert("The product entry was deleted. You will be redirected to product list");
                        window.location = document.URL.substring(0, document.URL.length - lEditURL.length) + lListURL;
                    }
                    else if (document.URL.endsWith(lDeleteURL)) {
                        if (lDeletingProductListEntry == false) {
                            alert("The product entry was deleted. You will be redirected to product list");
                            window.location = document.URL.substring(0, document.URL.length - lDeleteURL.length) + lListURL;
                        }
                    }
                }
                break;
            case "ProductListEntryEdited":
                {
                    var lListURL = "/ProductLists/" + aEventData.ListId;
                    var lEditURL = lListURL + "/Edit/" + aEventData.Id;
                    if (document.URL.endsWith(lEditURL)) {
                        if (lEditingProductListEntry == false) {
                            alert("The product entry was modified. Page will be refreshed");
                            location.reload();
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