# Uninstallation script for Obsidian CLI (PowerShell)

$ErrorActionPreference = "Stop"

# Configuration
$InstallDir = "$env:LOCALAPPDATA\Programs\obsidian"

# Colors
$ColorReset = "`e[0m"
$ColorRed = "`e[91m"
$ColorGreen = "`e[92m"
$ColorYellow = "`e[93m"

function Write-ColorOutput($ForegroundColor, $Message) {
    switch ($ForegroundColor) {
        "Red"    { Write-Host $ColorRed$Message$ColorReset }
        "Green"  { Write-Host $ColorGreen$Message$ColorReset }
        "Yellow" { Write-Host $ColorYellow$Message$ColorReset }
        default  { Write-Host $Message }
    }
}

Write-Host ""
Write-ColorOutput Green "Obsidian CLI Uninstallation"
Write-Host "=========================================="
Write-Host ""

# Check if obsidian is installed
$ExePath = Join-Path $InstallDir "obsidian.exe"
if (-not (Test-Path $ExePath)) {
    Write-ColorOutput Yellow "Obsidian CLI is not installed in $InstallDir"
    exit 0
}

# Get version before uninstalling
try {
    $Version = & $ExePath --version 2>$null
} catch {
    $Version = "unknown"
}

Write-Host "Found: obsidian $Version"
Write-Host "Location: $InstallDir"
Write-Host ""

# Confirm uninstallation
$confirm = Read-Host "Are you sure you want to uninstall Obsidian CLI? (y/N)"
if ($confirm -ne "y" -and $confirm -ne "Y") {
    Write-Host "Uninstallation cancelled."
    exit 0
}

# Remove entire installation directory
Write-Host "Removing Obsidian CLI files..."

try {
    Remove-Item $InstallDir -Recurse -Force -ErrorAction Stop
    Write-ColorOutput Green "✓ Removed all Obsidian CLI files"
} catch {
    Write-ColorOutput Red "✗ Failed to remove $InstallDir"
    Write-ColorOutput Red $_.Exception.Message
    exit 1
}

Write-Host ""
Write-ColorOutput Green "✓ Obsidian CLI has been uninstalled successfully"
Write-Host ""

# Check if install directory is in PATH
$CurrentPath = [Environment]::GetEnvironmentVariable("Path", "User")
if ($CurrentPath -like "*$InstallDir*") {
    Write-Host "Removing from PATH..."
    try {
        $NewPath = ($CurrentPath -split ';' | Where-Object { $_ -ne $InstallDir }) -join ';'
        [Environment]::SetEnvironmentVariable("Path", $NewPath, "User")
        Write-ColorOutput Green "✓ Removed from PATH"
        Write-Host ""
        Write-ColorOutput Yellow "⚠ Please restart your terminal for PATH changes to take effect"
    } catch {
        Write-ColorOutput Yellow "⚠ Failed to remove from PATH automatically"
        Write-Host "You may want to manually remove $InstallDir from your PATH"
    }
}

Write-Host ""
