/**
 * @author zhujunhan
 * @date 2018-05-15
 * description：基于百度webuploader的上传插件
 * html代码：<div id="upload" class="uploader"></div>
 * 生成控件代码：var upload = $("#upload").createUploader()或new UploadFile("upload")
 * 如果不是设置的自动上传，手动上传时调用
 * upload.uploadFile(function(vals) {
 *    console.log(vals);
 * }); 传入回调函数获取上传后的文件信息
 *
 */
(function (factory) {
    if (typeof define === 'function' && define.amd) {
        // AMD
        define(['jquery', 'webuploader'], factory);
    } else if (typeof exports === 'object') {
        // CommonJS
        factory(require('jquery', 'webuploader'));
    } else {
        // Browser globals
        factory(jQuery, WebUploader);
    }
}(function ($, WebUploader) {
    var toast = window.alert;

    $.fn.createUploader = function (initData) {
        var $this = $(this);
        return new UploadFile($this.attr("id"), initData);
    };

    function UploadFile(target, initData) {
        initData = $.extend({
            target: "#" + target,
            extraData: {},//额外传到服务器的参数
            fileList: [],//初始添加的文件，{name:'文件名',ext:'doc',prevurl:'图片预览地址',filepath:'文件存储地址'}
            maxCount: 9,//可添加的上传文件个数
            submitName: "fileName",//提交值的name属性值
            serverUrl: "/api/v1/upload",//文件上传的服务地址
            // 支持格式包括：png,jpg,jpeg,gif,bmp,flv,swf,mkv,avi,rm,rmvb,mpeg,mpg,ogg,ogv,mov,wmv,mp4,webm,mp3,wav,mid,rar,zip,tar,gz,7z,bz2,cab,iso,doc,docx,xls,xlsx,ppt,pptx,pdf,txt,md,xml
            acceptExt: "txt,doc,docx,xls,xlsx,jpg,jpeg,gif,png,bmp,pdf,avi,rm,wav,rmvb,mp3,mp4,zip,rar",// 允许上传的文件格式
            autoload: false, // 选择文件后是否自动上传
            promptText: "提示：最多可同时上传3个文件，每个文件不超过100M。"//上传提示
        }, initData);
        for (var key in initData) {
            this[key] = initData[key];
        }
        this.init();
    }

    UploadFile.prototype = {
        init: function () {
            this.initContainer();
            this.initUploader();
        },
        initContainer: function () {
            var _this = this;
            $(_this.target)
                .html('<div class=\"statusBar\" style=\"display: none;\">' +
                    '<div class=\"progress\"><span class=\"text\">0%</span><span class=\"percentage\"></span></div>' +
                    '<div class=\"info\"></div><div class=\"btns\"><div id=\"filePicker2\"></div>' +
                    '<div class=\"uploadBtn message-btn\">开始上传</div></div></div><div class=\"queueList\">' +
                    '<div id=\"dndArea\" class=\"placeholder\"><div id=\"filePicker\"></div><p>或将文件拖到这里</p></div></div></div>');
            _this.$wrap = $(_this.target).find('.queueList');
            $(_this.target).find('.uploader-prompt').text(_this.promptText);
        },
        /* 初始化容器 */
        initUploader: function () {
            var _this = this, $wrap = _this.$wrap,
                // 上传按钮
                $filePickerBlock = $wrap.find('.filePickerBlock'),
                // 添加的文件数量
                fileCount = 0,
                // 设置文件的总数量
                fileLimitCount = _this.maxCount,
                fileSingleSizeLimit = 100 * 1024 * 1024,//单个文件最多100M
                // 优化retina, 在retina下这个值是2
                ratio = window.devicePixelRatio || 1,
                // 缩略图大小
                thumbnailWidth = 113 * ratio, thumbnailHeight = 113 * ratio,
                // WebUploader实例
                uploader,
                uploaderPath = _this.getUploaderPath();

            uploader = _this.uploader = WebUploader.create({
                pick: {
                    id: _this.target + ' .filePickerBlock'
                },
                // 拖拽容器
                dnd: _this.target + ' .queueList',
                paste: document.body,
                auto: _this.autoload, // 自动上传
                withCredentials: false,// 跨域时，是否允许携带cookie, 只有html5 runtime才有效
                fileVal: 'file', //服务端接受文件流的名称
                formData: _this.extraData,
                duplicate: true, // 不去重
                chunked: true, // [默认值：false] 是否要分片处理大文件上传
                chunkSize: fileSingleSizeLimit, // {Boolean} [可选] [默认值：5242880]，如果要分片，分多大一片？ 默认大小为5M.
                chunkRetry: 4, // {Boolean} [可选] [默认值：2]，如果某个分片由于网络问题出错，允许自动重传多少次？
                threads: 10, // {Boolean} [可选] [默认值：3]，上传并发数。允许同时最大上传进程数
                swf: uploaderPath + 'Uploader.swf',
                ocx: uploaderPath + 'Flash11e.ocx#version=11.1.102.55',
                server: _this.serverUrl,
                fileNumLimit: fileLimitCount,
                //fileSizeLimit: 200 * 1024 * 1024, // 200 M
                fileSingleSizeLimit: fileSingleSizeLimit, // 100 M
                compress: false,
                accept: _this.getAccept()
            });

            // 当有文件添加进来时执行，负责view的创建
            function addFile(file) {
                var $li = $('<li class="' + file.id + '">' + '<p class="title">' + file.name + '</p>'
                    + '<p class="imgWrap"></p>' + '<p class="progress"><span></span></p>' + '</li>'),
                    $btns = $('<div class="file-panel">'
                        + '<span class="cancel">删除</span></div>').appendTo($li), $prgress = $li.find('p.progress span'),
                    $wrap = $li
                        .find('p.imgWrap'), $info = $('<p class="error"></p>').hide().appendTo($li),

                    showError = function (code) {
                        var text = '';
                        switch (code) {
                            case 'exceed_size':
                                text = '文件大小超出';
                                break;
                            case 'interrupt':
                                text = '上传暂停';
                                break;
                            case 'http':
                                text = '网络出错';
                                break;
                            case 'not_allow_type':
                                text = '文件类别不匹配';
                                break;
                            default:
                                text = '上传失败，请重试';
                                break;
                        }
                        $info.text(text).show();
                    };

                if (file.getStatus && file.getStatus() === 'invalid') {
                    showError(file.statusText);
                } else {
                    $wrap.text('预览中');
                    if ('|png|jpg|jpeg|bmp|gif|'.indexOf('|' + file.ext.toLowerCase() + '|') === -1) {
                        $wrap.empty().addClass('notimage').append('<i class="file-preview file-type-'
                            + file.ext.toLowerCase() + '"></i>' + '<span class="file-title" title="' + file.name
                            + '">' + file.name + '</span>');
                    } else {
                        if (file.hasload) {
                            var $img = $('<img src="' + file.prevurl + '">');
                            $wrap.empty().append($img);
                            $img.on('error', function () {
                                $wrap.text('不能预览');
                            });
                        } else {
                            //webuploader提供的图片压缩方法（转base64格式）
                            uploader.makeThumb(file, function (error, src) {
                                if (error || !src) {
                                    $wrap.text('不能预览');
                                } else {
                                    var $img = $('<img src="' + src + '">');
                                    $wrap.empty().append($img);
                                    $img.on('error', function () {
                                        $wrap.text('不能预览');
                                    });
                                }
                            }, thumbnailWidth, thumbnailHeight);
                        }

                    }

                    /* 检查文件格式 */
                    if (!file.hasload && (!file.ext || _this.acceptExt.indexOf(file.ext.toLowerCase()) === -1)) {
                        showError('not_allow_type');
                        uploader.removeFile(file);
                    }
                }
                if (file.hasload) {
                    $li.find('.file-panel').show().find('.cancel').show();
                    $btns.show();
                    var result = JSON.stringify({ name: file.name, path: file.filepath });
                    $li.addClass('state-complete').append(
                        "<span class='success'><input name='" + _this.submitName + "' type='hidden' value='" + result + "'/></span>");
                } else {
                    file.on('statuschange', function (cur, prev) {
                        if (prev === 'progress') {
                            $prgress.hide().width(0);
                        } else if (prev === 'queued') {
                            $li.find('.file-panel').hide().find('*').hide();
                            $btns.hide();
                        }
                        // 成功
                        if (cur === 'error' || cur === 'invalid') {
                            showError(file.statusText);
                            $li.find('.file-panel').show().find('.cancel').show();
                            $btns.show();
                        } else if (cur === 'interrupt') {
                            showError('interrupt');
                        }/* else if (cur === 'queued') {
                        }*/ else if (cur === 'progress') {
                            $info.hide();
                            $prgress.css('display', 'block');
                        } else if (cur === 'complete') {
                            $li.find('.file-panel').show().find('.cancel').show();
                            $btns.show();
                        }

                        $li.removeClass('state-' + prev).addClass('state-' + cur);
                    });
                }

                //删除按钮滑入滑出效果
                $li.on('mouseenter', function () {
                    $btns.stop().animate({
                        height: 30
                    });
                });
                $li.on('mouseleave', function () {
                    $btns.stop().animate({
                        height: 0
                    });
                });
                //点击删除去除文件
                $btns.on('click', 'span', function () {
                    var $li;
                    if (!file.hasload) {
                        uploader.removeFile(file);
                    }
                    $li = $(_this.target + ' .' + file.id);

                    $li.off().find('.file-panel').off().end().remove();
                    fileCount--;

                    if (fileCount < fileLimitCount) {
                        $filePickerBlock.removeClass("webuploader-element-invisible");
                    }
                });

                $li.insertBefore($filePickerBlock);

                fileCount++;
                //当添加文件达到最大允许上传个数时隐藏添加按钮
                if (fileCount === fileLimitCount) {
                    //必须使用这个样式来隐藏，而不能用jq的hide，否则使用flash时无法选择
                    $filePickerBlock.addClass("webuploader-element-invisible");
                }
            }

            //初始化数据
            for (var i = 0; i < _this.fileList.length; i++) {
                if (fileCount >= fileLimitCount) {
                    toast("文件数量超过限制，一次最多上传" + fileLimitCount + "个！");
                    break;
                }
                _this.fileList[i].id = "INIT_FILE_" + i;
                _this.fileList[i].hasload = true;
                addFile(_this.fileList[i]);
            }

            uploader.on('fileQueued', function (file) {
                addFile(file);
            });
            // 图片上传数量判断
            uploader.on('beforeFileQueued', function (file) {
                if (fileCount >= fileLimitCount) {
                    toast("文件数量超过限制，一次最多上传" + fileLimitCount + "个！");
                    return false;
                }
                if (file.size >= fileSingleSizeLimit) {
                    toast("文件大小超过限制，单个文件最多100兆！");
                    return false;
                }
            });
            // 图片上传进度条更新
            uploader.on('uploadProgress', function (file, percentage) {
                var $li = $(_this.target + ' .' + file.id), $percent = $li.find('.progress span');

                $percent.css('width', percentage * 100 + '%');
            });
            // 图片上传成功的时候
            uploader.on('uploadSuccess', function (file, ret) {
                var $file = $(_this.target + ' .' + file.id);
                try {
                    var responseText = (ret._raw || ret), json = JSON.parse(responseText);
                    if (json.success) {
                        var result = JSON.stringify({ name: file.name, path: json.filepath });
                        $file.append("<span class='success'><input name='" + _this.submitName
                            + "' type='hidden' value='" + result + "'/></span>");
                    } else {
                        $file.find('.error').text(json.message).show();
                    }
                } catch (e) {
                    $file.find('.error').text('服务器返回出错').show();
                }
            });
            // 图片上传错误的时候
            uploader.on('uploadError', function (file, code, ext) {
                if (code === 'Q_TYPE_DENIED' || code === 'F_EXCEED_SIZE') {
                    addFile(file);
                }

                if (code === 'Q_EXCEED_NUM_LIMIT') {
                    toast('只能上传' + fileLimitCount + '个文件');
                }
            });

            uploader.on('uploadBeforeSend', function (file, data, header) {
                //需要token可在此添加
                //header['token'] = 'tokenstr';
            });
            //当文件都上传完成时（手动上传时需要传入回调函数）
            uploader.on("uploadFinished", function () {
                if (_this.callback) {
                    var values = [];
                    $("input[name=" + _this.submitName + "]", _this.$wrap).each(function (index, item) {
                        values.push($(item).val());
                    });
                    if (values.length === fileCount) {
                        _this.callback(values);
                    } else {
                        toast("上传文件失败");//有失败的文件不回调
                    }
                }
            });
        },
        //手动上传时调用
        uploadFile: function (callback) {
            var _this = this;
            _this.uploader.upload();
            _this.callback = callback;
        },
        getUploaderPath: function () {
            var uploaderPath = "";
            if (!WebUploader.Uploader.support("html5")) {
                //使用flash上传时才会用到
                //带点排除路径中也有webuploader的情况，避免截取路径错误
                var searchTxt = "webuploader.";
                var uploaderTags = $("link[href*='" + searchTxt + "'],script[src*='" + searchTxt + "']");
                if (uploaderTags.length > 0) {
                    uploaderPath = uploaderTags.attr("href") || uploaderTags.attr("src");
                    uploaderPath = uploaderPath.substring(0, uploaderPath.indexOf(searchTxt));
                }
                if (!WebUploader.Base.supportFlash()) {
                    toast('上传控件不支持您的浏览器！如果您使用的是IE9及以下版本的浏览器，请尝试升级flash播放器');
                }
            }
            return uploaderPath;
        },
        getAccept: function () {
            var _this = this, accept;
            //常用类型的mimetype，可以控制文件选择框只能选择指定的文件
            var mimeTypes = {
                'avi': 'video/x-msvideo',
                'bmp': 'image/bmp',
                'doc': 'application/msword',
                'docx': 'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
                'gif': 'image/gif',
                'gz': 'application/x-gzip',
                'jpeg': 'image/jpeg',
                'jpg': 'image/jpeg',
                'mid': 'audio/midi',
                'mkv': 'video/x-matroska',
                'mov': 'video/quicktime',
                'mp3': 'audio/mpeg',
                'mp4': 'video/mp4',
                'mpeg': 'video/mpeg',
                'mpg': 'video/mpeg',
                'ogv': 'video/ogg',
                'pdf': 'application/pdf',
                'png': 'image/png',
                'ppt': 'application/vnd.ms-powerpoint',
                'pptx': 'application/vnd.openxmlformats-officedocument.presentationml.presentation',
                'swf': 'application/x-shockwave-flash',
                'tar': 'application/x-tar',
                'txt': 'text/plain',
                'wav': 'audio/x-wav',
                'webm': 'video/webm',
                'wmv': 'video/x-ms-wmv',
                'xls': 'application/vnd.ms-excel',
                'xlsx': 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
                'xml': 'application/xml',
                'zip': 'application/zip'
            };
            var acceptExtArr = _this.acceptExt.split(",");
            var mimeTypeArr = [];
            for (var key in mimeTypes) {
                if (acceptExtArr.indexOf(key) !== -1) {
                    mimeTypeArr.push(mimeTypes[key]);
                }
            }
            if (acceptExtArr.length === mimeTypeArr.length) {
                //都获取到了才配置accept
                accept = {
                    title: _this.acceptExt,
                    extensions: _this.acceptExt,
                    mimeTypes: mimeTypeArr.join(",")
                };
            }
            return accept;
        }
    };

    return UploadFile;
}));