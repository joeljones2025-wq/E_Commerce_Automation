Feature: Shopping Cart

  Scenario: Add product to shopping cart
    Given I am logged in
    When I add "Sauce Labs Backpack" to the cart
    And I open the shopping cart
    Then I should see "Sauce Labs Backpack" in the cart