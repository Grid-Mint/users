# users

## Структура

Чиста архітектура, `Users.slnx` — у корені, проєкти — у `src/`:

- `src/Domain` — сутності, енуми, помилки, інтерфейси репозиторіїв
- `src/Application` — use case'и та бізнес-логіка
- `src/Infrastructure` — доступ до БД, реалізації репозиторіїв
- `src/Api` — ASP.NET Core Web API (точка входу)

## Налаштування середовища

1. Скопіювати `.env.example` у `.env` і за потреби змінити значення.
2. Підняти БД (та pgAdmin) через docker-compose:

   ```powershell
   docker-compose up -d
   ```

## Збірка та запуск

```powershell
dotnet build
dotnet run --project src/Api
```

## Тести

```powershell
dotnet test
```
