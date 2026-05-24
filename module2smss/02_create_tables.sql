USE ProductionDB;
GO

IF OBJECT_ID('dbo.ProductionMaterial', 'U') IS NOT NULL DROP TABLE dbo.ProductionMaterial;
IF OBJECT_ID('dbo.ProductionItem', 'U') IS NOT NULL DROP TABLE dbo.ProductionItem;
IF OBJECT_ID('dbo.Production', 'U') IS NOT NULL DROP TABLE dbo.Production;
IF OBJECT_ID('dbo.CustomerOrderItem', 'U') IS NOT NULL DROP TABLE dbo.CustomerOrderItem;
IF OBJECT_ID('dbo.CustomerOrder', 'U') IS NOT NULL DROP TABLE dbo.CustomerOrder;
IF OBJECT_ID('dbo.SpecificationMaterial', 'U') IS NOT NULL DROP TABLE dbo.SpecificationMaterial;
IF OBJECT_ID('dbo.Specification', 'U') IS NOT NULL DROP TABLE dbo.Specification;
IF OBJECT_ID('dbo.ProductPrice', 'U') IS NOT NULL DROP TABLE dbo.ProductPrice;
IF OBJECT_ID('dbo.MaterialPrice', 'U') IS NOT NULL DROP TABLE dbo.MaterialPrice;
IF OBJECT_ID('dbo.Material', 'U') IS NOT NULL DROP TABLE dbo.Material;
IF OBJECT_ID('dbo.Product', 'U') IS NOT NULL DROP TABLE dbo.Product;
IF OBJECT_ID('dbo.CounterpartyRole', 'U') IS NOT NULL DROP TABLE dbo.CounterpartyRole;
IF OBJECT_ID('dbo.CounterpartyRoleType', 'U') IS NOT NULL DROP TABLE dbo.CounterpartyRoleType;
IF OBJECT_ID('dbo.Counterparty', 'U') IS NOT NULL DROP TABLE dbo.Counterparty;
GO

CREATE TABLE dbo.Counterparty (
    counterparty_id VARCHAR(9) NOT NULL,
    name NVARCHAR(200) NOT NULL,
    inn VARCHAR(20) NULL,
    address NVARCHAR(300) NULL,
    phone VARCHAR(20) NULL,
    CONSTRAINT PK_Counterparty PRIMARY KEY (counterparty_id)
);
GO

CREATE TABLE dbo.CounterpartyRoleType (
    role_type_id INT IDENTITY(1,1) NOT NULL,
    role_name NVARCHAR(50) NOT NULL,
    CONSTRAINT PK_CounterpartyRoleType PRIMARY KEY (role_type_id),
    CONSTRAINT UQ_CounterpartyRoleType_role_name UNIQUE (role_name)
);
GO

CREATE TABLE dbo.CounterpartyRole (
    counterparty_role_id INT IDENTITY(1,1) NOT NULL,
    counterparty_id VARCHAR(9) NOT NULL,
    role_type_id INT NOT NULL,
    CONSTRAINT PK_CounterpartyRole PRIMARY KEY (counterparty_role_id),
    CONSTRAINT FK_CounterpartyRole_Counterparty
        FOREIGN KEY (counterparty_id) REFERENCES dbo.Counterparty(counterparty_id),
    CONSTRAINT FK_CounterpartyRole_CounterpartyRoleType
        FOREIGN KEY (role_type_id) REFERENCES dbo.CounterpartyRoleType(role_type_id),
    CONSTRAINT UQ_CounterpartyRole UNIQUE (counterparty_id, role_type_id)
);
GO

CREATE TABLE dbo.Product (
    product_id INT IDENTITY(1,1) NOT NULL,
    product_name NVARCHAR(200) NOT NULL,
    unit NVARCHAR(20) NOT NULL,
    CONSTRAINT PK_Product PRIMARY KEY (product_id),
    CONSTRAINT UQ_Product_product_name UNIQUE (product_name)
);
GO

