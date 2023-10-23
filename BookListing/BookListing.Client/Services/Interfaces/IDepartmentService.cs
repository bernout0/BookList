using BookListing.Domain.Entities;

namespace BookListing.Client.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<List<Department>> GetDepartments();

    }
}
