using Microsoft.AspNetCore.Mvc;
using Todo.Api.Dtos.ProjectDtos;
using Todo.Api.Interfaces;
using Todo.Api.Mappers;

namespace Todo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public ProjectsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProjects()
    {
        var projects = await _unitOfWork.Projects.GetAllAsync();

        return Ok(projects);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProjectById(int id)
    {
        var project = await _unitOfWork.Projects.GetByIdAsync(id);
        return project == null ? NotFound() : Ok(project);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject(CreateProjectDto createProjectDto)
    {
        var createdProject = createProjectDto.ToProjectFromCreateDto();
        await _unitOfWork.Projects.AddAsync(createdProject);

        await _unitOfWork.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProjectById), new { id = createdProject.Id }, createdProject);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProject(int id, UpdateProjectDto updateProjectDto)
    {
        var project = await _unitOfWork.Projects.UpdateAsyncRequest(id, updateProjectDto);

        if (project == null)
        {
            return NotFound();
        }

        await _unitOfWork.Projects.UpdateAsync(project);
        await _unitOfWork.SaveChangesAsync();
        
        return Ok(project.ToProjectDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var project = await _unitOfWork.Projects.GetByIdAsync(id);
        if (project == null)
        {
            return NotFound();
        }

        await _unitOfWork.Projects.DeleteAsync(project);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }
}