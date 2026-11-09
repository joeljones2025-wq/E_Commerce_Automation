using Microsoft.Playwright;
using NUnit.Framework;
using System.Text.Json;

namespace E_Commerce_Automation.Pages
{
    [TestFixture]
    public class LoginFirstTest
    {
        private IPlaywright playwright = null!;
        private IBrowser browser = null!;
        private IPage page = null!;

        [SetUp]
        public async Task Setup()
        {
            playwright = await Playwright.CreateAsync();

            browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                SlowMo = 200
            });

            page = await browser.NewPageAsync();
        }

        [Test]
        public async Task LoginWithValidCredentials()
        {
            // Open SauceDemo
            await page.GotoAsync("https://www.saucedemo.com/");
            var data = TestDataLoader.Load();
            await page.Locator("#user-name").FillAsync(data.ValidUser.Username);
            await page.Locator("#password").FillAsync(data.ValidUser.Password);

            // Click Login
            await page.Locator("#login-button").ClickAsync();

            // Verify Products page
            Assert.That(await page.Locator(".title").InnerTextAsync(), Is.EqualTo("Products"));

            await Task.Delay(5000);
        }

        [TearDown]
        public async Task TearDown()
        {
            await browser.CloseAsync();
            playwright.Dispose();
        }
    }
}