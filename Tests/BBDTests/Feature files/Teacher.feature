Feature: Teacher

  Teacher authentication and class management.

Background:
	Given user signs in with "Dimitar" username and "teacher11" password.

Scenario: Sign in and receive a token
	Then validate that the user is signed in.

@Positive_flow
Scenario Outline: Create a class
	When teacher creates a class with {string} classname, {string} subject_1, {string} subject_2 and {string} subject_3.
	Then validate class is created {string}.

Examples:
	| classname | subject_1 | subject_2 | subject_3  | message       |
	| Class C   | Math      | History   | Biologic   | Class created |
	| Class B   | Chemistry | Physics   | Literature | Class created |

@Positive_flow
Scenario Outline: Add the stuednt to a class
	When the teacher adds student with {string} name and {string} class id.
	Then validate that student is successfully added {string}.

Examples:
	| name     | class_id                             | message       |
	| Student6 | ecadac35-dd50-4120-b876-411ec0d51cd9 | Student added |
	| Student2 | 2f2fa5e2-6c5e-4e58-80b4-bf469eff79e8 | Student added |


@Positive_flow
Scenario: Add and update grade
	When teacher adds this grade: {string}, to this student: {string}, in this subject: {string}.
	Then validate that grade is added to student {string}.
	And teacher update grade to {string}.
	And validate that grade is updated {string}.
