
function animateScroll($elment) {
    var itop = $($elment).offset().top;
    $(".content-right-box").stop().animate({
        scrollTop: itop
    }, 300);
}

var addindex;
var updateindex;
function initList() {
    ajaxHttp({
        url: api + 'product/listasync',
        type: 'Get',
        async: false,
        dataType: 'json',
        success: function (result) {
            var json = result.content;
            $('.detail_iframe').attr('src', '/system/product/detail?Id=' + json[0].Id)
            var htmlstr = '';
            for (var i = 0; i < json.length; i++) {
                htmlstr += '<li class="' + (i == 0 ? "active" : "") + '"><span class="prodelete" data-id="' + json[i].Id + '">-</span><span class="proname"  data-id="' + json[i].Id + '">' + json[i].ProductName + '</span><span class="editpro layui-icon" data-id="' + json[i].Id + '">&#xe642;</span></li>';
            }
            $('#projectlist').html(htmlstr);
        },
        complete: function () { }
    })

}
initList();
$('#projectlist').on('click', 'li span.prodelete', function () {
    var id = $(this).attr('data-id');
    if (!id)
        return false;
    layer.confirm('确定删除本条数据吗？', { btn: ['确定删除', '取消'] }, function (index) {
        ajaxHttp({
            url: api + 'Product/DeleteProItem/' + id,
            type: 'Post',
            async: false,
            success: function (json) {
                if (json.code == 200) {
                    tips.showSuccess(json.msg);
                    initList();
                } else {
                    tips.showFail(json.msg);
                }
            },
            complete: function () { }
        })
        layer.close(index)
    })
})
$('#projectlist').on('click', 'li span.proname', function () {
    var id = $(this).attr('data-id');
    if (!id)
        return false;
    $(this).parent('li').addClass('active').siblings().removeClass('active');
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
var updateid;
$('#projectlist').on('click','.editpro', function () {
    if (updateindex != undefined)
        layer.close(updateindex);
    var id = $(this).attr('data-id');
    if (!id)
        return false;
    updateid = id;
    //iframe窗
    updateindex = layer.open({
        type: 2,
        title: "编辑项目",
        shadeClose: true,
        shade: false,
        maxmin: true, //开启最大化最小化按钮
        area: ['80%', '80%'],
        content: '/system/Product/EditPage',
        end: function () {
            $('.detail_iframe').attr('src', '/system/product/detail?Id=' + id)
        }
    });
})
