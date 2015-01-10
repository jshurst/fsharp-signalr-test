open Microsoft.AspNet.SignalR
open Microsoft.AspNet.SignalR.Client
open Microsoft.AspNet.SignalR.Client.Hubs
open System
open System.Threading.Tasks

[<EntryPoint>]
let main argv = 
    let connection = new HubConnection("http://localhost:8080")
    let myHub = connection.CreateHubProxy "myHub"
    connection.Start().Wait()
    Console.ForegroundColor = ConsoleColor.Yellow |> ignore

    myHub.Invoke<string>("chatter", "Hi!! Server").ContinueWith(fun (task : Task) -> 
        if task.IsFaulted then 
            Console.WriteLine("Could not Invoke the server method Chatter: {0}", task.Exception.GetBaseException())
        else 
            Console.WriteLine("Success calling chatter method"))
    |> ignore

    myHub.On<string>
        ("addMessage", fun param -> Console.WriteLine("Client receiving value from server: {0}", param.ToString())) 
    |> ignore
    
    Console.ReadLine() |> ignore
    0
