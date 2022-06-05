create table period_parts (
    id uuid not null primary key,
    name varchar(1024) not null,
    from_date timestamp with time zone not null,
    to_date timestamp with time zone not null,
    period_id uuid not null references periods(id)
);