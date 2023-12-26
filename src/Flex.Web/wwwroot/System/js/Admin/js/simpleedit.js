$(".show").on("click", ".iconyanjing_bi", function () {
    $(this).removeClass("iconyanjing_bi").addClass("iconyanjing_kai");
    $('input[name=' + $(this).data('bind') + ']').attr("type", "text");
});
$(".show").on("click", ".iconyanjing_kai", function () {
    $(this).removeClass("iconyanjing_kai").addClass("iconyanjing_bi");
    $('input[name=' + $(this).data('bind') + ']').attr("type", "password");
});

// 操作列tips

var ischeck = true;
layui.config({
    base: '/Scripts/layui/module/cropper/' //layui自定义layui组件目录
}).use(['form', 'croppers'], function () {
    var form = layui.form, croppers = layui.croppers, layer = layui.layer;
    var tips = {
        timeout: 2000,
        msgboxtime: 1000,
        index: undefined,
        showProStatus: function ($emlemt, msg) {
            tips.index = layer.tips(msg, $emlemt, {
                tips: 2,
                time: tips.timeout     // 3秒消失
            })
        },
        showSuccess: function (msg) {
            layer.msg(msg, { icon: 6, time: tips.msgboxtime }, function () { });
        },
        showFail: function (msg) {
            layer.msg(msg, { icon: 5, time: tips.msgboxtime }, function () { });
        },
        closeTips: function () {
            layer.close(tips.index);
        }
    }
    var parent_json;
    function Init() {
        ajaxHttp({
            url: api + 'Admin/getLoginInfo',
            type: 'Get',
            datatype: 'json',
            async: false,
            success: function (json) {
                if (json.code == 200) {
                    parent_json = json.content;
                } else {
                    layer.msg(json.msg, { icon: 5, time: 1000 });
                }
            }
        })
        formRender();
    }
    Init();
    //创建一个头像上传组件
    croppers.render({
        elem: '#editimg'
        , saveW: 150     //保存宽度
        , saveH: 150
        , mark: 1 / 1    //选取比例
        , area: '900px'  //弹窗宽度
        , url: api + 'Admin/OnloadUserAvatar'  //图片上传接口返回和（layui 的upload 模块）返回的JOSN一样
        , done: function (data) { //上传完毕回调
            $("#inputimgurl").val(data);
            $("#srcimgurl").attr('src', data);
        }
    });
    function formRender() {
        form.val("formTest", {
            'Id': parent_json.Id
            , 'UserName': parent_json.UserName
            , 'UserSign': parent_json.UserSign
            , 'UserAvatar': parent_json.UserAvatar
            , 'AllowMultiLogin': parent_json.AllowMultiLogin
            , 'FilterIp': parent_json.FilterIp
            , 'Islock': parent_json.Islock
        });
        $('#srcimgurl').attr('src', parent_json.UserAvatar);
    }
    $('#CurrentLoginTime').html(parent_json.CurrentLoginTime);
    $('#CurrentLoginIP').html(parent_json.CurrentLoginIP);
    if (parent_json.adminLoginLog != undefined) {
        $('#LastLoginIP').html(parent_json.adminLoginLog.LastLoginIP);
        $('#LastLoginTime').html(parent_json.adminLoginLog.LastLoginTime);
    }
    $('#editimg').attr('data-id', parent_json.Id);
    //监听提交
    form.on('submit(formDemo)', function (data) {
        data.field.Version = parent_json.Version;
        ajaxHttp({
            url: api + 'Admin/UpdateUserAvatar',
            type: 'Post',
            datatype: 'json',
            data: JSON.stringify(data.field),
            async: false,
            success: function (json) {
                if (json.code == 200) {
                    layer.msg(json.msg, { icon: 6, time: 300 }, function () {
                        parent.InitAdmin();
                        Init();
                    });
                } else {
                    layer.msg(json.msg, { icon: 5, time: 1000 });
                }
            },
            complete: function () {

            }
        })
        return false;
    });
    $('#reset').click(function () {
        formRender();
        return false;
    })
});