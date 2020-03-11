CREATE DATABASE EventStoreDb
            WITH OWNER = postgres
            ENCODING = 'UTF8'
            CONNECTION LIMIT = -1;
            
create table if not exists idempotences
(
    id        char(38) not null,
    createdon date     not null,
    constraint idempotences_pk
        primary key (id)
);

create table if not exists positions
(
    id              char(38) not null,
    commitposition  bigint,
    prepareposition bigint,
    createdon       date,
    constraint positions_pk
        primary key (id)
);