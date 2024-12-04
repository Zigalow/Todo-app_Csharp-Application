using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Api.Interfaces;
using Todo.Api.Mappers;
using Todo.Api.Repositories.Interfaces;
using Todo.Core.Dtos.ProjectCollaboratorDtos;
using Todo.Core.Entities;

namespace Todo.Api.Controllers;

[Authorize]
[Route("api/projects/{projectId:int}/collaborators")]
public class ProjectCollaboratorController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthorizationRepository _authorizationRepository;

    public ProjectCollaboratorController(IUnitOfWork unitOfWork, IAuthorizationRepository authorizationRepository)
    {
        _unitOfWork = unitOfWork;
        _authorizationRepository = authorizationRepository;
    }

    [HttpGet("collaboratorsFromProject")]
    public async Task<IActionResult> GetCollaboratorsFromProject(int projectId)
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

        var currentUserId = GetCurrentUserId();
        if (!await _authorizationRepository.CanAccessProjectAsync(currentUserId, projectId))
        {
            return StatusCode(StatusCodes.Status403Forbidden, "User does not have access to this project");
        }

        var collaborators = await _unitOfWork.ProjectCollaborators.GetCollaboratorsFromProjectAsync(projectId);
        return Ok(collaborators.ToListedProjectCollaboratorDtos());
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetCollaborator(int projectId, string userId)
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

        var currentUserId = GetCurrentUserId();
        if (!await _authorizationRepository.CanAccessProjectAsync(currentUserId, projectId))
        {
            return StatusCode(StatusCodes.Status403Forbidden, "User does not have access to this project");
        }

        var collaborator = await _unitOfWork.ProjectCollaborators.GetProjectCollaboratorByIdAsync(projectId, userId);
        return collaborator == null ? NotFound("Collaborator not found") : Ok(collaborator.ToProjectCollaboratorDto());
    }

    [HttpPost]
    public async Task<IActionResult> AddProjectCollaborator(int projectId,
        AddProjectCollaboratorDto addProjectCollaboratorDto)
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

        var currentUserId = GetCurrentUserId();

        if (!await _authorizationRepository.CanManageProjectCollaborator(currentUserId, projectId))
        {
            return StatusCode(StatusCodes.Status403Forbidden,
                "User does not have permission to manage project collaborators");
        }

        var result = await _unitOfWork.ProjectCollaborators.AddCollaboratorAsync(projectId,
            addProjectCollaboratorDto.ToProjectCollaboratorFromAddDto(projectId));

        if (result.IsForbidden)
        {
            return StatusCode(StatusCodes.Status403Forbidden,
                $"{result.Error}");
        }

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        await _unitOfWork.SaveChangesAsync();

        var newCollaborator = await GetCollaboratorFromUserId(projectId, addProjectCollaboratorDto.UserId);
        return CreatedAtAction(nameof(GetCollaborator),
            new { projectId, userId = newCollaborator!.UserId },
            newCollaborator.ToProjectCollaboratorDto());
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateProjectCollaborator(int projectId, string userId,
        UpdateProjectCollaboratorDto updateProjectCollaboratorDto)
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

        var currentUserId = GetCurrentUserId();
        if (!await _authorizationRepository.CanManageProjectCollaborator(currentUserId, projectId))
        {
            return StatusCode(StatusCodes.Status403Forbidden,
                "User does not have permission to manage project collaborators");
        }

        var projectCollaborator = await _unitOfWork.ProjectCollaborators.GetProjectCollaboratorByIdAsync(projectId,
            userId);

        if (projectCollaborator == null)
        {
            return NotFound("Project collaborator not found");
        }

        projectCollaborator.UpdateProjectCollaboratorFromUpdateDto(updateProjectCollaboratorDto);
        await _unitOfWork.ProjectCollaborators.UpdateProjectCollaboratorAsync(projectCollaborator);
        await _unitOfWork.SaveChangesAsync();

        return Ok(projectCollaborator.ToProjectCollaboratorDto());
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> RemoveProjectCollaborator(int projectId, string userId)
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

        var currentUserId = GetCurrentUserId();
        if (!await _authorizationRepository.CanManageProjectCollaborator(currentUserId, projectId))
        {
            return StatusCode(StatusCodes.Status403Forbidden,
                "User does not have permission to manage project collaborators");
        }

        var result = await _unitOfWork.ProjectCollaborators.RemoveCollaboratorAsync(projectId, userId);

        if (result.IsForbidden)
        {
            return StatusCode(StatusCodes.Status403Forbidden,
                $"{result.Error}");
        }

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        await _unitOfWork.SaveChangesAsync();
        return Ok(result.Value!.ToProjectCollaboratorDto());
    }

    [HttpDelete("self")]
    public async Task<IActionResult> RemoveSelfFromProject(int projectId)
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

        var currentUserId = GetCurrentUserId();
        var result = await _unitOfWork.ProjectCollaborators.RemoveCollaboratorAsync(projectId, currentUserId);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        await _unitOfWork.SaveChangesAsync();

        return Ok(result.Value!.ToProjectCollaboratorDto());
    }

    private async Task<ProjectCollaborator?> GetCollaboratorFromUserId(int projectId, string userId)
    {
        return await _unitOfWork.ProjectCollaborators.GetProjectCollaboratorByIdAsync(projectId, userId);
    }
}