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
})
layui.use('table', function () {
    var table = layui.table;
    //JS 调用：
    var insTb = table.render({
        elem: '#demo'
        , url: routeLink + 'ListAsync'
        , height: 'full-30'
        , headers: httpTokenHeaders
        , toolbar: '#toolbarDemo'
        , limits: [1, 5, 10, 15, 20]
        , response: {
            statusCode: 200
        },
        where: {
            cateid: defaultOptions.cateid
        }
        , limit: 15
        , page: true //开启分页
        , method: 'Get'
        , cols: [columnlist],
        parseData: function (res) {
            console.log(res);
            return {
                "code": res.code, //数据状态的字段名称，默认：code
                "count": res.content.TotalCount, //状态信息的字段名称，默认：msg
                "data": res.content.Items//数据总数的字段名称，默认：count
            };
        }
        ,done: function (res, curr, count) {
            if (curr > 1 && res.data.length === 0) {
                insTb.page = curr - 1;
            }
        }
    });
    form.on('switch(loginstatus)', function (data) {
        // 得到开关的value值，实际是需要修改的ID值。
        var status = this.checked ? 'true' : 'false';
        const model = { Id: data.value, AllowMultiLogin: status };
        ajaxHttp({
            url: routeLink + "QuickEdit",
            data: JSON.stringify(model),
            type: 'Post',
            async: false,
            success: function (json) {
                if (json.code == 200) {
                    layer.msg(json.msg, { icon: 6, time: 1000 });
                } else {
                    layer.msg(json.msg, { icon: 5, time: 1000 })
                }
            },
            complete: function () { }
        })
    })
    form.on('switch(lockstatus)', function (data) {
        // 得到开关的value值，实际是需要修改的ID值。
        var status = this.checked ? 'true' : 'false';
        const model = { Id: data.value, Islock: status };
        ajaxHttp({
            url: routeLink + "QuickEdit",
            data: JSON.stringify(model),
            type: 'Post',
            async: false,
            success: function (json) {
                if (json.code == 200) {
                    layer.msg(json.msg, { icon: 6, time: 1000 });
                } else {
                    layer.msg(json.msg, { icon: 5, time: 1000 })
                }
            },
            complete: function () { }
        })
    })
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
                layer.confirm('确定删除选中数据吗？', { btn: ['确定删除', '取消'] }, function (index) {
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
                                defaultOptions.callBack(insTb);
                                // 删除
                            } else {
                                layer.msg(json.msg, { icon: 5, time: 1000 })
                            }
                        },
                        complete: function () { }
                    })
                })
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
            case 'roleAppUser':
                defaultOptions.roleAppUserIfame(layer, insTb);
                break;
        }
    });
});