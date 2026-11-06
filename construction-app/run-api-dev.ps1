# Starts the API for local development with settings from .local\dev.env (git-ignored).
# Copy .local\dev.env.example to .local\dev.env first and fill in your own values.
$ErrorActionPreference = 'Stop'
Set-Location $PSScriptRoot
if (-not (Test-Path .local\dev.env)) { throw 'Create .local\dev.env first (see README).' }
Get-Content .local\dev.env | Where-Object { $_ -match '^[A-Za-z_]+=' } | ForEach-Object {
    $k, $v = $_ -split '=', 2
    Set-Item -Path "env:$k" -Value $v
}
$env:ASPNETCORE_ENVIRONMENT = 'Development'
$env:ASPNETCORE_URLS = 'http://localhost:5160'
& dotnet run --project src/Construction.Api --no-launch-profile
