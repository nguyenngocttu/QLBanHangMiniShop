function AddToCart(id, url) {   
    $.ajax({
        type: 'POST',
        url: url + '/' + id,
        contentType: "application/json;charset=UTF-8",
        dataType: "json",  
        success: function (kq) {
            var cartMenu = document.getElementById("cartMenu");
            alert(kq.mess);
            cartMenu.innerHTML = '<text style="color:red">' + kq.value+'</text>';
        }
    })
}