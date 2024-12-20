using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Api.Interfaces;
using Todo.Api.Mappers;
using Todo.Api.Repositories.Interfaces;
using Todo.Core.Dtos.LabelDtos;

namespace Todo.Api.Controllers;

[Authorize]
public class LabelsController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthorizationRepository _authorizationRepository;

    public LabelsController(IUnitOfWork unitOfWork, IAuthorizationRepository authorizationRepository)
    {
        _unitOfWork = unitOfWork;
        _authorizationRepository = authorizationRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllLabels()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = GetCurrentUserId();

        var labels = await _unitOfWork.Labels.GetAllAsync(userId);
        return Ok(labels.ToListedLabelDtos());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetLabelById(int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var label = await _unitOfWork.Labels.GetByIdAsync(id);

        if (label == null)
        {
            return NotFound("Label not found");
        }

        var userId = GetCurrentUserId();

        if (!await _authorizationRepository.CanAccessLabelAsync(userId, id))
        {
            return StatusCode(StatusCodes.Status403Forbidden, "User does not have access to this label");
        }

        return Ok(label.ToLabelDto());
    }

    [HttpGet("by-project/{projectId:int}")]
    public async Task<IActionResult> GetAllLabelsForProject(int projectId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var labels = await _unitOfWork.Labels.GetAllLabelsForProject(projectId);

        if (labels == null)
        {
            return NotFound("Project not found");
        }

        var userId = GetCurrentUserId();

        if (!await _authorizationRepository.CanAccessProjectAsync(userId, projectId))
        {
            return StatusCode(StatusCodes.Status403Forbidden ,"User does not have access to this project");
        }

        return Ok(labels.ToListedLabelDtos());
    }

    [HttpPost("for-project/{projectId:int}")]
    public async Task<IActionResult> CreateLabel(int projectId, CreateLabelDto createLabelDto)
    {
        Console.WriteLine("Label begin");
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var projectExists = await _unitOfWork.Projects.ExistsAsync(projectId);

        if (!projectExists)
        {
            return NotFound("Project not found");
        }

        var userId = GetCurrentUserId();

        if (!await _authorizationRepository.CanCreateLabelAsync(userId, projectId))
        {
            return StatusCode(StatusCodes.Status403Forbidden, "User does not have access to this project");
        }

        var label = createLabelDto.ToLabelFromCreateDto(projectId);

        if (await _unitOfWork.Labels.ExistsAsync(projectId, createLabelDto.Name))
        {
            return BadRequest("Label already exists");
        }

        await _unitOfWork.Labels.AddAsync(label);
        await _unitOfWork.SaveChangesAsync();
        Console.WriteLine("Label Created");
        return CreatedAtAction(nameof(GetLabelById), new { id = label.Id }, label.ToLabelDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateLabel(int id, UpdateLabelDto updateLabelDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var label = await _unitOfWork.Labels.GetByIdAsync(id);

        if (label == null)
        {
            return NotFound("Label not found");
        }

        var userId = GetCurrentUserId();

        if (!await _authorizationRepository.CanModifyLabelAsync(userId, id))
        {
            return StatusCode(StatusCodes.Status403Forbidden, "User does not have access to modify this label");
        }

        label.UpdateLabelFromUpdateDto(updateLabelDto);

        if (!await _unitOfWork.Labels.ExistsAsync(label.ProjectId, label.Name))
        {
            return BadRequest("Label already exists");
        }

        await _unitOfWork.Labels.UpdateAsync(label);
        await _unitOfWork.SaveChangesAsync();

        return Ok(label.ToLabelDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteLabel(int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var label = await _unitOfWork.Labels.GetByIdAsync(id);

        if (label == null)
        {
            return NotFound("Label not found");
        }

        await _unitOfWork.Labels.DeleteAsync(label);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }
}