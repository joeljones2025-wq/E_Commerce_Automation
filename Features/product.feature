Feature: Product Search

  Scenario: View products after login
    Given I am logged in
    When I select the "Sauce Labs Backpack" product
    Then I should see the product details