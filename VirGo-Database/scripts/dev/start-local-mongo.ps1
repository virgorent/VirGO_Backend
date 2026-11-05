param(
  [string]$Binary = $(if ($env:MONGOD_BINARY) { $env:MONGOD_BINARY } else { 'C:\Program Files\MongoDB\Server\8.0\bin\mongod.exe' }),
  [int]$Port = 27018,
  [string]$ReplicaSet = 'rs0'
)

$ErrorActionPreference = 'Stop'
$root = Join-Path $env:TEMP 'virgo-mongo-dev'
$dbPath = Join-Path $root 'data'
$logPath = Join-Path $root 'mongod.log'
$pidPath = Join-Path $root 'pid.txt'

if (-not (Test-Path -LiteralPath $Binary)) {
  throw "No se encontro mongod en $Binary. Defina MONGOD_BINARY."
}

$listener = Get-NetTCPConnection -LocalPort $Port -State Listen -ErrorAction SilentlyContinue
if ($listener) {
  Write-Output "MongoDB ya esta escuchando en el puerto $Port."
  Write-Output "MONGODB_TEST_URI=mongodb://127.0.0.1:$Port/?replicaSet=$ReplicaSet"
  exit 0
}

New-Item -ItemType Directory -Force -Path $dbPath | Out-Null

$process = Start-Process -FilePath $Binary -ArgumentList @(
  '--replSet', $ReplicaSet,
  '--bind_ip', '127.0.0.1',
  '--port', "$Port",
  '--dbpath', $dbPath,
  '--logpath', $logPath
) -PassThru -WindowStyle Hidden

Set-Content -LiteralPath $pidPath -Value $process.Id

$deadline = (Get-Date).AddSeconds(30)
while ((Get-Date) -lt $deadline) {
  $listener = Get-NetTCPConnection -LocalPort $Port -State Listen -ErrorAction SilentlyContinue
  if ($listener) { break }
  Start-Sleep -Milliseconds 500
}

if (-not $listener) {
  throw "MongoDB no inicio. Revise $logPath"
}

$mongosh = Get-Command mongosh -ErrorAction SilentlyContinue
if ($mongosh) {
  $mongoshPath = $mongosh.Source
} else {
  $mongoshPath = 'C:\Program Files\MongoDB\Server\8.0\bin\mongosh.exe'
}

if (-not (Test-Path -LiteralPath $mongoshPath)) {
  throw 'No se encontro mongosh. Instale MongoDB Shell o inicie el replica set manualmente.'
}

& $mongoshPath --quiet --host 127.0.0.1 --port $Port --eval "try { rs.status().ok } catch (e) { rs.initiate({_id:'$ReplicaSet',members:[{_id:0,host:'127.0.0.1:$Port'}]}).ok }" | Out-Null
Start-Sleep -Seconds 2

Write-Output "MongoDB local iniciado en el puerto $Port."
Write-Output "MONGODB_TEST_URI=mongodb://127.0.0.1:$Port/?replicaSet=$ReplicaSet"
