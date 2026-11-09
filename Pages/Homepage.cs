using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace E_Commerce_Automation.Pages
{
    internal class HomepageTest
    {
        static async Task Main(string[] args)
        {
            using var playwright = await Playwright.CreateAsync();

            var browser = await playwright.Chromium.LaunchAsync(
                new BrowserTypeLaunchOptions
                {
                    Headless = false
                });

            var page = await browser.NewPageAsync();

            await page.GotoAsync("https://www.saucedemo.com/?utm_source=chatgpt.com");


            var title = await page.TitleAsync();
            Console.WriteLine($"Page title: {title}");

            await Task.Delay(5000);

            await browser.CloseAsync();
        }
    }
}

