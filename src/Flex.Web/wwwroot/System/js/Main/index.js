var elment;
var layer;

layui.use(['element', 'layer'], function () {
    elment = layui.element;
    layer = layui.layer;
    Init();
})
$('.loginout').on('click', function () {
    sessionStorage.removeItem('access_token');
    sessionStorage.removeItem('refresh_token');
    window.location.href = '/system/login'
})
function GetMsgCount() {
    ajaxHttp({
        url: api + "Message/GetNotReadMessageCount",
        type:'Get',
        dataType: 'json',
        success: function (res) {
            if (res.code == 200) {
                if (res.content != 0) {
                    $('.email_extension').html('<span class="email_num">' + res.content + '</span>');
                } else {
                    $('.email_extension').html('');
                }
            }
        }, complete: function () {
            setTimeout(function () { GetMsgCount() }, 5000)
        }
    })
}
GetMsgCount();
function InitAdmin() {
    ajaxHttp({
        url: api + 'Admin/getLoginInfo',
        type: 'Get',
        async: false,
        dataType: "json",
        success: function (json) {
            var admin = json.content;
            $('.admin_useravatar').each(function () {
                $(this).attr('src', admin.UserAvatar);
            })
            $('#sitename').text(admin.UserSign);
            $('.admin_username').text(admin.UserName)
        }
    })
}
InitAdmin();
function Init() {
    ajaxHttp({
        url: api + 'Menu/getMainMenuStrAsync',
        type: 'Get',
        //data: { _type: 'getMainMenuStr' },
        async: false,
        dataType: "json",
        success: function (result) {
            let count = 0;
            var content = '';
            if (result.code != 200) { tips.showFail(json.msg); return; }
            $.each(result.content[0].children, function (i, item) {
                if (!item.status) {
                    return;
                }
                content += '<li class="layui-nav-item">';
                if (count == 0)
                    content = '<li class="layui-nav-item layui-nav-itemed">';
                count++;
                content += '<a href="javascript:void(0);" data-linkstatus="' + item.isaspx + '" ' + (!item.ismenu ? "data-href=" + item.linkurl + " data-direction=" + item.parentid + "_" + item.id + "" : "") + '  lay-tips="' + item.title + '" lay-direction="' + item.parentid + "_" + item.id + '">' +
                    '<i class="' + item.className + ' ' + item.icode + '"></i>' +
                    '<cite>' + item.title + '</cite>' + (item.children != undefined && item.children.length > 0 ? '<i class="layui-icon layui-icon-down layui-nav-more"></i>' : "") +
                    '</a>';

                //这里是添加所有的子菜单
                content += loadchild(item);
                content += '</li>';
            });
            $(".layui-nav-tree").html(content);
            elment.init();
        },
        complete: function () { }
    })
}
//组装子菜单的方法
function loadchild(obj) {
    if (obj == null) {
        return;
    }
    var content = '';
    if (obj.children != null && obj.children.length > 0) {
        content += '<dl class="layui-nav-child">';
    } else {
        content += '<dl>';
    }
    if (obj.children != null && obj.children.length > 0) {
        $.each(obj.children, function (i, note) {
            if (!note.status) {
                return;
            }
            if (i == 0)
                content += '<dd class="layui-nav-itemed">';
            else
                content += '<dd>';
            content += '<a href="javascript:void(0);" data-linkstatus="' + note.isaspx + '" ' + (!note.ismenu ? "data-href=" + note.linkurl + " data-direction=" + note.parentid + "_" + note.id + "" : "") + ' lay-tips="' + note.title + '" lay-direction="' + note.parentid + "_" + note.id + '">' +
                '<i class="' + note.className + ' ' + (note.icode == 'null' || note.icode == '' ? "layui-icon-file" : note.icode) + '"></i>' +
                '<cite>' + note.title + '</cite>' + (note.children != undefined && note.children.length > 0 ? '<i class="layui-icon layui-icon-down layui-nav-more"></i>' : "") +
                '</a>';
            if (note.children == null) {
                return;
            }
            content += loadchild(note);
            content += '</dd>';
        });
        content += '</dl>';
    }
    return content;
}