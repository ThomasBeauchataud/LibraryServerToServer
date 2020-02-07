# LibraryServerToServer
### Description du fonctionnement coté client

Le client créé une commande qui est l’objet à traiter. 
```
Command command = new Command(commandParameters)
```
Il va ensuite créer un Flux qui est le transporteur de l’objet. 
```
Flux flux = new Flux();
```
Avant d’écrire dans le flux, nous le décorons d’une encryptions et d’une compression. 
```
CompressionDecorator compressedFlux = new CompressionDecorator(flux);
EncryptionDecorator encryptedCompressedFlux = new EncryptionDecorator(compressedFlux);
```
Une fois le flux bien créé, nous écrivons l’objet à transporter (dans notre cas la commande sérialisée).
```
encryptedCompressedFlux.WriteData(Command.Serialize(command));
```
Enfin, invoquons notre ISender pour envoyer notre flux vers un autre system (Dans notre cas, une communication par socket vers localhost:8888). Le flux reçut est du même objet que le flux envoyé.
```
List<object> parameters = new List<object>();
parameters.Add("localhost");
parameters.Add(8888);
flux = new WebSocketSender().Send(flux, parameters);
```
Pour finir nous récupérons les données en lisant le flux
```
Console.WriteLine(Encoding.Default.GetString(flux.ReadData()));
```
### Description du fonctionnement coté server

Le server créé une instance de SocketListener qui va écouter les communications par socket entrante (dans notre cas sur le port 8888). On associe a notre SocketListener un IExecutor ainsi qu’un IAdapter associé à l’IExecutor injecté).
> On peut utiliser la CDI pour injecter les IAdapter et les IExecutor
```
IExecutor executor = new CommandExecutor();
IAdapter adapter = new CommandAdapter();
WebSocketListener webSocketListener = new WebSocketListener(executor, adapter);
webSocketListener.StartServer(8888);
```
Dans le cas de notre WebSocketListener, le server va créer un nouveau Thread pour gérer chaque communication avec un client
```
TcpClient clientSocket = serverSocket.AcceptTcpClient();
ClientHandler clientHandler = new ClientHandler(clientSocket, executor, adapter);
clientHandler.Handle();
```
Apres avoir reconstitué le flux, nous allons adapté l’objet contenu dans le flux avec l’IAdaptor de notre Listener avant d’appeler l’IExecutor pour l’objet tout juste généré. L’objet Flux étant le même que celui envoyé par le Client, il effectuera naturellement les décompressions et décryptions nécessaires
```
Flux flux = Flux.DeSerialize(bytesFrom);
object result = executor.Execute(adapter.GenerateInstance(flux.ReadData()));
```
Une fois la réponse obtenue, nous la réécrivons dans le même flux, les encryptions et compressions seront donc les mêmes que celles envoyées par le client
```
flux.WriteData(ObjectToBytes(result));
byte[] sendBytes = Flux.Serialize(flux);
networkStream.Write(sendBytes, 0, sendBytes.Length);
networkStream.Flush();
clientSocket.Close();
```
