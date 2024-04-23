var columnlist;
var req_Data;
ajaxHttp({
    url: routeLink + 'Column',
    type: 'Get',
    async: false,
    dataType: 'json',
    success: function (result) {
        columnlist = result.content;
    },
    complete: function () { }
});
layui.use('table', function () {
    var table = layui.table;
    //JS 调用：
    var insTb = table.render({
        elem: '#demo_tree'
        , url: routeLink + 'ListAsync'
        , height: 'full-110'
        , toolbar: '#toolbarDemo'
        , limits: [1, 5, 10, 15, 20]
        , headers: httpTokenHeaders
        , response: {
            statusCode: 200
        },
        where: {
            LogSort: 0
        },
        parseData: function (res) {
            if (res.code != 200) {
                tips.showFail(res.msg);
                return false;
            }
            return {
                "code": res.code, //数据状态的字段名称，默认：code
                "count": res.content.TotalCount, //状态信息的字段名称，默认：msg
                "data": res.content.Items//数据总数的字段名称，默认：count
            };
        }
        , id: 'testReloadf'
        , limit: 15
        , page: true //开启分页
        , method: 'Get'
        , cols: [columnlist]
    });
    var insTbs = table.render({
        elem: '#demo_trees'
        , url: routeLink + 'ListAsync'
        , height: 'full-110'
        , headers: httpTokenHeaders
        , toolbar: '#toolbarDemo'
        , limits: [1, 5, 10, 15, 20]
        , response: {
            statusCode: 200
        }
        ,
        parseData: function (res) {
            if (res.code != 200) {
                tips.showFail(res.msg);
                return false;
            }
            return {
                "code": res.code, //数据状态的字段名称，默认：code
                "count": res.content.TotalCount, //状态信息的字段名称，默认：msg
                "data": res.content.Items//数据总数的字段名称，默认：count
            };
        }
        ,
        where: {
            LogSort: 1
        }
        , id: 'testReloads'
        , limit: 15
        , page: true //开启分页
        , method: 'Get'
        , cols: [columnlist]
    });

    var $ = layui.$, active = {
        reload: function () {
            var demoReload = $('#demoReload');
            var tableid = $('.layui-tab-title .layui-this').data('tableid');
            //执行重载
            table.reload(tableid, {
                page: {
                    curr: 1 //重新从第 1 页开始
                }
                , where: {
                    msg: demoReload.val()
                }
            });
        }
    };
    $('.demoTable .layui-btn').on('click', function () {
        var type = $(this).data('type');
        active[type] ? active[type].call(this) : '';
    });
    var insTbt = table.render({
        elem: '#demo_treet'
        , url: routeLink + 'ListAsync'
        , height: 'full-110'
        , toolbar: '#toolbarDemo'
        , limits: [1, 5, 10, 15, 20]
        , headers: httpTokenHeaders
        , response: {
            statusCode: 200
        },
        parseData: function (res) {
            if (res.code != 200) {
                tips.showFail(res.msg);
                return false;
            }
            return {
                "code": res.code, //数据状态的字段名称，默认：code
                "count": res.content.TotalCount, //状态信息的字段名称，默认：msg
                "data": res.content.Items//数据总数的字段名称，默认：count
            };
        }
        ,
        where: {
            LogSort: 2
        }
        , id: 'testReloadt'
        , limit: 15
        , page: true //开启分页
        , method: 'Get'
        , cols: [columnlist]
    });
    var delete_index = [];
    //监听表格复选框选择
    table.on('checkbox(demo)', function (obj) { // layui 内置方法
        // 自己做处理， 如果是选中
        if (obj.checked == true) {
            delete_index.push(obj.tr['selector']);
        } else {
            // 从列表删除
            var index = delete_index.indexOf(obj.tr['selector']);
            delete delete_index[index];
        }
    });
    //监听事件
    table.on('toolbar(test)', function (obj) {
        switch (obj.event) {
            case "Normal":
                searchbybutton(insTb, 2);
                break;
            case "":
                searchbybutton(insTb, 2);
                break;
            case "Warning":
                searchbybutton(insTb, -1);
                break;
            case "Error":
                searchbybutton(insTb, -2);
                break;
        }
    });
    function searchbybutton($table, $event) {
        $table.reload({
            url: routeLink + 'ListAsync'
            , where: {
                LogLevel: $event
            }
        });
    }
    //监听事件
    table.on('toolbar(tests)', function (obj) {
        switch (obj.event) {
            case "Normal":
                searchbybutton(insTbs, 2);
                break;
            case "":
                searchbybutton(insTbs, 0);
                break;
            case "Warning":
                searchbybutton(insTbs, -1);
                break;
            case "Error":
                searchbybutton(insTbs, -2);
                break;
        }
    });
    //监听事件
    table.on('toolbar(testt)', function (obj) {
        switch (obj.event) {
            case "Normal":
                searchbybutton(insTbt, 2);
                break;
            case "":
                searchbybutton(insTbt, 0);
                break;
            case "Warning":
                searchbybutton(insTbt, -1);
                break;
            case "Error":
                searchbybutton(insTbt, -2);
                break;
        }
    });
});