
using Microsoft.Playwright;
using NUnit.Framework;
using ECommerceAutomation.Reports;

public class CheckoutTest : ReportedPageTest
{
	[Test]
	public async Task ShouldCompleteCheckoutSuccessfully()
	{
		await Page.GotoAsync("https://www.saucedemo.com/");

		await Page.Locator("#user-name").FillAsync("standard_user");
		await Page.Locator("#password").FillAsync("secret_sauce");
		await Page.Locator("#login-button").ClickAsync();

		Assert.That(await Page.Locator(".title").InnerTextAsync(), Is.EqualTo("Products"));

		await Page.Locator("#add-to-cart-sauce-labs-bolt-t-shirt").ClickAsync();
		await Page.Locator("#add-to-cart-sauce-labs-backpack").ClickAsync();
		await Page.Locator("#add-to-cart-sauce-labs-bike-light").ClickAsync();

		await Page.Locator("a.shopping_cart_link").ClickAsync();
		await Page.Locator("[data-test='checkout']").ClickAsync();

		await Page.Locator("#first-name").FillAsync("Test");
		await Page.Locator("#last-name").FillAsync("User");
		await Page.Locator("#postal-code").FillAsync("560001");
		await Page.Locator("[data-test='continue']").ClickAsync();
		await Page.Locator("[data-test='finish']").ClickAsync();

		await Expect(Page.Locator("[data-test='complete-header']"))
			.ToHaveTextAsync("Thank you for your order!");
	}
}