CREATE TABLE dbo.Material (
    material_id INT IDENTITY(1,1) NOT NULL,
    material_name NVARCHAR(200) NOT NULL,
    unit NVARCHAR(20) NOT NULL,
    CONSTRAINT PK_Material PRIMARY KEY (material_id),
    CONSTRAINT UQ_Material_material_name UNIQUE (material_name)
);
GO

CREATE TABLE dbo.MaterialPrice (
    material_price_id INT IDENTITY(1,1) NOT NULL,
    material_id INT NOT NULL,
    price DECIMAL(18,2) NOT NULL,
    date_from DATE NOT NULL,
    date_to DATE NULL,
    CONSTRAINT PK_MaterialPrice PRIMARY KEY (material_price_id),
    CONSTRAINT FK_MaterialPrice_Material
        FOREIGN KEY (material_id) REFERENCES dbo.Material(material_id),
    CONSTRAINT CHK_MaterialPrice_price CHECK (price >= 0),
    CONSTRAINT CHK_MaterialPrice_dates CHECK (date_to IS NULL OR date_to >= date_from)
);
GO

CREATE TABLE dbo.ProductPrice (
    product_price_id INT IDENTITY(1,1) NOT NULL,
    product_id INT NOT NULL,
    price DECIMAL(18,2) NOT NULL,
    date_from DATE NOT NULL,
    date_to DATE NULL,
    CONSTRAINT PK_ProductPrice PRIMARY KEY (product_price_id),
    CONSTRAINT FK_ProductPrice_Product
        FOREIGN KEY (product_id) REFERENCES dbo.Product(product_id),
    CONSTRAINT CHK_ProductPrice_price CHECK (price >= 0),
    CONSTRAINT CHK_ProductPrice_dates CHECK (date_to IS NULL OR date_to >= date_from)
);
GO

CREATE TABLE dbo.Specification (
    specification_id INT IDENTITY(1,1) NOT NULL,
    specification_name NVARCHAR(200) NOT NULL,
    product_id INT NOT NULL,
    manufacturer_id VARCHAR(9) NOT NULL,
    output_quantity DECIMAL(18,3) NOT NULL,
    is_active BIT NOT NULL CONSTRAINT DF_Specification_is_active DEFAULT (1),
    CONSTRAINT PK_Specification PRIMARY KEY (specification_id),
    CONSTRAINT FK_Specification_Product
        FOREIGN KEY (product_id) REFERENCES dbo.Product(product_id),
    CONSTRAINT FK_Specification_Counterparty
        FOREIGN KEY (manufacturer_id) REFERENCES dbo.Counterparty(counterparty_id),
    CONSTRAINT CHK_Specification_output_quantity CHECK (output_quantity > 0)
);
GO

CREATE TABLE dbo.SpecificationMaterial (
    specification_material_id INT IDENTITY(1,1) NOT NULL,
    specification_id INT NOT NULL,
    material_id INT NOT NULL,
    material_quantity DECIMAL(18,3) NOT NULL,
    CONSTRAINT PK_SpecificationMaterial PRIMARY KEY (specification_material_id),
    CONSTRAINT FK_SpecificationMaterial_Specification
        FOREIGN KEY (specification_id) REFERENCES dbo.Specification(specification_id),
    CONSTRAINT FK_SpecificationMaterial_Material
        FOREIGN KEY (material_id) REFERENCES dbo.Material(material_id),
    CONSTRAINT UQ_SpecificationMaterial UNIQUE (specification_id, material_id),
    CONSTRAINT CHK_SpecificationMaterial_material_quantity CHECK (material_quantity > 0)
);
GO

