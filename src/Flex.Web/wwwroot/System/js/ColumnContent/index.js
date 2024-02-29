var routeLink = api + 'ColumnContent/';
var routePageLink = '/system/ColumnContent/';
var columnlist;
var req_Data;
var parentjson;
var currentparentId = getParameterFromUrl();
var btnpermission;
var dateRanage = ['', ''];
var keyword = $('#keyword');
var tableid = 'testReloadf';

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
layui.use(['form', 'laydate', 'util', "table", 'dropdown'], function () {
    var form = layui.form;
    var laydate = layui.laydate;
    var table = layui.table;
    var dropdown = layui.dropdown;

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
        toolbarhtml += '<button class="layui-btn layui-btn-sm" id="setProperty">设置属性<i class="layui-icon layui-icon-down layui-font-12"></i></button>';
        toolbarhtml += '<button class="layui-btn layui-btn-sm" id="cacelProperty">取消属性<i class="layui-icon layui-icon-down layui-font-12"></i></button>';
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
        , id: tableid
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
        , method: 'Get'
        , cols: [columnlist]
        , done: function (res, pageindex, count) {
            if (pageindex > 1 && res.data.length === 0) {
                insTb.reload({
                    page: {
                        curr: pageindex - 1
                    },
                });
            }

            var id = this.id;
            // 设置属性
            dropdown.render({
                elem: '#setProperty', // 可绑定在任意元素中，此处以上述按钮为例
                data: [
                    { id: 'IsTop', title: '置顶' },
                    { id: 'IsRecommend', title: '推荐' },
                    { id: 'IsHot', title: '热门' },
                    { id: 'IsSlide', title: '焦点' },
                    { id: 'IsHide', title: '隐藏' }
                ],
                // 菜单被点击的事件
                click: function (obj) {
                    var checkStatus = table.checkStatus(id)
                    var data = checkStatus.data; // 获取选中的数据
                    let ids = getIdsFromList(data);
                    UpdateStatus(ids, obj.id, true);
                }
            });
            // 取消属性
            dropdown.render({
                elem: '#cacelProperty', // 可绑定在任意元素中，此处以上述按钮为例
                data: [
                    { id: 'IsTop', title: '取消置顶' },
                    { id: 'IsRecommend', title: '取消推荐' },
                    { id: 'IsHot', title: '取消热门' },
                    { id: 'IsSlide', title: '取消焦点' },
                    { id: 'IsHide', title: '显示' }
                ],
                // 菜单被点击的事件
                click: function (obj) {
                    var checkStatus = table.checkStatus(id)
                    var data = checkStatus.data; // 获取选中的数据
                    let ids = getIdsFromList(data);
                    UpdateStatus(ids, obj.id, false);
                }
            });
        }
    });

    function getIdsFromList(data) {
        let ids = '';
        $.each(data, function (index, item) {
            if (ids == '')
                ids += item.Id;
            else
                ids += ',' + item.Id;
        })
        return ids;
    }

    // 复选框事件
    table.on('checkbox(test)', function (obj) {

    });
    function UpdateStatus(ids, event, result) {
        if (ids == "") {
            layer.msg("选择一条数据", { icon: 5, time: 1000 })
            return;
        }
        var model = { ParentId: currentparentId, Ids: ids, };
        model[event] = result;
        ajaxHttp({
            url: routeLink + 'UpdateStatus',
            type: 'Post',
            data: JSON.stringify(model),
            async: false,
            dataType: 'json',
            success: function (result) {
                if (result.code == 200) {
                    //执行重载
                    table.reload(tableid, {
                        page: {
                            curr: insTb.config.page.curr //刷新当前页码
                        }
                        , where: {
                            ParentId: currentparentId,
                            k: keyword.val(),
                            timefrom: dateRanage[0],
                            timeto: dateRanage[1]
                        }
                    });
                    tips.showSuccess(result.msg);
                }
            },
            complete: function () { }
        })
    }
    var $ = layui.$, active = {
        reload: function () {
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
            case 'History':
                if (data.length != 1) {
                    layer.msg("选择一条数据", { icon: 5, time: 1000 })
                    return;
                }
                req_Data = data[0];
                defaultOptions.openDataPermissionIframe(layer, insTb);
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