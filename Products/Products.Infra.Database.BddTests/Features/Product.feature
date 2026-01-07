
Feature: Product Persistence in MongoDB
  As a product system
  I want to save and query products
  To ensure data integrity in the database

Background:
    Given the MongoDB database is ready

Scenario: Successfully saving a product
  Given the MongoDB database is ready
  When I save a new product
  Then the product must be persisted in MongoDB

Scenario Outline: Successfully get products by type
  Given products of type "<type>" exist
  When I request products by type "<type>"
  Then only products of type "<type>" must be returned

Examples:
  | type           |
  | Snack          |
  | Dessert        |
  | Accompaniment  |
  | Drink          |


Scenario: Successfully get all products
  Given the MongoDB database is ready
  And products exist in the database
  When I request all products
  Then all products must be returned
