using plain.Entities;

namespace plain.Models;

public class IssueRequest
{
    public Issue Issue { get; set; }

    public IssueRequest(Issue issue)
    {
        Issue = issue;
    }
}
