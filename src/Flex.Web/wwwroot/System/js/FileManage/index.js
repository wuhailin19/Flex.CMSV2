layui.extend({ 'fileManager': '/Scripts/layui/module/filemanage/fileManager_src' });
layui.use(['fileManager', 'layer', 'upload'], function () {
    var fileManager = layui.fileManager
        , $ = layui.$
        , upload = layui.upload
        , layer = layui.layer;
    $('title').html($('title').html() + ' version:' + fileManager.v);
    var upIns = upload.render({
        elem: '#test1' //绑定元素
        , url: 'data.php?action=upload' //上传接口
        , field: 'file[]'
    })
    fileManager.render({
        elem: '#fileManager'
        , method: 'get'
        , id: 'fmTest'
        , btn_upload: true
        , btn_create: true
        , data: {  }
        , where: { path: "/" }
        , url: '/api/FileManage/GetFiles'
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
        layer.alert(JSON.stringify(data), {
            title: '当前数据：'
        });
    });
    //监听图片上传事件
    fileManager.on('uploadfile(test)', function (obj) {
        //obj.obj 当前对象
        //obj.path 路径
        //更改上传组件参数
        upIns.config.data = { 'path': obj.path };
        upIns.config.done = function (res) {
            fileManager.reload('fmTest');
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
        e = JSON.parse(e);
        $.post('data.php?action=folder', { 'folder': obj.folder, 'path': obj.path }, function (e) {
            layer.msg(e.msg);
            if (e.code == 1) {
                fileManager.reload('fmTest');
            }
        })
    });
});