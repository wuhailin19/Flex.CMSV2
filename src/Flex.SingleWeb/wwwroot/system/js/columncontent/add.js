var demojs = [];
var parent_json = parent.req_Data;
var IsAdd = true;
ajaxHttp({
    url: api + 'ColumnContent/GetContentById/' + parent.currentmodelId +'/' + parent.currentparentId + "/0",
    type: 'Get',
    async: false,
    dataType: 'json',
    success: function (result) {
        if (!result.content.NeedReview) {
            $('#bottomBtnbox').append('<button class="layui-btn layui-btn-sm" lay-submit lay-filter="formDemo">立即提交</button>');
            $('#bottomBtnbox').append('<button class="layui-btn layui-btn-sm layui-btn-danger btn_draft" data-event="draft">存草稿</button>');
            return false;
        }
        if (result.content.stepActionButtonDto.length > 0) {
            var buttons = result.content.stepActionButtonDto;
            for (var i = 0; i < buttons.length; i++) {
                let layui_class = buttons[i].stepToCate == "end-error" ? "layui-btn-warm" : buttons[i].stepToCate == "end-cancel" ? "layui-btn-danger" : "";
                $('#bottomBtnbox').append('<button class="layui-btn layui-btn-sm reviewbutton ' + layui_class + '" data-event="' + buttons[i].stepToCate + '" onclick="return false;" data-fromid="' + buttons[i].stepFromId + '" data-toid="' + buttons[i].stepToId + '">' + buttons[i].actionName + '</button>');
            }
        }
    },
    complete: function () { }
})
var parentformData;
//JavaScript代码区域
layui.extend({
    'croppers': '../../../layui/module/cropper/croppers',
    'formDesigner': '/scripts/layui/module/formdesigner/formDesigner'
});
layui.use(['formDesigner', 'form', 'layer', 'upload', 'croppers', 'laydate'], function () {
    var layer = layui.layer;
    var $ = layui.jquery;
    var upload = layui.upload;
    var index = layui.index;
    var formDesigner = layui.formDesigner;
    var form = layui.form;
    var element = layui.element;
    var croppers = layui.croppers;
    var laydate = layui.laydate;
    var render;

    // 日期
    laydate.render({
        elem: '#AddTime',
        type: 'datetime',
    });
    // 日期
    laydate.render({
        elem: '#PublishTime',
        type: 'datetime',
    });

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
            content: SystempageRoute + 'Message/SendMsg?stepToId=' + that.attr('data-toid') + '&stepFromId=' + that.attr('data-fromid') + '&parentId=' + parent.currentparentId + '&contentId=0&modelId=' + parent.currentmodelId,
            end: function () {
                setTimeout(function () {
                    var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
                    parent.layer.close(index); //再执行关闭
                }, 100)
            }
        });
    })

    ajaxHttp({
        url: api + 'ColumnContent/GetFormHtml/' + parent.currentmodelId,
        type: 'Get',
        async: false,
        dataType: 'json',
        success: function (result) {

            demojs = JSON.parse(result.msg);
        },
        complete: function () { }
    })

    var checkboxs = demojs.filter(item => item.tag == "checkbox");
    var remotedata_checkboxs = checkboxs.filter(item => item.LocalSource == false);
    var sliders = demojs.filter(item => item.tag == "slider");

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
    var dateRanges = demojs.filter(item => item.tag == "dateRange");

    render = formDesigner.render({
        elem: '#view',
        data: demojs,
        formId: 'view',
        dataFormId: 'formTest',
        formDefaultButton: false,
        viewOrDesign: true
        //formData: {"textarea_1":"123","input_2":"123","password_3":"123"}
    });
    var multiimages = render.getmultiImages();
    for (var i = 0; i < multiimages.length; i++) {
        let id = multiimages[i].select;
        let imageinput = "#uploader-list-" + id;
        //初始化上传工具
        let options = { single: false, autoupload: true, valueElement: imageinput };

        ___initUpload("#" + id, options);

        var sortableList = document.getElementById("uploader-list-" + id);

        var sortable = new Sortable(sortableList, {
            draggable: '.uploadimg_box',
            easing: "cubic-bezier(1, 0, 0, 1)",
            handle: ".sortbtn",
            chosenClass: "sortable-chosen",
            onEnd: function (evt) {

            }
        })
    }

    var images = render.getImages();
    
    for (var i = 0; i < images.length; i++) {
        let id = images[i].select;
        let imageinput = "input[name=" + id + "]";
        //初始化上传工具
        let options = { single: true, autoupload: true, valueElement: imageinput };
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
    var filesData = render.getFiles();

    for (var i = 0; i < filesData.length; i++) {
        let id = filesData[i].select;
        let imageinput = "input[name=" + id + "]";
        //初始化上传工具
        var options = { single: true, autoupload: true, isImage: false, serverUrl: filesData[i].uploadUrl, valueElement: imageinput };

        ___initUpload("#uploader-show_" + id, options);
    }
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
        data.ModelId = parent.currentmodelId;
        data.ParentId = parent.currentparentId;
        data.PId = parent.pId;
    }
    //监听提交
    form.on('submit(formDemo)', function (data) {
        collectData(data.field);
        data.field.StatusCode = 1;
        AddContent(data.field);
        return false;
    });
    $('.btn_draft').on('click', function () {
        var data = form.val('formTest');
        collectData(data);
        data.StatusCode = 2;
        AddContent(data);
        return false;
    })
    function AddContent(data) {
        ajaxHttp({
            url: api + 'ColumnContent/CreateColumnContent',
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


function getSubmitData() {
    var data = $('#view').form[0].serialize();
    return data;
}
