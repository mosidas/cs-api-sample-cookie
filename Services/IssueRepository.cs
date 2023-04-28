namespace plain.Services;

using plain.Entities;

/// <summary>
/// issueのリポジトリー
/// </summary>
public interface IIssueRepository
{
    /// <summary>
    /// 全てのissueを取得する。
    /// </summary>
    /// <returns>issueのリスト</returns>
    IEnumerable<Issue> GetAll();

    /// <summary>
    /// 指定したIDのissueを取得する。
    /// </summary>
    /// <param name="id">issueのID</param>
    /// <returns>issue</returns>
    Issue? Get(int id);

    /// <summary>
    /// issueを追加する。
    /// </summary>
    /// <param name="issue">issue</param>
    /// <returns>追加したissue</returns>
    Issue? Add(Issue issue);

    /// <summary>
    /// issueを更新する。
    /// </summary>
    /// <param name="issue"></param>
    /// <returns>更新したかどうか</returns>
    bool Update(Issue issue);

    /// <summary>
    /// issueを削除する。
    /// </summary>
    /// <param name="id">issueのID</param>
    /// <returns>削除したかどうか</returns>
    bool Delete(int id);
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

    public bool Delete(int id)
    {
        var target = _issues.FirstOrDefault(x => x.Id == id);
        if (target == null)
        {
            return false;
        }
        _issues.Remove(target);
        return true;
    }
}