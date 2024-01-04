var parent_json = parent.req_Data;
//Demo
var model = {};
ajaxHttp({
    url: api + 'ColumnContent/GetContentById/' + parent_json.ParentId + "/" + parent_json.Id,
    type: 'Get',
    async: false,
    dataType: 'json',
    success: function (result) {
        $.each(result.content, function (key, value) {
            model[key] = value;
        })
    },
    complete: function () { }
})

layui.config({
    base: '/Scripts/layui/module/'
}).use(['form', 'tree', 'laytpl'], function () {
    var form = layui.form;
    var laytpl = layui.laytpl;
    var elemView = document.getElementById('view'); // 视图对象
    ajaxHttp({
        url: api + 'ColumnContent/GetFormHtml/' + parent_json.ParentId,
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
    if (editorarray.length > 0) {
        for (var i = 0; i < editorarray.length; i++) {
            (function (index) {
                let editor = UE.getEditor(editorarray[index]);
                editor.ready(function () {
                    if (model[editorarray[index]] == null)
                        return false;
                    editor.setContent(model[editorarray[index]]);
                });
            })(i);
        }
    }
    function formRender() {
        var formData = form.val("formTest");
        //console.log(model)
        
        for (var key in model) {
            if (model.hasOwnProperty(key)) {
                formData[key] = model[key];
            }
        }
        form.val("formTest", formData);
    }
    formRender();
    //监听提交
    form.on('submit(formDemo)', function (data) {
        data.field.ParentId = parent_json.ParentId;
        data.field.Id = parent_json.Id;
        if (editorarray.length > 0) {
            for (var i = 0; i < editorarray.length; i++) {
                data.field[editorarray[i]] = UE.getEditor(editorarray[i]).getContent();
            }
        }
        ajaxHttp({
            url: api + 'ColumnContent',
            type: 'Post',
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