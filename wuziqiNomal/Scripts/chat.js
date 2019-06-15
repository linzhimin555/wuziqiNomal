"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
var connectionid = "";

connection.on("ReceiveMessage", function (user, message) {
    var data = JSON.parse(message);
    opponentDraw(data.pp, data.color);
});

connection.on("Start", function (isStart) {
    console.log(isStart);
    start = true;
});

connection.on("OthersChange", function (user) {
    console.log(user);
    $("#" + user.id).remove();
    var html = '<div id="' + user.id + '">    Play Two: Name：<em id="name1">' + user.id + '</em>    Color: <em id="color1">' + user.color + '</em>    Ready: <em id="ready1">' + user.ready + '</em>        </div >';
    $("#player").append(html);
});

$("#goIn").on("click", function () {
    if ($("#room").val() === room) {
        return;
    }
    if (room !== "") {
        connection.invoke("RemoveRoom", room).catch(function (err) {
            return console.error(err.toString());
        }).then(function (data) { console.log(data); });
        event.preventDefault();
    }
    room = $("#room").val();
    connection.invoke("JoinRoom", room).catch(function (err) {
        return console.error(err.toString());
    }).then(function (data) {
        console.log(data);
        $("#local").html(room);
        if (data === true) {
            flag = true;
            colors = 1;
        }
        else {
            flag = false;
            colors = -1;
        }
    });
});

$("#quite").on("click", function () {
    if (room !== "") {
        connection.invoke("RemoveRoom", room).catch(function (err) {
            return console.error(err.toString());
        }).then(function (data) {
            location.href = "index";
            console.log(data);
        });
        event.preventDefault();
    }
});

$("#ready").on("click", function () {
    connection.invoke("BeReady", room).catch(function (err) {
        return console.error(err.toString());
    }).then(function (data) {
        if (data.item1) {
            $("#ready").val("已准备");
            if (data.msg === "one") {
                console.log("自己准备");
            }
            else if(data.item2 === "two") {
                start = true;
            }
        }
        else {
            console.log(1);
            alert('准备失败');
        }
    });
});

function downChess(pp) {
    var data = {
        color: colors,
        pp: pp
    };
    connection.invoke("SendMessage", connectionid, JSON.stringify(data), room).catch(function (err) {
        return console.error(err.toString());
    });
}

connection.start(room).catch(function (err) {
    return console.error(err.toString());
});