namespace plain.Services;

using plain.Entities;

/// <summary>
/// 案件のリポジトリー
/// </summary>
public interface IIssueRepository
{
    /// <summary>
    /// 全ての案件を取得する。
    /// </summary>
    /// <returns>案件のリスト</returns>
    IEnumerable<Issue> GetAll();

    /// <summary>
    /// 指定したIDの案件を取得する。
    /// </summary>
    /// <param name="id">案件のID</param>
    /// <returns>案件</returns>
    Issue? Get(int id);

    /// <summary>
    /// 案件を追加する。
    /// </summary>
    /// <param name="issue">案件</param>
    /// <returns>追加した案件</returns>
    Issue? Add(Issue issue);

    bool Update(Issue issue);
}

public class IssueRepository : IIssueRepository
{
    private readonly ILogger<IssueRepository> _logger;

    private List<Issue> _issues = new List<Issue>();

    public IssueRepository(ILogger<IssueRepository> logger)
    {
        _logger = logger;

        // サンプルデータ作成
        _issues.Add(new Issue(1, "タイトル1", "説明1",DateTime.Now,"担当者1", 0, DateTime.Now.AddDays(1),0));
        _issues.Add(new Issue(2, "タイトル2", "説明2",DateTime.Now,"担当者2", 1, DateTime.Now.AddDays(2),0));
        _issues.Add(new Issue(3, "タイトル3", "説明3",DateTime.Now,"担当者3", 2, DateTime.Now.AddDays(3),0));
        _issues.Add(new Issue(4, "タイトル4", "説明4",DateTime.Now,"担当者4", 3, DateTime.Now.AddDays(4),0));
        _issues.Add(new Issue(5, "タイトル5", "説明5",DateTime.Now,"担当者5", 4, DateTime.Now.AddDays(5),0));
    }

    public IEnumerable<Issue> GetAll()
    {
        return _issues;
    }

    public Issue? Get(int id)
    {
        return _issues.FirstOrDefault(x => x.Id == id);
    }

    public Issue? Add(Issue issue)
    {
        if (_issues.Any(x => x.Id == issue.Id))
        {
            return null;
        }
        _issues.Add(issue);
        return issue;
    }

    public bool Update(Issue issue)
    {
        var target = _issues.FirstOrDefault(x => x.Id == issue.Id);
        if (target == null)
        {
            return false;
        }
        target.Title = issue.Title;
        target.Description = issue.Description;
        target.Priority = issue.Priority;
        target.Status = issue.Status;
        target.Assignee = issue.Assignee;
        target.CreatedAt = issue.CreatedAt;
        target.Deadline = issue.Deadline;

        return true;
    }
}