// 获取选中节点的id
var routeLink = api + 'SiteManage';
function getChecked_list(data, type) {
    var id = "";
    $.each(data, function (index, item) {
        if (item.children.length == 0) {
            if (id != "") {
                id = id + "," + item.id;
            }
            else {
                id = item.id;
            }
        }
        var i = getChecked_list(item.children);
        if (i != "") {
            id = id + "," + i;
        }
    });
    return id;
}

var parent_json = parent.req_Data;
var currentinfo;
var sitestr = '<option value="0" selected>不复制</option>';
ajaxHttp({
    url: routeLink + '/ListAsync',
    type: 'Get',
    datatype: 'json',
    async: false,
    success: function (json) {
        if (json.code == 200) {
            currentinfo = json.content;
            for (var i = 0; i < currentinfo.length; i++) {
                sitestr += '<option value="' + currentinfo[i].Id + '" ' + (parent_json.CopySiteId == currentinfo[i].Id ? "selected" : "") + '>' + currentinfo[i].SiteName + '</option>';
            }
        } else {
            tips.showFail(json.msg);
        }
        $('#CopySiteId').html(sitestr);
    }
})


layui.config({
    base: '/scripts/layui/module/'
}).use(['form', 'tree'], function () {
    var form = layui.form;

    var model = parent_json;
    //监听提交
    form.on('submit(formDemo)', function (data) {
        var json = data.field;
        ajaxHttp({
            url: routeLink,
            type: 'Post',
            datatype: 'json',
            data: JSON.stringify(json),
            async: false,
            success: function (json) {
                if (json.code == 200) {
                    tips.showSuccess(json.msg);
                    localStorage.removeItem('siteName');
                    top.initSiteList();
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
        return false;
    });

    function formRender() {
        var formData = form.val("formTest");
        for (var key in model) {
            if (model.hasOwnProperty(key)) {
                formData[key] = model[key];
            }
        }
        form.val("formTest", formData);
    }
    formRender();
    $('#reset').click(function () {
        formRender();
        return false;
    })
});