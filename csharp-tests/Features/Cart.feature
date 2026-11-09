Feature: Cart totals

  Scenario: Calculate the cart subtotal
    Given a product costs 25 dollars
    When I add 2 of them to the cart
    Then the cart total should be 50 dollars