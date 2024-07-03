var routeLink = api + 'ColumnContent/';
var routePageLink = SystempageRoute + 'ColumnContent/';
var columnlist;
var parentjson = parent.req_Data;
var currentparentId = $.getUrlParam("ParentId");
var btnpermission;
var dateRanage = ['', ''];
var keyword = $('#keyword');

ajaxHttp({
    url: routeLink + 'SoftDeleteColumn/' + currentparentId,
    type: 'Get',
    async: false,
    dataType: 'json',
    success: function (result) {
        btnpermission = result.content;
    },
    complete: function () { }
})
columnlist = btnpermission.TableHeads;
layui.use(['form', 'laydate', 'util', "table"], function () {
    var form = layui.form;
    var laydate = layui.laydate;
    var table = layui.table;
    var toolbarhtml = '<div class="layui-btn-container">';
    if (btnpermission.IsDelete)
        toolbarhtml += '<button class="layui-btn layui-btn-sm layui-btn-danger" lay-event="deleteAll">完全删除</button>';
    if (btnpermission.IsSelect)
        toolbarhtml += '<button class="layui-btn layui-btn-sm" lay-event="Edit">查看</button>';
    if (btnpermission.IsUpdate) {
        toolbarhtml += '<button class="layui-btn layui-btn-sm" lay-event="Reset">恢复</button>';
    }
    toolbarhtml += '</div>';
    //JS 调用：
    var insTb = table.render({
        elem: '#demo_tree'
        , url: routeLink + 'SoftDeleteListAsync'
        , height: 'full-10'
        , id: 'testReloadf'
        , headers: httpTokenHeaders
        , toolbar: toolbarhtml
        , limits: [1, 5, 10, 15, 20]
        , limit: 15
        , page: true //开启分页
        , response: {
            statusCode: 200
        },
        where: {
            ParentId: currentparentId,
            PId: parent.pId,
            ModelId: parent.currentmodelId,
        },
        parseData: function (res) {
            return {
                "code": res.code, //数据状态的字段名称，默认：code
                "count": res.content.TotalCount, //状态信息的字段名称，默认：msg
                "data": res.content.Items//数据总数的字段名称，默认：count
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

    //监听事件
    table.on('toolbar(test)', function (obj) {
        var data = table.checkStatus(obj.config.id).data;
        switch (obj.event) {
            case 'Reset':
                if (data.length == 0)
                    return;
                //获取所有选中节点id数组
                var nodeIds = getCheckedId(data);
                ajaxHttp({
                    url: routeLink + "RestContent/" + parent.currentmodelId + "/" + currentparentId + "/" + nodeIds,
                    type: 'Post',
                    async: false,
                    success: function (json) {
                        if (json.code == 200) {
                            tips.showSuccess(json.msg);
                            refreshTable(insTb);
                        } else {
                            tips.showFail(json.msg);
                        }
                    },
                    complete: function () { }
                });
                
                break;
            case 'Edit':
                if (data.length != 1) {
                    layer.msg("选择一条数据", { icon: 5, time: 1000 })
                    return;
                }
                req_Data = data[0];
                defaultOptions.openOperaIframe(layer, insTb);
                break;
            case 'deleteAll':
                if (data.length == 0)
                    return;
                //获取所有选中节点id数组
                var nodeIds = getCheckedId(data);
                layer.confirm('确定删除选中数据吗？此操作不可恢复', { btn: ['确定删除', '取消'] }, function (index) {
                    ajaxHttp({
                        url: routeLink + "CompletelyDelete/" + parent.currentmodelId + "/" + currentparentId + "/" + nodeIds,
                        type: 'Post',
                        async: false,
                        success: function (json) {
                            if (json.code == 200) {
                                tips.showSuccess(json.msg);
                                // 删除
                                refreshTable(insTb);
                            } else {
                                tips.showFail(json.msg);
                            }
                        },
                        complete: function () { }
                    })
                    layer.close(index)
                })
                break;

        };
    });
})
function refreshTable(tableIns) {
    //第二次调用
    tableIns.reload({
        where: {
            ParentId: currentparentId,
            PId: parent.pId,
            ModelId: parent.currentmodelId,
        } // 设定异步数据接口的额外参数，任意设
    });
}
function getCheckedId(data) {
    var delete_index = [];
    var id = "";
    $.each(data, function (index, item) {
        if (item.ID != "") {
            delete_index.push(item["Id"])
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