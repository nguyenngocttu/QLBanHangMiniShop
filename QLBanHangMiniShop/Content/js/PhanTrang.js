

function NextPage() {

    var index = 0;
    var totalPage = parseInt(document.getElementById("TotalPage").getAttribute("value"));
    var nextPage = document.getElementById("nextListPage");
    var firstIndex = parseInt(document.getElementById("nextListPage").getAttribute("value"));
    var listPage = document.getElementsByClassName("listPage");
    var backPage = document.getElementById("backListPage");
    var lastIndex = 0;

    for (valueItem = firstIndex; valueItem <= firstIndex + 4; valueItem++) {
        if (valueItem <= totalPage) {
            listPage[index].setAttribute("id", "index_" + valueItem);
            listPage[index].setAttribute("value", valueItem);
            listPage[index].setAttribute("class", "listPage");
            listPage[index].innerHTML = "<text style=\"font-size:50px\">" + (valueItem) + "</text>";
            if (valueItem == totalPage) {
                $("#nextListPage").attr("hidden", "hidden");
            }
        }
        else {
            listPage[index].setAttribute("id", "");
            listPage[index].setAttribute("value", "");
            listPage[index].innerHTML = "";
        }

        backPage.removeAttribute("hidden");
        for (i = 0; i < 5; i++) {
            if (listPage[i].getAttribute("value") == "") {
                lastIndex = parseInt(listPage[i - 1].getAttribute("value"));
                break;
            }
            if (i == 4) {
                lastIndex = parseInt(listPage[i].getAttribute("value"));
            }
        }
        nextPage.setAttribute("value", lastIndex + 1);
        backPage.setAttribute("value", parseInt(listPage[0].getAttribute("value")) - 5);
        index++;
    }
}
function BackPage() {

    var index = 0;
    var totalPage = parseInt(document.getElementById("TotalPage").getAttribute("value"));
    var firstIndex = parseInt(document.getElementById("backListPage").getAttribute("value"));
    var listPage = document.getElementsByClassName("listPage");
    var nextPage = document.getElementById("nextListPage");
    var backPage = document.getElementById("backListPage");

    for (valueItem = firstIndex; valueItem <= firstIndex + 4; valueItem++) {

        listPage[index].setAttribute("id", "index_" + valueItem);
        listPage[index].setAttribute("value", valueItem);
        listPage[index].innerHTML = "<text style=\"font-size:50px\">" + valueItem + "</text>";
        index++;
    }
    if (firstIndex == 1) {
        backPage.setAttribute("value", "");
        backPage.setAttribute("hidden", "hidden");

    }
    if (parseInt(listPage[4].getAttribute("value")) < totalPage) {

        nextPage.removeAttribute("hidden");
    }
    nextPage.setAttribute("value", parseInt(listPage[4].getAttribute("value")) + 1);
    backPage.setAttribute("value", parseInt(listPage[0].getAttribute("value")) - 5);
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
                    var div = document.getElementById("contentList");
                    div.innerHTML = kq;               
                    //
                  
                }
            })
        }
    }
})


    
