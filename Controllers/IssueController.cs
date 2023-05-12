using Microsoft.AspNetCore.Mvc;
using plain.Entities;
using plain.Services;
using plain.Models;
using Microsoft.AspNetCore.Authorization;

namespace plain.Controllers;


/// <summary>
/// issueのコントローラー
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class IssueController : ControllerBase
{
    private readonly ILogger<IssueController> _logger;
    private readonly IIssueRepository _issueRepository;

    private readonly IConfiguration _configuration;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="issueRepository"></param>
    /// <param name="configuration"></param>
    public IssueController(ILogger<IssueController> logger, IIssueRepository issueRepository, IConfiguration configuration)
    {
        _logger = logger;
        _issueRepository = issueRepository;
        _configuration = configuration;
    }

    /// <summary>
    /// 全てのissueを取得する。
    /// GET: api/issue
    /// </summary>
    /// <returns>issueのリスト</returns>
    /// <response code="200">成功</response>
    /// <response code="500">失敗</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IssueResponce> Get()
    {
        var issues = new IssueResponce(_issueRepository.GetAll());

        var str = _configuration["super-secret-key"] ?? "null";
        var sec = _configuration.GetConnectionString("super-secret-key") ?? "null";
        //var sec = _configuration["SQLAZURECONNSTR_super-secret-key"] ?? "null";
        //_configuration.GetChildren().ToList().ForEach(x => issues.Message += $"key: {x.Key} value: {x.Value}");
        _logger.LogInformation($"super-secret-key: {str}");
        issues.Message = $"super-secret-key: {str} SQLAZURECONNSTR_super-secret-key: {sec}";

        return Ok(issues);
    }

    /// <summary>
    /// 指定したIDのissueを取得する。
    /// GET: api/issue/{id}
    /// </summary>
    /// <param name="id">issueのID</param>
    /// <returns>issue</returns>
    /// <response code="200">成功</response>
    /// <response code="401">認証エラー</response>
    /// <response code="404">指定したIDのissueは存在しない</response>
    /// <response code="500">サーバーエラー</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IssueResponce> Get(int id)
    {
        var issue = _issueRepository.Get(id);
        if (issue == null)
        {
            return NotFound();
        }

        var responce = new IssueResponce(new List<Issue>() { issue });
        return Ok(responce);
    }

    /// <summary>
    /// issueを追加する。
    /// POST: api/issue
    /// </summary>
    /// <param name="issue">issue</param>
    /// <returns>追加したissue</returns>
    /// <response code="201">成功</response>
    /// <response code="400">リクエストボディがnull</response>
    /// <response code="401">認証エラー</response>
    /// <response code="409">IDが重複している</response>
    /// <response code="500">失敗</response>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IssueResponce> Post(IssueRequest issue)
    {
        if (issue == null)
        {
            return BadRequest(new IssueResponce(new List<Issue>(), "Request body is null."));
        }

        var newIssue = _issueRepository.Add(issue.Issue);
        if (newIssue == null)
        {
            return Conflict(new IssueResponce(new List<Issue>(), "Id conflict."));
        }

        var ret = new IssueResponce(new List<Issue>() { newIssue });

        return Ok(ret);
    }

    /// <summary>
    /// issueを更新する。
    /// PUT: api/issue/{id}
    /// </summary>
    /// <param name="id"></param>
    /// <param name="issue"></param>
    /// <returns></returns>
    /// <response code="200">成功</response>
    /// <response code="400">リクエストボディがnull</response>
    /// <response code="401">認証エラー</response>
    /// <response code="404">指定したIDのissueは存在しない</response>
    /// <response code="500">失敗</response>
    [HttpPut("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IssueResponce> Put(int id, [FromBody] IssueRequest issue)
    {
        if(issue == null)
        {
            return BadRequest(new IssueResponce(new List<Issue>(), "Request body is null."));
        }

        var ret = _issueRepository.Update(issue.Issue);
        if (!ret)
        {
            return NotFound(new IssueResponce(new List<Issue>(), "Not found."));
        }

        return Ok(new IssueResponce(new List<Issue>() { issue.Issue }));
    }


    /// <summary>
    /// issueを削除する。
    /// DELETE: api/issue/{id}
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <response code="204">成功</response>
    /// <response code="401">認証エラー</response>
    /// <response code="404">指定したIDのissueは存在しない</response>
    /// <response code="500">失敗</response>
    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IssueResponce> Delete(int id)
    {
        var ret = _issueRepository.Delete(id);
        if (!ret)
        {
            return NotFound(new IssueResponce(new List<Issue>(), "Not found."));
        }

        return NoContent();
    }
}
