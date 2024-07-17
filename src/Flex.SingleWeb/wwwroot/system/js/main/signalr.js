const connection = new signalR.HubConnectionBuilder()
    .withUrl('/exportHub', {
        accessTokenFactory: () => sessionStorage.getItem('access_token'),
        transport: signalR.HttpTransportType.WebSockets, // 强制使用 WebSockets
        skipNegotiation: true // 跳过协商过程
    })
    .withAutomaticReconnect([0, 2000, 10000, 30000]) //配置重连策略，按次数递增，此处单位为毫秒
    .build();
connection.on("ReceiveProgress", (progress) => {
    //tips.showSuccess(progress);
    // 更新进度条或其他UI元素
});
connection.on("ExportCompleted", (message) => {
    tips.showSuccess(message);
    GetMsgCount();
    // 处理导出完成的逻辑
});
connection.onreconnecting((error) => {
    console.warn("正在重新连接: ", error);
});

connection.onreconnected((connectionId) => {
    console.log("重新连接成功, 连接ID: ", connectionId);
});
connection.onclose((error) => {
    console.log("SignalR连接关闭: ", error);
    // 可以在这里处理连接关闭后的逻辑
});
connection.start()
    .then(() => {
        console.log("尝试链接SignalR");
        console.log(connection.state)
        if (connection.state === signalR.HubConnectionState.Connected) {
            console.log("已成功连接");
        } else {
            console.log("当前连接状态: ", connection.state);
        }
        conn_signalr_id = connection.connectionId;
    })
    .catch(err => console.error("SignalR连接失败: ", err));
var conn_signalr_id = undefined;