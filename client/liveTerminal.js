var connection = new signalR.HubConnectionBuilder()
    .withUrl("https://34.125.199.21:7046/terminal");

var outputWindow = document.getElementById("output");

document.getElementById("send-button").disabled = true;

connection.on("receiveStdOut", function(stdOut){
    outputWindow.append(
        `<p>machine: ${stdOut}</p>`
    );
});

document.getElementById("clear").onclick(function() {
    outputWindow.innerHTML = '';
});