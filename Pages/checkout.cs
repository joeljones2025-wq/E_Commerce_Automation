using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Text.Json;

namespace E_Commerce_Automation.Pages
{
    [TestFixture]
    public class CheckoutTest
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
            
            var boltTshirt = page.Locator("[data-test='inventory-item-name']").Filter(new() { HasText = "Sauce Labs Bolt T-Shirt" });
            await boltTshirt.HoverAsync();
            await Task.Delay(1000);
            await page.Locator("#add-to-cart-sauce-labs-bolt-t-shirt").ClickAsync();

            var backpack = page.Locator("[data-test='inventory-item-name']").Filter(new() { HasText = "Sauce Labs Backpack" });
            await backpack.HoverAsync();
            await Task.Delay(1000);
            await page.Locator("#add-to-cart-sauce-labs-backpack").ClickAsync();

            var bikeLight = page.Locator("[data-test='inventory-item-name']").Filter(new() { HasText = "Sauce Labs Bike Light" });
            await bikeLight.HoverAsync();
            await Task.Delay(1000);
            await page.Locator("#add-to-cart-sauce-labs-bike-light").ClickAsync();

            await page.Locator("a.shopping_cart_link").ClickAsync();
            await Task.Delay(1000);
            
            await page.Locator("[data-test='checkout']").ClickAsync();
            await Task.Delay(1000);

            await page.Locator("#first-name").FillAsync(data.Checkout.FirstName);
            await page.Locator("#last-name").FillAsync(data.Checkout.LastName);
            await page.Locator("#postal-code").FillAsync(data.Checkout.PostalCode);

            await page.Locator("[data-test='continue']").ClickAsync();
            await Task.Delay(1000);

            await page.Locator("[data-test='finish']").ClickAsync();

            await Task.Delay(3000); 
        }
    }
}