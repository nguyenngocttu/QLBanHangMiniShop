
function AddEventOpenPopUP() {
    var lstbutton = document.getElementsByName("LoadPopUp");
    for (i = 0; i < lstbutton.length; i++) {
        lstbutton[i].onclick = function () { OpenPopUp(this.id); }
    }
}

$(document).ready(function () {
    var listIndex = $("#ulPage>li");
    for (i = 0; i < listIndex.length; i++) {
        listIndex[i].onclick = function () {
            var unactive = $(".active");
            for (i = 0; i < unactive.length; i++) {
                unactive[i].setAttribute("class", "");
            }
            this.setAttribute("class", "active");
        }
    }
    var listIndex = document.getElementsByClassName("listPage");
    var leg = listIndex.length;   
    for (i = 0; i < leg; i++) {
        var id = listIndex[i];
        id.onclick = function () {          
            var z = document.getElementById(this.id);
            var val = z.getAttribute("value");
            var action = document.getElementById("ActionName").value;   
            var URL = "";

            if (action.lastIndexOf('&') == action.length - 1) {
                URL = action + "indexPage=" + val;
            }
            else {
                URL = action + "?indexPage=" + val;
            }
            $.ajax({
                type: 'POST',
                url: URL,             
                success: function (kq) {
                    var div = document.getElementById("tableBody");
                    div.innerHTML = kq;
                    AddEventOpenPopUP();
                                       
                }
            })
        }
    }
})
