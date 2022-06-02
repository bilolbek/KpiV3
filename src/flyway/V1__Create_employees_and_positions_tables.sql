create table positions (
    id uuid not null primary key,
    name varchar(255) not null unique
);

create table employees (
    id uuid not null primary key,
    email varchar(255) not null unique,
    first_name varchar(255) not null,
    last_name varchar(255) not null,
    middle_name varchar(255) null,
    position_id uuid not null references positions(id),
    reg_date timestamp with time zone not null
);