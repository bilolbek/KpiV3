using FluentAssertions;
using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Employees.Commands;
using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Employees.Ports;
using KpiV3.Domain.Ports;
using Moq;

namespace KpiV3.Domain.UnitTests.Employees;

public class RegisterEmployeeCommandTests
{
    private static readonly Guid EmployeeId = Guid.NewGuid();
    private static readonly DateTimeOffset RegistrationDate = new(2022, 6, 2, 20, 36, 0, TimeSpan.Zero);
    private const string Password = "qwerty123";
    private const string Hash = "SOME_RANDOM_HASH";

    private readonly Mock<IEmployeeRepository> _employeeRepository = new();
    private readonly Mock<IPasswordGenerator> _passwordGenerator = new();
    private readonly Mock<IPasswordHasher> _passwordHasher = new();
    private readonly Mock<IGuidProvider> _guidProvider = new();
    private readonly Mock<IDateProvider> _dateProvider = new();
    private readonly Mock<IEmailSender> _emailSender = new();

    [Fact]
    public async Task Returns_ok_on_happy_path()
    {
        // Arrange
        var command = CreateCommand();
        var handler = CreateHandler();

        _employeeRepository
            .Setup(repository => repository.InsertAsync(It.IsAny<Employee>()))
            .ReturnsAsync(Result<IError>.Ok());

        _emailSender
            .Setup(sender => sender.SendAsync(It.IsAny<EmailMessage>()))
            .ReturnsAsync(Result<IError>.Ok());

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Returns_fail_if_database_insert_fails()
    {
        // Arrange
        var command = CreateCommand();
        var handler = CreateHandler();

        var expecterError = new TestError("error");

        _employeeRepository
            .Setup(repository => repository.InsertAsync(It.IsAny<Employee>()))
            .ReturnsAsync(Result<IError>.Fail(expecterError));

        _emailSender
            .Setup(sender => sender.SendAsync(It.IsAny<EmailMessage>()))
            .ReturnsAsync(Result<IError>.Ok());

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Failure.Should().Be(expecterError);
    }

    [Fact]
    public async Task Returns_fail_if_email_sending_fails()
    {
        // Arrange
        var command = CreateCommand();
        var handler = CreateHandler();

        var expecterError = new TestError("error");

        _employeeRepository
            .Setup(repository => repository.InsertAsync(It.IsAny<Employee>()))
            .ReturnsAsync(Result<IError>.Ok());

        _emailSender
            .Setup(sender => sender.SendAsync(It.IsAny<EmailMessage>()))
            .ReturnsAsync(Result<IError>.Fail(expecterError));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Failure.Should().Be(expecterError);
    }

    [Fact]
    public async Task Inserts_employee_in_database()
    {
        // Arrange
        var command = CreateCommand();
        var handler = CreateHandler();

        _employeeRepository
            .Setup(repository => repository.InsertAsync(It.IsAny<Employee>()))
            .ReturnsAsync(Result<IError>.Ok());

        _emailSender
            .Setup(sender => sender.SendAsync(It.IsAny<EmailMessage>()))
            .ReturnsAsync(Result<IError>.Ok());

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _employeeRepository.Verify(repository => repository.InsertAsync(
            It.Is<Employee>(employee =>
                employee.Id == EmployeeId &&
                employee.Email == command.Email &&
                employee.Name == command.Name &&
                employee.PasswordHash == Hash &&
                employee.PositionId == command.PositionId &&
                employee.RegistrationDate == RegistrationDate)));
    }

    [Fact]
    public async Task Sends_email_to_employee_after_it_inserted_in_database()
    {
        // Arrange
        var command = CreateCommand();
        var handler = CreateHandler();

        _employeeRepository
            .Setup(repository => repository.InsertAsync(It.IsAny<Employee>()))
            .ReturnsAsync(Result<IError>.Ok());

        _emailSender
            .Setup(sender => sender.SendAsync(It.IsAny<EmailMessage>()))
            .ReturnsAsync(Result<IError>.Ok());

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _emailSender
            .Verify(sender => sender.SendAsync(It.Is<EmailMessage>(message =>
                message.Recipient == command.Email &&
                message.Subject == "KPI Platform Registration" &&
                message.Body == $"You have been registered in KPI platform. Your password is: {Password}")));
    }

    [Fact]
    public async Task Generates_password_using_password_generator()
    {
        // Arrange
        var command = CreateCommand();
        var handler = CreateHandler();

        _employeeRepository
            .Setup(repository => repository.InsertAsync(It.IsAny<Employee>()))
            .ReturnsAsync(Result<IError>.Ok());

        _emailSender
            .Setup(sender => sender.SendAsync(It.IsAny<EmailMessage>()))
            .ReturnsAsync(Result<IError>.Ok());

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _passwordGenerator
            .Verify(generator => generator.GeneratePassword());
    }

    [Fact]
    public async Task Hashes_password_using_password_hasher()
    {
        // Arrange
        var command = CreateCommand();
        var handler = CreateHandler();

        _employeeRepository
            .Setup(repository => repository.InsertAsync(It.IsAny<Employee>()))
            .ReturnsAsync(Result<IError>.Ok());

        _emailSender
            .Setup(sender => sender.SendAsync(It.IsAny<EmailMessage>()))
            .ReturnsAsync(Result<IError>.Ok());

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _passwordHasher
            .Verify(hasher => hasher.Hash(Password));
    }

    private RegisterEmployeeCommand CreateCommand()
    {
        return new RegisterEmployeeCommand
        {
            Email = "employee@kpi.com",

            Name = new Name
            {
                FirstName = "Carl",
                MiddleName = "Ashland",
                LastName = "Waterson",
            },

            PositionId = Guid.NewGuid(),
        };
    }

    private RegisterEmployeeCommandHandler CreateHandler()
    {
        _guidProvider
            .Setup(provider => provider.New())
            .Returns(EmployeeId);

        _dateProvider
            .Setup(provider => provider.Now())
            .Returns(RegistrationDate);

        _passwordGenerator
            .Setup(generator => generator.GeneratePassword())
            .Returns(Password);

        _passwordHasher
            .Setup(hasher => hasher.Hash(Password))
            .Returns(Hash);

        return new RegisterEmployeeCommandHandler(
            _employeeRepository.Object,
            _passwordGenerator.Object,
            _passwordHasher.Object,
            _emailSender.Object,
            _guidProvider.Object,
            _dateProvider.Object);
    }
}
