using BookListing.Application.Departments.Commands.CreateDepartment;
using BookListing.Application.Departments.Commands.DeleteDepartment;
using BookListing.Application.Departments.Commands.UpdateDepartment;
using BookListing.Application.Departments.Queries;
using BookListing.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookListing.Web.Controllers
{
    public class DepartmentsController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<DepartmentDTO>>> Get()
        {
            return await Mediator.Send(new GetDepartmentsQuery());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDTO>> Get(Guid id)
        {
            // Retrieve and return a specific client by ID
            var client = await Mediator.Send(new GetDepartmentByIdQuery.Query { Id = id });
            return Ok(client);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Department>> Create(CreateDepartmentCommand command)
        {
            return await Mediator.Send(command);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, UpdateDepartmentCommand command)
        {
            command.Id = id;

            await Mediator.Send(command);

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteDepartmentCommand { Id = id });

            return NoContent();
        }
    }
}
