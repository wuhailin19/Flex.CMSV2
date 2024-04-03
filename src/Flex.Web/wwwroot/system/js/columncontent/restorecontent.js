var parent_json = {};
//Demo
var model = {};

function InitParams() {
    parent_json.ParentId = $.getUrlParam("ParentId");
    parent_json.Id = $.getUrlParam("Id");
}
InitParams();
var NeedReview = false;
ajaxHttp({
    url: api + 'ColumnContent/GetContentById/' + parent_json.ParentId + "/" + parent_json.Id,
    type: 'Get',
    async: false,
    dataType: 'json',
    success: function (result) {
        model = result.content.Content;
        if (!result.content.NeedReview) {
            $('#bottomBtnbox').append('<button class="layui-btn layui-btn-sm" lay-submit lay-filter="formDemo">立即提交</button>');
            return false;
        }
        NeedReview = true;
        if (result.content.stepActionButtonDto.length > 0) {
            var buttons = result.content.stepActionButtonDto;
            for (var i = 0; i < buttons.length; i++) {
                let layui_class = buttons[i].stepToCate == "end-error" ? "layui-btn-warm" : buttons[i].stepToCate == "end-cancel" ? "layui-btn-danger" : "";
                $('#bottomBtnbox').append('<button class="layui-btn layui-btn-sm reviewbutton ' + layui_class + '" data-event="' + buttons[i].stepToCate + '" onclick="return false;" data-fromid="' + buttons[i].stepFromId + '" data-toid="' + buttons[i].stepToId + '">' + buttons[i].actionName + '</button>');
            }
        } else {
            $('#bottomBtnbox').append('<span class="layui-btn layui-btn-warm layui-btn-sm">内容审批中</span>');
            if (result.content.OwnerShip)
                $('#bottomBtnbox').append('<button class="layui-btn layui-btn-danger layui-btn-sm cancelreview">取消审批</button>');
        }
    },
    complete: function () { }
})

var demojs = [];
var parentformData;
//JavaScript代码区域
layui.extend({
    'croppers': '../../../layui/module/cropper/croppers',
    'formDesigner': '/scripts/layui/module/formdesigner/formdesigner'
});
layui.use(['formDesigner', 'form', 'layer', 'upload', 'croppers'], function () {
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
            url: api + 'ColumnContent/GetFormHtml/' + parent_json.ParentId,
            type: 'Get',
            async: false,
            dataType: 'json',
            success: function (result) {
                demojs = JSON.parse(result.msg);
            },
            complete: function () { }
        })
        var rates = demojs.filter(item => item.tag == "rate");
        if (rates.length > 0) {
            for (var i = 0; i < rates.length; i++) {
                if (model.hasOwnProperty(rates[i].id))
                    rates[i]["defaultValue"] = model[rates[i].id];
            }
        }
        var checkboxs = demojs.filter(item => item.tag == "checkbox");
        var remotedata_checkboxs = checkboxs.filter(item => item.LocalSource == false);
        if (remotedata_checkboxs.length > 0) {
            for (var i = 0; i < remotedata_checkboxs.length; i++) {
                (function (index) {
                    ajaxHttp({
                        url: remotedata_checkboxs[index].remoteUrl,
                        type: remotedata_checkboxs[index].remoteMethod,
                        async: false,
                        dataType: 'json',
                        success: function (res) {
                            if (res.code == 200) {
                                remotedata_checkboxs[index].options = res.content;
                            } else {
                                tips.showFail(res.msg);
                            }
                        },
                        complete: function (res) {
                        }
                    })
                })(i);
            }
        }
        var colorpickers = demojs.filter(item => item.tag == "colorpicker");
        if (colorpickers.length > 0) {
            for (var i = 0; i < colorpickers.length; i++) {
                if (model.hasOwnProperty(colorpickers[i].id))
                    colorpickers[i]["defaultValue"] = model[colorpickers[i].id];
            }
        }
        var dateRanges = demojs.filter(item => item.tag == "dateRange");
        if (dateRanges.length > 0) {
            for (var i = 0; i < dateRanges.length; i++) {
                if (model.hasOwnProperty(dateRanges[i].id))
                    dateRanges[i]["dateRangeDefaultValue"] = model[dateRanges[i].id];
            }
        }

        render = formDesigner.render({
            elem: '#view',
            data: demojs,
            formId: 'view',
            dataFormId: 'formTest',
            formDefaultButton: false,
            viewOrDesign: true,
            //formData: model
        });

        for (var i = 0; i < checkboxs.length; i++) {
            setCheckboxValue(checkboxs[i].id, model[checkboxs[i].id]);
        }
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
        var iceEditorObjects = render.geticeEditorObjects();
        var promises = [];

        for (let key in iceEditorObjects) {
            let editor = iceEditorObjects[key];
            // 使用 Promise 包装 editor.ready
            let readyPromise = new Promise(resolve => {
                editor.ready(function () {
                    resolve();
                });
            });
            promises.push(readyPromise);
        }
        // 等待所有编辑器准备好
        Promise.all(promises).then(() => {
            // 所有编辑器准备好后执行 setContent
            for (let key in iceEditorObjects) {
                let editor = iceEditorObjects[key];
                if (model[key] != null) {
                    editor.setContent(model[key]);
                }
            }
        });
        var filesData = render.getFiles();

        for (var i = 0; i < filesData.length; i++) {
            let id = filesData[i].select;
            let imageinput = "input[name=" + id + "]";
            //初始化上传工具
            var options = { single: true, autoupload: true, isImage: false, serverUrl: filesData[i].uploadUrl, valueElement: imageinput };

            ___initUpload("#uploader-show_" + id, options);
        }
        function formRender() {
            for (var i = 0; i < checkboxs.length; i++) {
                delete model[checkboxs[i].id];
            }
            form.val("formTest", model);
        }
        
        formRender();
        function collectData(data) {
            $('input[data-group=datastatus]').each(function () {
                if ($(this)[0].checked)
                    data[$(this).attr('name')] = true;
                else
                    data[$(this).attr('name')] = false;
            });
            if (checkboxs.length > 0) {
                for (var i = 0; i < checkboxs.length; i++) {
                    data[checkboxs[i].id] = getCheckboxValue(checkboxs[i].id);
                }
            }
            if (dateRanges.length > 0) {
                for (var i = 0; i < dateRanges.length; i++) {
                    data[dateRanges[i].id] = data["start" + dateRanges[i].id] + " - " + data["end" + dateRanges[i].id];
                }
            }
            delete data["editorValue_forUeditor"];
            for (let key in iceEditorObjects) {
                data[key] = iceEditorObjects[key].getContent();
            }
            data.ParentId = parent_json.ParentId;
            data.Id = parent_json.Id;
        }
    });

