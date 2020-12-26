function ChangeIMG(number) {
    //neu so luong hinh anh = 10 thi thong bao khong the them anh
    //$('#file' + number).on('change', function () {
    var img = $("#image_" + number + ">img").attr("src");
    if (number < 10 && img==null) {
        var no = parseInt(number) + 1;
        var AddBox = '<div class="col-3"  style=" max-height:175px;max-width:175px">' +
            '<label class="grid-container" style="border:1px solid black;font-size:30px" for="file' + no + '" id="image_' + no + '" >' +
            '<i class="item"></i>' +
            '<i class="fas fa-plus item"></i>' +
            '<i class="item"></i>' +
            '</label>' +
            '</div>'
        var e = document.getElementById("ImgControl");
        e.innerHTML += AddBox;
    }

    var file = $('#file' + number).prop('files')[0];

        var fileReader = new FileReader();
    fileReader.onload = function () {
        var str = '<label for="file' + number + '" id="image_' + number + '"><img class="img-thumbnail js-file-image' + number + '" style="width: 172px; height: 172px; max - height: 175px"></label>' 

            $('#image_' + number).replaceWith(str);
            var imageSrc = event.target.result;
            var fileName = file.name;
            var fileSize = file.size;
            
            console.log(fileName);
            $('.js-file-image' + number).attr('src', imageSrc);
        };
    fileReader.readAsDataURL(file);
   
    console.log($("#file" + number).val());

    //thêm ô thêm ảnh mới
   
  
    
}