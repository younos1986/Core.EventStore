see the samples for more info



### MySql tables


```
CREATE DATABASE IF NOT EXISTS EventStoreDb;

use EventStoreDb;

create table if not exists Idempotences
(
    Id        char(38) not null
        primary key,
    CreatedOn datetime not null
);

create table if not exists Positions
(
    Id              char(38) not null
        primary key,
    CommitPosition  bigint   not null,
    PreparePosition bigint   not null,
    CreatedOn       datetime not null
);


create table if not exists Customers
(
    Id char(38) not null primary key,
    FirstName varchar(32) not null,
    LastName varchar(32) not null,
    CreatedOn datetime not null
);
```