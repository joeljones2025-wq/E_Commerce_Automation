using NUnit.Framework;
using Reqnroll;

namespace ECommerceAutomation.CSharpTests.StepDefinitions;

[Binding]
public sealed class CartSteps
{
    private const string UnitPriceKey = "UnitPrice";
    private const string CartTotalKey = "CartTotal";
    private readonly ScenarioContext _scenarioContext;

    public CartSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Given("a product costs {int} dollars")]
    public void GivenAProductCosts(int unitPrice)
    {
        _scenarioContext[UnitPriceKey] = unitPrice;
    }

    [When("I add {int} of them to the cart")]
    public void WhenIAddItemsToTheCart(int quantity)
    {
        var unitPrice = (int)_scenarioContext[UnitPriceKey];
        _scenarioContext[CartTotalKey] = unitPrice * quantity;
    }

    [Then("the cart total should be {int} dollars")]
    public void ThenTheCartTotalShouldBe(int expectedTotal)
    {
        Assert.That((int)_scenarioContext[CartTotalKey], Is.EqualTo(expectedTotal));
    }
}