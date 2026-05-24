USE `demo_090207_m1_m2`;

WITH active_specifications AS (
    SELECT
        s.specification_id,
        s.product_id,
        s.output_quantity,
        ROW_NUMBER() OVER (
            PARTITION BY s.product_id
            ORDER BY s.is_active DESC, s.specification_id DESC
        ) AS rn
    FROM `specifications` s
    WHERE s.is_active = 1
),

product_material_cost AS (
    SELECT
        s.product_id,
        s.specification_id,
        CAST(
            SUM(
                sm.material_quantity *
                (
                    SELECT mp.price
                    FROM `material_prices` mp
                    WHERE mp.material_id = sm.material_id
                    ORDER BY mp.date_from DESC, mp.material_price_id DESC
                    LIMIT 1
                )
            ) / NULLIF(s.output_quantity, 0)
            AS DECIMAL(15,2)
        ) AS unit_material_cost
    FROM active_specifications s
    INNER JOIN `specification_materials` sm
        ON sm.specification_id = s.specification_id
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
    CAST(
        SUM(coi.quantity * pmc.unit_material_cost)
        AS DECIMAL(15,2)
    ) AS total_order_cost
FROM `customer_orders` co
INNER JOIN `counterparties` c
    ON c.counterparty_id = co.customer_id
INNER JOIN `customer_order_items` coi
    ON coi.order_id = co.order_id
INNER JOIN product_material_cost pmc
    ON pmc.product_id = coi.product_id
GROUP BY
    co.order_id,
    co.order_number,
    co.order_date,
    c.name
ORDER BY
    co.order_id;
