var columnlist;
var websitepermission = [];
var parent_json = parent.req_Data;

ajaxHttp({
    url: api + 'RolePermission/GetSitePermissionListById',
    data: { Id: parent_json.Id },
    type: 'Get',
    async: false,
    dataType:'json',
    success: function (json) {
        if (json.code == 200) {
            websitepermission = json.msg;
        }
    },
    complete: function () { }
})
layui.config({
    base: '/scripts/layui/module/'
}).use(['layer', 'treeTable', 'form'], function () {
    var form = layui.form;
    var $ = layui.jquery;
    var treeTable = layui.treeTable;

    // 渲染表格
    var insTb = treeTable.render({
        elem: '#demo_tree',
        url: api + 'SiteManage/SiteManageListAsync',
        method: 'Get',
        height: 'full',
        headers: httpTokenHeaders,
        tree: {
            iconIndex: -1,
            isPidData: false,
            idName: 'Id',
            pidName: 'ParentId'
        },
        defaultToolbar: ['filter', 'print', 'exports', {
            title: '提示',
            layEvent: 'LAYTABLE_TIPS',
            icon: 'layui-icon-tips'
        }],
        cols: [
            { type: "checkbox", fixed: "left", sort: false },
            { title: "站点名", align: "center", templet: "#siteName", sort: false, fixed: "right" },
            { title: "授权", align: "center", sort: false, templet: "#barDemo", fixed: "right" }
        ],
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
        style: 'margin-top:0;',
        done: function () {
            $('input[type=checkbox][name=select_all]').next().click(function () {
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

            $('td[data-key=0-2] .layui-form-checkbox').click(function () {
                if ($(this).hasClass('layui-form-checked')) {
                    $(this).prev().attr('checked', true);
                } else {
                    $(this).prev().attr('checked', false);
                }
            })

            $('.openDataPermission').click(function () {
                var boxtitle = $(this).text();
                var boxurl = $(this).attr('data-href');
                //iframe窗
                layer.open({
                    type: 2,
                    title: boxtitle,
                    shadeClose: true,
                    shade: false,
                    maxmin: true, //开启最大化最小化按钮
                    area: ['90%', '90%'],
                    content: boxurl,
                    end: function () {
                        
                    }
                });
            })
        }
    });

    //监听提交
    form.on('submit(formDemo)', function (data) {
        var json = data.field;
        let ids = '';
        $.each($('input[type=checkbox][name=select_all]'), function () {
            if (ids == '') {
                ids =  getCheckListArray($(this).attr('lay-event'));
            } else {
                ids += "," + getCheckListArray($(this).attr('lay-event'));
            }
        })
        ajaxHttp({
            url: api + 'RolePermission/UpdateSitePermission',
            type: 'Post',
            datatype: 'json',
            data: JSON.stringify({ Id: parseInt(parent_json.Id), chooseId: ids.toString() }),
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
        return false;
    })
    var delete_index = [];
    //监听表格复选框选择
    treeTable.on('checkbox(demo_tree)', function (obj) { // layui 内置方法
        var data = insTb.checkStatus();
        if (obj.checked == true) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].isIndeterminate == false) {
                    $('.layui-table tr[data-index=' + data[i].LAY_INDEX + '] td[data-key=0-2] input[type=checkbox]').next().addClass('layui-form-checked');
                    $('.layui-table tr[data-index=' + data[i].LAY_INDEX + '] td[data-key=0-2] input[type=checkbox]').attr('checked', true);
                } else {
                    $('.layui-table tr[data-index=' + data[i].LAY_INDEX + '] td[data-key=0-2] input[type=checkbox][name=select]').next().addClass('layui-form-checked');
                    $('.layui-table tr[data-index=' + data[i].LAY_INDEX + '] td[data-key=0-2] input[type=checkbox][name=select]').attr('checked', true);
                }
            }
        } else {
            setCheckboxstatus(obj.data);
        }
    });
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
        return id;
    }

    function setCheckboxstatus(datas) {
        if (datas == null || datas == undefined)
            return;
        $('.layui-table tr[data-index=' + datas.LAY_INDEX + '] td[data-key=0-2] input[type=checkbox]').next().removeClass('layui-form-checked');
        $('.layui-table tr[data-index=' + datas.LAY_INDEX + '] td[data-key=0-2] input[type=checkbox]').attr('checked', false);
        if (datas.children == null || datas.children == undefined) {
            return;
        }
        for (var i = 0; i < datas.children.length; i++) {
            $('.layui-table tr[data-index=' + datas.children[i].LAY_INDEX + '] td[data-key=0-2] input[type=checkbox]').next().removeClass('layui-form-checked');
            $('.layui-table tr[data-index=' + datas.children[i].LAY_INDEX + '] td[data-key=0-2] input[type=checkbox]').attr('checked', false);
            setCheckboxstatus(datas.children[i]);
        }
    }
    //获取所有选中的节点id
    function getCheckedId(data) {
        var delete_index = [];
        var id = "";
        $.each(data, function (index, item) {
            if (item.ID != "") {
                delete_index.push(item.ID)
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