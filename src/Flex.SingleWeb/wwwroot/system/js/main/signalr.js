const connection = new signalR.HubConnectionBuilder()
    .withUrl('/exportHub', {
        accessTokenFactory: () => sessionStorage.getItem('access_token'),
        transport: signalR.HttpTransportType.WebSockets, // 强制使用 WebSockets
        skipNegotiation: true // 跳过协商过程
    })
    .withAutomaticReconnect([0, 2000, 10000, 30000]) //配置重连策略，按次数递增，此处单位为毫秒
    .build();
connection.on("ReceiveProgress", (progress) => {
    GetRunningTaskCount();
    //tips.showSuccess(progress);
    // 更新进度条或其他UI元素
});
connection.on("ExportCompleted", (message) => {
    tips.showSuccess(message);
    // 检查当前的连接状态
    if (connection.state === signalR.HubConnectionState.Connected ||
        connection.state === signalR.HubConnectionState.Connecting) {
        GetMsgCount();
        tips.closeProgressbox();
        GetRunningTaskCount();
        getTaskList();
    }
    // 处理导出完成的逻辑
});

connection.on("SendProgress", (message) => {
    tips.showProgress("Remaining", message);

    // 检查当前的连接状态
    if (connection.state === signalR.HubConnectionState.Connected ||
        connection.state === signalR.HubConnectionState.Connecting) {
        getTaskList();
    }
});
connection.on("ExportError", (message) => {
    tips.showFail(message);
    tips.closeProgressbox();
    // 检查当前的连接状态
    if (connection.state === signalR.HubConnectionState.Connected ||
        connection.state === signalR.HubConnectionState.Connecting) {
        getTaskList();
    }
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

function reconnectSignalR() {
    // 检查当前的连接状态
    if (connection.state === signalR.HubConnectionState.Connected ||
        connection.state === signalR.HubConnectionState.Connecting) {
        // 如果已经连接或正在连接，则先停止连接
        connection.stop()
            .then(() => {
                console.log("SignalR连接已停止，准备重新连接...");
                return startSignalRConnection(); // 在停止后再启动连接
            })
            .catch(err => console.error("SignalR停止连接失败: ", err));
    } else {
        // 如果连接已关闭或尚未建立，则直接启动连接
        startSignalRConnection();
    }
}

function startSignalRConnection() {
    connection.start()
        .then(() => {
            console.log("重新连接SignalR成功");
            conn_signalr_id = connection.connectionId;
        })
        .catch(err => console.error("SignalR重新连接失败: ", err));
}

// 在适当的地方调用 refreshToken 来刷新 token 并重新连接 SignalR

