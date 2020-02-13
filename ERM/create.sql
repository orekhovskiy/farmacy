create table Producer
(
	id int primary key identity(1,1),
	name nvarchar(30) not null unique
);

create table Category
(
	id int primary key identity(1,1),
	name nvarchar(30) not null unique
);

create table Form
(
	id int primary key identity(1,1),
	name nvarchar(30) not null unique
);

create table Component
(
	id int primary key identity(1,1),
	name nvarchar(30) not null unique
);

create table Position
(
	id int primary key identity(1,1),
	name nvarchar(30) not null unique
);

create table [User]
(
	id int primary key identity(1,1),
	login nvarchar(15) not null unique,
	password nvarchar(32) not null,
	firstname nvarchar(30) not null,
	lastname nvarchar(30) not null,
	position int references Position(id) not null
);

create table Medicine
(
	id int primary key identity(1,1),
	name nvarchar(30) not null unique,
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

create table Purchase
(
	id int primary key identity(1,1),
	userId int references [User](id) not null,
	medicineId int references Medicine(id) not null,
	operation int not null,
	purchaseDate date not null 
);

--DB Initializing
--Producer
insert into Producer(name) values ('Bayer');
insert into Producer(name) values ('Pfizer');
insert into Producer(name) values ('Novartis');
--Category
insert into Category(name) values ('Косметика');
insert into Category(name) values ('Витамины');
insert into Category(name) values ('Метаболики');
insert into Category(name) values ('Гармоны');
insert into Category(name) values ('Анальгетики');
--Form
insert into Form(name) values ('Жидкое вещество');
insert into Form(name) values ('Аэрозоль');
insert into Form(name) values ('Спрей');
insert into Form(name) values ('Таблетки');
--Component
insert into Component(name) values ('Вода');
insert into Form(name) values ('Ацетилсалициловая кислота');
--Position
insert into Position(name) values ('admin');
insert into Position(name) values ('manager');
insert into Position(name) values ('seller');
--Medicine
insert into Medicine(name, producerId, categoryId, formId, shelfTime, count) values ('Аспирин', 1, 5,4, 60, 45);
--MedicineComposition
insert into MedicineComposition(medicineId, componentId) values (1, 2);