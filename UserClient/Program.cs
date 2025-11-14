using System.Net.Http.Headers;
using Shared;

var loginNotDone = true;

while (loginNotDone)
{
    Console.WriteLine("Input your userId");
    var login = Console.ReadLine();
    Console.WriteLine("Input your password");
    var password = Console.ReadLine();
    
    var hashedPw = Cryptography.GetHashString(password);
    
    // make web call to the identity provider
    /*using HttpClient client = new();
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
    client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

    await ProcessRepositoriesAsync(client);

    static async Task ProcessRepositoriesAsync(HttpClient client)
    {
    }*/
    
}



