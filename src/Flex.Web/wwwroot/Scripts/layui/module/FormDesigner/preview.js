﻿var demojs = [];
//JavaScript代码区域
layui.config(
    { base: '/scripts/layui/module/formDesigner/' }).use(['formDesigner', 'form', 'layer', 'upload'], function () {
        var layer = layui.layer;
        var $ = layui.jquery;
        var upload = layui.upload;
        var index = layui.index;
        var formDesigner = layui.formDesigner;
        var form = layui.form;
        var element = layui.element;
        var render;
        if (window.localStorage.getItem('layui_form_json') !== undefined) {
            demojs = JSON.parse(window.localStorage.getItem('layui_form_json'));
            render = formDesigner.render({
                elem: '#testdemo',
                data: demojs,
                viewOrDesign: true
                //formData: {"textarea_1":"123","input_2":"123","password_3":"123"}
            });
            var images = render.getImages();
            for (var i = 0; i < images.length; i++) {
                let id = images[i].select;
                upload.render({
                    elem: '#' + id
                    , url: '' + images[i].uploadUrl + ''
                    , multiple: true
                    , headers: httpTokenHeaders
                    , before: function (obj) {
                        layer.msg('图片上传中...', {
                            icon: 16,
                            shade: 0.01,
                            time: 0
                        })
                    }
                    , done: function (res) {
                        layer.close(layer.msg());//关闭上传提示窗口
                        //上传完毕
                        $('#uploader-list-' + id).append(
                            '<div id="" class="file-iteme">' +
                            '<div class="handle"><i class="layui-icon layui-icon-delete"></i></div>' +
                            '<img style="width: 100px;height: 100px;" src=' + res.msg + '>' +
                            '<div class="info">' + res.msg + '</div>' +
                            '</div>'
                        );
                    }
                });
            }

            var filesData = render.getFiles();

            for (var i = 0; i < filesData.length; i++) {
                upload.render({
                    elem: '#' + filesData[i].select
                    , elemList: $('#list-' + filesData[i].select) //列表元素对象
                    , url: '' + filesData[i].uploadUrl + ''
                    , accept: 'file'
                    , multiple: true
                    , number: 3
                    , auto: false
                    , bindAction: '#listAction-' + filesData[i].select
                    , choose: function (obj) {
                        var that = this;
                        var files = this.files = obj.pushFile(); //将每次选择的文件追加到文件队列
                        //读取本地文件
                        obj.preview(function (index, file, result) {
                            var tr = $(['<tr id="upload-' + index + '">'
                                , '<td>' + file.name + '</td>'
                                , '<td>' + (file.size / 1014).toFixed(1) + 'kb</td>'
                                , '<td><div class="layui-progress" lay-filter="progress-demo-' + index + '"><div class="layui-progress-bar" lay-percent=""></div></div></td>'
                                , '<td>'
                                , '<button class="layui-btn layui-btn-xs demo-reload layui-hide">重传</button>'
                                , '<button class="layui-btn layui-btn-xs layui-btn-danger demo-delete">删除</button>'
                                , '</td>'
                                , '</tr>'].join(''));

                            //单个重传
                            tr.find('.demo-reload').on('click', function () {
                                obj.upload(index, file);
                            });

                            //删除
                            tr.find('.demo-delete').on('click', function () {
                                delete files[index]; //删除对应的文件
                                tr.remove();
                                uploadListIns.config.elem.next()[0].value = ''; //清空 input file 值，以免删除后出现同名文件不可选
                            });

                            that.elemList.append(tr);
                            element.render('progress'); //渲染新加的进度条组件
                        });
                    }
                    , done: function (res, index, upload) { //成功的回调
                        var that = this;
                        //if(res.code == 0){ //上传成功
                        var tr = that.elemList.find('tr#upload-' + index)
                            , tds = tr.children();
                        tds.eq(3).html(''); //清空操作
                        delete this.files[index]; //删除文件队列已经上传成功的文件
                        return;
                        //}
                        this.error(index, upload);
                    }
                    , allDone: function (obj) { //多文件上传完毕后的状态回调
                        console.log(obj)
                    }
                    , error: function (index, upload) { //错误回调
                        var that = this;
                        var tr = that.elemList.find('tr#upload-' + index)
                            , tds = tr.children();
                        tds.eq(3).find('.demo-reload').removeClass('layui-hide'); //显示重传
                    }
                    , progress: function (n, elem, e, index) {
                        element.progress('progress-demo-' + index, n + '%'); //执行进度条。n 即为返回的进度百分比
                    }
                });
            }
        }
        //监听提交
        form.on('submit(demo1)', function (data) {
            var json = render.getFormData();
            console.log(json);
            layer.msg(JSON.stringify(json), { icon: 6 });
            /*$.ajax({
              url:"/activiti-manager/addActivitiManagera",
              type:"POST",
              data:temp,
              contentType:"application/json",
              dataType: "json",
              success:function(res){
                alert(res.message);
              }
            });*/
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
    console.log(data);
    return data;
}