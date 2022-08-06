# POC SignalR scaleout with redis and haproxy
Scaling out SignalR to multiple instances using Redis.

## Quickstart
```
$ docker compose up
```
Then you can access `localhost:80/api` to test if your instances are up. The returned value is the ip address of the instance, thus you should get another ip address everytime you refresh the page.  
  
To run the client, simply open the Client/ Project and execute:  
```
$ npm run build
```
Then you can open the index.html file in the dist/ folder.  
In the browser you can enter a name into the input field, then open another tab with the same url and insert another name. Then you should see the name you inserted in the second tab appear in the first one. (If you look at the output of docker compose you can also see that the two tabs are connected with two different instances)  


## How it works
SignalR stores all the connections and their messages in a redis server. All the websocket connections also have a cookie provided by the load-balancer. For example in HaProxy:  
```
cookie SERVERID insert indirect nocache
```
On the first connect, the client gets this cookies and uses it the whole time while the websocket is open. That means the client will always talk with the same instance. If that instance fails, it will automatically switch to the other instance.

## Resources
* https://docs.microsoft.com/en-us/aspnet/signalr/overview/performance/scaleout-with-redis   
* https://www.haproxy.com/blog/load-balancing-affinity-persistence-sticky-sessions-what-you-need-to-know/
