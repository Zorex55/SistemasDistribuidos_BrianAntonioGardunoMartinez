using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FileDownload;

public class Peer{
    private readonly TcpListener _listener;
    private TcpClient? _client;
    private const int Port = 8080;

    public Peer(){
        _listener = new TcpListener(IPAddress.Any, Port);
    }

    public async Task DownloadFile(string peerIP, int peerPort, string fileName, string savePath, CancellationToken cancellationToken){ 
        //async hace concurrencia, Task es propio de .net; CancellationToken da un tiempo de gracia para detener los aplicativos, en lugar de terminar abruptamente
        _client = new TcpClient(peerIP, peerPort);
        await using var stream = _client.GetStream(); //using reemplaza el try-catch en .net, hace un dipose de los datos si se genera un error
        var request = Encoding.UTF8.GetBytes(fileName); //Transforma una cadena de texto a bytes, toda la conmunicación se hace así
        await stream.WriteAsync(request, cancellationToken);

        await using var fs = new FileStream(savePath, FileMode.Create, FileAccess.Write);
        var buffer = new byte[1024];
        int bytesRead;
        while((bytesRead = await stream.ReadAsync(buffer, cancellationToken)) > 0){
            await fs.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken);
        }
        Console.WriteLine($"El archivo {fileName} se ha descargado en la ruta {savePath}");
    }

    public async Task Start(CancellationToken cancellationToken ){
        _listener.Start();
        while(true){
            _client = await _listener.AcceptTcpClientAsync(cancellationToken);
            await HandleClient(cancellationToken);
        }
    }

    private async Task HandleClient(CancellationToken cancellationToken){
        await using var stream = _client.GetStream();
        var buffer = new byte[1024];
        var bytesRead = await stream.ReadAsync(buffer, cancellationToken); //Espera el nombre del archivo
        var fileName = Encoding.UTF8.GetString(buffer, 0, bytesRead); //Cambia los bytes a un string legible con el nombre
        if(File.Exists(fileName)){
            var data = await File.ReadAllBytesAsync(fileName, cancellationToken); //El archivo en bytes
            await stream.WriteAsync(data, cancellationToken);
            Console.WriteLine($"File {fileName} sent to client");
        }else{
            var ErrorMessage = Encoding.UTF8.GetBytes("File not found :c");
            await stream.WriteAsync(ErrorMessage, cancellationToken); //Mandar el mensaje de error al cliente
            Console.WriteLine($"File not found: {fileName}");

        }

    }
}