CREATE TABLE dbo.CustomerOrder (
    order_id INT IDENTITY(1,1) NOT NULL,
    order_number NVARCHAR(50) NOT NULL,
    order_date DATE NOT NULL,
    customer_id VARCHAR(9) NOT NULL,
    seller_id VARCHAR(9) NOT NULL,
    CONSTRAINT PK_CustomerOrder PRIMARY KEY (order_id),
    CONSTRAINT UQ_CustomerOrder_order_number UNIQUE (order_number),
    CONSTRAINT FK_CustomerOrder_Customer
        FOREIGN KEY (customer_id) REFERENCES dbo.Counterparty(counterparty_id),
    CONSTRAINT FK_CustomerOrder_Seller
        FOREIGN KEY (seller_id) REFERENCES dbo.Counterparty(counterparty_id)
);
GO

CREATE TABLE dbo.CustomerOrderItem (
    order_item_id INT IDENTITY(1,1) NOT NULL,
    order_id INT NOT NULL,
    product_id INT NOT NULL,
    quantity DECIMAL(18,3) NOT NULL,
    sale_price DECIMAL(18,2) NOT NULL,
    line_amount AS (quantity * sale_price),
    CONSTRAINT PK_CustomerOrderItem PRIMARY KEY (order_item_id),
    CONSTRAINT FK_CustomerOrderItem_CustomerOrder
        FOREIGN KEY (order_id) REFERENCES dbo.CustomerOrder(order_id),
    CONSTRAINT FK_CustomerOrderItem_Product
        FOREIGN KEY (product_id) REFERENCES dbo.Product(product_id),
    CONSTRAINT CHK_CustomerOrderItem_quantity CHECK (quantity > 0),
    CONSTRAINT CHK_CustomerOrderItem_sale_price CHECK (sale_price >= 0)
);
GO

CREATE TABLE dbo.Production (
    production_id INT IDENTITY(1,1) NOT NULL,
    production_number NVARCHAR(50) NOT NULL,
    production_date DATE NOT NULL,
    specification_id INT NOT NULL,
    manufacturer_id VARCHAR(9) NOT NULL,
    CONSTRAINT PK_Production PRIMARY KEY (production_id),
    CONSTRAINT UQ_Production_production_number UNIQUE (production_number),
    CONSTRAINT FK_Production_Specification
        FOREIGN KEY (specification_id) REFERENCES dbo.Specification(specification_id),
    CONSTRAINT FK_Production_Counterparty
        FOREIGN KEY (manufacturer_id) REFERENCES dbo.Counterparty(counterparty_id)
);
GO

CREATE TABLE dbo.ProductionItem (
    production_item_id INT IDENTITY(1,1) NOT NULL,
    production_id INT NOT NULL,
    product_id INT NOT NULL,
    quantity DECIMAL(18,3) NOT NULL,
    CONSTRAINT PK_ProductionItem PRIMARY KEY (production_item_id),
    CONSTRAINT FK_ProductionItem_Production
        FOREIGN KEY (production_id) REFERENCES dbo.Production(production_id),
    CONSTRAINT FK_ProductionItem_Product
        FOREIGN KEY (product_id) REFERENCES dbo.Product(product_id),
    CONSTRAINT CHK_ProductionItem_quantity CHECK (quantity > 0)
);
GO

CREATE TABLE dbo.ProductionMaterial (
    production_material_id INT IDENTITY(1,1) NOT NULL,
    production_id INT NOT NULL,
    material_id INT NOT NULL,
    quantity DECIMAL(18,3) NOT NULL,
    CONSTRAINT PK_ProductionMaterial PRIMARY KEY (production_material_id),
    CONSTRAINT FK_ProductionMaterial_Production
        FOREIGN KEY (production_id) REFERENCES dbo.Production(production_id),
    CONSTRAINT FK_ProductionMaterial_Material
        FOREIGN KEY (material_id) REFERENCES dbo.Material(material_id),
    CONSTRAINT CHK_ProductionMaterial_quantity CHECK (quantity > 0)
);
GO

INSERT INTO dbo.CounterpartyRoleType (role_name)
VALUES (N'Продавец'), (N'Покупатель');
GO
