var routeLink = api + 'ColumnContent/';
var routePageLink = '/system/ColumnContent/';
var columnlist;
var req_Data;
var currentparentId = getParameterFromUrl();
var btnpermission;
var dateRanage = ['', ''];
var keyword = $('#keyword');

function getParameterFromUrl() {
    var url = window.location.href.toLowerCase();
    var match = url.match(/\/system\/columncontent\/index\/(\d+)/);

    if (match && match[1]) {
        var parameterValue = match[1];
        return parameterValue;
    } else {
        return null;
    }
}

ajaxHttp({
    url: routeLink + 'Column/' + currentparentId,
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
    // 日期范围 - 左右面板独立选择模式
    laydate.render({
        elem: '#ID-laydate-range',
        range: ['#ID-laydate-start-date', '#ID-laydate-end-date'],
        rangeLinked: true,
        done: function (value, date, startdate) {
            dateRanage = value.split(' - ');
            //执行重载
            table.reload('testReloadf', {
                page: {
                    curr: 1 //重新从第 1 页开始
                }
                , where: {
                    ParentId: currentparentId,
                    k: keyword.val(),
                    timefrom: dateRanage[0],
                    timeto: dateRanage[1]
                }
            });
        }
    });
    if (btnpermission.IsAdd)
        toolbarhtml += '<button class="layui-btn layui-btn-sm" lay-event="addRole">添加</button>';
    if (btnpermission.IsUpdate) {
        toolbarhtml += '<button class="layui-btn layui-btn-sm" lay-event="Edit">编辑</button>';
        toolbarhtml += '<button class="layui-btn layui-btn-sm" lay-event="Top">置顶</button>';
        toolbarhtml += '<button class="layui-btn layui-btn-sm" lay-event="Recommend">推荐</button>';
        toolbarhtml += '<button class="layui-btn layui-btn-sm" lay-event="Hot">热门</button>';
        toolbarhtml += '<button class="layui-btn layui-btn-sm" lay-event="Focus">焦点</button>';
        toolbarhtml += '<button class="layui-btn layui-btn-sm" lay-event="Hide">隐藏</button>';
        toolbarhtml += '<button class="layui-btn layui-btn-sm" lay-event="History">修改历史</button>';
    }
    if (btnpermission.IsDelete)
        toolbarhtml += '<button class="layui-btn layui-btn-sm layui-btn-danger" lay-event="deleteAll">删除</button>';
    toolbarhtml += '</div>';
    //JS 调用：
    var insTb = table.render({
        elem: '#demo_tree'
        , url: routeLink + 'ListAsync'
        , height: 'full-80'
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
            k: ' '
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

    var $ = layui.$, active = {
        reload: function () {
            var tableid = 'testReloadf';
            //执行重载
            table.reload(tableid, {
                page: {
                    curr: 1 //重新从第 1 页开始
                }
                , where: {
                    ParentId: currentparentId,
                    k: keyword.val(),
                    timefrom: dateRanage[0],
                    timeto: dateRanage[1]
                }
            });
        }
    };
    $('.listsearch_box .layui-btn').on('click', function () {
        var type = $(this).data('type');
        active[type] ? active[type].call(this) : '';
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
                        url: routeLink + currentparentId + "/" + nodeIds,
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
})