create table periods (
    id uuid not null primary key,
    name varchar(1024) not null,
    from_date timestamp with time zone not null,
    to_to timestamp with time zone not null
);