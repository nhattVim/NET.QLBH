-- dotnet ef dbcontext scaffold "Server=172.28.182.33;Database=QLBH;User Id=sa;Password=nhattVim2*;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models -c QLBHContext --force

IF NOT EXISTS (SELECT name
FROM sys.databases
WHERE name = 'QLBH')
BEGIN
    CREATE DATABASE QLBH;
END
GO

USE QLBH;
GO

-- Bảng danh mục sản phẩm
CREATE TABLE Category
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(255) NOT NULL UNIQUE,
    description NVARCHAR(MAX)
);

-- Bảng sản phẩm
CREATE TABLE Product
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(255) NOT NULL,
    description NVARCHAR(MAX),
    images NVARCHAR(MAX),
    price DECIMAL(18,2) NOT NULL CHECK(price >= 0),
    stock INT NOT NULL CHECK(stock >= 0),
    category_id INT,
    FOREIGN KEY (category_id) REFERENCES Category(id) ON DELETE SET NULL
);

-- Bảng người dùng
CREATE TABLE [User]
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    username NVARCHAR(255) NOT NULL UNIQUE,
    email NVARCHAR(255) NOT NULL UNIQUE,
    password NVARCHAR(255) NOT NULL,
    role NVARCHAR(50) CHECK(role IN ('admin', 'customer')) DEFAULT 'customer'
);
