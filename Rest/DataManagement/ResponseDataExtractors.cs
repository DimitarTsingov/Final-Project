using System;
using Newtonsoft.Json.Linq;

namespace BackEndAutomation.Rest.DataManagement;
public class ResponseDataExtractors
{
    public string Extractor(string jsonResponse, string jsonIdentifier)
    {
        try
        {
            JObject jsonObject = JObject.Parse(jsonResponse);
            return jsonObject[jsonIdentifier]?.ToString() ?? throw new Exception($"There is no {jsonIdentifier} found");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Extractor Error: Can't extract '{jsonIdentifier}' from the Response. Here is the reason: {ex.Message}");
            return $"Error extracting {jsonIdentifier}";
        }
    }

    public string AllGradesExtract(string jsonResponse, string arrayKey = "grades", string propertyKey = "grade")
    {
        try
        {
            JObject jsonObject = JObject.Parse(jsonResponse);
            JArray gradesArray = (JArray)jsonObject[arrayKey];

            if (gradesArray == null || !gradesArray.Any())
                throw new Exception("Grades array was not found or it's empty in response.");

            List<string> allGrades = gradesArray
                .Select(item => item[propertyKey]?.ToString())
                .Where(g => !string.IsNullOrEmpty(g))
                .ToList();

            if (!allGrades.Any())
                throw new Exception("There are no grades extracted from the array.");

            return string.Join(",", allGrades);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"AllGradesExtract Error: Can't extract the grades from key '{arrayKey}' with property '{propertyKey}'. Here is the reason: {ex.Message}");
            return null;
        }
    }

}