
using BookListing.Application.Common.Exceptions;
using FluentAssertions;
using FluentValidation.Results;
using NUnit.Framework;

namespace CleanArchitecture.Application.UnitTests.Common.Exceptions;

public class ValidationExceptionTests
{
    [Test]
    public void DefaultConstructorCreatesAnEmptyErrorDictionary()
    {
        var actual = new ValidationException().Errors;

        // Assert
        actual.Keys.Should().BeEmpty();
    }

    [Test]
    public void SingleValidationFailureCreatesASingleElementErrorDictionary()
    {
        // Arrange
        var failures = new List<ValidationFailure>
        {
            new ValidationFailure("Email", "must be a valid email format"),
        };

        // Act
        var actual = new ValidationException(failures).Errors;

        // Assert
        actual.Keys.Should().ContainSingle("Email");
        actual["Email"].Should().ContainSingle("must be a valid email format");
    }

    [Test]
    public void GivenMultipleValidationFailuresForMultipleProperties_ThenErrorDictionaryHasMultipleEntriesWithMultipleValues()
    {
        // Arrange
        var failures = new List<ValidationFailure>
        {
            new ValidationFailure("Username", "must be unique"),
            new ValidationFailure("Username", "cannot contain special characters"),
            new ValidationFailure("Password", "must contain at least 8 characters"),
            new ValidationFailure("Password", "must contain a digit"),
            new ValidationFailure("Password", "must contain an upper case letter"),
            new ValidationFailure("Password", "must contain a lower case letter"),
        };

        // Act
        var actual = new ValidationException(failures).Errors;

        // Assert
        actual.Keys.Should().BeEquivalentTo("Username", "Password");

        actual["Username"].Should().BeEquivalentTo("must be unique", "cannot contain special characters");

        actual["Password"].Should().BeEquivalentTo(
            "must contain at least 8 characters",
            "must contain a digit",
            "must contain an upper case letter",
            "must contain a lower case letter"
        );
    }
}
