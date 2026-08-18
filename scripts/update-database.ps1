$repoRoot = Split-Path -Parent $PSScriptRoot

Get-Content "$repoRoot\.env" | ForEach-Object {
    if ($_ -match '^\s*([^#=]+)=(.*)$') {
        [System.Environment]::SetEnvironmentVariable($matches[1].Trim(), $matches[2].Trim())
    }
}

# Скрипт виконується з хоста, а не всередині docker-мережі,
# тому ім'я контейнера з .env тут не резолвиться — підключаємось через опублікований порт.
[System.Environment]::SetEnvironmentVariable('USERS__DB__HOST', 'localhost')

dotnet ef database update --project "$repoRoot/src/Infrastructure" --startup-project "$repoRoot/src/Api"
