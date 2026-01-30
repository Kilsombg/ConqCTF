using ConqCTF.Application.Challenges.Commands.CreateChallenge;
using ConqCTF.Application.Challenges.Commands.SubmitFlag;
using ConqCTF.Application.Challenges.Queries.GetChallenges;
using ConqCTF.Application.Challenges.Queries.GetChallengeDetails;
using ConqCTF.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using ConqCTF.Application.Challenges.DTOs;
using ConqCTF.WebApi.Models.Challenges.Requests;
using ConqCTF.Application.Challenges.Queries.DownloadChallengeFile;
using ConqCTF.Application.Challenges.Commands.UpdateChallenge;
using ConqCTF.Application.Challenges.Commands.DeleteChallenge;

[ApiController]
[Route("api/[controller]")]
public class ChallengesController : ControllerBase
{
    private readonly ISender _mediator;

    public ChallengesController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<ChallengeDto>>> GetChallenges(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] int? category = null,
        [FromQuery] int? difficulty = null,
        [FromQuery] string? status = null,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetChallengesQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            Category = category,
            Difficulty = difficulty,
            Status = status
        }, ct);

        return Ok(result);
    }


    [HttpGet("{id:int}")]
    public async Task<ActionResult<ChallengeDetailsDto>> GetChallenge(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetChallengeDetailsQuery
        {
            ChallengeId = id
        }, ct);

        return Ok(result);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<int>> Create([FromForm] CreateChallengeRequest request, CancellationToken ct)
    {
        var command = new CreateChallengeCommand
        {
            Title = request.Title,
            Description = request.Description,
            Category = request.Category,
            Difficulty = request.Difficulty,
            Points = request.Points,
            Flag = request.Flag,
            Files = request.Files?
                .Select(MapToFileUpload)
                .ToList(),
            Hints = request.Hints
        };

        var (result, challengeId) = await _mediator.Send(command, ct);

        return result.Succeeded
            ? CreatedAtAction(nameof(GetChallenge), new { id = challengeId }, challengeId)
            : BadRequest(result.Errors);
    }


    [HttpPut("{id:int}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Update(int id, [FromForm] UpdateChallengeRequest request, CancellationToken ct)
    {
        var command = new UpdateChallengeCommand
        {
            ChallengeId = id,
            Title = request.Title,
            Description = request.Description,
            Category = request.Category,
            Difficulty = request.Difficulty,
            Points = request.Points,
            Flag = request.Flag,
            Files = request.Files?
                .Select(MapToFileUpload)
                .ToList(),
            Hints = request.Hints
        };

        var result = await _mediator.Send(command, ct);

        return result.Succeeded
            ? NoContent()
            : BadRequest(result.Errors);
    }

    [HttpPost("{id:int}/submit")]
    public async Task<IActionResult> SubmitFlag(int id, [FromBody] SubmitFlagRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new SubmitFlagCommand
        {
            ChallengeId = id,
            Flag = request.Flag
        }, ct);

        return result.Succeeded
            ? Ok()
            : BadRequest(result.Errors);
    }


    [HttpGet("{id:int}/files/{fileName}")]
    public async Task<IActionResult> DownloadFile(int id, string fileName, CancellationToken ct)
    {
        var file = await _mediator.Send(new DownloadChallengeFileQuery
        {
            ChallengeId = id,
            FileName = fileName
        }, ct);

        return File(file.Stream, "application/octet-stream", file.FileName);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteChallengeCommand { ChallengeId = id }, ct);

        return result.Succeeded
            ? NoContent()
            : BadRequest(result.Errors);
    }

    private static FileUpload MapToFileUpload(IFormFile file)
    {
        return new FileUpload
        {
            FileName = file.FileName,
            Content = file.OpenReadStream()
        };
    }
}