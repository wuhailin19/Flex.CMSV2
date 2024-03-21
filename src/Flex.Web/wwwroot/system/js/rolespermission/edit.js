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
    var parent_json = parent.req_Data;
    //监听提交
    form.on('submit(formDemo)', function (data) {
        var json = data.field;
        ajaxHttp({
            url: api + 'RolePermission',
            type: 'Post',
            datatype: 'json',
            data: JSON.stringify(json),
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
    function formRender() {
        form.val("formTest", {
            'RolesName': parent_json.RolesName
            , 'RolesDesc': parent_json.RolesDesc
            , 'Id': parent_json.Id
        });
    }
    formRender();
    $('#reset').click(function () {
        formRender();
        return false;
    })
});