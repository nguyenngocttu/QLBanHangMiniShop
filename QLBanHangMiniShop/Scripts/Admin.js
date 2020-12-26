



$(document).ready(function () {
    AddEventOpenPopUP();

   
})

function AddEventOpenPopUP() {
    var lstbutton = document.getElementsByName("LoadPopUp");
    var leg = lstbutton.length
    for (i = 0; i < leg; i++) {
        lstbutton[i].onclick = function () { OpenPopUp(this.id); }
    }
}
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
function UpdateListPage(actionNameListPage, action) {
    var listIndexPage = document.getElementsByClassName("listPage");
    var lastIndex = -1;
    for (i = 0; i < 5; i++) {
        if (listIndexPage[i].getAttribute("value") == "") {
            break;
        }
        else {
            lastIndex = parseInt(listIndexPage[i].getAttribute("value"));
        }
    }
    if (action == "create") {
        $.ajax({
            type: "POST",
            url: actionNameListPage,
            success: function (kq) {
                $("#TotalPage").attr("value", kq);
                var totalPage = parseInt(kq);
                if (totalPage > lastIndex && lastIndex != 0) {
                    if (lastIndex % 5 == 0) {
                        var nextListPage = $("#nextListPage").attr("hidden");
                        if (nextListPage != null) {
                            $("#nextListPage").removeAttr("hidden");
                            $("#nextListPage").attr("value", parseInt(listIndexPage[4].getAttribute("value")) + 1);
                        }
                    }
                    else {
                        for (i = 0; i < 5; i++) {
                            if (listIndexPage[i].getAttribute("value") == "") {
                                listIndexPage[i].setAttribute("value", kq);
                                listIndexPage[i].setAttribute("id", "index_" + kq);
                                listIndexPage[i].setAttribute("class", "listPage");
                                listIndexPage[i].innerHTML = kq;
                                break;
                            }
                        }
                    }
                }
            }
        })
    }
    if (action == "delete") {
        $.ajax({
            type: "POST",
            url: actionNameListPage,
            success: function (kq) {
                var countItemListPage = 0;
                $("#TotalPage").attr("value", kq);
                var totalPage = parseInt(kq);
                if (totalPage == lastIndex) {
                    $("#nextListPage").attr("hidden", "hidden");
                }

                for (valueItem = 0; valueItem < 5; valueItem++) {
                    if (parseInt(listIndexPage[valueItem].getAttribute("value")) > totalPage) {
                        listIndexPage[valueItem].setAttribute("value", "");
                        listIndexPage[valueItem].setAttribute("id", "");
                        listIndexPage[valueItem].innerHTML = "";
                    }
                    if (listIndexPage[valueItem].getAttribute("value").length < 1) {
                        countItemListPage++;
                    }
                }
                if (countItemListPage == 5) {
                    BackPage();
                }
            }
        })
    }
    if (action == "updateListPage") {
        $.ajax({
            type: "GET",
            url: actionNameListPage,
            success: function (kq) {
                var div = document.getElementById("DivListPage");
                div.innerHTML = kq;
                AddEventOpenPopUP();
            }
        })
    }

}
function ClickPage(id) {
    var z = document.getElementById(id);//id
    var val = z.getAttribute("value");
    var action = document.getElementById("ActionName").value;
    var URL = "";
    
    if (action.lastIndex("&") == action.length -1 ) {
        URL = action + "indexPage=" + val;
    }
    else {
        URL = action + "?indexPage=" + val;
    }
   
    if (action=="")
    $.ajax({
        type: "POST",
        url: URL,
        success: function (kq) {
            var div = document.getElementById("tableBody");
            div.innerHTML = kq;
            AddEventOpenPopUP();
        }
    })
}
function demo()
{
    var lstbutton = document.getElementsByName("LoadPopUp");
    var leg = lstbutton.length;
   
}
//load popup
function OpenPopUp(id) {
    
  
    $.ajax({
        type: "GET",
        url: id,
        success: function (kq) {
            var body = document.getElementsByTagName("body");
            body[0].setAttribute("class", "sidebar-mini layout-fixed modal-open");
            var modal = document.getElementById("myModal");
            modal.setAttribute("class", "modal fade bs-example-modal-lg show");
            modal.setAttribute("style", "display:block");
            var backGroud = document.getElementById("backgroud-Popup");
            backGroud.setAttribute("class", "modal-backdrop fade show")
            var Partial = document.getElementById("modal-content-main");
            Partial.innerHTML = kq;           
          
        }
    })
}
//đóng popup
function ClosePopUp() {  
    var modal = document.getElementById("myModal");
    modal.setAttribute("class", "modal fade bs-example-modal-lg");
    modal.setAttribute("style", "");
    var backGroud = document.getElementById("backgroud-Popup");
    backGroud.setAttribute("class", "");
    var body = document.getElementsByTagName("body");
    body[0].setAttribute("class", "");
    var Partial = document.getElementById("modal-content-main");
    Partial.innerHTML = "";    
    AddEventOpenPopUP();
}



function DeleteResult(idElement, actionNamePostTable,
    parameterPostTable, actionNameListPage)
{
    
    var e = document.getElementById(idElement);
    e.remove();
    var lstbutton = document.getElementsByName("LoadPopUp");
    if (lstbutton.length <2 ) {
        UpdateDataTable(actionNamePostTable, parameterPostTable);
    }   
    ClosePopUp();
    UpdateListPage(actionNameListPage, "delete");
}
function UpdateDataTable(actionNamePostTable, parameterPostTable) {
    $.ajax({
        type: "POST",
        url: actionNamePostTable + parameterPostTable,
        success: function (data) {
            var tb = document.getElementById("tableBody");
            tb.innerHTML = data;
            AddEventOpenPopUP();
        }
    })
}
function Success(data, actionNameListPage) {
    if (data == 'false') {
        alert("vui long nhap day du thong tin can thiet");
    }
    else {
        alert(data);
        
        UpdateListPage(actionNameListPage, "create");
    }
   
}

function SuccessBill(data) {
        alert(data);
        ClosePopUp();
}








