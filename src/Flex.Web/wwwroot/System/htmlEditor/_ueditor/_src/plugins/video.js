///import core
///import plugins/inserthtml.js
///commands 视频
///commandsName InsertVideo
///commandsTitle  插入视频
///commandsDialog  dialogs\video\video.html
UE.plugins['video'] = function () {
    var me = this,
        div;

    /**
    * 创建插入视频字符窜
    * @param url 视频地址
    * @param width 视频宽度
    * @param height 视频高度
    * @param align 视频对齐
    * @param toEmbed 是否以flash代替显示
    * @param addParagraph  是否需要添加P 标签
    */
    function creatInsertStr(url, width, height, align, toEmbed, addParagraph) {
        var ispaly = false;
        var str = '';
        if (rel != null && rel == "true") {
            ispaly = rel; str = 'rel="player.swf"  flashvars="file=' + url + '&autostart=true&stretching=fill&height=640&width=360&controlbar=bottom&backcolor=0xCCCCCC&frontcolor=0x000000&lightcolor=0x0a3968"';
        }
        return !toEmbed ?
                (addParagraph ? ('<p ' + (align != "none" ? (align == "center" ? ' style="text-align:center;" ' : ' style="float:"' + align) : '') + '>') : '') +
                '<img ' + str + ' align="' + align + '"  width="' + width + '" height="' + height + '" _url="' + url + '" class="edui-faked-video"' +
                ' src="' + me.options.UEDITOR_HOME_URL + 'themes/default/images/spacer.gif" style="background:url(' + me.options.UEDITOR_HOME_URL + 'themes/default/images/videologo.gif) no-repeat center center; border:1px solid gray;" />' +
                (addParagraph ? '</p>' : '')
                :
                '<embed  ' + str + ' type="application/x-shockwave-flash" class="edui-faked-video" pluginspage="http://www.macromedia.com/go/getflashplayer"' +
                ' src="' + ((ispaly) ? 'player.swf' : url) + '" _url=' + url + '  width="' + width + '" height="' + height + '" align="' + align + '"' +
                (align != "none" ? ' style= "' + (align == "center" ? "display:block;" : " float: " + align) + '"' : '') +
                ' wmode="transparent" play="true" loop="false" menu="false" allowscriptaccess="never" allowfullscreen="true" >';
    }

    function switchImgAndEmbed(img2embed) {
        var tmpdiv,
            nodes = domUtils.getElementsByTagName(me.document, !img2embed ? "embed" : "img");
        for (var i = 0, node; node = nodes[i++]; ) {
            if (node.className != "edui-faked-video") {
                continue;
            }
            tmpdiv = me.document.createElement("div");
            //先看float在看align,浮动有的是时候是在float上定义的
            var align = node.style.cssFloat;
            var rel = node.getAttribute("rel") != null ? "true" : "false";
            tmpdiv.innerHTML = creatInsertStr(((!rel) ? (img2embed ? node.getAttribute("_url") : node.getAttribute("src")) : (node.getAttribute("_url"))), node.width, node.height, align || node.getAttribute("align"), img2embed, false, rel);
            node.parentNode.replaceChild(tmpdiv.firstChild, node);
        }
    }
    me.addListener("beforegetcontent", function () {
        switchImgAndEmbed(true);
    });
    me.addListener('aftersetcontent', function () {
        switchImgAndEmbed(false);
    });
    me.addListener('aftergetcontent', function (cmdName) {
        if (cmdName == 'aftergetcontent' && me.queryCommandState('source')) {
            return;
        }
        switchImgAndEmbed(false);
    });

    me.commands["insertvideo"] = {
        execCommand: function (cmd, videoObjs) {
            videoObjs = utils.isArray(videoObjs) ? videoObjs : [videoObjs];
            var html = [];
            for (var i = 0, vi, len = videoObjs.length; i < len; i++) {
                if (vi.rel != null && vi.rel == "true") {
                    html.push(creatInsertStr(vi.url, vi.width || 420, vi.height || 280, vi.align || "none", false, false, vi.rel));
                }
                else {
                    html.push(creatInsertStr(vi.url, vi.width || 420, vi.height || 280, vi.align || "none", false, false));
                }
            }
            me.execCommand("inserthtml", html.join(""));
        },
        queryCommandState: function () {
            var img = me.selection.getRange().getClosedNode(),
                flag = img && (img.className == "edui-faked-video");
            return this.highlight ? -1 : (flag ? 1 : 0);
        }
    };
};