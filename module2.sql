USE `demo_090207_m1_m2`;

START TRANSACTION;

INSERT INTO `counterparty_role_types`
  (`role_type_id`, `role_name`)
VALUES
  (1, 'Заказчик'),
  (2, 'Продавец')
ON DUPLICATE KEY UPDATE
  `role_name` = VALUES(`role_name`);

INSERT INTO `counterparties`
  (`counterparty_id`, `name`, `inn`, `address`, `phone`)
VALUES
  (1,  'ООО "Поставка"',        '',            'г.Пятигорск',                    '+79198634592'),
  (2,  'ООО "Кинотеатр Квант"', '26320045123', 'г. Железноводск, ул. Мира, 123', '+79884581555'),
  (8,  'ООО "Новый JDTO"',      '26320045111', 'г. Железноводсу',                '+79884581555'),
  (3,  'ООО "Ромашка"',         '4140784214',  'г. Омск, ул. Строителей, 294',   '+79882584546'),
  (9,  'ООО "Ипподром"',        '5874045632',  'г. Уфа, ул. Набережная,  37',    '+79627486389'),
  (10, 'ООО "Ассоль"',          '2629011278',  'г. Калуга, ул. Пушкина, 94',     '+79184572398')
ON DUPLICATE KEY UPDATE
  `name` = VALUES(`name`),
  `inn` = VALUES(`inn`),
  `address` = VALUES(`address`),
  `phone` = VALUES(`phone`);

INSERT INTO `counterparty_roles`
  (`counterparty_id`, `role_type_id`)
VALUES
  (1, 2),
  (1, 1),
  (2, 2),
  (8, 2),
  (3, 1),
  (9, 2),
  (9, 1),
  (10, 1)
ON DUPLICATE KEY UPDATE
  `counterparty_id` = VALUES(`counterparty_id`),
  `role_type_id` = VALUES(`role_type_id`);

COMMIT;
