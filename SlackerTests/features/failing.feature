Feature:
As a developer
I want to hook up automated tests to continuous integration
So that I am notified of failing tests

Scenario:
  Given I have a test
  When it passes
  Then I have a passing test

Scenario:
  Given I have a test
  When it fails
  Then I have a failing test