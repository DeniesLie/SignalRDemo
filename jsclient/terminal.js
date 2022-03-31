const HEADER_USER = 'lienko_device';
const HEADER_NONE = '';
const HEADER_ARROW = '-->';

const connectionUrl = "https://localhost:5000/terminal"

var logSection = document.getElementById('log');
var input = document.getElementById('input');
var sendBtn = document.getElementById('send');
var stateLabel = document.getElementById('conn-state');

var _connId = null;

// Connect to signalR server
updateState(); // set intial state(disconnected)
var hubConnection = new signalR.HubConnectionBuilder().withUrl(connectionUrl).build();
hubConnection.start().then(function () {
    updateState();  // trying to connect
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
    updateState();
    hubConnection.stop();   // close connection with server;
}

// events
sendBtn.onclick = function () {
    var stdin = input.value;
    sendStdIn(stdin);
};

hubConnection.on("receiveConnId", function(connId) {
    _connId = connId;
});

hubConnection.on('receiveStdOut', function (stdOut) {
    writeToLog(HEADER_ARROW, stdOut);
});

// helper functions
function sendStdIn(stdIn) {
    if (stdIn) {
        var stdinRequest = buildJSONStdinRequest(stdIn);
        hubConnection.invoke("SendStdInAsync", stdinRequest);
    }
}

function buildJSONStdinRequest(stdIn) {
    return JSON.stringify({
        'FromUser': _connId,
        'ToDevices': ['*'],
        'StdIn': stdIn
    });
}

function writeToLog(header, data) {
    logSection.innerHTML += `<p>${header} ${data}</p>`;
}
function clearLog(){
    logSection.innerHTML = '';
}

function updateState() {
    function disable() {
        input.disabled = true;
        sendBtn.disabled = true;
    }

    function enable() {
        input.disabled = false;
        sendBtn.disabled = false;
    }

    if (!hubConnection)
        disable();
    else {
        switch (hubConnection.state) {
            case "Disconnected":
                stateLabel.innerHTML = '🔴 Disconnected';
                disable();
                break;
            case "Connecting":
                stateLabel.innerHTML = '🟠 Connecting...';
                disable();
                break;
            case "Connected":
                stateLabel.innerHTML = '🟢 Connected';
                enable();
                break;
            default:
                stateLabel.innerHTML = '🤔❓ Unknown connection state';
                disable();
                break;
        }
    }
}