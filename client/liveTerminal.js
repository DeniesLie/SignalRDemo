var connection = new signalR.HubConnectionBuilder()
    .withUrl("https://webapp-220326221938.azurewebsites.net/terminal");

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