using System.Net.Sockets;
using TL;
using WTelegram;

public class TelegramClient

{
    long ChatId;
    private Client client;
    private InputPeer peer;

    public TelegramClient()
    {
        int appId = Convert.ToInt32(Environment.GetEnvironmentVariable("appID"));
        string apiHash = Environment.GetEnvironmentVariable("apiHash");
        client = new Client(appId, apiHash,"WTelegram.session");
        ChatId = Convert.ToInt64(Environment.GetEnvironmentVariable("ChatID"));
        Login();
        //Disable Log
        Helpers.Log = delegate(int i, string s) { };
    }

    async void Login()
    {
        try
        {
            string? number = Environment.GetEnvironmentVariable("phoneNumber");
            await DoLogin(number); // initial call with user's phone_number
        }
        catch (SocketException e)
        {
            Console.WriteLine(e);
            await Task.Run(() => Thread.Sleep(5000));
            try
            {
                await client.ConnectAsync();
            }
            catch (SocketException e1)
            {
                Console.WriteLine(e1);
                Login();
            }
            
        }

        async Task DoLogin(string? loginInfo) // (add this method to your code)
        {
            while (client.User == null)
                switch (await client.Login(loginInfo)) // returns which config is needed to continue login
                {
                    case "verification_code":
                        Console.Write("Code: ");
                        loginInfo = Console.ReadLine();
                        break;
                    case "name":
                        loginInfo = "John Doe";
                        break; // if sign-up is required (first/last_name)
                    case "password":
                        Console.Write("2FA Code: ");
                        loginInfo = Console.ReadLine();
                        break; // if user has enabled 2FA
                    default:
                        loginInfo = null;
                        break;
                }

            Console.WriteLine($"We are logged-in as {client.User} (id {client.User.id})");
        }
    }

    public async Task<Message?> SendVideo(string caption, string media, Files file)
    {
        await Task.Run(() => Thread.Sleep(client.FloodRetryThreshold));
        if (peer is null)
        {
            var chats = await client.Messages_GetAllChats();
            peer = chats.chats[ChatId];
        }

        var inputFile = new InputMediaDocumentExternal
            { url = media };

        var entities = client.HtmlToEntities(ref caption);


        var inputFileWebm = await UploadFromUrl(inputFile.url);

        var foo = new
            InputMediaDocumentExternal { url = RestClient.Host + file.thumbnail };

        var photoInputFile =
            await UploadFromUrl(foo.url);
        await Task.Run(() => Thread.Sleep(1100));
        var message = await client.SendMessageAsync(peer, caption, new InputMediaUploadedDocument
        {
            thumb = photoInputFile,
            file = inputFileWebm, mime_type = "video/mp4", flags = InputMediaUploadedDocument.Flags.has_thumb,
            attributes = new[]
            {
                new DocumentAttributeVideo
                {
                    duration = file.duration_secs, w = file.width, h = file.height,
                    flags = DocumentAttributeVideo.Flags.supports_streaming
                }
            }
        }, entities: entities);

        return message;
    }

    HttpClient httpClient = null;

    private int retryCount = 0;

    async Task<InputFileBase> UploadFromUrl(string url)
    {
        try
        {
            var filename = Path.GetFileName(new Uri(url).LocalPath);
            httpClient ??= new();
            var response = await httpClient.GetAsync(url);
            await using var stream = await response.Content.ReadAsStreamAsync();
            if (response.Content.Headers.ContentLength is long length)
                return await client.UploadFileAsync(new Helpers.IndirectStream(stream) { ContentLength = length },
                    filename);
            else
            {
                using var ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                ms.Position = 0;
                return await client.UploadFileAsync(ms, filename);
            }
        }
        catch (HttpRequestException e)
        {
            //TODO retry 5
            while (retryCount < 5)
            {
                await Task.Run(() => Thread.Sleep(5000));
                await UploadFromUrl(url);
                retryCount++;
            }

            Console.WriteLine(e);
        }

        return null;
    }
}