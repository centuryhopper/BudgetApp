-- Create LOGS table
CREATE TABLE IF NOT EXISTS LOGS (
    log_id SERIAL PRIMARY KEY,
    date_logged DATE NOT NULL,
    level VARCHAR(15) NOT NULL,
    message VARCHAR(256) NOT NULL
);

-- Create Users table
CREATE TABLE IF NOT EXISTS Users (
    ID SERIAL PRIMARY KEY,
    ums_userid VARCHAR(450) NOT NULL UNIQUE,  -- Add UNIQUE constraint to avoid duplicate ums_userid
    EMAIL VARCHAR(256) NOT NULL,
    FirstName VARCHAR(256) NOT NULL,
    LastName VARCHAR(256) NOT NULL,
    DateLastLogin TIMESTAMP,
    DateLastLogout TIMESTAMP,
    DateCreated TIMESTAMP DEFAULT CURRENT_TIMESTAMP,  -- Default to current timestamp for DateCreated
    DateRetired TIMESTAMP
);

-- Create Transactions table
CREATE TABLE IF NOT EXISTS Transactions (
    TransactionsId SERIAL PRIMARY KEY,
    UserId INT REFERENCES Users(ID) ON DELETE CASCADE,  -- Ensure UserId is INT and use ON DELETE CASCADE for cleanup
    Details VARCHAR(15),
    PostingDate DATE,
    Description VARCHAR(256),
    Amount DECIMAL(10, 2),  -- Specify precision for Amount
    TYPE VARCHAR(64),
    Balance DECIMAL(10, 2),  -- Specify precision for Balance
    CheckOrSlip INT
);


