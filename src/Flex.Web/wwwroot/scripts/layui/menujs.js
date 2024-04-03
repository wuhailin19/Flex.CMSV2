$('.flexible').on('click', function () {
    $('.layui-body,.layui-layout-admin .layui-side,.layui-tab-title').toggleClass('active');
    $(this).find('i').toggleClass('layui-icon-spread-left');

})

var tips_index;
$('.layui-layout-admin').on('mouseenter', '.layui-side.active .layui-nav-item i', function () {
    var name = $(this).siblings('cite').text();
    tips_index = layer.tips(name, $(this), { time: 50000 });
})
$('.layui-layout-admin').on('mouseleave', '.layui-side.active .layui-nav-item i', function () {
    layer.close(tips_index);
})
function removeArray(arr, val) {
    for (var i = 0; i < arr.length; i++) {
        if (arr[i] == val) {
            arr.splice(i, 1);
            break;
        }
    }
}
layui.config({
    base: '/scripts/layui/module/',
}).use(['element', 'tabrightmenu'], function () {
    let element = layui.element;
    let rightmenu_ = layui.tabrightmenu;

    // 默认方式渲染全部：关闭当前（closethis）、关闭所有（closeall）、关闭其它（closeothers）、关闭左侧所有（closeleft）、关闭右侧所有（closeright）、刷新当前页（refresh）
    rightmenu_.render({
        container: '#nav1',
        filter: 'main_content',
        navArr: [
            { eventName: 'refresh', title: '刷新当前页' },
            { eventName: 'closethis', title: '关闭当前页' },
            { eventName: 'closeall', title: '关闭所有页' },
            { eventName: 'closeothers', title: '关闭其它页' }
        ],
        isClickMidCloseTab: true
    });
});

layui.use('element', function () {
    var element = layui.element;
    //获取hash来切换选项卡，假设当前地址的hash为lay-id对应的值
    var layid = location.hash.replace(/^#main_content=/, '');
    element.tabChange('main_content', layid);
    //监听Tab切换，以改变地址hash值
    element.on('tab(main_content)', function () {
        location.hash = 'main_content=' + this.getAttribute('lay-id');
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
    $('.addnewIframe a').click(function () {
        let $elemt = $(this);
        if ($elemt.data('type') == 'addtype') {
            let id = $elemt.data('id');
            let href = $elemt.data('href');
            let text = $elemt.data('cite');
            var isaspx = $elemt.data('linkstatus');
            //if (isaspx == false) {
            //    href = api + href;
            //}
            if (items.indexOf(id) == -1) {
                items[index] = id;
                index++;//避免值被覆盖
                element.tabAdd("main_content", {
                    title: text,
                    id: id,
                    content: '<div class="loadings">' +
                        '<i class="layui-icon layui-anim layui-anim-loop">&#xe63d;</i>' +
                        '<iframe src="' + href + '" class="layadmin-iframe" id="iframe' + id + '" frameborder="0"></iframe>' +
                        '</div>'
                });
                element.tabChange("main_content", id);
            } else {
                element.tabChange("main_content", id);
            }
        }
    })
    element.on('tabAdd(main_content)', function () {

    });
    ////点击主页
    //$('.first-tab-title').on('click', function () {
    //    element.tabChange('main_content', '0_0_0');
    //    var src = $(".layui-tab-item.layui-show").find("iframe").attr("src");
    //    $(".layui-tab-item.layui-show").find("iframe").attr("src", src);
    //})
    //刷新当前选项卡
    $('.refresh').on('click', function () {
        var content = $(".layui-tab-item.layui-show").html();
        $(".layui-tab-item.layui-show").html(content);
    })
    var msgindex;
    $('.rabbit_email').click(function () {
        layer.close(msgindex);
        msgindex=layer.open({
            type: 2,
            title: "消息",
            shadeClose: true,
            shade: false,
            maxmin: true, //开启最大化最小化按钮
            area: ['60%', '80%'],
            content: SystempageRoute + "Message/Index",
            success: function (layero, index) {
            }
        });
        $('.messagebox').hide();
    })
    var items = new Array();
    items[0] = undefined;
    var index = 1;
    //以上为全局变量 以下为模块加载里面的内容

    //nav 导航栏专用 menu 为lay-filter="nenu"；
    element.on("nav(menu)", function (elem) {
        //console.log(elem.attr("name")); //得到当前点击的DOM对象
        var name = elem.attr("data-direction");
        var id = name;//因为加载外部页面name肯定不一样，所以在这id也唯一
        var href = elem.attr("data-href");//传入要加载的页面名称
        var text = elem.find('cite').text();//获取点击导航栏一列的内容
        var isaspx = elem.data('linkstatus');
        //if (isaspx == false) {
        //    href = api + href;
        //}
        //indexOf()方法 如果存在目标返回下标 默认为0，不存在返回-1
        //因为要实现同一个页面不多次添加，而是如果存在直接切换tab
        if (items.indexOf(id) == -1) {
            items[index] = id;
            index++;//避免值被覆盖
            element.tabAdd("main_content", {
                title: text,
                id: id,
                content: '<div class="loadings">' +
                    '<i class="layui-icon layui-anim layui-anim-loop">&#xe63d;</i>' +
                    '<iframe src="' + href + '" class="layadmin-iframe" id="iframe' + id + '" frameborder="0"></iframe>' +
                    '</div>'
            });
            element.tabChange("main_content", id);
        } else {
            element.tabChange("main_content", id);
        }
    });
    //监听tab
    element.on('tabDelete(main_content)', function (data) {
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
        index--;
    });
});
