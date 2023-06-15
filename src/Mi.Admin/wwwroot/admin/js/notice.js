"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/noticeHub").build();

connection.on("ReceiveMessage", function (title, content) {
    layer.msg("【消息】" + title + "：" + content)
});

connection.start().then(() => {
    let intervalTime = 1000 * 1;
    setInterval(function () {
        connection.invoke("SendMessage", "", "", false).catch(function (err) {
            return console.error(err.toString());
        });
    }, intervalTime)
}).catch(function (err) {
    return console.error(err.toString());
});