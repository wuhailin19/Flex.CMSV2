var basePath="";



var ue;//当前页面所操作的UE编辑器实例

// 模板空间设置可上传的类型
var TEMPLATE_RULE = ".thtml,.ftl,.xml,.shtml,.htm,.html,.js,.css,.txt,.pdf,.swf,.flv,.png,.jpeg,.gif,.bmp,.jpg,.f4v,.avi,.mpg,.mpeg,.mp3,.wav,.mp4,.doc,.docx,.pptx,.ppt,.xlsx,.xls,.vsd,.zip,.rar,.dat";

function linkBlur()
{
    for(var i=0; i<document.links.length; i++)
    {
        document.links[i].onclick=function(){this.blur()}
    }
}

//textarea字数统计
function wordLeg(obj)
{
    var currleg = $(obj).val().length;

    var length = $(obj).attr('maxlength');

    if (currleg > length) {

        layer.msg('字数请在' + length + '字以内');

    } else {

        $('#text_count_'+obj.id).text(currleg);

    }
}


var splitChar='*';
//  ua = navigator.userAgent.toLowerCase(),

//   check = function(r){
//      return r.test(ua);
//    },
//    isStrict = document.compatMode == "CSS1Compat",//    isOpera = check(/opera/),
//    isChrome = check(/chrome/),
//    isWebKit = check(/webkit/),
//    isSafari = !isChrome && check(/safari/),
//    isSafari3 = isSafari && check(/version\/3/),
//    isSafari4 = isSafari && check(/version\/4/),
//    isOpera = check(/opera/),
//    isIE = !isOpera && check(/msie/),
//    isIE7 = isIE && check(/msie 7/),
//    isIE8 = isIE && check(/msie 8/),
//    isFF = check(/firefox/),
//    isGecko = !isWebKit && check(/gecko/),
//    isGecko3 = isGecko && check(/rv:1\.9/),
//    isBorderBox = isIE && !isStrict,
//    isWindows = check(/windows|win32/),
//    isMac = check(/macintosh|mac os x/),
//    isAir = check(/adobeair/),
//    isLinux = check(/linux/),
//    isSecure = /^https/i.test(window.location.protocol);



//自定义模型
/**
 * 取指定frameName中 指定targetId的元素值
 */
function getResultFromFrame(frameName,targetId)
{
    return document.getElementById(frameName).contentWindow.document.getElementById(targetId).value;
}
/********************************************************** Util **********************************************************/

//IE8 console不支持
window.console = window.console || (function () {
    var c ={};
    c.log = c.warn = c.debug = c.info = c.error = c.time = c.dir = c.profile= c.clear = c.exception = c.trace = c.assert = function(){};
    return c;
})();

//兼容的添加 onpropertychange 事件
function onpropertychange(element, fun, capture)
{/* shawl.qiu code, return default, func:none */

    if(document.addEventListener) return element.addEventListener("input", func, capture);
    if(document.attachEvent) return element.attachEvent("onpropertychange",func);
    alert("浏览器不支持 onpropertychange 函数!");
    return;
}

//扩展Date的format方法
Date.prototype.format = function (format) {
    var o = {
        "M+": this.getMonth() + 1,
        "d+": this.getDate(),
        "h+": this.getHours(),
        "m+": this.getMinutes(),
        "s+": this.getSeconds(),
        "q+": Math.floor((this.getMonth() + 3) / 3),
        "S": this.getMilliseconds()
    }
    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
}
/**
 *转换日期对象为日期字符串
 * @param date 日期对象
 * @param isFull 是否为完整的日期数据,
 *               为true时, 格式如"2000-03-05 01:05:04"
 *               为false时, 格式如 "2000-03-05"
 * @return 符合要求的日期字符串
 */
function getSmpFormatDate(date, isFull) {
    var pattern = "";
    if (isFull == true || isFull == undefined) {
        pattern = "yyyy-MM-dd hh:mm:ss";
    } else {
        pattern = "yyyy-MM-dd";
    }
    return getFormatDate(date, pattern);
}
/**
 *转换当前日期对象为日期字符串
 * @param date 日期对象
 * @param isFull 是否为完整的日期数据,
 *               为true时, 格式如"2000-03-05 01:05:04"
 *               为false时, 格式如 "2000-03-05"
 * @return 符合要求的日期字符串
 */

function getSmpFormatNowDate(isFull) {
    return getSmpFormatDate(new Date(), isFull);
}
/**
 *转换long值为日期字符串
 * @param l long值
 * @param isFull 是否为完整的日期数据,
 *               为true时, 格式如"2000-03-05 01:05:04"
 *               为false时, 格式如 "2000-03-05"
 * @return 符合要求的日期字符串
 */

function getSmpFormatDateByLong(l, isFull) {
    return getSmpFormatDate(new Date(l), isFull);
}
/**
 *转换long值为日期字符串
 * @param l long值
 * @param pattern 格式字符串,例如：yyyy-MM-dd hh:mm:ss
 * @return 符合要求的日期字符串
 */

function getFormatDateByLong(l, pattern) {
    return getFormatDate(new Date(l), pattern);
}
/**
 *转换日期对象为日期字符串
 * @param l long值
 * @param pattern 格式字符串,例如：yyyy-MM-dd hh:mm:ss
 * @return 符合要求的日期字符串
 */
function getFormatDate(date, pattern) {
    if (date == undefined) {
        date = new Date();
    }
    if (pattern == undefined) {
        pattern = "yyyy-MM-dd hh:mm:ss";
    }
    return date.format(pattern);
}





//扩展String的一些方法

String.prototype.trim=function()
{
    return this.replace(/(^\s*)|(\s*$)/g,'');
}

String.prototype.replaceAll = function(reallyDo, replaceWith, ignoreCase) {
    if (!RegExp.prototype.isPrototypeOf(reallyDo)) {
        return this.replace(new RegExp(reallyDo, (ignoreCase ? "gi": "g")), replaceWith);
    } else {
        return this.replace(reallyDo, replaceWith);
    }
}

function replaceAll(targetStr, oldStr, newStr)
{
    var endStr = targetStr;
    var index = targetStr.indexOf( oldStr );

    var i = 0;

    while( index != -1 )
    {
        endStr = endStr.replace(oldStr, newStr);

        index = endStr.indexOf(oldStr,  index+1);
    }

    return endStr;
}

String.prototype.startWith=function(str){
    var reg=new RegExp("^"+str);

    return reg.test(this);
}

String.prototype.endWith=function(str){
    var reg=new RegExp(str+"$");
    return reg.test(this);
}


//StringBuffer
var StringBuffer = function(){
    this._strings_ = new Array();
}

StringBuffer.prototype.append = function(str){
    this._strings_ .push(str);
}

StringBuffer.prototype.toString = function(sc){
    return this._strings_.join(sc);
}

//获取select已经选中的文本html
function getSelectedText(name)
{
    var obj=document.getElementById(name);
    for(i=0;i<obj.length;i++)
    {
        if(obj[i].selected == true)
        {
            return obj[i].innerHTML;
        }
    }
}

//初始化radio,针对指定的radio名字，将等于给定value值的选项选中
function initRadio(targetRadioName,initValue)
{
    var radioArray = document.getElementsByName(targetRadioName);

    for (var i=0; i<radioArray.length; i++)
    {
        if (radioArray[i].value==initValue)
        {
            radioArray[i].checked= true;
            break;
        }
    }
}


//获取radio选取的值
function getRadioCheckedValue(targetRadioName)
{
    var radioArray = document.getElementsByName(targetRadioName);

    for (var i=0; i<radioArray.length; i++)
    {
        if (radioArray[i].checked == true)
        {
            return radioArray[i].value;
        }
    }

    return null;
}

//初始化select,针对指定的select ID，将等于给定value值的选项选中
function initSelect(targetSelectId,initValue)
{
    var select = document.getElementById(targetSelectId);

    if(select == null)
    {
        return;
    }

    for (var i=0; i<select.length; i++)
    {
        if (select.options[i].value==initValue)
        {
            select.options[i].selected=true;
            break;
        }
    }
}

//初始化select,针对指定的多个select ID，将等于给定value值的选项选中
function initMupSelect(targetSelectId,initValueArray)
{
    var select = document.getElementById(targetSelectId);

    for (var i=0; i<select.length; i++)
    {
        for(var j=0;j<initValueArray.length;j++ )
        {
            if (select.options[i].value==initValueArray[j])
            {
                select.options[i].selected=true;
            }
        }
    }
}



//勾选所有checkbox
function selectAll(targetCheckName, allObj)
{
    var allCheck = allObj.checked;

    var checks = document.getElementsByName(targetCheckName);

    for(var i = 0; i < checks.length; i++)
    {
        if('none' == checks[i].style.display)
        {
            continue;
        }

        if(allCheck)
        {
            checks[i].checked=true;
        }
        else
        {
            checks[i].checked=false;
        }

        //targetMap.put(checks[i].value,checks[i].value);
    }
    // targetMap.remove(checkAllIdName);

}

//勾选所有checkbox
function selectAllLay(targetCheckName, isCheck, form)
{

    var checks = document.getElementsByName(targetCheckName);

    for(var i = 0; i < checks.length; i++)
    {
        if('none' == checks[i].style.display)
        {
            continue;
        }

        if(isCheck)
        {
            checks[i].checked=true;

        }
        else
        {
            checks[i].checked=false;

        }

        //targetMap.put(checks[i].value,checks[i].value);
    }
    // targetMap.remove(checkAllIdName);
    form.render();
}


//勾选所有checkbox
function selectAllBox(targetCheckName)
{

    var checks = document.getElementsByName(targetCheckName);

    for(var i = 0; i < checks.length; i++)
    {

        checks[i].checked=true;

        //targetMap.put(checks[i].value,checks[i].value);
    }
    // targetMap.remove(checkAllIdName);

}


function checkSelectAll(flag,allFlag)
{
    var boxs = document.getElementsByName(flag);

    var box;
    var checkStateIn = false;
    var notCheckStateIn = false;
    for(var i = 0; i < boxs.length; i++)
    {
        box = boxs[i];

        if(box.checked)
        {
            checkStateIn = true;
        }
        else
        {
            notCheckStateIn = true;
        }
    }

    if(checkStateIn && notCheckStateIn)
    {
        document.getElementById(allFlag).checked = false;
    }
    else if(checkStateIn)
    {
        document.getElementById(allFlag).checked = true;
    }
    else if(notCheckStateIn)
    {
        document.getElementById(allFlag).checked = false;
    }
}




//取消勾选所有checkbox
function unSelectAllBox(targetCheckName)
{
    var checks = document.getElementsByName(targetCheckName);

    for(var i = 0; i < checks.length; i++)
    {
        checks[i].checked=false;
    }
    //targetMap.removeAll();
}

//背景选取固定色
function changeTRBG(obj, index)
{
    //背景色

    if(obj.checked == true)
    {
        $('#tr-'+index).addClass("tdbgyewck");
    }
    else
    {
        $('#tr-'+index).removeClass("tdbgyewck");
    }

}
//公用方法 结束


//当前页后退
function back()
{
    window.history.back();
}

//
function backAndReload()
{
    window.history.back();
    window.location.reload();
}

// 禁用、启用超链接 ie ff通用
// @targetId : 超链接ID
// @disable: true,禁用； false：启用
function disableAnchorElement(targetId, disableFlag)
{
    var obj = document.getElementById(targetId);

    if(obj == null)
    {
        return;
    }

    if (disableFlag)
    { // disable
        var href = obj.getAttribute("href");
        if (href && href != "" && href != null)
        {
            obj.setAttribute('href_bak', href);
        }

        //移去href
        obj.removeAttribute('href');
        // 处理onclick事件,无效化
        var onclick = obj.getAttribute("onclick");
        if(onclick != null)
        {
            obj.setAttribute('onclick_bak', onclick);
            obj.setAttribute('onclick', "void(0);");
        }

        obj.onclick = null;
        obj.style.color = "gray";
        obj.style.cursor="not-allowed";
        obj.setAttribute('disabled', 'disabled');
    }
    else
    { // enable,还原值
        if (obj.attributes['href_bak'])
        {
            obj.setAttribute('href', obj.attributes['href_bak'].nodeValue);
        }

        if(obj.attributes['onclick_bak']!=null)
        {
            obj.setAttribute('onclick', obj.attributes['onclick_bak'].nodeValue);
        }

        obj.removeAttribute('style');
        obj.removeAttribute('disabled');
    }
}

function disableAnchorElementByName(targetName, disableFlag)
{
    var objs = document.getElementsByName(targetName);

    var obj;
    for(var i = 0; i < objs.length; i++)
    {
        obj = objs[i];

        if(obj == null)
        {
            return;
        }

        if (disableFlag)
        { // disable
            var href = obj.getAttribute("href");
            if (href && href != "" && href != null)
            {
                obj.setAttribute('href_bak', href);
            }

            //移去href
            obj.removeAttribute('href');
            // 处理onclick事件,无效化
            var onclick = obj.getAttribute("onclick");
            if(onclick != null)
            {
                obj.setAttribute('onclick_bak', onclick);
                obj.setAttribute('onclick', "void(0);");
            }

            obj.onclick = null;
            obj.style.color = "gray";
            obj.style.cursor="not-allowed";
            obj.setAttribute('disabled', 'disabled');
        }
        else
        { // enable,还原值
            if (obj.attributes['href_bak'])
            {
                obj.setAttribute('href', obj.attributes['href_bak'].nodeValue);
            }

            if(obj.attributes['onclick_bak']!=null)
            {
                obj.setAttribute('onclick', obj.attributes['onclick_bak'].nodeValue);
            }

            obj.removeAttribute('style');
            obj.removeAttribute('disabled');
        }

    }


}


//除法函数，用来得到精确的除法结果

function accDiv(arg1,arg2)
{
    if(arg2 ==0)
    {
        return 0;
    }

    var t1=0,t2=0,r1,r2;
    try{t1=arg1.toString().split(".")[1].length}catch(e){}
    try{t2=arg2.toString().split(".")[1].length}catch(e){}
    with(Math){
        r1=Number(arg1.toString().replace(".",""));
        r2=Number(arg2.toString().replace(".",""));
        return (r1/r2)*pow(10,t2-t1);
    }
}
//function initChaeckBox(targetBoxName,initValueArray)
//{
//
//
//
//	var radioArray = document.getElementsByName(targetRadioName);
//    for (var i=0; i<radioArray.length; i++)
//    {
//
//    }
//}

/**
 * 和当前日期对比,获取多少天后的日期
 */
function getOtherDate(n)
{
    var nn = new Date();
    nn.setDate(nn.getDate()+n);
    year1 = nn.getYear();
    mon1 = nn.getMonth() + 1;
    date1 = nn.getDate();
    var monstr1;
    var datestr1
    if (mon1 < 10)
        monstr1 = "0" + mon1;
    else
        monstr1 = "" + mon1;

    if (date1 < 10)
        datestr1 = "0" + date1;
    else
        datestr1 = "" + date1;
    return year1 + "-" + monstr1 + "-" + datestr1;
}


/********************************************************** HashTable **********************************************************/
function Hashtable()
{
    this.container = new Object();

    /**//** put element */
this.put = function (key, value)
{
    if (typeof (key) == "undefined")
    {
        return false;
    }
    if (this.contains(key))
    {
        return false;
    }
    this.container[key] = typeof (value) == "undefined" ? null : value;
    return true;
};

    /**//** remove element */
this.remove = function (key)
{
    delete this.container[key];
};

    /**//** get size */
this.size = function ()
{
    var size = 0;
    for (var attr in this.container)
    {
        size++;
    }
    return size;
};

    /**//** get value by key */
this.get = function (key)
{
    return this.container[key];
};

    /**//** containts a key */
this.contains = function (key)
{
    return typeof (this.container[key]) != "undefined";
};

    /**//** clear all entrys */
this.clear = function ()
{
    for (var attr in this.container)
    {
        delete this.container[attr];
    }
};

    /**//** hashTable 2 string */
this.toString = function()
{
    var str = "";
    for (var attr in this.container)
    {
        str += "," + attr + "=" + this.container[attr];
    }
    if(str.length>0)
    {
        str = str.substr(1, str.length);
    }
    return "{" + str + "}";
};
}

var hashtable = new Hashtable();
hashtable.put('1','huyvanpull');
hashtable.put('2','ensoodge');
hashtable.put('3','huyfan');

hashtable.remove('2');

















/********************************************************** HashMap **********************************************************/
var HashMapJs = function() {

    this.initialize();

}



HashMapJs.hashmap_instance_id = 0;



HashMapJs.prototype = {

    hashkey_prefix: "<#HashMapHashkeyPerfix>",

    hashcode_field: "<#HashMapHashcodeField>",



    initialize: function() {

        this.backing_hash = {};

        this.code = 0;

        this.mapSize=0;

        HashMapJs.hashmap_instance_id += 1;

        this.instance_id = HashMapJs.hashmap_instance_id;

    },



    hashcodeField: function() {

        return this.hashcode_field + this.instance_id;

    },

    /*

     maps value to key returning previous assocciation

     */

    put: function(key, value) {

        var prev;



        if (key && value) {

            var hashCode;

            if (typeof(key) === "number" || typeof(key) === "string") {

                hashCode = key;

            } else {

                hashCode = key[this.hashcodeField()];

            }

            if (hashCode) {

                prev = this.backing_hash[hashCode];

            } else {

                this.code += 1;

                hashCode = this.hashkey_prefix + this.code;

                key[this.hashcodeField()] = hashCode;

            }

            this.backing_hash[hashCode] = [key, value];

        }

        this.mapSize++;

        return prev === undefined ? undefined : prev[1];

    },

    /*

     returns value associated with given key

     */

    get: function(key) {

        var value;

        if (key) {

            var hashCode;

            if (typeof(key) === "number" || typeof(key) === "string") {

                hashCode = key;

            } else {

                hashCode = key[this.hashcodeField()];

            }

            if (hashCode) {

                value = this.backing_hash[hashCode];

            }

        }

        return value === undefined ? undefined : value[1];

    },

    /*

     deletes association by given key.

     Returns true if the assocciation existed, false otherwise

     */

    remove: function(key) {

        var success = false;

        if (key)
        {

            var hashCode;

            if (typeof(key) === "number" || typeof(key) === "string") {

                hashCode = key;

            } else {

                hashCode = key[this.hashcodeField()];


            }

            if (hashCode) {

                var prev = this.backing_hash[hashCode];

                this.backing_hash[hashCode] = undefined;

                if (prev !== undefined){

                    key[this.hashcodeField()] = undefined; //let's clean the key object

                    success = true;

                }

            }

        }

        if(success)
        {
            this.mapSize--;
        }

        return success;

    },

    /*

     iterate over key-value pairs passing them to provided callback

     the iteration process is interrupted when the callback returns false.

     the execution context of the callback is the value of the key-value pair

     @ returns the HashMap (so we can chain)                                                                  (

     */

    each: function(callback, args) {

        var key;

        for (key in this.backing_hash){

            if (callback.call(this.backing_hash[key][1], this.backing_hash[key][0], this.backing_hash[key][1]) === false)

                break;

        }

        return this;

    },

    /**将当前map中的所有值转换为一个String，分隔符由参数传入  **/
    allValueToString: function(sep)
    {
        var key;
        var endValueStr='';
        var index = 0;
        for (key in this.backing_hash)
        {
            if( this.backing_hash[key] !== undefined)
            {
                index ++;
                if(index == this.mapSize)
                {
                    endValueStr += this.backing_hash[key][1];
                }
                else
                {
                    endValueStr += this.backing_hash[key][1]+sep;
                }
            }
        }
        return endValueStr;
    },

    /**将当前map中的所有key转换为一个String，分隔符由参数传入  **/
    allKeyToString: function(sep)
    {
        var key;
        var endKeyStr='';
        var index = 0;
        for (key in this.backing_hash)
        {
            index ++;
            if( this.backing_hash[key] !== undefined)
            {
                if(index == this.mapSize)
                {
                    endKeyStr += key;
                }
                else
                {
                    endKeyStr += key+sep;
                }
            }
        }
        return endKeyStr;
    },

    /**当前map的大小  **/
    size: function()
    {
        return this.mapSize;
    },

    /**删除map中所有元素  **/
    removeAll: function()
    {
        this.backing_hash = {};
        this.mapSize=0;
    },

    toString: function() {
        return "HashMapJS"

    }
}


