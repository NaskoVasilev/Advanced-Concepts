-- https://www.mssqltips.com/sqlservertip/5211/sql-server-temporal-tables-vs-change-data-capture-vs-change-tracking-part-1/

CREATE DATABASE TrackChange
GO

USE TrackChange
GO   

CREATE TABLE Customer ( 
CustomerId INT IDENTITY (1,1) 
,FirstName VARCHAR(30) 
,LastName VARCHAR(30) NOT NULL 
,Amount_purchased DECIMAL 
) 
GO 

ALTER TABLE dbo.Customer ADD CONSTRAINT PK_Customer PRIMARY KEY (CustomerId, LastName) 
GO 

INSERT INTO dbo.Customer ( FirstName, LastName, Amount_Purchased)
VALUES ( 'Frank', 'Sinatra',20000.00), ( 'Shawn', 'McGuire',30000.00), ( 'Amy', 'Carlson',40000.00)
GO

SELECT * FROM dbo.Customer

-- Now enable change Tracking at Database Level
ALTER DATABASE TrackChange
SET CHANGE_TRACKING = ON (CHANGE_RETENTION = 2 DAYS, AUTO_CLEANUP = ON)

-- Then enable change Tracking at Table Level
ALTER TABLE dbo.Customer
ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = ON)

-- Verify the status of the change tracking
-- You will find that there is no version history yet.
SELECT CHANGE_TRACKING_CURRENT_VERSION () AS CT_Version

SELECT * FROM CHANGETABLE (CHANGES Customer, 0) as CT ORDER BY SYS_CHANGE_VERSION

SELECT c.CustomerId, c.LastName ,  ct.SYS_CHANGE_VERSION, ct.SYS_CHANGE_CONTEXT
FROM Customer AS c
CROSS APPLY CHANGETABLE (VERSION Customer, (customerId, Lastname), (c.CustomerId, c.LastName)) AS ct;

-- Now make some changes in the table
-- insert a row
INSERT INTO Customer(FirstName, LastName, Amount_purchased)
VALUES('Ameena', 'Lalani', 50000)
GO

-- delete a row
DELETE FROM dbo.Customer 
WHERE CustomerId = 2
GO

-- update a row
UPDATE Customer
SET  Lastname = 'Clarkson' WHERE CustomerId = 3
GO

-- Let us query to see what it reports
SELECT CHANGE_TRACKING_CURRENT_VERSION () AS CT_Version

SELECT * FROM CHANGETABLE (CHANGES Customer, 0) as CT ORDER BY SYS_CHANGE_VERSION

SELECT c.CustomerId, c.LastName ,  ct.SYS_CHANGE_VERSION, ct.SYS_CHANGE_CONTEXT
FROM Customer AS c
CROSS APPLY CHANGETABLE (VERSION Customer, (customerId, Lastname), (c.CustomerId, c.LastName)) AS ct; 

-- Update the above row one more time
UPDATE Customer
SET  Lastname = 'Blacksmith' WHERE CustomerId = 3
GO

-- Let INSERT few more rows
INSERT INTO Customer(FirstName, LastName, Amount_purchased)
VALUES('Sponge', 'Bob', 5000)
GO

INSERT INTO Customer(FirstName, LastName, Amount_purchased)
VALUES('Donald', 'Duck', 6000)
GO

-- Let us query to see what it reports now
SELECT CHANGE_TRACKING_CURRENT_VERSION () as CT_Version

SELECT * FROM CHANGETABLE (CHANGES Customer, 0) as CT ORDER BY SYS_CHANGE_VERSION

SELECT c.CustomerId, c.LastName ,  ct.SYS_CHANGE_VERSION, ct.SYS_CHANGE_CONTEXT
FROM Customer AS c
CROSS APPLY CHANGETABLE (VERSION Customer, (customerId, Lastname), (c.CustomerId, c.LastName)) AS ct;

-- Let us make one more update
UPDATE Customer
SET  Lastname = 'Cool' WHERE CustomerId = 6
GO

SELECT CHANGE_TRACKING_CURRENT_VERSION () as CT_Version

SELECT * FROM CHANGETABLE (CHANGES Customer,0) as CT ORDER BY SYS_CHANGE_VERSION

SELECT c.CustomerId, c.LastName ,  ct.SYS_CHANGE_VERSION, ct.SYS_CHANGE_CONTEXT
FROM Customer AS c
CROSS APPLY CHANGETABLE (VERSION Customer, (customerId, Lastname), (c.CustomerId, c.LastName)) AS ct;
