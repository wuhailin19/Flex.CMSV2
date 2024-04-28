let json_data = [];
ajaxHttp({
    url: api + 'ContentModel/GetFormHtml/' + parent.req_Data.Id,
    type: 'Get',
    async: false,
    dataType: 'json',
    success: function (result) {
        if (result.msg != null)
            json_data = JSON.parse(result.msg);
    },
    complete: function () { }
})

layui.extend({ 'formDesigner': '/scripts/layui/module/formdesigner/formdesigner' });
layui.use(['layer', 'formDesigner'], function () {
    var formDesigner = layui.formDesigner;
    let formdata = json_data ? json_data.slice() : [];
    var render = formDesigner.render({
        data: formdata,
        elem: '#formdesigner'
    });

});