create table specialties(
    id uuid not null primary key,
    name varchar(1024) not null,
    description text not null,
    position_id uuid not null references positions(id)
);