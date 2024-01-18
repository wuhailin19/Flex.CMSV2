var route_href = window.location.href;
//var api = route_href.split('/')[0] + "//" + route_href.split('/')[2]+"/";
var api = "http://127.0.01:5003/api/";
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
    options.token = localStorage.getItem('access_token');
    options.refreshtoken = localStorage.getItem('refresh_token');
    HttpRequest(options);
}

var httpTokenHeaders = {
    'Authorization': "Bearer " + localStorage.getItem('access_token'),
    'Refresh_token': "Bearer " + localStorage.getItem('refresh_token')
}

var global_notice;
layui.config({
    base: '/Scripts/layui/module/notice/' //layui自定义layui组件目录
}).use('notice', function () {
    global_notice = layui.notice;
    // 初始化配置，同一样式只需要配置一次，非必须初始化，有默认配置
    global_notice.options = {
        closeButton: true,//显示关闭按钮
        debug: false,//启用debug
        positionClass: "toast-top-right",//弹出的位置,
        showDuration: "300",//显示的时间
        hideDuration: "1000",//消失的时间
        timeOut: "2000",//停留的时间
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
    showProStatus: function ($emlemt, msg) {
        tips.index = layer.tips(msg, $emlemt, {
            tips: 4,
            time: tips.timeout     // 3秒消失
        })
    },
    message: function (msg,cate) {
        global_notice[cate](msg);
    },
    showSuccess: function (msg) {
        top.tips.message(msg,'success');
    },
    showFail: function (msg) {
        top.tips.message(msg, 'error');
    },
    closeTips: function () {
        layer.close(tips.index);
    }
}
