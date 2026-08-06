# users

## Налаштування середовища

1. Скопіювати `.env.example` у `.env` і за потреби змінити значення.
2. Підняти БД (та pgAdmin) через docker-compose:

   ```powershell
   docker-compose up -d
   ```

## Встановлення залежностей (goose, sqlc)

```powershell
go install github.com/pressly/goose/v3/cmd/goose@latest
go install github.com/sqlc-dev/sqlc/cmd/sqlc@latest
```

Переконайтесь, що `$env:GOPATH\bin` (зазвичай `%USERPROFILE%\go\bin`) додано до `PATH`, щоб команди `goose` та `sqlc` були доступні глобально.

## Створення нової міграції

```powershell
goose -dir src/infrastructure/database/migrations create <migration_name> sql
```

Це створить файл на кшталт `20260804120000_<migration_name>.sql` у `src/infrastructure/database/migrations`.

## Застосування міграцій

Підключення до БД береться з `.env` через `migrate.ps1`:

```powershell
.\migrate.ps1 up      # застосувати всі нові міграції
.\migrate.ps1 down    # відкатити останню міграцію
.\migrate.ps1 status  # перевірити статус міграцій
```

## Генерація коду з SQL (sqlc)

```powershell
cd src
sqlc generate
```
