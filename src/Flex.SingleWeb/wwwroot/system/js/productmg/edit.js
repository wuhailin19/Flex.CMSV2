layui.config({
    base: '/scripts/layui/module/'
}).use(['form', 'tree'], function () {
    let editorOption = {
        initialFrameWidth: '100%',
        initialFrameHeight: 300,
        simpleEdit: true
    };
    var ue = new UE.getEditor("content", editorOption);

    var form = layui.form;
    var data;
    ajaxHttp({
        url: api + 'Product/DetailAsync?id=' + parent.updateid,
        type: 'Get',
        async: false,
        dataType: 'json',
        success: function (json) {
            if (json.code == 200) {
                data = json.content;
                ue.ready(function () {
                    ue.setContent(UnitHtml.html(data.ServerInfo));
                })
                setCheckboxValue("participants",data.Participants);
                form.val("formTest", {
                    'ProductName': data.ProductName
                });
            } else {
                tips.showFail(json.msg);
            }
        },
        complete: function () { }
    })
    //监听提交
    form.on('submit(formDemo)', function (data) {
        var participants = getCheckboxValue("participants");
        var content = ue.getContent();
        var newcontent = UnitHtml.unhtml(content);
        data.field['Id'] = parent.updateid;
        data.field['ServerInfo'] = newcontent;
        data.field['participants'] = participants;
        var json = data.field;
        ajaxHttp({
            url: api + 'Product/UpdateProject',
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
    //$('#reset').click(function () {
    //    insTb.reload();
    //    return false;
    //})
});