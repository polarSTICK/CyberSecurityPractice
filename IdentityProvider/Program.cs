using IdentityProvider;

HttpServer server = new HttpServer("127.0.0.1", 1043);
server.Start();
var db = new Database();

var LoginMatch = db.UserExists("user", "password");

Console.ReadLine();