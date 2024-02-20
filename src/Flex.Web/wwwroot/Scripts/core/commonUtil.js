function encodeData(data) {
    temp = data;

    temp = temp.replace(/\'/g, "**!1**");
    temp = temp.replace(/\(/g, "**!2**");
    temp = temp.replace(/\)/g, "**!3**");

    temp = replaceAll(temp, '..', "**!4**");
    //temp = replaceAll(temp, "\'", "**!5**"  );
    temp = replaceAll(temp, '"', "**!6**");
    //temp = replaceAll(temp, '\"', "**!7**"  );
    temp = replaceAll(temp, '<', "**!8**");
    temp = replaceAll(temp, '>', "**!9**");
    temp = replaceAll(temp, '|', "**!10**");
    temp = replaceAll(temp, '\\', "**!11**");
    //temp = replaceAll(temp, '+', "**!12**"  );
    //支付宝微信等接口需要明文传递+号字符，故现去掉+号转码（需CMS安全框架匹配），若安全系统对此有异议，请联系我们
    temp = temp.replace(/\+/g, "**!12**");
    //temp = replaceAll(temp, ';', "**!13**");
    //temp = temp.replace(/\;/g, "**!13**");
    //temp = replaceAll(temp, '@', "**!14**");
    temp = temp.replace(/\@/g, "**!14**");
    temp = replaceAll(temp, '$', "**!15**");
    temp = replaceAll(temp, ':', "**!16**");
    //temp = replaceAll(temp, '/', "**!17**");
    temp = replaceAll(temp, ' a', "**!18**");
    temp = replaceAll(temp, ' A', "**!19**");
    temp = replaceAll(temp, '/**/', "**!20**");

    return temp
}
function replaceAll(targetStr, oldStr, newStr) {
    var endStr = targetStr;
    var index = targetStr.indexOf(oldStr);

    var i = 0;

    while (index != -1) {
        endStr = endStr.replace(oldStr, newStr);

        index = endStr.indexOf(oldStr, index + 1);
    }

    return endStr;
}
//定义一个方法从地址栏取出想要的信息
(function ($) {
    $.getUrlParam = function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null)
            return unescape(r[2]);
        return null;
    }
})(jQuery);
String.prototype.startWith = function (str) {
    var reg = new RegExp("^" + str);

    return reg.test(this);
}
var UnitHtml = {
    unhtml: function (str, reg) {
        return str ? str.replace(reg || /[&<">]/g, function (m) {
            return {
                "<": "&lt;",
                "&": "&amp;",
                '"': "&quot;",
                ">": "&gt;"
            }[m]
        }) : ""
    },
    html: function (str) {
        return str ? str.replace(/&((g|l|quo)t|amp);/g, function (m) {
            return {
                "&lt;": "<",
                "&amp;": "&",
                "&quot;": '"',
                "&gt;": ">"
            }[m]
        }) : ""
    }
}
String.prototype.endWith = function (str) {
    var reg = new RegExp(str + "$");
    return reg.test(this);
}