jQuery(function () {
    jQuery(".w3_dd a").mouseenter(function () {
        jQuery(this).addClass("cur");
    }).mouseleave(function () {
        jQuery(this).removeClass("cur");
    })
})
function Init(mode_arr) {
    if (mode_arr == [] || mode_arr == undefined)
        mode_arr = [1, 2, 7];
    let parent;
    for (var i = 0; i < mode_arr.length; i++) {
        parent = $('.operation.mode_' + mode_arr[i] + '').parents('.layui-card').find('.layui-card-box');
        ajaxHttp({
            url: api + 'SystemIndex/getMenuShortcut',
            data: { mode: mode_arr[i] },
            dataType: 'json',
            async: false,
            success: function (res) {
                let box_str = '';
                if (!res.content)
                    return;
                for (var i = 0; i < res.content.length; i++) {
                    box_str += '<div class="boxchart layui-anim hvr-grow-shadow">'
                        + '<div class="iconbox">'
                        + (
                            res.content[i].Icode != '' ?
                                '<i class="' + (res.content[i].FontSort == 'fontClass' ? 'layui-icon' : res.content[i].FontSort) + ' leftconfig ' + res.content[i].Icode + '"></i>'
                                : '<i class="iconfont leftconfig iconwendang"></i>'
                        )
                        + '</div>'
                        + '<span class="icon_title">' + res.content[i].Name + '</span>'
                        + '<a class="addnewIframe LinkA iconfont" data-href="' + res.content[i].LinkUrl + '" data-id="' + res.content[i].ID + '" data-cite="' + res.content[i].Name + '" data-linkstatus="' + res.content[i].IsControllerUrl + '" ></a>'
                        + '</div>';
                }
                parent.html(box_str);
            }
        })
    }
}
function InitCoumnShortcut(mode_arr) {
    if (mode_arr == [] || mode_arr == undefined)
        mode_arr = [5];
    let parent;
    for (var i = 0; i < mode_arr.length; i++) {
        parent = $('.operation.mode_' + mode_arr[i] + '').parents('.layui-card').find('.layui-card-box');
        ajaxHttp({
            url: api + 'ColumnCategory/getColumnShortcut',
            data: { mode: mode_arr[i] },
            dataType: 'json',
            async: false,
            success: function (res) {
                let box_str = '';
                for (var i = 0; i < res.content.length; i++) {
                    //console.log(res.content[i].Id)
                    box_str += '<div class="boxchart layui-anim hvr-grow-shadow types">'
                        + '<div class="iconbox">'
                        + '<i class="iconfont leftconfig iconwendang"></i>'
                        + '</div>'
                        + '<span class="icon_title">' + res.content[i].title + '</span>'
                        + '<a class="addnewIframe LinkA iconfont" data-href="' + res.content[i].href + '" data-id="' + res.content[i].id + '" data-cite="' + res.content[i].title + '" data-linkstatus="ture" ></a>'
                        + '</div>';
                }
                parent.html(box_str);
            }
        })
    }
}

var element = parent.elment;
$('.layui-card-box').on('click', 'a.addnewIframe', function () {
    let $elemt = $(this);
    let id = $elemt.data('id');
    let href = $elemt.data('href');
    let text = $elemt.data('cite');
    var isaspx = $elemt.data('linkstatus');
    if (isaspx == false) {
        href = api + href;
    }
    var index = layer.open({
        type: 2,
        title: text,
        shadeClose: true,
        shade: false,
        maxmin: true, //开启最大化最小化按钮
        area: ['90%', '95%'],
        content: href,
        success: function (layero, index) {
        }
    });
})
//初始化
Init();
InitCoumnShortcut();