/********************************************************** 临时的弹出窗口 **********************************************************/

function openWindow(targetPageUrl,pageId,height,width,topX,leftX,testMode)
{
    if(!testMode)
    {
        window.open(targetPageUrl,pageId,'height='+height+',width='+width+',top='+topX+',left='+leftX+',toolbar=no,meaubar=no,scrollbars=no,resizable=no,titlebar=no,location=no,status=no');
    }
    else
    {

    }
}

function gotoCurrentPage(url)
{
    var link=document.createElement("a");
    link.href=url;
    document.body.appendChild(link);


    //TODO FF不支持
    link.click();

}


/********************************************************** File function **********************************************************/

function entryAction(targetURL,entry,param)
{
    var prevEntry='';

    var es = entry.split(splitChar);

    for(var i=0; i<es.length; i++)
    {
        if(i+1 != es.length)
        {
            if(i+2==es.length)
            {
                prevEntry+=es[i];
            }else
            {
                prevEntry+=es[i]+splitChar;
            }
        }
    }

    if(typeof(param) != "undefined")
    {
        //document.location.href=targetURL+"?entry="+entry+"&prevEntry="+prevEntry+param;
        gotoCurrentPage(targetURL+"?entry="+entry+"&prevEntry="+prevEntry+param);
    }
    else
    {
        //document.location.href=targetURL+"?entry="+entry+"&prevEntry="+prevEntry;
        gotoCurrentPage(targetURL+"?entry="+entry+"&prevEntry="+prevEntry);
    }
}





/*****************************数据模型*********************************/

function getResultFromFrame(frameName,targetName)
{
    return document.getElementById(frameName).contentWindow.document.getElementById(targetName).value;
}



/*****************************URL工具*********************************/

/**
 * 替换当前URL中的参数，若当前URL无此参数，则加上新的参数:targetNameValueStr (a=1&b=2)
 */
function replaceUrlParam(loc,targetNameValueStr)
{
    var targetName;
    var value;

    var splitCharPos;

    var tmpKey;
    var tmpVal;

    var params = loc.search;

    targetNameValueStr = encodeURI(targetNameValueStr);

    if(targetNameValueStr == null || targetNameValueStr == '')
    {
        loc.href=href;
    }

    if(params.indexOf("?")!=-1)//已有参数
    {
        var href = loc.href.substring(0,loc.href.indexOf("?"));


        var nvArray = targetNameValueStr.split('&');
        for(var j=0; j<nvArray.length; j++)
        {
            splitCharPos = nvArray[j].indexOf('=');

            tmpKey = nvArray[j].substring(0, splitCharPos);
            tmpVal = nvArray[j].substring((splitCharPos+1), nvArray[j].length );

            targetName = tmpKey;
            value = tmpVal;

            if(targetName != 'undefined')
            {


                if(params.indexOf('&'+targetName+'=')!=-1 || params.indexOf('?'+targetName+'=')!=-1)//已存在当前参数
                {

                    var searchStrArray = params.substring(1,params.length).split('&');
                    var targetP;
                    var targetV;
                    for(var i=0;i<searchStrArray.length;i++)
                    {
                        splitCharPos = searchStrArray[i].indexOf('=');

                        tmpKey = searchStrArray[i].substring(0, splitCharPos);
                        tmpVal = searchStrArray[i].substring((splitCharPos+1), searchStrArray[i].length );

                        if(tmpKey==targetName)
                        {
                            targetP = tmpKey;
                            targetV = tmpVal;
                            break;
                        }
                    }
                    if(params.indexOf('?'+targetName+'=')!=-1)
                    {
                        params =replaceAll(params,'?'+targetP+'='+targetV,'?'+targetName+'='+value);//替换
                    }
                    else
                    {
                        params =replaceAll(params,'&'+targetP+'='+targetV,'&'+targetName+'='+value);//替换
                    }

                }
                else//不存在当前参数
                {
                    params+="&"+targetName+"="+value;
                }
            }
        }

        loc.href=href+params;

    }
    else//没有参数
    {
        loc.href+="?"+targetNameValueStr;
        ;
    }

}

function getFormJson(frm) {    var o = {};    var a = $(frm).serializeArray();    $.each(a, function () {        if (o[this.name] !== undefined) {            if (!o[this.name].push) {                o[this.name] = [o[this.name]];            }            o[this.name].push(this.value || '');        } else {            o[this.name] = this.value || '';        }    });    return o;}


/////////////////////////////////////////////UI///////////////////////////////////////////////////

//获取内容主frame窗口


function fnGetMainWin()
{
    var fs = parent.frames;
    for(var i=0; i< fs.length; i++)
    {

        if(fs[i].location.href.indexOf('/core/content/manage-general-content.thtml') != -1)
        {
            return fs[i].frames['main'];
        }
    }
}



//获取父窗口弹出的layer窗口对象

function fnGetLayWin(frn)
{
    var fl = $("div.layui-layer-content > iframe", parent.document);

    for(var index in  fl)
    {
        var ifnw = fl[index].contentWindow;

        if(ifnw != null && typeof(ifnw) != 'undefined')
        {
            if(ifnw.name == frn)
            {
                return ifnw;
            }
        }
    }

    return null;

}

//设置Layui input disabled状态和解除
function fnSetLayInputDis(id, status)
{
    if(1 == status)
    {
        $('#'+id).attr('disabled', 'disabled');
        $('#'+id).addClass('layui-btn-disabled');
        $('#'+id).css( {'background-color': '#FBFBFB','color': '#C9C9C9'});
    }
    else if(0 == status)
    {
        $('#'+id).removeAttr('disabled');
        $('#'+id).removeClass('layui-btn-disabled');
        $('#'+id).css( {'background-color': '','color': ''});
    }

}

var imgIndexArray = new Array();
var orderIndexArray = new Array();


////////////////图片集////////////////////

function fnInitImageGroup(fn, mw, mh, order, url, resizeUrl, resId, reUrl, fileName, height, width, photoDesc, resSize, photoTitle, link, classId, du, re, groupType)
{
    $('#noimg-'+fn).hide();

    var fileListView = $('#group-list-'+fn)

    //2022重构

    var hp = getGroupHtml(order, fn, fileName, classId,'true', groupType);

    hp = hp.replace('${result}', url);
    hp = hp.replace('${fileSize}', sizeToStr(resSize));


    hp = replaceAll(hp,'${fileName}', fileName);
    hp = replaceAll(hp,'${width}', width);
    hp = replaceAll(hp,'${height}', height);
    hp = replaceAll(hp,'${resId}', resId);
    hp = replaceAll(hp,'${relatePath}', reUrl);
    hp = replaceAll(hp,'${title}', photoTitle);
    hp = replaceAll(hp,'${desc}', photoDesc);
    hp = replaceAll(hp,'${link}', link);

    hp = replaceAll(hp,'${mw}', mw);
    hp = replaceAll(hp,'${mh}', mh);

    hp = replaceAll(hp,'${time}', du+"秒");
    hp = replaceAll(hp,'${wh}', re);

    var tr = $([hp].join(''));



    //删除
    tr.find('.test-upload-demo-delete').on('click', function(){
        // delete files[index]; //删除对应的文件
        tr.remove();
        if($("#group-list-"+fn).children("tr").length == 1)
        {
            $('#noimg-'+fn).show();
        }
        //修正高度
        fixMW();
        // uploadListIns.config.elem.next()[0].value = ''; //清空 input file 值，以免删除后出现同名文件不可选
    });

    fileListView.append(tr);

}

/**
 * 文件集内排序
 */
function changePos(fieldSign, currentPos, action)
{

    var targetPos;

    var testId;

    if('down' == action)
    {
        targetPos = currentPos+1;

        testId = $("#upload-resId-"+fieldSign+'-'+targetPos).val();

        if(!testId)
        {
            for(var i=targetPos; i<1000; i++)
            {
                testId = $("#upload-resId-"+fieldSign+'-'+i).val();

                if(testId)
                {
                    targetPos=i;
                    break;
                }

            }
        }

        if(!testId)
        {
            layer.msg('已在末位');
            return;
        }
    }
    else if('up' == action)
    {
        targetPos = currentPos-1;

        testId = $("#upload-resId-"+fieldSign+'-'+targetPos).val();

        if(!testId)
        {
            for(var i=targetPos; i>-1; i--)
            {
                testId = $("#upload-resId-"+fieldSign+'-'+i).val();

                if(testId)
                {
                    targetPos=i;
                    break;
                }

            }
        }

        if(!testId)
        {
            layer.msg('已在首位');
            return;
        }
    }

    if(!testId)
    {
        return;
    }

    var tempShowImageHref = $("#upload-src-"+fieldSign+'-'+targetPos).attr('src');
    var tempAlt = $("#upload-resName-"+fieldSign+'-'+targetPos).attr('title');

    $("#upload-src-"+fieldSign+'-'+targetPos).attr('src', $("#upload-src-"+fieldSign+'-'+currentPos).attr('src'));
    $("#upload-src-"+fieldSign+'-'+currentPos).attr('src',tempShowImageHref);

    $("#upload-src-"+fieldSign+'-'+targetPos).attr('title', $("#upload-src-"+fieldSign+'-'+currentPos).attr('title'));
    $("#upload-src-"+fieldSign+'-'+currentPos).attr('title',tempAlt);

    changePosVal("upload-resName-", fieldSign, currentPos, targetPos);

    changePosVal("upload-resId-", fieldSign, currentPos, targetPos);

    changePosVal("upload-relatePath-", fieldSign, currentPos, targetPos);

    changePosVal("upload-desc-", fieldSign, currentPos, targetPos);


    //2022
    changePosVal("upload-title-", fieldSign, currentPos, targetPos);

    changePosVal("upload-link-", fieldSign, currentPos, targetPos);

    changePosVal("upload-width-", fieldSign, currentPos, targetPos);

    changePosVal("upload-height-", fieldSign, currentPos, targetPos);

    changePosText("upload-width-show-", fieldSign, currentPos, targetPos);

    changePosText("upload-height-show-", fieldSign, currentPos, targetPos);

    changePosText("upload-name-show-", fieldSign, currentPos, targetPos);

    changePosText("upload-size-show-", fieldSign, currentPos, targetPos);

    changePosText("upload-time-show-", fieldSign, currentPos, targetPos);

    changePosText("upload-wh-show-", fieldSign, currentPos, targetPos);

}

function changePosVal(idFlag, fieldSign, currentPos, targetPos)
{
    var temp = $("#"+idFlag+fieldSign+'-'+targetPos).val();
    $("#"+idFlag+fieldSign+'-'+targetPos).val($("#"+idFlag+fieldSign+'-'+currentPos).val());
    $("#"+idFlag+fieldSign+'-'+currentPos).val(temp);
}

function changePosText(idFlag, fieldSign, currentPos, targetPos)
{
    var temp = $("#"+idFlag+fieldSign+'-'+targetPos).text();
    $("#"+idFlag+fieldSign+'-'+targetPos).text($("#"+idFlag+fieldSign+'-'+currentPos).text());
    $("#"+idFlag+fieldSign+'-'+currentPos).text(temp);
}



function fnInitUploadImgGroup(accType, maxSize, classId)
{
    $(".upload-sys-group").each(function(){

        var fn = this.id.replace('upload-group-','');

        imgIndexArray[fn] = $('#group-list-'+fn).find('tr').size();

        //多文件列表示例
        var fileListView = $('#group-list-'+fn)
            ,uploadListIns = upload.render({
            elem: this
            ,url: basePath+'content/multiUpload.do?type=image&classId='+classId
            ,accept: 'file' //允许上传的文件类型
            ,acceptMime: accType
            ,size: 1024 * maxSize //最大允许上传的文件大小
            ,multiple: true
            ,auto: true
            //,bindAction: '#'
            ,before: function(obj){

                var size = 0;

                for(var x in this.files)
                {
                    size++;
                }

                if(size != 0)
                {
                    layer.load(); //上传loading
                }
            }
            ,choose: function(obj){

                $('#noimg-'+fn).hide();

                var files = this.files = obj.pushFile(); //将每次选择的文件追加到文件队列

                //读取本地文件
                obj.preview(function(index, file, result){

                    var order = imgIndexArray[fn];
                    // console.log('1:'+result);

                    orderIndexArray[index] = order;

                    var hp = getGroupHtml(order,fn, file.name, classId,'', 'upload-sys-group')

                    //从file中获取的数据替换
                    hp = replaceAll(hp,'${result}', result);
                    hp = replaceAll(hp,'${fileSize}', sizeToStr(file.size));
                    hp = replaceAll(hp,'${result}', result);

                    hp = replaceAll(hp,'${mw}', 0);
                    hp = replaceAll(hp,'${mh}', 0);



                    var tr = $([hp].join(''));

                    var cor = order;

                    imgIndexArray[fn] = (++order);



                    //单个重传
                    // tr.find('.test-upload-demo-reload').on('click', function(){
                    //
                    //  // obj.upload(index, file);
                    //
                    //
                    // });

                    //删除
                    tr.find('.test-upload-demo-delete').on('click', function(){
                        delete files[index]; //删除对应的文件
                        tr.remove();
                        if($("#group-list-"+fn).children("tr").length == 1)
                        {
                            $('#noimg-'+fn).show();
                        }

                        //修正高度
                        fixMW();

                        uploadListIns.config.elem.next()[0].value = ''; //清空 input file 值，以免删除后出现同名文件不可选
                    });

                    fileListView.append(tr);

                    //图集单图重传,必须id模式
                    console.log(fn+'-'+cor)
                    imgGroupIdReUpload(fn+'-'+cor, 'image/jpeg,image/png,image/jpg,image/bmg,image/gif', maxSize, classId);


                });
            }
            ,allDone: function(obj)
            { //当文件全部被提交后，才触发
                fixMW();
            }
            ,done: function(res, index, upload){
                layer.closeAll('loading'); //关闭loading

                var or = orderIndexArray[index];

                if(res.obj_0.resId > 0){ //上传成功
                    // var tr = fileListView.find('tr#upload-'+ or)
                    // ,tds = tr.children();
                    // tds.eq(2).html('<span style="color: #5FB878;">上传成功</span>');
                    // tds.eq(3).html(''); //清空操作

                    //console.log(JSON.stringify(res.obj_0))
                    $('#upload-resId-'+fn+ '-'+or).val(res.obj_0.resId);
                    $('#upload-relatePath-'+fn+ '-'+or).val(res.obj_0.relatePath);

                    $('#upload-width-show-'+fn+ '-'+or).text(res.obj_0.width);
                    $('#upload-height-show-'+fn+ '-'+or).text(res.obj_0.height);


                    //2022:关闭上传状态
                    $('#upload-status-'+fn+ '-'+or).hide();

                    $('#upload-width-'+fn+ '-'+or).val(res.obj_0.width);
                    $('#upload-height-'+fn+ '-'+or).val(res.obj_0.height);

                    //修正高度
                    fixMW();

                    return delete this.files[index]; //删除文件队列已经上传成功的文件
                }
                this.error(index, upload);
            }
            ,error: function(index, upload){
                var or = orderIndexArray[index];;

                layer.closeAll('loading'); //关闭loading
                // var tr = fileListView.find('tr#upload-'+ or)
                // ,tds = tr.children();
                // tds.eq(2).html('<span style="color: #FF5722;">上传失败</span>');
                //2022:上传失败
                $('#upload-status-'+fn+ '-'+or).html('<span style="color: #FF5722;">上传失败!</span>')

                //tds.eq(3).find('.test-upload-demo-reload').removeClass('layui-hide'); //显示重传

            }
        });


    })

}

//图集单图重传
function imgGroupReUploadEditMode( accType, imageMaxC, classId)
{
    $(".sgimgup").each(function(){

        var fn = this.id.replace('upload-id-','');

        imgGroupIdReUpload(fn,accType, imageMaxC, classId);
    })
}
//
// //图集内链
// function imgGroupLinkSelect()
// {
//     dropdown.render({
//         elem: '.upload-link-btn' //可绑定在任意元素中，此处以上述按钮为例
//         ,data: [{
//             title: '内容链接'
//             ,id: 100
//             ,href: "javascript:openIGSelectSiteAndPushContentDialog();"
//         },{
//             title: '栏目链接'
//             ,id: 101
//             ,href: 'https://www.layui.com/' //开启超链接
//             ,target: '_blank' //新窗口方式打开
//         }]
//         ,id: 'demo1'
//         //菜单被点击的事件
//         ,click: function(obj){
//             console.log(obj);
//             layer.msg('回调返回的参数已显示再控制台');
//         }
//     });
// }

