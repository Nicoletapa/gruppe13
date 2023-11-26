-- Step 1: Create the database and use it
CREATE DATABASE IF NOT EXISTS webAppDatabase2;
USE webAppDatabase2;

-- Step 2: Create tables without foreign key dependencies
CREATE TABLE if not EXISTS AspNetRoles
(
    Id               VARCHAR(255) NOT NULL PRIMARY KEY,
    Name             VARCHAR(255) NULL,
    NormalizedName   VARCHAR(255) NULL,
    ConcurrencyStamp VARCHAR(255) NULL
    );

CREATE TABLE if not EXISTS AspNetUsers
(
    Id                   VARCHAR(255) NOT NULL PRIMARY KEY,
    UserName             VARCHAR(255) NULL,
    NormalizedUserName   VARCHAR(255) NULL,
    Email                VARCHAR(255) NULL,
    NormalizedEmail      VARCHAR(255) NULL,
    EmailConfirmed       BIT NOT NULL,
    PasswordHash         VARCHAR(255) NULL,
    SecurityStamp        VARCHAR(255) NULL,
    ConcurrencyStamp     VARCHAR(255) NULL,
    PhoneNumber          VARCHAR(50) NULL,
    PhoneNumberConfirmed BIT NOT NULL,
    TwoFactorEnabled     BIT NOT NULL,
    LockoutEnd           TIMESTAMP NULL,
    LockoutEnabled       BIT NOT NULL,
    AccessFailedCount    INT NOT NULL,
    Discriminator        VARCHAR(255) NULL
    );

CREATE TABLE if not EXISTS Kategori
(
    KategoriID   INT NOT NULL PRIMARY KEY,
    KategoriNavn VARCHAR(50) NULL
    );

CREATE TABLE if not EXISTS Ordre1
(
    OrdreNr               int auto_increment
    primary key,
    Navn                  varchar(255)   null,
    TelefonNr             int            null,
    Epost                 varchar(255)   null,
    Type                  varchar(255)   null,
    Gjelder               varchar(255)   null,
    Adresse               varchar(255)   null,
    Uke                   int            null,
    Registrert            datetime       null,
    Bestilling            longtext       null,
    AvtaltLevering        datetime       null,
    ProduktMotatt         datetime       null,
    AvtaltFerdigstillelse datetime       null,
    ServiceFerdig         datetime       null,
    AntallTimer           decimal(10, 2) null,
    Status                tinyint(1)     null
    );

CREATE TABLE if not EXISTS __EFMigrationsHistory
(
    MigrationId    VARCHAR(150) NOT NULL PRIMARY KEY,
    ProductVersion VARCHAR(32) NOT NULL
    );

CREATE TABLE if not EXISTS Users
(
    Id    INT AUTO_INCREMENT PRIMARY KEY,
    Name  VARCHAR(255) NULL,
    Email VARCHAR(255) NULL,
    CONSTRAINT Email UNIQUE (Email)
    );

-- Step 3: Create tables with foreign key dependencies
CREATE TABLE if not EXISTS AspNetRoleClaims
(
    Id         int auto_increment
    primary key,
    ClaimType  varchar(255) not null,
    ClaimValue varchar(255) not null,
    RoleId     varchar(255) null,
    constraint Id
    unique (Id),
    constraint aspnetroleclaims_ibfk_1
    foreign key (RoleId) references AspNetRoles (Id)
    );

CREATE INDEX RoleId ON AspNetRoleClaims (RoleId);

CREATE TABLE if not EXISTS AspNetUserClaims
(
    Id         int auto_increment
    primary key,
    ClaimType  varchar(255) null,
    ClaimValue varchar(255) null,
    UserId     varchar(255) null,
    constraint Id
    unique (Id),
    constraint aspnetuserclaims_ibfk_1
    foreign key (UserId) references AspNetUsers (Id)
    );

CREATE INDEX UserId ON AspNetUserClaims (UserId);

