var ws = new WebSocket("ws://192.168.31.64:8080");
//申请一个WebSocket对象，参数是服务端地址，同http协议使用http://开头一样，WebSocket协议的url使用ws://开头，另外安全的WebSocket协议使用wss://开头
ws.onopen = function () {
    //当WebSocket创建成功时，触发onopen事件
    console.log("open");
}
ws.onclose = function (e) {
    //当客户端收到服务端发送的关闭连接请求时，触发onclose事件
    console.log("close");
}
ws.onerror = function (e) {
    //如果出现连接、处理、接收、发送数据失败的时候触发onerror事件
    console.log(error);
}
