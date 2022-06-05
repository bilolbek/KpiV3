create table comment_blocks(
    id uuid not null primary key,
    type int not null
);

create table comments(
    id uuid not null primary key,
    block_id uuid not null references comment_blocks(id),
    author_id uuid not null references employees(id),
    content text not null,
    written_date timestamp with time zone not null
);

create table posts (
    id uuid not null primary key,
    title varchar(1024) not null,
    content text not null,
    author_id uuid not null references employees(id),
    comment_block_id uuid not null references comment_blocks(id),
    written_date timestamp with time zone not null
);