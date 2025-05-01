Feature: Admin

  Admin authentication and user management.

Background:
	Given user signs in with username "admin1" and password "admin123".

Scenario: Sign in and receive a token
	Then verify that the user is signed in.

@Positive_flow
Scenario: Create user
	When admin creates a user with "<username>" username, "<password>" password, and "<role>" role.
	Then validate user is created. "created successfully" 

Examples:
	| title              | username		| password  | role      | 
	| CREATING TEACHER   | Dimitar		| teacher11 | teacher   | 
	| CREATING MODERATOR | Tsingov		| moderator | moderator | 
	| CREATING PARENT    | parent1		| parent1   | parent    | 

@Negative_flow
Scenario: Try to create user that already exists
	When admin tries to create an existing user with "Dimitar" username, "teacher11" password, and "teacher" role.
	Then validate that the user is already created.


@Positive_flow
Scenario: Connecting a parent to the student
	When admin connects a parent "parent1" to student with id: "43bac5dc-ecba-4826-8b8d-204cecd07b18".
	Then validate that the parent is connected to the student "Parent linked to student".
    

