var demojs = [];
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
            url: api + 'ContentModel/GetFormHtml/' + parent.parent.req_Data.Id,
            type: 'Get',
            async: false,
            dataType: 'json',
            success: function (result) {
                demojs = JSON.parse(result.msg);
            },
            complete: function () { }
        })

        render = formDesigner.render({
            elem: '#testdemo',
            data: demojs,
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
        form.on('submit(demo1)', function (data) {
            var json = render.getFormData();

            layer.msg(JSON.stringify(json), { icon: 6 });
            return false;
        });

        //导入数据
        $('#importJsonData').on('click', function () {
            layer.open({
                type: 1
                , title: 'JSON模板数据导入'
                , id: 'Lay_layer_importjsoncodeview'
                , content: $('.importjsoncodedataview')
                , area: ['800px', '640px']
                , shade: false
                , resize: false
                , success: function (layero, index) {
                }
                , end: function () {
                    $('.importjsoncodeview').css("display", "none")
                }
            });
        });

        //导入数据
        $('#getFormData').on('click', function () {
            var _value = render.getFormData();
            $('#get-form-data').val(JSON.stringify(_value, null, 4));
            layer.open({
                type: 1
                , title: 'JSON模板数据导入'
                , id: 'Lay_layer_importjsoncodeview'
                , content: $('.getFormData')
                , area: ['800px', '640px']
                , shade: false
                , resize: false
                , success: function (layero, index) {
                }
                , end: function () {
                    $('.getFormData').css("display", "none")
                }
            });
        });

        $('#import-json-code-data').on('click', function () {
            var _value = document.getElementById("import-json-code-view").value;
            try {
                var json = JSON.parse(_value);
                render.setFormData(json);
                layer.closeAll();
                layer.msg('导入成功');
            } catch (e) {
                layer.closeAll();
                layer.msg('导入数据格式异常');
            }
        });

        $('#globalDisable').on('click', function () {
            render.globalDisable();
        });
        $('#globalNoDisable').on('click', function () {
            render.globalNoDisable();
        });
    });


function getSubmitData() {
    var data = $('#testdemo').form[0].serialize();
    return data;
}
