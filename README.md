# POC SignalR scaleout with redis and haproxy
Scaling out SignalR to multiple instances using Redis.

## Quickstart
```
$ docker compose up
```
Then you can access `localhost:80/api` to test if your instances are up. The returned value is the ip address of the instance, thus you should get another ip address everytime you refresh the page.  
  
  
The haproxy status page is available at `localhost:8404`.
  
To run the client, simply open the Client/ Project and execute:  
```
$ npm run build
```
Then you can open the index.html file in the dist/ folder.  
In the browser you can enter a name into the input field, then open another tab with the same url and insert another name. Then you should see the name you inserted in the second tab appear in the first one. (If you look at the output of docker compose you can also see that the two tabs are connected with two different instances)  


## How it works
SignalR stores all the connections and their messages in a redis server. By storing them, every instance can send a message to all clients, while that instance is not even connected with every client. Although they "share" the connections through redis, every client still has to keep the connection open with the same server.   
That means clients "stick" to their server until the connection is closed. If an instance goes down, it automatically switches to the other instance and the connection is still working(because all the info is stored in redis).  
  
 
The easiest solution to the "sticking client to server" part is to let haproxy do the work:    
Haproxy can keep track of the client's IP Address, or other information on the Application layer, to route the connection always to the same server (While the connection is open). ([More info here](https://www.haproxy.com/blog/load-balancing-affinity-persistence-sticky-sessions-what-you-need-to-know/))  
The only problem is that this only works with WebSocket connections. But SignalR falls back to long polling, if WebSockets are not available. That means we have to explicitly allow only websockets to be used (see the transport option):  
```js
let connection = new HubConnectionBuilder()
    .withUrl("http://localhost:80/statusHub", {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets
    })
    .build();
```
  
But there is also a workaround for this. We can use session cookies to stick a client to a server and thus also allow long polling to be used.
Using this, when a websocket connection gets instantiated, the clients gets a cookie from the server, and uses that cookie for every request while the connection is open. Haproxy can intercept that cookie and route the requests to the correct server. 
  
  
Haproxy config:  
```
cookie SERVERID insert indirect nocache
```
On the first connect, the client gets this cookies and uses it the whole time while the websocket is open. That means the client will always talk with the same instance. If that instance fails, it will automatically switch to the other instance.

## Resources
* https://docs.microsoft.com/en-us/aspnet/signalr/overview/performance/scaleout-with-redis   
* https://www.haproxy.com/blog/load-balancing-affinity-persistence-sticky-sessions-what-you-need-to-know/
