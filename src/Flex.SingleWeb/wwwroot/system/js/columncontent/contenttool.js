var contentIds = $.getUrlParam('Ids');
var currentmodelId = $.getUrlParam('modelId');
var parentId = $.getUrlParam('parentId');
var currentPId = $.getUrlParam('pId');

ajaxHttp({
    url: api + 'SiteManage/ListAsync',
    type: 'Get',
    datatype: 'json',
    async: false,
    success: function (json) {
        if (json.code == 200) {
            if (json.content.length > 0) {
                $.each(json.content, function (index, item) {
                    $('#siteId').append('<option value="' + item.Id + '" ' + (item.Id == sessionStorage.getItem('siteId') ? "selected" : "") + '>' + item.SiteName + '</option>');
                })
            }
        } else {
            tips.showFail(json.msg);
        }
    }
})

layui.config({
    base: '/scripts/layui/lay/modules/'
}).use(['form', 'newtree'], function () {
    var form = layui.form, layer = layui.layer;
    var columnlist;
    var tree = layui.newtree;
    var chooseIdArray = [];
    topmodelId = sessionStorage.getItem(parentId + "_topmodelId");
    var NotChild = false;
    if (currentPId == "0")
        NotChild = true;


    tree.render({
        elem: '#parentIdhide'
        , data: columnlist
        , id: 'columntree'
        , checkChild: false
        , isJump: !NotChild
        , showCheckbox: NotChild
        , onlyIconControl: true
        , oncheck: function (obj) {
            if (obj.checked == true) {
                chooseIdArray.push(obj.data.id);
            }
            else {
                var index = chooseIdArray.indexOf(obj.data.id);
                delete chooseIdArray[index]
            }
        }
    });

    if (!NotChild) {
        $('.demo-tree-more').on('click', '.layui-tree-txt', function (e) {
            e.preventDefault();
            let linkurl = $(this).attr('href');
            let ids = $(this).attr('data-id');
            //console.log(ids)
            if (linkurl.indexOf('javascript:;') != -1 || ids == null) {
                return;
            }
            //iframe窗
            var index = layer.open({
                type: 2,
                title: $(this).text(),
                shadeClose: true,
                shade: 0.2,
                area: ['97.5%', '95%'],
                offset: ['2.5%'],
                content: SystempageRoute + "ColumnContent/Index?parentId=" + ids
                    + "&modelId=" + parent.parent.currentmodelId
                    + "&pId=0", //这里是使用parent.parent的原因是直接跳到当前子列表的父级去
                end: function (index) {

                }
            });

        });
    }
    form.on('radio(operation)', function (data) {
        chooseIdArray = [];
        if (NotChild) {
            // 得到开关的value值，实际是需要修改的ID值。
            if (data.value == "2") {
                // 重载
                tree.reload('columntree', { // options
                    data: columnlist
                    , showCheckbox: false
                    , click: function (obj) {
                        chooseIdArray = [];
                        var objelem = $(obj.elem);
                        var that = objelem.find('.layui-tree-entry');
                        if (obj.data.children) {
                            that = $(objelem.children('.layui-tree-entry').get(0));
                        }

                        if (!that.hasClass('active')) {
                            $('.layui-tree-entry').removeClass('active');
                            that.addClass("active");
                            chooseIdArray.push(obj.data.id);
                        }
                        else {
                            var index = chooseIdArray.indexOf(obj.data.id);
                            delete chooseIdArray[index]
                            that.removeClass("active");
                        }
                    }
                });
            }
            else {
                // 重载
                tree.reload('columntree', { // options
                    data: columnlist,
                    showCheckbox: NotChild
                    , click: function (obj) {
                    }
                });
            }
        }
    })
    //获取所有选中的节点id
    function getCheckedId() {
        var id = "";
        if (!chooseIdArray)
            return id;
        if (chooseIdArray.length > 0) {
            for (var i = 0; i < chooseIdArray.length; i++) {
                if (!chooseIdArray[i])
                    continue;
                if (id != "") {
                    id += '-' + chooseIdArray[i];
                }
                else {
                    id = chooseIdArray[i];
                }
            }
        }
        return id;
    }
    //监听提交
    form.on('submit(formDemo)', function (data) {
        if (!getCheckedId()) {
            layer.msg("请选择栏目", { icon: 5, time: 1000 })
            return false;
        }
        data.field["checkcolumnId"] = getCheckedId();
        data.field["checkcontentId"] = contentIds;
        data.field["parentId"] = parentId;
        data.field["operation"] = parseInt(data.field["operation"]);
        data.field["modelId"] = parseInt(currentmodelId);
        ajaxHttp({
            url: api + 'ColumnContent/ContentTools',
            type: 'Post',
            datatype: 'json',
            data: JSON.stringify(data.field),
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
        return false;
    });

    function InitColumnList(siteId) {
        chooseIdArray = []
        ajaxHttp({
            url: api + 'ColumnCategory/GetTreeListBySiteIdAsync',
            type: 'Get',
            data: { siteId: siteId, modelId: topmodelId },
            async: false,
            success: function (json) {
                if (json.code == 200) {
                    columnlist = json.content;
                }
            },
            complete: function () { }
        })
        // 重载
        tree.reload('columntree', { // options
            data: columnlist
        });
    }
    form.render();

    InitColumnList(0);
    // select 事件
    form.on('select(siteId-select)', function (data) {
        var elem = data.elem; // 获得 select 原始 DOM 对象
        var value = data.value; // 获得被选中的值
        var othis = data.othis; // 获得 select 元素被替换后的 jQuery 对象
        InitColumnList(value);
    });
})