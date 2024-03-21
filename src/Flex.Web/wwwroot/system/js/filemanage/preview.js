require.config({ paths: { 'vs': '/scripts/core/vs' } });
require(['vs/editor/editor.main'], function () {
    var path = $.getUrlParam('path');
    var extension = getFileExtension(path);
    // 添加语言支持
    var monacoeditor = monaco.editor.create(document.getElementById('editor'), {
        language: extension,
        locale: 'zh-cn',
        theme: 'vs'// vs, hc-black, or vs-dark
    });
    $.ajax({
        url: path,
        type: 'GET',
        async: false,
        dataType: 'html',
        success: function (res) {
            //$('.code-demo').html("<textarea>" + res + "</textarea>");
            monacoeditor.getModel().setValue(res);
        }
    })
    $('.button_box').on('click', function () {
        var contents = monacoeditor.getModel().getValue();
        ajaxHttp({
            url: api + 'FileManage/ChangeFileContent',
            type: 'post',
            data: JSON.stringify({ path: path, content: contents }),
            success: function (res) {
                if (res.code == 200) 
                    tips.showSuccess(res.msg);
                else
                    tips.showFail(res.msg);
            }
        })
    })
    function getFileExtension(path) {
        path = path.toLowerCase();
        if (path.indexOf('.css.map') > -1) {
            return 'json';
        }
        if (path.indexOf('.css') > -1) {
            return 'css';
        }
        if (path.indexOf('.json') > -1) {
            return 'json';
        }
        if (path.indexOf('.map') > -1) {
            return 'json';
        }
        if (path.indexOf('.js') > -1) {
            return 'javascript';
        }
        if (path.indexOf('.html') > -1) {
            return 'html';
        }
        return 'javascript';
    }
});
//layui.use(function () {
//    var path = $.getUrlParam('path');
//    $.ajax({
//        url: path,
//        type: 'GET',
//        async: false,
//        success: function (res) {
//            console.log(monacoeditor)
//            //$('.code-demo').html("<textarea>" + res + "</textarea>");
//            monacoeditor.getModel().setValue(res);
//        }
//    })
//})