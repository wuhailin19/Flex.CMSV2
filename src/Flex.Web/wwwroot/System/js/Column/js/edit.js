var Id = parent.req_Data.Id;
var parent_json;
var click_count = 0;
$('#uploader-show').on('click', function () {
    if ($('.uploader-box').hasClass('active')) {
        $('.uploader-box').removeClass('active');
    }
    else {
        $('.uploader-box').addClass('active');
        if (click_count == 0) { ___initUpload(); click_count++; }
    }
})
ajaxHttp({
    url: api + 'ColumnCategory/GetColumnById/' + Id,
    type: 'Get',
    datatype: 'json',
    async: false,
    success: function (json) {
        if (json.code == 200) {
            parent_json = json.content;
        } else {
            layer.msg(json.msg, { icon: 5, time: 1000 });
        }
    }
})
ajaxHttp({
    url: api + 'ContentModel/GetSelectItem',
    type: 'Get',
    datatype: 'json',
    async: false,
    success: function (json) {
        if (json.code == 200) {
            for (var i = 0; i < json.content.length; i++) {
                $('#ModelId').append('<option value="' + json.content[i].Id + '" ' + (parent_json.ModelId == json.content[i].Id ? "selected" : "") + '>' + json.content[i].Name + '</option>');
            }
        } else {
            layer.msg(json.msg, { icon: 5, time: 1000 });
        }
    }
})
//Demo
ajaxHttp({
    url: api + 'ColumnCategory/GetTreeSelectListDtos',
    type: 'Get',
    //data: { _type: 'getTreeColumnStr' },
    async: false,
    success: function (json) {
        if (json.code == 200) {

            for (var i = 0; i < json.content.length; i++) {
                $('#chooseName').append('<option value="' + json.content[i].id + '" ' + (parent_json.ParentId == json.content[i].id ? "selected" : "") + '>' + json.content[i].title + '</option>');
            }
        }
    },
    complete: function () {
    }
})
layui.config({
    base: '/Scripts/layui/module/cropper/' //layui自定义layui组件目录
}).use('croppers', function () {
    var croppers = layui.croppers;

    //创建一个头像上传组件
    croppers.render({
        elem: '#cropper-btn'
        , saveW: 1920     //保存宽度
        , saveH: 650
        , mark: 1 / 1    //选取比例
        , area: ['90%', '95%']  //弹窗宽度
        , url: api + 'Admin/OnloadUserAvatar'  //图片上传接口返回和（layui 的upload 模块）返回的JOSN一样
        , done: function (data) { //上传完毕回调
            $("input[name=ColumnImage]").val(data);
            //$("#srcimgurl").attr('src', data);
        }
    });
})
layui.config({
    base: '/Scripts/layui/module/'
}).use(['iconHhysFa', 'form', 'tree'], function () {
    var form = layui.form;
    var tree = layui.tree;

    function formRender() {
        form.val("formTest", {
            'Name': parent_json.Name
            , 'Id': parent_json.Id
            , 'ParentID': parent_json.ParentId
            , 'ModelId': parent_json.ModelId
            , 'ColumnImage': parent_json.ColumnImage
            , 'ColumnUrl': parent_json.ColumnUrl
            , 'SeoTitle': parent_json.SeoTitle
            , 'SeoKeyWord': parent_json.SeoKeyWord
            , 'SeoDescription': parent_json.SeoDescription
        });
    }
    formRender();

    var columnlist;
    ajaxHttp({
        url: api + 'ColumnCategory/TreeListAsync',
        type: 'Get',
        //data: { _type: 'getTreeColumn' },
        async: false,
        success: function (json) {
            if (json.code == 200) {
                columnlist = json.content;
            }
        },
        complete: function () { }
    })
    //监听提交
    form.on('submit(formDemo)', function (data) {
        ajaxHttp({
            url: api + 'ColumnCategory',
            type: 'Post',
            datatype: 'json',
            data: JSON.stringify(data.field),
            //data: { _type: 'UpdateMenuInfo', _data: JSON.stringify(data.field) },
            async: false,
            success: function (json) {
                if (json.code == 200) {
                    layer.msg(json.msg, { icon: 6, time: 300 }, function () {
                        var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
                        parent.layer.close(index); //再执行关闭
                        //parent.parent.Init();
                    });
                } else {
                    layer.msg(json.msg, { icon: 5, time: 1000 });
                }

            },
            complete: function () {

            }
        })
        return false;
    });

    $('#reset').click(function () {
        formRender();
        return false;
    })

    var tree = layui.tree;
    tree.render({
        elem: '#ParentID',
        data: columnlist
        , onlyIconControl: true
        , click: function (obj) {
            if (obj.data.id == parent_json.Id) {
                layer.msg("不能选择自身", { icon: 5, time: 1000 });
                return;
            }
            $("#chooseName").val(obj.data.id);
            form.render();
        }
    });
    //下拉交互显示
    $(".downpanel").on("click", ".layui-select-title", function (e) {
        $(".downpanel").not($(this).parents(".downpanel")).removeClass("layui-form-selected");
        $(this).parents(".downpanel").toggleClass("layui-form-selected");
        layui.stope(e);
    }).on("click", "dl i", function (e) {
        layui.stope(e);
    });
    $(document).on("click", function (e) {
        $(".downpanel").removeClass("layui-form-selected");
    });
});