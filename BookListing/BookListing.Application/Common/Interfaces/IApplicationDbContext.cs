using BookListing.Domain.Entities;
using BookListing.Domain.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookListing.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Book> Books { get; }

        DbSet<Category> Categories { get; }

        DbSet<Department> Departments { get;  }

        DbSet<BookListing.Domain.Identity.UserAccess> UserAccesses { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }

}
