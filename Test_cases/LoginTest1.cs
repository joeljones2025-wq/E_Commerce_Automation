using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

public class LoginTest : ECommerceAutomation.Reports.ReportedPageTest
{
    [Test]
    public async Task ShouldLoginSuccessfully()
    {
        await Page.GotoAsync("https://www.saucedemo.com/");

        await Page.Locator("#user-name").FillAsync("standard_user");
        await Page.Locator("#password").FillAsync("secret_sauce");
        await Page.Locator("#login-button").ClickAsync();

        await Expect(Page).ToHaveURLAsync(
            "https://www.saucedemo.com/inventory.html");
    }
}