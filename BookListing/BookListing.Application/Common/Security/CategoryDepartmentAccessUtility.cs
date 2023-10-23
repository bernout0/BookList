using BookListing.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookListing.Application.Common.Security
{
    public class CategoryDepartmentAccessUtility
    {
        public void GetAllowedCategoriesDepartment(IApplicationDbContext _context, ICurrentUserService _currentUserService,
            out List<Guid> allowedCategoryIds, out List<Guid> allowedDepartmentIds)
        {
            // Fetch the categories and departments that the user has access to.
            var userAccesses = _context.UserAccesses
                                        .Where(ua => ua.UserId == _currentUserService.UserId)
                                        .ToList();

            allowedCategoryIds = userAccesses
                                        .Where(ua => ua.CategoryId.HasValue)
                                        .Select(ua => ua.CategoryId.Value)
                                        .ToList();
            allowedDepartmentIds = userAccesses
                                        .Where(ua => ua.DepartmentId.HasValue)
                                        .Select(ua => ua.DepartmentId.Value)
                                        .ToList();
        }
    }
}
