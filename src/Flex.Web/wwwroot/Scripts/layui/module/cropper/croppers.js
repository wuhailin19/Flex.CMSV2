/*!
 * Cropper v3.0.0
 */
var testimg;
layui.config({
    base: '/Scripts/layui/module/cropper/' //layui自定义layui组件目录
}).define(['jquery', 'layer', 'cropper'], function (exports) {
    var $ = layui.jquery
        , layer = layui.layer;
    var html = "<link rel=\"stylesheet\" href=\"/Scripts/layui/module/cropper/cropper.css\">\n" +
        "<div class=\"layui-fluid showImgEdit\" style=\"display: none\">\n" +
        "    <div class=\"layui-form-item\" style=\"padding-top:10px;\">\n" +
        "        <div class=\"layui-input-inline layui-btn-container  choose_img\" style=\"width: auto;\">\n" +
        "            <label for=\"cropper_avatarImgUpload\" class=\"layui-btn layui-btn-primary\">\n" +
        "                <i class=\"layui-icon\">&#xe67c;</i>选择图片\n" +
        "            </label>\n" +
        "            <input class=\"layui-upload-file\" id=\"cropper_avatarImgUpload\" type=\"file\" value=\"选择图片\" name=\"file\">\n" +
        "        </div>\n" +
        "        <div class=\"layui-form-mid layui-word-aux\">头像的尺寸限定150x150px,大小在50kb以内，鼠标放于非选框区域即可移动图片位置</div>\n" +
        "    </div>\n" +
        "    <div class=\"layui-row layui-col-space15\">\n" +
        "        <div class=\"layui-col-xs9\">\n" +
        "            <div class=\"readyimg\" style=\"height:450px;background-color: rgb(247, 247, 247);\">\n" +
        "                <img src=\"\" >\n" +
        "            </div>\n" +
        "        </div>\n" +
        "        <div class=\"layui-col-xs3\">\n" +
        "            <div class=\"img-preview\" style=\"width:200px;height:200px;overflow:hidden\">\n" +
        "            </div>\n" +

        "        </div>\n" +
        "    </div>\n" +
        "    <div class=\"layui-row layui-col-space15\">\n" +
        "        <div class=\"layui-col-xs9 btn_opreation\">\n" +
        "            <div class=\"layui-row\">\n" +
        "                <div class=\"layui-col-xs10\">\n" +
        "                    <button type=\"button\" class=\"layui-btn layui-icon layui-icon-left\" cropper-event=\"rotate\" data-option=\"-15\" title=\"Rotate -90 degrees\">左旋转</button>\n" +
        "                    <button type=\"button\" class=\"layui-btn layui-icon layui-icon-right\" cropper-event=\"rotate\" data-option=\"15\" title=\"Rotate 90 degrees\">右旋转</button>\n" +
        "                    <label style=\"padding-left:10px;\">当前尺寸：</label><input value=\"\" class=\"layui-input size_box\" style=\"width:100px;display:inline-block;\" id=\"currentsize\">" +
        "                </div>\n" +
        "                <div class=\"layui-col-xs2\" style=\"text-align: right;\">\n" +
        "                    <button type=\"button\" class=\"layui-btn layui-icon layui-icon-refresh refresh_btn_image\" cropper-event=\"reset\" title=\"重置图片\"></button>\n" +
        "                </div>\n" +
        "            </div>\n" +
        "        </div>\n" +
        "        <div class=\"layui-col-xs3 btn_save\">\n" +
        "            <button class=\"layui-btn layui-btn-fluid\" cropper-event=\"confirmSave\" type=\"button\"> 保存修改</button>\n" +
        "        </div>\n" +
        "    </div>\n" +
        "\n" +
        "</div>";

    //"                    <button type=\"button\" class=\"layui-btn layui-icon\" title=\"移动\"></button>\n" +
    //    "                    <button type=\"button\" class=\"layui-btn\" title=\"放大图片\"></button>\n" +
    //    "                    <button type=\"button\" class=\"layui-btn\" title=\"缩小图片\"></button>\n" +
    var obj = {
        render: function (e) {
            $('body').append(html);
            var self = this,
                elem = e.elem,
                saveW = e.saveW,
                saveH = e.saveH,
                mark = e.mark,
                area = e.area,
                readyimgurl = e.readyimgurl,
                url = e.url,
                done = e.done,
                adminid = undefined,
                boxIndex = undefined;

            var content = $('.showImgEdit')
                , image = $(".showImgEdit .readyimg img")
                , currentsize = $('#currentsize')
                , preview = '.showImgEdit .img-preview'
                , file = $(".showImgEdit input[name='file']")
                , options = {
                    aspectRatio: mark, preview: preview, viewMode: 1, toggleDragModeOnDblclick: false, dragMode: 'move'
                    , ready: function () {
                        var data = image.cropper('getData');
                        currentsize.val(Math.round(data.width) + "x" + Math.round(data.height));
                    }
                    , cropmove: function (e) {
                        var data = image.cropper('getData');
                        currentsize.val(Math.round(data.width) + "x" + Math.round(data.height));
                    }, zoom: function (e) {
                        var data = image.cropper('getData');
                        currentsize.val(Math.round(data.width) + "x" + Math.round(data.height));
                    }
                };

            $(elem).on('click', function () {
                self.adminid = $(this).attr('data-id')
                boxIndex = layer.open({
                    type: 1
                    , content: content
                    , area: area
                    , success: function () {
                        if (readyimgurl != undefined)
                            image.attr("src", readyimgurl); //图片链接
                        image.cropper(options);
                    }
                    , cancel: function (index) {
                        layer.close(index);
                        content.hide();
                        image.cropper('destroy');
                    }
                });
            });
            $(".layui-btn").on('click', function () {
                var event = $(this).attr("cropper-event");
                //监听确认保存图像
                if (event === 'confirmSave') {
                    image.cropper("getCroppedCanvas", {
                        width: saveW,
                        height: saveH
                    }).toBlob(function (blob) {
                        var formData = new FormData();
                        formData.append('file', blob, 'head.jpg');
                        formData.append('adminId', self.adminid);
                        ajaxHttp({
                            type: "post",
                            url: url, //用于文件上传的服务器端请求地址
                            data: formData,
                            processData: false,
                            contentType: false,
                            setcontentType: true,
                            dataType: 'json',
                            success: function (result) {
                                if (result.code == 200) {
                                    //layer.msg("修改成功", { icon: 1 });
                                    layer.closeAll();
                                    content.hide();
                                    return done(result.msg);
                                } else {
                                    layer.alert(result.msg, { icon: 2 });
                                }
                            }
                        });
                    });
                    //监听旋转
                } else if (event === 'rotate') {
                    var option = $(this).attr('data-option');
                    image.cropper('rotate', option);
                    //重设图片
                } else if (event === 'reset') {
                    image.cropper('reset');
                } else if (event === 'move') {
                    var option = $(this).attr('data-option');
                    if (option == 'left') {
                        image.cropper('setDragMode', 'move');
                    } else {
                        image.cropper('setDragMode', 'none');
                    }
                }
                //文件选择
                file.change(function () {
                    var r = new FileReader();
                    var f = this.files[0];
                    r.readAsDataURL(f);
                    r.onload = function (e) {
                        image.cropper('destroy').attr('src', this.result).cropper(options);
                    };
                });
            });
        }
    };
    exports('croppers', obj);
});