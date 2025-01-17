﻿var defaultOptions = {
    addwidths: 800,
    addheights: 800,
    editwidths: 800,
    editheights: 800,
    operawidths: 400,
    operaheights: 400,
    datawidths: 1000,
    dataheights: 500,
    isEditReload: true,
    isOperaReload: true,
    addTitle: '添加',
    editTitle: '编辑',
    operationTitle: '修改',
    IdName: 'Id',
    setAddIframe: function (layer, insTb) {
        var self = this;
        let widthstr = "80%";
        let heightstr = "80%";
        //iframe窗
        layer.open({
            type: 2,
            title: self.addTitle,
            shadeClose: true,
            shade: false,
            maxmin: true, //开启最大化最小化按钮
            area: [widthstr, heightstr],
            content: routeLink + 'AddPage',
            end: function () {
                defaultOptions.callBack(insTb);
            }
        });
    },
    callBack: function (tableIns) {
        //第二次调用
        tableIns.reload({
            where: { 'username': "user-0" } // 设定异步数据接口的额外参数，任意设
            , page: {
                curr: tableIns.page //重新从第 1 页开始
            }
        });
    },
    editIframe: function (layer, insTb) {
        var self = this;
        let widthstr = "80%";
        let heightstr = "80%";
        let isreload = self.isEditReload;
        //iframe窗
        layer.open({
            type: 2,
            title: self.editTitle,
            shadeClose: true,
            shade: false,
            maxmin: true, //开启最大化最小化按钮
            area: [widthstr, heightstr],
            content: routeLink + 'Edit',
            end: function () {
                if (isreload) {
                    defaultOptions.callBack(insTb);
                }
            }
        });
    },
    openOperaIframe: function (layer, insTb) {
        var self = this;
        let widthstr = self.operawidths + 'px';
        let heightstr = self.operaheights + 'px';
        let isreload = self.isOperaReload;
        //iframe窗
        layer.open({
            type: 2,
            title: self.operationTitle,
            shadeClose: true,
            shade: false,
            maxmin: true, //开启最大化最小化按钮
            area: [widthstr, heightstr],
            content: routeLink + 'OperationPermission',
            end: function () {
                if (isreload) {
                    defaultOptions.callBack(insTb);
                }
            }
        });
    },openDataPermissionIframe: function (layer, insTb) {
        var self = this;
        let widthstr = self.datawidths + 'px';
        let heightstr = '90%';
        //iframe窗
        layer.open({
            type: 2,
            title: '数据权限',
            shadeClose: true,
            shade: false,
            maxmin: true, //开启最大化最小化按钮
            area: [widthstr, heightstr],
            content: routeLink + 'DataPermission',
            end: function () {
            }
        });
    },
    //获取所有选中的节点id
    getCheckedId: function (data) {
        var self = this;
        var delete_index = [];
        var id = "";
        $.each(data, function (index, item) {
            if (item.ID != "") {
                delete_index.push(item[self.IdName])
            }
        });
        if (delete_index.length > 0) {
            for (var i = 0; i < delete_index.length; i++) {
                if (id != "") {
                    id += '-' + delete_index[i];
                }
                else {
                    id = delete_index[i];
                }
            }
        }
        return id;
    }

}