//图集单图重传
function imgGroupIdReUpload( id, accType, imageMaxC, classId)
{
    let fn = id;

    let indexLoad;

    let uploadInner = layui.upload; //得到 upload 对象

    uploadInner.render({
        elem: '#upload-id-'+id
        ,url: basePath+'content/multiUpload.do?type=image&classId='+classId

        ,before: function(obj)
        {
            //this.data={'':''};
            indexLoad = layer.load(0, {
                shade: [0.1,'#fff'] //0.1透明度的白色背景
            });
        }
        ,choose: function(obj){

            console.log(fn)
        }
        ,error: function(index, upload){
            layer.closeAll('loading'); //关闭loading
        }
        ,done: function(res, index, upload){ //上传后的回调

            layer.close(indexLoad);

            //hideTips(fn);

            fnSetIGImage(fn, res);
        }
        ,accept: 'file' //允许上传的文件类型
        //,exts: 'jpg'

        ,acceptMime: accType
        ,size: 1024 * imageMaxC //最大允许上传的文件大小
        //,……
    })





}


//设置图集重传传的图像信息
function fnSetIGImage(fn, res)
{


    $('#upload-src-'+fn).attr('src', res.obj_0.imageUrl);

    var gn = res.obj_0.genName;
    var type = gn.substring(gn.lastIndexOf('.'), gn.length);

    $('#upload-src-'+fn).attr('src', res.obj_0.imageUrl);
    $('#upload-resName-'+fn).val(res.obj_0.imageName);
    $('#upload-resId-'+fn).val(res.obj_0.resId);
    $('#upload-relatePath-'+fn).val(res.obj_0.relatePath);
    $('#upload-width-'+fn).val(res.obj_0.width);
    $('#upload-height-'+fn).val(res.obj_0.height);

    $('#upload-name-show-'+fn).text(res.obj_0.imageName+type);
    $('#upload-width-show-'+fn).text(res.obj_0.width);
    $('#upload-height-show-'+fn).text(res.obj_0.height);

    var resObj = getSysRes(res.obj_0.resId);

    $('#upload-size-show-'+fn).text(sizeToStr(resObj.resSize))

}


////////////////附件集////////////////////

//初始化文件集
function fnInitUploadFileGroup(accType, maxSize, classId)
{
    $(".upload-sys-group-file").each(function(){

        var fn = this.id.replace('upload-group-','');

        imgIndexArray[fn] = $('#group-list-'+fn).find('tr').size();

        //多文件列表示例
        var fileListView = $('#group-list-'+fn)
            ,uploadListIns = upload.render({
            elem: this
            ,url: basePath+'content/multiUpload.do?type=file&classId='+classId
            ,accept: 'file' //允许上传的文件类型
            ,acceptMime: accType
            ,size: 1024 * maxSize //最大允许上传的文件大小
            ,multiple: true
            ,auto: true
            //,bindAction: '#'
            ,before: function(obj){

                var size = 0;

                for(var x in this.files)
                {
                    size++;
                }

                if(size != 0)
                {
                    layer.load(); //上传loading
                }
            }
            ,choose: function(obj){

                $('#noimg-'+fn).hide();

                var files = this.files = obj.pushFile(); //将每次选择的文件追加到文件队列

                //读取本地文件
                obj.preview(function(index, file, result){

                    var order = imgIndexArray[fn];
                    // console.log('1:'+result);

                    orderIndexArray[index] = order;

                    var hp = getGroupHtml(order,fn, file.name, classId,'', 'upload-sys-group-file')

                    hp = replaceAll(hp,'${result}', result);
                    hp = replaceAll(hp,'${fileSize}', sizeToStr(file.size));
                    hp = replaceAll(hp,'${result}', result);

                    hp = replaceAll(hp,'${mw}', 0);
                    hp = replaceAll(hp,'${mh}', 0);



                    var tr = $([hp].join(''));

                    var cor = order;

                    imgIndexArray[fn] = (++order);


                    //删除
                    tr.find('.test-upload-demo-delete').on('click', function(){
                        delete files[index]; //删除对应的文件
                        tr.remove();
                        if($("#group-list-"+fn).children("tr").length == 1)
                        {
                            $('#noimg-'+fn).show();
                        }

                        //修正高度
                        fixMW();

                        uploadListIns.config.elem.next()[0].value = ''; //清空 input file 值，以免删除后出现同名文件不可选
                    });

                    fileListView.append(tr);

                    //图集单图重传,必须id模式

                    fileGroupIdReUpload(fn+'-'+cor, accType, maxSize, classId);

                });
            }
            ,done: function(res, index, upload){
                layer.closeAll('loading'); //关闭loading

                var or = orderIndexArray[index];

                if(res.obj_0.resId > 0){ //上传成功
                    // var tr = fileListView.find('tr#upload-'+ or)
                    // ,tds = tr.children();
                    // tds.eq(2).html('<span style="color: #5FB878;">上传成功</span>');
                    // tds.eq(3).html(''); //清空操作

                    //console.log(JSON.stringify(res.obj_0))
                    $('#upload-resId-'+fn+ '-'+or).val(res.obj_0.resId);
                    $('#upload-relatePath-'+fn+ '-'+or).val(res.obj_0.relatePath);

                    var gn = res.obj_0.genName;

                    $('#upload-resName-'+fn+ '-'+or).val(res.obj_0.resName+gn.substring(gn.lastIndexOf('.'), gn.length));

                    $('#upload-width-show-'+fn+ '-'+or).text(res.obj_0.width);
                    $('#upload-height-show-'+fn+ '-'+or).text(res.obj_0.height);


                    //2022:关闭上传状态
                    $('#upload-status-'+fn+ '-'+or).hide();

                    $('#upload-width-'+fn+ '-'+or).val(res.obj_0.width);
                    $('#upload-height-'+fn+ '-'+or).val(res.obj_0.height);


                    //修正高度
                    fixMW();

                    return delete this.files[index]; //删除文件队列已经上传成功的文件
                }
                this.error(index, upload);
            }
            ,allDone: function(obj){ //当文件全部被提交后，才触发
                fixMW();
            }
            ,error: function(index, upload){
                var or = orderIndexArray[index];;

                layer.closeAll('loading'); //关闭loading
                // var tr = fileListView.find('tr#upload-'+ or)
                // ,tds = tr.children();
                // tds.eq(2).html('<span style="color: #FF5722;">上传失败</span>');
                //2022:上传失败
                $('#upload-status-'+fn+ '-'+or).html('<span style="color: #FF5722;">上传失败!</span>')

                //tds.eq(3).find('.test-upload-demo-reload').removeClass('layui-hide'); //显示重传

            }
        });


    })

}


//文件单文件重传
function fileGroupReUploadEditMode( accType, imageMaxC, classId)
{
    $(".sgfileup").each(function(){

        var fn = this.id.replace('upload-id-','');

        fileGroupIdReUpload(fn,accType, imageMaxC, classId);
    })
}

//文件集单文件重传
function fileGroupIdReUpload( id, accType, imageMaxC, classId)
{
    let fn = id;

    let indexLoad;

    let uploadInner = layui.upload; //得到 upload 对象

    uploadInner.render({
        elem: '#upload-id-'+id
        ,url: basePath+'content/multiUpload.do?type=file&classId='+classId

        ,before: function(obj)
        {
            //this.data={'':''};
            indexLoad = layer.load(0, {
                shade: [0.1,'#fff'] //0.1透明度的白色背景
            });
        }
        ,choose: function(obj){

            console.log(fn)
        }
        ,error: function(index, upload){
            layer.closeAll('loading'); //关闭loading
        }
        ,done: function(res, index, upload){ //上传后的回调

            layer.close(indexLoad);

            //hideTips(fn);

            fnSetIGFile(fn, res);
        }
        ,accept: 'file' //允许上传的文件类型
        //,exts: 'jpg'

        ,acceptMime: accType
        ,size: 1024 * imageMaxC //最大允许上传的文件大小
        //,……
    })
}

//设置文件集重传传的信息
function fnSetIGFile(fn, res)
{
    //console.log(res)

    $('#upload-src-'+fn).attr('src', res.obj_0.imageUrl);

    var gn = res.obj_0.genName;
    var type = gn.substring(gn.lastIndexOf('.'), gn.length);

    $('#upload-resName-'+fn).val(res.obj_0.resName);
    $('#upload-resId-'+fn).val(res.obj_0.resId);
    $('#upload-relatePath-'+fn).val(res.obj_0.relatePath);


    $('#upload-name-show-'+fn).text(res.obj_0.resName+type);

    $('#upload-size-show-'+fn).text(sizeToStr(res.obj_0.length))

}





////////////////媒体集////////////////////

//初始化媒体集
function fnInitUploadMediaGroup(accType, maxSize, classId)
{
    $(".upload-sys-group-media").each(function(){

        var fn = this.id.replace('upload-group-','');

        imgIndexArray[fn] = $('#group-list-'+fn).find('tr').size();

        //多文件列表示例
        var fileListView = $('#group-list-'+fn)
            ,uploadListIns = upload.render({
            elem: this
            ,url: basePath+'content/multiUpload.do?type=media&classId='+classId
            ,accept: 'file' //允许上传的文件类型
            ,acceptMime: accType
            ,size: 1024 * maxSize //最大允许上传的文件大小
            ,multiple: true
            ,auto: true
            //,bindAction: '#'
            ,before: function(obj){

                var size = 0;

                for(var x in this.files)
                {
                    size++;
                }

                if(size != 0)
                {
                    layer.load(); //上传loading
                }
            }
            ,choose: function(obj){

                $('#noimg-'+fn).hide();

                var files = this.files = obj.pushFile(); //将每次选择的文件追加到文件队列

                //读取本地文件
                obj.preview(function(index, file, result){

                    var order = imgIndexArray[fn];
                    // console.log('1:'+result);

                    orderIndexArray[index] = order;

                    var hp = getGroupHtml(order,fn, file.name, classId,'', 'upload-sys-group-media')

                    hp = replaceAll(hp,'${result}', result);
                    hp = replaceAll(hp,'${fileSize}', sizeToStr(file.size));
                    hp = replaceAll(hp,'${result}', result);

                    hp = replaceAll(hp,'${mw}', 0);
                    hp = replaceAll(hp,'${mh}', 0);



                    var tr = $([hp].join(''));

                    var cor = order;

                    imgIndexArray[fn] = (++order);


                    //删除
                    tr.find('.test-upload-demo-delete').on('click', function(){
                        delete files[index]; //删除对应的文件
                        tr.remove();
                        if($("#group-list-"+fn).children("tr").length == 1)
                        {
                            $('#noimg-'+fn).show();
                        }

                        //修正高度
                        fixMW();

                        uploadListIns.config.elem.next()[0].value = ''; //清空 input file 值，以免删除后出现同名文件不可选
                    });
                    console.log(fileListView)
                    fileListView.append(tr);

                    //重传,必须id模式

                    mediaGroupIdReUpload(fn+'-'+cor, accType, maxSize, classId);

                });
            }
            ,done: function(res, index, upload){
                layer.closeAll('loading'); //关闭loading

                var or = orderIndexArray[index];

                if(res.obj_0.resId > 0){ //上传成功
                    // var tr = fileListView.find('tr#upload-'+ or)
                    // ,tds = tr.children();
                    // tds.eq(2).html('<span style="color: #5FB878;">上传成功</span>');
                    // tds.eq(3).html(''); //清空操作

                    //console.log(JSON.stringify(res.obj_0))
                    $('#upload-resId-'+fn+ '-'+or).val(res.obj_0.resId);
                    $('#upload-relatePath-'+fn+ '-'+or).val(res.obj_0.relatePath);

                    $('#upload-src-'+fn+ '-'+or).attr('href', res.obj_0.fileUrl);

                    var gn = res.obj_0.genName;

                    $('#upload-resName-'+fn+ '-'+or).val(res.obj_0.resName+gn.substring(gn.lastIndexOf('.'), gn.length));

                    $('#upload-time-show-'+fn+ '-'+or).text(res.obj_0.videoT+'秒');
                    $('#upload-wh-show-'+fn+ '-'+or).text(res.obj_0.videoW+' x '+res.obj_0.videoH);


                    //2022:关闭上传状态
                    $('#upload-status-'+fn+ '-'+or).hide();

                    $('#upload-width-'+fn+ '-'+or).val(res.obj_0.width);
                    $('#upload-height-'+fn+ '-'+or).val(res.obj_0.height);


                    //修正高度
                    fixMW();

                    return delete this.files[index]; //删除文件队列已经上传成功的文件
                }
                this.error(index, upload);
            }
            ,allDone: function(obj){ //当文件全部被提交后，才触发
                fixMW();
            }
            ,error: function(index, upload){
                var or = orderIndexArray[index];;

                layer.closeAll('loading'); //关闭loading
                // var tr = fileListView.find('tr#upload-'+ or)
                // ,tds = tr.children();
                // tds.eq(2).html('<span style="color: #FF5722;">上传失败</span>');
                //2022:上传失败
                $('#upload-status-'+fn+ '-'+or).html('<span style="color: #FF5722;">上传失败!</span>')

                //tds.eq(3).find('.test-upload-demo-reload').removeClass('layui-hide'); //显示重传

            }
        });


    })

}


//媒体单文件重传
function mediaGroupReUploadEditMode( accType, imageMaxC, classId)
{
    $(".sgmediaup").each(function(){

        var fn = this.id.replace('upload-id-','');

        mediaGroupIdReUpload(fn,accType, imageMaxC, classId);
    })
}

//媒体集单文件重传
function mediaGroupIdReUpload( id, accType, imageMaxC, classId)
{
    let fn = id;

    let indexLoad;

    let uploadInner = layui.upload; //得到 upload 对象

    uploadInner.render({
        elem: '#upload-id-'+id
        ,url: basePath+'content/multiUpload.do?type=media&classId='+classId

        ,before: function(obj)
        {
            //this.data={'':''};
            indexLoad = layer.load(0, {
                shade: [0.1,'#fff'] //0.1透明度的白色背景
            });
        }
        ,choose: function(obj){

            console.log(fn)
        }
        ,error: function(index, upload){
            layer.closeAll('loading'); //关闭loading
        }
        ,done: function(res, index, upload){ //上传后的回调

            layer.close(indexLoad);

            //hideTips(fn);

            fnSetIGMedia(fn, res);
        }
        ,accept: 'file' //允许上传的文件类型
        //,exts: 'jpg'

        ,acceptMime: accType
        ,size: 1024 * imageMaxC //最大允许上传的文件大小
        //,……
    })
}

//设置媒体集重传传的信息
function fnSetIGMedia(fn, res)
{
    //console.log(res)

    $('#upload-src-'+fn).attr('href', res.obj_0.fileUrl);

    var gn = res.obj_0.genName;
    var type = gn.substring(gn.lastIndexOf('.'), gn.length);

    $('#upload-resName-'+fn).val(res.obj_0.resName);
    $('#upload-resId-'+fn).val(res.obj_0.resId);
    $('#upload-relatePath-'+fn).val(res.obj_0.relatePath);


    $('#upload-name-show-'+fn).text(res.obj_0.resName+type);

    $('#upload-size-show-'+fn).text(sizeToStr(res.obj_0.length))


    $('#upload-time-show-'+fn).text(res.obj_0.videoT+'秒');
    $('#upload-wh-show-'+fn).text(res.obj_0.videoW+' x '+res.obj_0.videoH);

}

//媒体集从媒体库选择
function importMekGroup(fn)
{
    var obj = document.getElementById("classId");

    var classId = '';
    if(obj != null)
    {
        classId = obj.value;
    }

    parent.layer.open({
        type: 2
        ,title: '媒体库'

        ,shadeClose: false
        ,scrollbar: false
        ,shade: [0.3, '#393D49']
        ,maxmin: false
        ,content:  basePath+'core/content/media-res-for-group.thtml?classId='+classId+'&valId='+fn+'&mainfn='+window.name
        ,area: ['700px', '620px']


    });

}

function getGroupHtml(order, fn, fileName, classId, editMode, type)
{
    var url = basePath+"content/getMGFHtml.do?editMode="+editMode+'&type='+type;

    var data = 'classId='+classId+'&fn='+fn+'&order='+order+'&fileName='+encodeURIComponent(encodeData(fileName));

    var res = '';

    $.ajax({
        type: "GET",
        url: url,
        data:data,
        async:false,
        success: function(msg)
        {

            res = msg

        }
    });

    return res;
}

//管理员获取文件资源信息
function getSysRes(id)
{
    var url = basePath+"content/sysGetResInfo.do?resId="+id;


    var res = '';

    $.ajax({
        type: "GET",
        url: url,
        data:'',
        async:false,
        success: function(msg)
        {

            res = msg

        }
    });

    return res;
}

function sizeToStr(size) {
    var data = "";
    if (size < 0.1 * 1024) { //如果小于0.1KB转化成B
        data = size.toFixed(2) + "B";
    } else if (size < 0.1 * 1024 * 1024) {//如果小于0.1MB转化成KB
        data = (size / 1024).toFixed(2) + "KB";
    } else if (size < 0.1 * 1024 * 1024 * 1024) { //如果小于0.1GB转化成MB
        data = (size / (1024 * 1024)).toFixed(2) + "MB";
    } else { //其他转化成GB
        data = (size / (1024 * 1024 * 1024)).toFixed(2) + "GB";
    }
    var sizestr = data + "";
    var len = sizestr.indexOf("\.");
    var dec = sizestr.substr(len + 1, 2);
    if (dec == "00") {//当小数点后为00时 去掉小数部分
        return sizestr.substring(0, len) + sizestr.substr(len + 3, 2);
    }
    return sizestr;
}


function deleteAllImg(fn)
{

    var noImg = '<tr id="noimg-'+fn+'" height="50" style="background-color: transparent;">'+
        '<td   colspan="6">'+
        '<center>'+
        '<i class="layui-icon layuiadmin-button-btn" style="font-size: 20px; ">&#xe60b;</i>  没有图片'+
        '</center>'+
        '</td>'+
        '</tr>';

    parent.layer.confirm('您确认删除所有文件吗？', {
        btn: ['确认','取消'] //按钮
        // ,offset: [msgH+'px', msgW+'px']
        ,icon:7
    }, function()
    {

        parent.layer.closeAll('dialog');

        $('#group-list-'+fn).find('tr').remove();
        $('#group-list-'+fn).append(noImg);




    }, function(index)
    {

        //layer.close(index);
    });

    //修正高度
    fixMW();
}

