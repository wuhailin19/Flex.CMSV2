﻿var columnlist;
var req_Data;
$.ajax({
    url: routeLink + 'getTableColumn',
    type: 'Post',
    async: false,
    dataType: 'json',
    success: function (result) {
        columnlist = result.data;
    },
    complete: function () { }
})
layui.use('table', function () {
    var table = layui.table;
    //JS 调用：
    var insTb = table.render({
        elem: '#demo'
        , url: routeLink + 'GetJsonTreeTable'
        , height: 'full-30'
        , toolbar: '#toolbarDemo'
        , limits: [1, 5, 10, 15, 20]
        , limit: 15
        , page: true //开启分页
        , method: 'Post'
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
            delete delete_index[index]
        }
    });
    //监听事件
    table.on('toolbar(test)', function (obj) {
        var data = table.checkStatus(obj.config.id).data;
        switch (obj.event) {
            case 'addRole':
                defaultOptions.setAddIframe(layer, insTb);
                break;
            case 'deleteAll':
                if (data.length == 0)
                    return;
                //获取所有选中节点id数组
                var nodeIds = defaultOptions.getCheckedId(data);
                layer.confirm('确定删除选中数据吗？', { btn: ['确定删除', '取消'] }, function (index) {
                    $.ajax({
                        url: routeLink + 'Delete/' + nodeIds,
                        type: 'Post',
                        async: false,
                        success: function (result) {
                            if (result == undefined || result == '')
                                return;
                            let json = JSON.parse(result);
                            if (json.code == 200) {
                                layer.msg(json.msg, { icon: 6, time: 1000 });
                                // 删除
                                delete_index = [];
                                defaultOptions.callBack(insTb);
                            } else {
                                layer.msg(json.msg, { icon: 5, time: 1000 })
                                delete_index = [];
                            }
                        },
                        complete: function () { }
                    })
                })
                break;
            
        };
    });
    //监听行工具事件
    table.on('tool(test)', function (obj) {
        var data = obj.data;
        req_Data = data;
        switch (obj.event) {
            case 'del':
                break;
            case 'update':
                defaultOptions.updateIframe(layer, insTb);
                break;
            case 'edit':
                defaultOptions.editIframe(layer, insTb);
                break;
            case 'operationPermission':
                defaultOptions.openOperaIframe(layer, insTb);
                break;
        }
    });
});