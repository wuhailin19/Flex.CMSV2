var routeLink = api + 'ColumnContent/';
var routePageLink = '/system/ColumnContent/';
var columnlist;
var req_Data;

$.ajax({
    url: routeLink + 'Column/1',
    type: 'Get',
    async: false,
    dataType: 'json',
    success: function (result) {
        columnlist = result.content;
    },
    complete: function () { }
})
layui.use(['form', 'laydate', 'util', "table"], function () {
    var form = layui.form;
    var laydate = layui.laydate;
    var table = layui.table;
    // 日期范围 - 左右面板独立选择模式
    laydate.render({
        elem: '#ID-laydate-range',
        range: ['#ID-laydate-start-date', '#ID-laydate-end-date']
    });
    //JS 调用：
    var insTb = table.render({
        elem: '#demo_tree'
        , url: routeLink + 'ListAsync'
        , height: 'full-120'
        , headers: httpTokenHeaders
        , toolbar: '#toolbarDemo'
        , limits: [1, 5, 10, 15, 20]
        , limit: 15
        , page: true //开启分页
        , response: {
            statusCode: 200
        },
        where: {
            ParentId: 1
        },
        parseData: function (res) {
            return {
                "code": res.code, //数据状态的字段名称，默认：code
                "count": res.content.TotalCount, //状态信息的字段名称，默认：msg
                "data": res.content.Items//数据总数的字段名称，默认：count
            };
        }
        , done: function (res, curr, count) {
            //console.log(columnlist);
        }
        , method: 'Get'
        , cols: [columnlist]
    });

    //监听事件
    table.on('toolbar(test)', function (obj) {
        var data = table.checkStatus(obj.config.id).data;
        switch (obj.event) {
            case 'addRole':
                defaultOptions.setAddIframe(layer, insTb);
                break;
            case 'Edit':
                if (data.length != 1) {
                    layer.msg("选择一条数据", { icon: 5, time: 1000 })
                    return;
                }
                req_Data = data[0];
                defaultOptions.editIframe(layer, insTb, data.Id);
                break;
            case 'deleteAll':
                if (data.length == 0)
                    return;
                //获取所有选中节点id数组
                var nodeIds = defaultOptions.getCheckedId(data);
                layer.confirm('确定删除选中数据吗？', { btn: ['确定删除', '取消'] }, function (index) {
                    ajaxHttp({
                        url: routeLink + nodeIds,
                        type: 'Delete',
                        async: false,
                        success: function (json) {
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
})