function fnInitUploadFile( accType, maxSize, classId )
{
    $(".file-upload").each(function(){

        var fn = this.id.replace('file-upload-','');

        upload.render({
            elem: this
            ,url: basePath+'content/multiUpload.do?type=file&classId='+classId

            ,before: function(obj)
            {
                layer.load();
            }

            ,error: function(index, upload){
                layer.closeAll('loading'); //关闭loading
            }
            ,done: function(res, index, upload){ //上传后的回调

                layer.closeAll('loading'); //关闭loading

                hideTips(fn);

                var gn = res.obj_0.genName;

                $('#'+fn+'_sys_cms_file_info').val(res.obj_0.resName+gn.substring(gn.lastIndexOf('.'), gn.length));

                $('#'+fn).val(res.obj_0.resId);

            }
            ,accept: 'file' //允许上传的文件类型
            //,exts: 'jpg'

            ,acceptMime: accType
            ,size: 1024 * maxSize //最大允许上传的文件大小
            //,……
        })
    })
}



function fnInitUploadImage( accType, maxSize, classId)
{
    $(".upload-sys-img").each(function(){

        var fn = '';

        if(this.id.indexOf('-upload-drag') != -1)
        {
            fn = this.id.replace('-upload-drag','');
        }
        else
        {
            fn = this.id.replace('upload-img-','');
        }

        var dm,mw,mh;

        //默认需干预图片规格处理
        if('contentImage' == fn)
        {
            dm = ('0' != classCoDM) ? classCoDM : siteCoDM;
            mw = ('0' != classCoW) ? classCoW : siteCoW;
            mh = ('0' != classCoH) ? classCoH : siteCoH;
        }
        else if('classImage' == fn)
        {
            dm = ('0' != classLiDM) ? classLiDM : siteLiDM;
            mw = ('0' != classLiW) ? classLiW : siteLiW;
            mh = ('0' != classLiH) ? classLiH : siteLiH;
        }
        else if('channelImage' == fn)
        {
            dm = ('0' != classClDM) ? classClDM : siteClDM;
            mw = ('0' != classClW) ? classClW : siteClW;
            mh = ('0' != classClH) ? classClH : siteClH;
        }
        else if('homeImage' == fn)
        {
            dm = ('0' != classHoDM) ? classHoDM : siteHoDM;
            mw = ('0' != classHoW) ? classHoW : siteHoW;
            mh = ('0' != classHoH) ? classHoH : siteHoH;
        }


        //创建一个上传组件
        var indexLoad;

        var upload = layui.upload; //得到 upload 对象

        upload.render({
            elem: this
            ,url: basePath+'content/multiUpload.do?type=image&classId='+classId+'&disposeMode='+dm+'&maxWidth='+mw+'&maxHeight='+mh

            ,before: function(obj)
            {
                //this.data={'':''};
                indexLoad = layer.load(0, {
                    shade: [0.1,'#fff'] //0.1透明度的白色背景
                });
            }
            ,error: function(index, upload){
                layer.closeAll('loading'); //关闭loading
            }
            ,done: function(res, index, upload){ //上传后的回调

                layer.close(indexLoad);

                hideTips(fn);

                fnSetImage(fn, res);
            }
            ,accept: 'file' //允许上传的文件类型
            //,exts: 'jpg'

            ,acceptMime: accType
            ,size: 1024 * maxSize //最大允许上传的文件大小
            //,……
        })

    })

}




function fnInitSingeUploadImage( accType, maxSize, idv, classId)
{
    $("#upload-img-"+idv).each(function(){

        var fn = idv;
        //创建一个上传组件
        var indexLoad;

        var upload = layui.upload; //得到 upload 对象

        upload.render({
            elem: this
            ,url: basePath+'content/multiUpload.do?type=image&classId='+classId

            ,before: function(obj)
            {
                //this.data={'':''};
                indexLoad = layer.load(0, {
                    shade: [0.1,'#fff'] //0.1透明度的白色背景
                });
            }
            ,error: function(index, upload){
                layer.closeAll('loading'); //关闭loading
            }
            ,done: function(res, index, upload){ //上传后的回调

                layer.close(indexLoad);

                hideTips(fn);

                fnSetImage(fn, res);
            }
            ,accept: 'file' //允许上传的文件类型
            //,exts: 'jpg'

            ,acceptMime: accType
            ,size: 1024 * maxSize //最大允许上传的文件大小
            //,……
        })

    })

}


function fnInitUploadMedia( accType, maxSize, classId)
{
    $(".upload-sys-media").each(function(){

        var fn = this.id.replace('upload-media-','');
        //创建一个上传组件
        var indexLoad;

        var at = accType;

        var upload = layui.upload; //得到 upload 对象

        upload.render({
            elem: this
            ,url: basePath+'content/multiUpload.do?type=media&classId='+classId

            ,before: function(obj)
            {
                //this.data={'':''};
                indexLoad = layer.load(0, {
                    shade: [0.1,'#fff'] //0.1透明度的白色背景
                });
            }
            ,error: function(index, upload){
                layer.closeAll('loading'); //关闭loading
            }
            ,done: function(res, index, upload){ //上传后的回调

                layer.close(indexLoad);

                hideTips(fn);

                fnSetMedia(fn, res, at);
            }
            ,accept: 'file' //允许上传的文件类型
            //,exts: 'jpg'

            ,acceptMime: accType
            ,size: 1024 * maxSize //最大允许上传的文件大小
            //,……
        })

    })

}


//设置上传的图像信息
function fnSetImage(imageValId, jsonObj)
{

    var imageSrcId = imageValId+'CmsSysImgShow';


    if(imageValId == 'contentImage' || imageValId == 'classImage' || imageValId == 'channelImage' || imageValId == 'homeImage')
    {
        $('#'+imageValId+'CmsSysImgShow').show();
        $('#'+imageValId+'-upload-drag').hide();

        window.document.getElementById(imageSrcId).title = jsonObj.obj_0.width + ' x ' + jsonObj.obj_0.height;
    }
    else
    {
        window.document.getElementById(imageSrcId).style.opacity = 1;
        window.document.getElementById(imageSrcId).width = 90;
        window.document.getElementById(imageSrcId).height = 67;
    }

    window.document.getElementById(imageValId+'CmsSysShowSingleImage').href = jsonObj.obj_0.imageUrl;
    window.document.getElementById(imageSrcId).src=jsonObj.obj_0.resizeImageUrl;


    //用于cms处理
    window.document.getElementById(imageValId).value= jsonObj.obj_0.resId;

    window.document.getElementById(imageValId+'CmsSysImgWidth').value= jsonObj.obj_0.width;
    window.document.getElementById(imageValId+'CmsSysImgHeight').value= jsonObj.obj_0.height;

}



//设置上传的媒体信息
function fnSetMedia(mediaSrcId, jsonObj, at)
{
    //console.log(JSON.stringify(jsonObj))

    var type = jsonObj.obj_0.genName.substring(jsonObj.obj_0.genName.indexOf('.')+1,jsonObj.obj_0.genName.length);
    //用于显示
    if('audio/*' == at)
    {
        document.getElementById(mediaSrcId+'_sys_cms_iframe').src = basePath+'core/content/upload-audio-module.thtml?fileUrl='+jsonObj.obj_0.fileUrl+'&autoStart=true';
    }
    else
    {
        document.getElementById(mediaSrcId+'_sys_cms_iframe').src = basePath+'core/content/upload-video-module.thtml?fileUrl='+jsonObj.obj_0.fileUrl+'&autoStart=true';

    }

    //用于显示
    document.getElementById(mediaSrcId+'_sys_cms_media_showtime').value = jsonObj.obj_0.videoT;

    if(document.getElementById(mediaSrcId+'_sys_cms_media_width') !=null )
    {
        document.getElementById(mediaSrcId+'_sys_cms_media_width').value = jsonObj.obj_0.videoW;
    }

    if(document.getElementById(mediaSrcId+'_sys_cms_media_height') !=null )
    {
        document.getElementById(mediaSrcId+'_sys_cms_media_height').value = jsonObj.obj_0.videoH;
    }


    $('#'+mediaSrcId+'_sys_cms_media_name').val(jsonObj.obj_0.resName+'.'+type);


    $('#'+mediaSrcId+'_sys_cms_media_type').val(type);

    //每次上传新的视频需要将以前截取的图片信息置空
    $('#'+mediaSrcId+'_sys_cms_media_cover_src').val('');
    $('#'+mediaSrcId+'_sys_cms_media_cover_w').val('');
    $('#'+mediaSrcId+'_sys_cms_media_cover_h').val('');
    $('#'+mediaSrcId+'_sys_cms_media_cover_n').val('');

    //用于cms处理
    $('#'+mediaSrcId).val(jsonObj.obj_0.resId);
}






//图象上传
function showModuleImageDialog(srcId,valId,tw,th,dm,needMark)
{
    if(typeof(dialogApiId) == 'undefined' || dialogApiId == '')
    {
        var targetClass = $('#classId').val();

        var classId = -9999;
        if(targetClass != 'undefined' && targetClass != null)
        {
            classId = targetClass;
        }

        if(typeof(W) == 'undefined' || W == '')
        {
            var upload = layui.upload; //得到 upload 对象

            //创建一个上传组件
            upload.render({
                elem: '#'+valId
                ,url: ''
                ,done: function(res, index, upload){ //上传后的回调

                }
                ,accept: 'file' //允许上传的文件类型
                //,size: 50 //最大允许上传的文件大小
                //,……
            })


        }
        else
        {

            W.$.dialog({
                id:'oud',
                title :'图片',
                width: '520px',
                height: '375px',
                parent: api,
                lock: true,
                max: false,
                min: false,
                resize: false,
                content: 'url:'+basePath+'core/content/dialog/ImageUploadModule.jsp?classId='+classId+'&srcId='+srcId+'&valId='+valId+'&tw='+tw+'&th='+th+'&dm='+dm+'&mark='+needMark+'&apiId=main_content&isDialogMode=true'
            });
        }
    }
    else
    {
        var targetClass = W.document.getElementById("classId");

        var classId = -9999;

        if(targetClass != 'undefined' && targetClass != null)
        {
            classId = W.document.getElementById("classId").value;
        }
        else
        {
            //取当前页

            targetClass =  document.getElementById("classId");

            if(targetClass != 'undefined' && targetClass != null)
            {
                classId = document.getElementById("classId").value;
            }
        }


        targetClass = W.document.getElementById("classChildMode");

        var classChildMode = "false";

        if(targetClass != 'undefined' && targetClass != null)
        {
            classChildMode = W.document.getElementById("classChildMode").value;
        }
        else
        {
            //取当前页

            targetClass =  document.getElementById("classChildMode");

            if(targetClass != 'undefined' && targetClass != null)
            {
                classChildMode = document.getElementById("classChildMode").value;
            }
        }


        targetClass = W.document.getElementById("rootClassId");

        var rootClassId = -9999;

        if(targetClass != 'undefined' && targetClass != null)
        {
            rootClassId = W.document.getElementById("rootClassId").value;
        }
        else
        {
            //取当前页

            targetClass =  document.getElementById("rootClassId");

            if(targetClass != 'undefined' && targetClass != null)
            {
                rootClassId = document.getElementById("rootClassId").value;
            }
        }

        if('true' == classChildMode)
        {
            classId = rootClassId;
        }



        W.$.dialog({
            id:'oud',
            title :'图片',
            width: '520px',
            height: '375px',
            parent:api,
            lock: true,
            max: false,
            min: false,
            resize: false,
            content: 'url:'+basePath+'core/content/dialog/ImageUploadModule.jsp?classId='+classId+'&classChildMode='+classChildMode+'&srcId='+srcId+'&valId='+valId+'&tw='+tw+'&th='+th+'&dm='+dm+'&mark='+needMark+'&apiId='+dialogApiId+'&isDialogMode=true'
        });
    }
}


//图象上传,多窗口模式
function showModuleImageDialogForDialog(srcId,valId,tw,th,dm,needMark,apiId)
{
    var targetClass = W.document.getElementById("classId");

    var classId = -9999;

    if(targetClass != 'undefined' && targetClass != null)
    {
        classId = W.document.getElementById("classId").value;
    }

    W.$.dialog({
        id:'oud',
        title :'图片',
        width: '520px',
        height: '375px',
        parent:api,
        lock: true,
        max: false,
        min: false,
        resize: false,
        content: 'url:'+basePath+'core/content/dialog/ImageUploadModule.jsp?classId='+classId+'&srcId='+srcId+'&valId='+valId+'&tw='+tw+'&th='+th+'&dm='+dm+'&mark='+needMark+'&apiId='+apiId+'&isDialogMode=true'
    });

}

//单图片模式,图集模式
function showEditSingleImageDialog(fieldSign, imageOrder)
{
    var tw = $('#'+fieldSign+'CmsSysMaxWidth').val();
    var th = $('#'+fieldSign+'CmsSysMaxHeight').val();
    var dm = $('#'+fieldSign+'CmsSysDisposeMode').val();

    var mark = $('#'+fieldSign+'CmsSysNeedMark').attr('checked');

    var targetClass = $('#classId').val();

    var classId = -9999;
    if(targetClass != 'undefined' && targetClass != null)
    {
        classId = targetClass;
    }

    if('checked' == mark)
    {
        mark = 1;
    }
    else
    {
        mark = 0;
    }

    if(typeof(W) == 'undefined' || W == '')
    {
        $.dialog({
            id:'osid',
            title :'将替换当前图片 - '+$('#'+fieldSign+'-photoName-'+imageOrder).val(),
            width: '520px',
            height: '375px',
            lock: true,
            max: false,
            min: false,
            resize: false,
            content: 'url:'+basePath+'core/content/dialog/ImageUploadModule.jsp?resId='+document.getElementById(fieldSign+'-resId-'+imageOrder).value+'&photoName='+document.getElementById(fieldSign+'-photoName-'+imageOrder).value+'&imageOrder='+imageOrder+'&tw='+tw+'&th='+th+'&dm='+dm+'&mark='+mark+'&classId='+classId+'&fieldSign='+fieldSign+'&groupEditMode=true'
        });
    }
    else
    {
        W.$.dialog({
            id:'osid',
            title :'将替换当前图片 - '+$('#'+fieldSign+'-photoName-'+imageOrder).val(),
            width: '520px',
            height: '375px',
            parent:api,
            lock: true,
            max: false,
            min: false,
            resize: false,
            content: 'url:'+basePath+'core/content/dialog/ImageUploadModule.jsp?resId='+document.getElementById(fieldSign+'-resId-'+imageOrder).value+'&photoName='+document.getElementById(fieldSign+'-photoName-'+imageOrder).value+'&imageOrder='+imageOrder+'&tw='+tw+'&th='+th+'&dm='+dm+'&mark='+mark+'&classId='+classId+'&fieldSign='+fieldSign+'&groupEditMode=true&apiId=main_content&isDialogMode=true'
        });

    }


}


//图象上传,图集模式
function showModuleImageGroupDialog(fieldSign)
{
    var targetClass = document.getElementById("classId");

    var classId = -9999;
    if(targetClass != 'undefined' && targetClass != null)
    {
        classId = document.getElementById("classId").value;
    }

    var mw = document.getElementById(fieldSign+"CmsSysMaxWidth").value;

    var mh = document.getElementById(fieldSign+"CmsSysMaxHeight").value;

    var dw = document.getElementById(fieldSign+"CmsSysDisposeMode").value;


    if(typeof(W) == 'undefined' || W == '')
    {
        $.dialog({
            id:'oigud',
            title :'批量上传图片',
            width: '570px',
            height: '240px',
            lock: true,
            max: false,
            min: false,
            resize: false,
            content: 'url:'+basePath+'core/content/dialog/ImageGroupUploadModule.jsp?classId='+classId+'&fieldSign='+fieldSign+'&mw='+mw+'&mh='+mh+'&dw='+dw
        });
    }
    else
    {
        W.$.dialog({
            id:'oigud',
            title :'批量上传图片',
            width: '570px',
            height: '240px',
            parent:api,
            lock: true,
            max: false,
            min: false,
            resize: false,
            content: 'url:'+basePath+'core/content/dialog/ImageGroupUploadModule.jsp?classId='+classId+'&fieldSign='+fieldSign+'&mw='+mw+'&mh='+mh+'&dw='+dw+'&apiId=main_content'
        });
    }


}





//媒体上传
function showModuleMediaDialog(srcId,valId)
{
    if(typeof(W) == 'undefined' || W == '')
    {
        $.dialog({
            id:'smmd',
            title :'多媒体',
            width: '520px',
            height: '415px',
            lock: true,
            max: false,
            min: false,
            resize: false,
            content: 'url:'+basePath+'core/content/dialog/MediaUploadModule.jsp?classId='+document.getElementById("classId").value+'&srcId='+srcId+'&valId='+valId
        });
    }
    else
    {
        W.$.dialog({
            id:'smmd',
            title :'多媒体',
            width: '520px',
            height: '415px',
            parent:api,
            lock: true,
            max: false,
            min: false,
            resize: false,
            content: 'url:'+basePath+'core/content/dialog/MediaUploadModule.jsp?classId='+document.getElementById("classId").value+'&srcId='+srcId+'&valId='+valId+'&dialog=true&api=main_content'
        });

    }

}

//文件上传
function showModuleFileDialog(srcId,valId)
{
    if(typeof(dialogApiId) == 'undefined' || dialogApiId == '')
    {
        var targetClass = $('#classId').val();

        var classId = -9999;
        if(targetClass != 'undefined' && targetClass != null)
        {
            classId = targetClass;
        }

        if(typeof(W) == 'undefined' || W == '')
        {
            $.dialog({
                id:'smfd',
                title :'文件',
                width: '520px',
                height: '415px',
                lock: true,
                max: false,
                min: false,
                resize: false,
                content: 'url:'+basePath+'core/content/dialog/FileUploadModule.jsp?classId='+classId+'&srcId='+srcId+'&valId='+valId
            });
        }
        else
        {
            W.$.dialog({
                id:'smfd',
                title :'文件',
                width: '520px',
                height: '415px',
                parent:api,
                lock: true,
                max: false,
                min: false,
                resize: false,
                content: 'url:'+basePath+'core/content/dialog/FileUploadModule.jsp?classId='+classId+'&srcId='+srcId+'&valId='+valId+'&apiId=main_content&isDialogMode=true'
            });
        }



    }
    else
    {
        var targetClass = W.document.getElementById("classId");

        var classId = -9999;

        if(targetClass != 'undefined' && targetClass != null)
        {
            classId = W.document.getElementById("classId").value;
        }

        W.$.dialog({
            id:'smfd',
            title :'文件',
            width: '520px',
            height: '375px',
            parent:api,
            lock: true,
            max: false,
            min: false,
            resize: false,
            content: 'url:'+basePath+'core/content/dialog/FileUploadModule.jsp?classId='+classId+'&srcId='+srcId+'&valId='+valId+'&apiId='+dialogApiId+'&isDialogMode=true'
        });
    }
}



