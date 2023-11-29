layui.config({
    base: '/Scripts/layui/module/'
}).use(['form'], function () {
    var form = layui.form;
    //监听提交
    form.on('submit(formDemo)', function (data) {
        var json = data.field;
        $.ajax({
            url: api + 'MenuApi/Add?cateid=' + cateid,
            type: 'Post',
            datatype: 'json',
            data: JSON.stringify(json),
            async: false,
            success: function (result) {
                let json = JSON.parse(result);
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
    $('#reset').click(function () {
        insTb.reload();
        return false;
    })
});