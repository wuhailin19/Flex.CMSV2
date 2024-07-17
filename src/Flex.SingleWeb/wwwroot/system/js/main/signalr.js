const connection = new signalR.HubConnectionBuilder()
    .withUrl('/exportHub', {
        accessTokenFactory: () => sessionStorage.getItem('access_token'),
        transport: signalR.HttpTransportType.WebSockets, // 强制使用 WebSockets
        skipNegotiation: true // 跳过协商过程
    })
    .configureLogging(signalR.LogLevel.Information)
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
connection.start()
    .then(() => {
        console.log("SignalR连接成功");
        conn_signalr_id = connection.connectionId;

        // 发送自定义标识符到服务器
        const userId = sessionStorage.getItem("UserId"); // 自定义用户 ID，替换为实际用户 ID
        connection.invoke("RegisterUser", userId)
            .then(() => console.log("用户注册到socket成功"))
            .catch(err => console.error("用户注册到socket失败: ", err));
    })
    .catch(err => console.error("SignalR连接失败: ", err));
var conn_signalr_id = undefined;