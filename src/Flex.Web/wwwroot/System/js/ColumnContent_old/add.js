﻿//var parent_json = parent.req_Data == null ? { Id: 0, ModelId: 0 } : parent.req_Data;
//Demo

layui.config({
    base: '/Scripts/layui/module/'
}).use(['form', 'tree', 'laytpl'], function () {
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
        url: api + 'ColumnContent/GetFormHtml/' + parent.currentparentId,
        type: 'Get',
        async: false,
        dataType: 'json',
        success: function (result) {
            if (result.code != 200) {
                tips.showFail(json.msg);
                return;
            }
            // 渲染并输出结果
            laytpl(result.msg).render("", function (str) {
                elemView.innerHTML = str;
            });
        },
        complete: function () { }
    })
    //监听提交
    form.on('submit(formDemo)', function (data) {
        data.field.ParentId = parent.currentparentId;
        if (editorarray.length > 0) {
            for (var i = 0; i < editorarray.length; i++) {
                data.field[editorarray[i]] = UE.getEditor(editorarray[i]).getContent();
            }
        }
        $('input[type=checkbox]').each(function () {
            if ($(this)[0].checked)
                data.field[$(this).attr('name')] = true;
            else
                data.field[$(this).attr('name')] = false;
        });
        ajaxHttp({
            url: api + 'ColumnContent',
            type: 'Put',
            data: JSON.stringify(data.field),
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
});