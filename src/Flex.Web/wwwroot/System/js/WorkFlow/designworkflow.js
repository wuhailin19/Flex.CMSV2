var lock = false;
var parent_json = parent.req_Data;
//步骤和动作配置
var actArray = new Array();
var stepArray = new Array();

$(function () {
    var jsonRes = parent_json.WorkFlowContent;
    if ('' == jsonRes) {
        jsonRes = '{}';
    }
    $('#myflow').myflow({
        basePath: "",
        restore: eval("(" + jsonRes + ")"),
        tools: {
            save: function (data) {
                var sjson = [];
                for (var key in stepArray) {
                    var jsono = {};
                    jsono[key] = stepArray[key];
                    sjson.push(jsono);
                }
                var ajson = [];
                for (var key in actArray) {
                    var jsono = {};
                    jsono[key] = actArray[key];

                    ajson.push(jsono);
                }
                save(data, sjson, ajson);
                //window.localStorage.setItem("data",data)
            },
            publish: function (data) {
                //console.log("发布",eval("("+data+")"));
            },
            addPath: function (id, data) {
                //console.log("添加路径",id,eval("("+data+")"));
            },
            addRect: function (id, data) {
                //console.log("添加状态",id,eval("("+data+")"));
            },
            clickPath: function (id) {
                //console.log("点击线",id)
            },
            clickRect: function (id, data) {
                //console.log("点击状态",id,eval("("+data+")"));
            },
            deletePath: function (id) {
                //console.log("删除线",id);
            },
            deleteRect: function (id, data) {
                //console.log("删除状态",id,eval("("+data+")"));
            },
            revoke: function (id) {

            }
        }
    });
});
function init() {
    var actionNS = parent_json.actionString;
    var stepOrg = '';
    var stepRole = '';
    var stepMan = '';

    //解决设计器初始化action丢失
    $("tspan").each(function () {
        var ns = actionNS.split(',');
        for (var i = 0; i < ns.length; i++) {
            if (ns[i] == $(this).text()) {
                $(this).click();
                break;
            }
        }

    });

    $(document).click();
    $('#input_pwkname').val(parent_json.Name);
    $('#input_pwkdesc').val(parent_json.Introduction);
}

function deleteNodeOrPath() {
    layer.confirm('您确认删除？', {
        btn: ['确认', '取消'] //按钮
        // ,offset: [msgH+'px', msgW+'px']
        , icon: 7
    }, function () {
        $(document).trigger('keydown', true);
        layer.closeAll('dialog');
    }, function (index) {
        lock = false;
        //layer.close(index);
    });
}

function openEditFlowStepDialog(flowId, step, stepName) {
    layer.open({
        type: 2
        , title: stepName + ' - 审核参与者'
        , shadeClose: false
        , scrollbar: false
        , shade: [0.3, '#393D49']
        , maxmin: false
        , content: api + '/workflow/EditWorkflowManager?flowId=' + flowId + '&stepDesc=' + step
        , area: ['850px', '80%']
        , end: function () {
            //window.location.reload();
            // $('#myflow_save').click();
        }
    });
}
function save(djson, sjson, ajson) {
    if (!lock) {
        lock = true;//锁定
        layer.closeAll('dialog');
        var dj = eval("(" + djson + ")");
        var errorFlag = false;
        var actNameArray = new Array();
        var stepNameArray = new Array();

        for (var keyp in dj.paths) {
            var ob = dj.paths[keyp];
            for (var keys in dj.states) {
                var obs = dj.states[keys];
                //alert( JSON.stringify(obs.text.text))
                stepNameArray.push(JSON.stringify(obs.text.text));
                actNameArray.push(JSON.stringify(ob.text.text));
                //alert( JSON.stringify(ob.text.text))
                if (ob.from == keys && (obs.type == 'end' || obs.type == 'end-cancel' || obs.type == 'end-error')) {
                    layer.alert('结束状态节点不能指向任何步骤',
                        {
                            title: '提示'
                            , icon: 7
                            //,offset: [msgH+'px' , msgW+'px']
                            , shadeClose: false
                            , shade: [0.05, '#393D49']
                            , closeBtn: 0
                        },
                        function (index) {
                            layer.closeAll();
                        }
                    );
                    errorFlag = true;
                    break;
                }
                if (obs.text.text == '') {
                    //console.log(obs)
                    layer.alert('存在没有命名的步骤',
                        {
                            title: '提示'
                            , icon: 7
                            //,offset: [msgH+'px' , msgW+'px']
                            , shadeClose: false
                            , shade: [0.05, '#393D49']
                            , closeBtn: 0
                        },
                        function (index) {
                            layer.closeAll();
                        }
                    );
                    errorFlag = true;
                    break;
                }
            }
            if (ob.text.text == '') {
                layer.alert('存在没有命名的动作',
                    {
                        title: '提示'
                        , icon: 7
                        //,offset: [msgH+'px' , msgW+'px']
                        , shadeClose: false
                        , shade: [0.05, '#393D49']
                        , closeBtn: 0
                    },
                    function (index) {
                        layer.closeAll();
                    }
                );
                errorFlag = true;
                break;
            }
        }
        //动作不能以步骤名称开头
        stepNameArray = Array.from(new Set(stepNameArray));
        actNameArray = Array.from(new Set(actNameArray));
        for (var index in actNameArray) {
            for (var indexSt in stepNameArray) {
                if (actNameArray[index] == stepNameArray[indexSt]) {
                    layer.alert('动作名称 ' + actNameArray[index] + ' 不能以已存在的相同步骤名开头，建议命名:提交XX,退回XX 等行为类描叙',
                        {
                            title: '提示'
                            , icon: 7
                            //,offset: [msgH+'px' , msgW+'px']
                            , shadeClose: false
                            , shade: [0.05, '#393D49']
                            , closeBtn: 0
                        },
                        function (index) {
                            layer.closeAll();
                        }
                    );
                    errorFlag = true;
                    break;
                }
            }
            if (errorFlag) {
                break;
            }
        }
        //不能存在多个起始节点
        //流程是否完整，至少包含一个通过节点，从起始到终结动作是否连通
        if (errorFlag) {
            lock = false;
            return;
        }
        layer.msg('正在执行...', {
            icon: 16
            , shade: 0.01
            , time: 60 * 1000
        });
        var url = api + "WorkFlow";
        var postData =
        {
            Id: parent_json.Id,
            design: encodeData(djson),
            stepDesign: encodeData(JSON.stringify(sjson)),
            actDesign: encodeData(JSON.stringify(ajson))
        }
        ajaxHttp({
            type: "Post",
            url: url,
            data: JSON.stringify(postData),
            datatype: 'json',
            success: function (json) {
                if (json.code == 200) {
                    tips.showSuccess(json.msg);
                }
                else {
                    tips.showFail(json.msg);
                }
            }
        });
    }//锁结束
}