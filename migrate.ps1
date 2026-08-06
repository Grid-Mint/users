param([string]$Command = "up")

$ErrorActionPreference = "Stop"

$vars = @{}
Get-Content (Join-Path $PSScriptRoot ".env") | ForEach-Object {
    $line = $_.Trim()
    if ($line -eq "" -or $line.StartsWith("#")) { return }
    $kv = $line -split "=", 2
    $vars[$kv[0].Trim()] = $kv[1].Trim()
}

$env:GOOSE_DRIVER = "postgres"
$env:GOOSE_MIGRATION_DIR = Join-Path $PSScriptRoot "src\infrastructure\database\migrations"
$env:GOOSE_DBSTRING = "postgres://$($vars['DB__USER']):$($vars['DB__PASSWORD'])@localhost:$($vars['DB__PORT'])/$($vars['DB__NAME'])?sslmode=disable"

goose $Command