const HEADER_USER = 'lienko_device';
const HEADER_NONE = '';
const HEADER_ARROW = '-->';

const connectionUrl = "http://localhost:5000/terminal"


var logSection = document.getElementById('log');
var input = document.getElementById('input');
var sendBtn = document.getElementById('send');

// connect to signalR server
var hubConnection = new signalR.HubConnectionBuilder().withUrl(connectionUrl).build();
hubConnection.start().then(function () {
    
});

sendBtn.onclick = function () {
    var stdin = input.value;
    if (stdin == 'clear') clearLog();
    writeToLog(HEADER_USER, stdin);
    // send via signalR
    // receive stdout
    // writeToLog(HEADER_ARROW, stdout);
    input.value = '';
}

// close connection on window close
window.onbeforeunload = function (){
    // close connection with server;
}

function writeToLog(header, data) {
    logSection.innerHTML += `<p>${header} ${data}</p>`;
}
function clearLog(){
    logSection.innerHTML = '';
}