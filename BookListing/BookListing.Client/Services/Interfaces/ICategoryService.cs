using BookListing.Domain.Entities;

namespace BookListing.Client.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategories();

    }
}
