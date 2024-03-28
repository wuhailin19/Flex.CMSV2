
function animateScroll($elment) {
    var itop = $($elment).offset().top;
    $(".content-right-box").stop().animate({
        scrollTop: itop
    }, 300);
}


var addindex;
function initList() {
    ajaxHttp({
        url: api + 'product/listasync',
        type: 'Get',
        async: false,
        dataType: 'json',
        success: function (result) {
            var json = result.content;
            if (json.length == 0)
                return false;
            $('.detail_iframe').attr('src', '/system/product/detail?Id=' + json[0].Id)
            var htmlstr = '';
            for (var i = 0; i < json.length; i++) {
                htmlstr += '<li class="' + (i == 0 ? "active" : "") + '" data-id="' + json[i].Id + '"><span>' + json[i].ProductName + '</span></li>';
            }
            $('#projectlist').html(htmlstr);
        },
        complete: function () { }
    })

}
initList();
$('#projectlist').on('click', 'li', function () {
    var id = $(this).attr('data-id');
    if (!id)
        return false;
    $(this).addClass('active').siblings().removeClass('active');
    $('.detail_iframe').attr('src', '/system/product/detail?Id=' + id)
})
$('#add-project').on('click', function () {
    if (addindex != undefined)
        layer.close(addindex);
    //iframe窗
    addindex = layer.open({
        type: 2,
        title: "新增项目",
        shadeClose: true,
        shade: false,
        maxmin: true, //开启最大化最小化按钮
        area: ['80%', '80%'],
        content: '/system/Product/AddPage',
        end: function () {
            //window.location.reload();
            initList();
        }
    });
})
