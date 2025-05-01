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
    public class UpdateGradeStepDefinitions
    {
        private readonly RestCalls _restCalls;
        private readonly ResponseDataExtractors _extractResponseData;
        private readonly ScenarioContext _scenarioContext;
        private readonly ExtentTest _test;

        public UpdateGradeStepDefinitions(ScenarioContext scenarioContext, RestCalls restCalls, ResponseDataExtractors extractResponseData)
        {
            _scenarioContext = scenarioContext;
            _restCalls = restCalls;
            _extractResponseData = extractResponseData;
            _test = scenarioContext.Get<ExtentTest>("ExtentTest");
        }
        [Then("teacher update grade to {string}.")]
        public void TeacherUpdateGrade_(int newGrade)
        {
            int grade = _scenarioContext.Get<int>("Grade");
            string student_id = _scenarioContext.Get<string>("StudentID");
            string subject = _scenarioContext.Get<string> ("Subject");

            _test.Info($"Updating grade from {grade} to {newGrade} for student ID: {student_id} in subject: {subject}.");

            string token = _scenarioContext.Get<string>("UserToken");
            RestResponse response = _restCalls.GradeAddCall(newGrade, student_id, subject, token);
            string message = _extractResponseData.Extractor(response.Content, "message");
            _scenarioContext["Message"] = message;
            _scenarioContext.Add("NewGrade", newGrade);

            Console.WriteLine(response.Content);
            _test.Pass($"Successfully updated the grade from {grade} to {newGrade} for this student ID: {student_id} in subject: {subject}. Response message: {message}.");
        }

        [Then("validate that grade is updated {string}.")]
        public void ValidateGradeIsUpdated_(string expectedMessage)
        {
            string actualMessage = _scenarioContext.Get<string>("Message");
            string student_id = _scenarioContext.Get<string>("StudentID");
            int grade = _scenarioContext.Get<int>("Grade");
            string subject = _scenarioContext.Get<string>("Subject");
            int newGrade = _scenarioContext.Get<int>("NewGrade");

            Utilities.UtilitiesMethods.AssertEqual(
                expectedMessage,
                actualMessage,
                $"Grade update was failed: Expected message did not matched the actual when updating grade from {grade} to {newGrade} for student ID: {student_id} in subject: {subject}.",
                _scenarioContext);

            _test.Pass($"Validationwas  passed: Grade was successfully updated from {grade} to {newGrade} for student ID: {student_id} in subject: {subject}.");
        }
    }
}