create table Producer
(
	id int primary key identity(1,1),
	name nchar(10) not null
);

create table Category
(
	id int primary key identity(1,1),
	name nchar(10) not null
);

create table Form
(
	id int primary key identity(1,1),
	name nchar(10) not null
);

create table Component
(
	id int primary key identity(1,1),
	name nchar(10) not null
);

create table Position
(
	id int primary key identity(1,1),
	name nchar(10) not null
);

create table Employee
(
	id int primary key identity(1,1),
	login nchar(10) not null,
	password nchar(32) not null,
	firstname nchar(10) not null,
	lastname nchar(10) not null,
	position int references Position(id)
);

create table Medicine
(
	id int primary key identity(1,1),
	name nchar(10) not null,
	producerId int references Producer(id),
	categoryId int references Category(id),
	formId int references Form(id),
	count int not null,
	check (count >= 0)
);

create table Change
(
	id int primary key identity(1,1),
	userId int references Employee(id),
	medicineId int references Medicine(id),
	operation int not null,
	changeDate date not null 
);
