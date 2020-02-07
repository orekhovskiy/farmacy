create table Producers
(
	id int primary key,
	name nchar(10) not null
);

create table Categories
(
	id int primary key,
	name nchar(10) not null
);

create table Forms
(
	id int primary key,
	name nchar(10) not null
);

create table Components
(
	id int primary key,
	name nchar(10) not null
);

create table Users
(
	id int primary key,
	login nchar(10) not null,
	password nchar(32) not null,
	firstname nchar(10) not null,
	lastname nchar(10) not null,
	position nchar(10) not null
);

create table Medicines
(
	id int primary key,
	name nchar(10) not null,
	producerId int references Producers(id),
	categoryId int references Categories(id),
	formId int references Forms(id),
	count int not null,
	check (count >= 0)
);

create table Changes
(
	id int primary key,
	userId int references Users(id),
	medicineId int references Medicines(id),
	operation int not null,
	changeDate date not null 
);
