open System
open Microsoft.AspNet.SignalR
open Microsoft.AspNet.SignalR.Hubs
open Microsoft.Owin.Hosting
open Microsoft.AspNet.SignalR.Owin
open EkonBenefits.FSharp.Dynamic

type public Startup() = 
    member public this.Configuration(app) = 
        let config = new HubConfiguration()
        config.EnableDetailedErrors <- true
        //Owin.OwinExtensions.MapSignalR(app);
        Owin.MapExtensions.Map(app, "/signalr", 
                               fun map -> 
                                   Owin.CorsExtensions.UseCors(map, Microsoft.Owin.Cors.CorsOptions.AllowAll) |> ignore
                                   Owin.OwinExtensions.RunSignalR(map, config))
        |> ignore

[<HubName("myHub")>]
type public MyHub() = 
    inherit Hub()
    member public x.Chatter(param) = 
        base.Clients.All?addMessage (param)
        |> ignore

[<EntryPoint>]
let main argv = 
    let url = "http://localhost:8080/"
    use webApp = WebApp.Start<Startup>(url)
    Console.ForegroundColor = ConsoleColor.Green |> ignore
    Console.WriteLine("Server running on {0}", url)
    Console.WriteLine("Press any key to start sending events to connected clients")
    Console.ReadLine() |> ignore
    let context : IHubContext = GlobalHost.ConnectionManager.GetHubContext<MyHub>()
    for x in [ 0..1000 ] do
        System.Threading.Thread.Sleep(1000)
        Console.WriteLine("Server Sending Value to Client X: " + x.ToString())
        context.Clients.All?addMessage (x.ToString())
    Console.ReadLine() |> ignore
    0
