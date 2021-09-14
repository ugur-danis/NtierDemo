/* MySql script for generate database */

drop database if exists ntierdemo;
create database ntierdemo; /* Creating a database called niterdemo */
use ntierdemo; /* Use the created database */

/* Database in create a new table called authors */
create table authors (
	id int auto_increment primary key not null unique,
    name varchar(50) not null,
    last_name varchar(50) not null
);

/* Database in create a new table called books */
create table books (
	id int auto_increment primary key not null,
    title varchar(50) not null,
    relase_year varchar(4) not null
);

/* Database in create a new table called types */
create table types (
	id int auto_increment primary key not null,
    type_name varchar(50) not null
);

/* Database in create a new table called books_types */
create table books_types (
	book_id int not null,
    type_id int not null,
    primary key (book_id, type_id),
    constraint fk_bt_book_id foreign key (book_id) references books(id), /* create a foreign key for book_id */
    constraint fk_bt_type_id foreign key (type_id) references types(id) /* create a foreign key for type_id */
);

/* Database in create a new table called authors_books */
create table authors_books (
	author_id int not null,
	book_id int not null,
    primary key (author_id, book_id),
    constraint fk_ab_author_id foreign key (author_id) references authors(id), /* create a foreign key for author_id */
    constraint fk_ab_book_id foreign key (book_id) references books(id) /* create a foreign key for book_id */
);

/* Add data to authors table */
insert into authors (name, last_name) values
("Halide Edib", "Adıvar"),
("Sabahattin", "Ali"),
("Sait Faik", "Abasıyanık"),
("Halit Ziya", "Uşaklıgil"),
("Ahmet Hamdi", "Tanpınar"),
("Mehmet Akif", "Ersoy");

/* Add data to books table */
insert into books (title, relase_year) values
/* authorId 1 */
("Sinekli Bakkal", 1935),
("Ateşten Gömlek", 1922),
("Vurun Kahpeye", 1926),
/* authorId 2 */
("Kürk Mantolu Madonna", 1943),
("Kuyucaklı Yusuf", 1937),
("İçimizdeki Şeytan", 1940),
/* authorId 3 */
("Son Kuşlar", 1952),
("Lüzumsuz Adam", 1948),
("Mahalle Kahvesi", 1950),
/* authorId 4 */
("Aşk-ı Memnu", 1899),
("Mai ve Siyah", 1897),
("Kırık Hayatlar", 1923),
/* authorId 5 */
("Huzur", 1948),
("Beş Şehir", 1946),
("Mahur Beste", 1944),
/* authorId 6 */
("Safahat", 1888),
("Asım", 1924),
("Hatıralar", 1917);

/* Add data to types table */
insert into types (type_name) values ("Roman"), ("Kurgu"), ("Aşk Romanı"), ("Hikaye"), ("Tarihî Kurgu"), ("Şiir"), ("Dram"), ("Biyografi"), ("Gezi Edebiyatı");

/* Add data to books_types table */
insert into books_types values
(1, 1), (2, 2), (3, 2),
(4, 1), (4, 3), (5, 2),
(6, 3), (6, 7), (7, 4),
(8, 4), (9, 4), (10, 2),
(11, 1), (12, 1), (13, 1),
(13, 3), (13, 5), (14, 8),
(14, 9), (15, 2), (16, 5),
(17, 4), (18, 6);

/* Add data to authors_books table */
insert into authors_books values
(1, 1), (1, 2), (1, 3),
(2, 4), (2, 5), (2, 6),
(3, 7), (3, 8), (3, 9),
(4, 10), (4, 11), (4, 12),
(5, 13), (5, 14), (5, 15),
(6, 16), (6, 17), (6, 18);