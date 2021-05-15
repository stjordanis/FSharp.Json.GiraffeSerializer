namespace FSharp.Json.GiraffeSerializer

open FSharp.Control.Tasks
open FSharp.Json
open Giraffe.Serialization.Json
open System.IO
open System.Text

type FSharpJsonSerializer() =
    interface IJsonSerializer with
        member self.Deserialize<'T>(encoding: byte []) : 'T =
            let serializer : IJsonSerializer = upcast self

            Encoding.UTF8.GetString encoding
            |> serializer.Deserialize<'T>

        member _.Deserialize<'T>(payload: string) : 'T = Json.deserialize<'T> payload

        member self.DeserializeAsync stream =
            task {
                use reader = new StreamReader(stream, Encoding.UTF8)

                let serializer : IJsonSerializer = upcast self

                return
                    reader.ReadToEndAsync()
                    |> Async.AwaitTask
                    |> Async.RunSynchronously
                    |> serializer.Deserialize<'T>
            }

        member self.SerializeToBytes serialize =
            let serializer : IJsonSerializer = upcast self

            serialize
            |> serializer.SerializeToString
            |> Encoding.UTF8.GetBytes

        member self.SerializeToStreamAsync serialize stream =
            let T =
                task {
                    use writer = new StreamWriter(stream, Encoding.UTF8)

                    let serializer : IJsonSerializer = upcast self

                    do!
                        serialize
                        |> serializer.SerializeToString
                        |> writer.WriteAsync
                }
                
            upcast T

        member _.SerializeToString(serialize) = Json.serialize serialize
