Feature: Checkout

  Scenario: Complete checkout
    Given I am logged in
    When I add "Sauce Labs Backpack" to the cart
    And I open the shopping cart
    And I proceed to checkout
    And I enter first name "Joel"
    And I enter last name "Jones"
    And I enter postal code "560001"
    And I finish the checkout
    Then I should see the order confirmation