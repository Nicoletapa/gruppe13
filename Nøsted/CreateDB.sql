create table AspNetRoleClaims
(
    Id         int auto_increment
        primary key,
    ClaimType  varchar(255) not null,
    ClaimValue varchar(255) not null,
    RoleId     varchar(255) null,
    constraint Id
        unique (Id),
    constraint aspnetroleclaims_ibfk_1
        foreign key (RoleId) references webappdatabase.AspNetRoles (Id)
);

create index RoleId
    on AspNetRoleClaims (RoleId);

create table AspNetRoles
(
    Id               varchar(255) not null
        primary key,
    Name             varchar(255) null,
    NormalizedName   varchar(255) null,
    ConcurrencyStamp varchar(255) null
);

create table AspNetUserClaims
(
    Id         int auto_increment
        primary key,
    ClaimType  varchar(255) null,
    ClaimValue varchar(255) null,
    UserId     varchar(255) null,
    constraint Id
        unique (Id),
    constraint aspnetuserclaims_ibfk_1
        foreign key (UserId) references webappdatabase.AspNetUsers (Id)
);

create index UserId
    on AspNetUserClaims (UserId);

create table AspNetUserLogins
(
    LoginProvider       int auto_increment
        primary key,
    ProviderKey         varchar(255) not null,
    ProviderDisplayName varchar(255) not null,
    UserId              varchar(255) not null,
    constraint LoginProvider
        unique (LoginProvider),
    constraint aspnetuserlogins_ibfk_1
        foreign key (UserId) references webappdatabase.AspNetUsers (Id)
);

create index UserId
    on AspNetUserLogins (UserId);

create table AspNetUserRoles
(
    UserId varchar(255) not null,
    RoleId varchar(255) not null,
    primary key (UserId, RoleId),
    constraint aspnetuserroles_ibfk_1
        foreign key (UserId) references webappdatabase.AspNetUsers (Id),
    constraint aspnetuserroles_ibfk_2
        foreign key (RoleId) references webappdatabase.AspNetRoles (Id)
);

create index RoleId
    on AspNetUserRoles (RoleId);

create table AspNetUserTokens
(
    UserId        varchar(255) not null,
    LoginProvider varchar(255) not null,
    Name          varchar(255) not null,
    Value         varchar(255) null,
    primary key (UserId, LoginProvider)
);

create table AspNetUsers
(
    Id                   varchar(255) not null
        primary key,
    UserName             varchar(255) null,
    NormalizedUserName   varchar(255) null,
    Email                varchar(255) null,
    NormalizedEmail      varchar(255) null,
    EmailConfirmed       bit          not null,
    PasswordHash         varchar(255) null,
    SecurityStamp        varchar(255) null,
    ConcurrencyStamp     varchar(255) null,
    PhoneNumber          varchar(50)  null,
    PhoneNumberConfirmed bit          not null,
    TwoFactorEnabled     bit          not null,
    LockoutEnd           timestamp    null,
    LockoutEnabled       bit          not null,
    AccessFailedCount    int          not null,
    constraint Id
        unique (Id)
);

create table Ordre1
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

create table SjekklisteElektro
(
    SjekklisteElektroID      int         not null
        primary key,
    SjekkLedningsnettPåVinsj varchar(50) null,
    SjekkOgTestRadio         varchar(50) null,
    SjekkOgTestKnappekasse   varchar(50) null
);

create table SjekklisteFunksjonsTest
(
    SjekklisteFunksjonsTestID     int         not null
        primary key,
    TestVinsjOgKjørAlleFunksjoner varchar(50) null,
    TrekkkraftKN                  varchar(50) null,
    BremseKraftKN                 varchar(50) null
);

create table SjekklisteHydraulisk
(
    SjekklisteHydrauliskID                  int         not null
        primary key,
    SjekkHydraulikkSylinderForLekkasje      varchar(50) null,
    SjekkSlangerForSkaderOgLekkasje         varchar(50) null,
    TestHydraulikkBlokkITestbenk            varchar(50) null,
    SkiftOljeITank                          varchar(50) null,
    SkiftOljePåGirBoks                      varchar(50) null,
    SjekkRingsylinderÅpneOgSkiftTetninger   varchar(50) null,
    SjekkBremseSylinderÅpneOgSkiftTetninger varchar(50) null
);

create table SjekklisteKommentarer
(
    SjekklisteKommentarerID int         not null
        primary key,
    Kommentar               varchar(50) null
);

create table SjekklisteMekanisk
(
    SjekklisteMekaniskID           int         not null
        primary key,
    SjekkClutchLamellerForSlitasje varchar(50) null,
    SjekkBremserBåndPål            varchar(50) null,
    SjekkLagerForTrommel           varchar(50) null,
    SjekkPTOOgOpplagring           varchar(50) null,
    SjekkKjedeStrammer             varchar(50) null,
    SjekkWire                      varchar(50) null,
    SjekkPinionLager               varchar(50) null,
    SjekkKilePåKjedehjul           varchar(50) null
);

create table SjekklisteTrykkSettinger
(
    SjekklisteTrykkSettingerID int           not null
        primary key,
    xx_Bar                     varchar(5000) null
);

create table SjekklisteViewModel1
(
    SjekklisteID                            int auto_increment
        primary key
 
);

create table __EFMigrationsHistory
(
    MigrationId    varchar(150) not null
        primary key,
    ProductVersion varchar(32)  not null
);

