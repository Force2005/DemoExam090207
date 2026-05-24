USE `demo_090207_m1_m2`;

-- =====================================================
-- Пример данных для проверки модуля 3
-- =====================================================

-- 1. Материалы

INSERT INTO `materials`
  (`material_id`, `material_name`, `unit`)
VALUES
  (1, 'Дерево', 'м2'),
  (2, 'Краска', 'л'),
  (3, 'Металл', 'кг')
ON DUPLICATE KEY UPDATE
  `material_name` = VALUES(`material_name`),
  `unit` = VALUES(`unit`);

-- 2. Цены материалов

INSERT INTO `material_prices`
  (`material_price_id`, `material_id`, `price`, `date_from`, `date_to`)
VALUES
  (1, 1, 500.00, '2026-01-01', NULL), -- дерево 500 за м2
  (2, 2, 200.00, '2026-01-01', NULL), -- краска 200 за л
  (3, 3, 100.00, '2026-01-01', NULL)  -- металл 100 за кг
ON DUPLICATE KEY UPDATE
  `price` = VALUES(`price`),
  `date_from` = VALUES(`date_from`),
  `date_to` = VALUES(`date_to`);

-- 3. Продукция

INSERT INTO `products`
  (`product_id`, `product_name`, `unit`)
VALUES
  (1, 'Стол деревянный', 'шт'),
  (2, 'Стул деревянный', 'шт')
ON DUPLICATE KEY UPDATE
  `product_name` = VALUES(`product_name`),
  `unit` = VALUES(`unit`);

-- 4. Спецификации продукции

INSERT INTO `specifications`
  (`specification_id`, `specification_name`, `product_id`, `manufacturer_id`, `output_quantity`, `is_active`)
VALUES
  (1, 'Спецификация стола деревянного', 1, 1, 1.000, 1),
  (2, 'Спецификация стула деревянного', 2, 1, 1.000, 1)
ON DUPLICATE KEY UPDATE
  `specification_name` = VALUES(`specification_name`),
  `product_id` = VALUES(`product_id`),
  `manufacturer_id` = VALUES(`manufacturer_id`),
  `output_quantity` = VALUES(`output_quantity`),
  `is_active` = VALUES(`is_active`);

-- 5. Нормы расхода материалов по спецификациям

INSERT INTO `specification_materials`
  (`specification_material_id`, `specification_id`, `material_id`, `material_quantity`)
VALUES
  -- 1 стол = 2 м2 дерева + 0.5 л краски + 3 кг металла
  -- Себестоимость материалов стола:
  -- 2 * 500 + 0.5 * 200 + 3 * 100 = 1400
  (1, 1, 1, 2.000),
  (2, 1, 2, 0.500),
  (3, 1, 3, 3.000),

  -- 1 стул = 1 м2 дерева + 0.25 л краски + 1 кг металла
  -- Себестоимость материалов стула:
  -- 1 * 500 + 0.25 * 200 + 1 * 100 = 650
  (4, 2, 1, 1.000),
  (5, 2, 2, 0.250),
  (6, 2, 3, 1.000)
ON DUPLICATE KEY UPDATE
  `specification_id` = VALUES(`specification_id`),
  `material_id` = VALUES(`material_id`),
  `material_quantity` = VALUES(`material_quantity`);

-- 6. Заказ покупателя

INSERT INTO `customer_orders`
  (`order_id`, `order_number`, `order_date`, `customer_id`, `seller_id`)
VALUES
  (1, 'ORD-001', '2026-02-01', 3, 1)
ON DUPLICATE KEY UPDATE
  `order_number` = VALUES(`order_number`),
  `order_date` = VALUES(`order_date`),
  `customer_id` = VALUES(`customer_id`),
  `seller_id` = VALUES(`seller_id`);

-- 7. Состав заказа

INSERT INTO `customer_order_items`
  (`order_item_id`, `order_id`, `product_id`, `quantity`, `sale_price`)
VALUES
  -- заказано 2 стола
  -- 2 * 1400 = 2800
  (1, 1, 1, 2.000, 2500.00),

  -- заказано 4 стула
  -- 4 * 650 = 2600
  (2, 1, 2, 4.000, 1200.00)
ON DUPLICATE KEY UPDATE
  `order_id` = VALUES(`order_id`),
  `product_id` = VALUES(`product_id`),
  `quantity` = VALUES(`quantity`),
  `sale_price` = VALUES(`sale_price`);
