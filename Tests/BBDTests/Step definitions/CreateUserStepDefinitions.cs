using System;
using AventStack.ExtentReports;
using BackEndAutomation.Rest.Calls;
using BackEndAutomation.Rest.DataManagement;
using BackEndAutomation.Utilities;
using Reqnroll;
using RestSharp;

namespace BackEndAutomation
{
    [Binding]
    public class CreateUserStepDefinitions
    {
        private readonly RestCalls _restCalls;
        private readonly ResponseDataExtractors _extractResponseData;
        private readonly ScenarioContext _scenarioContext;
        private readonly ExtentTest _test;

        public CreateUserStepDefinitions(ScenarioContext scenarioContext, RestCalls restCalls, ResponseDataExtractors extractResponseData)
        {
            _scenarioContext = scenarioContext;
            _restCalls = restCalls;
            _extractResponseData = extractResponseData;
            _test = scenarioContext.Get<ExtentTest>("ExtentTest");
        }

        [When("admin creates a user with {string} username, {string} password, and {string} role.")]
        public void AdminCreateUser_(string username, string password, string role)
        {
            var timestamp = DateTime.Now.ToString("ddMMyyyy_HHmmss");
            username = $"{username}_{timestamp}";

            _test.Info($"Attempting to create a user with this username: {username} and this role: {role}.");

            string token = _scenarioContext.Get<string>("UserToken");
            RestResponse response = _restCalls.UserCreateCall(username, password, role, token);
            string message = _extractResponseData.Extractor(response.Content, "message");
            _scenarioContext.Add("Message", message);
            _scenarioContext.Add("Role", role);
            _scenarioContext["ParentUsername"] = username;

            Console.WriteLine(response.Content);
            _test.Pass($"The user '{username}' with role '{role}' was successfully created. Response message: {message}.");
        }

        [Then("validate user is created. {string}")]
        public void ValidateUserIsCreated_(string expectedMessage)
        {
            string role = _scenarioContext.Get<string>("Role");
            string actualMessage = _scenarioContext.Get<string>("Message");
            string username = _scenarioContext.Get<string>("Username");
            expectedMessage = $"{role} '{username}' {expectedMessage}";

            Utilities.UtilitiesMethods.AssertEqual(
                expectedMessage,
                actualMessage,
                $"User creation was failed for role '{role}' with this username '{username}'.",
                _scenarioContext);

            _test.Pass($"Validation was successful: The user '{username}' with role '{role}' was created.");
        }

        [When("admin try to create existing user with {string} username, {string} password, and {string} role.")]
        public void AdminTryToCreateExistingUser_(string username, string password, string role)
        {
            _test.Info($"Attempting to create an existing user with username: {username} and role: {role}.");

            string token = _scenarioContext.Get<string>("UserToken");
            RestResponse response = _restCalls.UserCreateCall(username, password, role, token);
            string detail = _extractResponseData.Extractor(response.Content, "detail");
            _scenarioContext.Add("Detail", detail);
            _scenarioContext.Add("Role", role);

            Console.WriteLine(response.Content);
            _test.Pass($"Received an expected error when trying to create the existing user '{username}'. Response message: {detail}.");
        }

        [Then("validate user is already created {string}.")]
        public void ValidateUserIsAlreadyCreated_(string expectedMessage)
        {
            string role = _scenarioContext.Get<string>("Role");
            string actualMessage = _scenarioContext.Get<string>("Detail");

            Utilities.UtilitiesMethods.AssertEqual(
                expectedMessage,
                actualMessage,
                $"Validation was failed: Expected duplicate user creation error for role '{role}', but received different message.",
                _scenarioContext);

            _test.Pass($"Validation was successful: Duplicate user for role '{role}' was correctly identified by the system.");
        }

    }
}