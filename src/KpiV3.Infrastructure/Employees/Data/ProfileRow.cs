using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Positions.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KpiV3.Infrastructure.Employees.Data;

internal class ProfileRow
{
    public Guid Id { get; set; }

    public string Email { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? MiddleName { get; set; }

    public Guid? AvatarId { get; set; }

    public Guid PositionId { get; set; }
    public string PositionName { get; set; } = default!;
    public PositionType PositionType { get; set; }

    public Profile ToModel()
    {
        return new Profile
        {
            Id = Id,
            AvatarId = AvatarId,
            Email = Email,
            Name = new()
            {
                FirstName = FirstName,
                LastName = LastName,
                MiddleName = MiddleName,
            },
            Position = new()
            {
                Id = PositionId,
                Name = PositionName,
                Type = PositionType,
            },
        };
    }
}
