var defaultOptions = {
    addwidths: 400,
    addheights: 400,
    editwidths: 400,
    editheights: 400,
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
        let widthstr = '95%';
        let heightstr = '95%';
        //iframe窗
        layer.open({
            type: 2,
            title: self.addTitle,
            shadeClose: true,
            shade: false,
            maxmin: true, //开启最大化最小化按钮
            area: [widthstr, heightstr],
            content: routePageLink + 'AddPage',
            end: function () {
                defaultOptions.callBack(insTb);
            }
        });
    },
    callBack: function (tableIns) {
        //第二次调用
        tableIns.reload({
            where: { 'ParentId': currentparentId } // 设定异步数据接口的额外参数，任意设
        });
        //console.log(tableIns)
    },
    editIframe: function (layer, insTb, pId) {
        var self = this;
        let widthstr = '95%';
        let heightstr = '95%';
        let isreload = self.isEditReload;
        //iframe窗
        layer.open({
            type: 2,
            title: self.editTitle,
            shadeClose: true,
            shade: false,
            maxmin: true, //开启最大化最小化按钮
            area: [widthstr, heightstr],
            content: routePageLink + 'Edit',
            end: function () {
                if (isreload) {
                    defaultOptions.callBack(insTb);
                }
            }
        });
    }, filedsIframe: function (layer, insTb) {
        var self = this;
        let widthstr = '80%';
        let heightstr = '90%';
        let isreload = self.isEditReload;
        //iframe窗
        layer.open({
            type: 2,
            title: '字段列表',
            shadeClose: true,
            shade: false,
            maxmin: true, //开启最大化最小化按钮
            area: [widthstr, heightstr],
            content: '/system/Field/Index',
            end: function () {
                if (isreload) {
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
            content: routePageLink + 'OperationPermission',
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
            content: routePageLink + 'DataPermission',
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