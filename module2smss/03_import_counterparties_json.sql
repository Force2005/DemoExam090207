USE ProductionDB;
GO

EXEC sp_configure 'show advanced options', 1;
RECONFIGURE;
EXEC sp_configure 'Ad Hoc Distributed Queries', 1;
RECONFIGURE;
GO

DECLARE @json NVARCHAR(MAX);

SELECT @json = BulkColumn
FROM OPENROWSET(
    BULK 'C:\\Exam\\Заказчики.json',
    SINGLE_CLOB
) AS j;

INSERT INTO dbo.Counterparty (counterparty_id, name, inn, address, phone)
SELECT
    src.id,
    src.name,
    NULLIF(src.inn, ''),
    src.addres,
    src.phone
FROM OPENJSON(@json)
WITH (
    id VARCHAR(9) '$.id',
    name NVARCHAR(200) '$.name',
    inn VARCHAR(20) '$.inn',
    addres NVARCHAR(300) '$.addres',
    phone VARCHAR(20) '$.phone',
    salesman BIT '$.salesman',
    buyer BIT '$.buyer'
) AS src
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo.Counterparty c
    WHERE c.counterparty_id = src.id
);

INSERT INTO dbo.CounterpartyRole (counterparty_id, role_type_id)
SELECT src.id, 1
FROM OPENJSON(@json)
WITH (
    id VARCHAR(9) '$.id',
    salesman BIT '$.salesman'
) AS src
WHERE src.salesman = 1
  AND NOT EXISTS (
      SELECT 1
      FROM dbo.CounterpartyRole cr
      WHERE cr.counterparty_id = src.id
        AND cr.role_type_id = 1
  );

INSERT INTO dbo.CounterpartyRole (counterparty_id, role_type_id)
SELECT src.id, 2
FROM OPENJSON(@json)
WITH (
    id VARCHAR(9) '$.id',
    buyer BIT '$.buyer'
) AS src
WHERE src.buyer = 1
  AND NOT EXISTS (
      SELECT 1
      FROM dbo.CounterpartyRole cr
      WHERE cr.counterparty_id = src.id
        AND cr.role_type_id = 2
  );
GO
