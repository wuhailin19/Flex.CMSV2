//var parent_json = parent.req_Data == null ? { Id: 0, ModelId: 0 } : parent.req_Data;
//Demo

layui.config({
    base: '/Scripts/layui/module/'
}).use(['form', 'tree','laytpl'], function () {
    var form = layui.form;
    var laytpl = layui.laytpl;
    var elemView = document.getElementById('view'); // 视图对象
    //var model = {
    //    Name: parent_json.Name,
    //    Description: parent_json.Description,
    //    TableName: parent_json.TableName,
    //    ArticleContent: parent_json.ArticleContent,
    //    Id: parent_json.Id
    //}
    ajaxHttp({
        url: api + 'ColumnContent/GetFormHtml/1',
        type: 'Get',
        async: false,
        dataType: 'json',
        success: function (result) {
            // 渲染并输出结果
            laytpl(result.msg).render("", function (str) {
                elemView.innerHTML = str;
            });
        },
        complete: function () { }
    })
    //监听提交
    form.on('submit(formDemo)', function (data) {
        ajaxHttp({
            url: api + 'ColumnCategory',
            type: 'Put',
            data: JSON.stringify(data.field),
            async: false,
            success: function (json) {
                if (json.code == 200) {
                    layer.msg(json.msg, { icon: 6, time: 300 }, function () {
                        var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
                        parent.layer.close(index); //再执行关闭
                        //parent.parent.Init();
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
});