create table specialty_choices (
    specialty_id uuid not null references specialties(id),
    employee_id uuid not null references employees(id),
    period_id uuid not null references periods(id),
    can_be_changed boolean not null
);