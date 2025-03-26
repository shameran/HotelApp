SELECT Id, Name, Email, City
FROM Customers
ORDER BY Name ASC;


SELECT * 
FROM Bookings
WHERE CheckInDate >= '2025-01-01'
ORDER BY CheckInDate ASC;

SELECT * 
FROM Rooms;
