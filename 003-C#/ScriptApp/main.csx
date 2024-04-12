#r "sdk:Microsoft.NET.Sdk.Web"

using Microsoft.AspNetCore.Builder;

var app = WebApplication.Create();
app.MapGet("/", () => "Hello world");
app.Run();

Console.WriteLine("Hello World!");  
