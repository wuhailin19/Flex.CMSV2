//const { ajax } = require("jquery");

$(".show").on("click", ".iconyanjing_bi", function () {
    $(this).removeClass("iconyanjing_bi").addClass("iconyanjing_kai");
    $('input[name=' + $(this).data('bind') + ']').attr("type", "text");
});
$(".show").on("click", ".iconyanjing_kai", function () {
    $(this).removeClass("iconyanjing_kai").addClass("iconyanjing_bi");
    $('input[name=' + $(this).data('bind') + ']').attr("type", "password");
});


// 操作列tips
var ischeck = false;
layui.config({
    base: '/Scripts/layui/module/cropper/' //layui自定义layui组件目录
}).use(['form', 'croppers'], function () {
    var form = layui.form, croppers = layui.croppers, layer = layui.layer;
    var parent_json;
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
    var password = $('input[name=Password]');
    var isvalid = false;
    var helperText = {
        charLength: $('#pwChar'),
        lowercase: $('#pwLower'),
        uppercase: $('#pwCap'),
        number: $('#pwNum'),
        other: $('#pwOther'),
    };
    var pattern = {
        charLength: function () {
            if (password.val().length >= 8) {
                return true;
            }
        },
        lowercase: function () {
            var regex = /^(?=.*[a-z]).+$/;
            if (regex.test(password.val())) {
                return true;
            }
        },
        uppercase: function () {
            var regex = /^(?=.*[A-Z]).+$/;
            if (regex.test(password.val())) {
                return true;
            }
        },
        number: function () {
            var regex = /^(?=.*[0-9]).+$/;
            if (regex.test(password.val())) {
                return true;
            }
        },
        other: function () {
            var regex = /^(?=([\x21-\x7e]+)[^a-zA-Z0-9]).+$/;
            if (regex.test(password.val())) {
                return true;
            }
        }
    };
    function checkAll() {
        if (password.val().length >= 1) {
            $('#pwdbox').show();
            validClass(pattern.charLength(), helperText.charLength);
            validClass(pattern.lowercase(), helperText.lowercase);
            validClass(pattern.uppercase(), helperText.uppercase);
            validClass(pattern.number(), helperText.number);
            validClass(pattern.other(), helperText.other);
            if (pattern.charLength()
                && pattern.lowercase()
                && pattern.uppercase()
                && pattern.number()
                && pattern.other()
            ) {
                isvalid = true;
            } else {
                isvalid = false;
            }
        }
        else {
            $('#pwdbox').hide(); isvalid = true;
        }
    }
    function validClass(result, $element) {
        if (result) {
            $element.addClass('active');
        }
        else { $element.removeClass('active'); }
    }
    $('input[name=Password]').keyup(function () {
        checkAll();
    })
    $('input[name=CheckPwd]').blur(function () {
        let checkpwd = $(this).val();
        let pwd = $('input[name=Password]').val();
        if (checkpwd === pwd) {
            ischeck = true;
            tips.closeTips();
            return;
        }
        ischeck = false;
        tips.showProStatus($(this), "两次密码不一致")
    })
    function formRender() {
        form.val("formTest", {
            'Account': parent_json.Account
        });
    }
    //监听提交
    form.on('submit(formDemo)', function (data) {
        if (password.val().length >= 1) {
            if (!isvalid) {
                password.focus();
                tips.showProStatus(password, "密码不符合规范");
                return false;
            }
            if (!ischeck) {
                tips.showProStatus($('input[name=CheckPwd]'), "请确认密码")
                $('input[name=CheckPwd]').focus();
                return false;
            }
        }
        data.field.Version = parent_json.Version;
        ajaxHttp({
            url: api + 'Admin/UpdatePassword',
            type: 'Post',
            datatype: 'json',
            data: JSON.stringify(data.field),
            async: false,
            success: function (json) {
                if (json.code == 200) {
                    tips.showSuccess(json.msg);
                    //var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
                    //parent.layer.close(index); //再执行关闭
                    setTimeout(function () {
                        top.location.href = "/system/Login/Logout";
                    }, 300);
                } else {
                    tips.showFail(json.msg);
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
