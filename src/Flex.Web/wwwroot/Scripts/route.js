var route_href = window.location.href;
//var api = route_href.split('/')[0] + "//" + route_href.split('/')[2]+"/";
var api = "http://127.0.01:5003/api/";
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
            'Refresh_token': o.refreshtoken
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
            if (res.code != 200 && res.code != 226) { tips.showFail(res.msg); return; }
            o.success && o.success(res);
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
    HttpRequest(options);
}

var httpTokenHeaders = {
    'Authorization': "Bearer " + sessionStorage.getItem('access_token'),
    'Refresh_token': "Bearer " + sessionStorage.getItem('refresh_token')
}

var global_notice;
layui.extend({ 'notice': '/Scripts/layui/module/notice/notice' });

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
        global_notice.clear();
    }
}
