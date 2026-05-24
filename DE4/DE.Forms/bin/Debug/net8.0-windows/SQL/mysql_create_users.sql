CREATE DATABASE IF NOT EXISTS `demo_090207_m1_m2`
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

USE `demo_090207_m1_m2`;

CREATE TABLE IF NOT EXISTS `users`
(
    `user_id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
    `full_name` VARCHAR(200) NOT NULL,
    `login` VARCHAR(100) NOT NULL,
    `password_hash` VARCHAR(500) NOT NULL,
    `role_name` VARCHAR(50) NOT NULL,
    `failed_attempts` INT NOT NULL DEFAULT 0,
    `is_blocked` TINYINT(1) NOT NULL DEFAULT 0,
    `created_at` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (`user_id`),
    UNIQUE KEY `uk_users_login` (`login`)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_unicode_ci
COMMENT = 'Пользователи приложения';
