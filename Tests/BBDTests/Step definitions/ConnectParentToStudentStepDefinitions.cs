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
    public class ConnectParentToStudentStepDefinitions
    {
        private readonly RestCalls _restCalls;
        private readonly ResponseDataExtractors _extractResponseData;
        private readonly ScenarioContext _scenarioContext;
        private readonly ExtentTest _test;

        public ConnectParentToStudentStepDefinitions(ScenarioContext scenarioContext, RestCalls restCalls, ResponseDataExtractors extractResponseData)
        {
            _scenarioContext = scenarioContext;
            _restCalls = restCalls;
            _extractResponseData = extractResponseData;
            _test = scenarioContext.Get<ExtentTest>("ExtentTest");
        }
        [When("admin connect parent {string} to student with id: {string}.")]
        public void AdminConnectParentToStudent_(string parent_username, string student_id)
        {
            _test.Info($"Initiating request to link parent '{parent_username}' with student ID '{student_id}'.");
            string token = _scenarioContext.Get<string>("UserToken");
            RestResponse response = _restCalls.ParentConnectCall(parent_username, student_id, token);
            string message = _extractResponseData.Extractor(response.Content, "message");
            _scenarioContext.Add("Message", message);
            _scenarioContext.Add("ParentUsername", parent_username);
            _scenarioContext.Add("StudentID", student_id);

            Console.WriteLine(response.Content);
            _test.Pass($"The API call was successful: The parent '{parent_username}' has been linked to the student ID '{student_id}'. Response message: \"{message}\".");
        }

        [Then("validate parent is connected to student {string}.")]
        public void ValidateParentIsConnected_(string expectedMessage)
        {
            string actualMessage = _scenarioContext.Get<string>("Message");
            string parent_username = _scenarioContext.Get<string>("ParentUsername");
            string student_id = _scenarioContext.Get<string>("StudentID");

            Utilities.UtilitiesMethods.AssertEqual(
                expectedMessage,
                actualMessage,
                 $"Validation was failed: Expected message did not matched after connecting the parent '{parent_username}' to this student ID '{student_id}'.",
                _scenarioContext);

            _test.Pass($"Validationwas  successful: The parent '{parent_username}' is connected to this student ID '{student_id}'.");
        }
    }
}