function resizeImage(img)
{
    var w = img.width;
    var h = img.height;

    var newWH = checkSize(w,h,120,160);

    img.width = newWH[0];
    img.height = newWH[1];
}

function checkSize(width, height, maxWidth, maxHeight)
{
    if (width && height)
    {
        if ((width / height) > (maxWidth / maxHeight))
        {
            if (width > maxWidth)
            {
                height = height * maxWidth / width;
                width = maxWidth;
            }
        }
        else
        {
            if (height > maxHeight)
            {
                width = width * maxHeight / height;
                height = maxHeight;
            }
        }

        return [ Math.round(width), Math.round(height) ];
    }
}


function insertVideoPlayerForIE(fileUrl,autoStart,cover)
{
    var str = '';

    if(fileUrl.lastIndexOf('.rm') != -1)
    {
        str +='<object id="rep1" classid="clsid:CFCDAA03-8BE4-11cf-B84B-0020AFBBCCFA" width=754 height=320>';
        str +='<param name="_ExtentX" value="11298">';
        str +='<param name="_ExtentY" value="7938">';
        str +='<param name="AUTOSTART" value="0">';
        str +='<param name="SHUFFLE" value="0">';
        str +='<param name="PREFETCH" value="0">';
        str +='<param name="NOLABELS" value="-1">';
        str +='<param name="SRC" value="'+fileUrl+'";>';
        str +='<param name="CONTROLS" value="Imagewindow">';
        str +='<param name="CONSOLE" value="clip1">';
        str +='<param name="LOOP" value="1">';
        str +='<param name="NUMLOOP" value="0">';
        str +='<param name="CENTER" value="0">';
        str +='<param name="MAINTAINASPECT" value="1">';
        str +='<param name="BACKGROUNDCOLOR" value="#000000">';
        str +='<param name="wmode" value="transparent">';
        str +='</object>';
        str +='<br />';
        str +='<object id="rep2" classid="clsid:CFCDAA03-8BE4-11cf-B84B-0020AFBBCCFA" width=754 height=320>';
        str +='<param name="_ExtentX" value="11298">';
        str +='<param name="_ExtentY" value="794">';
        str +='<param name="AUTOSTART" value="0">';
        str +='<param name="SHUFFLE" value="0">';
        str +='<param name="PREFETCH" value="0">';
        str +='<param name="NOLABELS" value="-1">';
        str +='<param name="SRC" value="'+fileUrl+'";>';
        str +='<param name="CONTROLS" value="ControlPanel">';
        str +='<param name="CONSOLE" value="clip1">';
        str +='<param name="LOOP" value="1">';
        str +='<param name="NUMLOOP" value="0">';
        str +='<param name="CENTER" value="0">';
        str +='<param name="MAINTAINASPECT" value="1">';
        str +='<param name="BACKGROUNDCOLOR" value="#000000">';
        str +='<param name="wmode" value="transparent">';
        str +='</object>';
    }
    else if(fileUrl.lastIndexOf('.wmv') != -1 || fileUrl.lastIndexOf('.wma') != -1)
    {
        str +=	'<object id="player" height="320" width="754" classid="CLSID:6BF52A52-394A-11d3-B153-00C04F79FAA6">';
        str +=	'<param NAME="AutoStart" VALUE="-1">';
        str +=	'<param NAME="Balance" VALUE="0">';
        str +=	'<param name="enabled" value="-1">';
        str +=	'<param NAME="EnableContextMenu" VALUE="-1">';
        str +=	'<param NAME="url" value="'+fileUrl+'">';
        str +=	'<param NAME="PlayCount" VALUE="1">';
        str +=	'<param name="rate" value="1">';
        str +=	'<param name="currentPosition" value="0">';
        str +=	'<param name="currentMarker" value="0">';
        str +=	'<param name="defaultFrame" value="">';
        str +=	'<param name="invokeURLs" value="0">';
        str +=	'<param name="baseURL" value="">';
        str +=	'<param name="stretchToFit" value="0">';
        str +=	'<param name="volume" value="50">';
        str +=	'<param name="mute" value="0">';
        str +=	'<param name="uiMode" value="full">';
        //以下参数非常重要！解决遮挡DIV问题
        str +=	'<param name="windowlessVideo" value="true">';
        str +=	'<param name="fullScreen" value="0">';
        str +=	'<param name="enableErrorDialogs" value="-1">';
        str +=	'<param name="SAMIStyle" value>';
        str +=	'<param name="SAMILang" value>';
        str +=	'<param name="SAMIFilename" value>';
        str +='<param name="wmode" value="Opaque">';
        str +=	'</object>';
    }
    else
    {
        str +=	'<object id="testCmsSysMediaImgShow" classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" color="black" allowfullscreen="true" quality="high" type="application/x-shockwave-flash" width="754" height="320" wmode="transparent" data="'+basePath+'core/extools/player/jwplayer/5.9/player.swf?file='+fileUrl+'&screencolor=black&autoStart='+autoStart+'&image='+cover+'">';
        str +=		'<param id="testCmsSysMediaImgShowParam" name="movie" value="'+basePath+'core/extools/player/jwplayer/5.9/player.swf?file='+fileUrl+'&screencolor=black&autoStart='+autoStart+'&image='+cover+'" />';
        str +=		'<param name="wmode" value="transparent" />';
        str +=		'<param name="quality" value="high" />';
        str +=		'<param name="allowfullscreen" value="true" />';
        str +=		'<param name="displayheight" value="0" />';
        str +=      '<param name="wmode" value="Opaque">';
        str +=		'<embed id="embedPlayer" name="embedPlayer" wmode="transparent" width="754" height="320" allowfullscreen="true" quality="high" type="application/x-shockwave-flash" src="'+basePath+'core/extools/player/jwplayer/5.9/player.swf?file='+fileUrl+'&screencolor=black&autoStart='+autoStart+'&image='+cover+'" />';
        str +=	'</object>';


    }

    document.write(str);
}

/**
 * 删除视频文件信息,等待删除
 */
function deleteMedia(fieldSign, voice)
{
    parent.layer.confirm('您确认删除当前媒体文件吗？', {
        time: 0 //不自动关闭
        ,btn: ['确认', '取消']
        ,yes: function(index){
            parent.layer.close(index);
            $("#"+fieldSign +"_delete_flag").val('true');

            $("#"+fieldSign).val('');

            $("#"+fieldSign +"_sys_cms_media_showtime").val('');

            $("#"+fieldSign +"_sys_cms_media_width").val('');

            $("#"+fieldSign +"_sys_cms_media_height").val('');

            $("#"+fieldSign +"_sys_cms_media_name").val('');

            $("#"+fieldSign +"_sys_cms_media_type").val('');

            $("#"+fieldSign +"_sys_cms_media_cover_src").val('');

            $("#"+fieldSign +"_sys_cms_media_cover_w").val('');

            $("#"+fieldSign +"_sys_cms_media_cover_h").val('');

            $("#"+fieldSign +"_sys_cms_media_cover_n").val('');

            if(voice)
            {
                document.getElementById(fieldSign+'_sys_cms_iframe').src = basePath+'core/content/upload-audio-module.thtml?fileUrl=&autoStart=false&cover=';

            }
            else
            {
                document.getElementById(fieldSign+'_sys_cms_iframe').src = basePath+'core/content/upload-video-module.thtml?fileUrl=&autoStart=false&cover=';

            }


        }
    });


}

function importMek(fn)
{
    var obj = document.getElementById("classId");

    var classId = '';
    if(obj != null)
    {
        classId = obj.value;
    }

    parent.layer.open({
        type: 2
        ,title: '媒体库'

        ,shadeClose: false
        ,scrollbar: false
        ,shade: [0.3, '#393D49']
        ,maxmin: false
        ,content:  basePath+'core/content/editor/media-res-cov.thtml?classId='+classId+'&valId='+fn+'&mainfn='+window.name
        ,area: ['700px', '620px']


    });

}


function openCopyContentToSiteClassDialog(flag, modelId, refClassIdStr, siteId)
{
    parent.layer.open({
        type: 2
        ,title: '复制内容至栏目'
        ,shadeClose: false
        ,shade: [0.3, '#393D49']
        ,maxmin: false
        ,content: basePath+'core/content/dialog/show-copy-content-class.thtml?siteId='+siteId+'&modelId='+modelId+'&refClassIdStr='+refClassIdStr+'&flag='+flag+'&mainfn='+window.name
        ,area: ['520px', '703px']

    });


}


function openLinkContentToSiteClassDialog(flag, modelId, refClassIdStr, siteId)
{
    parent.layer.open({
        type: 2
        ,title: '引用内容到其他栏目'
        ,shadeClose: false
        ,shade: [0.3, '#393D49']
        ,maxmin: false
        ,content: basePath+'core/content/dialog/show-link-content-class.thtml?siteId='+siteId+'&modelId='+modelId+'&refClassIdStr='+refClassIdStr+'&flag='+flag+'&mainfn='+window.name
        ,area: ['520px', '703px']

    });

}




function openMoveContentToSiteClassDialog(modelId, classId)
{
    parent.layer.open({
        type: 2
        ,title: '移动内容至栏目'
        ,shadeClose: false
        ,shade: [0.3, '#393D49']
        ,maxmin: false
        ,content: basePath+'core/content/dialog/show-move-content-class.thtml?uid='+Math.random()+'&modelId='+modelId+'&classId='+classId
        ,area: ['520px', '703px']

    });

}
/**
 * iframe动态自适应大小
 */
function iframeResize(id)
{
    var targetFrame = null;
    if (document.getElementById)
    {
        targetFrame = document.getElementById(id);
    }
    else
    {
        eval('targetFrame = ' + id + ';');
    }

    if (targetFrame && !window.opera)
    {

        targetFrame.style.display="block"
        if(targetFrame.contentDocument && targetFrame.contentDocument.body.offsetHeight)
        {
            //ns6 syntax
            targetFrame.height = targetFrame.contentDocument.body.offsetHeight ;
            targetFrame.width = targetFrame.contentDocument.body.scrollWidth;
        }
        else if (targetFrame.Document && targetFrame.Document.body.scrollHeight)
        {
            //ie5+ syntax
            alert(targetFrame.Document.body.scrollHeight+' '+targetFrame.Document.body.scrollWidth);
            targetFrame.height = targetFrame.Document.body.scrollHeight;
            targetFrame.width = targetFrame.Document.body.scrollWidth;
        }
    }
}

/////////////////////////////////////////////字段组件辅助方法///////////////////////////////////////////////////

//页面全局图集排序
var allImageGroupSortInfo = new Array();


function initUploadImage(sid, type, size)
{

    try{
        swfu = new SWFUpload({
            upload_url: basePath+"content/multiUpload.do;jsessionid="+sid,

// File Upload Settings
            file_size_limit : size+" MB", // 1000MB
            file_types : type,//设置可上传的类型
            file_types_description : "图片文件",
            file_upload_limit : "50",

            file_queue_error_handler : fileQueueError,//选择文件后出错
            file_dialog_complete_handler : fileDialogComplete,//选择好文件后提交
            file_queued_handler : fileQueued,
            upload_progress_handler : uploadProgress,
            upload_error_handler : uploadError,
            upload_success_handler : uploadSuccess,
            upload_complete_handler : uploadComplete,

// Button Settings
            button_image_url :  basePath+"core/javascript/upload/SWFUpload/upload.png",
            button_placeholder_id : "spanButtonPlaceholder",
            button_width: 62,
            button_height: 22,
            button_text : '',
            button_text_style : '',
            button_text_top_padding: 10,
            button_text_left_padding: 18,
            button_window_mode: SWFUpload.WINDOW_MODE.TRANSPARENT,
            button_cursor: SWFUpload.CURSOR.HAND,


// Flash Settings
            flash_url : basePath+"core/javascript/upload/SWFUpload/swfupload.swf",

            use_query_string : true,
            custom_settings : {
                upload_target : "divFileProgressContainer"
            },

// Debug Settings
            debug: false //是否显示调试窗口


        });
    }catch(ex)
    {
        alert(ex);
    }
}

function startUploadFile()
{
    swfu.startUpload();
}

/**
 * 图集内图片排序
 */
function changePhotoPos(fieldSign, currentPos, action)
{
    var sortArray = allImageGroupSortInfo[fieldSign];

    var targetPos = null;
    if('up' === action)
    {
        if(currentPos == 0)
        {
            return;
        }

        for(var i = (currentPos-1); i >= 0; i--)
        {
            targetPos = sortArray[i];

            if(targetPos != null)
            {
                break;
            }
        }
    }
    else if('down' === action)
    {
        if(currentPos == (sortArray.length-1))
        {
            return;
        }

        for(var i = (currentPos+1); i < sortArray.length; i++)
        {
            targetPos = sortArray[i];

            if(targetPos != null)
            {
                break;
            }
        }
    }

    if(targetPos == null)
    {
        return;
    }

    var currentCover = $("#"+fieldSign +"-cover-"+currentPos).is(':checked');
    var targetCover = $("#"+fieldSign +"-cover-"+targetPos).is(':checked');

    if(currentCover)
    {
        $("#"+fieldSign +"-cover-"+targetPos).attr('checked',true);
        $("#"+fieldSign +"-cover-"+currentPos).attr('checked',false);
    }
    else if(targetCover)
    {
        $("#"+fieldSign +"-cover-"+targetPos).attr('checked',false);
        $("#"+fieldSign +"-cover-"+currentPos).attr('checked',true);
    }




    var tempShowImageHref = $("#"+fieldSign +"-cmsSysImageShowUrl-"+targetPos).attr('href');
    var tempResizeUrl = $("#"+fieldSign +"-resizeUrl-"+targetPos).attr('src');
    var tempAlt = $("#"+fieldSign +"-resizeUrl-"+targetPos).attr('title');
    var tempShowName = $("#"+fieldSign +"-name-show-"+targetPos).val();

    var tempResId = $("#"+fieldSign +"-resId-"+targetPos).val();
    var tempRelatePath = $("#"+fieldSign +"-relatePath-"+targetPos).val();
    var tempPhotoName = $("#"+fieldSign +"-photoName-"+targetPos).val();
    var tempPhotoOutLinkUrl = $("#"+fieldSign +"-photoOutLinkUrl-"+targetPos).val();
    var tempPhotoDesc = $("#"+fieldSign +"-photoDesc-"+targetPos).val();
    var tempHeight = $("#"+fieldSign +"-height-"+targetPos).val();
    var tempWidth = $("#"+fieldSign +"-width-"+targetPos).val();

    var tempHeightShow = $("#"+fieldSign +"-height-show-"+targetPos).val();
    var tempWidthShow  = $("#"+fieldSign +"-width-show-"+targetPos).val();

    $("#"+fieldSign +"-cmsSysImageShowUrl-"+targetPos).attr('href',$("#"+fieldSign +"-cmsSysImageShowUrl-"+currentPos).attr('href'));
    $("#"+fieldSign +"-cmsSysImageShowUrl-"+currentPos).attr('href',tempShowImageHref);

    $("#"+fieldSign +"-resizeUrl-"+targetPos).attr('src',$("#"+fieldSign +"-resizeUrl-"+currentPos).attr('src'));
    $("#"+fieldSign +"-resizeUrl-"+currentPos).attr('src',tempResizeUrl);

    $("#"+fieldSign +"-resizeUrl-"+targetPos).attr('title',$("#"+fieldSign +"-resizeUrl-"+currentPos).attr('title'));
    $("#"+fieldSign +"-resizeUrl-"+currentPos).attr('title',tempAlt);

    $("#"+fieldSign +"-photoDesc-"+targetPos).val($("#"+fieldSign +"-photoDesc-"+currentPos).val());
    $("#"+fieldSign +"-photoDesc-"+currentPos).val(tempPhotoDesc);

    $("#"+fieldSign +"-name-show-"+targetPos).val($("#"+fieldSign +"-name-show-"+currentPos).val());
    $("#"+fieldSign +"-name-show-"+currentPos).val(tempShowName);

    $("#"+fieldSign +"-photoOutLinkUrl-"+targetPos).val($("#"+fieldSign +"-photoOutLinkUrl-"+currentPos).val());
    $("#"+fieldSign +"-photoOutLinkUrl-"+currentPos).val(tempPhotoOutLinkUrl);

    $("#"+fieldSign +"-photoName-"+targetPos).val($("#"+fieldSign +"-photoName-"+currentPos).val());
    $("#"+fieldSign +"-photoName-"+currentPos).val(tempPhotoName);

    $("#"+fieldSign +"-resId-"+targetPos).val($("#"+fieldSign +"-resId-"+currentPos).val());
    $("#"+fieldSign +"-resId-"+currentPos).val(tempResId);

    $("#"+fieldSign +"-relatePath-"+targetPos).val($("#"+fieldSign +"-relatePath-"+currentPos).val());
    $("#"+fieldSign +"-relatePath-"+currentPos).val(tempRelatePath);

    $("#"+fieldSign +"-height-"+targetPos).val($("#"+fieldSign +"-height-"+currentPos).val());
    $("#"+fieldSign +"-height-"+currentPos).val(tempHeight);

    $("#"+fieldSign +"-width-"+targetPos).val($("#"+fieldSign +"-width-"+currentPos).val());
    $("#"+fieldSign +"-width-"+currentPos).val(tempWidth);

    $("#"+fieldSign +"-height-show-"+targetPos).val($("#"+fieldSign +"-height-show-"+currentPos).val());
    $("#"+fieldSign +"-height-show-"+currentPos).val(tempHeightShow);

    $("#"+fieldSign +"-width-show-"+targetPos).val($("#"+fieldSign +"-width-show-"+currentPos).val());
    $("#"+fieldSign +"-width-show-"+currentPos).val(tempWidthShow);
}

/**
 * 切换图集封面
 */
function setImageGroupCover(fieldSign, pos, filePath)
{
    var imageGroup = document.getElementsByName(fieldSign+'-cover');

    var imageCoverCheck;
    for(var i=0; i<imageGroup.length;i++)
    {
        imageCoverCheck = imageGroup[i];

        if(imageCoverCheck.id != fieldSign+'-cover-'+pos)
        {
            imageCoverCheck.checked = false;
        }
    }

    var checkedFlag = document.getElementById(fieldSign+'-cover-'+pos).checked;

    if(checkedFlag)
    {
        $("#"+fieldSign +"CmsSysImageCover").val(filePath);
    }
    else
    {
        $("#"+fieldSign +"CmsSysImageCover").val('');
    }
}

