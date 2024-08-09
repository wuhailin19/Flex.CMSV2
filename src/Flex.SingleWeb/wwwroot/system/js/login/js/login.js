$(function () {
    // 绑定事件处理
    bindEvents();

    // 初始化验证码
    reloadcode();
});

// 绑定事件处理函数
function bindEvents() {
    // 按回车键触发登录按钮点击事件
    $("input").keyup(function (e) {
        if (e.keyCode === 13) {
            $("#LoginBtn").click();
        }
    });

    // 登录按钮点击事件
    $("#LoginBtn").click(handleLogin);

    // 输入框失去焦点事件
    $("#LoginName, #PassWord, #code").blur(handleBlurEvent);

    // 验证码输入框键盘事件
    $("#code").keyup(handleCodeKeyUp);
}

// 登录处理函数
function handleLogin() {
    if (!validateLoginForm()) {
        return false;
    }
    if (!isClick)
        return false;
    const encrypto = new JSEncrypt();
    encrypto.setPublicKey(`-----BEGIN PUBLIC KEY-----${publickey}-----END PUBLIC KEY-----`);

    const datajson = {
        Account: EncryptoStr(encrypto, $("#LoginName").val()),
        Password: EncryptoStr(encrypto, $("#PassWord").val()),
        CodeId: codeid,
        Codenum: $("#code").val()
    };

    $.ajax({
        type: "POST",
        url: api + "Account/LoginAsync",
        data: JSON.stringify(datajson),
        contentType: 'application/json',
        dataType: "json",
        cache: false,
        beforeSend: function () {
            isClick = false;
            $("#LoginBtn").val("登录..").attr("disabled", true);
        },
        success: handleLoginSuccess,
        error: handleAjaxError,
        complete: function () {

            
        }
    });
}

// 验证表单输入
function validateLoginForm() {
    const $loginName = $("#LoginName");
    if ($loginName.val() === "") {
        tips.showProStatus($loginName, "亲，缺少账户名？请输入哦！");
        $loginName.focus();
        return false;
    } else {
        $loginName.next().hide();
    }

    const $passWord = $("#PassWord");
    if ($passWord.val() === "") {
        tips.showProStatus($passWord, "亲，缺少密码？请输入哦！");
        $passWord.focus();
        return false;
    } else {
        $passWord.next().hide();
    }

    const $code = $("#code");
    if ($code.val() === "" || $code.val().length !== 4) {
        tips.showProStatus($code, "亲，请正确输入验证码！");
        $code.focus();
        return false;
    } else {
        $code.nextAll(".tip").hide();
    }

    return $code.attr("issuccess") === "1";
}

var isClick = true;
// 登录成功处理
function handleLoginSuccess(json) {
    if (json.code === 200) {
        $(".loadings").fadeIn();
        sessionStorage.setItem('access_token', json.content.AccessToken);
        sessionStorage.setItem('refresh_token', json.content.RefreshToken);
        sessionStorage.removeItem('sitelink');
        sessionStorage.removeItem('siteId');
        sessionStorage.removeItem('siteName');
        tips.showSuccess("登录成功");
        setTimeout(() => {
            $("#LoginBtn").removeAttr("disabled").val("登录");
            isClick = false;
            window.location = SystempageRoute + 'Main';
        }, 1300);
    } else {
        tips.showFail(json.msg);
        reloadcode();
    }
}

// AJAX 错误处理
function handleAjaxError() {
    tips.showFail("网络错误，请稍后再试");
}

// 输入框失去焦点事件处理
function handleBlurEvent() {
    const $input = $(this);
    if ($input.val() === "") {
        tips.showProStatus($input, `亲，缺少${$input.attr('data-title')}？请输入哦！`);
    } else {
        $input.next().fadeOut();
    }
}

// 验证码输入框键盘事件处理
function handleCodeKeyUp(e) {
    const $code = $(this);
    // 检查是否按下 ctrl + A，阻止验证码刷新
    if (e.ctrlKey && e.key === 'a') {
        return;
    }
    if ($code.val().length === 4 && $code.attr("issuccess") === "0") {
        const datajson = {
            CodeId: codeid,
            Codenum: $code.val()
        };

        $.ajax({
            type: "POST",
            url: api + "Account/CheckAuthCode",
            data: JSON.stringify(datajson),
            contentType: 'application/json',
            dataType: "json",
            cache: false,
            beforeSend: function () {
                tips.showProStatus($code, "&nbsp;&nbsp;验证请求中...");
            },
            success: function (json) {
                if (json.code === 200) {
                    tips.closeTips();
                    $code.attr("issuccess", "1");
                } else {
                    tips.showProStatus($code, "验证码错误，请重新输入！");
                    reloadcode();
                    $code.attr("issuccess", "0");
                }
            },
            error: handleAjaxError
        });
    }
}

// 加密字符串
function EncryptoStr(k, str) {
    let t1 = k.encrypt(str);
    let ec1 = 0;
    while (t1.length !== 172 && ec1 < 10) {
        t1 = k.encrypt(str);
        ec1++;
    }
    return t1;
}

// 重新加载验证码
function reloadcode() {
    const verify = document.getElementById('CheckCode');
    const $code = $("#code");

    $.ajax({
        type: "GET",
        url: api + "Account/getAuthCode",
        dataType: "json",
        cache: false,
        success: function (json) {
            if (json.code === 200) {
                codeid = json.content.CodeId;
                publickey = json.content.Publickey;
                verify.setAttribute('src', `data:image/jpeg;base64,${json.content.ImageCode}`);
                $code.attr("issuccess", "0");
            }
        },
        error: handleAjaxError
    });
}
