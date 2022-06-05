using KpiV3.Domain.Employees.DataContracts;

namespace KpiV3.Domain.DataContracts.Models;

public class Author
{
    public Author()
    {
    }

    public Author(Employee employee)
    {
        Id = employee.Id;
        Name = employee.Name;
        AvatarId = employee.AvatarId;
    }

    public Guid Id { get; set; }

    public Name Name { get; set; }

    public Guid? AvatarId { get; set; }
}
