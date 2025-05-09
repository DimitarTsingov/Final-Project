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
    public class ParentViewGradesStepDefinitions
    {
        private readonly RestCalls _restCalls;
        private readonly ResponseDataExtractors _extractResponseData;
        private readonly ScenarioContext _scenarioContext;
        private readonly ExtentTest _test;

        public ParentViewGradesStepDefinitions(ScenarioContext scenarioContext, RestCalls restCalls, ResponseDataExtractors extractResponseData)
        {
            _scenarioContext = scenarioContext;
            _restCalls = restCalls;
            _extractResponseData = extractResponseData;
            _test = scenarioContext.Get<ExtentTest>("ExtentTest");
        }
        [When("parent views the grades of a student with id: {string}.")]
        public void ParentViewGrades_(string student_id)
        {
            _test.Info($"Parent is attempting to view grades for student with ID: {student_id}.");

            string token = _scenarioContext.Get<string>("UserToken");
            RestResponse response = _restCalls.GradesViewCall(student_id, token);
            string allGrades = _extractResponseData.AllGradesExtract(response.Content);
            string detail = _extractResponseData.Extractor(response.Content, "detail");
            _scenarioContext.Add("StudentID", student_id);
            _scenarioContext.Add("AllGrades", allGrades);
            _scenarioContext.Add("Detail", detail);

            Console.WriteLine(response.Content);
            _test.Pass($"Grades were successfully retrieved for student ID: {student_id}. Extracted grades: {allGrades}.");
        }

        [Then("validate that the grades are displayed.")]
        public void ValidateGradesAreVisible_()
        {
            string allGrades = _scenarioContext.Get<string>("AllGrades");
            bool areGradesExtracted = !string.IsNullOrEmpty(_scenarioContext.Get<string>("AllGrades"));

            Utilities.UtilitiesMethods.AssertEqual(
                true,
                areGradesExtracted,
                "Grade extraction was failed: No grades were found or student has no assigned grades.",
                _scenarioContext);

            _test.Pass($"Validation successful: The grades are visible for the student. Grades: {allGrades}.");
        }

        [Then("validate that the student id is not valid {string}.")]
        public void ValidateStudentIdIsInvalid_(string expectedMessage)
        {
            string actualMessage = _scenarioContext.Get<string>("Detail");

            Utilities.UtilitiesMethods.AssertEqual(
                expectedMessage,
                actualMessage,
                $"Validation was failed: Expected error message for an invalid student ID was not received.",
                _scenarioContext);

            _test.Pass($"Validation was successful: The system correctly handled the invalid student ID. Message: {expectedMessage}.");
        }

        [Then("validate that the student is not linked to a parent {string}.")]
        public void ThenValidateStudentIsNotLinkedToParent_(string expectedMessage)
        {
            string actualMessage = _scenarioContext.Get<string>("Detail");

            Utilities.UtilitiesMethods.AssertEqual(
                expectedMessage,
                actualMessage,
                $"Validation was failed: The system did not returned the expected message for the unlinked student-parent relation.",
                _scenarioContext);

            _test.Pass($"Validation was successful: The student was correctly not linked to a parent. Message: {expectedMessage}.");
        }

    }
}
