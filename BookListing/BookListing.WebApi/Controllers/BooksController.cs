using BookListing.Application.Books.Commands.CreateBook;
using BookListing.Application.Books.Commands.DeleteBook;
using BookListing.Application.Books.Commands.UpdateBook;
using BookListing.Application.Books.Queries;
using BookListing.Application.Common.Security;
using BookListing.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookListing.Web.Controllers
{
    public class BooksController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<BookDTO>>> Get()
        {
            return await Mediator.Send(new GetBooksQuery());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> Get(Guid id)
        {
            // Retrieve and return a specific client by ID
            var client = await Mediator.Send(new GetBookByIdQuery.Query { Id = id });
            return Ok(client);
        }


        [HttpPost]
        public async Task<ActionResult<BookDTO>> Create(CreateBookCommand command)
        {
            return await Mediator.Send(command);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, UpdateBookCommand command)
        {
            command.Id = id;           

            await Mediator.Send(command);

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteBookCommand { Id = id });

            return NoContent();
        }
    }
}
