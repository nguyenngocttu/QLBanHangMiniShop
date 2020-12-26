$(window).bind("beforeunload", function () {
    this.alert("are you sure?");
})