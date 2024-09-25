namespace FileDownload;

public class Program(){
    public static async Task Main(string[] args){
        var cancellationToken = new CancellationTokenSource();
        var peer = new Peer();
        var task = peer.Start(cancellationToken.Token);

        //dotnet run download ip port filePath savePath 
        if(args.Length > 0 && args[0] == "download"){
            await peer.DownloadFile(args[1], Convert.ToInt32(args[2]), args[3], args[4], cancellationToken.Token);
        }else{
            Console.WriteLine("Waiting for other peers to connect...");
        } 
        await task;
    }
}

using Microsoft.EntityFrameworkCore;
using SoapApi.Contracts;
using SoapApi.Infraestructure;
using SoapApi.Repositories;
using SoapApi.Services;
using SoapCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSoapCore();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserContract, UserService>();

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookContract, BookService>();

builder.Services.AddDbContext<RelationalDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();
app.UseSoapEndpoint<IUserContract>("/UserService.svc", new SoapEncoderOptions());
app.UseSoapEndpoint<IBookContract>("/BookService.svc", new SoapEncoderOptions());

app.Run()
