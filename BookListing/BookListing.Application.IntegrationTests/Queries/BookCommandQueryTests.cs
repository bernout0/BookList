using NUnit.Framework.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookListing.Application.Books.Queries;
using FluentAssertions;
using BookListing.Domain.Entities;
using BookListing.Application.Books.Commands.CreateBook;

namespace BookListing.Application.IntegrationTests.Queries
{
    public class BookCommandQueryTests: TestBase
    {
        [Test]
        public async Task ShouldCreateBookAsAdmin()
        {
            var userId = await TestManager.RunAsAdministratorAsync();

            var command = new CreateBookCommand
            {
                Title = "Data structures", Author = "Bernie", Description = "Saddest book"
            };

            var id = await TestManager.SendAsync(command);

            var list = await TestManager.FindAsync<Book>(id);

            list.Should().NotBeNull();
            list!.Title.Should().Be(command.Title);
            list!.Author.Should().Be(command.Author);
            list!.Description.Should().Be(command.Description);
            list.CreatedBy.Should().Be(userId);
        }
    }
}
