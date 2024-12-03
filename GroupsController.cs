
using Microsoft.AspNetCore.Mvc;
using RestApi.Dtos;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using RestApi.Dtos;
using RestApi.Exceptions;
using RestApi.Mappers;
using RestApi.Services;

namespace RestApi.Controllers;

[ApiController]
[Route("[controller]")]
public class GroupsController : ControllerBase {
    private readonly IGroupService _groupService;

    public GroupsController (IGroupService groupService) {
        _groupService = groupService;
    }

    // localhost:port/groups/28728723
    [HttpGet("{id}")]
    public async Task<ActionResult<GroupResponse>> GetGroupById(string id, CancellationToken cancellationToken) {
        var group = await _groupService.GetGroupByIdAsync(id, cancellationToken);

        if (group is null) {
            return NotFound();
        }

        return Ok(group.ToDto());
    }
     [HttpGet]
    public async Task<ActionResult<IList<GroupResponse>>> GetGroupByName([FromQuery] string name, 
    [FromQuery] int page, [FromQuery] int pageS, [FromQuery] string orderBy, CancellationToken cancellationToken){
        
        var groups = await _groupService.GetGroupByNameAsync(name, page, pageS, orderBy, cancellationToken);

        return Ok(groups.Select(group => group.ToDto()).ToList());
    }

=======
[ApiController] //Le dice a la api que este código lo mapee como ruta
[Route("[controller]")]
public class GroupsController : ControllerBase{
    
    private readonly IGroupService _groupService;

    public GroupsController(IGroupService groupService){
        _groupService = groupService;
    }

    [HttpGet("{id}")] // Esta es la ruta para obtener por ID
    public async Task<ActionResult<GroupResponse>> GetGroupById(string Id, CancellationToken cancellationToken){
    var group = await _groupService.GetGroupByIdAsync(Id, cancellationToken);
    if (group is null){
        return NotFound();
    }
    return Ok(group.ToDto());
}

    //Get localhost/groups/id
        //200 - regresamos el objeto
        //404 - no existe el objeto
        //400 (bad request) - error del cliente
    //Query parameter pone variables en la url para hacer la consulta directamente
    //Get All localhost/groups?name=723ijfu0oahd
        //200 - retornar el listado de objetos
        //200 - retornar listado vacío
        //400 - Algún campo en el query parameter es inválido
    //Paginación
    //Delete loalhost/groups/id 
        //404 - no existe el recurso (opcional) -> 204 - no content
    //Post localhost/groups
        //400 - bad request
        //409 - conflict (name != name)
        //201 - Created (response del objeto con el nuevo id)
    //Put localhost/groups/id - Siempre mandas todos los campos
        //400 - bad request
        //409 - conflict
        //200 - response del objeto actualizado
        //204 - sin response
    //Patch localhost/groups/id (fijijshid) - Solo se actualiza el campo con el que se manda
        //400 - bad request
        //409 - conflict
        //200 - response del objeto actualizado
        //204 - sin response
    //Esta es la ruta para buscar por nombre
    
    // GET /groups?name={name}&pageIndex={pageIndex}&pageSize={pageSize}&orderBy={orderBy}
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GroupResponse>>> GetGroupsByName(
        CancellationToken cancellationToken,
        [FromQuery] string name, 
        [FromQuery] int pageIndex = 1, 
        [FromQuery] int pageSize = 10, 
        [FromQuery] string orderBy = "name")
    {
        var groups = await _groupService.GetGroupsByNameAsync(name, pageIndex, pageSize, orderBy, cancellationToken);
        
        if(groups == null || !groups.Any())
        {
            return Ok(new List<GroupResponse>());
        }

        return Ok(groups.Select(group => group.ToDto()));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGroup(string id, CancellationToken cancellationToken){
        try{
            await _groupService.DeleteGroupByIdAsync(id, cancellationToken);
            return NoContent();
        }catch(GroupNotFoundException){
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<ActionResult<GroupResponse>> CreateGroup([FromBody] CreateGroupRequest groupRequest, int pageSize, int pageIndex, string orderBy, CancellationToken cancellationToken){
        try{
            var group = await _groupService.CreateGroupAsync(groupRequest.Name, groupRequest.Users, pageIndex, pageSize, orderBy, cancellationToken);
            return CreatedAtAction(nameof(GetGroupById), new { id = group.Id}, group.ToDto());
        }catch(InvalidGroupRequestFormatException){
            return BadRequest(NewValidationProblemDetails("One or more validation problems ocurred", 
            HttpStatusCode.BadRequest, new Dictionary<string, string[]> {
                {"Groups", ["Users array is empty"]}
            }));
        }catch(GroupAlreadyExistsException){
            return Conflict(NewValidationProblemDetails("One or more validation problems ocurred", 
            HttpStatusCode.Conflict, new Dictionary<string, string[]> {
                {"Groups", ["Group with same name already exists"]}
            }));
        }catch(IdAlreadyExistsException){
            return BadRequest(NewValidationProblemDetails("One or more validation problems ocurred",
            HttpStatusCode.BadRequest, new Dictionary<string, string[]> {
                {"Id", ["The specified id already exists in the database"]}
            }));
        }
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGroup(string id, [FromBody] UpdateGroupRequest groupRequest, CancellationToken cancellationToken){
        try{
            await _groupService.UpdateGroups(id, groupRequest.Name, groupRequest.Users, cancellationToken);
            return NoContent();
        }catch(GroupNotFoundException){
            return NotFound();
        }catch(InvalidGroupRequestFormatException){
            return BadRequest(NewValidationProblemDetails("One or more validation problems ocurred", 
            HttpStatusCode.BadRequest, new Dictionary<string, string[]> {
                {"Groups", ["Users array is empty"]}
            }));
        }catch(IdAlreadyExistsException){
            return BadRequest(NewValidationProblemDetails("One or more validation problems ocurred",
            HttpStatusCode.BadRequest, new Dictionary<string, string[]> {
                {"Id", ["The specified id already exists in the database"]}
            }));
        }
    }
    //Regresa un objeto de errores personalizados
    private static ValidationProblemDetails NewValidationProblemDetails(string title, HttpStatusCode statusCode, Dictionary<string, string[]> errors){ //Dictionary funciona como una hashtable
        return new ValidationProblemDetails{
            Title = title,
            Status = (int) statusCode,
            Errors = errors
        };
    }
}