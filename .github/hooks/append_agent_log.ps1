param(
    [Parameter(Mandatory=$true)] [string]$Role,
    [Parameter(Mandatory=$true)] [string]$Summary
)

$logPath = "c:\Users\Franko\source\repos\Lab1.RezervacijeProstora\.github\hooks\agent_log"
$timestamp = (Get-Date).ToString("o")
"$timestamp - $Role - $Summary" | Out-File -FilePath $logPath -Encoding utf8 -Append
