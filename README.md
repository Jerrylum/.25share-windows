<p align="center">
	<a href="https://github.com/jerrylum/.25share"><img src="https://i.imgur.com/SZTjDWl.png" alt="IntroIcon" width="100"></a>
</p>
<h3 align="center">QuarterShare</h3>
<p align="center">
Quarter Share is made to enable easy message communication between mobile phones and computers. <br><br>
This project is divided into three parts, Windows server (this repository) , Linux server
(<a href="https://github.com/jerrylum/.25share">link</a>) and Android application
(<a href="https://github.com/jerrylum/.25share-android">link</a>). Pay a visit to our wiki page for more information about setup and usage.
</p>

<h4 align="center"><a href="https://github.com/jerrylum/.25share-windows/wiki">Setup Now</a></h4>

---

### Have a quick look

Click the connect button. Then type something on your phone and send it.

<h5 align="left">
<img src="https://imgur.com/ZqoJJke.gif">
</h5>
<br>

### Is it dangerous?

The connection between the phone and the computer is secured with AES 256bit Encryption. By default, each new connection needs to be manually confirmed on the server to allow the client to send messages. Also, you can kick any clients you want.

<h5 align="left">
<img src="https://imgur.com/YYMzynK.gif">
</h5>
<br>

<br>


### Other Features

- Computers and mobile phones can send messages in both directions
- Support multiple lines
- Quick connection speed
- Offline compatibility
- Command-line support
- Internal commands can be used within the server 
- No installation is necessary



---

### Wait! I am using Linux!

Use .25Share Linux server [.25Share](https://github.com/Jerrylum/.25share). 

### Wait! I am using iPhone!

You can check out [One Share](https://github.com/Jerrylum/OneShare). 


---


### Command Line

```

usage: .25share [--help] [--host:HOST] [--port:PORT] [--allow] [--flag:FLAG]  

optional arguments:
  --help         Show this help message and exit
  --host HOST    The server's hostname or IP address
  --port PORT    The port to listen on
  --allow        Allow all clients to send messages to the server
                 without the user's permission
  --flag FLAG    Mode flag

```


### Internal Command

```
.help                            show this help message
.flag                            show how the server handles messages
.chflag [flag]                   change how the server handles messages
.ls                              list all connected clients
.allow <client>                  allow client(s) to send messages
.kick <client>                   kick specified client(s)
.send <client> <content>         send a message to client(s)
.stop                            stop the server
```

#### Client Selector
```
@a      all clients
@p      the latest client who sent a message / connected
<ID>    specified client id, e.g. `5`
```

For example, using `.kick @a` command will kick all connected clients, using `.kick 30` command will kick a connected client with id **30**.


#### Note
1. Commands must be preceded by a period.
2. Any input that does not start with a period is understood as sending
the entire sentence to the latest client (@p).
3. If you want to send a message that starts with a period, use command 
`.send @p YOUR MESSAGE`

### Mode Flag

When you press the "Send" button on your phone to send a message, what should the server do after receiving it

```
p       Print on the console
c       Copy to clipboard
t       Typing text
```

You can use multiple flags at the same time. e.g. `t`, `c` and `pct` are acceptable.  

It should look like this:
```
.25share --flag:tcp        # command line
.chflag tcp                # internal command
```


---

### Setup

Please go to [the wiki page](https://github.com/jerrylum/.25share-windows/wiki).  

<br>

### Special Thanks

Thanks you [SamNg](https://github.com/ngkachunhlp) and [COMMANDER.WONG](https://github.com/COMMANDERWONG) for their suggestions and software testing.
