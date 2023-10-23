namespace BookListing.Application.Reporting.Queries
{
    public class TotalBookCountDto
    {
        public int BookCount { get; set; }
    }


    public class AuthorBookCountDto
    {
        public string Author { get; set; }
        public int BookCount { get; set; }
    }

    public class CategoryBookCountDto
    {
        public string CategoryName { get; set; }
        public int BookCount { get; set; }
    }

    public class DepartmentBookCountDto
    {
        public string DepartmentName { get; set; }
        public int BookCount { get; set; }
    }
}
