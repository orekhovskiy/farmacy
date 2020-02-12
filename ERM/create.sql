create table Producer
(
	id int primary key identity(1,1),
	name nchar(10) not null unique
);

create table Category
(
	id int primary key identity(1,1),
	name nchar(10) not null unique
);

create table Form
(
	id int primary key identity(1,1),
	name nchar(10) not null unique
);

create table Component
(
	id int primary key identity(1,1),
	name nchar(10) not null unique
);

create table Position
(
	id int primary key identity(1,1),
	name nchar(10) not null unique
);

create table [User]
(
	id int primary key identity(1,1),
	login nchar(10) not null unique,
	password nchar(32) not null,
	firstname nchar(10) not null,
	lastname nchar(10) not null,
	position int references Position(id)
);

create table Medicine
(
	id int primary key identity(1,1),
	name nchar(10) not null unique,
	producerId int references Producer(id),
	categoryId int references Category(id),
	formId int references Form(id),
	count int not null,
	check (count >= 0)
);

create table MedicineComposition
(
	id int primary key identity(1,1),
	medicineId int references Medicine(id),
	componentId int references Component(id),
	unique(medicineId, componentId)
);

create table Change
(
	id int primary key identity(1,1),
	userId int references [User](id),
	medicineId int references Medicine(id),
	operation int not null,
	changeDate date not null 
);
