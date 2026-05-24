USE ProductionDB;
GO

/*
Модуль 3. Создание запроса

Назначение:
Запрос вычисляет полную стоимость заказа покупателя с учетом:
1) количества продукции в заказе;
2) стоимости всех материалов, использованных для производства данной продукции,
   с учетом нормы расхода по спецификации.

Логика:
- для каждой строки заказа берется продукция;
- для продукции определяется активная спецификация;
- по спецификации берутся материалы и их норма расхода;
- стоимость материалов считается как:
      material_quantity * актуальная_цена_материала
- себестоимость единицы продукции считается как:
      сумма стоимостей материалов / output_quantity
- полная стоимость строки заказа:
      quantity_в_заказе * себестоимость_единицы
- полная стоимость заказа:
      сумма по всем строкам заказа

Примечание:
Если по материалу есть несколько цен, берется последняя актуальная цена
(по максимальной date_from).
*/

;WITH LastMaterialPrice AS (
    SELECT
        mp.material_id,
        mp.price,
        ROW_NUMBER() OVER (
            PARTITION BY mp.material_id
            ORDER BY mp.date_from DESC, mp.material_price_id DESC
        ) AS rn
    FROM dbo.MaterialPrice mp
),
ActiveSpecification AS (
    SELECT
        s.specification_id,
        s.product_id,
        s.output_quantity,
        ROW_NUMBER() OVER (
            PARTITION BY s.product_id
            ORDER BY s.is_active DESC, s.specification_id DESC
        ) AS rn
    FROM dbo.Specification s
    WHERE s.is_active = 1
),
ProductMaterialCost AS (
    SELECT
        s.product_id,
        s.specification_id,
        CAST(
            SUM(sm.material_quantity * lmp.price) / NULLIF(s.output_quantity, 0)
            AS DECIMAL(18,2)
        ) AS unit_material_cost
    FROM ActiveSpecification s
    INNER JOIN dbo.SpecificationMaterial sm
        ON s.specification_id = sm.specification_id
    INNER JOIN LastMaterialPrice lmp
        ON sm.material_id = lmp.material_id
       AND lmp.rn = 1
    WHERE s.rn = 1
    GROUP BY
        s.product_id,
        s.specification_id,
        s.output_quantity
)

SELECT
    co.order_id,
    co.order_number,
    co.order_date,
    c.name AS customer_name,
    p.product_name,
    coi.quantity,
    pmc.unit_material_cost,
    CAST(coi.quantity * pmc.unit_material_cost AS DECIMAL(18,2)) AS order_item_full_cost
FROM dbo.CustomerOrder co
INNER JOIN dbo.Counterparty c
    ON co.customer_id = c.counterparty_id
INNER JOIN dbo.CustomerOrderItem coi
    ON co.order_id = coi.order_id
INNER JOIN dbo.Product p
    ON coi.product_id = p.product_id
INNER JOIN ProductMaterialCost pmc
    ON p.product_id = pmc.product_id
ORDER BY
    co.order_id,
    p.product_name;
GO

/*
Итог по каждому заказу
*/
;WITH LastMaterialPrice AS (
    SELECT
        mp.material_id,
        mp.price,
        ROW_NUMBER() OVER (
            PARTITION BY mp.material_id
            ORDER BY mp.date_from DESC, mp.material_price_id DESC
        ) AS rn
    FROM dbo.MaterialPrice mp
),
ActiveSpecification AS (
    SELECT
        s.specification_id,
        s.product_id,
        s.output_quantity,
        ROW_NUMBER() OVER (
            PARTITION BY s.product_id
            ORDER BY s.is_active DESC, s.specification_id DESC
        ) AS rn
    FROM dbo.Specification s
    WHERE s.is_active = 1
),
ProductMaterialCost AS (
    SELECT
        s.product_id,
        CAST(
            SUM(sm.material_quantity * lmp.price) / NULLIF(s.output_quantity, 0)
            AS DECIMAL(18,2)
        ) AS unit_material_cost
    FROM ActiveSpecification s
    INNER JOIN dbo.SpecificationMaterial sm
        ON s.specification_id = sm.specification_id
    INNER JOIN LastMaterialPrice lmp
        ON sm.material_id = lmp.material_id
       AND lmp.rn = 1
    WHERE s.rn = 1
    GROUP BY
        s.product_id,
        s.output_quantity
)
SELECT
    co.order_id,
    co.order_number,
    co.order_date,
    c.name AS customer_name,
    CAST(SUM(coi.quantity * pmc.unit_material_cost) AS DECIMAL(18,2)) AS total_order_full_cost
FROM dbo.CustomerOrder co
INNER JOIN dbo.Counterparty c
    ON co.customer_id = c.counterparty_id
INNER JOIN dbo.CustomerOrderItem coi
    ON co.order_id = coi.order_id
INNER JOIN ProductMaterialCost pmc
    ON coi.product_id = pmc.product_id
GROUP BY
    co.order_id,
    co.order_number,
    co.order_date,
    c.name
ORDER BY
    co.order_id;
GO

/*
Вариант с фильтром по номеру заказа
Измени значение переменной @OrderNumber при необходимости.
*/
DECLARE @OrderNumber NVARCHAR(50) = N'ORD-001';

;WITH LastMaterialPrice AS (
    SELECT
        mp.material_id,
        mp.price,
        ROW_NUMBER() OVER (
            PARTITION BY mp.material_id
            ORDER BY mp.date_from DESC, mp.material_price_id DESC
        ) AS rn
    FROM dbo.MaterialPrice mp
),
ActiveSpecification AS (
    SELECT
        s.specification_id,
        s.product_id,
        s.output_quantity,
        ROW_NUMBER() OVER (
            PARTITION BY s.product_id
            ORDER BY s.is_active DESC, s.specification_id DESC
        ) AS rn
    FROM dbo.Specification s
    WHERE s.is_active = 1
),
ProductMaterialCost AS (
    SELECT
        s.product_id,
        CAST(
            SUM(sm.material_quantity * lmp.price) / NULLIF(s.output_quantity, 0)
            AS DECIMAL(18,2)
        ) AS unit_material_cost
    FROM ActiveSpecification s
    INNER JOIN dbo.SpecificationMaterial sm
        ON s.specification_id = sm.specification_id
    INNER JOIN LastMaterialPrice lmp
        ON sm.material_id = lmp.material_id
       AND lmp.rn = 1
    WHERE s.rn = 1
    GROUP BY
        s.product_id,
        s.output_quantity
)
SELECT
    co.order_id,
    co.order_number,
    co.order_date,
    c.name AS customer_name,
    CAST(SUM(coi.quantity * pmc.unit_material_cost) AS DECIMAL(18,2)) AS total_order_full_cost
FROM dbo.CustomerOrder co
INNER JOIN dbo.Counterparty c
    ON co.customer_id = c.counterparty_id
INNER JOIN dbo.CustomerOrderItem coi
    ON co.order_id = coi.order_id
INNER JOIN ProductMaterialCost pmc
    ON coi.product_id = pmc.product_id
WHERE co.order_number = @OrderNumber
GROUP BY
    co.order_id,
    co.order_number,
    co.order_date,
    c.name;
GO
