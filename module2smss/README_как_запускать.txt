Порядок запуска файлов в SQL Server Management Studio (SSMS):

1. Открой 01_create_database.sql и выполни его.
2. Открой 02_create_tables.sql и выполни его.
3. Скопируй файл Заказчики.json в папку:
   C:\Exam\Заказчики.json

   Если путь другой, открой файл 03_import_counterparties_json.sql
   и замени путь в строке BULK.

4. Открой 03_import_counterparties_json.sql и выполни его.
5. Открой 04_check_import.sql и выполни его.

Результат:
- создана база ProductionDB;
- созданы таблицы по ER-диаграмме;
- заданы PK, FK, UNIQUE, CHECK;
- импортированы данные из Заказчики.json;
- импортированы роли контрагентов: Продавец и Покупатель.
