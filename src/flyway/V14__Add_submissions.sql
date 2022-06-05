create table submissions (
    id uuid not null primary key,
    requirement_id uuid not null references requirements(id),
    file_id uuid not null references files(id),
    uploader_id uuid not null references employees(id),
    note text,
    status int not null,
    submission_date timestamp with time zone not null
);