CREATE TABLE if not EXISTS AspNetUserLogins
(
    LoginProvider       int auto_increment
    primary key,
    ProviderKey         varchar(255) not null,
    ProviderDisplayName varchar(255) not null,
    UserId              varchar(255) not null,
    constraint LoginProvider
    unique (LoginProvider),
    constraint aspnetuserlogins_ibfk_1
    foreign key (UserId) references AspNetUsers (Id)
    );

CREATE INDEX UserId ON AspNetUserLogins (UserId);

CREATE TABLE if not EXISTS AspNetUserRoles
(
    UserId varchar(255) not null,
    RoleId varchar(255) not null,
    primary key (UserId, RoleId),
    constraint aspnetuserroles_ibfk_1
    foreign key (UserId) references AspNetUsers (Id),
    constraint aspnetuserroles_ibfk_2
    foreign key (RoleId) references AspNetRoles (Id)
    );

CREATE INDEX RoleId ON AspNetUserRoles (RoleId);

CREATE TABLE if not EXISTS AspNetUserTokens
(
    UserId        varchar(255) not null,
    LoginProvider varchar(255) not null,
    Name          varchar(255) not null,
    Value         varchar(255) null,
    primary key (UserId, LoginProvider)
    );



CREATE TABLE if not EXISTS Sjekkpunkt
(
    SjekkpunktID   int          not null
    primary key,
    SjekkpunktNavn varchar(255) null,
    KategoriID     int          null,
    constraint sjekkpunkt2_ibfk_1
    foreign key (KategoriID) references Kategori (KategoriID)
    );

CREATE INDEX KategoriID ON Sjekkpunkt (KategoriID);

CREATE TABLE if not EXISTS SjekklisteSjekkpunkt
(
    SjekklisteSjekkpunktID int          not null
    primary key,
    SjekklisteID           int          null,
    Status                 varchar(255) null,
    SjekkpunktID           int          not null,
    OrdreNr      INT NOT NULL,
    CONSTRAINT Sjekkliste___fk FOREIGN KEY (OrdreNr) REFERENCES Ordre1 (OrdreNr),
    constraint SjekklisteSjekkpunkt___fk
    foreign key (SjekkpunktID) references Sjekkpunkt (SjekkpunktID)
    );
-- Insert data into Kategori table
INSERT INTO Kategori (KategoriID, KategoriNavn) VALUES
                                                    (1, 'Mekanisk'),
                                                    (2, 'Hydraulisk'),
                                                    (3, 'Elektro'),
                                                    (4, 'Trykk Settinger'),
                                                    (5, 'Funksjonstest');
INSERT INTO Sjekkpunkt (SjekkpunktID, SjekkpunktNavn, KategoriID) VALUES
                                                                       (1, 'Sjekk clutch lameller for slitasje', 1),
                                                                       (2, 'Sjekk bremser. Bånd/pål', 1),
                                                                       (3, 'Sjekk lager for trommel', 1),
                                                                       (4, 'Sjekk PTO og opplagring', 1),
                                                                       (5, 'Sjekk kjede strammer', 1),
                                                                       (6, 'Sjekk wire', 1),
                                                                       (7, 'Sjekk pinion lager', 1),
                                                                       (8, 'Sjekk kile på kjedehjul', 1),
                                                                       (9, 'Sjekk hydraulikk sylinder for lekkasje', 2),
                                                                       (10, 'Sjekk kile på kjedehjul', 2),
                                                                       (11, 'Test hydraulikk blokk i testbenk', 2),
                                                                       (12, 'Skift olje i tank', 2),
                                                                       (13, 'Skift olje på gir boks', 2),
                                                                       (14, 'Sjekk ringsylinder åpne og skift tetninger', 2),
                                                                       (15, 'Sjekk bremse sylinder åpne og skift tetninger', 2),
                                                                       (16, 'Sjekk ledningsnett på vinsj', 3),
                                                                       (17, 'Sjekk og test radio', 3),
                                                                       (18, 'Sjekk og test knappekasse', 3),
                                                                       (19, 'Xx_bar', 4),
                                                                       (20, 'Test vinsj og kjør alle funksjoner', 5),
                                                                       (21, 'TrekkKraft KN', 5),
                                                                       (22, 'BremseKraft KN', 5),
                                                                       (23, 'Kommentar', 5);

