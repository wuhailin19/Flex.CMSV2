
$('.email_title').on('click', 'li', function () {
    $(this).addClass('active').siblings().removeClass('active');
    var that = $(this);
    ajaxHttp({
        url: api + 'Message/GetMessage/' + $(this).data('id'),
        type: 'get',
        dataType: 'json',
        success: function (res) {
            if (res.code == 200) {
                that.removeClass('new');
                var json = res.content;
                $('._title').html(json.Title);
                $('._sender').html(json.AddUserName);
                $('._addTime').html(json.AddTime);
                $('.content_desc').html(UnitHtml.html(json.MsgContent));
            }
        }
    })
})
var totalCount = 0;
layui.use(function () {
    var laypage = layui.laypage;
    var layer = layui.layer;
    pageClick(1);
    laypage.render({
        elem: 'pager',
        count: totalCount, // 数据总数，从后端得到
        layout: ['count', 'prev', 'page', 'next', 'refresh', 'skip'], // 功能布局
        jump: function (obj, first) {
            // 首次不执行
            if (!first) {
                pageClick(obj.curr);
                // do something
            }
        }
    });
});

function pageClick(page) {
    ajaxHttp({
        url: api + 'Message/GetMessageTitleListDtoAsync?page=' + page,
        type: 'get',
        async: false,
        dataType: 'json',
        success: function (res) {
            if (res.code == 200) {
                var json = res.content.Items;
                totalCount = res.content.TotalCount;

                var li_str = '';
                for (var i = 0; i < json.length; i++) {
                    if (json[i].IsRead)
                        li_str += '<li class=\"layui-icon\" data-id=\"' + json[i].Id + '\"><span class=\"title_box\">' + json[i].Title + '</span><span class=\"timespan\">' + json[i].AddTime + '</span></li>';
                    else
                        li_str += '<li class=\"layui-icon new\" data-id=\"' + json[i].Id + '\"><span class=\"title_box\">' + json[i].Title + '</span><span class=\"timespan\">' + json[i].AddTime + '</span></li>';
                }
                $('.email_title').html(li_str);
            }
        }
    })
}

//$(function () {
//    $('.email_title li').eq(0).trigger('click');
//})
//GMT时间格式转换成字符串
function GMTToStr(time) {
    let date = new Date(time)
    let Str = date.getFullYear() + '-' +
        check(date.getMonth() + 1) + '-' +
        date.getDate() + ' ' +
        check(date.getHours()) + ':' +
        check(date.getMinutes()) + ':' +
        check(date.getSeconds())
    return Str
}
//判断时间是否为个位数，如果时间为个位数就在时间之前补上一个“0”
function check(val) {
    if (val < 10) {
        return ("0" + val);
    }
    else {
        return (val);
    }
}