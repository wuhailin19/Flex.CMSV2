var columnlist;
var req_Data;
$.ajax({
    url: routeLink + 'Column',
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
        , url: routeLink + 'List'
        , height: 'full-30'
        , where: {
            MenuId: parent.req_Data.MenuId
        }
        , toolbar: '#toolbarDemo'
        , limits: [1, 5, 10, 15, 20]
        , limit: 15
        , response: {
            statusCode: 200
        }
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
                /*defaultOptions.setAddIframe(layer, insTb);*/
                var load = layer.load(3, {
                    shade: [0.5, '#000'],
                    content: '生成中...',
                    success: function (layerContentStyle) {
                        layerContentStyle.find('.layui-layer-content').css({
                            'padding-top': '35px',
                            'text-align': 'left',
                            'width': '120px',
                            'color': '#fff'
                        });
                    }
                });
                $.ajax({
                    url: routeLink + 'Add',
                    type: 'Post',
                    datatype: 'json',
                    data: { MenuId: parent.req_Data.MenuId },
                    async: false,
                    success: function (result) {
                        let json = JSON.parse(result);
                        if (json.code == 200) {
                            layer.msg(json.msg, { icon: 6, time: 300 }, function () {
                                defaultOptions.callBack(insTb);
                            });
                        } else {
                            layer.msg(json.msg, { icon: 5, time: 1000 });
                        }
                    },
                    complete: function () {
                        layer.close(load);
                    }
                })
                break;
            case 'deleteAll':
                if (data.length == 0)
                    return;
                //获取所有选中节点id数组
                var nodeIds = defaultOptions.getCheckedId(data);
                layer.confirm('确定删除选中数据吗？', { btn: ['确定删除', '取消'] }, function (index) {
                    $.ajax({
                        url: routeLink + 'Delete',
                        data: { Id: nodeIds },
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
                let indexid = obj.data[defaultOptions.IdName];
                layer.confirm('确定删除本行么', function (index) {
                    $.ajax({
                        url: routeLink + 'Delete/' + indexid,
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
                            } else {
                                layer.msg(json.msg, { icon: 5, time: 1000 })
                                delete_index = [];
                            }
                        },
                        complete: function () { }
                    })
                    layer.close(index);
                });
                break;
            case 'update':
                defaultOptions.updateIframe(layer, insTb);
                break;
            case 'edit':
                defaultOptions.editIframe(layer, insTb);
                break;
            case 'dataPermission':
                defaultOptions.openDataPermissionIframe(layer, insTb);
                break;
            case 'operationPermission':
                defaultOptions.openOperaIframe(layer, insTb);
                break;
        }
    });
});