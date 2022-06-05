create table grades (
    requirement_id uuid not null references requirements(id),
    employee_id uuid not null references employees(id),
    value float not null,

    constraint grades_pk primary key (requirement_id, employee_id)
);