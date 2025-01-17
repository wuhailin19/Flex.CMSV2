layui.define(['layer'], function (exports) {

    var formField = {
        components : {
            input: {
                id:'-1',
                index:'-1',uuid:'-1',IsEdit:false,
                label: "单行文本",
                tag: "input",
                tagIcon: 'input',
                placeholder: "请输入",
                defaultValue: null,
                labelWidth: 110,
                width:"100%",
                clearable: true,
                maxlength: null,
                showWordLimit: false,
                readonly: false,
                disabled: false,
                required: false,
                hideLabel:false,
                expression:"",
                document: '',
            },
            password: {
                id:'-1',
                index:'-1',uuid:'-1',IsEdit:false,
                label: "密码框",
                tag: "password",
                tagIcon: 'password',
                placeholder: "请输入",
                defaultValue: null,
                labelWidth: 110,
                width:"100%",
                clearable: true,
                maxlength: null,
                showWordLimit: false,
                readonly: false,
                disabled: false,
                required: false,
                hideLabel:false,
                document: ''
            },
            select: {
                id:'-1',
                index:'-1',uuid:'-1',IsEdit:false,
                label: "下拉框",
                tag: "select",
                tagIcon: 'select',
                labelWidth: 110,
                width:"100%",
                disabled: false,
                required: true,
                hideLabel:false,
                document: '',
                datasourceType: 'local',
                remoteUrl: 'http://',
                remoteMethod: 'post',
                remoteOptionText:'options.data.dictName',//映射到text
                remoteOptionValue:'options.data.dictId',//映射到value text和value可以是一样的
                remoteDefaultValue:'12',//表示对应的remoteOptionValue的值
                options: [
                    {
                        text: 'option1',
                        value: 'value1',
                        checked: true,
                    },
                    {
                        text: 'option2',
                        value: 'value2',
                        checked: false,
                    },
                    {
                        text: 'option3',
                        value: 'value3',
                        checked: false,
                    },
                ]
            },
            radio: {
                id:'-1',
                index:'-1',uuid:'-1',IsEdit:false,
                label: "单选组",
                tag: "radio",
                tagIcon: 'radio',
                labelWidth: 110,
                disabled: false,
                hideLabel:false,
                document: '',
                datasourceType: 'local',
                remoteUrl: 'http://',
                remoteMethod: 'post',
                remoteOptionText:'options.data.dictName',//映射到text
                remoteOptionValue:'options.data.dictId',//映射到value text和value可以是一样的
                options: [
                    {
                        text: 'option1',
                        value: 'value1',
                        checked: true,
                    },
                    {
                        text: 'option2',
                        value: 'value2',
                        checked: false,
                    },
                    {
                        text: 'option3',
                        value: 'value3',
                        checked: false,
                    },
                ]
            },
            checkbox: {
                id:'-1',
                index:'-1',uuid:'-1',IsEdit:false,
                label: "复选组",
                tag: "checkbox",
                tagIcon: 'checkbox',
                labelWidth: 110,
                disabled: false,
                required: true,
                hideLabel:false,
                document: '',
                LocalSource: true, //本地数据或者接口数据
                remoteUrl: '/api/ColumnContent/ContentOptions/{栏目Id}',
                remoteMethod: 'get',
                remoteOptionText:'options.data.dictName',//映射到text
                remoteOptionValue:'options.data.dictId',//映射到value text和value可以是一样的
                options: [
                    {
                        text: 'option1',
                        value: 'value1',
                        checked: true,
                    },
                    {
                        text: 'option2',
                        value: 'value2',
                        checked: true,
                    },
                    {
                        text: 'option3',
                        value: 'value3',
                        checked: false,
                    },
                ]
            },
            switch: {
                id:'-1',
                index:'-1',uuid:'-1',IsEdit:false,
                label: "开关",
                tag: "switch",
                tagIcon: 'switch',
                labelWidth: 110,
                width:"100%",
                switchValue: false,
                showWordLimit: false,
                disabled: false,
                hideLabel:false,
                document: '',
            },
            slider: {
                id:'-1',
                index:'-1',uuid:'-1',IsEdit:false,
                label: "滑块",
                tag: "slider",
                tagIcon: 'slider',
                labelWidth: 110,
                width:"100%",
                defaultValue: 10,
                maxValue: 100,
                minValue: 1,
                stepValue: 2,
                isInput:true,
                disabled: false,
                hideLabel:false,
                document: '',
            },
            numberInput: {
                id:'-1',
                index:'-1',uuid:'-1',IsEdit:false,
                label: "排序文本框",
                tag: "numberInput",
                tagIcon: 'numberInput',
                labelWidth: 110,
                width:"100%",
                defaultValue: 0,
                maxValue: 100,
                minValue: 0,
                stepValue: 1,
                disabled: false,
                hideLabel:false,
                document: '',
            },
            labelGeneration: {
                id:'-1',
                index:'-1',uuid:'-1',IsEdit:false,
                label: "标签组件",
                tag: "labelGeneration",
                tagIcon: 'labelGeneration',
                labelWidth: 110,
                width:"100%",
                isEnter: false,
                disabled: false,
                document: '',
            },
            bottom: {
                id:'-1',
                index:'-1',uuid:'-1',IsEdit:false,
                label: "按钮组件",
                tag: "bottom",
                tagIcon: 'bottom',
                labelWidth: 110,
                buttonIcon:"",
                buttonVlaue:"按钮",
                buttonType:"",
                buttonSize:"",
                disabled: false,
                hideLabel:false,
                document: '',
            },
            sign: {
                id:'-1',
                index:'-1',uuid:'-1',IsEdit:false,
                label: "签名组件",
                tag: "sign",
                tagIcon: 'sign',
                labelWidth: 110,
                buttonVlaue:"手写签名",
                buttonIcon:"",
                data:"",
                disabled: false,
                hideLabel:false,
                document: '',
            },
            iconPicker: {
                id:'-1',
                index:'-1',uuid:'-1',IsEdit:false,
                label: "图标选择器",
                tag: "iconPicker",
                tagIcon: 'iconPicker',
                labelWidth: 110,
                defaultValue: '',
                iconPickerSearch: true,
                iconPickerPage: true,
                iconPickerLimit: 12,
                iconPickerCellWidth: '43px',
                disabled: false,
                hideLabel:false,
                document: '',
            },
            cron: {
                id:'-1',
                index:'-1',uuid:'-1',IsEdit:false,
                label: "Cron表达式",
                tag: "cron",
                tagIcon: 'cron',
                placeholder: "请输入cron表达式,如:0 0 12 * * ?",
                labelWidth: 110,
                width:"100%",
                defaultValue: '* * * * * ?',
                cronUrl: '',
                disabled: false,
                required: true,
                hideLabel:false,
                document: '',
            },
            date: {
                id:'-1',
                index:'-1',uuid:'-1',IsEdit:false,
                label: "日期",
                tag: "date",
                tagIcon: 'date',
                labelWidth: 110,
                width:"100%",
                clearable: true,
                maxlength: null,
                dateDefaultValue: '2021-05-25',
                dateType: "date",//year month date time datetime
                range: false,
                dateFormat: "yyyy-MM-dd",
                isInitValue: false,
                dataMaxValue: "2088-12-31",
                dataMinValue: "1900-01-01",
                trigger: null,//自定义弹出控件的事件
                position: "absolute",//fixed,static,abolute
                theme: "default",
                mark: null,//每年的日期	{'0-9-18': '国耻'}	0 即代表每一年
                showBottom: true,
                zindex:66666666,
                disabled: false,
                required: true,
                hideLabel:false,
                document: '',
            },
            dateRange: {
                id:'-1',
                index:'-1',uuid:'-1',IsEdit:false,
                label: "日期范围",
                tag: "dateRange",
                tagIcon: 'dateRange',
                labelWidth: 110,
                //width:"100%",
                clearable: true,
                maxlength: null,
                dateType: "date",//year month date time datetime
                dateFormat: "yyyy-MM-dd",
                isInitValue: true,
                dataMaxValue: "2088-12-31",
                dataMinValue: "1900-01-01",
                trigger: null,//自定义弹出控件的事件
                position: "absolute",//fixed,static,abolute
                theme: "default",
                mark: null,//每年的日期	{'0-9-18': '国耻'}	0 即代表每一年
                showBottom: true,
                zindex:66666666,
                disabled: false,
                required: true,
                hideLabel:false,
                document: '',
            },
            rate: {
                id:'-1',
                index:'-1',uuid:'-1',IsEdit:false,
                label: "评分",
                tag: "rate",
                tagIcon: 'rate',
                labelWidth: 110,
                defaultValue: 0,
                rateLength: 5,//星星长度
                half: false,
                text: false,
                theme: "default",
                colorSelection:"#ffb800",
                showBottom: true,
                readonly: false,
                hideLabel:false,
                document: '',
            },
            carousel: {
                id:'-1',
                index:'-1',uuid:'-1',IsEdit:false,
                label: "轮播图",
                tag: "carousel",
                tagIcon: 'carousel',
                width: "100%",
                height: "500px",
                full: false,//是否全屏
                anim: "default", //轮播切换动画方式,
                interval: 3000,//切换时间 毫秒
                startIndex: 0,//初始索引
                arrow: "hover",//切换箭头默认显示状态
                autoplay: true,//是否自动切换
                document: '',
                datasourceType: 'local',
                remoteUrl: 'http://',
                remoteMethod: 'post',
                remoteOptionText:'options.data.dictName',//映射到text
                remoteOptionValue:'options.data.dictId',//映射到value text和value可以是一样的
                options: [
                    {
                        text: 'banner1',
                        value: './ayq/images/banner1.PNG',
                        checked: true,
                    },
                    {
                        text: 'banner2',
                        value: './ayq/images/banner2.PNG',
                        checked: false,
                    },
                    {
                        text: 'banner3',
                        value: './ayq/images/banner3.PNG',
                        checked: false,
                    },
                ]
            },
            colorpicker: {
                id:'-1',
                index:'-1',uuid:'-1',IsEdit:false,
                label: "颜色选择器",
                tag: "colorpicker",
                tagIcon: 'colorpicker',
                labelWidth: 110,
                defaultValue: 'rgba(0, 0, 0, 1)',
                colorformat: "#fff",
                alpha: false,
                colors: [],
                size: "",
                showBottom: true,
                disabled: false,
                hideLabel:false,
                document: '',
            },
            image: {
                id:'-1',
                index:'-1',uuid:'-1',IsEdit:false,
                label: "上传图片",
                tag: "image",
                tagIcon: 'image',
                placeholder: "请输入",
                defaultValue: null,
                labelWidth: null,
                disabled: false,
                required: true,
                document: '',
                uploadUrl: api +'Upload/UploadImage',
            },
            file: {
                id:'-1',
                index:'-1',uuid:'-1',IsEdit:false,
                label: "上传文件",
                tag: "file",
                tagIcon: 'file',
                placeholder: "请输入",
                defaultValue: null,
                labelWidth: null,
                disabled: false,
                required: true,
                document: '',
                uploadUrl: api+'Upload/UploadFile',
            },
            textarea: {
                id:'-1',
                index:'-1',uuid:'-1',IsEdit:false,
                label: "多行文本",
                tag: "textarea",
                tagIcon: 'textarea',
                placeholder: "请输入",
                defaultValue: null,
                width:"100%",
                readonly: false,
                disabled: false,//这里就是readonly的医生
                required: true,
                hideLabel:false,
                document: ''
            },
            editor: {
                id:'-1',
                index:'-1',uuid:'-1',IsEdit:false,
                label: "编辑器",
                tag: "editor",
                tagIcon: 'editor',
                width:"100%",
                clearable: true,
                maxlength: null,
                showWordLimit: false,
                menu: ['backColor', 'fontSize', 'foreColor', 'bold', 'italic', 'underline', 'strikeThrough', 'justifyLeft', 'justifyCenter', 'justifyRight', 'indent', 'outdent', 'insertOrderedList', 'insertUnorderedList', 'superscript', 'subscript', 'createLink', 'unlink', 'hr', 'face','table', 'files', 'music', 'video', 'insertImage', 'removeFormat', 'code', 'line'],
                height: "200px",
                uploadUrl: '/upload/',
                disabled:false,
                hideLabel:false,
                defaultValue:'',
                document: ''
            },
            blockquote: {
                id:'-1',
                index:'-1',uuid:'-1',IsEdit:false,
                label: "便签信息",
                tag: "blockquote",
                tagIcon: 'blockquote',
                defaultValue: "便签信息",
                width:"100%",
                colorSelection:"#5fb878",
                document: ''
            },
            line: {
                id:'-1',
                index:'-1',uuid:'-1',IsEdit:false,
                label: "分割线",
                tag: "line",
                tagIcon: 'line',
                defaultValue: "分割线",
                width:"100%",
                colorSelection:"#5fb878",
                document: ''
            },
            spacing: {
                id:'-1',
                index:'-1',uuid:'-1',IsEdit:false,
                label: "间距",
                tag: "spacing",
                tagIcon: 'spacing',
                defaultValue: "间距",
                whiteSpace:"30",
                document: ''
            },
            textField: {
                id:'-1',
                index:'-1',uuid:'-1',IsEdit:false,
                label: "HTML",
                tag: "textField",
                tagIcon: 'textField',
                defaultValue: "HTML",
                document: ''
            },
            grid:{
                id:'-1',
                index:'-1',uuid:'-1',IsEdit:false,
                tag: 'grid',
                span: 2,
                columns: [
                    {
                        span: 12,
                        list: [],
                    },
                    {
                        span: 12,
                        list: [],
                    }
                ]
            }
        },
        componentsLang : [
            {
                component : "c1",
                name:"输入型组件(基于layui)",
                list:[
                    {"key":"input","icon":"layui-icon layui-icon-layer"},
                    {"key":"password","icon":"layui-icon layui-icon-auz"},
                    {"key":"textarea","icon":"layui-icon layui-icon-list"}
                ]
            },
            {
                component : "c2",
                name:"选择型组件(基于layui)",
                list:[
                    {"key":"select","icon":"layui-icon layui-icon-align-left"},
                    {"key":"radio","icon":"layui-icon layui-icon-radio"},
                    {"key":"checkbox","icon":"layui-icon layui-icon-list"},
                    {"key":"switch","icon":"layui-icon layui-icon-key"},
                    {"key":"slider","icon":"layui-icon layui-icon-slider"},
                    {"key":"date","icon":"layui-icon layui-icon-time"},
                    {"key":"rate","icon":"layui-icon layui-icon-rate-solid"},
                    {"key":"carousel","icon":"layui-icon layui-icon-carousel"},
                    {"key":"colorpicker","icon":"layui-icon layui-icon-theme"},
                    {"key":"image","icon":"layui-icon layui-icon-picture"},
                    {"key":"file","icon":"layui-icon layui-icon-export"},
                    {"key":"dateRange","icon":"layui-icon layui-icon-date"}
                ]
            },
            {
                component : "c3",
                name:"布局型组件(基于layui)",
                list:[
                    {"key":"grid","icon":"layui-icon layui-icon-layer"},
                    {"key":"blockquote","icon":"layui-icon layui-icon-note"},
                    {"key":"line","icon":"layui-icon layui-icon-subtraction"},
                    {"key":"spacing","icon":"layui-icon layui-icon-more-vertical"},
                    {"key":"bottom","icon":"layui-icon layui-icon-prev-circle"}
                ]
            },
            {
                component : "c4",
                name:"扩展组件(基于layui)",
                list:[
                    {"key":"numberInput","icon":"layui-icon layui-icon-top"},
                    {"key":"iconPicker","icon":"layui-icon layui-icon-auz"},
                    {"key":"cron","icon":"layui-icon layui-icon-survey"},
                    //{"key":"labelGeneration","icon":"layui-icon layui-icon-auz"},
                    {"key":"sign","icon":"layui-icon layui-icon-layer"}
                ]
            },
            {
                component : "c5",
                name:"扩展组件(外部)",
                list:[
                    {"key":"editor","icon":"layui-icon layui-icon-layer"}
                ]
            }
        ],
    }
    exports('formField', formField);

});