/**
 * 加载图片展示效果
 */
function loadImageShow()
{
    $(".cmsSysShowSingleImage").fancybox
    ({
        'overlayShow'	: false,
        'transitionIn'	: 'elastic',
        'transitionOut'	: 'elastic'
    });

    $(".videoShow").fancybox
    ({
        'overlayShow'	: false,
        'type' : 'swf',
        'transitionIn'	: 'elastic',
        'transitionOut'	: 'elastic'
    });
}

/**
 * 初始化图片
 */
function addGroupPhotoToPage(fieldSign, mw, mh, imageCount, url, resizeUrl, resId, reUrl, photoName, height, width, photoDesc)
{
    imageCount = imageCount-1;//从0开始排序,所以减1

    var uploadImageTr =	 '<div id="'+fieldSign +'-cmsSysDiv-'+imageCount+'" style="height:10px"></div>'
        +'<table id="'+fieldSign +'-uploadPhotoTable-'+imageCount+'" border="0" cellpadding="0" cellspacing="0" class="form-table-upload">'
        +'<tr>'
        +'<td>'
        +'<a id="'+fieldSign +'-cmsSysImageShowUrl-'+imageCount+'" href="'+url+'" class="cmsSysShowSingleImage"><img id="'+fieldSign +'-resizeUrl-'+imageCount+'" src="'+resizeUrl+'" width="141" height="102" title="'+photoName+'"/></a>'
        +'<input type="hidden" id="'+fieldSign +'-resId-'+imageCount+'" name="'+fieldSign +'-resId-'+imageCount+'" value="'+resId+'" />'
        +'<input type="hidden" id="'+fieldSign +'-relatePath-'+imageCount+'" name="'+fieldSign +'-relatePath-'+imageCount+'" value="'+reUrl+'" />'
        +'<input type="hidden" id="'+fieldSign +'-photoName-'+imageCount+'" name="'+fieldSign +'-photoName-'+imageCount+'" value="'+photoName+'" />'
        +'<input type="hidden" id="'+fieldSign +'-photoOutLinkUrl-'+imageCount+'" name="'+fieldSign +'-photoOutLinkUrl-'+imageCount+'" value="" />'
        +'<input type="hidden" id="'+fieldSign +'-orderFlag-'+imageCount+'" name="'+fieldSign +'-orderFlag-'+imageCount+'" value="'+imageCount+'" />'
        +'<input type="hidden" id="'+fieldSign +'-height-'+imageCount+'" name="'+fieldSign +'-height-'+imageCount+'" value="'+height+'" />'
        +'<input type="hidden" id="'+fieldSign +'-width-'+imageCount+'" name="'+fieldSign +'-width-'+imageCount+'" value="'+width+'" />'
        +'</td>'
        +'<td>'
        +'<table border="0" cellpadding="0" cellspacing="0" height="95px" class="form-table-big">'
        +'<tr>'
        +'<td>'
        +'&nbsp;&nbsp;'
        +'<textarea id="'+fieldSign +'-photoDesc-'+imageCount+'" name="'+fieldSign +'-photoDesc-'+imageCount+'" style="height:65px;width:598px" class="form-textarea">'+photoDesc+'</textarea>'
        +'&nbsp;&nbsp;&nbsp;<img class="a_pointer" src="../../core/style/icons/arrow-skip-090.png" onclick=\'javascript:changePhotoPos("'+fieldSign+'", ' +imageCount+',"up")\' />'
        +'</td>'
        +'</tr>'
        +'<tr>'
        +'<td>'
        +'&nbsp;&nbsp;封面&nbsp;<input class="form-checkbox" id="'+fieldSign +'-cover-'+imageCount+'" name="'+fieldSign +'-cover" onclick="javascript:setImageGroupCover(\''+fieldSign +'\',\''+imageCount+'\',\''+reUrl+'\');" type="checkbox" value="1"/>&nbsp;'
        +'&nbsp;&nbsp;名称&nbsp;<input style="width:177px;" class="form-input" id="'+fieldSign +'-name-show-'+imageCount+'" name="'+fieldSign +'-name-show-'+imageCount+'" type="text" value="'+photoName+'"/>&nbsp;'
        +'&nbsp;&nbsp;宽&nbsp;'
        +'<input id="'+fieldSign +'-width-show-'+imageCount+'" class="form-input" readonly type="text" style="width:43px;" value="'+width+'"/>'
        +'&nbsp;&nbsp;高&nbsp;'
        +'<input id="'+fieldSign +'-height-show-'+imageCount+'" class="form-input" readonly type="text" style="width:43px;" value="'+height+'"/>'

        //+'&nbsp;<input type="checkbox" class="form-checkbox" />水印'
        //+'&nbsp;<input type="checkbox" class="form-checkbox" />内容'
        +'&nbsp;&nbsp;'
        +'&nbsp;&nbsp;&nbsp;<input type="button" value="替换" onclick="javascript:showEditSingleImageDialog(\''+fieldSign+'\', \''+imageCount+'\')" class="btn-1" />&nbsp;'

        +'<input type="button" value="裁剪" onclick="javascript:disposeImage(\''+fieldSign +'\',\''+mw+'\',\''+mh+'\','+true+',\''+imageCount+'\');" class="btn-1" />&nbsp;'

        +'<input type="button" value="删除" onclick="javascript:deleteGroupPhotoSingleInfo(\''+fieldSign+'\', \''+imageCount+'\', \''+reUrl+'\');" width="16" height="16" alt="删除图片" class="btn-1" />'

        +'&nbsp;&nbsp;&nbsp;<img class="a_pointer" src="../../core/style/icons/arrow-skip-270.png" onclick=\'javascript:changePhotoPos("'+fieldSign+'", '+imageCount+', "down")\' />'
        +'</td>'
        +'</tr>'
        +'</table>'
        +'</td>'
        +'</tr>'
        +'</table>';


    var sortArray = allImageGroupSortInfo[fieldSign];
    sortArray[imageCount] = imageCount;

    $('#'+fieldSign+'CmsSysImageUploadTab').append(uploadImageTr);

    //当前最后的排序ID
    $('#'+fieldSign+'CmsSysImageCurrentCount').val(imageCount+1);
}

function deleteGroupPhotoSingleInfo(fieldSign, pos, relatePath)
{
    if(typeof(W) == 'undefined' || W == '')
    {
        var dialog = $.dialog({
            title :'提示',
            width: '160px',
            height: '60px',
            lock: true,
            icon: '32X32/i.png',

            content: '您确认删除此图片？',

            ok: function () {
                //若为封面，则去掉封面
                var checkedFlag = document.getElementById(fieldSign+'-cover-'+pos).checked;

                if(checkedFlag)
                {
                    document.getElementById(fieldSign+'CmsSysImageCover').value = '';
                }

                $('#'+fieldSign+'-uploadPhotoTable-'+pos).remove();
                $('#'+fieldSign+'-cmsSysDiv-'+pos).remove();

                var sortArray = allImageGroupSortInfo[fieldSign];
                sortArray[pos] = null;
                var currentCount = parseInt(document.getElementById(fieldSign+'CmsSysImageCurrentCount').value);

                document.getElementById(fieldSign+'CmsSysImageCurrentCount').value = currentCount-1;

                document.getElementById(fieldSign+'CmsSysImageGroupCount').innerHTML = document.getElementById(fieldSign+'CmsSysImageCurrentCount').value;

            },
            cancel: true

        });

    }
    else
    {
        var dialog = W.$.dialog({
            title :'提示',
            width: '160px',
            height: '60px',
            parent:api,
            lock: true,
            icon: '32X32/i.png',

            content: '您确认删除此图片？',

            ok: function () {
                //若为封面，则去掉封面
                var checkedFlag = document.getElementById(fieldSign+'-cover-'+pos).checked;

                if(checkedFlag)
                {
                    document.getElementById(fieldSign+'CmsSysImageCover').value = '';
                }

                $('#'+fieldSign+'-uploadPhotoTable-'+pos).remove();
                $('#'+fieldSign+'-cmsSysDiv-'+pos).remove();

                var sortArray = allImageGroupSortInfo[fieldSign];
                sortArray[pos] = null;
                var currentCount = parseInt(document.getElementById(fieldSign+'CmsSysImageCurrentCount').value);

                document.getElementById(fieldSign+'CmsSysImageCurrentCount').value = currentCount-1;

                document.getElementById(fieldSign+'CmsSysImageGroupCount').innerHTML = document.getElementById(fieldSign+'CmsSysImageCurrentCount').value;

            },
            cancel: true

        });

    }

}

function deleteAllGroupPhoto(fieldSign)
{
    var maxPos = document.getElementById(fieldSign+'CmsSysImageArrayLength').value;

    if(typeof(W) == 'undefined' || W == '')
    {
        var dialog = $.dialog({
            title :'提示',
            width: '160px',
            height: '60px',
            lock: true,
            icon: '32X32/i.png',

            content: '您确认删除全部文件？',

            ok: function () {
                //去封面
                document.getElementById(fieldSign+'CmsSysImageCover').value = '';

                var pos;

                var sortArray = allImageGroupSortInfo[fieldSign];

                for(var i = 0; i < maxPos; i++)
                {
                    pos = i;



                    $('#'+fieldSign+'-uploadPhotoTable-'+pos).remove();
                    $('#'+fieldSign+'-cmsSysDiv-'+pos).remove();


                    sortArray[pos] = null;

                }

                document.getElementById(fieldSign+'CmsSysImageCurrentCount').value = 0;

                document.getElementById(fieldSign+'CmsSysImageGroupCount').innerHTML = document.getElementById(fieldSign+'CmsSysImageCurrentCount').value;

            },
            cancel: true

        });
    }
    else
    {
        var dialog = W.$.dialog({
            title :'提示',
            width: '160px',
            height: '60px',
            parent:api,
            lock: true,
            icon: '32X32/i.png',

            content: '您确认删除全部文件？',

            ok: function () {
                //去封面
                document.getElementById(fieldSign+'CmsSysImageCover').value = '';

                var pos;

                var sortArray = allImageGroupSortInfo[fieldSign];

                for(var i = 0; i < maxPos; i++)
                {
                    pos = i;



                    $('#'+fieldSign+'-uploadPhotoTable-'+pos).remove();
                    $('#'+fieldSign+'-cmsSysDiv-'+pos).remove();


                    sortArray[pos] = null;

                }

                document.getElementById(fieldSign+'CmsSysImageCurrentCount').value = 0;

                document.getElementById(fieldSign+'CmsSysImageGroupCount').innerHTML = document.getElementById(fieldSign+'CmsSysImageCurrentCount').value;

            },
            cancel: true

        });

    }



}



/**
 * 获取指定组件字段指定数据
 */
function getValueFormFieldValInfo(fieldVal, valFlag)
{
    return fieldVal.substring(fieldVal.indexOf(valFlag+'=') + (valFlag.length+1), fieldVal.indexOf( ';' ,fieldVal.indexOf(valFlag+'=') + (valFlag.length+1)));
}


/////////////////////////////////////////////管理内容组件方法///////////////////////////////////////////////////

function disposeImage(fieldSign, mw, mh, gm, order)
{
    var resId = $('#'+fieldSign).val();

    if(true == gm)
    {
        var resId = $('#upload-resId-'+fieldSign+'-'+order).val();
    }

    if(resId == '' || resId == '-1' || resId == null)
    {
        parent.layer.msg('没有图片');

        return;
    }

    if(mw == '' || mw == null)
    {
        mw = $('#'+fieldSign+'CmsSysImgWidth').val();
    }

    if(mh == '' ||  mh == null )
    {
        mh = $('#'+fieldSign+'CmsSysImgHeight').val();
    }


    if(typeof(mw) == 'undefined' || mw == '')
    {
        mw = 100;
    }

    if(typeof(mh) == 'undefined' || mh == '')
    {
        mh = 100;
    }

    parent.layer.open({
        type: 2
        ,title: '裁剪图片'

        ,shadeClose: false
        ,scrollbar: false
        ,shade: [0.3, '#393D49']
        ,maxmin: false
        ,content:  basePath+'/core/content/dispose-image.thtml?fieldSign='+fieldSign+'&mw='+mw+'&mh='+mh+'&fmw='+mw+'&fmh='+mh+'&orgResId='+resId+'&resId='+resId+'&ratio=false'+'&gm='+gm+'&order='+order+'&mainfn='+window.name
        ,area: ['990px', '610px']


    });


}

function deleteImage(flag)
{
    var old = document.getElementById(flag+'_sys_jtopcms_old');

    if(typeof(old) != 'undefined' && old != null)
    {
        document.getElementById(flag+'_sys_jtopcms_old').value = document.getElementById(flag).value;
    }


    document.getElementById(flag).value= '';

    if(flag == 'contentImage' || flag == 'classImage' || flag == 'channelImage' || flag == 'homeImage')
    {
        $('#' + flag + 'CmsSysImgShow').hide();
        $('#' + flag + '-upload-drag').show();
    }

    document.getElementById(flag+'CmsSysShowSingleImage').href = basePath+'core/style/blue/images/no-image.gif';

    document.getElementById(flag+'CmsSysImgShow').src = basePath+'core/style/blue/images/no-image.gif';

    document.getElementById(flag+'CmsSysImgWidth').value = '';

    document.getElementById(flag+'CmsSysImgHeight').value = '';
}


function deleteFile(flag)
{
    var old = document.getElementById(flag+'_sys_jtopcms_old');

    if(typeof(old) != 'undefined' && old != null)
    {
        document.getElementById(flag+'_sys_jtopcms_old').value = document.getElementById(flag).value;
    }

    document.getElementById(flag).value= '';

    document.getElementById(flag+'_sys_cms_file_info').value = '';
}

function systemDownloadFile(flag)
{
    var obj = null;

    var dialog = false;

    obj = document.getElementById(flag);

    if(typeof(dialogApiId) == 'undefined' || dialogApiId == '')
    {
        //现在不需要处理dialog的情况
    }
    else
    {
        dialog = true;
    }


    if(typeof(obj) != 'undefined' && obj != null)
    {
        var resId = obj.value;

        if(resId == null || '' == resId ||  resId < 0)
        {
            if(dialog)
            {
                W.$.dialog.tips('文件不存在或失效',1,'32X32/i.png');
            }
            else
            {
                $.dialog.tips('文件不存在或失效',1,'32X32/i.png');
            }


            return;
        }

        window.location = basePath+'content/clientDf.do?id='+resId;

    }

}



/**
 * 访问UploadVideoModule的截图方法
 */
function cutCover(frmaeId, id)
{
    document.getElementById(frmaeId).contentWindow.snapshotImage(id);
}

/**
 * 展示视频截图
 */
function showCover(fieldSign)
{
    var src = document.getElementById(fieldSign+'_sys_jtopcms_media_cover_src').value;

    var type = document.getElementById(fieldSign+'_sys_jtopcms_media_type').value;

    if('rm' == type)
    {
        alert('当前视频类型不支持截取图片!');
        return;
    }

    if('' == src || null == src)
    {
        if(typeof(W) == 'undefined' || W == '')
        {
            $.dialog({
                title :'提示',
                width: '160px',
                height: '60px',
                lock: true,
                icon: '32X32/i.png',

                content: '当前视频无截取图片!',
                cancel: true
            });

        }
        else
        {
            W.$.dialog({
                title :'提示',
                width: '160px',
                height: '60px',
                lock: true,
                parent:api,
                icon: '32X32/i.png',

                content: '当前视频无截取图片!',
                cancel: true
            });

        }

        return;
    }

    var w = document.getElementById(fieldSign+'_sys_jtopcms_media_cover_w').value;

    var h = document.getElementById(fieldSign+'_sys_jtopcms_media_cover_h').value;

    var n = document.getElementById(fieldSign+'_sys_jtopcms_media_cover_n').value;

    var newWH = checkSize(w, h, 800, 800);

    if(typeof(W) == 'undefined' || W == '')
    {
        $.dialog
        ({
            title: n+' ('+w+' x '+h+')',
            lock: true,
            max: false,
            min: false,
            content: '<img src="'+src+'" width="'+newWH[0]+'" height="'+newWH[1]+'" />',
            padding: 0
        });
    }
    else
    {
        W.$.dialog
        ({
            title: n+' ('+w+' x '+h+')',
            lock: true,
            parent:api,
            max: false,
            min: false,
            content: '<img src="'+src+'" width="'+newWH[0]+'" height="'+newWH[1]+'" />',
            padding: 0
        });
    }


}

// 清除内容占用状态
function clearContentMT(cId)
{
    var url = '/content/clearContentMT.do';

    var postData = 'id='+cId;

    var uid = '';

    $.ajax
    (
        {
            type: 'POST', async: true, url: url, data: postData, success:
                function (da, textStatus) {

                    console.log('clearMT:'+cId)
                }
        }
    )

    return uid;
}

//刷新栏目内容管理页面的tab
function reloadCMTab(page, obj)
{
    $( ".layadmin-iframe", obj ).each(function( index ) {

        if($(this).attr('src').indexOf(page) != -1)
        {

            this.contentWindow.reloadMI();

        }
    });
}

//刷新指定页面的tab
function reloadTab(page, obj)
{
    $( ".layadmin-iframe", obj ).each(function( index ) {

        if($(this).attr('src').indexOf(page) != -1)
        {

            this.contentWindow.location.reload();

        }
    });
}

function showOutLink(obj)
{
    if(obj.checked)
    {
        $('#outlinkTr').show();
        $('#modelDataDIV').hide();

    }
    else
    {
        $('#outlinkTr').hide();
        $('#modelDataDIV').show();
    }

    fixMW();
}

function fixMW()
{
    var lmW = $('#lc-div').height();
    var rmW = $('#rc-div').height();

    var lcW = $('#left-card').height();
    var rcW = $('#right-card').height();

    if(lcW > rcW)
    {
        $('#right-card').height(lmW-55 )
    }
    else
    {
        $('#left-card').height(rmW+45 )
    }

    //需要再次fix
    if(lcW > rcW)
    {
        $('#right-card').height(lmW-55 )
    }
    else
    {
        $('#left-card').height(rmW-55 )
    }

}

function showTagKW(obj)
{
    if(obj.checked)
    {
        $('#tkTr').show();
    }
    else
    {
        $('#tkTr').hide();
    }
}

