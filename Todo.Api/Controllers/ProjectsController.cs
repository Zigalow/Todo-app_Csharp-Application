using Microsoft.AspNetCore.Mvc;
using Todo.Api.Dtos.ProjectDtos;
using Todo.Api.Interfaces;
using Todo.Api.Mappers;

namespace Todo.Api.Controllers;

[Route("api/projects")]
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
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var projects = await _unitOfWork.Projects.GetAllAsync();

        return Ok(projects.ToListedProjectDtos());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProjectById(int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var project = await _unitOfWork.Projects.GetByIdAsync(id);
        return project == null ? NotFound() : Ok(project.ToProjectDto());
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject(CreateProjectDto createProjectDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = "12345" ; // Hardcoded for now so they are all linked to the same aspnet user
        
        var createdProject = createProjectDto.ToProjectFromCreateDto(userId);
        await _unitOfWork.Projects.AddAsync(createdProject);
        await _unitOfWork.SaveChangesAsync();

        var newProject = await _unitOfWork.Projects.GetByIdAsync(createdProject.Id);
        return CreatedAtAction(nameof(GetProjectById), new { id = newProject!.Id }, newProject.ToProjectDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProject(int id, UpdateProjectDto updateProjectDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var project = await _unitOfWork.Projects.GetByIdAsync(id);

        if (project == null)
        {
            return NotFound();
        }

        project.UpdateProjectFromUpdateDto(updateProjectDto);

        await _unitOfWork.Projects.UpdateAsync(project);
        await _unitOfWork.SaveChangesAsync();

        return Ok(project.ToProjectDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

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