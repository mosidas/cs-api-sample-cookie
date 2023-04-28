using plain.Entities;

namespace plain.Models;

public class IssueResponce
{
    public IEnumerable<Issue> Issues { get; set; }
    public string Message { get; set; }

    public IssueResponce(IEnumerable<Issue> issues, string message = "")
    {
        Issues = issues;
        Message = message;
    }
}