function getKeywordFromContent()
{
    var text = '';

    var content = '';
    try
    {
        //目前只考虑标题和摘要中取关键字
        //content = getEditorTextContents('content');
    }
    catch(ex)
    {

    }

    //优先content
    if('' != content)
    {
        text = content;
    }

    //if('' == text)
    {
        text = document.getElementById('title').value + ' '+text;
    }

    //if('' == text)
    {
        text = document.getElementById('summary').value + ' ' + text;
    }



    if('' == text)
    {
        parent.layer.msg('标题不存在,无法获取关键字');

        document.getElementById('keywords').value = '';

        return;
    }



    var url = basePath+"search/distillKeyword.do";

    $("#jtopcms_sys_keyword_content").val(text);
    //var postData = encodeURI($("#content,#title").serialize());
    var postData = encodeURI($("#jtopcms_sys_keyword_content").serialize());

    postData = encodeData(postData);

    $.ajax({
        type: "POST",
        url: url,
        data:postData,

        success: function(msg)
        {

            document.getElementById('keywords').value = msg;

            parent.layer.msg('获取关键字成功');

        }
    });

}

function textCounter(obj, showid)
{
    var len = obj.value.length;

    $('#'+showid).html(len + ' / 400');
}

var titleEm = false;
var titleStrong = false;
var titleUnderline = false;

var simpleTitleEm = false;
var simpleTitleStrong = false;
var simpleTitleUnderline = false;


function setTitleChar(obj)
{
    var titleText = document.getElementById('title');

    //titleText.style.color = '';

    if(obj.id == 'strongTitle')
    {
        if(titleStrong)
        {
            titleStrong = false;
            titleText.style.fontWeight = "";
        }
        else
        {
            titleStrong = true;
            titleText.style.fontWeight = "bold";
        }
    }

    if(obj.id == 'emTitle')
    {
        if(titleEm)
        {
            titleEm = false;
            titleText.style.fontStyle = "";
        }
        else
        {
            titleEm = true;
            titleText.style.fontStyle = "italic";
        }
    }

    if(obj.id == 'underTitle')
    {
        if(titleUnderline)
        {
            titleUnderline = false;
            titleText.style.textDecoration = "none";
        }
        else
        {
            titleUnderline = true;
            titleText.style.textDecoration = "underline";
        }
    }

    //titleText.style.color = document.getElementById('titleColor').value;
}

function setSimpleTitleChar(obj)
{
    var simpleTitleText = document.getElementById('simpleTitle');

    //titleText.style.color = '';

    if(obj.id == 'strongSimpleTitle')
    {
        if(simpleTitleStrong)
        {
            simpleTitleStrong = false;
            simpleTitleText.style.fontWeight = "";
        }
        else
        {
            simpleTitleStrong = true;
            simpleTitleText.style.fontWeight = "bold";
        }
    }

    if(obj.id == 'emSimpleTitle')
    {
        if(simpleTitleEm)
        {
            simpleTitleEm = false;
            simpleTitleText.style.fontStyle = "";
        }
        else
        {
            simpleTitleEm = true;
            simpleTitleText.style.fontStyle = "italic";
        }
    }

    if(obj.id == 'underSimpleTitle')
    {
        if(simpleTitleUnderline)
        {
            simpleTitleUnderline = false;
            simpleTitleText.style.textDecoration = "none";
        }
        else
        {
            simpleTitleUnderline = true;
            simpleTitleText.style.textDecoration = "underline";
        }
    }

    //simpleTitleText.style.color = document.getElementById('simpleTitleColor').value;
}

function changeTitleColor(titleId,val)
{
    var titleText = document.getElementById(titleId);
    titleText.style.color = val;
}

function showTitleInput(flag1,flag2,obj)
{
    if(obj.checked == true)
    {
        $('#'+flag1+'Tr').show();
        $('#'+flag2+'Tr').show();
    }
    else
    {
        $('#'+flag1+'Tr').hide();
        $('#'+flag2+'Tr').hide();
    }
}

function disposeTitleStyle()
{
    //获取title样式
    var titleBgColor = document.getElementById('titleBgVal').value;

    var titleFontStyle = document.getElementById('title').style.fontStyle;

    var titleFontWeight = document.getElementById('title').style.fontWeight;

    var titleTextDec = document.getElementById('title').style.textDecoration;

    document.getElementById('titleStyle').value = 'color:'+titleBgColor+';font-weight:'+titleFontWeight+';font-style:'+titleFontStyle+';text-decoration:'+titleTextDec;

}





/**
 * 发布已选择的内容
 */
function publishContent(type)
{
    if(type != '3')
    {
        $.dialog({
            title :'提示',
            width: '230px',
            height: '60px',
            lock: true,
            icon: '32X32/i.png',

            content: '当前栏目内容为动态发布,无需静态化！',
            cancel: true
        });

        return;
    }

    var cidCheck = document.getElementsByName('checkContent');

    var ids='';
    for(var i=0; i<cidCheck.length;i++)
    {
        if(cidCheck[i].checked)
        {
            ids += cidCheck[i].value+',';
        }
    }

    if('' == ids)
    {
        $.dialog({
            title :'提示',
            width: '160px',
            height: '60px',
            lock: true,
            icon: '32X32/i.png',

            content: '请选择需要发布的内容！',
            cancel: true


        });
        return;
    }

    var url = basePath+"/publish/generateContent.do";

    $("#someContentId").val(ids);

    var postData = $("#someContentId,#staticType").serialize();

    $.ajax({
        type: "POST",
        url: url,
        data:postData,

        success: function(msg)
        {

            if('success' == msg)
            {
                $.dialog({
                    title :'提示',
                    width: '160px',
                    height: '60px',
                    lock: true,
                    icon: '32X32/succ.png',

                    content: '发布内容成功！',
                    ok: true
                });

                for(var i=0; i<cidCheck.length;i++)
                {
                    cidCheck[i].checked = false;
                }
            }
            else
            {
                $.dialog({
                    title :'提示',
                    width: '320px',
                    height: '60px',
                    lock: true,
                    icon: '32X32/succ.png',

                    content: msg,
                    cancel: true
                });

            }
        }
    });


}


/**
 * 选择参数
 */
function selectRule(rule,target)
{
    var fileRuleName = document.getElementById(target);

    var selection = document.selection;

    fileRuleName.focus();

    if (typeof fileRuleName.selectionStart != "undefined")
    {
        var s = fileRuleName.selectionStart;
        fileRuleName.value = fileRuleName.value.substr(0, fileRuleName.selectionStart) + rule.value + fileRuleName.value.substr(fileRuleName.selectionEnd);
        fileRuleName.selectionEnd = s + rule.value.length;
    }
    else if (selection && selection.createRange)
    {
        var sel = selection.createRange();
        sel.text = rule.value;
    } else
    {
        fileRuleName.value += rule.value;
    }


    var opts = rule.options;

    for(var i=0; i<opts.length; i++)
    {
        if(opts[i].value == '-1')
        {
            opts[i].selected=true;
            break;
        }
    }
}


function selectParamForFCK(obj,id)
{
    var oEditor = FCKeditorAPI.GetInstance(id);

    if(obj.value == '' || obj.value == null)
    {
        return;
    }


    if(oEditor.EditMode == FCK_EDITMODE_WYSIWYG)
    {
        oEditor.InsertHtml('$'+obj.value);
    }
}

function selectParamForUE(obj,id)
{

    if(obj.value == '' || obj.value == null)
    {
        return;
    }

    UE.getEditor(id).execCommand('insertHtml', '$'+obj.value)


}


/**
 * 选择模板
 */
function openSelectTempletDialog(mode, objId, vm)
{
    var targetName = '';

    var vmName = 'PC端';


    if('mob' == vm)
    {
        vmName = 'Mob端';
    }
    else if('pad' == vm)
    {
        vmName = 'Pad端';
    }


    if('channel' == mode)
    {
        targetName = vmName+'栏目首页';
    }
    else if('class' == mode)
    {
        targetName = vmName+'列表页';
    }
    else if('content' == mode)
    {
        targetName = vmName+'内容页';
    }
    else if('home' == mode)
    {
        targetName = vmName+'首页';
    }

    else if('block' == mode)
    {
        targetName = '区块页';
    }

    parent.layer.open({
        type: 2
        ,title: '选择'+targetName+'模版'
        ,shadeClose: false
        ,shade: [0.3, '#393D49']
        ,scrollbar: false
        ,resize:false
        ,maxmin: false
        ,content: basePath+'/core/channel/select-channel-templet.thtml?mode='+mode+'&objId='+objId+'&vm='+vm+'&fn='+window.name
        ,area: ['820px', '680px'],

    });



}



//选取相关内容
function openSelectContentSourceDialog()
{

    parent.layer.open({
        type: 2
        ,title: '选取来源'
        ,shadeClose: false
        ,scrollbar: false
        ,shade: [0.3, '#393D49']
        ,maxmin: false
        ,content: basePath+'core/content/dialog/show-content-source.thtml?mainfn='+window.name
        ,area: ['580px', '710px']

    });

}

//选取Tag
function openSelectTagDialog()
{
    parent.layer.open({
        type: 2
        ,title: '选取Tag标签'
        ,shadeClose: false
        ,scrollbar: false
        ,shade: [0.3, '#393D49']
        ,maxmin: false
        ,content: basePath+'core/content/dialog/show-site-tag.thtml?mainfn='+window.name
        ,area: ['650px', '710px']

    });
}

//选取站群节点
function openSelectSiteGroupDialog(ids,flag)
{
    parent.layer.open({
        type: 2
        ,title: '共享到站群'
        ,shadeClose: false
        ,scrollbar: false
        ,shade: [0.3, '#393D49']
        ,maxmin: false
        ,content: basePath+'/core/content/dialog/show-site-group.thtml?ids='+ids+'&flag='+flag+'&mainfn='+window.name
        ,area: ['460px', '560px']

    });

}

//选取相关内容
function openSelectRelatedContentDialog(contentId,classId)
{
    parent.layer.open({
        type: 2
        ,title: '选取相关内容'
        ,shadeClose: false
        ,shade: [0.3, '#393D49']
        ,scrollbar: false
        ,resize:false
        ,maxmin: false
        ,content: basePath+'/core/content/dialog/show-related-content.thtml?classId='+classId+'&contentId='+contentId+'&rids='+$('#relateIds').val()+'&clId=0'+'&mainfn='+window.name
        ,area: ['900px', '85%'],

    });

}

//选取站点调查
function openSelectSurveyDialog(contentId,classId)
{
    W.$.dialog({
        id : 'ossd',
        title : '选取站点调查',
        width: '690px',
        height: '480px',
        parent:api,
        lock: true,
        max: false,
        min: false,
        resize: false,

        content: 'url:'+basePath+'/core/content/dialog/ShowRelateSurvey.jsp?uid='+Math.random()+'&classId='+classId+'&contentId='+contentId+'&rsids='+$('#relateSurvey').val()
    });
}


//////////////////////////////////////////////////////验证通用///////////////////////////////////////////////
/**
 * 通用的自定义模型字段验证规则，分别绑定 focus propertychange blur 三事件
 */


function validateExistFlag(target, val)
{
    val = encodeData(val);

    var url = basePath+"common/sysFlagExist.do?target="+target;

    var count = 0;

    $.ajax({
        type: "POST",
        url: url,
        async:false,
        data: 'val='+val,
        dataType: "json",
        success: function(msg)
        {

            count =  msg;
        }
    });

    return count;
}

