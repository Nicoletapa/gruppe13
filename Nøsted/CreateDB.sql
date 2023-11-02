
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

create table Sjekkliste3
(
    SjekklisteID                  int auto_increment
        primary key,
    Kommentar                     longtext null,
    TestVinsjOgKjørAlleFunksjoner longtext null,
    TrekkKraftKN                  longtext null,
    BremseKraftKN                 longtext null
);

create table SjekklisteSjekkpunkt1
(
    SjekklisteID int          not null,
    SjekkpunktID int          not null,
    Status       varchar(255) null,
    primary key (SjekklisteID, SjekkpunktID),
    constraint sjekklistesjekkpunkt1_ibfk_1
        foreign key (SjekklisteID) references webappdatabase2.Sjekkliste3 (SjekklisteID),
    constraint sjekklistesjekkpunkt1_ibfk_2
        foreign key (SjekkpunktID) references webappdatabase2.Sjekkpunkt (SjekkpunktID)
);

create index SjekkpunktID
    on SjekklisteSjekkpunkt1 (SjekkpunktID);

create table Sjekkpunkt
(
    SjekkpunktID   int auto_increment
        primary key,
    SjekkpunktNavn varchar(255) null,
    KategoriID     int          null,
    constraint sjekkpunkt_ibfk_1
        foreign key (KategoriID) references webappdatabase2.kategori (KategoriID)
);

create index KategoriID
    on Sjekkpunkt (KategoriID);

create table __EFMigrationsHistory
(
    MigrationId    varchar(150) not null
        primary key,
    ProductVersion varchar(32)  not null
);

