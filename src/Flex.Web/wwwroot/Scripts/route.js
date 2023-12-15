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
        console.log(options.type)
        if (options.type.toLowerCase() == 'post' && !options.setcontentType) {
            options.contentType = 'application/json';
            console.log(options.type)
            console.log(options.contentType)
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
