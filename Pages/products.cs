using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace E_Commerce_Automation.Pages
{
    [TestFixture]
    public class ProductsTest
    {
        [Test]
        public async Task ViewProducts_SingleRun()
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                SlowMo = 500 
            });
            
            var page = await browser.NewPageAsync();

            await page.GotoAsync("https://www.saucedemo.com/inventory.html");
            var data = TestDataLoader.Load();
            await page.Locator("#user-name").FillAsync(data.ValidUser.Username);

            await page.Locator("#password").FillAsync(data.ValidUser.Password);

            await page.Locator("#login-button").ClickAsync();

            Assert.That(await page.Locator(".title").InnerTextAsync(), Is.EqualTo("Products"));
            
            await page.Locator("[data-test='item-1-title-link']").ClickAsync();
            await Task.Delay(1000);

            await page.Locator("#add-to-cart").ClickAsync();
            

            await Task.Delay(3000); 
        }
    }
}