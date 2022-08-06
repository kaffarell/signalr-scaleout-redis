import { HttpTransportType, HubConnectionBuilder } from '@microsoft/signalr';

let connection = new HubConnectionBuilder()
    .withUrl("http://localhost:80/statusHub", {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets
    })
    .build();

let displayName = '';

document.getElementById('button').addEventListener('click', () => {
    displayName = document.getElementById('input').value;
    console.log('Name: ' + displayName);

    connection.start()
        .then(() => connection.invoke("SendPing", displayName));
});


connection.on("online", (value) => {
    console.log('Received Message');
    let list = document.getElementById('list');
    let element = document.createElement('li');
    element.innerText = value;
    console.log(value)
    list.appendChild(element);
});

connection.on("offline", (value) => {
    console.log('Received Message');
    let list = document.getElementById('list');
    for(let i = 0; i < list.children.length; i++) {
        console.log(list.children[i].innerText);
        if(list.children[i].innerText == value) {
            console.log('removing child');
            list.children[i].remove();
        }
    }
});

window.addEventListener('beforeunload', function (e) {
    console.log('Invoking RemoveDisplay');
    connection.invoke('RemoveDisplay', displayName);
});


