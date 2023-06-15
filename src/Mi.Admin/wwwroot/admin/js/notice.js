"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/noticeHub").build();

connection.on("ReceiveMessage", function (title, content) {
    if (title != null && title != undefined && title != "") {
        //��Ϣ����
        layer.msg("����Ϣ��" + title + "��" + content)
    }
});

connection.start().then(() => {
    let intervalTime = 1000 * 30;
    setInterval(function () {
        connection.invoke("SendMessage").catch(function (err) {
            return console.error(err.toString());
        });
    }, intervalTime)
}).catch(function (err) {
    return console.error(err.toString());
});