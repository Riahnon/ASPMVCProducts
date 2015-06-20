var productsContainer;
var getProducts = function () {
    
    var lUrl = "//" + document.location.hostname + ":" + document.location.port + "/api/productsapi";
    var options = {
        type: "get",
        url: lUrl,
    };
    $.ajax(options).done(function (data, textStatus, jqXHR) {
        var lData = JSON.parse(data);
        var lProductsHtml = "";
        for (i = 0; i < lData.length; i++) {
            lProductsHtml += "\
            <div class=\"productentry\">\
                <h3 class=\"productname\">"+ lData[i].Name + "</h3>\
                <div class=\"productdetails\">\
                    <div class=\"productdescription\">" + lData[i].Description + "</div>\
                    <a href=\"/Products/Edit/" + lData[i].Id + "\">Edit</a> &nbsp;|\
                    <form action=\"/Products/Delete?id=\"" + lData[i].Id + "\" method=\"post\">\
                        <input class=\"deleteProductInput\" type=\"submit\" value=\"Delete\" />\
                    </form>\
                </div>\
            </div>\
            ";
        }
        productsContainer.innerHTML = lProductsHtml;
        console.log(data);
    });
}

$(function () {
    productsContainer = document.getElementById('productscontainer');
    getProducts();
});

