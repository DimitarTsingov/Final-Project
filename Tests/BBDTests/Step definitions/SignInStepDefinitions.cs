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
    public class UserSignInStepDefinitions
    {
        private readonly RestCalls _restCalls;
        private readonly ResponseDataExtractors _extractResponseData;
        private readonly ScenarioContext _scenarioContext;
        private readonly ExtentTest _test;

        public UserSignInStepDefinitions(ScenarioContext scenarioContext, RestCalls restCalls, ResponseDataExtractors extractResponseData)
        {
            _scenarioContext = scenarioContext;
            _restCalls = restCalls;
            _extractResponseData = extractResponseData;
            _test = scenarioContext.Get<ExtentTest>("ExtentTest");
        }

        [Given("user signs in with {string} username and {string} password.")]
        public void UserSignIn_(string username, string password)
        {
            _test.Info($"Attempting user sign-in with username: {username}.");

            RestResponse response = _restCalls.UserSignInCall(username, password);
            string tokenValue = _extractResponseData.Extractor(response.Content, "access_token");
            _scenarioContext.Add("UserToken", tokenValue);
            _scenarioContext.Add("Username", username);

            Console.WriteLine(response.Content);
            _test.Pass($"User '{username}' was successfully signed in. Access token was retrieved.");
        }

        [Then("validate that the user is signed in.")]
        public void ValidateUserIsSignedIn_()
        {
            string username = _scenarioContext.Get<string>("Username");
            bool isTokenExtracted = string.IsNullOrEmpty(_scenarioContext.Get<string>("UserToken"));

            Utilities.UtilitiesMethods.AssertEqual(
                false,
                isTokenExtracted,
                $"Sign-in validation was failed: Token was not extracted or the user '{username}' was not signed in.",
                _scenarioContext);

            _test.Pass($"Validation was passed: User '{username}' was signed in and token was present.");
        }
    }
}