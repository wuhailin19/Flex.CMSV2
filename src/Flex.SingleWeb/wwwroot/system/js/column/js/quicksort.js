var parent_json;

ajaxHttp({
    url: api + 'ColumnCategory/GetTreeSelectListDtos',
    type: 'Get',
    //data: { _type: 'getTreeColumnStr' },
    async: false,
    success: function (json) {
        if (json.code == 200) {

            for (var i = 0; i < json.content.length; i++) {
                $('#chooseName').append('<option value="' + json.content[i].id + '">' + json.content[i].title + '</option>');
            }
        }
    },
    complete: function () {
    }
})

layui.config({
    base: '/scripts/layui/module/'
}).use(['iconHhysFa', 'form', 'tree'], function () {
    var form = layui.form;

    function GetData(pid) {
        ajaxHttp({
            url: api + 'ColumnCategory/GetColumnSortListByParentIdAsync/' + pid,
            type: 'Get',
            //data: { _type: 'getTreeColumn' },
            async: false,
            success: function (json) {
                if (json.code == 200) {
                    var list = json.content;
                    var htmlstr = '';
                    if (list.length > 0) {
                        for (var i = 0; i < list.length; i++) {
                            htmlstr += '<div class="item" data-id="' + list[i].Id + '"><div class="columnname">' + list[i].Name + '</div><div class="sortbtn"><span class="iconfont">&#xea8d;</span></div></div>';
                        }

                    } else {
                        htmlstr += '<div class="item" data-id="0"><div class="columnname nochild">没有下级栏目</div></div>';
                    }
                    $("#cloumnList").html(htmlstr);
                }
            },
            complete: function () { }
        })
    }
    GetData(0);

    var sortableList = document.getElementById('cloumnList');
    var sortable = new Sortable(sortableList, {
        draggable: '.item',
        easing: "cubic-bezier(1, 0, 0, 1)",
        handle: ".sortbtn",
        chosenClass: "sortable-chosen",
        onEnd: function (evt) {
            var ids = '';
            $('#cloumnList .item').each(function (index, item) {
                if (ids == '')
                    ids = $(item).attr('data-id');
                else
                    ids += ',' + $(item).attr('data-id');
            })
            ajaxHttp({
                url: api + 'ColumnCategory/QuickSortColumn',
                type: 'Post',
                data: JSON.stringify({ IdString: ids }),
                async: false,
                success: function (json) {
                    if (json.code == 200) {
                        tips.showSuccess(json.msg);
                        initTree();
                    } else {
                        tips.showFail(json.msg);
                    }
                },
                complete: function () {
                }
            })
        }
    })

    var columnlist;
    var tree = layui.tree;
    initTree();
    function initTree() {
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
        tree.render({
            elem: '#ParentID',
            id: 'columntree',
            data: columnlist
            , onlyIconControl: true
            , click: function (obj) {
                $("#chooseName").val(obj.data.id);
                GetData(obj.data.id);
                form.render();
            }
        });
    }
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
})