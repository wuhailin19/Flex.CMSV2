var req_Data = null;
layui.config({
    base: '/Scripts/layui/module/'
}).use(['layer', 'table', 'treeTable'], function () {
    var table = layui.table;
    var layer = layui.layer;
    var $ = layui.jquery;
    var columnlist;
    ajaxHttp({
        url: api + 'ColumnCategory/Column',
        type: 'Get',
        //data: { _type: 'getTableColumn' },
        async: false,
        dataType: 'json',
        success: function (result) {
            columnlist = result.content;
        },
        complete: function () { }
    })
    var treeTable = layui.treeTable;
    // 渲染表格
    var insTb = treeTable.render({
        elem: '#demo_tree',
        url: api + 'ColumnCategory/ListAsync',
        method: 'Get',
        toolbar: '#toolbarDemo',
        height: 'full-10',
        headers: httpTokenHeaders,
        response: {
            statusCode: 200
        },
        parseData: function (res) {
            if (res.code != 200) {
                tips.showFail(res.msg);
                return false;
            }
            return {
                "code": res.code, //数据状态的字段名称，默认：code
                "data": res.content//数据总数的字段名称，默认：count
            };
        },
        tree: {
            iconIndex: 2,
            isPidData: true,
            openName: 'Name',
            idName: 'Id',
            pidName: 'ParentId'
        },
        defaultToolbar: ['filter', 'print', 'exports', {
            title: '提示',
            layEvent: 'LAYTABLE_TIPS',
            icon: 'layui-icon-tips'
        }],
        cols: [
            columnlist
        ],
        style: 'margin-top:0;'
    });
    var delete_index = [];
    //监听表格复选框选择
    treeTable.on('checkbox(demo_tree)', function (obj) { // layui 内置方法
        var data = insTb.checkStatus();
        console.log(data)
        // 自己做处理， 如果是选中
        if (obj.checked == true) {
            if (obj.type == 'all') {
                for (var i = 0; i < data.length; i++) {
                    delete_index.push(data[i].ID);
                }
                return;
            }
            delete_index.push(obj.data.ID);
        } else {
            // 从列表删除
            if (obj.type == 'all')
                return;
            var index = delete_index.indexOf(obj.data.ID);
            delete delete_index[index]
        }
    });
    //监听事件
    treeTable.on('toolbar(demo_tree)', function (obj) {
        var data = insTb.checkStatus(false);
        switch (obj.event) {
            case 'getCheckData':
                layer.alert(JSON.stringify(data));
                break;
            case 'getCheckLength':
                layer.msg('选中了：' + data.length + ' 个');
                break;
            case 'isAll':
                layer.msg(checkStatus.isAll ? '全选' : '未全选')
                break;
            case 'addRole':
                req_Data = null
                let widthstr = '80%';
                let heightstr = '90%';
                //iframe窗
                layer.open({
                    type: 2,
                    title: "添加栏目",
                    shadeClose: true,
                    shade: false,
                    maxmin: true, //开启最大化最小化按钮
                    area: [widthstr, heightstr],
                    content: routePageLink + 'AddPage',
                    end: function () {
                        insTb.refresh();
                    }
                });
                break;
            case 'deleteAll':
                if (data.length == 0)
                    return;
                //获取所有选中节点id数组
                var nodeIds = getCheckedId(data);

                layer.confirm('确定删除选中数据吗？', { btn: ['确定删除', '取消'] }, function (index) {
                    ajaxHttp({
                        url: api + 'ColumnCategory/' + nodeIds,
                        type: 'Delete',
                        //data: { Ids: nodeIds },
                        async: false,
                        success: function (json) {
                            if (json.code == 200) {
                                tips.showSuccess(json.msg);
                                // 删除
                                insTb.refresh();
                                delete_index = [];
                                //insTb.reload();
                                //parent.Init();
                            } else {
                                tips.showFail(json.msg);
                                delete_index = [];
                            }
                        },
                        complete: function () { }
                    })
                    layer.close(index);
                })
                break;
        };
    });
    //监听行工具事件
    treeTable.on('tool(demo_tree)', function (obj) {
        var data = obj.data;
        req_Data = data;
        //console.log(obj)
        if (obj.event === 'del') {
            let indexid = obj.data.Id;
            layer.confirm('确定删除本行么', function (index) {
                ajaxHttp({
                    url: api + 'ColumnCategory/' + indexid,
                    type: 'Delete',
                    //data: { Ids: indexid },
                    async: false,
                    success: function (json) {
                        if (json.code == 200) {
                            tips.showSuccess(json.msg);
                            // 删除
                            insTb.refresh();
                            //parent.Init();
                            delete_index = [];
                            //insTb.reload();
                        } else {
                            tips.showFail(json.msg);
                            delete_index = [];
                        }
                    },
                    complete: function () { }
                })
                layer.close(index);
            });
        }
        else if ("add" == obj.event) {
            //iframe窗
            req_Data = obj.data;
            var index = layer.open({
                type: 2,
                title: '添加',
                shadeClose: true,
                shade: false,
                maxmin: true, //开启最大化最小化按钮
                area: ['80%', '90%'],
                content: routePageLink + 'AddPage',
                success: function (layero, index) {
                }, end: function () {
                    insTb.refresh();
                }
            });
            //$(window).on("resize", function () {
            //    layer.full(index);
            //});
        }
        else if (obj.event === 'edit') {
            req_Data = obj.data;
            //iframe窗
            var index = layer.open({
                type: 2,
                title: '编辑',
                shadeClose: true,
                shade: false,
                maxmin: true, //开启最大化最小化按钮
                area: ['80%', '95%'],
                content: routePageLink + 'Edit',
                success: function (layero, index) {
                }, end: function () {
                    insTb.refresh();
                }
            });
            //$(window).on("resize", function () {
            //    layer.full(index);
            //});
        }
    });
    //获取所有选中的节点id
    function getCheckedId(data) {
        var delete_index = [];
        var id = "";
        $.each(data, function (index, item) {
            if (item.Id != "") {
                delete_index.push(item.Id)
            }
        });
        if (delete_index.length > 0) {
            for (var i = 0; i < delete_index.length; i++) {
                if (id != "") {
                    id += '-' + delete_index[i];
                }
                else {
                    id = delete_index[i];
                }
            }
        }
        return id;
    }
});