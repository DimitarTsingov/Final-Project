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
    public class CreateClassStepDefinitions
    {
        private readonly RestCalls _restCalls;
        private readonly ResponseDataExtractors _extractResponseData;
        private readonly ScenarioContext _scenarioContext;
        private readonly ExtentTest _test;

        public CreateClassStepDefinitions(ScenarioContext scenarioContext, RestCalls restCalls, ResponseDataExtractors extractResponseData)
        {
            _scenarioContext = scenarioContext;
            _restCalls = restCalls;
            _extractResponseData = extractResponseData;
            _test = scenarioContext.Get<ExtentTest>("ExtentTest");
        }
        [When("teacher creates a class with {string} classname, {string} subject_1, {string} subject_2 and {string} subject_3.")]
        public void TeacherCreateClass_(string classname, string subject_1, string subject_2, string subject_3)
        {
            _test.Info($"Attempting to create class '{classname}' with subjects: '{subject_1}', '{subject_2}', '{subject_3}'.");

            string token = _scenarioContext.Get<string>("UserToken");
            RestResponse response = _restCalls.ClassCreateCall(classname, subject_1, subject_2, subject_3, token);
            string message = _extractResponseData.Extractor(response.Content, "message");
            string classID = _extractResponseData.Extractor(response.Content, "class_id");
            _scenarioContext.Add("Message", message);
            _scenarioContext.Add("ClassID", classID);
            _scenarioContext.Add("ClassName", classname);

            Console.WriteLine(response.Content);
            _test.Pass($"The class '{classname}' was successfully created. Response message: \"{message}\".");
        }

        [Then("validate class is created {string}.")]
        public void ValidateClassIsCreated_(string expectedMessage)
        {

            string actualMessage = _scenarioContext.Get<string>("Message");
            string class_id = _scenarioContext.Get<string>("ClassID");
            string classname = _scenarioContext.Get<string>("ClassName");
            bool isClassIdExtracted = string.IsNullOrEmpty(_scenarioContext.Get<string>("ClassID"));

            Utilities.UtilitiesMethods.AssertEqual(
                false,
                isClassIdExtracted,
                $"Validation failed: The class ID was not returned, indicating that '{classname}' may not have been created.",
                _scenarioContext);

            Utilities.UtilitiesMethods.AssertEqual(
                expectedMessage,
                actualMessage,
                $"Validation was failed: The expected message mismatched when creating this class '{classname}'.",
                _scenarioContext);

            _test.Pass($"Validation passed: The class '{classname}' was successfully created with ID '{class_id}'.");
        }
    }
}
