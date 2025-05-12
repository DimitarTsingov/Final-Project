# 📘 School API – BDD Automated Testing Project

This project contains automated API tests for the **School API**, developed using **C#**, **SpecFlow/Reqnroll**, and **RestSharp**, with a BDD (Behavior Driven Development) approach.

## 🧪 Project Summary

The purpose of this test suite is to verify critical API operations by writing clear and maintainable BDD-style tests. The scenarios are authored in **Gherkin**, allowing both technical and non-technical stakeholders to understand the test coverage and logic.

## ✅ Features

- Built using **.NET 6** and **C#**
- Follows the **BDD methodology** with **SpecFlow/Reqnroll**
- Uses **RestSharp** for handling HTTP requests
- Structured test definitions using **Gherkin syntax**
- Detailed **HTML reports** generated via Extent Reports
- Integrated **logging** with NLog for debugging and traceability
- Robust **error handling** with descriptive output in logs and reports

## 📋 API Test Scenarios

The suite includes automated test cases that simulate real-life API interactions, such as:

- User registration and authentication
- Creating and managing classes
- Linking students to classes
- Assigning grades to students
- Allowing parents to check student performance

### Example Test Cases

- Register a new user
- Log in and obtain a valid token
- Prevent duplicate user creation
- Link a parent to a student
- Retrieve a student’s grades (valid and invalid cases)
- Create a new class
- Enroll a student in a class
- Handle invalid class IDs during enrollment
- Add and update student grades
- Handle missing or incorrect student IDs when grading

## 🛠 Tech Stack

- **.NET 6 / .NET Core**
- **C#**
- **SpecFlow / Reqnroll** for BDD support
- **RestSharp** for API calls
- **NUnit** as the test runner
- **Extent Reports** for HTML reporting
- **NLog** for logging output

## ⚙️ Setup & Execution

### 1. Clone the Repository

```bash
git clone git@github.com:YourUsername/YourRepoName.git
cd YourRepoName
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Build the Project

```bash
dotnet build
```

### 4. Run the Tests

```bash
dotnet test
```

### Output

- HTML report: `Reports/ExtentReport.html`
- Log file: `Logs/logfile.txt`

## 🧾 Error Reporting

All test failures are captured and reported with:

- Clear, human-readable error messages
- Logged technical details to assist debugging

---

## 🐞 Known Issues Found During Testing

During test execution, several functional bugs were discovered in the School API:

- Duplicate class creation is allowed (same name, no restriction)
- Students can be added to classes with invalid `class_id` values
- Grades can be added without validating if the `student_id` exists

---

> ⚠️ This project is intended for **educational and demo purposes** and assumes access to the API with the required permissions.

