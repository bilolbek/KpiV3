alter table employees
add column avatar_id uuid references files(id);