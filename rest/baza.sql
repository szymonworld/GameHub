CREATE TABLE Accounts
(
	AccountID int,
	LastName varchar(255),
	FirstName varchar(255),
	Login varchar(255),
	Password varchar(255),
	PRIMARY KEY (AccountID)
);

CREATE TABLE Games
(
	GameID int,
	GameName varchar(255),
	Description varchar(255),
	ImageURL varchar(255),
	PRIMARY KEY (GameID)
);

CREATE TABLE Events
(
	EventID int,
	EventName varchar(255),
	GameID int,
	Description varchar(255),
	Date varchar(255),
	PRIMARY KEY (EventID),
	FOREIGN KEY (GameID) REFERENCES Games(GameID)
);

CREATE TABLE EventParticipants
(
	ParticipationID int,
	EventID int,
	AccountID int
	PRIMARY KEY (ParticipationID),
	FOREIGN KEY (EventID) REFERENCES Events(EventID),
	FOREIGN KEY (AccountID) REFERENCES Accounts(AccountID)
);

CREATE TABLE Friends
(
	FriendshipID int,
	AccountID1 int,
	AccountID2 int,
	PRIMARY KEY (FriendshipID),
	FOREIGN KEY (AccountID1) REFERENCES Accounts(AccountID),
	FOREIGN KEY (AccountID2) REFERENCES Accounts(AccountID)
);

CREATE TABLE Invitations
(
	InviteID int,
	AccountID int,
	EventID int,
	PRIMARY KEY (InviteID),
	FOREIGN KEY (EventID) REFERENCES Events(EventID),
	FOREIGN KEY (AccountID) REFERENCES Accounts(AccountID)
);