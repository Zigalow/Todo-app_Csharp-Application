using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Api.Interfaces;
using Todo.Api.Mappers;
using Todo.Api.Repositories.Interfaces;
using Todo.Core.Dtos.ProjectDtos;

namespace Todo.Api.Controllers;

[Authorize]
public class ProjectsController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthorizationRepository _authorizationRepository;

    public ProjectsController(IUnitOfWork unitOfWork, IAuthorizationRepository authorizationRepository)
    {
        _unitOfWork = unitOfWork;
        _authorizationRepository = authorizationRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProjects()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = GetCurrentUserId();
        var projects = await _unitOfWork.Projects.GetAllAsync(userId);

        return Ok(projects.ToListedProjectDtos());
    }

    [HttpGet("shared")]
    public async Task<IActionResult> GetAllSharedProjects()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = GetCurrentUserId();
        var sharedProjectsWithRoles = await _unitOfWork.Projects.GetAllSharedProjectsAsync(userId);
        return Ok(sharedProjectsWithRoles.ToListedSharedProjectDtos());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProjectById(int id)
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

        var userId = GetCurrentUserId();
        if (!await _authorizationRepository.CanAccessProjectAsync(userId, id))
        {
            return StatusCode(StatusCodes.Status403Forbidden, "User does not have access to this project");
        }

        return Ok(project.ToProjectDto());
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject(CreateProjectDto createProjectDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = GetCurrentUserId();

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

        var userId = GetCurrentUserId();

        if (!await _authorizationRepository.CanModifyProjectAsync(userId, id))
        {
            return StatusCode(StatusCodes.Status403Forbidden, "User does not have permission to modify this project");
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

        var userId = GetCurrentUserId();

        if (!await _authorizationRepository.CanModifyProjectAsync(userId, id))
        {
            return StatusCode(StatusCodes.Status403Forbidden, "User does not have permission to delete this project");
        }

        await _unitOfWork.Projects.DeleteAsync(project);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }
}