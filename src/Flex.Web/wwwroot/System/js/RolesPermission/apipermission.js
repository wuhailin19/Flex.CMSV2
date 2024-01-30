$('.layui-tab-title li').click(function () {
    let t_index = $(this).index();
    $(this).addClass('layui-this').siblings().removeClass('layui-this');
    $('.layui-tab-item').eq(t_index).addClass('layui-show').siblings().removeClass('layui-show');
})
var datamission = [];
var parent_json = parent.req_Data;
ajaxHttp({
    url: api + 'RoleUrl/GetRoleUrlListById',
    data: { Id: parent_json.Id },
    type: 'Get',
    async: false,
    success: function (json) {
        if (json.code == 200) {
            datamission = json.content;
        }
    },
    complete: function () { }
})

layui.config({
    base: '/Scripts/layui/module/'
}).use(['layer', 'form', 'element', 'treeTable'], function () {
    var form = layui.form;
    var $ = layui.jquery;
    var treeTable = layui.treeTable;
    var element = layui.element; //Tab的切换功能，切换事件监听等，需要依赖element模块
    // 渲染表格
    var insTb = treeTable.render({
        elem: '#demo_tree',
        url: api + 'RoleUrl/ListAsync',
        method: 'Get',
        height: 'full',
        headers: httpTokenHeaders,
        where: { cateid: 1 },
        tree: {
            iconIndex: 1,
            isPidData: true,
            openName: 'Name',
            idName: 'Id',
            pidName: 'ParentId'
        },
        parseData: function (res) {
            return {
                "code": res.code,
                "count": res.content.TotalCount,
                "data": res.content
            };
        },
        defaultToolbar: ['filter', 'print', 'exports', {
            title: '提示',
            layEvent: 'LAYTABLE_TIPS',
            icon: 'layui-icon-tips'
        }],
        id: 'testReloadf',
        cols: [
            { type: "checkbox", fixed: "left", sort: false },
            { field: "Name", title: "接口名", sort: false },
            { field: "Url", title: "链接", sort: false },
            { title: "授权", align: "center", sort: false, templet: "#barDemo", fixed: "right" }
        ],
        style: 'margin-top:0;',
        done: function () {
            $('input[type=checkbox][name=select_all]').next().click(function () {
                console.log(11)
                let event = $(this).prev().attr('lay-event');
                var checkbox = $('input[type=checkbox][name=' + event + ']');
                var next = checkbox.next();
                let checkclass = 'layui-form-checked';
                let count = 0;
                $.each(next, function () {
                    if ($(this).hasClass(checkclass)) {
                        count++;
                    }
                })
                if (next.length == count) {
                    next.removeClass(checkclass)
                    checkbox.attr('checked', false);
                    $(this).removeClass(checkclass);
                } else {
                    next.addClass(checkclass);
                    checkbox.attr('checked', true);
                    $(this).addClass(checkclass);
                }
            })
            $('td[data-key=0-3] .layui-form-checkbox').click(function () {
                if ($(this).hasClass('layui-form-checked')) {
                    $(this).prev().attr('checked', true);
                } else {
                    $(this).prev().attr('checked', false);
                }
            })
        }
    });
    var insTbs = treeTable.render({
        elem: '#demo_trees',
        url: api + 'RoleUrl/ListAsync',
        method: 'Get',
        height: 'full',
        headers: httpTokenHeaders,
        where: { cateid: 2 },
        tree: {
            iconIndex: 1,
            isPidData: true,
            openName: 'Name',
            idName: 'Id',
            pidName: 'ParentId'
        },
        parseData: function (res) {
            return {
                "code": res.code,
                "count": res.content.TotalCount,
                "data": res.content
            };
        },
        id: 'testReloads',
        defaultToolbar: ['filter', 'print', 'exports', {
            title: '提示',
            layEvent: 'LAYTABLE_TIPS',
            icon: 'layui-icon-tips'
        }],
        cols: [
            { type: "checkbox", fixed: "left", sort: false },
            { field: "Name", title: "接口名", sort: false },
            { field: "Url", title: "链接", sort: false },
            { title: "授权", align: "center", sort: false, templet: "#barDemos", fixed: "right" }
        ],
        style: 'margin-top:0;',
        done: function () {
            $('input[type=checkbox][name=select_alls]').next().click(function () {
                console.log(22)

                let event = $(this).prev().attr('lay-event');
                var checkbox = $('input[type=checkbox][name=' + event + ']');
                var next = checkbox.next();
                let checkclass = 'layui-form-checked';
                let count = 0;
                $.each(next, function () {
                    if ($(this).hasClass(checkclass)) {
                        count++;
                    }
                })
                if (next.length == count) {
                    next.removeClass(checkclass)
                    checkbox.attr('checked', false);
                    $(this).removeClass(checkclass);
                } else {
                    next.addClass(checkclass);
                    checkbox.attr('checked', true);
                    $(this).addClass(checkclass);
                }
            })
            $('td[data-key=0-3] .layui-form-checkbox').click(function () {
                if ($(this).hasClass('layui-form-checked')) {
                    $(this).prev().attr('checked', true);
                } else {
                    $(this).prev().attr('checked', false);
                }
            })
        }
    });
    function setCheckboxstatus(datas) {
        if (datas == null || datas == undefined)
            return;
        $('.layui-table tr[data-index=' + datas.LAY_INDEX + '] td[data-key=0-3] input[type=checkbox]').next().removeClass('layui-form-checked');
        $('.layui-table tr[data-index=' + datas.LAY_INDEX + '] td[data-key=0-3] input[type=checkbox]').attr('checked', false);
        if (datas.children == null || datas.children == undefined) {
            return;
        }
        for (var i = 0; i < datas.children.length; i++) {
            $('.layui-table tr[data-index=' + datas.children[i].LAY_INDEX + '] td[data-key=0-3] input[type=checkbox]').next().removeClass('layui-form-checked');
            $('.layui-table tr[data-index=' + datas.children[i].LAY_INDEX + '] td[data-key=0-3] input[type=checkbox]').attr('checked', false);
            setCheckboxstatus(datas.children[i]);
        }
    }
    //监听表格复选框选择
    treeTable.on('checkbox(demo_tree)', function (obj) { // layui 内置方法
        var data = insTb.checkStatus();
        if (obj.checked == true) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].isIndeterminate == false) {
                    $('.layui-table tr[data-index=' + data[i].LAY_INDEX + '] td[data-key=0-3] input[type=checkbox]').next().addClass('layui-form-checked');
                    $('.layui-table tr[data-index=' + data[i].LAY_INDEX + '] td[data-key=0-3] input[type=checkbox]').attr('checked', true);
                } else {
                    $('.layui-table tr[data-index=' + data[i].LAY_INDEX + '] td[data-key=0-3] input[type=checkbox][name=select]').next().addClass('layui-form-checked');
                    $('.layui-table tr[data-index=' + data[i].LAY_INDEX + '] td[data-key=0-3] input[type=checkbox][name=select]').attr('checked', true);
                }
            }
        } else {
            setCheckboxstatus(obj.data);
        }
    });
    //监听表格复选框选择
    treeTable.on('checkbox(demo_trees)', function (obj) { // layui 内置方法
        var data = insTbs.checkStatus();
        if (obj.checked == true) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].isIndeterminate == false) {
                    $('.layui-table tr[data-index=' + data[i].LAY_INDEX + '] td[data-key=0-3] input[type=checkbox]').next().addClass('layui-form-checked');
                    $('.layui-table tr[data-index=' + data[i].LAY_INDEX + '] td[data-key=0-3] input[type=checkbox]').attr('checked', true);
                } else {
                    $('.layui-table tr[data-index=' + data[i].LAY_INDEX + '] td[data-key=0-3] input[type=checkbox][name=select]').next().addClass('layui-form-checked');
                    $('.layui-table tr[data-index=' + data[i].LAY_INDEX + '] td[data-key=0-3] input[type=checkbox][name=select]').attr('checked', true);
                }
            }
        } else {
            setCheckboxstatus(obj.data);
        }
    });
    var $ = layui.$, active = {
        reload: function () {
            var demoReload = $('#demoReload');
            var tableid = $('.layui-tab-title .layui-this').data('tableid');
            var model = insTb;
            console.log(tableid)
            if (tableid == "testReloadf")
                model = insTb;
            else
                model = insTbs;
            //执行重载
            model.reload({
                page: {
                    curr: 1 //重新从第 1 页开始
                }
                , where: {
                    k: demoReload.val()
                }
            });
        }
    };
    $('.demoTable .layui-btn').on('click', function () {
        var type = $(this).data('type');
        active[type] ? active[type].call(this) : '';
    });

    //监听提交
    form.on('submit(formDemo)', function (data) {
        var json = data.field;
        let ids = '';
        $.each($('input[type=checkbox][name=select_all]'), function () {
            if (ids == '') {
                ids = "{" + getCheckListArray($(this).attr('lay-event'));
            } else {
                ids += "," + getCheckListArray($(this).attr('lay-event'));
            }
        })
        $.each($('input[type=checkbox][name=select_alls]'), function () {
            if (ids == '') {
                ids = "{" + getCheckListArray($(this).attr('lay-event'));
            } else {
                ids += "," + getCheckListArray($(this).attr('lay-event'));
            }
        })
        ids += "}";
        ajaxHttp({
            url: api + 'RolePermission/UpdateApiPermission',
            type: 'Post',
            datatype: 'json',
            data: JSON.stringify({ Id: parent_json.Id, sysapis: ids }),
            async: false,
            success: function (json) {
                if (json.code == 200) {
                    tips.showSuccess(json.msg);
                    setTimeout(function () {
                        var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
                        parent.layer.close(index); //再执行关闭
                    }, 300)
                } else {
                    tips.showFail(json.msg);
                }
            },
            complete: function () {

            }
        })
        return false;
    });
    $('#reset').click(function () {
        insTb.refresh();
        insTbs.refresh();
        return false;
    })
    function getCheckListArray(event) {
        var checkbox = $('input[type=checkbox][name=' + event + '][checked]');
        let id = '';
        for (var i = 0; i < checkbox.length; i++) {
            if (id != '') {
                id += '-' + $(checkbox[i]).data('id');
            }
            else {
                id = $(checkbox[i]).data('id');
            }
        }
        return '"' + event + '":"' + id + '"';
    }
});