create table files (
    id uuid not null primary key,
    name varchar(1024) not null,
    content_type varchar(1024) not null,
    length bigint not null,
    uploader_id uuid not null references employees(id)
);