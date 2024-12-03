using Microsoft.AspNetCore.Mvc;
using RestApi.Dtos;
using RestApi.Mappers;
using RestApi.Services;
using Microsoft.AspNetCore.Authorization;

namespace RestApi.Controllers;

[ApiController] //Le dice a la api que este código lo mapee como ruta
[Route("[controller]")]
[Authorize]
public class GroupsController : ControllerBase{
    
    private readonly IGroupService _groupService;

    public GroupsController(IGroupService groupService){
        _groupService = groupService;
    }

    [HttpGet("{id}")] // Esta es la ruta para obtener por ID
    public async Task<ActionResult<GroupResponse>> GetGroupById(string id, CancellationToken cancellationToken){
    var group = await _groupService.GetGroupByIdAsync(id, cancellationToken);
    if (group is null){
        return NotFound();
    }
    return Ok(group.ToDto());
}

    [HttpGet] // Esta es la ruta para buscar por nombre
    public async Task<ActionResult<List<GroupResponse>>> GetGroupsByName([FromQuery]string name, int pageIndex, int pageSize, string orderBy, CancellationToken cancellationToken){
    var groups = await _groupService.GetGroupsByNameAsync(name, pageIndex, pageSize, orderBy, cancellationToken);
    return Ok(groups.Select(g => g.ToDto()).ToList());
    }

}