var add_more = {
    openindex: undefined,
    getStr: function (classname) {
        if (classname == undefined)
            classname = '';
        return "<div class=\"boxchart layui-anim hvr-grow-shadow more_box " + (classname == '6' ? "types" : "") + "\" data-mode='" + (classname == '' ? '3' : classname) + "'>"
            + "<div class=\"iconbox\">"
            + "<i class=\"iconfont leftconfig\">+</i>"
            + "</div>"
            + "<span class=\"icon_title\">添加快捷方式</span>"
            + "<a href=\"javascript:;\" class=\"LinkA\"></a>"
            + "</div>";
    },
    cancel: function () {
        let box_body = $('.layui-card-body');
        let boxchart = $('.boxchart.hvr-grow-shadow');
        boxchart.find('.boxchart_delete').remove();
        boxchart.removeClass('layui-anim-scale');
        box_body.find('.layui-card-box').find('.more_box').remove();
        $('.operation').html('<span class="iconfont iconbianji" title="编辑"></span>');
    },
    openPopup: function (mode) {
        if (mode == undefined)
            mode = 3;
        ajaxHttp({
            url: api + 'SystemIndex/getMenuShortcut',
            data: { mode: mode },
            dataType: 'json',
            type: 'get',
            async: false,
            success: function (res) {
                let box_str = '';
                for (var i = 0; i < res.content.length; i++) {
                    box_str += '<div class="boxchart layui-anim hvr-grow-shadow">'
                        + '<div class="iconbox">'
                        + (
                            res.content[i].Icode != '' ?
                                '<i class="' + (res.content[i].FontSort == 'fontClass' ? 'layui-icon' : res.content[i].FontSort) + ' leftconfig ' + res.content[i].Icode + '"></i>'
                                : '<i class="iconfont leftconfig iconwendang"></i>'
                        )
                        + '</div>'
                        + '<span class="icon_title">' + res.content[i].Name + '</span>'
                        + '<a class="addnewIframe LinkA iconfont" data-href="' + res.content[i].LinkUrl + '" data-id="' + res.content[i].ID + '" data-cite="' + res.content[i].Name + '" data-linkstatus="' + res.content[i].IsControllerUrl + '" ></a>'
                        + '</div>';
                }
                $('.Popup .layui-card-box').html(box_str);
                $('.Popup .layui-btn').attr('data-mode', mode);
            }
        })
        let contents = $('.Popup').html();
        add_more.layerOpenBox(contents);
    },
    openColumnPopup: function (mode) {
        ajaxHttp({
            url: api + 'ColumnCategory/getColumnShortcut',
            dataType: 'json',
            async: false,
            data: { mode: mode },
            type: 'get',
            success: function (res) {
                let box_str = '';
                for (var i = 0; i < res.content.length; i++) {
                    box_str += '<div class="boxchart layui-anim hvr-grow-shadow">'
                        + '<div class="iconbox">'
                        + '<i class="iconfont leftconfig iconwendang"></i>'
                        + '</div>'
                        + '<span class="icon_title">' + res.content[i].title + '</span>'
                        + '<a class="addnewIframe LinkA iconfont" data-href="' + res.content[i].href + '" data-id="' + res.content[i].id + '" data-cite="' + res.content[i].title + '" data-linkstatus="true" ></a>'
                        + '</div>';
                }
                $('.Popup .layui-card-box').html(box_str);
                $('.Popup .layui-btn').attr('data-mode', mode);
            }
        })
        let contents = $('.Popup').html();
        add_more.layerOpenBox(contents);
    }
    , layerOpenBox: function (contents) {
        //页面层
        add_more.openindex = layer.open({
            type: 1,
            title: '选择快捷方式',
            skin: 'layui-layer-rim', //加上边框
            area: ['50%', ''], //宽高
            content: contents,
            success: function () {
                $('.layui-layer-content .layui-card-box .boxchart a.LinkA').click(function () {
                    $(this).toggleClass('active layui-anim layui-anim-scalesmall-spring');
                })
            },
            end: function () {
                add_more.cancel();
            }
        });
    }, close: function () {
        layer.close(add_more.openindex);
    }
}
$('.layui-card-box').on('click', '.more_box', function () {
    let mode = $(this).data('mode');
    if (mode != '6') {
        add_more.openPopup(mode);
    } else {
        add_more.openColumnPopup(mode);
    }
})

function chooseShortCut(that) {
    let idactive = that.parent().siblings('.layui-card-body').find('.addnewIframe.active');
    let idstr = '';
    $.each(idactive, function (index, item) {
        if (idstr == '') {
            idstr = $(item).data('id');
        } else {
            idstr += ',' + $(item).data('id');
        }
    })
    let mode = that.data('mode');
    ajaxHttp({
        url: api + 'SystemIndex/Update',
        dataType: 'json',
        async: false,
        data: JSON.stringify({ mode: mode, Menu: idstr }),
        type: 'post',
        success: function (json) {
            if (json.code == 200) {
                tips.showSuccess(json.msg);
                setTimeout(function () {
                    add_more.close();
                    if (mode != "6") {
                        Init();
                    } else {
                        InitCoumnShortcut();
                    }
                }, 300)
            } else {
                tips.showFail(json.msg);
            }
        }
    })
}
var anim_scale = 'layui-anim-scale';
var bianji = 'iconbianji';
var cancel = 'iconcancel';
$('.operation').on('click', 'span', function () {
    let parent = $(this).parent();
    let parents = $(this).parents('.layui-card-header');
    let box_body = parents.siblings('.layui-card-body');
    let boxchart = box_body.find('.boxchart.hvr-grow-shadow');
    let mode = $(this).parent('.operation').data('name');
    if ($(this).hasClass(bianji)) {
        boxchart.toggleClass(anim_scale);
        boxchart.append('<div class="boxchart_delete iconfont iconguanbi"></div>');
        box_body.find('.layui-card-box').append(add_more.getStr(mode));
        parent.html('<span class="iconfont iconbaocun" title="保存"></span><span class="iconfont iconcancel" title="取消"></span>');
    } else if ($(this).hasClass(cancel)) {
        boxchart.toggleClass(anim_scale);
        boxchart.find('.boxchart_delete').remove();
        box_body.find('.layui-card-box').find('.more_box').remove();
        parent.html('<span class="iconfont iconbianji" title="编辑"></span>');
        console.log(mode)
        if (mode != "6") {
            Init();
        } else {
            InitCoumnShortcut();
        }
    } else {
        let idstr = '';
        $.each(boxchart, function (index, item) {
            if ($(item).hasClass('layui-anim-fadeout')) {
                return;
            }
            if ($(item).hasClass('more_box')) {
                return;
            }
            if (idstr == '') {
                idstr = $(item).find('.addnewIframe').attr('data-id');
            } else {
                idstr += ',' + $(item).find('.addnewIframe').attr('data-id');
            }
        })
        ajaxHttp({
            url: api + 'SystemIndex/Delete',
            dataType: 'json',
            async: false,
            data: JSON.stringify({ mode: mode, Menu: idstr }),
            type: 'post',
            success: function (json) {
                if (json.code == 200) {
                    tips.showSuccess(json.msg);
                    setTimeout(function () {
                        add_more.cancel();
                    }, 300)
                } else {
                    tips.showFail(json.msg);
                }
            }
        })
    }
})

$('.layui-card-box').on('click', '.boxchart .boxchart_delete', function () {
    $(this).parent('.boxchart').addClass('layui-anim-fadeout')
    $(this).parent('.boxchart').fadeOut();
})