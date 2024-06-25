var parent_json = {};
//Demo
var model = {};
var IsAdd = false;
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
            $('#bottomBtnbox').append('<button class="layui-btn layui-btn-sm layui-btn-danger btn_draft" data-event="draft">存草稿</button>');
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
    'formDesigner': '/scripts/layui/module/formdesigner/formDesigner'
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

    $(document).on('click', '.reviewbutton', function () {
        //console.log($(this).attr('data-id'))
        var that = $(this);
        var data = form.val('formTest');
        collectData(data)
        parentformData = data;
        //iframe窗
        layer.open({
            type: 2,
            title: that.text(),
            shadeClose: true,
            shade: 0.3,
            maxmin: true, //开启最大化最小化按钮
            area: ['80%', '80%'],
            content: SystempageRoute + 'Message/SendMsg?stepToId=' + that.attr('data-toid') + '&stepFromId=' + that.attr('data-fromid') + '&parentId=' + parent_json.ParentId + '&contentId=' + parent_json.Id,
            end: function () {
                //window.location.reload();
            }
        });
    })

    $(document).on('click', '.cancelreview', function () {
        var that = $(this);
        var data = {
            ParentId: parent_json.ParentId,
            Id: parent_json.Id,
            StatusCode: 5,
            ReviewStepId: ''
        };
        var nowurl = location.href;
        ajaxHttp({
            url: api + 'ColumnContent/CancelReview',
            type: 'Post',
            async: false,
            data: JSON.stringify(data),
            dataType: 'json',
            success: function (result) {
                if (result.code == 200)
                    tips.showSuccess(result.msg);
            },
            complete: function () { }
        })
        return false;
    })

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
    var sliders = demojs.filter(item => item.tag == "slider");
    if (sliders.length > 0) {
        for (var i = 0; i < sliders.length; i++) {
            if (model.hasOwnProperty(sliders[i].id))
                sliders[i]["defaultValue"] = model[sliders[i].id];
        }
    }
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
    
    var multiimagedata = demojs.filter(item => item.tag == "multiimage");
    if (multiimagedata.length > 0) {
        for (var i = 0; i < multiimagedata.length; i++) {
            if (model.hasOwnProperty(multiimagedata[i].id))
                multiimagedata[i]["defaultValue"] = model[multiimagedata[i].id];
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


    var multiimages = render.getmultiImages();
    for (var i = 0; i < multiimages.length; i++) {
        let id = multiimages[i].select;
        let imageinput = "#uploader-list-" + id;
        //初始化上传工具
        let options = { single: false, autoupload: true, valueElement: imageinput };

        ___initUpload("#" + id, options);
    }
    $(document).on('click', '.uploadimg_box .previewimg', function () {
        previewimg($(this).attr('data-src'));
    })
    function previewimg(imgsrc) {
        layer.photos({
            photos: {
                "title": "", // 相册标题
                "start": 0, // 初始显示的图片序号，默认 0
                "data": [   // 相册包含的图片，数组格式
                    {
                        "alt": imgsrc,
                        "src": imgsrc, // 原图地址
                        "thumb": imgsrc // 缩略图地址
                    }
                ],
                error: function () {

                }
            }
        });
    }
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

    var multiimages = render.getmultiImages();
    for (var i = 0; i < multiimages.length; i++) {
        let id = multiimages[i].select;
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
        if (sliders.length > 0) {
            for (var i = 0; i < sliders.length; i++) {
                data[sliders[i].id] = sliders[i].defaultValue;
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
        if (multiimages.length > 0) {
            for (var i = 0; i < multiimages.length; i++) {
                let currentimglist = [];
                $('#uploader-list-' + multiimages[i].id + ' .uploadimg_box').each(
                    function (index, item) {
                        let imgsrc = $(item).find('img.uploadimg_item').attr('src');
                        let title = $(item).find('input.layui-input').val();
                        let content = $(item).find('textarea.layui-textarea').val();
                        currentimglist.push(
                            {
                                title: title,
                                imgsrc: imgsrc,
                                content: content
                            }
                        );
                    }
                )
                data[multiimages[i].id] = currentimglist;
            }
        }
    }

    //监听提交
    form.on('submit(formDemo)', function (data) {
        collectData(data.field);
        data.field.StatusCode = 1;
        updateContent(data.field);
        return false;
    });

    $('.btn_draft').on('click', function () {
        var data = form.val('formTest');
        collectData(data);
        data.StatusCode = 2;
        updateContent(data);
        return false;
    })
    function updateContent(data) {
        ajaxHttp({
            url: api + 'ColumnContent',
            type: 'Post',
            data: JSON.stringify(data),
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
    }
    $('#globalDisable').on('click', function () {
        render.globalDisable();
    });
    $('#globalNoDisable').on('click', function () {
        render.globalNoDisable();
    });
});

