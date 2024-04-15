layui.extend({ 'fileManager': '/scripts/layui/module/filemanage/fileManager_src' });
layui.use(['fileManager', 'layer', 'upload'], function () {
    var fileManager = layui.fileManager
        , $ = layui.$
        , upload = layui.upload
        , layer = layui.layer;

    $('title').html($('title').html() + ' version:' + fileManager.v);
    var upIns = upload.render({
        elem: '#test1' //绑定元素
        , url: api + 'Upload/UploadFilesToPath' //上传接口
        , headers: httpTokenHeaders
        , accept: 'file'
        , exts: 'jpg|png|gif|bmp|jpeg|svg|css|js|html|txt|less'
        , field: 'file'
    })
    var realteditem, currentitem;
    fileManager.render({
        elem: '#fileManager'
        , method: 'get'
        , id: 'fmTest'
        , btn_upload: true
        , btn_create: true
        , data: {}
        , where: { path: "/" }
        , url: api + 'FileManage/GetFiles'
        , thumb: { 'nopic': '/scripts/layui/module/filemanage/ico/null-100x100.jpg', 'width': 100, 'height': 100 }
        , parseData: function (res) {
            /*
            data:[{
                thumb:文件地址用于显示
                ,type:文件类型  directory文件夹,png|gif|png|image图片,其它任意
                ,path:文件夹路径用于打开本文件夹
            }]
            */
            let _res = [];
            _res.code = 0;
            _res.data = res.content;
            _res.count = res.count
            return _res;
        }
        , done: function (res, curr, count) {
            // console.log(res,curr,count)
            var sortableList = document.getElementById('picmanager');
            var sortable = new Sortable(sortableList, {
                onStart: function (evt) {
                    currentitem = undefined;
                    realteditem = undefined;
                },
                onMove: function (evt) {
                    // Check if the mouse is over the target element
                    if ($(evt.dragged).data('index') == "-1")
                        return false;
                    if ($(evt.related).data('type') != 'DIR') {
                        currentitem = undefined;
                        realteditem = undefined;

                        return false;
                    }
                    $(evt.related).addClass('targetactive').siblings().removeClass('targetactive');
                    currentitem = $(evt.dragged);
                    realteditem = $(evt.related);


                    return false;
                },
                onEnd: function (evt) {
                    let items = sortableList.querySelectorAll('li');
                    items.forEach(function (item) {
                        item.classList.remove('targetactive');
                    });
                    if (realteditem == undefined) {
                        return false;
                    }


                    let data = fileManager.cache["fmTest"];

                    let relateindex = realteditem.data('index');
                    let currentindex = currentitem.data('index');
                    let newpath;
                    let currentdata = data[currentindex];
                    if (relateindex == "-1")
                        newpath = fileManager.dirRoot[(fileManager.dirRoot.length - 2)].path;
                    else {
                        let reladtedata = data[relateindex];
                        newpath = reladtedata.path;
                    }
                    let requestdata = {
                        oldpath: currentdata.path, newpath: newpath, name: currentdata.name, type: currentdata.type, Isoverride: false
                    }
                    if (currentdata.type == "directory") {
                        layer.confirm("确定移动文件夹吗", { icon: 3 }, function (index) {
                            layer.close(index)
                            excutechangedir(requestdata);
                        }, function () {

                        });
                    } else { excutechangedir(requestdata); }
                }
            });
        }
        , page: false
    });
    function excutechangedir(requestdata) {
        ajaxHttp({
            url: api + 'FileManage/ChangeDirectory',
            type: 'post',
            data: JSON.stringify(requestdata),
            success: function (res) {
                if (res.code == 200) {
                    fileManager.reload('fmTest');
                }
                else if (res.code == 226) {
                    layer.confirm(res.msg, { icon: 3 }, function (index) {
                        requestdata.Isoverride = true;
                        layer.close(index)
                        excutechangedir(requestdata);
                    }, function () {

                    });
                }
                else
                    tips.showFail(res.msg);
            }
        })
    }
    //监听图片选择事件
    fileManager.on('pic(test)', function (obj) {
        //obj.obj 当前对象
        var data = obj.data;
    });
    //监听文件夹选择事件
    fileManager.on('dir(test)', function (obj) {
        //obj.obj 当前对象
        var data = obj.data;
    });
    //监听文件双击事件
    fileManager.on('picdb(test)', function (obj) {
        //obj.obj 当前对象
        //obj.data 当前图片数据
        var data = obj.data;

        if (IsImage(data.type))
            OpenImage(data);
        else if (IsCode(data.type)) {
            layer.open({
                type: 2,
                title: data.name,
                shadeClose: true,
                maxmin: true, //开启最大化最小化按钮
                area: ['100%', '100%'],
                content: SystempageRoute + "FileManage/Preview?path=" + data.path
            });
        }
        else if (data.type == "mp4") {
            layer.open({
                type: 2,
                title: data.name,
                shadeClose: true,
                maxmin: true, //开启最大化最小化按钮
                area: ['90%', '90%'],
                content: data.path
            });
        }
        return false;
    });
    function OpenImage(data) {
        layer.photos({
            photos: {
                "title": "", // 相册标题
                "start": 0, // 初始显示的图片序号，默认 0
                "data": [   // 相册包含的图片，数组格式
                    {
                        "alt": data.name,
                        "src": data.path, // 原图地址
                        "thumb": data.path // 缩略图地址
                    }
                ],
                error: function () {

                }
            }
        });
    }
    function IsCode(type) {
        switch (type) {
            case "html": return true;
            case "css": return true;
            case "less": return true;
            case "js": return true;
            case "json": return true;
            case "map": return true;
            default: return false;
        }
    }
    function IsImage(type) {
        switch (type) {
            case "png": return true;
            case "gif": return true;
            case "jpg": return true;
            case "jpeg": return true;
            case "ico": return true;
            case "svg": return true;
            case "image": return true;
            default: return false;
        }
    }
    //监听图片上传事件
    fileManager.on('uploadfile(test)', function (obj) {
        //obj.obj 当前对象
        //obj.path 路径
        //更改上传组件参数
        upIns.config.data = { 'path': obj.path };
        upIns.config.done = function (res) {
            if (res.code == 200) {
                fileManager.reload('fmTest');
            }
            else
                tips.showFail(res.msg);
        }
        var e = document.createEvent("MouseEvents");
        e.initEvent("click", true, true);
        document.getElementById("test1").dispatchEvent(e)
        return false;
    });
    //监听重命名文件夹事件
    fileManager.on('rename_dir(test)', function (obj) {
        //obj.obj 当前对象
        //obj.folder 文件夹名称
        //obj.path 路径
        //e = JSON.parse(e);
        ajaxHttp({
            url: api + 'FileManage/RenameDirectoryorFile',
            type: 'post',
            data: JSON.stringify({ folder: obj.path, path: obj.data.path, newName: obj.folder, type: obj.data.type }),
            success: function (res) {
                if (res.code == 200) {
                    fileManager.reload('fmTest');
                }
                else
                    tips.showFail(res.msg);
            }
        })
    });
    //监听新建文件夹事件
    fileManager.on('new_dir(test)', function (obj) {
        //obj.obj 当前对象
        //obj.folder 文件夹名称
        //obj.path 路径
        //e = JSON.parse(e);
        ajaxHttp({
            url: api + 'FileManage/CreateDirectory',
            type: 'post',
            data: JSON.stringify({ folder: obj.folder, path: obj.path }),
            success: function (res) {
                if (res.code == 200) {
                    fileManager.reload('fmTest');
                }
                else
                    tips.showFail(res.msg);
            }
        })
    });
});