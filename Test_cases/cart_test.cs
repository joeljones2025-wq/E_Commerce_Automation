
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using ECommerceAutomation.Reports;

public class CartTest : ReportedPageTest
{
	[Test]
	public async Task ShouldDisplayAddedProductsInCart()
	{
		await Page.GotoAsync("https://www.saucedemo.com/");

		await Page.Locator("#user-name").FillAsync("standard_user");
		await Page.Locator("#password").FillAsync("secret_sauce");
		await Page.Locator("#login-button").ClickAsync();

		Assert.That(await Page.Locator(".title").InnerTextAsync(), Is.EqualTo("Products"));

		await Page.Locator("#add-to-cart-sauce-labs-bolt-t-shirt").ClickAsync();
		await Page.Locator("#add-to-cart-sauce-labs-backpack").ClickAsync();
		await Page.Locator("#add-to-cart-sauce-labs-bike-light").ClickAsync();

		await Expect(Page.Locator("[data-test='shopping-cart-badge']"))
			.ToHaveTextAsync("3");
		await Page.Locator("a.shopping_cart_link").ClickAsync();

		await Expect(Page).ToHaveURLAsync("https://www.saucedemo.com/cart.html");
	}
}
