﻿var columnlist;
var req_Data;
$.ajax({
    url: routeLink + 'Column',
    type: 'Get',
    async: false,
    dataType: 'json',
    success: function (result) {
        columnlist = result.content;
    },
    complete: function () { }
})
layui.use(['table', 'form'], function () {
    var table = layui.table;
    var form = layui.form;
    //JS 调用：
    var insTb = table.render({
        elem: '#demo'
        , url: routeLink + 'ListAsync'
        , height: 'full-30'
        , headers: httpTokenHeaders
        , toolbar: '#toolbarDemo'
        , limits: [1, 5, 10, 15, 20]
        , limit: 15
        , page: false //开启分页
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
                "data": res.content//数据总数的字段名称，默认：count
            };
        }
        , done: function (res, pageindex, count) {
            if (pageindex > 1 && res.data.length === 0) {
                insTb.reload({
                    page: {
                        curr: pageindex - 1
                    },
                });
            }
        }
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
            delete delete_index[index]
        }
    });
    form.on('switch(usestatus)', function (data) {
        // 得到开关的value值，实际是需要修改的ID值。
        var status = this.checked ? 'true' : 'false';
        const model = { Id: data.value, SelfUse: status };
        ajaxHttp({
            url: routeLink + "QuickEdit",
            data: JSON.stringify(model),
            type: 'Post',
            async: false,
            success: function (json) {
                if (json.code == 200) {
                    tips.showSuccess(json.msg);
                } else {
                    tips.showFail(json.msg);
                }
            },
            complete: function () { }
        })
    })
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
                    ajaxHttp({
                        url: routeLink + nodeIds,
                        type: 'Post',
                        async: false,
                        success: function (json) {
                            if (json.code == 200) {
                                tips.showSuccess(json.msg);
                                // 删除
                                delete_index = [];
                                defaultOptions.callBack(insTb);
                            } else {
                                tips.showFail(json.msg);
                                delete_index = [];
                            }
                        },
                        complete: function () { }
                    })
                    layer.close(index)
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
                    ajaxHttp({
                        url: routeLink +indexid,
                        type: 'Post',
                        async: false,
                        success: function (json) {
                            if (json.code == 200) {
                                tips.showSuccess(json.msg);
                                // 删除
                                delete_index = [];
                                defaultOptions.callBack(insTb);
                            } else {
                                tips.showFail(json.msg);
                                delete_index = [];
                            }
                        },
                        complete: function () { }
                    })
                    layer.close(index);
                });
                break;
            case 'edit':
                defaultOptions.editIframe(layer, insTb);
                break;
            case 'menuEdit':
                defaultOptions.menuEditIframe(layer, insTb);
                break;
            case 'fileds':
                defaultOptions.filedsIframe(layer, insTb);
                break;
            case 'dataPermission':
                defaultOptions.openDataPermissionIframe(layer, insTb);
                break;
            case 'formdesigner':
                defaultOptions.openOperaIframe(layer, insTb);
                break;
        }
    });
});