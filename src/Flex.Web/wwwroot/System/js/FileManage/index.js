layui.extend({ 'fileManager': '/Scripts/layui/module/filemanage/fileManager_src' });
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
    fileManager.render({
        elem: '#fileManager'
        , method: 'get'
        , id: 'fmTest'
        , btn_upload: true
        , btn_create: true
        , data: {}
        , where: { path: "/" }
        , url: api + 'FileManage/GetFiles'
        , thumb: { 'nopic': '/Scripts/layui/module/filemanage/ico/null-100x100.jpg', 'width': 100, 'height': 100 }
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
        }
        , page: false
    });
    //监听图片选择事件
    fileManager.on('pic(test)', function (obj) {
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
                area: ['90%', '90%'],
                content: "/system/FileManage/Preview?path=" + data.path
            });
        }
        else if (data.type=="mp4"){
            layer.open({
                type: 2,
                title: data.name,
                shadeClose: true,
                maxmin: true, //开启最大化最小化按钮
                area: ['90%', '90%'],
                content: data.path
            });
        }
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
                    },
                ]
            }
        });
    }
    function IsCode(type) {
        switch (type) {
            case "html": return true;
            case "css": return true;
            case "js": return true;
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
            if (res.code == 200)
                fileManager.reload('fmTest');
            else
                tips.showFail(res.msg);
        }
        var e = document.createEvent("MouseEvents");
        e.initEvent("click", true, true);
        document.getElementById("test1").dispatchEvent(e)
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