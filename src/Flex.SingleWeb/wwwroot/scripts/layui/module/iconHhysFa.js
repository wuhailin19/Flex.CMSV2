﻿/**
 * iconHhysFa 1.0 字体图标选择
 * User: jackhhy
 * Date: 2020/06/23-11:09
 * Link: https://gitee.com/luckygyl/iconFonts
 */
layui.define(['laypage', 'form'], function (exports) {
    "use strict";

    var IconHhys = function () {
        this.v = '1.0';
    }, _MOD = 'iconHhysFa',
        _this = this,
        $ = layui.jquery,
        laypage = layui.laypage,
        form = layui.form,
        BODY = 'body',
        TIPS = '请选择图标';

    var thistmp;
    /**
     * 渲染组件
     */
    IconHhys.prototype.render = function (options) {
        var opts = options,
            // DOM选择器
            elem = opts.elem,
            // 数据类型：fontClass/awesome
            type = opts.type == null ? 'fontClass' : opts.type,
            fonttype = opts.fonttype == null||opts.fonttype == '' ? opts.type : opts.fonttype,
            //当数据类型为awesome 的时候 需要配置 url
            url = opts.url,
            // 是否分页：true/false
            page = opts.page == null ? true : opts.page,
            // 每页显示数量
            limit = opts.limit == null ? 12 : opts.limit,
            // 是否开启搜索：true/false
            search = opts.search == null ? true : opts.search,
            // 每个图标格子的宽度：'43px'或'20%'
            cellWidth = opts.cellWidth == null ? '20%' : opts.cellWidth,
            // 点击回调
            click = opts.click,
            // 渲染成功后的回调
            success = opts.success,
            // json数据
            data = {},
            // 唯一标识
            tmp = new Date().getTime(),
            // 初始化时input的值
            ORIGINAL_ELEM_VALUE = $(elem).val(),
            TITLE = 'layui-select-title',
            TITLE_ID = 'layui-select-title-' + tmp,
            ICON_BODY = 'layui-iconpicker-' + tmp,
            PICKER_BODY = 'layui-iconpicker-body-' + tmp,
            PAGE_ID = 'layui-iconpicker-page-' + tmp,
            LIST_BOX = 'layui-iconpicker-list-box',
            selected = 'layui-form-selected',
            unselect = 'layui-unselect';
        thistmp = TITLE_ID;
        var a = {
            init: function () {
                if (type.indexOf("layui") > -1) {
                    data = common.getfont["fontClass"]();
                } else {
                    data = common.getfont[fonttype]();
                }
                a.hideElem().createSelect().createBody().toggleSelect();
                a.preventEvent().inputListen();
                common.loadCss();
                if (success) {
                    success(this.successHandle());
                }
                return a;
            },
            typeof: function (obj) {
                return obj == null ?
                    String(obj) :
                    class2type[toString.call(obj)] || "object";
            },
            successHandle: function () {
                var d = {
                    options: opts,
                    data: data,
                    id: tmp,
                    elem: $('#' + ICON_BODY)
                };
                return d;
            },
            /**
             * 隐藏elem
             */
            hideElem: function () {
                $(elem).hide();
                return a;
            },

            /**
             * 绘制select下拉选择框
             */
            createSelect: function () {
                if (type.indexOf("fontClass") > -1) {
                    var oriIcon = '<i class="layui-icon">';

                    // 默认图标
                    if (ORIGINAL_ELEM_VALUE === '') {
                        ORIGINAL_ELEM_VALUE = 'layui-icon-circle-dot';
                    }
                    oriIcon = '<i class="layui-icon ' + ORIGINAL_ELEM_VALUE + '">';
                    oriIcon += '</i>';

                    var selectHtml = '<div class="layui-iconpicker layui-unselect layui-form-select" id="' + ICON_BODY + '">' +
                        '<div class="' + TITLE + '" id="' + TITLE_ID + '">' +
                        '<div class="layui-iconpicker-item">' +
                        '<span class="layui-iconpicker-icon layui-unselect">' +
                        oriIcon +
                        '</span>' +
                        '<i class="layui-edge"></i>' +
                        '</div>' +
                        '</div>' +
                        '<div class="layui-anim layui-anim-upbit" style="">' +
                        '123' +
                        '</div>';
                    $(elem).after(selectHtml);
                    return a;
                } else {
                    var oriIcon = '<i class="' + fonttype + '">';
                    // 默认图标
                    if (ORIGINAL_ELEM_VALUE === '') {
                        ORIGINAL_ELEM_VALUE = 'fa-adjust';
                    }
                    oriIcon = '<i class="' + fonttype + ' ' + ORIGINAL_ELEM_VALUE + '">';
                    oriIcon += '</i>';

                    var selectHtml = '<div class="layui-iconpicker layui-unselect layui-form-select" id="' + ICON_BODY + '">' +
                        '<div class="' + TITLE + '" id="' + TITLE_ID + '">' +
                        '<div class="layui-iconpicker-item">' +
                        '<span class="layui-iconpicker-icon layui-unselect">' +
                        oriIcon +
                        '</span>' +
                        '<i class="layui-edge"></i>' +
                        '</div>' +
                        '</div>' +
                        '<div class="layui-anim layui-anim-upbit" style="">' +
                        '123' +
                        '</div>';
                    $(elem).after(selectHtml);
                    return a;
                }

            },

            /**
             * 展开/折叠下拉框
             */
            toggleSelect: function () {
                var item = '#' + TITLE_ID + ' .layui-iconpicker-item,#' + TITLE_ID + ' .layui-iconpicker-item .layui-edge';
                a.event('click', item, function (e) {
                    var $icon = $('#' + ICON_BODY);
                    if ($icon.hasClass(selected)) {
                        $icon.removeClass(selected).addClass(unselect);
                    } else {
                        // 隐藏其他picker
                        $('.layui-form-select').removeClass(selected);
                        // 显示当前picker
                        $icon.addClass(selected).removeClass(unselect);
                    }
                    e.stopPropagation();
                });
                return a;

            },

            /**
             * 绘制主体部分
             */
            createBody: function () {
                // 获取数据
                var searchHtml = '';

                if (search) {
                    searchHtml = '<div class="layui-iconpicker-search">' +
                        '<input class="layui-input">' +
                        '<i class="layui-icon">&#xe615;</i>' +
                        '</div>';
                }
                // 组合dom
                var bodyHtml = '<div class="layui-iconpicker-body" id="' + PICKER_BODY + '">' +
                    searchHtml +
                    '<div class="' + LIST_BOX + '"></div> ' +
                    '</div>';
                $('#' + ICON_BODY).find('.layui-anim').eq(0).html(bodyHtml);
                a.search().createList().check().page();
                return a;
            },


            /**
             * 绘制图标列表
             * @param text 模糊查询关键字
             * @returns {string}
             */
            createList: function (text) {
                var d = data,
                    l = d.length,
                    pageHtml = '',
                    listHtml = $('<div class="layui-iconpicker-list">')//'<div class="layui-iconpicker-list">';

                // 计算分页数据
                var _limit = limit, // 每页显示数量
                    _pages = l % _limit === 0 ? l / _limit : parseInt(l / _limit + 1), // 总计多少页
                    _id = PAGE_ID;

                // 图标列表
                var icons = [];

                for (var i = 0; i < l; i++) {
                    var obj = d[i];

                    // 判断是否模糊查询
                    if (text && obj.indexOf(text) === -1) {
                        continue;
                    }

                    // 是否自定义格子宽度
                    var style = '';
                    if (cellWidth !== null) {
                        style += ' style="width:' + cellWidth + '"';
                    }
                    // 每个图标dom
                    var icon = '<div class="layui-iconpicker-icon-item" title="' + obj + '" ' + style + '>';

                    if (type.indexOf("fontClass") > -1) {
                        icon += '<i class="layui-icon ' + obj + '"></i>';
                    } else {
                        icon += '<i class="' + fonttype + ' ' + obj + '"></i>';
                    }
                    icon += '</div>';

                    icons.push(icon);
                }

                // 查询出图标后再分页
                l = icons.length;
                _pages = l % _limit === 0 ? l / _limit : parseInt(l / _limit + 1);
                for (var i = 0; i < _pages; i++) {
                    // 按limit分块
                    var lm = $('<div class="layui-iconpicker-icon-limit" id="layui-iconpicker-icon-limit-' + tmp + (i + 1) + '">');

                    for (var j = i * _limit; j < (i + 1) * _limit && j < l; j++) {
                        lm.append(icons[j]);
                    }
                    listHtml.append(lm);
                }
                // 无数据
                if (l === 0) {
                    listHtml.append('<p class="layui-iconpicker-tips">无数据</p>');
                }
                // 判断是否分页
                if (page) {
                    $('#' + PICKER_BODY).addClass('layui-iconpicker-body-page');
                    pageHtml = '<div class="layui-iconpicker-page" id="' + PAGE_ID + '">' +
                        '<div class="layui-iconpicker-page-count">' +
                        '<span id="' + PAGE_ID + '-current">1</span>/' +
                        '<span id="' + PAGE_ID + '-pages">' + _pages + '</span>' +
                        ' (<span id="' + PAGE_ID + '-length">' + l + '</span>)' +
                        '</div>' +
                        '<div class="layui-iconpicker-page-operate">' +
                        '<span class="iconclear" id="icon_clear">清空</span> ' +
                        '<i class="layui-icon" id="' + PAGE_ID + '-prev" data-index="0" prev>&#xe603;</i> ' +
                        '<i class="layui-icon" id="' + PAGE_ID + '-next" data-index="2" next>&#xe602;</i> ' +
                        '</div>' +
                        '</div>';
                }
                $('#' + ICON_BODY).find('.layui-anim').find('.' + LIST_BOX).html('').append(listHtml).append(pageHtml);

                return a;
            },
            // 阻止Layui的一些默认事件
            preventEvent: function () {
                var item = '#' + ICON_BODY + ' .layui-anim';
                a.event('click', item, function (e) {
                    e.stopPropagation();
                });
                return a;
            },

            // 分页
            page: function () {
                var icon = '#' + PAGE_ID + ' .layui-iconpicker-page-operate .layui-icon';

                $(icon).unbind('click');
                a.event('click', icon, function (e) {
                    var elem = e.currentTarget,
                        total = parseInt($('#' + PAGE_ID + '-pages').html()),
                        isPrev = $(elem).attr('prev') !== undefined,
                        // 按钮上标的页码
                        index = parseInt($(elem).attr('data-index')),
                        $cur = $('#' + PAGE_ID + '-current'),
                        // 点击时正在显示的页码
                        current = parseInt($cur.html());

                    // 分页数据
                    if (isPrev && current > 1) {
                        current = current - 1;
                        $(icon + '[prev]').attr('data-index', current);
                    } else if (!isPrev && current < total) {
                        current = current + 1;
                        $(icon + '[next]').attr('data-index', current);
                    }
                    $cur.html(current);

                    // 图标数据
                    $('#' + ICON_BODY + ' .layui-iconpicker-icon-limit').hide();
                    $('#layui-iconpicker-icon-limit-' + tmp + current).show();
                    e.stopPropagation();
                });
                return a;
            },
            /**
             * 搜索
             */
            search: function () {
                var item = '#' + PICKER_BODY + ' .layui-iconpicker-search .layui-input';
                a.event('input propertychange', item, function (e) {
                    var elem = e.target,
                        t = $(elem).val();
                    a.createList(t);
                });
                return a;
            },
            /**
             * 点击选中图标
             */
            check: function () {
                if (type.indexOf("fontClass") > -1) {
                    var item = '#' + PICKER_BODY + ' .layui-iconpicker-icon-item';
                    a.event('click', item, function (e) {
                        var el = $(e.currentTarget).find('.layui-icon'),
                            icon = '';

                        var clsArr = el.attr('class').split(/[\s\n]/),
                            cls = clsArr[1],
                            icon = cls;
                        $('#' + TITLE_ID).find('.layui-iconpicker-item .layui-icon').html('').attr('class', clsArr.join(' '));


                        $('#' + ICON_BODY).removeClass(selected).addClass(unselect);
                        $(elem).val(icon).attr('value', icon);
                        // 回调
                        if (click) {
                            click({
                                icon: icon
                            });
                        }
                    });
                } else {
                    var item = '#' + PICKER_BODY + ' .layui-iconpicker-icon-item';
                    a.event('click', item, function (e) {
                        var el = $(e.currentTarget).find('.' + fonttype + ''),
                            icon = '';

                        var clsArr = el.attr('class').split(/[\s\n]/),
                            cls = clsArr[1],
                            icon = cls;
                        $('#' + TITLE_ID).find('.layui-iconpicker-item .' + fonttype + '').html('').attr('class', clsArr.join(' '));

                        $('#' + ICON_BODY).removeClass(selected).addClass(unselect);
                        $(elem).val(icon).attr('value', icon);
                        // 回调
                        if (click) {
                            click({
                                icon: icon
                            });
                        }

                    });
                }

                return a;

            },


            // 监听原始input数值改变
            inputListen: function () {
                var el = $(elem);
                a.event('change', elem, function () {
                    var value = el.val();
                })
                // el.change(function(){

                // });
                return a;
            },
            event: function (evt, el, fn) {
                $(BODY).on(evt, el, fn);
            }

        };



        var common = {
            bufferStr: null, // 字体编码内容
            iconList: [],
            /**
             * 加载样式表
             */
            loadCss: function () {
                var css = '.layui-iconpicker {max-width: 280px;}.layui-iconpicker .layui-anim{display:none;position:absolute;left:0;top:42px;padding:5px 0;z-index:899;min-width:100%;border:1px solid #d2d2d2;max-height:300px;overflow-y:auto;background-color:#fff;border-radius:2px;box-shadow:0 2px 4px rgba(0,0,0,.12);box-sizing:border-box;}.layui-iconpicker-item{border:1px solid #e6e6e6;width:90px;height:38px;border-radius:4px;cursor:pointer;position:relative;}.layui-iconpicker-icon{border-right:1px solid #e6e6e6;-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;display:block;width:60px;height:100%;float:left;text-align:center;background:#fff;transition:all .3s;}.layui-iconpicker-icon i{line-height:38px;font-size:18px;}.layui-iconpicker-item > .layui-edge{left:70px;}.layui-iconpicker-item:hover{border-color:#D2D2D2!important;}.layui-iconpicker-item:hover .layui-iconpicker-icon{border-color:#D2D2D2!important;}.layui-iconpicker.layui-form-selected .layui-anim{display:block;}.layui-iconpicker-body{padding:6px;}.layui-iconpicker .layui-iconpicker-list{background-color:#fff;border:1px solid #ccc;border-radius:4px;}.layui-iconpicker .layui-iconpicker-icon-item{display:inline-block;width:21.1%;line-height:36px;text-align:center;cursor:pointer;vertical-align:top;height:36px;margin:4px;border:1px solid #ddd;border-radius:2px;transition:300ms;}.layui-iconpicker .layui-iconpicker-icon-item i.layui-icon{font-size:17px;}.layui-iconpicker .layui-iconpicker-icon-item:hover{background-color:#eee;border-color:#ccc;-webkit-box-shadow:0 0 2px #aaa,0 0 2px #fff inset;-moz-box-shadow:0 0 2px #aaa,0 0 2px #fff inset;box-shadow:0 0 2px #aaa,0 0 2px #fff inset;text-shadow:0 0 1px #fff;}.layui-iconpicker-search{position:relative;margin:0 0 6px 0;border:1px solid #e6e6e6;border-radius:2px;transition:300ms;}.layui-iconpicker-search:hover{border-color:#D2D2D2!important;}.layui-iconpicker-search .layui-input{cursor:text;display:inline-block;width:86%;border:none;padding-right:0;margin-top:1px;}.layui-iconpicker-search .layui-icon{position:absolute;top:11px;right:4%;}.layui-iconpicker-tips{text-align:center;padding:8px 0;cursor:not-allowed;}.layui-iconpicker-page{margin-top:6px;margin-bottom:-6px;font-size:12px;padding:0 2px;}.layui-iconpicker-page-count{display:inline-block;}.layui-iconpicker-page-operate{display:inline-block;float:right;cursor:default;}.layui-iconpicker-page-operate .layui-icon{font-size:12px;cursor:pointer;}.layui-iconpicker-body-page .layui-iconpicker-icon-limit{display:none;}.layui-iconpicker-body-page .layui-iconpicker-icon-limit:first-child{display:block;}';
                var $style = $('head').find('style[iconpicker]');
                if ($style.length === 0) {
                    $('head').append('<style rel="stylesheet" iconpicker>' + css + '</style>');
                }
            },
            /**
             * 获取数据
             */
            getfont: {
                fontClass: function () {
                    var arr = ["layui-icon-rate-half", "layui-icon-rate", "layui-icon-rate-solid", "layui-icon-cellphone", "layui-icon-vercode", "layui-icon-login-wechat", "layui-icon-login-qq", "layui-icon-login-weibo", "layui-icon-password", "layui-icon-username", "layui-icon-refresh-3", "layui-icon-auz", "layui-icon-spread-left", "layui-icon-shrink-right", "layui-icon-snowflake", "layui-icon-tips", "layui-icon-note", "layui-icon-home", "layui-icon-senior", "layui-icon-refresh", "layui-icon-refresh-1", "layui-icon-flag", "layui-icon-theme", "layui-icon-notice", "layui-icon-website", "layui-icon-console", "layui-icon-face-surprised", "layui-icon-set", "layui-icon-template-1", "layui-icon-app", "layui-icon-template", "layui-icon-praise", "layui-icon-tread", "layui-icon-male", "layui-icon-female", "layui-icon-camera", "layui-icon-camera-fill", "layui-icon-more", "layui-icon-more-vertical", "layui-icon-rmb", "layui-icon-dollar", "layui-icon-diamond", "layui-icon-fire", "layui-icon-return", "layui-icon-location", "layui-icon-read", "layui-icon-survey", "layui-icon-face-smile", "layui-icon-face-cry", "layui-icon-cart-simple", "layui-icon-cart", "layui-icon-next", "layui-icon-prev", "layui-icon-upload-drag", "layui-icon-upload", "layui-icon-download-circle", "layui-icon-component", "layui-icon-file-b", "layui-icon-user", "layui-icon-find-fill", "layui-icon-loading", "layui-icon-loading-1", "layui-icon-add-1", "layui-icon-play", "layui-icon-pause",
                        "layui-icon-headset", "layui-icon-video", "layui-icon-voice", "layui-icon-speaker", "layui-icon-fonts-del", "layui-icon-fonts-code", "layui-icon-fonts-html", "layui-icon-fonts-strong", "layui-icon-unlink", "layui-icon-picture", "layui-icon-link", "layui-icon-face-smile-b", "layui-icon-align-left", "layui-icon-align-right", "layui-icon-align-center", "layui-icon-fonts-u", "layui-icon-fonts-i", "layui-icon-tabs", "layui-icon-radio", "layui-icon-circle", "layui-icon-edit", "layui-icon-share", "layui-icon-delete", "layui-icon-form", "layui-icon-cellphone-fine", "layui-icon-dialogue", "layui-icon-fonts-clear", "layui-icon-layer", "layui-icon-date", "layui-icon-water", "layui-icon-code-circle", "layui-icon-carousel", "layui-icon-prev-circle", "layui-icon-layouts", "layui-icon-util", "layui-icon-templeate-1", "layui-icon-upload-circle", "layui-icon-tree", "layui-icon-table", "layui-icon-chart", "layui-icon-chart-screen", "layui-icon-engine", "layui-icon-triangle-d", "layui-icon-triangle-r", "layui-icon-file", "layui-icon-set-sm", "layui-icon-add-circle", "layui-icon-404", "layui-icon-about", "layui-icon-up", "layui-icon-down", "layui-icon-left", "layui-icon-right", "layui-icon-circle-dot", "layui-icon-search", "layui-icon-set-fill", "layui-icon-group", "layui-icon-friends", "layui-icon-reply-fill", "layui-icon-menu-fill", "layui-icon-log", "layui-icon-picture-fine", "layui-icon-face-smile-fine", "layui-icon-list", "layui-icon-release", "layui-icon-ok",
                        "layui-icon-help", "layui-icon-chat", "layui-icon-top", "layui-icon-star", "layui-icon-star-fill", "layui-icon-close-fill", "layui-icon-close", "layui-icon-ok-circle", "layui-icon-add-circle-fine", "layui-icon-wifi", "layui-icon-rss", "layui-icon-mute", "layui-icon-heart", "layui-icon-heart-fill", "layui-icon-windows", "layui-icon-ios", "layui-icon-android", "layui-icon-logout", "layui-icon-email", "layui-icon-key", "layui-icon-mike", "layui-icon-slider", "layui-icon-print", "layui-icon-export", "layui-icon-service", "layui-icon-subtraction", "layui-icon-addition", "layui-icon-screen-restore", "layui-icon-screen-full", "layui-icon-time", "layui-icon-light", "layui-icon-at", "layui-icon-bluetooth"];
                    return arr;
                },
                iconfont: function () {
                    var arr = common.getOnlineCSS();
                    return arr;
                }
            },
            // ajax 请求
            ajax(options) {
                options = options || {};
                let xhr = new XMLHttpRequest();
                if (options.type === 'buffer') {
                    xhr.responseType = 'arraybuffer';
                }
                xhr.onreadystatechange = function () {
                    if (xhr.readyState === 4) {
                        let status = xhr.status;
                        if (status >= 200 && status < 300) {
                            options.success && options.success(xhr.response);
                        } else {
                            options.fail && options.fail(status);
                        }
                    }
                };
                xhr.open("GET", options.url, true);
                xhr.send(null);
            },
            stringToArrayBuffer(str) {
                var buf = new ArrayBuffer(str.length);
                var bufView = new Uint8Array(buf);
                for (var i = 0, strLen = str.length; i < strLen; i++) {
                    bufView[i] = str.charCodeAt(i);
                }
                return buf;
            },
            // 字符串转为ArrayBuffer对象，参数为字符串
            str2ab(str) {
                var buf = new ArrayBuffer(str.length * 2); // 每个字符占用2个字节
                var bufView = new Uint16Array(buf);
                for (var i = 0, strLen = str.length; i < strLen; i++) {
                    bufView[i] = str.charCodeAt(i);
                }
                return buf;
            },
            // 打开所有副本
            showAll() {
                this.parseIcon(this.bufferStr, true)
            },
            /**
             * 获取数据
             */
            getData: function (url) {
                ////远程获取文件
                //this.ajax({
                //    url: '/scripts/iconfont/iconfont.ttf',
                //    type: 'buffer',
                //    success: (params) => { console.log(params); this.parseIcon(params); }
                //})
                // 远程获取文件
                ajaxHttp({
                    url: api + 'Common/getBufferArray',
                    type: 'get',
                    async: false,
                    datatype: 'text',
                    success: (params) => { var bufferarray = this.stringToArrayBuffer(params); console.log(bufferarray); this.parseIcon(bufferarray); }
                })
                this.setStyle(this.url);
                return this.iconList;
            },
            // 解析 CSS 文件
            getOnlineCSS() {
                let urls = options.url;
                $.ajax({
                    url: urls,
                    type: 'get',
                    async: false,
                    datatype: 'text',
                    success: (params) => {
                        params.replace(/\.([^:^ ]+):[\s\S]+?content: "\\([^"]+)";/gi, (...item) => {
                            this.iconList.push(item[1])
                        })
                    }
                })
                this.loadCsss(urls, null)
                return this.iconList;
            },
            loadCsss(url, callback) {
                var script = document.createElement('link'),
                    fn = callback || function () { };
                script.rel = 'stylesheet';
                //IE
                if (script.readyState) {
                    script.onreadystatechange = function () {
                        if (script.readyState == 'loaded' || script.readyState == 'complete') {
                            script.onreadystatechange = null;
                            fn();
                        }
                    };
                } else {
                    //其他浏览器
                    script.onload = function () {
                        fn();
                    };
                }
                script.href = url;
                document.getElementsByTagName('head')[0].appendChild(script);
            },
            //填充Box
            FillFontBox() {
                var arr = [];
                for (var i = 0; i < this.iconList.length; i++) {
                    let json = this.iconList[i];
                    arr.push(json.name);
                }
                return arr;
            },
            // 添加style
            setStyle(url, cssFile) {
                let $style = document.createElement('style')
                if (cssFile) {
                    $style.innerHTML = cssFile
                } else {
                    $style.innerHTML = `
                                            @font-face {
                                      font-family: 'iconfont';
                                      src: url('${url}') format('truetype');
                                    }
                                    .iconfont {
                                      font-family: "iconfont" !important;
                                      font-size: 24px;font-style: normal;
                                      -webkit-font-smoothing: antialiased;
                                      -webkit-text-stroke-width: 0.2px;
                                      -moz-osx-font-smoothing: grayscale;
                                    }`;
                }
                document.body.append($style);
            },
            // 解析icon
            parseIcon(bufferStr, showAll) {
                this.bufferStr = bufferStr
                let result = window.opentype.parse(this.bufferStr);
                for (let key in result.glyphs.glyphs) {
                    let item = result.glyphs.glyphs[key]
                    if (!item.unicode) {
                    } else if (showAll) { // 是否显示所有 unicodes
                        let valueStr = ''
                        item.unicodes.forEach(unicode => valueStr += `&#${unicode};\n`)
                        this.iconList.push(item.name)
                    } else {
                        this.iconList.push(item.name)
                    }
                }
            }
        };

        a.init();
        return new IconHhys();

    }

    IconHhys.prototype.clearValue = function (filter, fonttype) {
        var className = 'layui-icon';
        className = fonttype == 'layui-icon' ? 'layui-icon' : fonttype;
        var el = $('*[lay-filter=' + filter + ']'),
            p = el.next().find('.layui-iconpicker-icon.layui-unselect .' + className);
        el.attr('value', '');
        p.attr('class', 'layui-icon');
    }

    /**
     * 选中图标
     * @param filter lay-filter
     * @param iconName 图标名称，自动识别fontClass/unicode
     */
    IconHhys.prototype.checkIcon = function (filter, iconName, fonttype) {
        var className = 'layui-icon';
        className = fonttype == 'layui-icon' ? 'layui-icon' : fonttype;
        var el = $('*[lay-filter=' + filter + ']'),
            p = el.next().find('.layui-iconpicker-item .' + className),
            c = iconName;
        if (c.indexOf('#xe') > -1) {
            p.html(c);
        } else {
            p.html('').attr('class', className + ' ' + c);
        }
        el.attr('value', c).val(c);
    };
    /**
     * 删除结构
     * @param filter lay-filter
     * @param iconName 图标名称，自动识别fontClass/unicode
     */
    IconHhys.prototype.remove = function () {
        $('#' + thistmp).remove();
    };



    /**
     * 选中图标auwosome
     * @param filter lay-filter
     * @param iconName 图标名称，自动识别fontClass/unicode
     */
    IconHhys.prototype.checkAwesome = function (filter, iconName, fonttype) {
        var el = $('*[lay-filter=' + filter + ']'),
            p = el.next().find('.layui-iconpicker-item .' + iconName + ''),
            c = iconName;
        console.log(iconName)
        if (c.indexOf('#xe') > -1) {
            p.html(c);
        } else {
            p.html('').attr('class', fonttype == 'fontClass' ? 'layui-icon' : fonttype + ' ' + c);
        }
        el.attr('value', c).val(c);
    };


    var iconHhys = new IconHhys();
    exports(_MOD, iconHhys);
});