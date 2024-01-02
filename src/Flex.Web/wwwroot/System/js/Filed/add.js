// 获取选中节点的id
var routeLink = api + 'Field/';
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
    base: '/Scripts/layui/module/'
}).use(['form', 'tree'], function () {
    var form = layui.form;
    //监听提交
    form.on('submit(formDemo)', function (data) {
        //let validationstr = '';
        //$.each($('.Validation'), function (index, item) {
        //    validationstr += " " + $(item).val();
        //})
        //data.field.Validation = validationstr;
        data.field.ModelId = parent.parentdata.Id;
        ajaxHttp({
            url: routeLink,
            type: 'Put',
            datatype: 'json',
            data: JSON.stringify(data.field),
            async: false,
            success: function (json) {
                if (json.code == 200) {
                    layer.msg(json.msg, { icon: 6, time: 300 }, function () {
                        var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
                        parent.layer.close(index); //再执行关闭
                    });
                } else {
                    layer.msg(json.msg, { icon: 5, time: 1000 });
                }
            },
            complete: function () {

            }
        })
        return false;
    });
    //$('#reset').click(function () {
    //    insTb.reload();
    //    return false;
    //})
});