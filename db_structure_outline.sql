CREATE TABLE IF NOT EXISTS LOGS (
    log_id SERIAL PRIMARY KEY,
    date_logged DATE NOT NULL,
    level VARCHAR(15) NOT NULL,
    message VARCHAR(256) NOT NULL
);

CREATE TABLE IF NOT EXISTS Users (
    ID SERIAL PRIMARY KEY,
    ums_userid VARCHAR(450) NOT NULL,
    EMAIL VARCHAR(256) NOT NULL,
    FirstName VARCHAR(256) NOT NULL,
    LastName VARCHAR(256) NOT NULL,
    DateLastLogin TIMESTAMP,
    DateLastLogout TIMESTAMP,
    DateCreated TIMESTAMP,
    DateRetired TIMESTAMP
);

CREATE TABLE IF NOT EXISTS Account (
    AccountId SERIAL PRIMARY KEY,
    UserId REFERENCES Users(ID),
    AccountType VARCHAR(15),
);

CREATE TABLE IF NOT EXISTS Transactions (
    TransactionsId SERIAL PRIMARY KEY,
    AccountId REFERENCES Account(AccountId),
    Details VARCHAR(15),
    PostingDate DATE,
    Description VARCHAR(256),
    Amount DECIMAL,
    TYPE VARCHAR(64),
    Balance DECIMAL,
    CheckOrSlip INT
);





