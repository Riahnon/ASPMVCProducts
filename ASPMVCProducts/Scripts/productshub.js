function endsWith(str, suffix) {
    return str.indexOf(suffix, str.length - suffix.length) !== -1;
}

$(function () {
    // Reference the auto-generated proxy for the hub.  
    var productsHub = $.connection.productsHub;
    // Create a function that the hub can call back to display messages.
    productsHub.client.onServerEvent = function (aEventName, aEventData) {
        switch (aEventName) {
            case "ProductListCreated":
                if (document.URL.endsWith("/ProductLists")) {
                    $('#table-productlists').append(
                        '<tr i>' + 
                            '<td>' +
                                aEventData.Name + 
                            '</td>' +
                        '<td>' + 
                            '<a href="/ProductLists/' + aEventData.Id + '">Edit</a>&nbsp;|&nbsp;&nbsp;' +
                            '<a href="/ProductLists/Details/' + aEventData.Id + '">Details</a>&nbsp;|&nbsp;&nbsp;' +
                            '<a href="/ProductLists/Delete/' + aEventData.Id + '">Delete</a>' +
                        '</td>' + 
                    '</tr>');
                }
                break;
            case "ProductListDeleted":
                if (document.URL.endsWith("/ProductLists")) {
                    var lElement = document.getElementById("tr-productlist-" + aEventData.Id);
                    lElement.parentNode.removeChild(lElement);
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