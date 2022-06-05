create table requirements (
    id uuid not null primary key,

    specialty_id uuid not null references specialties(id),
    indicator_id uuid not null references indicators(id),
    period_part_id uuid not null references period_parts(id),

    note text,
    
    weight float not null
);