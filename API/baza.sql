CREATE TABLE Accounts
(
    AccountID int NOT NULL AUTO_INCREMENT,
    LastName varchar(255),
    FirstName varchar(255),
    Login varchar(255),
    Password varchar(255),
    Email varchar(255),
    Description varchar(MAX),
    Microphone bit,
    ProfilePicture varchar(255),
    Status varchar(255),
    Language varchar(255),
    RepPoint real,
    PRIMARY KEY (AccountID)
);

CREATE TABLE LinkAccount
(
    LinkAccountID int NOT NULL AUTO_INCREMENT,
    PSN_Account varchar(255),
    XBOX_Account varchar(255),
    STEAM_Account varchar(255),
    ORIGIN_Account varchar(255),
    DISCORD_Account varchar(255),
    UPLAY_Account varchar(255),
    BATTLE_Account varchar(255),
    LOL_Account varchar(255),
    SKYPE_Account varchar(255),
    AccountID int,
    CONSTRAINT fk_lAccountID FOREIGN KEY (AccountID) REFERENCES Accounts(AccountID),
    PRIMARY KEY (LinkAccountID)
);

CREATE TABLE Games
(
	GameID int NOT NULL AUTO_INCREMENT,
	GameName varchar(255),
	Description varchar(255),
	ImageURL varchar(255),
	PRIMARY KEY (GameID)
);

CREATE TABLE Events
(
	EventID int NOT NULL AUTO_INCREMENT,
	EventName varchar(255),
	GameID int,
	Description varchar(255),
	Date varchar(255),
	PRIMARY KEY (EventID),
	CONSTRAINT fk_GameID FOREIGN KEY (GameID) REFERENCES Games(GameID)
);

CREATE TABLE EventParticipants
(
	ParticipationID int NOT NULL AUTO_INCREMENT,
	EventID int,
	AccountID int,
	PRIMARY KEY (ParticipationID),
	CONSTRAINT fk_EventID FOREIGN KEY (EventID) REFERENCES Events(EventID),
	CONSTRAINT fk_AccountID FOREIGN KEY (AccountID) REFERENCES Accounts(AccountID)
);

CREATE TABLE Friends
(
	FriendshipID int NOT NULL AUTO_INCREMENT,
	AccountID1 int,
	AccountID2 int,
	PRIMARY KEY (FriendshipID),
	CONSTRAINT fk_AccountID1 FOREIGN KEY (AccountID1) REFERENCES Accounts(AccountID),
	CONSTRAINT fk_AccountID2 FOREIGN KEY (AccountID2) REFERENCES Accounts(AccountID)
);

CREATE TABLE Invitations
(
	InviteID int NOT NULL AUTO_INCREMENT,
	AccountID int,
	EventID int,
	PRIMARY KEY (InviteID),
	CONSTRAINT fk_IEventID FOREIGN KEY (EventID) REFERENCES Events(EventID),
	CONSTRAINT fk_IAccountID FOREIGN KEY (AccountID) REFERENCES Accounts(AccountID)
);