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
    public class AddStudentStepDefinitions
    {
        private readonly RestCalls _restCalls;
        private readonly ResponseDataExtractors _extractResponseData;
        private readonly ScenarioContext _scenarioContext;
        private readonly ExtentTest _test;

        public AddStudentStepDefinitions(ScenarioContext scenarioContext, RestCalls restCalls, ResponseDataExtractors extractResponseData)
        {
            _scenarioContext = scenarioContext;
            _restCalls = restCalls;
            _extractResponseData = extractResponseData;
            _test = scenarioContext.Get<ExtentTest>("ExtentTest");
        }
        [When("the teacher adds student with {string} name and {string} class id.")]
        public void TeacherAddStudent(string studentName, string class_id)
        {
            _test.Info($"Sending the request for adding the student '{studentName}' to class ID '{class_id}'.");

            string token = _scenarioContext.Get<string>("UserToken");
            RestResponse response = _restCalls.StudentAddCall(studentName, class_id, token);
            string message = _extractResponseData.Extractor(response.Content, "message");
            string studentID = _extractResponseData.Extractor(response.Content, "student_id");
            _scenarioContext.Add("Message", message);
            _scenarioContext.Add("StudentName", studentName);
            _scenarioContext.Add("StudentID", studentID);
            _scenarioContext.Add("ClassID", class_id);

            Console.WriteLine(response.Content);
            _test.Pass($"The student '{studentName}' (ID: {studentID}) was successfully added to the class '{class_id}'. Here is the API response: \"{message}\".");
        }

        [Then("validate that student is successfully added {string}.")]
        public void ValidateStudentIsAdded_(string expectedMessage)
        {
            string actualMessage = _scenarioContext.Get<string>("Message");
            string class_id = _scenarioContext.Get<string>("ClassID");
            string studentName = _scenarioContext.Get<string>("StudentName");
            bool isStudentIdExtracted = string.IsNullOrEmpty(_scenarioContext.Get<string>("StudentID"));

            Utilities.UtilitiesMethods.AssertEqual(
                false,
                isStudentIdExtracted,
                $"Failed to extract the Student ID. This suggests that the student '{studentName}' may not have been successfully added.",
                _scenarioContext);

            Utilities.UtilitiesMethods.AssertEqual(
                expectedMessage,
                actualMessage,
                $"API response did not match the expected result when adding the student '{studentName}'.",
                _scenarioContext);

            _test.Pass($"Validation passed: The student '{studentName}' has been successfully added to the class '{class_id}'.");
        }

        [Then("validate that student is not added {string}.")]
        public void ValidateStudentIsNotAdded_(string expectedMessage)
        {
            string actualMessage = _scenarioContext.Get<string>("Message");
            string class_id = _scenarioContext.Get<string>("ClassID");
            string studentName = _scenarioContext.Get<string>("StudentName");

            Utilities.UtilitiesMethods.AssertEqual(
                expectedMessage,
                actualMessage,
                $"Validation failed: The expected error message when adding the student '{studentName}' to class '{class_id}' was not returned.",
                _scenarioContext);

            _test.Pass($"Validation passed: The student '{studentName}' was not added to the class '{class_id}' as expected.");
        }

    }
}