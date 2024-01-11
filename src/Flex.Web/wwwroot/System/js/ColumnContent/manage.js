$('.slide-box').click(function () {
    $(this).toggleClass('layui-icon-left layui-icon-right', 500);
});

function getTailParameterFromUrl(urllink) {
    if (urllink.indexOf('/') > -1) {
        let links = urllink.split('/');
        let lastSegment = links[links.length - 1];

        return lastSegment;
    }
    return null; // 如果没有尾部参数，则返回 null 或其他适当的值
}
layui.config({
    base: '/Scripts/layui/module/',
}).use(['element', 'tabrightmenu'], function () {
    let element = layui.element;
    let rightmenu_ = layui.tabrightmenu;

    // 默认方式渲染全部：关闭当前（closethis）、关闭所有（closeall）、关闭其它（closeothers）、关闭左侧所有（closeleft）、关闭右侧所有（closeright）、刷新当前页（refresh）
    rightmenu_.render({
        container: '#nav2',
        filter: 'main_contents',
        navArr: [
            { eventName: 'refresh', title: '刷新当前页' },
            { eventName: 'closethis', title: '关闭当前页' },
            { eventName: 'closeall', title: '关闭所有页' },
            { eventName: 'closeothers', title: '关闭其它页' }
        ],
        isClickMidCloseTab: true
    });
});
layui.use(['tree', 'util'], function () {
    var tree = layui.tree
        , layer = layui.layer
        , util = layui.util
        //模拟数据
        , columnlist = [];
    ajaxHttp({
        url: api + 'ColumnCategory/GetManageTreeListAsync',
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
    //基本演示
    tree.render({
        elem: '#test12'
        , data: columnlist
        , showCheckbox: false  //是否显示复选框
        , id: 'demoId1'
        , icon: 'layui-icon layui-icon-file'
        , isJump: true //是否允许点击节点时弹出新窗口跳转
        , onlyIconControl: true
        , showLine: false
        , click: function (obj) {
            //var data = obj.data;  //获取当前点击的节点数据
            //layer.msg('状态：' + obj.state + '<br>节点数据：' + JSON.stringify(data));
        }, operate: function () {
            $('.demo-tree-more a').each(function () {
                //alert($(this).attr('target'))
                $(this).attr('target', 'right');
            })
        }
    });
})
var items = new Array();
var index = 0;
function isMobile() {
    return window.matchMedia('(max-width: 767px)').matches;
}
layui.use('element', function () {
    var element = layui.element;
    $('.demo-tree-more').on('click', '.layui-tree-txt', function (e) {
        e.preventDefault();
        let linkurl = $(this).attr('href');
        let ids = getTailParameterFromUrl(linkurl);
        //console.log(ids)
        if (linkurl.indexOf('javascript:;') != -1 || ids == null) {
            return;
        }
        if (isMobile()) {
            //iframe窗
            layer.open({
                type: 2,
                title: $(this).text(),
                shadeClose: true,
                shade: false,
                maxmin: true, //开启最大化最小化按钮
                area: ['80%','90%'],
                content: linkurl,
                end: function () {
                    
                }
            });
            return;
        }
        if (items.indexOf(ids) == -1) {
            items[index] = ids;
            index++;//避免值被覆盖
            element.tabAdd("main_contents", {
                title: $(this).text(),
                id: ids,
                content: '<div class="loadings">' +
                    '<i class="layui-icon layui-anim layui-anim-loop">&#xe63d;</i>' +
                    '<iframe src="' + linkurl + '" class="layadmin-iframe" id="iframe' + ids + '" frameborder="0"></iframe>' +
                    '</div>'
            });
            element.tabChange("main_contents", ids);
        } else {
            element.tabChange("main_contents", ids);
        }
    });
    //监听Tab切换，以改变地址hash值
    element.on('tab(main_contents)', function () {
        let iframe = document.getElementById('iframe' + this.getAttribute('lay-id'));
        if (iframe != undefined) {
            if (iframe.attachEvent) {
                iframe.attachEvent("onload", function () {
                    $('.loadings .layui-anim-loop').hide();
                });
            } else {
                iframe.onload = function () {
                    $('.loadings .layui-anim-loop').hide();
                };
            }
        }
    });
    //监听tab
    element.on('tabDelete(main_contents)', function (data) {
        var ind = data.index;
        var newArray = new Array();
        for (var i = 0; i < items.length; i++) {
            if (i < ind) {
                newArray[i] = items[i];
            } else if (i > ind) {
                newArray[i - 1] = items[i];
            }
        }
        items = newArray;
        console.log(items)
        index--;
    });
})