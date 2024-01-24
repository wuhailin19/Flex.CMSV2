var demojs = [];
var parent_json = parent.req_Data;
//JavaScript代码区域
layui.config(
    { base: '/scripts/layui/module/formDesigner/' })
    .extend({
        croppers: '../../../layui/module/cropper/croppers'
    })
    .use(['formDesigner', 'form', 'layer', 'upload', 'croppers'], function () {
        var layer = layui.layer;
        var $ = layui.jquery;
        var upload = layui.upload;
        var index = layui.index;
        var formDesigner = layui.formDesigner;
        var form = layui.form;
        var element = layui.element;
        var croppers = layui.croppers;
        var render;

        ajaxHttp({
            url: api + 'ColumnContent/GetFormHtml/' + parent.currentparentId,
            type: 'Get',
            async: false,
            dataType: 'json',
            success: function (result) {
                demojs = JSON.parse(result.msg);
            },
            complete: function () { }
        })

        render = formDesigner.render({
            elem: '#view',
            data: demojs,
            formId: 'view',
            dataFormId: 'formTest',
            formDefaultButton: false,
            viewOrDesign: true
            //formData: {"textarea_1":"123","input_2":"123","password_3":"123"}
        });
        var images = render.getImages();
        for (var i = 0; i < images.length; i++) {
            let id = images[i].select;
            let imageinput = "input[name=" + id + "]";
            //初始化上传工具
            var options = { single: true, autoupload: true, valueElement: imageinput };

            ___initUpload("#uploader-show_" + id, options);

            //创建一个裁剪组件
            croppers.render({
                elem: '#cropper-btn_' + id
                , mark: NaN    //选取比例
                , area: '90%'  //弹窗宽度
                , readyimgelement: imageinput
                , url: images[i].uploadUrl  //图片上传接口返回和（layui 的upload 模块）返回的JOSN一样
                , done: function (data) { //上传完毕回调
                    $(imageinput).val(data);
                }
            });
        }

        var filesData = render.getFiles();

        for (var i = 0; i < filesData.length; i++) {
            let id = filesData[i].select;
            let imageinput = "input[name=" + id + "]";
            //初始化上传工具
            var options = { single: true, autoupload: true, isImage: false, serverUrl: filesData[i].uploadUrl, valueElement: imageinput };

            ___initUpload("#uploader-show_" + id, options);
        }
        //监听提交
        form.on('submit(formDemo)', function (data) {
            $('input[data-group=datastatus]').each(function () {
                if ($(this)[0].checked)
                    data.field[$(this).attr('name')] = true;
                else
                    data.field[$(this).attr('name')] = false;
            });

            var json = render.getOptions().data;
            var checkboxs = json.filter(item => item.tag == "checkbox");
            if (checkboxs.length > 0) { 
                for (var i = 0; i < checkboxs.length; i++) {
                    $('input[data-filedgroup=' + checkboxs[i].id + ']').each(function () {
                        if ($(this)[0].checked)
                            data.field[$(this).attr('data-filedgroup')] = $(this).val();
                        else
                            data.field[$(this).attr('data-filedgroup')] = "";
                    });
                }
            }
            layer.msg(JSON.stringify(data.field), { icon: 6 });
            console.log(data.field)
            return false;
        });
        $('#globalDisable').on('click', function () {
            render.globalDisable();
        });
        $('#globalNoDisable').on('click', function () {
            render.globalNoDisable();
        });
    });


function getSubmitData() {
    var data = $('#view').form[0].serialize();
    return data;
}
