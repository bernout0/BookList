namespace BookListing.Application.IntegrationTests
{
    using NUnit.Framework;
    using static TestManager;

    public class TestBase
    {
        [SetUp]
        public async Task TestSetUp()
        {
            await ResetState();
        }
    }

}
