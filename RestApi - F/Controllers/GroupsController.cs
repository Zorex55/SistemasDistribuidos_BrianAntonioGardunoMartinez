using Microsoft.AspNetCore.Mvc;
using RestApi.Dtos;
using RestApi.Mappers;
using RestApi.Services;

namespace RestApi.Controllers;

[ApiController] //Le dice a la api que este código lo mapee como ruta
[Route("[controller]")]
public class GroupsController : ControllerBase{
    
    private readonly IGroupService _groupService;

    public GroupsController(IGroupService groupService){
        _groupService = groupService;
    }
    //localhost:port/groups/29876509124
    [HttpGet("{id}")]
    
    public async Task<ActionResult<GroupResponse>> GetGroupById(string id, CancellationToken cancellationToken){ //Ayuda a que .net le ponga los encabezados a las respuestas 
        var group = await _groupService.GetGroupByIdAsync(id, cancellationToken);
        if (group is null){
            return NotFound();
        }
        return Ok(group.ToDto());
    }
}