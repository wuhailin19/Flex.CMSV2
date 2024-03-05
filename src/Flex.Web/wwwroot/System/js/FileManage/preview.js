layui.use(function () {

    var path = $.getUrlParam('path');
    $.ajax({
        url: path,
        type: 'GET',
        async: false,
        success: function (res) {
            $('.code-demo').html("<textarea>" + res + "</textarea>");
        }
    })
})