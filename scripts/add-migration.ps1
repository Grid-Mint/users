param(
    [Parameter(Mandatory = $true)]
    [string]$Name
)

$repoRoot = Split-Path -Parent $PSScriptRoot

Get-Content "$repoRoot\.env" | ForEach-Object {
    if ($_ -match '^\s*([^#=]+)=(.*)$') {
        [System.Environment]::SetEnvironmentVariable($matches[1].Trim(), $matches[2].Trim())
    }
}


[System.Environment]::SetEnvironmentVariable('USERS__DB__HOST', 'localhost')

dotnet ef migrations add $Name --project "$repoRoot/src/Infrastructure" --startup-project "$repoRoot/src/Api" --output-dir Database/Migrations
