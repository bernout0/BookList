using BookListing.Application.Categories.Commands.CreateCategory;
using BookListing.Application.Categories.Commands.DeleteCategory;
using BookListing.Application.Categories.Commands.UpdateCategory;
using BookListing.Application.Categories.Queries;
using BookListing.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookListing.Web.Controllers
{
    public class CategoriesController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<CategoryDTO>>> Get()
        {
            return await Mediator.Send(new GetCategoriesQuery());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> Get(Guid id)
        {
            // Retrieve and return a specific client by ID
            var client = await Mediator.Send(new GetCategoryByIdQuery.Query { Id = id });
            return Ok(client);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Category>> Create(CreateCategoryCommand command)
        {
            return await Mediator.Send(command);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, UpdateCategoryCommand command)
        {
            command.Id = id;

            await Mediator.Send(command);

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteCategoryCommand { Id = id });

            return NoContent();
        }
    }
}
