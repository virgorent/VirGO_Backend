param(
  [int]$Port = 27018
)

$ErrorActionPreference = 'Stop'
$root = Join-Path $env:TEMP 'virgo-mongo-dev'
$pidPath = Join-Path $root 'pid.txt'

$mongosh = Get-Command mongosh -ErrorAction SilentlyContinue
if ($mongosh) {
  $mongoshPath = $mongosh.Source
} else {
  $mongoshPath = 'C:\Program Files\MongoDB\Server\8.0\bin\mongosh.exe'
}

if (Test-Path -LiteralPath $mongoshPath) {
  & $mongoshPath --quiet --host 127.0.0.1 --port $Port --eval "db.getSiblingDB('admin').shutdownServer()" 2>$null | Out-Null
  Start-Sleep -Seconds 2
}

if (Test-Path -LiteralPath $pidPath) {
  $processId = Get-Content -LiteralPath $pidPath
  $process = Get-Process -Id $processId -ErrorAction SilentlyContinue
  if ($process) {
    Stop-Process -Id $processId -Force
  }
  Remove-Item -LiteralPath $pidPath -Force -ErrorAction SilentlyContinue
}

Write-Output "MongoDB local detenido."
