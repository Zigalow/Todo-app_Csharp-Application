using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Api.Interfaces;
using Todo.Api.Mappers;
using Todo.Core.Dtos.LabelDto;

namespace Todo.Api.Controllers;

[Authorize]
public class LabelsController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;

    public LabelsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllLabels()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var labels = await _unitOfWork.Labels.GetAllAsync();
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
        return label == null ? NotFound() : Ok(label.ToLabelDto());
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

        return Ok(labels.ToListedLabelDtos());
    }

    [HttpPost("for-project/{projectId:int}")]
    public async Task<IActionResult> CreateLabel(int projectId, CreateLabelDto createLabelDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var projectExists = await _unitOfWork.Projects.ExistsAsync(projectId);

        if (!projectExists)
        {
            return NotFound("Project not found");
        }

        var label = createLabelDto.ToLabelFromCreateDto(projectId);

        if (await _unitOfWork.Labels.ExistsAsync(label))
        {
            return BadRequest("Label already exists");
        }

        await _unitOfWork.Labels.AddAsync(label);
        await _unitOfWork.SaveChangesAsync();

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

        label.UpdateLabelFromUpdateDto(updateLabelDto);

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