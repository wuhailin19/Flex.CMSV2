﻿layui.config({
    base: '/scripts/layui/module/'
}).use(['form', 'tree'], function () {
    let editorOption = {
        initialFrameWidth: '100%',
        initialFrameHeight: 300,
        simpleEdit: true
    };
    var ue = new UE.getEditor("content", editorOption);


    var form = layui.form;
    //监听提交
    form.on('submit(formDemo)', function (data) {
        var participants = getCheckboxValue("participants");
        var content = ue.getContent();
        var newcontent = UnitHtml.unhtml(content);
        data.field['Content'] = newcontent;
        data.field['ProjectId'] = parent.id;
        data.field['participants'] = participants;
        var json = data.field;
        ajaxHttp({
            url: api + 'Product/AddRecord',
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