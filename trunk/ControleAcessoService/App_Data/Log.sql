CREATE TABLE Log (
  Id            integer PRIMARY KEY NOT NULL,
  Date          datetime NOT NULL,
  Thread        varchar(255) NOT NULL,
  Level         varchar(50) NOT NULL,
  Logger        varchar(255) NOT NULL,
  Message       text NOT NULL,
  Location      varchar(500),
  Exception     varchar(4000),
  Request		text
);