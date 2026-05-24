# DE.Forms Integration Tests

The tests are opt-in because they create and drop temporary databases.

Configure providers in `testsettings.json`:

```json
{
  "MySql": {
    "Enabled": true,
    "Server": "localhost",
    "Port": 3306,
    "UserId": "root",
    "Password": "your-password",
    "ConnectionTimeout": 15
  },
  "SqlServer": {
    "Enabled": true,
    "Server": "(localdb)\\MSSQLLocalDB",
    "TrustedConnection": true
  }
}
```

Then run:

```powershell
dotnet test .\DE.Forms.IntegrationTests\DE.Forms.IntegrationTests.csproj
```

Environment variables still override the JSON settings, for example `DE_INTEGRATION_MYSQL=1` or `DE_TEST_MYSQL_PASSWORD=...`.

Each enabled test creates a database with the `de_forms_it_` prefix and drops only that database.
