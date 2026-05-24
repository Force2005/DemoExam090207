USE ProductionDB;
GO

SELECT * FROM dbo.Counterparty;
SELECT * FROM dbo.CounterpartyRoleType;
SELECT * FROM dbo.CounterpartyRole;

SELECT 
    c.counterparty_id,
    c.name,
    c.inn,
    c.address,
    c.phone,
    rt.role_name
FROM dbo.Counterparty c
LEFT JOIN dbo.CounterpartyRole cr
    ON c.counterparty_id = cr.counterparty_id
LEFT JOIN dbo.CounterpartyRoleType rt
    ON cr.role_type_id = rt.role_type_id
ORDER BY c.counterparty_id, rt.role_name;
GO
