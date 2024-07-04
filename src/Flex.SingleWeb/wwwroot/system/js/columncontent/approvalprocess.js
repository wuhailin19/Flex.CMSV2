var parent_json = parent.req_Data;
var container = $('.container');
ajaxHttp({
    url: api + 'Message/GetApprovalProcess/' + parent.currentmodelId + '/' + parent_json.Id,
    type: 'Get',
    async: false,
    dataType: 'json',
    success: function (json) {
        if (json.code != 200) {
            container.append('<p class="boxcenter">无数据</p>')
            return;
        }
        var data = json.content;

        for (var i = 0; i < data.length; i++) {
            // 创建一个列表容器
            var $listbox = $('<ul class="listbox"></ul>');
            appendProcess(data[i]);
            // 将列表容器添加到主容器
            container.append('<p class="listtitle">第' + (data.length - i) + '次审批：</p>')
            container.append($listbox);
        }
        // 递归处理链表
        function appendProcess(process) {
            let className = '';
            className = process.IsStart ? 'start' : process.IsEnd ? 'end' : 'normal';
            if (className == '') {
                className = process.MessageCate == 2 ? 'reject' : process.MessageCate == 4 ? 'veto' : 'normal';
            }
            $listbox.append(`
                    <li class="item ${className}">
                        <p>标题：${process.Title}</p>
                        <p>参与人：${process.AddUserName}</p>
                        <p>流程类型：${process.MessageResult}</p>
                    </li>
                `);
            if (process.next) {
                $listbox.append('<li class="line iconfont">&#xe607;</li>');
                appendProcess(process.next);
            }
        }
    }
})