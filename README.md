# FSharp.Json

[Giraffe](https://github.com/giraffe-fsharp) `IJsonSerializer` implementation for [FSharp.Json](https://github.com/vsapronov/FSharp.Json)

[![NuGet Info](https://buildstats.info/nuget/FSharp.Json.GiraffeSerializer?includePreReleases=true)](https://www.nuget.org/packages/FSharp.Json.GiraffeSerializer)

## Usage
```fsharp
open FSharp.Json.GiraffeSerializer (* Open the module *)
open Giraffe
open Giraffe.Serialization.Json (* Open the module *)
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting

type U = Killua of string

type T = { Killua: U option }

let router =
    choose [ route "/"
             >=> (fun next context -> json { Killua = Some <| U.Killua "Zoldyck" } next context) ]

type Startup() =
    member __.ConfigureServices(services: IServiceCollection) =
        services.AddSingleton<IJsonSerializer>(FSharpJsonSerializer())
        |> ignore (* Register the IJsonSerializer class and all done! *)

        services.AddGiraffe() |> ignore

    member __.Configure(builder: IApplicationBuilder) = builder.UseGiraffe router

[<EntryPoint>]
let main args =
    Host
        .CreateDefaultBuilder()
        .ConfigureWebHostDefaults(fun builder -> builder.UseStartup<Startup>() |> ignore)
        .Build()
        .Run()

    0
```

## License

This project is distributed under the [Apache License 2.0](LICENSE)