function validateLayui(filedSign,notNull,reg,msg)
{
    var obj = $("#"+filedSign);

    if($("#"+filedSign).length < 1)
    {
        obj = $("input[name='"+filedSign+"']");
    }

    if(obj.attr('type') == 'radio' || obj.attr('type') == 'checkbox' )
    {
        filedSign = 'sys-obj-'+filedSign;
    }

    obj.bind('focus', function()
    {

        // $( 'div.'+filedSign+'_jtop_ui_tips_class' ).remove();


        var target = obj.val();

        if(obj.attr('type') == 'radio' || obj.attr('type') == 'checkbox' )
        {
            target = getRadioCheckedValue(filedSign);
        }

        if(target != null && target != undefined  )
        {
            target =target.replace(/\\/g,'\\\\');
        }

        if(target != null && target != undefined  )
        {
            target =target.replace(/\"/g,'\\"');
        }

        if(target != null && target != undefined && target.trim() != '' )
        {
            if(reg != null && reg !='' )
            {
                if(!eval('regexTest("'+target+'",'+reg+')'))
                {
                    showTips(filedSign,msg);
                }
            }
        }
        else if(1 == notNull)
        {
            showTips(filedSign,'不可为空');
        }
    });


    obj.bind('keyup', function()
    {

        $( 'div.'+filedSign+'_jtop_ui_tips_class' ).remove();

        var target = obj.val();

        if(obj.attr('type') == 'radio' || obj.attr('type') == 'checkbox' )
        {
            target = getRadioCheckedValue(filedSign);
        }

        if(target != null && target != undefined  )
        {
            target =target.replace(/\\/g,'\\\\');
        }

        if(target != null && target != undefined  )
        {
            target =target.replace(/\"/g,'\\"');
        }

        if(target != null && target != undefined && target.trim() != '' )
        {
            if(reg != null && reg !='' )
            {
                if(!eval('regexTest("'+target+'",'+reg+')'))
                {
                    showTips(filedSign,msg);
                }
            }
        }
        else if(1 == notNull)
        {
            showTips(filedSign,'不可为空');
        }

    });

    obj.bind('blur', function()
    {
        $( 'div.'+filedSign+'_jtop_ui_tips_class' ).remove();
    });
}





/**
 * 通用的自定义模型字段验证规则,提交验证规则,若不通过,将所有错误信息提示
 */
function submitValidateLayui(filedSign,notNull,reg,msg,filedName)
{
    var obj = $("#"+filedSign);

    if($("#"+filedSign).length < 1)
    {
        obj = $("input[name='"+filedSign+"']");

    }

    var target = obj.val();

    var editorMode = false;



    if ( typeof(UE) != "undefined" )
    {
        if(typeof($('#'+filedSign+'_jtop_sys_hidden_temp_html').val())!= "undefined")
        {
            target =getEditorHTMLContents(filedSign);
            editorMode = true;
        }
    }

    var hasError = false;


    if(obj.attr('type') == 'radio' || obj.attr('type') == 'checkbox' )
    {
        target = getRadioCheckedValue(filedSign);

        filedSign = 'sys-obj-'+filedSign;
    }

    if(editorMode)
    {
        filedSign = 'sys-obj-'+filedSign;
    }


    if(target != null && target != undefined  )
    {
        target =target.replace(/\\/g,'\\\\');
    }

    if(target != null && target != undefined  )
    {
        target =target.replace(/\"/g,'\\"');
    }


    if((target == null || target == undefined || target.trim() == '' ) && 1 == notNull)
    {
        if(filedName)
        {
            showTips(filedSign, filedName+'不可为空');
        }
        else
        {
            showTips(filedSign,'不可为空');
        }

        hasError = true;
    }
    else
    {
        if(reg != null && reg !='' )
        {
            if(!eval('regexTest("'+target+'",'+reg+')'))
            {
                showTips(filedSign,msg);
                hasError = true;
            }
        }
    }

    return hasError;
}


/**
 * 通用的自定义模型字段验证规则，分别绑定 focus propertychange blur 三事件
 */
function validate(filedSign,notNull,reg,msg)
{
    var obj = $("#"+filedSign);

    if($("#"+filedSign).length < 1)
    {
        obj = $("input[name='"+filedSign+"']");
    }

    if(obj.attr('type') == 'radio' || obj.attr('type') == 'checkbox' )
    {
        filedSign = 'sys-obj-'+filedSign;
    }

    obj.bind('focus', function()
    {

        // $( 'div.'+filedSign+'_jtop_ui_tips_class' ).remove();


        var target = obj.val();

        if(obj.attr('type') == 'radio' || obj.attr('type') == 'checkbox' )
        {
            target = getRadioCheckedValue(filedSign);
        }

        if(target != null && target != undefined  )
        {
            target =target.replace(/\\/g,'\\\\');
        }

        if(target != null && target != undefined  )
        {
            target =target.replace(/\"/g,'\\"');
        }

        if(target != null && target != undefined && target.trim() != '' )
        {
            if(reg != null && reg !='' )
            {
                if(!eval('regexTest("'+target+'",'+reg+')'))
                {
                    showTips(filedSign,msg);
                }
            }
        }
        else if(1 == notNull)
        {
            showTips(filedSign,'不可为空');
        }
    });


    obj.bind('keyup', function()
    {

        $( 'div.'+filedSign+'_jtop_ui_tips_class' ).remove();

        var target = obj.val();

        if(obj.attr('type') == 'radio' || obj.attr('type') == 'checkbox' )
        {
            target = getRadioCheckedValue(filedSign);
        }

        if(target != null && target != undefined  )
        {
            target =target.replace(/\\/g,'\\\\');
        }

        if(target != null && target != undefined  )
        {
            target =target.replace(/\"/g,'\\"');
        }

        if(target != null && target != undefined && target.trim() != '' )
        {
            if(reg != null && reg !='' )
            {
                if(!eval('regexTest("'+target+'",'+reg+')'))
                {
                    showTips(filedSign,msg);
                }
            }
        }
        else if(1 == notNull)
        {
            showTips(filedSign,'不可为空');
        }

    });

    obj.bind('blur', function()
    {
        $( 'div.'+filedSign+'_jtop_ui_tips_class' ).remove();
    });
}



/**
 * 通用的自定义模型字段验证规则,提交验证规则,若不通过,将所有错误信息提示
 */
function submitValidate(filedSign,notNull,reg,msg)
{
    var obj = $("#"+filedSign);

    if($("#"+filedSign).length < 1)
    {
        obj = $("input[name='"+filedSign+"']");

    }

    var target = obj.val();

    var editorMode = false;



    if ( typeof(UE) != "undefined" )
    {
        if(typeof($('#'+filedSign+'_jtop_sys_hidden_temp_html').val())!= "undefined")
        {
            target =getEditorHTMLContents(filedSign);
            editorMode = true;
        }
    }

    var hasError = false;



    if(obj.attr('type') == 'radio' || obj.attr('type') == 'checkbox' )
    {
        target = getRadioCheckedValue(filedSign);

        filedSign = 'sys-obj-'+filedSign;
    }

    if(editorMode)
    {
        filedSign = 'sys-obj-'+filedSign;
    }


    if(target != null && target != undefined  )
    {
        target =target.replace(/\\/g,'\\\\');
    }

    if(target != null && target != undefined  )
    {
        target =target.replace(/\"/g,'\\"');
    }


    if((target == null || target == undefined || target.trim() == '' ) && 1 == notNull)
    {


        if(typeof($('#noimg-'+filedSign).css('display')) != 'undefined' && $('#noimg-'+filedSign).css('display') == 'none')
        {
            return false;
        }


        showTips(filedSign,'不可为空');
        hasError = true;
    }
    else
    {
        if(reg != null && reg !='' )
        {
            if(!eval('regexTest("'+target+'",'+reg+')'))
            {
                showTips(filedSign,msg);
                hasError = true;
            }
        }
    }

    return hasError;
}

function regexTest(target,patrn)
{

    var test = !patrn.exec(target);

    if (test)
    {
        return false;
    }
    return true;
}


function showTipsLayui( id, tips )
{
    $('#'+id).addClass('layui-form-danger');
    $('#'+id).focus();

    //layer.msg(tips, {icon: 5,time:2500, anim: 6});
    //layer.tips("<span style='font-size:14px'>"+tips+"</span>", '#'+id, {
    //  tips: [1, '#3595CC'],
    // time: 2000
    //});
}



function showTips( id,tips )
{


    var mt = '7px';

    var selx = 0;

    var tagName = '';

    var tagNameIG = '';


    if(typeof($('#'+id)[0]) != 'undefined')
    {
        tagName = $('#'+id)[0].tagName;
    }

    if(typeof($('#upload-group-'+id)[0]) != 'undefined')
    {
        tagNameIG = $('#upload-group-'+id)[0].tagName;
    }

    if('hidden' == $('#'+id).attr('type') || tagName == 'SELECT' || tagNameIG == 'BUTTON' ||id.startWith('sys-obj-'))
    {
        if($('#'+id+'CmsSysShowSingleImage').attr('class') == 'cmsSysShowSingleImage' )
        {
            mt = '20px';
        }

        if(tagName == 'SELECT')
        {
            selx = 330;

            showTipsLayui( id, tips );

            return;
        }

        if(id.startWith('sys-obj-'))
        {
            id = id.replace('sys-obj-', '');

            layer.tips("<span style='font-size:14px;color:black'>"+tips+"</span>", '#'+'label-' + id, {
                tips: [1, '#ffffcd'],
                tipsMore: true,
                time: 4000
            });

            //showTipsLayui( id, tips );
            return;
        }

        if(tagNameIG == 'BUTTON')
        {
            //id = id.replace('sys-obj-', '');

            layer.tips("<span style='font-size:14px;color:black'>"+tips+"</span>", '#'+'label-' + id, {
                tips: [1, '#ffffcd'],
                tipsMore: true,
                time: 4000
            });

            //showTipsLayui( id, tips );
            return;
        }

        id = 'label-' + id;
    }


    var offset = $('#'+id).offset();

    if(offset == null)
    {
        offset = 	$("input[name='"+id+"']").offset();

        if(offset == null)
        {
            return;
        }
    }

    var y = offset.top-9;

    var width = $('#'+id).width();

    if(width == null)
    {
        width = $("input[name='"+id+"']") .width();

        if(width == null)
        {
            return;
        }
    }

    var x = 0;


    if(id.startWith('sys-obj'))
    {
        x = width +5+ offset.left;
    }
    else
    {
        x = width +18+ offset.left+selx;
    }

    var tipsDiv = "<div class='"+id+"_jtop_ui_tips_class' style='margin-top: "+mt+"'><div class='onError'><div class='onError-bottom'><div class='onError-right'><div class='onError-left'><p class='onError-top-right'>"+tips+"</p></div></div></div></div></div>";

    $( 'body' ).append( tipsDiv );
    $( '.'+id+"_jtop_ui_tips_class" ).css(
        {
            'top' : y + 'px',
            'left' : x + 'px',
            'position' : 'absolute',
            'z-index' : '99'

        }).show();

//setTimeout( function(){$( 'div._jtop_ui_tips_class' ).fadeOut();}, ( time * 1000 ) );
}

function hideTips(filedSign)
{
    if('hidden' == $('#'+filedSign).attr('type') || $('#'+filedSign)[0].tagName == 'SELECT' || filedSign.startWith('sys-obj-'))
    {
        filedSign = 'label-' + filedSign;
    }
    //$( '.'+id+"_jtop_ui_tips_class" ).hide();
    $( 'div.'+filedSign+'_jtop_ui_tips_class' ).remove();
}


function tipLay( id, tips )
{
    layer.tips("<span style='font-size:14px'>"+tips+"</span>", '#'+id, {
        tips: [2, '#3595CC'],
        time: 2000
    });
}

function alertLay( id, tips )
{
    layer.tips("<span style='font-size:14px'>"+tips+"</span>", '#'+id, {
        tips: [1, '#3595CC'],
        time: 2000
    });
}




function encodeData(data)
{
    temp = data;

    temp = temp.replace(/\'/g, "**!1**");
    temp = temp.replace(/\(/g, "**!2**");
    temp = temp.replace(/\)/g, "**!3**");

    temp = replaceAll(temp, '..', "**!4**"  );
    //temp = replaceAll(temp, "\'", "**!5**"  );
    temp = replaceAll(temp, '"', "**!6**"  );
    //temp = replaceAll(temp, '\"', "**!7**"  );
    temp = replaceAll(temp, '<', "**!8**"  );
    temp = replaceAll(temp, '>', "**!9**"  );
    temp = replaceAll(temp, '|', "**!10**"  );
    temp = replaceAll(temp, '\\', "**!11**"  );
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

function encodeFormInput(formName, encodeParam)
{
    var uform = document.getElementById(formName);

    if (uform == null || typeof (uform) == "undefined")
    {
        alert('安全编码encodeFormInput()提示：错误的form名称，请检查指定的form表单id是否正确');

        return;
    }

    for(var i = 0; i < uform.length; i++)
    {

        if(uform.elements[i].type == 'text' || uform.elements[i].type == 'textarea' || uform.elements[i].type == 'hidden' || uform.elements[i].type == 'password')
        {

            if(uform.elements[i].id == null || uform.elements[i].id =="")
            {
                continue;
            }

            encodeForInput(uform.elements[i].id, encodeParam);

        }
    }
}


function encodeFormEditor(formName, encodeParam)
{
    var uform = document.getElementById(formName);

    for(var i = 0; i < uform.length; i++)
    {

        if(uform.elements[i].type == 'text' || uform.elements[i].type == 'textarea' || uform.elements[i].type == 'hidden' )
        {

            if(uform.elements[i].id == null || uform.elements[i].id =="")
            {
                continue;
            }

            if (typeof (document.getElementById(uform.elements[i].id)) == "undefined")
            {
                return;
            }


            if(uform.elements[i].id.endWith('_jtop_sys_hidden_temp_html'))
            {
                var editorId = replaceAll(uform.elements[i].id, '_jtop_sys_hidden_temp_html', ''  );

                var temp =  getEditorHTMLContents(editorId);

                if(temp != null )
                {
                    if(encodeParam)
                    {
                        temp =  encodeURIComponent(temp);
                    }

                    clearEditor(editorId);

                    temp = encodeData(temp);

                    document.getElementById(editorId+'_jtop_sys_hidden_temp_html').value = temp;

                    document.getElementById(editorId).value = temp;
                }
            }


        }
    }
}

function encodeForInput(fieldSign, jsEncode)
{
    if (typeof (document.getElementById(fieldSign)) == "undefined")
    {
        return;
    }


    if(fieldSign.endWith('_jtop_sys_hidden_temp_html'))
    {
        var editorId = replaceAll(fieldSign, '_jtop_sys_hidden_temp_html', ''  );

        // setFCKeditorReadOnly(editorId);
        var temp =  getEditorHTMLContents(editorId);

        if(temp != null)
        {
            if(jsEncode)
            {
                temp =  encodeURIComponent(temp);
            }

            clearEditor(editorId);

            temp = encodeData(temp);

            document.getElementById(editorId+'_jtop_sys_hidden_temp_html').value = temp;

        }
    }
    else
    {

        //var temp =  document.getElementById(fieldSign).value;
        var temp =  $('#'+fieldSign).val();

        //if('textarea' == $('#'+fieldSign).attr("type"))
        //{

        //temp = $('#'+fieldSign).html();

        //temp = replaceAll(temp, '&lt;', "<");

        //temp = replaceAll(temp, '&gt;', ">");

        //temp = replaceAll(temp, '&amp;', "&");

        //}

        if(temp != null)
        {
            if(jsEncode)
            {
                temp =  encodeURIComponent(temp);
            }


            temp = encodeData(temp);


            //document.getElementById(fieldSign).value = temp;
            $('#'+fieldSign).val(temp);
        }
    }



}



/////////////////////////////////////////////FCK编辑器API////////////////////////////////////////////////

//设置编辑器中内容
function setContentsFK(codeStr){
    var oEditor = FCKeditorAPI.GetInstance(content) ;
    oEditor.SetHTML(codeStr) ;
}

// 获取编辑器中HTML内容
function getEditorHTMLContentsFK(EditorName) {
    var oEditor = FCKeditorAPI.GetInstance(EditorName);
    return(oEditor.GetXHTML(true));
}
// 获取编辑器中文字内容
function getEditorTextContentsFK(EditorName) {
    var oEditor = FCKeditorAPI.GetInstance(EditorName);

    if (document.all)
    {
        return(oEditor.EditorDocument.body.innerText);
    }
    else
    {
        var r = oEditor.EditorDocument.createRange();
        r.selectNodeContents(oEditor.EditorDocument.body);
        return r;
    }

}
// 设置编辑器中内容
function SetEditorContentsFK(EditorName, ContentStr) {
    try
    {
        var oEditor = FCKeditorAPI.GetInstance(EditorName) ;
        oEditor.SetHTML(ContentStr) ;
    }
    catch(e)
    {
    }
}


function clearEditorFK(EditorName)
{
    if(FCKeditorAPI.GetInstance(EditorName).EditorDocument != null)
    {
        FCKeditorAPI.GetInstance(EditorName).EditorDocument.body.innerHTML='';
    }
    else
    {
        SetEditorContents(EditorName,'');
    }
}

//向编辑器插入指定代码
function insertHTMLToEditorFK(content,codeStr){
    var oEditor = FCKeditorAPI.GetInstance(content);
    if (oEditor.EditMode==FCK_EDITMODE_WYSIWYG){
        oEditor.InsertHtml(codeStr);
    }else{
        return false;
    }
}


/*设置FCKEDITOR为只读 */
function setFCKeditorReadOnlyFK( editorId ){
    var editor = FCKeditorAPI.GetInstance(editorId);
    try{
        editor.EditorDocument.body.contentEditable = false;
        editor.EditMode=FCK_EDITMODE_SOURCE;
        editor.ToolbarSet.RefreshModeState();
        editor.EditMode=FCK_EDITMODE_WYSIWYG;
        editor.ToolbarSet.RefreshModeState();
        editor.EditorWindow.parent.document.getElementById('xExpanded').style.display = 'none';
        editor.EditorWindow.parent.document.getElementById('xCollapsed').style.display = 'none';
        editor.EditorWindow.blur();
    }
    catch(e){
    }
}




/////////////////////////////////////////////UE编辑器API////////////////////////////////////////////////

//设置编辑器中内容
function setContents(EditorName, isAppendTo, codeStr){
    var ue = UE.getEditor(EditorName);

    ue.setContent(codeStr, isAppendTo)

}

// 获取编辑器中HTML内容
function getEditorHTMLContents(EditorName) {
    var ue = UE.getEditor(EditorName);
    return ue.getContent();
}
// 获取编辑器中文字内容
function getEditorTextContents(EditorName) {

    var ue = UE.getEditor(EditorName);
    return ue.getContentTxt();

}

// 设置编辑器中内容
function SetEditorContents(EditorName, ContentStr) {

    var ue = UE.getEditor(EditorName);

    ue.setContent(ContentStr)

}


function clearEditor(EditorName)
{
    var ue = UE.getEditor(EditorName);

    ue.setContent('')
}




function brType()
{

    var userAgent = navigator.userAgent,
        rMsie = /(msie\s|trident.*rv:)([\w.]+)/,
        rFirefox = /(firefox)\/([\w.]+)/,
        rOpera = /(opera).+version\/([\w.]+)/,
        rChrome = /(chrome)\/([\w.]+)/,
        rSafari = /version\/([\w.]+).*(safari)/;
    var browser;
    var version;
    var ua = userAgent.toLowerCase();
    function uaMatch(ua){
        var match = rMsie.exec(ua);
        if(match != null){
            return { browser : "IE", version : match[2] || "0" };
        }
        var match = rFirefox.exec(ua);
        if (match != null) {
            return { browser : match[1] || "", version : match[2] || "0" };
        }
        var match = rOpera.exec(ua);
        if (match != null) {
            return { browser : match[1] || "", version : match[2] || "0" };
        }
        var match = rChrome.exec(ua);
        if (match != null) {
            //chrome返回代号
            return { browser : 'c1' || "", version : match[2] || "0" };
        }
        var match = rSafari.exec(ua);
        if (match != null) {
            return { browser : match[2] || "", version : match[1] || "0" };
        }
        if (match != null) {
            return { browser : "", version : "0" };
        }
    }
    var browserMatch = uaMatch(userAgent.toLowerCase());
    if ( typeof(browserMatch) != 'undefined' && browserMatch.browser){
        browser = browserMatch.browser;
        version = browserMatch.version;
    }
    return (browser+version);


}



/////////////////////////////////////////////正则表达式////////////////////////////////////////////////
//校验是否全由数字组成

//校验Flag：只能输入1-25个字母、数字、下划线
function isFlag(s)
{
    var patrn=/^(\w){1,25}$/;
    if (!patrn.exec(s)) return false
    return true
}

function isDigit(s)
{
    var patrn=/^[0-9]{1,20}$/;
    if (!patrn.exec(s)) return false
    return true
}

//校验登录名：只能输入5-20个以字母开头、可带数字、“_”、“.”的字串

function isRegisterUserName(s)
{
    var patrn=/^[a-zA-Z]{1}([a-zA-Z0-9]|[._]){4,19}$/;
    if (!patrn.exec(s)) return false
    return true
}


//function isSearch(s)
//{
//var patrn=/^[^`~!@#$%^&*()+=|\\\][\]\{\}:;'\,.<>/?]{1}[^`~!@$%^&()+=|\\\][\]\{\}:;'\,.<>?]{0,19}$/;
//if (!patrn.exec(s)) return false
//return true
//}


//校验用户姓名：只能输入1-30个以字母开头的字串

function isTrueName(s)
{
    var patrn=/^[a-zA-Z]{1,30}$/;
    if (!patrn.exec(s)) return false
    return true
}


//校验密码：只能输入6-20个字母、数字、下划线
function isPasswd(s)
{
    var patrn=/^(\w){6,20}$/;
    if (!patrn.exec(s)) return false
    return true
}



//校验普通电话、传真号码：可以“+”开头，除数字外，可含有“-”
function isTel(s)
{
//var patrn=/^[+]{0,1}(\d){1,3}[ ]?([-]?(\d){1,12})+$/;
    var patrn=/^[+]{0,1}(\d){1,3}[ ]?([-]?((\d)|[ ]){1,12})+$/;
    if (!patrn.exec(s)) return false
    return true
}

//校验手机号码：必须以数字开头，除数字外，可含有“-”
function isMobile(s)
{
    var patrn=/^1[3|4|5|8][0-9]\d{4,8}$/;
    if (!patrn.exec(s)) return false
    return true
}

//校验邮政编码
function isPostalCode(s)
{
    var patrn=/^\d{6}$/;
    if (!patrn.exec(s)) return false
    return true
}



function isIP(s)
{
    var patrn=/^[0-9.]{1,20}$/;
    if (!patrn.exec(s)) return false
    return true
}


function idCard(v)
{
    var r15=/[1-6]\d{5}\d{2}(?:0\d|1[12])(?:0\d|[12]\d|3[01])\d{3}/;
    var r18=/[1-6]\d{5}(?:19|20)\d{2}(?:0\d|1[12])(?:0\d|[12]\d|3[01])\d{3}[\dXx]/;
    if(v.length==15)
    {
        return r15.test(v);
    }
    else if(v.length==18)
    {
        var coefficient = new Array(7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7,9, 10, 5, 8, 4, 2);
        var count = 0;
        for (var i=0;i<v.length&&i<coefficient.length;i++)
        {
            var t = parseInt(v.charAt(i));
            count += t * coefficient[i];
        }
        count = count % 11;
        var c = null;
        switch (count)
        {		case 0:			c = "1";			break;		case 1:			c = "0";			break;		case 2:			c = "x";			break;		case 3:			c = "9";			break;		case 4:			c = "8";			break;		case 5:			c = "7";			break;		case 6:			c = "6";			break;		case 7:			c = "5";			break;		case 8:			c = "4";			break;		case 9:			c = "3";			break;		default:			c = "2";			break;

        }

        return r18.test(v)&&(v.charAt(17)==c);
    }
    return false;
}






/**
 * 显示提示信息
 */
function showSuccessTips( tips, height, time ){
    var windowWidth = document.documentElement.clientWidth;
    var tipsDiv = '<div class="tipsClass">' + tips + '</div>';

    $( 'body' ).append( tipsDiv );
    $( 'div.tipsClass' ).css({
        'top' : height + 'px',
        'left' : ( windowWidth / 2 ) - ( tips.length * 13 / 2 ) + 'px',
        'position' : 'absolute',
        'padding' : '3px 5px',
        'background': '#8FBC8F',
        'font-size' : 12 + 'px',
        'margin' : '0 auto',
        'text-align': 'center',
        'width' : 'auto',
        'color' : '#fff',
        'opacity' : '0.8'
    }).show();
    setTimeout( function(){$( 'div.tipsClass' ).fadeOut();}, ( time * 1000 ) );
}




var reg_d_ymd = /^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/;
var reg_d_ymd_hms = /^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})\s*(\d{1,2}):(\d{1,2}):(\d{1,2})$/;
var reg_d_hms = /^((20|21|22|23|[0-1]\d)\:[0-5][0-9])(\:[0-5][0-9])?$/;

var reg_cc = /^[\u0391-\uFFE5]+$/;
var reg_ec = /^[a-zA-Z]+$/;
var reg_dit = /^[-+]?\d*$/;
var reg_double = /^[-\+]?\d+(\.\d+)?$/;
var reg_email  =/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
var reg_url = /^(https|http|ftp|rtsp|mms):\/\/[A-Za-z0-9]+\.[A-Za-z0-9]+[\/=\?%\-&_~`@[\]\':+!]*([^<>\"\"])*$/;









