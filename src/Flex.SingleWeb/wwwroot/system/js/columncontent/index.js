var routeLink = api + 'ColumnContent/';
var routePageLink = SystempageRoute + 'ColumnContent/';
var columnlist;
var req_Data;
var parentjson;

var btnpermission;
var dateRanage = ['', ''];
var keyword = $('#keyword');
var tableid = 'testReloadf';

function getParameterFromUrl() {
    var url = window.location.href.toLowerCase();
    var match = url.match(/\/system\/columncontent\/index\/(\d+)/);
    if (match && match[1]) {
        var parameterValue = match[1];
        return parameterValue;
    } else {
        return null;
    }
}

ajaxHttp({
    url: routeLink + 'Column/' + currentmodelId + '/' + currentparentId,
    type: 'Get',
    async: false,
    dataType: 'json',
    success: function (result) {
        btnpermission = result.content;
    },
    complete: function () { }
})
columnlist = btnpermission.TableHeads;
layui.use(['form', 'laydate', 'util', "table", 'dropdown', 'upload', 'element'], function () {
    var form = layui.form;
    var laydate = layui.laydate;
    var table = layui.table;
    var dropdown = layui.dropdown;
    var upload = layui.upload
    var upIns = undefined;
    var element = layui.element;
    var progressboxindex = undefined;

    var toolbarhtml = '<div class="layui-btn-container">';
    // 日期范围 - 左右面板独立选择模式
    laydate.render({
        elem: '#ID-laydate-range',
        range: ['#ID-laydate-start-date', '#ID-laydate-end-date'],
        rangeLinked: true,
        done: function (value, date, startdate) {
            dateRanage = value.split(' - ');
            //执行重载
            table.reload('testReloadf', {
                page: {
                    curr: 1 //重新从第 1 页开始
                }
                , where: {
                    ParentId: currentparentId,
                    k: keyword.val(),
                    ModelId: currentmodelId,
                    PId: pId,
                    timefrom: dateRanage[0],
                    timeto: dateRanage[1]
                }
            });
        }
    });
    if (btnpermission.IsAdd)
        toolbarhtml += '<button class="layui-btn layui-btn-sm" lay-event="addRole">添加</button>';
    if (btnpermission.IsUpdate) {
        toolbarhtml += '<button class="layui-btn layui-btn-sm" lay-event="Edit">编辑</button>';
        toolbarhtml += '<button class="layui-btn layui-btn-sm" id="setProperty">设置属性<i class="layui-icon layui-icon-down layui-font-12"></i></button>';
        toolbarhtml += '<button class="layui-btn layui-btn-sm" id="cacelProperty">取消属性<i class="layui-icon layui-icon-down layui-font-12"></i></button>';
        toolbarhtml += '<button class="layui-btn layui-btn-sm" lay-event="DataTools">数据操作</button>';
        toolbarhtml += '<button class="layui-btn layui-btn-sm" lay-event="Import" id="Importdata">导入</button>';
        toolbarhtml += '<button class="layui-btn layui-btn-sm Export" lay-event="Export">导出</button>';
        toolbarhtml += '<button class="layui-btn layui-btn-sm" lay-event="History">修改历史</button>';
        toolbarhtml += '<button class="layui-btn layui-btn-sm" lay-event="Delete">回收站</button>';
        toolbarhtml += '<button class="layui-btn layui-btn-sm" lay-event="ApprovalProcess">审批流程</button>';
    }
    if (btnpermission.IsDelete)
        toolbarhtml += '<button class="layui-btn layui-btn-sm layui-btn-danger" lay-event="deleteAll">删除</button>';
    toolbarhtml += '</div>';



    //JS 调用：
    var insTb = table.render({
        elem: '#demo_tree'
        , url: routeLink + 'ListAsync'
        , height: 'full-80'
        , id: tableid
        , headers: httpTokenHeaders
        , toolbar: toolbarhtml
        , limits: [1, 5, 10, 15, 50, 500]
        , limit: 15
        , cellExpandedMode: 'tips'
        , page: true //开启分页
        , response: {
            statusCode: 200
        },
        where: {
            ParentId: currentparentId,
            PId: pId,
            ModelId: currentmodelId,
            k: ' '
        },
        parseData: function (res) {
            return {
                "code": res.code, //数据状态的字段名称，默认：code
                "count": res.content.TotalCount, //状态信息的字段名称，默认：msg
                "data": res.content.Items//数据总数的字段名称，默认：count
            };
        }
        , method: 'Get'
        , cols: [columnlist]
        , done: function (res, pageindex, count) {
            if (pageindex > 1 && res.data.length === 0) {
                insTb.reload({
                    page: {
                        curr: pageindex - 1
                    },
                });
            }

            upIns = upload.render({
                elem: '#Importdata' //绑定元素
                , url: routeLink + 'ImportExcel' //上传接口
                , headers: httpTokenHeaders
                , accept: 'file'
                , exts: 'xls|xlsx'
                , field: 'file'
                , auto: false //选择文件后不自动上传
                , before: function () {
                    element.progress('lock_progress', '0%');
                    progressboxindex = layer.open({
                        type: 1,
                        title: '上传进度',
                        closeBtn: 0,
                        area: ['300px', '100px'],
                        shadeClose: false,
                        content: $("#uploadLoadingDiv").html(),
                        offset: '100px'
                    });
                }
                , choose: function (obj) {
                    //将每次选择的文件追加到文件队列
                    var files = obj.pushFile();

                    //预读本地文件，如果是多文件，则会遍历。(不支持ie8/9)
                    obj.preview(function (index, file, result) {
                        var reader = new FileReader();

                        reader.onload = function (e) {
                            var data = new Uint8Array(e.target.result);
                            var workbook = XLSX.read(data, { type: 'array' });

                            var firstSheetName = workbook.SheetNames[0];
                            var worksheet = workbook.Sheets[firstSheetName];

                            // 获取表头（假设表头在第一行）
                            var headers = [];
                            var range = XLSX.utils.decode_range(worksheet['!ref']);
                            var firstRow = range.s.r; // 第一行的索引

                            for (var col = range.s.c; col <= range.e.c; col++) {
                                var cellAddress = XLSX.utils.encode_cell({ r: firstRow, c: col });
                                var cell = worksheet[cellAddress];

                                if (cell && cell.t) {
                                    headers.push(cell.v);
                                }
                            }
                            ajaxHttp({
                                url: api + 'Field/GetFieldDictAsync/' + currentmodelId,
                                type: 'get',
                                success: function (result) {
                                    var jsondata = result.content;
                                    if (!jsondata || jsondata.length == 0) {
                                        tips.showFail("数据为空");
                                        return;
                                    }

                                    var columnstr = '<option value="0">不选择</option>';
                                    for (var j = 0; j < headers.length; j++) {

                                        columnstr += "<option value='" + headers[j] + "'>" + headers[j] + "</option>";
                                    }
                                    var filedhtmlstr = '';

                                    // 遍历 Fields
                                    $.each(jsondata.Fileds, function (key, value) {
                                        filedhtmlstr += '<div class="layui-form-item layui-form-text" style="margin-bottom:0px;">';
                                        filedhtmlstr += '<label class="layui-form-label" style="text-align: left;">' + key + '<br>' + value + '</label>';
                                        filedhtmlstr += '<div class="layui-input-block layui-form-select selectbox">';
                                        filedhtmlstr += '<select lay-append-to="body" lay-append-position="fixed" lay-search="" data-field="' + key + '">';
                                        filedhtmlstr += columnstr;
                                        filedhtmlstr += "</select>";
                                        filedhtmlstr += "</div>";
                                        filedhtmlstr += "</div>";
                                    })
                                    layer.open({
                                        type: 1,
                                        title: "字段选择",
                                        area: ['50%', '80%'], // 宽高
                                        content: '<div class="layui-form choosebox" style="padding: 3%;">'
                                            + filedhtmlstr
                                            + '<div class="layui-form-item">'
                                            + '<div class="layui-input-block">'
                                            + '<button class="layui-btn" data-type="choose">立即提交</button>'
                                            + '</div>'
                                            + '</div></div>',
                                        success: function (layero) {
                                            // 定向渲染 select
                                            form.render(layero.find('.layui-form select'));
                                            // 鼠标滑动 layer 内部滚动条时移除下拉框，以规避错位
                                            // 若 layer 内部不存在滚动条，以下代码可删除
                                            var selectElem = layero.find('.layui-form-select');
                                            layero.find('.layui-layer-content').on('scroll', function () {
                                                selectElem.removeClass('layui-form-selected');
                                                layui.$('.layui-select-panel-wrap').detach();
                                            });

                                        },
                                        end: function () {
                                            //upIns.reload();
                                        }
                                    });
                                }
                            })
                        };

                        reader.readAsArrayBuffer(file);
                    });
                }
                , progress: function (n, elem, res, index) {
                    var percent = n + '%' //获取进度百分比
                    element.progress('lock_progress', percent); //可配合 layui 进度条元素使用
                    element.progress('lock_progress', n + '%'); //进度条
                    if (n == 100) {
                        layer.close(progressboxindex);
                    }
                }
            })
            $('.realtionbtn').click(function () {
                let widthstr = '95%';
                let heightstr = '95%';
                let link = $(this).attr('data-href');
                let title = $(this).text();
                //iframe窗
                layer.open({
                    type: 2,
                    title: title,
                    shadeClose: true,
                    shade: 0.5,
                    maxmin: true, //开启最大化最小化按钮
                    area: [widthstr, heightstr],
                    content: link,
                    end: function () {

                    }
                });
            })
            var id = this.id;
            // 设置属性
            dropdown.render({
                elem: '#setProperty', // 可绑定在任意元素中，此处以上述按钮为例
                data: [
                    { id: 'IsTop', title: '置顶' },
                    { id: 'IsRecommend', title: '推荐' },
                    { id: 'IsHot', title: '热门' },
                    { id: 'IsSilde', title: '焦点' },
                    { id: 'IsHide', title: '隐藏' }
                ],
                // 菜单被点击的事件
                click: function (obj) {
                    var checkStatus = table.checkStatus(id)
                    var data = checkStatus.data; // 获取选中的数据
                    let ids = getIdsFromList(data);
                    UpdateStatus(ids, obj.id, true, true);
                }
            });
            // 取消属性
            dropdown.render({
                elem: '#cacelProperty', // 可绑定在任意元素中，此处以上述按钮为例
                data: [
                    { id: 'IsTop', title: '取消置顶' },
                    { id: 'IsRecommend', title: '取消推荐' },
                    { id: 'IsHot', title: '取消热门' },
                    { id: 'IsSilde', title: '取消焦点' },
                    { id: 'IsHide', title: '取消隐藏' }
                ],
                // 菜单被点击的事件
                click: function (obj) {
                    var checkStatus = table.checkStatus(id)
                    var data = checkStatus.data; // 获取选中的数据
                    let ids = getIdsFromList(data);
                    UpdateStatus(ids, obj.id, false, true);
                }
            });
        }
    });

    function getIdsFromList(data) {
        let ids = '';
        $.each(data, function (index, item) {
            if (ids == '')
                ids += item.Id;
            else
                ids += ',' + item.Id;
        })
        return ids;
    }

    // 复选框事件
    table.on('checkbox(test)', function (obj) {

    });
    // 单元格编辑事件
    table.on('edit(test)', function (obj) {
        var field = obj.field; // 得到字段
        var value = obj.value; // 得到修改后的值
        var data = obj.data; // 得到所在行所有键值
        // 值的校验
        if (field === 'OrderId') {
            if (!/[0-9]+$/.test(obj.value)) {
                layer.tips('输入的排序号格式不正确，请重新编辑', this, { tips: 1 });
                return obj.reedit(); // 重新编辑 -- v2.8.0 新增
            }
        }
        value = parseInt(value);

        UpdateStatus(data.Id, field, value);

    });
    function UpdateStatus(ids, event, result, reload) {
        if (ids == "") {
            layer.msg("选择一条数据", { icon: 5, time: 1000 })
            return;
        }
        var model = { ParentId: currentparentId, Ids: ids, ModelId: currentmodelId };
        model[event] = result;
        ajaxHttp({
            url: routeLink + 'UpdateStatus',
            type: 'Post',
            data: JSON.stringify(model),
            async: false,
            dataType: 'json',
            success: function (result) {
                if (result.code == 200) {
                    if (reload) {
                        //执行重载
                        table.reload(tableid, {
                            page: {
                                curr: insTb.config.page.curr //刷新当前页码
                            }
                            , where: {
                                ParentId: currentparentId,
                                k: keyword.val(),
                                PId: pId,
                                ModelId: currentmodelId,
                                timefrom: dateRanage[0],
                                timeto: dateRanage[1]
                            }
                        });
                    }
                    tips.showSuccess(result.msg);
                }
            },
            complete: function () { }
        })
    }
    var $ = layui.$, active = {
        reload: function () {
            //执行重载
            table.reload(tableid, {
                page: {
                    curr: 1 //重新从第 1 页开始
                }
                , where: {
                    ParentId: currentparentId,
                    PId: pId,
                    ModelId: currentmodelId,
                    k: keyword.val(),
                    timefrom: dateRanage[0],
                    timeto: dateRanage[1]
                }
            });
        },
        choose: function () {
            
            var selects = document.querySelectorAll('.choosebox select');
            var results = [];

            selects.forEach(function (select) {
                var field = select.getAttribute('data-field');
                var value = select.value;
                results.push({
                    field: value,
                    value: field
                });
            });
            upIns.config.data = { 'ParentId': currentparentId, 'ModelId': currentmodelId, 'PId': pId, 'FieldDictStr': JSON.stringify(results) };
            upIns.config.done = function (res, index, upload) {
                if (res.code == 200) {
                    tips.showSuccess(res.msg);
                }
                else
                    tips.showFail(res.msg);
            }
            upIns.upload();
            
        }
    };
    $(document).on('click', '.layui-btn', function () {
        var type = $(this).data('type');
        active[type] ? active[type].call(this) : '';
    });


    async function startExport() {
        var url = routeLink + "ExportExcel?ParentId=" + currentparentId + "&ModelId=" + currentmodelId
            + "&PId=" + pId + "&k=" + keyword.val() + "&timefrom=" + dateRanage[0] + "&timeto=" + dateRanage[1];
        ;
        const response = await fetch(url, {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + sessionStorage.getItem('access_token'),
                'Refresh_token': 'Bearer ' + sessionStorage.getItem('refresh_token'),
            }
        });
        if (response.ok) {
            const json = await response.json();
            if (json.code == 200) {
                tips.showSuccess(json.msg);
            } else {
                tips.showFail(json.msg);
            }
        } else {
            console.error('导出请求失败');
        }
    }
    //监听事件
    table.on('toolbar(test)', function (obj) {
        var data = table.checkStatus(obj.config.id).data;
        switch (obj.event) {
            case 'addRole':
                defaultOptions.setAddIframe(layer, insTb);
                break;
            case 'History':
                if (data.length != 1) {
                    layer.msg("选择一条数据", { icon: 5, time: 1000 })
                    return;
                }
                req_Data = data[0];
                defaultOptions.openDataPermissionIframe(layer, insTb);
                break;
            case "Export":
                startExport();
                break;
            case "Import":
                
                
                break;
            case 'ApprovalProcess':
                if (data.length != 1) {
                    layer.msg("选择一条数据", { icon: 5, time: 1000 })
                    return;
                }
                req_Data = data[0];
                defaultOptions.openApprovalProcessIframe(layer, insTb);
                break;
            case 'DataTools':
                if (data.length == 0) {
                    layer.msg("先选择数据", { icon: 5, time: 1000 })
                    return;
                }
                let ids = getIdsFromList(data);
                defaultOptions.openCopyContentIframe(layer, insTb, ids);
                break;
            case 'Delete':
                req_Data = data[0];
                defaultOptions.openSoftDeleteIframe(layer, insTb);
                break;
            case 'Edit':
                if (data.length != 1) {
                    layer.msg("选择一条数据", { icon: 5, time: 1000 })
                    return;
                }
                req_Data = data[0];
                defaultOptions.editIframe(layer, insTb, data.Id);
                break;
            case 'deleteAll':
                if (data.length == 0)
                    return;
                //获取所有选中节点id数组
                var nodeIds = defaultOptions.getCheckedId(data);
                layer.confirm('确定删除选中数据吗？', { btn: ['确定删除', '取消'] }, function (index) {
                    ajaxHttp({
                        url: routeLink + currentmodelId + "/" + currentparentId + "/" + nodeIds,
                        type: 'Post',
                        async: false,
                        success: function (json) {
                            if (json.code == 200) {
                                tips.showSuccess(json.msg);
                                // 删除
                                delete_index = [];
                                defaultOptions.callBack(insTb);
                            } else {
                                tips.showFail(json.msg);
                                delete_index = [];
                            }
                        },
                        complete: function () { }
                    })
                    layer.close(index)
                })
                break;
        };
    });
})