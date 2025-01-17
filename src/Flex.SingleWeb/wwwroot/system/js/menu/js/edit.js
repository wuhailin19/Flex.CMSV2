﻿var parent_json = parent.req_Data;
//Demo
ajaxHttp({
    url: api + 'Menu/GetTreeMenuAsync',
    type: 'Get',
    //data: { _type: 'getTreeColumnStr' },
    async: false,
    success: function (json) {
        if (json.code == 200) {
            for (var i = 0; i < json.content.length; i++) {
                $('#chooseName').append('<option value="' + json.content[i].id + '" ' + (parent_json.ParentID == json.content[i].id ? "selected" : "") + '>' + json.content[i].title + '</option>');
            }
        }
    },
    complete: function () {

    }
})
layui.config({
    base: '/scripts/layui/module/'
}).extend({
    iconHhysFa: 'iconHhysFa'
}).use(['iconHhysFa', 'form', 'tree'], function () {
    var form = layui.form;
    var tree = layui.tree;
    var iconHhysFa = layui.iconHhysFa;
    var fonttype = parent_json.FontSort;
    
    form.on('select(FontSort)', function (data) {
        iconHhysFa.remove();
        fonttype = data.value;
        icondom = iconHhysFa.render({
            elem: '#iconHhysFa',
            type: fonttype,
            fonttype: fonttype,
            search: true,
            page: true,
            limit: 12,
            url: '/scripts/layui/css/iconfont/iconfont.css'
        });
    });
    $('#icon_click').delegate('#icon_clear', 'click', function () {
        iconHhysFa.clearValue("iconHhysFa", fonttype);
        $('#iconHhysFa').val('');
    })
    form.val("formTest", parent_json);
    var icondom = iconHhysFa.render({
        // 选择器，推荐使用input
        elem: '#iconHhysFa',
        // 数据类型：fontClass/awesome，推荐使用fontClass
        type: fonttype,
        fonttype: fonttype,
        // 是否开启搜索：true/false，默认true
        search: true,
        // 是否开启分页：true/false，默认true
        page: true,
        // 每页显示数量，默认12
        limit: 12,
        url: '/scripts/layui/css/iconfont/iconfont.css',
        // 点击回调
        click: function (data) {
        },
        // 渲染成功后的回调
        success: function (d) {

        }
    });
    iconHhysFa.checkIcon("iconHhysFa", parent_json.Icode == undefined ? '' : parent_json.Icode, fonttype);

    //监听提交
    form.on('submit(formDemo)', function (data) {

        ajaxHttp({
            url: api + 'Menu',
            type: 'Post',
            datatype: 'json',
            data: JSON.stringify(data.field),
            //data: { _type: 'UpdateMenuInfo', _data: JSON.stringify(data.field) },
            async: false,
            success: function (json) {
                if (json.code == 200) {
                    tips.showSuccess(json.msg);
                    setTimeout(function () {
                        var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
                        parent.layer.close(index); //再执行关闭
                        top.Init();
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

    var columnlist;
    ajaxHttp({
        url: api + 'Menu/TreeListAsync',
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

    var tree = layui.tree;
    tree.render({
        elem: '#ParentID',
        data: columnlist
        , onlyIconControl: true
        , click: function (obj) {
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