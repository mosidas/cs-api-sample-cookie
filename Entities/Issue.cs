namespace plain.Entities;

/// <summary>
/// 案件
/// </summary>
public class Issue
{
    /// <summary>
    /// ID
    /// </summary>
    /// <value></value>
    public int Id { get; set; }

    /// <summary>
    /// タイトル
    /// </summary>
    /// <value></value>
    public string Title { get; set; }

    /// <summary>
    /// 説明
    /// </summary>
    /// <value></value>
    public string Description { get; set; }


    /// <summary>
    /// 優先度
    /// </summary>
    /// <value></value>
    public int Priority { get; set; }

    /// <summary>
    /// 状態
    /// </summary>
    /// <value></value>
    public int Status { get; set; }

    /// <summary>
    /// 担当者
    /// </summary>
    /// <value></value>
    public string Assignee { get; set; }

    /// <summary>
    /// 作成日時
    /// <summary>
    /// <value></value>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 期限
    /// </summary>
    /// <value></value>
    public DateTime Deadline { get; set; }

    /// <summary>
    /// コンストラクター
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="title">タイトル</param>
    /// <param name="description">説明</param>
    /// <param name="createdAt">作成日時</param>
    /// <param name="assignee">担当者</param>
    /// <param name="priority">優先度</param>
    /// <param name="deadline">期限</param>
    /// <param name="status">状態</param>
    public Issue(int id, string title, string description, DateTime createdAt, string assignee, int priority, DateTime deadline, int status)
    {
        Id = id;
        Title = title;
        Description = description;
        CreatedAt = createdAt;
        Assignee = assignee;
        Priority = priority;
        Deadline = deadline;
        Status = status;
    }

}