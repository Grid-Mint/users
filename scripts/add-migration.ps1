param(
    [Parameter(Mandatory = $true)]
    [string]$Name
)

dotnet ef migrations add $Name --project ../src/Infrastructure --startup-project ../src/Api --output-dir Database/Migrations
