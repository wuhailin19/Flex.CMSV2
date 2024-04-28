// 获取选中节点的id
function getChecked_list(data, type) {
    var id = "";
    $.each(data, function (index, item) {
        if (item.children.length == 0) {
            if (id != "") {
                id = id + "," + item.id;
            }
            else {
                id = item.id;
            }
        }
        var i = getChecked_list(item.children);
        if (i != "") {
            id = id + "," + i;
        }
    });
    return id;
}
layui.config({
    base: '/scripts/layui/module/'
}).use(['form', 'tree'], function () {
    var form = layui.form;
    var columnlist;
    var parent_json = parent.req_Data;

    ajaxHttp({
        url: api + 'Menu/GetTreeListByIdAsync/' + parent_json.Id,
        type: 'Get',
        async: false,
        dataType: 'json',
        success: function (json) {
            if (json.code == 200) {
                columnlist = json.content;
            }
        },
        complete: function () { }
    })
    var tree = layui.tree;
    var insTb = tree.render({
        elem: '#ParentID',
        data: columnlist
        , id: 'id'
        , showCheckbox: true
        , onlyIconControl: true
        , click: function (obj) {

        }
    });
    //监听提交
    form.on('submit(formDemo)', function (data) {
        var json = data.field;
        var checkData = tree.getChecked('id');
        var list = getChecked_list(checkData);
        var model = {
            Id: parent_json.Id,
            MenuPermissions: list
        }
        ajaxHttp({
            url: api + 'RolePermission/UpdateMenuPermission',
            type: 'Post',
            datatype: 'json',
            data: JSON.stringify(model),
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
        insTb.reload();
        return false;
    })
});