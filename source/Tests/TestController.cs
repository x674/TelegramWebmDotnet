using Xunit;

namespace telegramWebm.Tests;


public class TestController
{
    //279386461
    [Fact]
    public async Task GetThread()
    {
        var postsArray = await RestClient.GetPosts("b",279386461);
        Assert.NotNull(postsArray);
    }
}