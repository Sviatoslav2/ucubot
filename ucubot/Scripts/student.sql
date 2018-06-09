USE ucubot;
CREATE TABLE student(
id int NOT NULL AUTO_INCREMENT,
first_name varchar(255) NOT NULL,
last_name varchar(255) NOT NULL,
user_id varchar(55) NOT NULL UNIQUE,
PRIMARY KEY (id)
);