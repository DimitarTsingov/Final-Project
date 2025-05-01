using System;
using System.Reactive.Subjects;
using BackEndAutomation.Utilities;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace BackEndAutomation.Rest.Calls;
public class RestCalls
{
    private readonly RestClient _client;

    public RestCalls()
    {
        var options = new RestClientOptions("https://schoolprojectapi.onrender.com")
        {
            Timeout = TimeSpan.FromSeconds(120)
        };
        _client = new RestClient(options);
    }

    public RestResponse UserSignInCall(string username, string password)
    {
        var request = new RestRequest("/auth/login", Method.Post);

        request.AlwaysMultipartFormData = true; 
        request.AddParameter("username", username);
        request.AddParameter("password", password);

        RestResponse response = _client.Execute(request);
        Console.WriteLine($"UserSignIn Response '{username}': {response.Content}");

        return HandleResponse(response);
    }

    public RestResponse UserCreateCall(string username, string password, string role, string token)
    {
        var request = new RestRequest($"/users/create", Method.Post);
        request.AddHeader("Authorization", $"Bearer {token}");

        request.AddQueryParameter("username", username);
        request.AddQueryParameter("password", password);
        request.AddQueryParameter("role", role);

        RestResponse response = _client.Execute(request);
        Console.WriteLine($"UserCreate Response for role '{role}', username '{username}': {response.Content}");

        if (JObject.Parse(response.Content).ContainsKey("detail"))
        {
            return response;
        }

        return HandleResponse(response);
    }

    public RestResponse ClassCreateCall(string classname, string subject_1, string subject_2, string subject_3, string token)
    {
        var request = new RestRequest($"/classes/create", Method.Post);
        request.AddHeader("Authorization", $"Bearer {token}");

        request.AddQueryParameter("class_name", classname);
        request.AddQueryParameter("subject_1", subject_1);
        request.AddQueryParameter("subject_2", subject_2);
        request.AddQueryParameter("subject_3", subject_3);

        RestResponse response = _client.Execute(request);
        Console.WriteLine($"ClassCreate Response for classname '{classname}' with subjects '{subject_1}, {subject_2}, {subject_3}': {response.Content}");

        return HandleResponse(response);
    }

    public RestResponse StudentAddCall(string studentName, string class_id, string token)
    {
        var request = new RestRequest($"/classes/add_student", Method.Post);
        request.AddHeader("Authorization", $"Bearer {token}");

        request.AddQueryParameter("name", studentName);
        request.AddQueryParameter("class_id", class_id);

        RestResponse response = _client.Execute(request);
        Console.WriteLine($"StudentAdd Response: Adding student '{studentName}' to class ID '{class_id}': {response.Content}");

        return HandleResponse(response);
    }

    public RestResponse GradeAddCall(int grade, string student_id, string subject, string token)
    {
        var request = new RestRequest($"/grades/add", Method.Put);
        request.AddHeader("Authorization", $"Bearer {token}");

        request.AddQueryParameter("grade", grade);
        request.AddQueryParameter("student_id", student_id);
        request.AddQueryParameter("subject", subject);

        RestResponse response = _client.Execute(request);
        Console.WriteLine($"GradeAdd Response: Updating grade to '{grade}' for student ID '{student_id}' in subject '{subject}': {response.Content}");

        return HandleResponse(response);
    }

    public RestResponse ParentConnectCall(string parent_username, string student_id, string token)
    {
        var request = new RestRequest($"/users/connect_parent", Method.Put);
        request.AddHeader("Authorization", $"Bearer {token}");

        request.AddQueryParameter("parent_username", parent_username);
        request.AddQueryParameter("student_id", student_id);

        RestResponse response = _client.Execute(request);
        Console.WriteLine($"ParentConnect Response: Linking parent '{parent_username}' to student ID '{student_id}': {response.Content}");

        return HandleResponse(response);
    }

    public RestResponse GradesViewCall(string student_id, string token)
    {
        var request = new RestRequest($"/grades/student/{student_id}", Method.Get);
        request.AddHeader("Authorization", $"Bearer {token}");

        RestResponse response = _client.Execute(request);
        Console.WriteLine($"GradesView Response for student ID '{student_id}': {response.Content}");

        if (JObject.Parse(response.Content).ContainsKey("detail"))
        {
            return response;
        }

        return HandleResponse(response);

    }

    private RestResponse HandleResponse(RestResponse response)
    {
        if (!response.IsSuccessful)
        {
            Console.WriteLine($"Error with the API: {response.StatusCode} - {response.ErrorMessage}");
            Console.WriteLine($"Content of  the Response: {response.Content}");
            throw new Exception($"Wrong API Request: {response.StatusCode}");
        }

        return response;
    }


}
