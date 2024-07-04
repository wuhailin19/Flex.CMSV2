var codeid = "";
var publickey = "";
$(function () {
    $(".login .row4 .rmb").click(function () {
        if ($(this).hasClass("rmbOn")) {
            $(this).removeClass("rmbOn").find("span").animate({ left: 0 }, 100);
            $("#isremember").val(0);
        } else {
            $(this).addClass("rmbOn").find("span").animate({ left: 18 }, 100);
            $("#isremember").val(1);
        }
    });
    jQuery("input").keyup(function (e) {
        if (e.keyCode == 13) {
            jQuery("#LoginBtn").click();
        }
    })

    //登录验证
    jQuery("#LoginBtn").click(function () {
        var LoginName = jQuery("#LoginName");
        if (LoginName.val() == "") {
            tips.showProStatus(LoginName, "亲，缺少账户名？请输入哦！");
            LoginName.focus();
            return false;
        }
        else {
            LoginName.next().hide();
        };
        var PassWord = jQuery("#PassWord");
        if (PassWord.val() == "") {
            tips.showProStatus(PassWord, "亲，缺少密码？请输入哦！");
            PassWord.focus();
            return false;
        }
        else { PassWord.next().hide(); };
        var code = jQuery("#code");
        if (code.val() == "") {
            tips.showProStatus(code, "亲，缺少验证码？请输入哦！");
            code.focus();
            return false;
        }
        else {
            if (code.val().length != 4) {
                tips.showProStatus(code, "亲，请正确输入验证码！");
            }
            else {
                code.nextAll(".tip").hide();
            }
        };
        if (code.attr("issuccess") == "1") {
            let encrypto = new JSEncrypt();
            encrypto.setPublicKey('-----BEGIN PUBLIC KEY-----' + publickey + '-----END PUBLIC KEY-----');
            let datajson = {
                Account: EncryptoStr(encrypto, LoginName.val()),
                Password: EncryptoStr(encrypto, PassWord.val()),
                CodeId: codeid,
                Codenum: code.val()
            };
            //ajax  
            jQuery.ajax({
                type: "POST",
                url: api + "Account/LoginAsync",
                data: JSON.stringify(datajson),
                contentType: 'application/json',
                dataType: "json",
                cache: false,
                beforeSend: function () {
                    jQuery(this).val("登录..").attr("disabled", "disabled");
                },
                success: function (json) {
                    if (json.code == 200) {
                        jQuery(".loadings").fadeIn();
                        sessionStorage.setItem('access_token', json.content.AccessToken);
                        sessionStorage.setItem('refresh_token', json.content.RefreshToken);

                        localStorage.removeItem('sitelink');
                        localStorage.removeItem('siteId');
                        localStorage.removeItem('siteName');
                        tips.showSuccess("登录成功")
                        setTimeout(function () { window.location = SystempageRoute + 'Main'; }, 1300);
                    } else {
                        tips.showFail(json.msg);
                        reloadcode();
                    }
                },
                error: function () {
                    tips.showFail("网络错误，请稍后再试")
                }
            });
        }
        return false;
    });
    //输入账户密码自动消失提示
    jQuery("#LoginName").blur(function () {
        var obj = jQuery(this);
        if (obj.val() == "") {
            tips.showProStatus(obj, "亲，缺少账户名？请输入哦！");
            return false;
        }
        else { obj.next().fadeOut(); }
    });
    jQuery("#PassWord").blur(function () {
        var obj = jQuery(this);
        if (obj.val() == "") {
            tips.showProStatus(obj, "亲，缺少密码？请输入哦！");
            return false;
        }
        else { obj.next().fadeOut(); }
    });
    jQuery("#code").blur(function () {
        var obj = jQuery(this);
        if (obj.val() == "") {
            tips.showProStatus(obj, "亲，缺少验证码？请输入哦！");
            return false;
        }
        else {
            if (obj.val().length != 4) {
                tips.showProStatus(obj, "亲，请正确输入验证码！");
            }
        }
    }).keyup(function (e) {
        var obj = jQuery(this);
        if (obj.val().length < 4) {
            return false;
        }
        //ajax异步验证
        if (obj.attr("issuccess") == "0") {
            if (obj.val().length == 4) {
                let datajson = {
                    CodeId: codeid,
                    Codenum: obj.val()
                };
                jQuery.ajax({
                    type: "POST",
                    url: api + "Account/CheckAuthCode",
                    data: JSON.stringify(datajson),
                    contentType: 'application/json',
                    dataType: "json",
                    cache: false,
                    beforeSend: function () {
                        tips.showProStatus(obj, "&nbsp;&nbsp;验证请求中...");
                    },
                    success: function (json) {
                        if (json.code != "200") {
                            tips.showProStatus(obj, "验证码错误，请重新输入！");
                            reloadcode();
                            obj.attr("issuccess", "0");
                        }
                        else {
                            tips.closeTips();
                            obj.attr("issuccess", "1");
                        }
                    },
                    error: function () {
                        tips.showFail("网络错误，请稍后再试")
                    }
                });
            }
        }
    });
});
function EncryptoStr(k, str) {
    let t1 = k.encrypt(str);
    let ec1 = 0;
    while (t1.length != 172 && ec1 < 10) {
        t1 = k.encrypt(str);
        ec1++;
    }
    return t1;
}
function reloadcode() {
    var verify = document.getElementById('CheckCode');
    let obj = jQuery("#code");
    jQuery.ajax({
        type: "Get",
        url: api + "Account/getAuthCode",
        dataType: "json",
        cache: false,
        success: function (json) {
            if (json.code == "200") {
                codeid = json.content.CodeId;
                publickey = json.content.Publickey;
                verify.setAttribute('src', 'data:image/jpeg;base64,' + json.content.ImageCode);
                obj.attr("issuccess", "0");
            }
        },
        error: function () {
            tips.showFail("网络错误，请稍后再试")
        }
    });
    obj.attr("issuccess", "0");
}

reloadcode();