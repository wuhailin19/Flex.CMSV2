﻿var route_href = window.location.href;
var hostlink = route_href.split('/')[0] + "//" + route_href.split('/')[2];
//var hostlink = "http://127.0.0.1:5004";
var api = hostlink + "/api/";
var SystempageRoute = "/System/";

function getCheckboxValue(name) {
    var arraybox = [];
    $('input[name=' + name + ']:checked').each(function () {
        arraybox.push($(this).val());
    });
    return arraybox.join(',');
}

function setCheckboxValue(name, value) {
    if (value == null || value == undefined || value == "")
        return;
    var arraybox = value.split(',');
    for (var i = 0; i < arraybox.length; i++) {
        $('input[name=' + name + '][value=' + arraybox[i] + ']').attr('checked', true)
    }
}

var HttpRequest = function (options) {
    var defaults = {
        type: 'get',
        headers: {},
        data: {},
        dataType: 'json',
        async: true,
        cache: false,
        beforeSend: null,
        success: null,
        processData: true,
        contentType: null,
        complete: null,
        setcontentType: true
    };
    var o = $.extend({}, defaults, options);

    $.ajax({
        url: o.url,
        type: o.type,
        headers: {
            'Authorization': "Bearer " + o.token,
            'Refresh_token': "Bearer " + o.refreshtoken,
            'siteId': o.siteId
        },
        processData: o.processData,
        contentType: o.contentType,
        data: o.data,
        dataType: o.dataType,
        async: o.async,
        beforeSend: function () {
            o.beforeSend && o.beforeSend();
        },
        success: function (res) {
            if (res.code == 217) {
                refreshToken(
                    function () {
                        // 重新发起请求
                        o.token = sessionStorage.getItem('access_token');
                        HttpRequest(o);
                    }
                )
            } else if (res.code != 200 && res.code != 226) {
                tips.showFail(res.msg);
            }
            else {
                o.success && o.success(res);
            }
        },
        complete: function () {
            o.complete && o.complete();
        }
    });
};

var loginHttp = function (options) {
    // 登入页无需携带token
    // 后台如果要求 Content-Type 
    if (options.type == 'post') {
        options.contentType = 'application/x-www-form-urlencoded';
    }
    HttpRequest(options);
}
var ajaxHttp = function (options) {
    if (options.type != undefined) {
        if (!options.setcontentType) {
            if (options.type.toLowerCase() == 'post' || options.type.toLowerCase() == 'put') {
                options.contentType = 'application/json';
            }
        }
    }

    // 每次请求携带token
    options.token = sessionStorage.getItem('access_token');
    options.refreshtoken = sessionStorage.getItem('refresh_token');
    options.siteId = sessionStorage.getItem('siteId');
    HttpRequest(options);
}

var httpTokenHeaders = {
    'Authorization': "Bearer " + sessionStorage.getItem('access_token'),
    'Refresh_token': "Bearer " + sessionStorage.getItem('refresh_token'),
    'siteId': sessionStorage.getItem('siteId'),
}
var isrefresh = true;
var refreshToken = function (callback) {
    if (isrefresh) {
        isrefresh = false;
        $.ajax({
            url: api + 'Account/RefreshAccessTokenAsync', // 刷新 token 的接口
            type: 'post',
            dataType: 'json',
            data: { RefreshToken: sessionStorage.getItem('refresh_token'), AccessToken: sessionStorage.getItem('access_token') },
            async: false,
            success: function (res) {
                if (res.code == 200) {
                    sessionStorage.setItem('access_token', res.content.AccessToken);
                    sessionStorage.setItem('refresh_token', res.content.RefreshToken);
                    top.reconnectSignalR();
                    callback(); // 刷新成功后重新发起原请求
                } else {
                    tips.showFail(res.msg);
                }
            },
            complete: function () {
                isrefresh = true;
            },
            error: function () {
                tips.showFail('请重新登录');
            }
        });
    }
};

var global_notice;
layui.extend({ 'notice': '/scripts/layui/module/notice/notice' });

layui.use('notice', function () {
    global_notice = layui.notice;
    // 初始化配置，同一样式只需要配置一次，非必须初始化，有默认配置
    global_notice.options = {
        closeButton: true,//显示关闭按钮
        debug: false,//启用debug
        positionClass: "toast-top-right",//弹出的位置,
        showDuration: "300",//显示的时间
        hideDuration: "1000",//消失的时间
        timeOut: "3000",//停留的时间
        extendedTimeOut: "1000",//控制时间
        showMethod: "slideDown",//控制时间
        showEasing: "swing",//显示时的动画缓冲方式
        hideEasing: "linear",//消失时的动画缓冲方式
        progressBar: true,//消失时的动画缓冲方式
        iconClass: 'toast-info', // 自定义图标，有内置，如不需要则传空 支持layui内置图标/自定义iconfont类名
        onclick: null, // 点击关闭回调
    };
})

// 操作列tips
var tips = {
    timeout: 6000,
    msgboxtime: 1000,
    index: undefined,
    boxindex: undefined,
    width: '90%',
    height: '90%',
    showInfoBox: function (title, url, w, h) {
        tips.closeTips();
        if (w == undefined || w == '')
            w = tips.width;
        if (h == undefined || h == '')
            h = tips.height;
        tips.index = layer.open({
            type: 2,
            title: title,
            shadeClose: true,
            shade: false,
            maxmin: true, //开启最大化最小化按钮
            area: [w, h],
            content: url,
            success: function (layero, index) {
            }
        });
    },
    showProStatus: function ($emlemt, msg) {
        tips.index = layer.tips(msg, $emlemt, {
            tips: 4,
            time: tips.timeout     // 3秒消失
        })
    },
    message: function (msg, cate) {
        if (global_notice != undefined)
            global_notice[cate](msg);
    },
    uploadProgressInfo: function (fileid, msg) {
        fileid = fileid + "_progress_box";
        if ($("#" + fileid).length > 0) {
            $("#" + fileid).text(msg);
            return;
        }
        let options = {
            timeOut: "300000",
            extendedTimeOut: "300000",
            progressBar: false,
            closeButton: false,
            closeOnHover: false,
            setId: fileid
        }
        global_notice.info(msg, "", options);
    },
    showProgress(fileid, msg) {
        tips.uploadProgressInfo(fileid, msg);
    },
    showSuccess: function (msg) {
        top.tips.message(msg, 'success');
    },
    showFail: function (msg) {
        top.tips.message(msg, 'error');
    },
    closeTips: function () {
        layer.close(tips.index);
    },
    closeProgressbox: function () {
        setTimeout(function () {
            global_notice.clear();
        }, 3000)
    }
}
