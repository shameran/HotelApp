-- Skapa tabell för kunder
CREATE TABLE Customers (
    Id INT PRIMARY KEY IDENTITY(1,1),     
    Name NVARCHAR(100) NOT NULL            
);

-- Skapa tabell för rum
CREATE TABLE Rooms (
    Id INT PRIMARY KEY IDENTITY(1,1),     
    RoomNumber NVARCHAR(20) NOT NULL,     
    RoomType NVARCHAR(50) NOT NULL,      
    Capacity INT NOT NULL                 
);

-- Skapa tabell för bokningar
CREATE TABLE Bookings (
    Id INT PRIMARY KEY IDENTITY(1,1),     
    CustomerId INT,                        
    RoomId INT,                            
    CheckInDate DATETIME NOT NULL,         
    CheckOutDate DATETIME NOT NULL,        
    BookingDate DATETIME NOT NULL DEFAULT GETDATE(), 
    FOREIGN KEY (CustomerId) REFERENCES Customers(Id),  
    FOREIGN KEY (RoomId) REFERENCES Rooms(Id)          
);
