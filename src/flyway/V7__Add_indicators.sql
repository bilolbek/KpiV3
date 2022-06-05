create table indicators (
    id uuid not null primary key,
    name varchar(1024) not null,
    description text not null,
    comment text not null
);