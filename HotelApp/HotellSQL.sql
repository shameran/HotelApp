
CREATE TABLE Customers (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL
);


CREATE TABLE Rooms (
    Id INT PRIMARY KEY IDENTITY(1,1),
    RoomNumber NVARCHAR(50) NOT NULL,
    RoomType NVARCHAR(50) NOT NULL,
    Capacity INT NOT NULL
);


CREATE TABLE Bookings (
    Id INT PRIMARY KEY IDENTITY(1,1),
    CustomerId INT,
    RoomId INT,
    CheckInDate DATE NOT NULL,
    CheckOutDate DATE NOT NULL,
    BookingDate DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (CustomerId) REFERENCES Customers(Id),
    FOREIGN KEY (RoomId) REFERENCES Rooms(Id)
);




SELECT * FROM Customers;


SELECT * FROM Rooms;


SELECT 
    b.Id AS BookingId, 
    c.Name AS CustomerName, 
    r.RoomNumber, 
    r.RoomType, 
    b.CheckInDate, 
    b.CheckOutDate
FROM Bookings b
JOIN Customers c ON b.CustomerId = c.Id
JOIN Rooms r ON b.RoomId = r.Id;

-- Hämta bokningar för en viss kund
SELECT 
    b.Id AS BookingId, 
    c.Name AS CustomerName, 
    r.RoomNumber, 
    r.RoomType, 
    b.CheckInDate, 
    b.CheckOutDate
FROM Bookings b
JOIN Customers c ON b.CustomerId = c.Id
JOIN Rooms r ON b.RoomId = r.Id
WHERE c.Id = @customerId;  -- Ersätter @customerId med ett specifikt kund-ID


SELECT 
    b.Id AS BookingId, 
    c.Name AS CustomerName, 
    r.RoomNumber, 
    r.RoomType, 
    b.CheckInDate, 
    b.CheckOutDate
FROM Bookings b
JOIN Customers c ON b.CustomerId = c.Id
JOIN Rooms r ON b.RoomId = r.Id
WHERE r.Id = @roomId;  -- Ersätter @roomId med ett specifikt rums-ID

-- Kontrollera om ett rum är tillgängligt för en viss period
SELECT 
    COUNT(*) AS BookingsCount
FROM Bookings b
WHERE b.RoomId = @roomId
  AND (
      (@checkInDate >= b.CheckInDate AND @checkInDate < b.CheckOutDate) OR
      (@checkOutDate > b.CheckInDate AND @checkOutDate <= b.CheckOutDate)
  );


