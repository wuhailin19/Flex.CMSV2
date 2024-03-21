(function ($) {
    var myflow = $.myflow;

    $.extend(true, myflow.config.rect, {
        attr: {
            r: 8,
            fill: '#F6F7FF',
            stroke: '#03689A',
            "stroke-width": 2
        }
    });

    $.extend(true, myflow.config.props.props, {
        wkname: { name: 'wkname', label: '名称', value: '', editor: function () { return new myflow.editors.inputEditor(); } },
        //key : {name:'key', label:'标识', value:'', editor:function(){return new myflow.editors.inputEditor();}},
        wkdesc: { name: 'wkdesc', label: '描述', value: '', editor: function () { return new myflow.editors.inputEditor(); } }
    });


    $.extend(true, myflow.config.tools.states, {
        start: {
            showType: 'image',
            type: 'start',
            name: { text: '<<start>>' },
            text: { text: '开始' },
            img: { src: '/content/myflow-min/img/48/start_event_empty.png', width: 48, height: 48 },
            attr: { width: 50, heigth: 50 },
            props: {
                text: { name: 'text', label: '显示', value: '', editor: function () { return new myflow.editors.textEditor(); }, value: '开始' }

            }
        },
        end: {
            showType: 'image', type: 'end',
            name: { text: '<<end>>' },
            text: { text: '结束(通过审核)' },
            img: { src: '/content/myflow-min/img/48/end_event_terminate.png', width: 48, height: 48 },
            attr: { width: 50, heigth: 50 },
            props: {
                text: { name: 'text', label: '显示', value: '', editor: function () { return new myflow.editors.textEditor(); }, value: '结束(通过审核)' }
            }
        },
        'end-cancel': {
            showType: 'image', type: 'end-cancel',
            name: { text: '<<end-cancel>>' },
            text: { text: '否决(请求不通过)' },
            img: { src: '/content/myflow-min/img/48/end_event_cancel.png', width: 48, height: 48 },
            attr: { width: 50, heigth: 50 },
            props: {
                text: { name: 'text', label: '显示', value: '', editor: function () { return new myflow.editors.textEditor(); }, value: '否决(请求不通过)' }
            }
        },
        'end-error': {
            showType: 'image', type: 'end-error',
            name: { text: '<<end-error>>' },
            text: { text: '取消(退回草稿)' },
            img: { src: '/content/myflow-min/img/48/end_event_error.png', width: 48, height: 48 },
            attr: { width: 50, heigth: 50 },
            props: {
                text: { name: 'text', label: '显示', value: '', editor: function () { return new myflow.editors.textEditor(); }, value: '取消(退回草稿)' }
            }
        },
        state: {
            showType: 'text', type: 'state',
            name: { text: '<<state>>' },
            text: { text: '状态' },
            img: { src: '/content/myflow-min/img/48/task_empty.png', width: 48, height: 48 },
            props: {
                text: { name: 'text', label: '显示', value: '', editor: function () { return new myflow.editors.textEditor(); }, value: '状态' },
                temp1: { name: 'temp1', label: '文本', value: '', editor: function () { return new myflow.editors.inputEditor(); } }
            }
        },
        fork: {
            showType: 'image', type: 'fork',
            name: { text: '<<fork>>' },
            text: { text: '分支' },
            img: { src: '/content/myflow-min/img/48/gateway_parallel.png', width: 48, height: 48 },
            attr: { width: 50, heigth: 50 },
            props: {
                text: { name: 'text', label: '显示', value: '', editor: function () { return new myflow.editors.textEditor(); }, value: '分支' },
                temp1: { name: 'temp1', label: '文本', value: '', editor: function () { return new myflow.editors.inputEditor(); } },
                temp2: { name: 'temp2', label: '选择', value: '', editor: function () { return new myflow.editors.selectEditor('select.json'); } }
            }
        },
        join: {
            showType: 'image', type: 'join',
            name: { text: '<<join>>' },
            text: { text: '合并' },
            img: { src: '/content/myflow-min/img/48/gateway_parallel.png', width: 48, height: 48 },
            attr: { width: 50, heigth: 50 },
            props: {
                text: { name: 'text', label: '显示', value: '', editor: function () { return new myflow.editors.textEditor(); }, value: '合并' },
                temp1: { name: 'temp1', label: '文本', value: '', editor: function () { return new myflow.editors.inputEditor(); } },
                temp2: { name: 'temp2', label: '选择', value: '', editor: function () { return new myflow.editors.selectEditor('select.json'); } }
            }
        },
        task: {
            showType: 'text', type: 'task',
            name: { text: '<<task>>' },
            text: { text: '步骤' },
            img: { src: '/content/myflow-min/img/48/task_empty.png', width: 48, height: 48 },
            props: {
                text: { name: 'text', label: '名称', value: '', editor: function () { return new myflow.editors.textEditor(); }, value: '步骤' },
                //org: { name: 'org', label: '部门', value: '', editor: function () { return new myflow.editors.selectEditor([{ name: '所有部门参与审核', value: 0 }, { name: '仅所属部门可审核', value: 1 }]); } },
                //avoid: { name: 'avoid', label: '回避', value: '', editor: function () { return new myflow.editors.selectEditor([{ name: '可审核自己内容', value: 0 }, { name: '不可审核自己内容', value: 1 }]); } }

            }
        }
    });
})(jQuery);