create table Producer
(
	id int primary key identity(1,1),
	name nchar(30) not null unique
);

create table Category
(
	id int primary key identity(1,1),
	name nchar(30) not null unique
);

create table Form
(
	id int primary key identity(1,1),
	name nchar(30) not null unique
);

create table Component
(
	id int primary key identity(1,1),
	name nchar(30) not null unique
);

create table Position
(
	id int primary key identity(1,1),
	name nchar(30) not null unique
);

create table [User]
(
	id int primary key identity(1,1),
	login nchar(15) not null unique,
	password nchar(32) not null,
	firstname nchar(30) not null,
	lastname nchar(30) not null,
	position int references Position(id) not null
);

create table Medicine
(
	id int primary key identity(1,1),
	name nchar(30) not null unique,
	producerId int references Producer(id) not null,
	categoryId int references Category(id) not null,
	formId int references Form(id) not null,
	shelfTime int not null,
	count int not null,
	check (count >= 0 and shelfTime > 0)
);

create table MedicineComposition
(
	id int primary key identity(1,1),
	medicineId int references Medicine(id) not null,
	componentId int references Component(id) not null,
	unique(medicineId, componentId)
);

create table Change
(
	id int primary key identity(1,1),
	userId int references [User](id) not null,
	medicineId int references Medicine(id) not null,
	operation int not null,
	changeDate date not null 
);
