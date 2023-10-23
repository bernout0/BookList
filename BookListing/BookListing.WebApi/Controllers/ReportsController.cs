using BookListing.Application.Books.Queries;
using BookListing.Application.Reporting.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookListing.Web.Controllers
{
    public class ReportsController : ApiControllerBase
    {
        [HttpGet("total-books")]
        public async Task<ActionResult<TotalBookCountDto>> GetTotalNumberOfBooks()
        {
            return await Mediator.Send(new GetTotalNumberOfBooksQuery());
        }


        [HttpGet("books-per-author")]
        public async Task<ActionResult<List<AuthorBookCountDto>>> GetBooksPerAuthor()
        {
            return await Mediator.Send(new GetAuthorBookCounQuery());

        }

        [HttpGet("books-per-category")]
        public async Task<ActionResult<List<CategoryBookCountDto>>> GetBooksPerCategory()
        {
            return await Mediator.Send(new GetCategoryBookCountQuery());

        }


        [HttpGet("books-per-department")]
        public async Task<ActionResult<IEnumerable<DepartmentBookCountDto>>> GetBooksPerDepartment()
        {
            return await Mediator.Send(new GetDepartmentBookCountQuery());

        }
    }
}
