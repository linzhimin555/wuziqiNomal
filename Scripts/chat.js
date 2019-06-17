"use strict";

var chat = $.connection.chatHub;

var connectionid ="";
$.connection.hub.start().done(function () {
    connectionid = chat.connection.id;
    $("#quite").on("click", function () {
        if (room !== "") {
            chat.server.removeRoom(room);
            location.href = "index";
            console.log(data);
            event.preventDefault();
        }
    });

    $("#ready").on("click", function () {
        chat.server.beReady(room).then(function (data) {
            if (data.Item1) {
                $("#ready").val("已准备");
                if (data.msg === "one") {
                    console.log("自己准备");
                }
                else if (data.Item2 === "two") {
                    start = true;
                }
            }
            else {
                console.log(1);
                alert('准备失败');
            }
        });
    });
    
});

chat.client.othersChange = function (user) {
    console.log(user);
    $("#" + user.id).remove();
    var html = '<div id="' + user.id + '">    Play Two: Name：<em id="name1">' + user.id + '</em>    Color: <em id="color1">' + user.color + '</em>    Ready: <em id="ready1">' + user.ready + '</em>        </div >';
    $("#player").append(html);
};

chat.client.start = function (isStart) {
    console.log(isStart);
    start = true;
};
chat.client.send = function(asd,user, message) {
    var data = JSON.parse(message);
    opponentDraw(data.pp, data.color);
};

function downChess(pp) {
    var data = {
        color: colors,
        pp: pp
    };
    chat.invoke("SendMessage",connectionid, JSON.stringify(data), room);
}

