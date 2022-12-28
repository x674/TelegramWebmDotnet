using telegramWebm.Database;

namespace telegramWebm
{
    internal class Program
    {
        private static MediaRepository _mediaRepository = new MediaRepository();

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //await _mediaRepository.Database.EnsureDeletedAsync();
            await _mediaRepository.Database.EnsureCreatedAsync();
            bool run = true;
            TelegramClient telegramClient = new TelegramClient();
            while (run)
            {
                var dvachCatalog = await RestClient.GetCatalog("b");
                if (dvachCatalog is not null)
                {
                    foreach (var thread in dvachCatalog.threads)
                    {
                        var posts = await RestClient.GetPosts("b", thread.num);
                        Thread.Sleep(5000);
                        if (posts != null)
                        {
                            foreach (var post in posts)
                            {
                                if (post.files != null)
                                {
                                    foreach (var file in post.files)
                                    {
                                        if (file.name.Contains("webm") ||
                                            file.name.Contains("mp4") && file.duration_secs > 10)
                                        {
                                            var files = await _mediaRepository.Files.FindAsync(file.md5);
                                            if (files is null)
                                            {
                                                var caption = "";
                                                if (!string.IsNullOrEmpty(thread.subject))
                                                {
                                                    var shortDescription = thread.subject.Length > 30
                                                        ? thread.subject.Substring(0, 30)
                                                        : thread.subject;
                                                    caption =
                                                        $"<a href=\"{RestClient.Host}{post.board}/res/{thread.num}.html#{post.num}\">{shortDescription}</a>";
                                                }
                                                else
                                                {
                                                    caption =
                                                        $"<a href=\"{RestClient.Host}{post.board}/res/{thread.num}.html#{post.num}\">Link</a>";
                                                }

                                                var message = await telegramClient.SendVideo(caption,
                                                    $"{RestClient.Host}{file.path}", file);
                                                if (message is not null)
                                                {
                                                    _mediaRepository.Add(file);
                                                    await _mediaRepository.SaveChangesAsync();
                                                }
                                            }

                                            Thread.Sleep(5000);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                Thread.Sleep(5000);
            }

            Console.ReadKey();
        }
    }
}