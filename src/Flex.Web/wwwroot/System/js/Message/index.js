var UnitHtml = {
    unhtml: function (str, reg) {
        return str ? str.replace(reg || /[&<">]/g, function (m) {
            return {
                "<": "&lt;",
                "&": "&amp;",
                '"': "&quot;",
                ">": "&gt;"
            }[m]
        }) : ""
    },
    html: function (str) {
        return str ? str.replace(/&((g|l|quo)t|amp);/g, function (m) {
            return {
                "&lt;": "<",
                "&amp;": "&",
                "&quot;": '"',
                "&gt;": ">"
            }[m]
        }) : ""
    }
}
$('.email_title').on('click', 'li', function () {
    $(this).addClass('active').siblings().removeClass('active');
    var that = $(this);
    $.ajax({
        url: api + 'queueMessage/GetEmail?id=' + $(this).data('id'),
        type: 'get',
        dataType: 'json',
        success: function (res) {
            if (res.code == 200) {
                that.removeClass('new');
                var json = res.data[0];
                $('._title').html(json.title);
                $('._sender').html(json.sender);
                var t = Date.parse(json.addTime);
                $('._addTime').html(GMTToStr(t));
                $('.content_desc').html(UnitHtml.html(json.content));
            }
        }
    })
})
layui.use(function () {
    var laypage = layui.laypage;
    var layer = layui.layer;
    laypage.render({
        elem: 'pager',
        count: 70, // 数据总数，从后端得到
        layout: ['count', 'prev', 'page', 'next', 'refresh', 'skip'], // 功能布局
        jump: function (obj, first) {
            console.log(obj.curr); // 得到当前页，以便向服务端请求对应页的数据。
            pageClick(obj.curr);
            // 首次不执行
            if (!first) {
                // do something
            }
        }
    });
});

function pageClick(page) {
    $.ajax({
        url: api + 'queueMessage/getMessageList?page=' + page,
        type: 'get',
        dataType: 'json',
        success: function (res) {
            if (res.code == 200) {
                var json = res.data;
                var li_str = '';
                for (var i = 0; i < json.length; i++) {
                    if (json[i].IsRead)
                        li_str += '<li class=\"layui-icon\" data-id=\"' + json[i].mId + '\"><span class=\"title_box\">' + json[i].title + '</span><span class=\"timespan\">' + json[i].timespan + '</span></li>';
                    else
                        li_str += '<li class=\"layui-icon new\" data-id=\"' + json[i].mId + '\"><span class=\"title_box\">' + json[i].title + '</span><span class=\"timespan\">' + json[i].timespan + '</span></li>';
                }
                $('.email_title').html(li_str);
                that.addClass('active').siblings().removeClass('active');
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