# ConfigurationApp

Uygulama Storoge kısmını MsSql'den almaktadır. Database yapımı aşağıdaki kodlar ile oluşturdum ve Connection String ile uygulama appsettings.json dosyasından almaktadır. Uygulama çalışır durumdadır .Net 8 ile yazdım.
```
CREATE TABLE Configurations (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Type NVARCHAR(50) NOT NULL,
    Value NVARCHAR(255) NOT NULL,
    IsActive BIT NOT NULL,
    ApplicationName NVARCHAR(100) NOT NULL
);
```

```
INSERT INTO Configurations (Name, Type, Value, IsActive, ApplicationName) VALUES 
('SiteName', 'string', 'soty.io', 1, 'SERVICE-A'),
('IsBasketEnabled', 'bool', '1', 1, 'SERVICE-B'),
('MaxItemCount', 'int', '50', 0, 'SERVICE-A');
```
