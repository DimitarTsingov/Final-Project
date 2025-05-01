using System;
using System.Reactive.Subjects;
using AventStack.ExtentReports;
using BackEndAutomation.Rest.Calls;
using BackEndAutomation.Rest.DataManagement;
using BackEndAutomation.Utilities;
using Reqnroll;
using RestSharp;

namespace BackEndAutomation
{
    [Binding]
    public class GradeAddStepDefinitions
    {
        private readonly RestCalls _restCalls;
        private readonly ResponseDataExtractors _extractResponseData;
        private readonly ScenarioContext _scenarioContext;
        private readonly ExtentTest _test;

        public GradeAddStepDefinitions(ScenarioContext scenarioContext, RestCalls restCalls, ResponseDataExtractors extractResponseData)
        {
            _scenarioContext = scenarioContext;
            _restCalls = restCalls;
            _extractResponseData = extractResponseData;
            _test = scenarioContext.Get<ExtentTest>("ExtentTest");
        }
        [When("teacher adds this grade: {string}, to this student: {string}, in this subject: {string}.")]
        public void AddGradeTeacher_(int grade, string student_id, string subject)
        {
            _test.Info($"Initiating request to assign grade '{grade}' to student ID '{student_id}' for subject '{subject}'.");

            string token = _scenarioContext.Get<string>("UserToken");
            RestResponse response = _restCalls.GradeAddCall(grade, student_id, subject, token);
            string message = _extractResponseData.Extractor(response.Content, "message");
            _scenarioContext.Add("Message", message);
            _scenarioContext.Add("Subject", subject);
            _scenarioContext.Add("StudentID", student_id);
            _scenarioContext.Add("Grade", grade);

            Console.WriteLine(response.Content);
            _test.Pass($"Successfully assigned the grade '{grade}' to the student ID '{student_id}' in subject '{subject}'. API response: \"{message}\".");
        }

        [Then("validate that grade is added to student {string}.")]
        public void ValidateGradeIsAdded_(string expectedMessage)
        {
            string actualMessage = _scenarioContext.Get<string>("Message");
            string student_id = _scenarioContext.Get<string>("StudentID");
            int grade = _scenarioContext.Get<int>("Grade");
            string subject = _scenarioContext.Get<string>("Subject");

            Utilities.UtilitiesMethods.AssertEqual(
                expectedMessage,
                actualMessage,
                $"Validation was failed: Expected message did not matched the actual after assigning grade '{grade}' to student ID '{student_id}' for subject '{subject}'.",
                _scenarioContext);

            _test.Pass($"Validation was successful: Grade '{grade}' has been added to student ID '{student_id}' for subject '{subject}'.");
        }
    }
}