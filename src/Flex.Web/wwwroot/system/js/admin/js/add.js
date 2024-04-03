$(".show").on("click", ".iconyanjing_bi", function () {
    $(this).removeClass("iconyanjing_bi").addClass("iconyanjing_kai");
    $('input[name=' + $(this).data('bind') + ']').attr("type", "text");
});
$(".show").on("click", ".iconyanjing_kai", function () {
    $(this).removeClass("iconyanjing_kai").addClass("iconyanjing_bi");
    $('input[name=' + $(this).data('bind') + ']').attr("type", "password");
});

ajaxHttp({
    url: api + 'RolePermission/PermissionList',
    type: 'Get',
    datatype: 'json',
    async: false,
    success: function (json) {
        if (json.code == 200) {
            $.each(json.content, function (index, item) {
                $('#RoleId').append('<option value="' + item.Id + '">' + item.RolesName + '</option>');
            })
        } else {
            tips.showFail(json.msg);
        }
    }
})
// 操作列tips

var ischeck = true;
layui.config({
    base: '/scripts/layui/module/cropper/' //layui自定义layui组件目录
}).use(['form', 'croppers'], function () {
    var form = layui.form, croppers = layui.croppers, layer = layui.layer;
    
    //创建一个头像上传组件
    croppers.render({
        elem: '#editimg'
        , saveW: 150     //保存宽度
        , saveH: 150
        , mark: 1 / 1    //选取比例
        , area: '900px'  //弹窗宽度
        , url: api + "Admin/OnloadUserAvatar"  //图片上传接口返回和（layui 的upload 模块）返回的JOSN一样
        , done: function (data) { //上传完毕回调
            $("#inputimgurl").val(data);
            $("#srcimgurl").attr('src', data);
        }
    });
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
    $('input[name=CheckPwd]').keyup(function () {
        let checkpwd = $(this).val();
        let pwd = $('input[name=Password]').val();
        if (checkpwd == pwd) {
            ischeck = true;
            tips.closeTips();
            return;
        }
        ischeck = false;
        tips.showProStatus($(this), "两次密码不一致")
    })
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
        const rolename = $('select[name="RoleId"] > option:selected').text();
        data.field.RoleName = rolename;
        ajaxHttp({
            url: api + 'Admin/CreateAccount',
            type: 'Post',
            datatype: 'json',
            data: JSON.stringify(data.field),
            async: false,
            success: function (json) {
                if (json.code == 200) {
                    layer.msg(json.msg, { icon: 6, time: 300 }, function () {
                        var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
                        parent.layer.close(index); //再执行关闭
                    });
                } else {
                    tips.showFail(json.msg);
                }
            },
            complete: function () {

            }
        })
        return false;
    });
});