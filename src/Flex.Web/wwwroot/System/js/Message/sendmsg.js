let editorOption = {
    initialFrameWidth: '100%',
    initialFrameHeight: 300,
    simpleEdit: true
};
var ue = new UE.getEditor("content", editorOption);

// 操作列tips
var ischeck = true;
layui.config({
    base: '/Scripts/layui/module/cropper/' //layui自定义layui组件目录
}).use(['form'], function () {
    var form = layui.form, layer = layui.layer;
    
    //监听提交
    form.on('submit(formDemo)', function (data) {
        var content = ue.getContent();
        var newcontent = UnitHtml.unhtml(content);
        data.field['ToPathId'] = $.getUrlParam("stepToId");
        data.field['FromPathId'] = $.getUrlParam("stepFromId");
        data.field['ParentId'] = $.getUrlParam("parentId");
        data.field['ContentId'] = $.getUrlParam("contentId");
        data.field['MsgContent'] = newcontent;
        data.field['BaseFormContent'] = parent.parentformData;
        var json_data = JSON.stringify(data.field);
        ajaxHttp({
            url: api + 'Message/SendReviewMessage',
            type: 'Post',
            datatype: 'json',
            data: json_data,
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
        ue.body.innerHTML = '';
    })
});