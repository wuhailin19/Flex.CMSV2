var id = $.getUrlParam('Id');

ajaxHttp({
    url: api + 'product/DetailAsync?id=' + id,
    type: 'Get',
    async: false,
    dataType: 'json',
    success: function (result) {
        var json = result.content;
        if (json.length == 0)
            return false;
        var htmlcontentstr = '';
        htmlcontentstr += "<div class=\"event-operation\">";
        htmlcontentstr += "<div class=\"action\">";
        htmlcontentstr += "项目信息<br />";
        htmlcontentstr += "时间：" + json.AddTime + "";
        htmlcontentstr += "</div>";
        htmlcontentstr += "</div>";
        htmlcontentstr += "<div class=\"event-content\">";
        htmlcontentstr += UnitHtml.html(json.ServerInfo);
        htmlcontentstr += "</div>";
        htmlcontentstr += "<div class=\"event-member\">";
        htmlcontentstr += "<div class=\"event-member-title\">参与人：</div>";
        htmlcontentstr += "<div class=\"event-member-flexbox\">";
        var splitlist = json.Participants.split(',');
        for (var j = 0; j < splitlist.length; j++) {
            htmlcontentstr += "<span class=\"member-name\">" + splitlist[j] + "</span>";
        }
        htmlcontentstr += "</div>";
        htmlcontentstr += "</div>";
        $('#id_0').html(htmlcontentstr);
    },
    complete: function () { }
})
$('.middle-link').on('click', 'a', function (event) {
    //event.preventDefault();
    var id = $(this).attr('data-id');
    if (id == 0) {
        $(this).addClass('active')
        $('#projecttitlelist a').removeClass('active');
    }
    else {
        $('a[data-id=0]').removeClass('active')
        $(this).addClass('active').siblings('a').removeClass('active');
    }
    //var hash = $(this).attr("href").split("#")[1];
    //animateScroll("#"+hash);
})
function initList() {
    ajaxHttp({
        url: api + 'product/DetailListAsync?id=' + id,
        type: 'Get',
        async: false,
        dataType: 'json',
        success: function (result) {
            var json = result.content;
            var htmlstr = '';
            var htmlcontentstr = '';
            for (var i = 0; i < json.length; i++) {
                htmlstr += '<a href="#id_' + json[i].Id + '"><span class="prodelete" data-id="' + json[i].Id + '">-</span><span class="prodetailtitle">' + json[i].Title + '</span></a>';

                htmlcontentstr += "<div class=\"event\" id=\"id_" + json[i].Id + "\">";
                htmlcontentstr += "<div class=\"event-operation\">";
                htmlcontentstr += "<div class=\"action\">";
                htmlcontentstr += json[i].Title;
                htmlcontentstr += "<br />";
                htmlcontentstr += "时间：" + json[i].AddTime + "";
                htmlcontentstr += "</div>";
                htmlcontentstr += "</div>";
                htmlcontentstr += "<div class=\"event-content\">";
                htmlcontentstr += UnitHtml.html(json[i].Content);
                htmlcontentstr += "</div>";
                htmlcontentstr += "<div class=\"event-member\">";
                htmlcontentstr += "<div class=\"event-member-title\">参与人：</div>";
                htmlcontentstr += "<div class=\"event-member-flexbox\">";
                var splitlist = json[i].Participants.split(',');
                for (var j = 0; j < splitlist.length; j++) {
                    htmlcontentstr += "<span class=\"member-name\">" + splitlist[j] + "</span>";
                }
                htmlcontentstr += "</div>";
                htmlcontentstr += "</div>";
                htmlcontentstr += "</div>";
            }

            $('#projecttitlelist').html(htmlstr);
            $('#projectdetaillist').html(htmlcontentstr);
        },
        complete: function () { }
    })

}
initList();
var addindex;
$('#projecttitlelist').on('click', 'a span.prodelete', function () {
    var id = $(this).attr('data-id');
    if (!id)
        return false;
    layer.confirm('确定删除本条数据吗？', { btn: ['确定删除', '取消'] }, function (index) {
        ajaxHttp({
            url: api + 'Product/DeleteProDetail/' + id,
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
$('#addrecord').on('click', function () {
    if (addindex != undefined)
        layer.close(addindex);
    //iframe窗
    addindex = layer.open({
        type: 2,
        title: "新增项目修改详情",
        shadeClose: true,
        shade: false,
        maxmin: true, //开启最大化最小化按钮
        area: ['80%', '80%'],
        content: '/system/Product/AddRecord',
        end: function () {
            //window.location.reload();
            initList();
        }
    });
})