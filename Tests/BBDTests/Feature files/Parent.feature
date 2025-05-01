Feature: Parent

  Parent authentication and student grades view.

Background:
	Given user signs in with "parent1" username and "parent1" password.

Scenario: Sign in and receive a token
	Then validate that the user is signed in.

@Positive_flow
Scenario: View the student grades
	When parent views the grades of a student with id: "43bac5dc-ecba-4826-8b8d-204cecd07b18".
	Then validate that the grades are displayed.

@Positive_flow
Scenario: Try to view student grades with invalid id
	When parent views the grades of a student with id: "invalid-student-id".
	Then validate that the student id is not valid "Student not found".

@Negative_flow
Scenario: Try to view the student grades, when is not connected to a parent
	When parent views the grades of a student with id: "5b238374-36b6-477b-9ff0-9333a0b194b1".
	Then validate that the student is not linked to a parent "You can't view the grades of this student".
