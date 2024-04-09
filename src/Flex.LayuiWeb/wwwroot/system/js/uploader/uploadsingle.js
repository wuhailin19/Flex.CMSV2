/**
 * upload
 */
; function ___initUpload($elment, options) {
    // 当domReady的时候开始初始化
    //$(window).on('load', function () {

    var $wrap = $($elment);
    options = $.extend({
        single: false,
        isImage: true,
        autoupload: false,
        serverUrl: '/api/Upload/UploadImage',
        imageExtensions: 'jpg,jpeg,gif,png,bmp,svg',
        FileExtensions: 'txt,doc,docx,xls,xlsx,mp4,zip,pdf',
        valueElement: undefined
    }, options)

    init();
    var $queue = $('<ul class="filelist"></ul>').appendTo($wrap.find('.queueList')),// 图片容器
        $statusBar = $wrap.find('.statusBar'),// 状态栏，包括进度和控制按钮
        $info = $statusBar.find('.info'), // 文件总体选择信息。
        $upload = $wrap.find('.uploadBtn'),// 上传按钮
        $placeHolder = $wrap.find('.placeholder'),// 没选择文件之前的内容。
        $progress = $statusBar.find('.progress').hide(),
        fileCount = 0,// 添加的文件数量
        fileSize = 0,// 添加的文件总大小
        ratio = window.devicePixelRatio || 1,// 优化retina, 在retina下这个值是2
        thumbnailWidth = 110 * ratio,// 缩略图大小
        thumbnailHeight = 110 * ratio,// 缩略图大小
        state = 'pedding',// 可能有pedding, ready, uploading, confirm, done.
        percentages = {},// 所有文件的进度信息，key为file id
        $filePicker2 = $($elment + " .filePicker2")
    //判断浏览器是否支持图片的base64
    isSupportBase64 = (function () {
        var data = new Image();
        var support = true;
        data.onload = data.onerror = function () {
            if (this.width != 1 || this.height != 1) {
                support = false;
            }
        }
        data.src = "data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///ywAAAAAAQABAAACAUwAOw==";
        return support;
    })(),
        supportTransition = (function () {
            var s = document.createElement('p').style,
                r = 'transition' in s ||
                    'WebkitTransition' in s ||
                    'MozTransition' in s ||
                    'msTransition' in s ||
                    'OTransition' in s;
            s = null;
            return r;
        })();
    var uploader;// WebUploader实例
    var extensionsf = options.isImage ? options.imageExtensions : options.FileExtensions
    SizeLimit = 20, FullSizeLimit = 60;
    function init() {

        $wrap.addClass("webUploader");
        if (options.valueElement != undefined)
            $wrap.append('<input type="hidden" id="' + $elment + 'fujian" value="" />');
        if (options.single) {
            $wrap.addClass("singleButton");
            return;
        }
        $wrap
            .append('<div class=\"statusBar\" style=\"display: none;\">' +
                '<div class=\"progress\"><span class=\"text\">0%</span><span class=\"percentage\"></span></div>' +
                '<div class=\"info\"></div><div class=\"btns\"><div class=\"filePicker2\"></div>' +
                '<div class=\"uploadBtn message-btn\">开始上传</div></div></div><div class=\"queueList\">' +
                '<div class=\"placeholder dndArea\"><div class=\"filePicker\"></div><p>或将文件拖到这里</p></div></div></div>');
    }
    // 实例化
    uploader = WebUploader.create({
        pick: options.single ? $elment : {
            id: $elment + ' .filePicker',
            label: '点击选择文件'
        },
        formData: {
            fileType: 'file'
        },
        dnd: options.single ? '' : $elment + ' .dndArea',
        paste: options.single ? '' : $elment,
        chunked: false,
        chunkSize: 512 * 1024,
        server: options.serverUrl,
        // runtimeOrder: 'flash',
        accept: {
            extensions: extensionsf,
            mimeTypes: '*'
        },
        headers: httpTokenHeaders,
        // 禁掉全局的拖拽功能。这样不会出现图片拖进页面的时候，把图片打开。
        disableGlobalDnd: true,
        fileNumLimit: options.single ? 1 : 300,
        fileSizeLimit: FullSizeLimit * 1024 * 1024,    // 30 M
        fileSingleSizeLimit: SizeLimit * 1024 * 1024    // 10 M
    });

    // 拖拽时不接受 js, txt 文件。
    uploader.on('dndAccept', function (items) {
        var denied = false,
            len = items.length,
            i = 0,
            // 修改js类型
            unAllowed = 'text/plain;application/javascript ';
        for (; i < len; i++) {
            // 如果在列表里面
            if (~unAllowed.indexOf(items[i].type)) {
                denied = true;
                break;
            }
        }
        return !denied;
    });

    uploader.on('dialogOpen', function () {
        console.log('here');
    });

    // uploader.on('filesQueued', function() {
    //     uploader.sort(function( a, b ) {
    //         if ( a.name < b.name )
    //           return -1;
    //         if ( a.name > b.name )
    //           return 1;
    //         return 0;
    //     });
    // });

    // 添加“添加文件”的按钮，
    uploader.addButton({
        id: $elment + ' .filePicker2',
        label: '继续添加'
    });

    uploader.on('ready', function () {
        window.uploader = uploader;
    });

    // 当有文件添加进来时执行，负责view的创建
    function addFile(file) {
        var $li = $('<li id="' + file.id + '">' +
            '<p class="title">' + file.name + '</p>' +
            '<p class="imgWrap"></p>' +
            '<p class="progress"><span></span></p>' +
            '</li>'),
            $btns = $('<div class="file-panel">' +
                '<span class="cancel">删除</span>' +
                '<span class="rotateRight">向右旋转</span>' +
                '<span class="rotateLeft">向左旋转</span></div>').appendTo($li),
            $prgress = $li.find('p.progress span'),
            $wrap = $li.find('p.imgWrap'),
            $info = $('<p class="error"></p>'),
            showError = function (code) {
                switch (code) {
                    case 'exceed_size':
                        text = '文件大小超出';
                        break;
                    case 'interrupt':
                        text = '上传暂停';
                        break;
                    case 'errorfile':
                        text = '文件格式错误';
                        break;
                    default:
                        text = '上传失败，请重试';
                        break;
                }

                $info.text(text).appendTo($li);
            };
        if (file.getStatus() === 'invalid') {
            showError(file.statusText);
        } else {
            // @todo lazyload
            $wrap.text('预览中');
            uploader.makeThumb(file, function (error, src) {
                var img;
                if (error) {
                    $wrap.text('不能预览');
                    return;
                }
                if (isSupportBase64) {
                    img = $('<img src="' + src + '">');
                    $wrap.empty().append(img);
                } else {
                    $wrap.text("预览出错");
                }
            }, thumbnailWidth, thumbnailHeight);
            percentages[file.id] = [file.size, 0];
            file.rotation = 0;
        }

        file.on('statuschange', function (cur, prev) {
            if (prev === 'progress') {
                $prgress.hide().width(0);
            } else if (prev === 'queued') {
                $li.off('mouseenter mouseleave');
                $btns.remove();
            }
            // 成功
            if (cur === 'error' || cur === 'invalid') {
                showError(file.statusText);
                percentages[file.id][1] = 1;
            } else if (cur === 'interrupt') {
                showError('interrupt');
            } else if (cur === 'queued') {
                $info.remove();
                $prgress.css('display', 'block');
                percentages[file.id][1] = 0;
            } else if (cur === 'progress') {
                $info.remove();
                $prgress.css('display', 'block');
            } else if (cur === 'complete') {
                $prgress.hide().width(0);
                $li.append('<span class="success"></span>');
            }

            $li.removeClass('state-' + prev).addClass('state-' + cur);
        });

        $li.on('mouseenter', function () {
            $btns.stop().animate({ height: 30 });
        });

        $li.on('mouseleave', function () {
            $btns.stop().animate({ height: 0 });
        });

        $btns.on('click', 'span', function () {
            var index = $(this).index(),
                deg;

            switch (index) {
                case 0:
                    uploader.removeFile(file);
                    return;

                case 1:
                    file.rotation += 90;
                    break;

                case 2:
                    file.rotation -= 90;
                    break;
            }
            if (supportTransition) {
                deg = 'rotate(' + file.rotation + 'deg)';
                $wrap.css({
                    '-webkit-transform': deg,
                    '-mos-transform': deg,
                    '-o-transform': deg,
                    'transform': deg
                });
            } else {
                $wrap.css('filter', 'progid:DXImageTransform.Microsoft.BasicImage(rotation=' + (~~((file.rotation / 90) % 4 + 4) % 4) + ')');
            }
        });
        $li.appendTo($queue);
    }

    // 负责view的销毁
    function removeFile(file) {
        var $li = $('#' + file.id);

        delete percentages[file.id];
        updateTotalProgress();
        $li.off().find('.file-panel').off().end().remove();
    }

    function updateTotalProgress(file) {
        var loaded = 0,
            total = 0,
            spans = $progress.children(),
            percent;

        $.each(percentages, function (k, v) {
            total += v[0];
            loaded += v[0] * v[1];

        });

        percent = total ? loaded / total : 0;
        if (file != undefined && percent > 0) {
            tips.showProgress(file.id, file.name + "上传中：" + Math.round(percent * 100) + '%')
        }
        spans.eq(0).text(Math.round(percent * 100) + '%');
        spans.eq(1).css('width', Math.round(percent * 100) + '%');
        updateStatus();
    }

    function updateStatus() {
        var text = '', stats;

        if (state === 'ready') {
            text = '选中' + fileCount + '个文件，共' +
                WebUploader.formatSize(fileSize) + '。';
        } else if (state === 'confirm') {
            stats = uploader.getStats();
            if (stats.uploadFailNum) {
                text = '已成功上传' + stats.successNum + '个文件，' +
                    stats.uploadFailNum + '个文件上传失败，<a class="retry" href="#">重新上传</a>失败文件或<a class="ignore" href="#">忽略</a>'
            }

        } else {
            stats = uploader.getStats();
            text = '共' + fileCount + '个（' +
                WebUploader.formatSize(fileSize) +
                '），已上传' + stats.successNum + '个';

            if (stats.uploadFailNum) {
                text += '，失败' + stats.uploadFailNum + '个';
            }

        }
        //$info.html(text);
    }

    function setState(val) {
        var file, stats;

        if (val === state) {
            return;
        }

        $upload.removeClass('state-' + state);
        $upload.addClass('state-' + val);
        state = val;

        switch (state) {
            case 'pedding':
                $placeHolder.removeClass('element-invisible');
                $queue.hide();
                $statusBar.addClass('element-invisible');
                uploader.refresh();
                break;

            case 'ready':
                $placeHolder.addClass('element-invisible');
                $filePicker2.removeClass('element-invisible');
                $queue.show();
                $statusBar.removeClass('element-invisible');
                uploader.refresh();
                break;

            case 'uploading':
                $filePicker2.addClass('element-invisible');
                $progress.show();
                $upload.text('暂停上传');
                break;

            case 'paused':
                $progress.show();
                $upload.text('继续上传');
                break;

            case 'confirm':
                $progress.hide();
                $filePicker2.removeClass('element-invisible');
                $upload.text('开始上传');

                stats = uploader.getStats();
                if (stats.successNum && !stats.uploadFailNum) {
                    setState('finish');
                    return;
                }
                break;
            case 'finish':
                stats = uploader.getStats();
                if (stats.successNum) {
                    tips.closeProgressbox();
                    //alert('上传成功');
                } else {
                    // 没有成功的图片，重设
                    state = 'done';
                    location.reload();
                }
                break;
        }

        updateStatus();
    }

    uploader.onUploadProgress = function (file, percentage) {
        var $li = $('#' + file.id),
            $percent = $li.find('.progress span');

        $percent.css('width', percentage * 100 + '%');
        percentages[file.id][1] = percentage;
        updateTotalProgress(file);
    };

    uploader.onFileQueued = function (file) {
        fileCount++;
        fileSize += file.size;

        if (fileCount === 1) {
            $placeHolder.addClass('element-invisible');
            $statusBar.show();
        }

        if (!options.autoupload) {
            addFile(file);
        }
        setState('ready');

        updateTotalProgress(file);
        if (options.autoupload) {
            percentages[file.id] = [file.size, 0];
            file.rotation = 0;
            uploader.upload();
        }
    };

    uploader.onFileDequeued = function (file) {
        fileCount--;
        fileSize -= file.size;

        if (!fileCount) {
            setState('pedding');
        }

        removeFile(file);
        updateTotalProgress();

    };

    uploader.on('all', function (type) {
        var stats;
        switch (type) {
            case 'uploadFinished':
                setState('confirm');
                break;

            case 'startUpload':
                setState('uploading');
                break;

            case 'stopUpload':
                setState('paused');
                break;

        }
    });
    function showError(code) {
        switch (code) {
            case "Q_TYPE_DENIED": return "文件格式错误，支持" + extensionsf + "格式文件";
            case "Q_EXCEED_SIZE_LIMIT": return "文件太大，最大支持总量" + FullSizeLimit + "M大小的文件";
            case "F_EXCEED_SIZE": return "单个文件太大，最大支持" + SizeLimit + "M大小的文件";
            case "Q_EXCEED_NUM_LIMIT": return "文件数量超出";
            default: return code;
        }
    }
    uploader.onError = function (code) {
        tips.showFail(showError(code));
    };

    $upload.on('click', function () {
        if ($(this).hasClass('disabled')) {
            return false;
        }

        if (state === 'ready') {
            uploader.upload();
        } else if (state === 'paused') {
            uploader.upload();
        } else if (state === 'uploading') {
            uploader.stop();
        }
    });

    $info.on('click', '.retry', function () {
        uploader.retry();
    });

    $info.on('click', '.ignore', function () {
        alert('todo');
    });

    $upload.addClass('state-' + state);
    updateTotalProgress();


    uploader.on('uploadSuccess', function (file, data) {
        if (options.single) { fileCount = 0; fileSize = 0; uploader.removeFile(file); }

        var idsobj = options.valueElement == undefined ? $('#' + $elment + 'fujian') : $(options.valueElement);
        if (data.code == 200) {
            idsobj.val(data.msg);
            
            tips.showSuccess('上传成功');
        }
        else
            tips.showFail(data.msg);
        //if (idsobj.attr('data-' + data.type) != '' && idsobj.attr('data-' + data.type) != undefined) {
        //    if (data.type != undefined && data.type != '') {
        //        var idstr = idsobj.attr('data-' + data.type) + "," + data.id;
        //        idsobj.attr('data-' + data.type, idstr);
        //    }
        //} else {
        //    idsobj.attr('data-' + data.type, data.id);
        //}
    });
    //})
};


