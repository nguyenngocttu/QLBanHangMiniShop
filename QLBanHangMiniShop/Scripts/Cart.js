
function RemoveItem(action, idItem) {
    var item = $("#itemCart_" + idItem);
    var c = document.getElementById("cartMenu");
    
    $.ajax({
        type: 'POST',
        url: action,
        success: function (kq) {
            item.remove();
            var totalItem = parseInt(c.innerText) - 1;
            c.innerText = totalItem;
            alert(kq.mess);
           
        }
    })
}