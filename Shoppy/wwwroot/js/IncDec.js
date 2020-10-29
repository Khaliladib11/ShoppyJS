

const plusIcon = document.getElementById('plus-icon');
const subIcon = document.getElementById('sub-icon');
var nb = parseInt(parseInt(document.getElementById('number').innerHTML, 10));

plusIcon.addEventListener('click', function () {
    nb = isNaN(nb) ? 0 : nb;
   var max =  document.getElementById('quantityAvailble').value;
    if (nb <= max - 1) {
        nb++;
    }
    document.getElementById('number').innerHTML = nb;
    document.getElementById('quantity_product').value = nb;
});
subIcon.addEventListener('click', function () {
    nb = isNaN(nb) ? 0 : nb;
    if (nb > 0) {
        nb--;
    }
    document.getElementById('number').innerHTML = nb;
    document.getElementById('quantity_product').value = nb;
});



function getSizes(color) {
    var pid = document.getElementById('productId').value;
    $.ajax({
        type: "GET",
        url: "/home/getProductSizesByColor",
        data: { "pid": pid, "color": color },
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#sizeSelect").empty();
            $("#sizeSelect").append('<option value="0"> Select Size </option>');
            for (var i in result) {
                $("#sizeSelect").append('<option value="' + result[i].sizeId + '">' + result[i].size + '</option>');
            }
            var sizeSection = document.querySelector('.product-section_Specification_size');
            //sizeSection.style.display = "block";
            sizeSection.style.maxHeight = sizeSection.scrollHeight + "px";
            document.getElementById('colorField').value = color;
        },
        complete: function () {
            getQuantity();
            document.getElementById('number').innerHTML = '0';
        }
    });
    
}

function getQuantity() {
    var pid = document.getElementById('productId').value;
    var sid = document.getElementById("sizeSelect").value;
    var colors = document.getElementsByName('radio');
    var cid;
    for (var i = 0; i < colors.length; i++) {
        if (colors[i].checked) {
            cid = colors[i].value;
        }
    }

    alert("Pid: " + pid + " Sid: " + sid + " Cid: " + cid);

    $.ajax({
        type: "GET",
        url: "/home/getProductQuantity",
        data: { "pid": pid, "cid": cid, "sid": sid },
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            document.getElementById('quantityAvailble').value = result;
        },
    });
}


function addToCart() {
    var pid = document.getElementById('productId').value;
    var sid = document.getElementById("sizeSelect").value;
    var colors = document.getElementsByName('radio');
    var quantity = document.getElementById('quantity_product').value;
    var cid;
    for (var i = 0; i < colors.length; i++) {
        if (colors[i].checked) {
            cid = colors[i].value;
        }
    }

    if (!cid || sid == 0) {
        document.querySelector('.addToCart_validation_select').style.display = 'block';
    }
    else {
        document.querySelector('.addToCart_validation_select').style.display = 'none';
        document.querySelector('.addToCart_validation_quantity').style.display = 'none';

        $.ajax({
            type: "GET",
            url: "/shoppingCart/AddToCart",
            data: { "pid": pid, "sid": sid, "cid": cid, "quantity": quantity,},
            beforeSend: function () {
                $("#loader").show();
                $("#addToCart_btn").hide();
            },
            success: function (response) {
                if (response.result == "Redirect") {
                    window.location.href = response.url;
                } else if (response.result == "Success") {
                    alert("Added Successfully");
                    $("#Cart").load('/shoppingCart/updateCart');
                    document.getElementById('number').innerHTML = '0';
                }
                else if (response.result == "unsuccess") {
                    alert("add to cart failed");
                }
            },
            complete: function () {
                $("#loader").hide();
                $("#addToCart_btn").show();
            },
            failure: function (response) {
                alert(response);
            },
            error: function (response) {
                alert(response);
            }
        });

        alert("Pid: " + pid + " Sid: " + sid + " Cid: " + cid + " quantity: " + quantity);
    }
       
}