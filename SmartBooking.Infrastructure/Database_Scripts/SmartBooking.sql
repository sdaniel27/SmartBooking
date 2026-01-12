use [master]
go

IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'SmartBooking')
BEGIN
    -- Create the database
    CREATE DATABASE SmartBooking;
END
ELSE
BEGIN
   DROP DATABASE SmartBooking;
END

Go
use SmartBooking
go


-- Hotels Table.
CREATE TABLE Hotels (
    HotelId INT IDENTITY(1,1),    
    HotelName NVARCHAR(100) NOT NULL,    
    CONSTRAINT PK_Hotels_HotelId PRIMARY KEY (HotelId)
);

CREATE TABLE Rooms (
    RoomId INT IDENTITY(1,1),
    HotelId INT NOT NULL,
    RoomNumber NVARCHAR(4) NOT NULL,
    RoomType INT NOT NULL, -- Single, Double, Deluxe
    Capacity INT NOT NULL 
    CONSTRAINT PK_Rooms_RoomId PRIMARY KEY (RoomId),
    CONSTRAINT FK_Rooms_HotelId FOREIGN KEY (HotelId) REFERENCES Hotels(HotelId)
);

CREATE TABLE Bookings (
    BookingId INT IDENTITY(1,1),
    Reference NVARCHAR(8) NOT NULL,
    RoomId INT NOT NULL,    
    CheckIn DATETIME2 NOT NULL,
    CheckOut DATETIME2 NOT NULL,
	Guests INT NOT NULL,
    CONSTRAINT PK_Bookings_BookingId PRIMARY KEY (BookingId),
    CONSTRAINT FK_Bookings_RoomId FOREIGN KEY (RoomId) REFERENCES Rooms(RoomId)
);