# Installation script for Obsidian CLI (PowerShell)
# This script builds and installs the Obsidian CLI tool to your local system

$ErrorActionPreference = "Stop"

# Configuration
$InstallDir = "$env:LOCALAPPDATA\Programs\obsidian"
$ProjectDir = $PSScriptRoot
$CliProject = Join-Path $ProjectDir "src\CLI\CLI.csproj"

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
Write-ColorOutput Green "Obsidian CLI Installation"
Write-Host "=========================================="
Write-Host ""

# Check if .NET SDK is installed
try {
    $dotnetVersion = dotnet --version
    Write-ColorOutput Green "✓ Found .NET SDK version: $dotnetVersion"
} catch {
    Write-ColorOutput Red "Error: .NET SDK not found!"
    Write-Host "Please install .NET 10 SDK from: https://dotnet.microsoft.com/download"
    exit 1
}

# Check if CLI project exists
if (-not (Test-Path $CliProject)) {
    Write-ColorOutput Red "Error: CLI project not found at $CliProject"
    exit 1
}

# Determine runtime identifier
$Rid = if ([Environment]::Is64BitOperatingSystem) {
    "win-x64"
} else {
    "win-x86"
}

Write-ColorOutput Green "✓ Detected platform: $Rid"
Write-Host ""

# Run tests before installing
Write-Host "Running tests..."
$TestProject = Join-Path $ProjectDir "tests\obsidian.tests\obsidian.tests.csproj"

try {
    $testResult = dotnet test $TestProject --verbosity quiet --nologo 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-ColorOutput Green "✓ All tests passed"
    } else {
        Write-ColorOutput Red "✗ Tests failed"
        Write-Host ""
        $continue = Read-Host "Continue with installation anyway? (y/N)"
        if ($continue -ne "y" -and $continue -ne "Y") {
            Write-Host "Installation cancelled."
            exit 1
        }
    }
} catch {
    Write-ColorOutput Yellow "⚠ Could not run tests"
}

Write-Host ""
Write-Host "Building Obsidian CLI..."

# Clean previous build
dotnet clean $CliProject --configuration Release --nologo --verbosity quiet | Out-Null

# Build and publish
$PublishDir = Join-Path $ProjectDir "publish"
dotnet publish $CliProject `
    --configuration Release `
    --runtime $Rid `
    --self-contained false `
    --output $PublishDir `
    --nologo `
    --verbosity quiet

if ($LASTEXITCODE -ne 0) {
    Write-ColorOutput Red "✗ Build failed"
    exit 1
}

Write-ColorOutput Green "✓ Build completed successfully"
Write-Host ""

# Create install directory if it doesn't exist
if (-not (Test-Path $InstallDir)) {
    Write-Host "Creating installation directory: $InstallDir"
    New-Item -ItemType Directory -Path $InstallDir -Force | Out-Null
}

# Copy all files to installation directory
Write-Host "Installing to $InstallDir..."
Copy-Item "$PublishDir\*" $InstallDir -Recurse -Force

Write-ColorOutput Green "✓ Installation completed successfully"
Write-Host ""

# Check if install directory is in PATH
$CurrentPath = [Environment]::GetEnvironmentVariable("Path", "User")
if ($CurrentPath -notlike "*$InstallDir*") {
    Write-ColorOutput Yellow "⚠ Warning: $InstallDir is not in your PATH"
    Write-Host ""
    Write-Host "Adding to user PATH..."

    try {
        $NewPath = $CurrentPath + ";" + $InstallDir
        [Environment]::SetEnvironmentVariable("Path", $NewPath, "User")
        Write-ColorOutput Green "✓ Added to PATH"
        Write-Host ""
        Write-ColorOutput Yellow "⚠ Please restart your terminal for PATH changes to take effect"
        Write-Host ""
    } catch {
        Write-ColorOutput Red "✗ Failed to add to PATH automatically"
        Write-Host ""
        Write-Host "To add it manually, run this command in an elevated PowerShell:"
        Write-Host ""
        Write-ColorOutput Green "  [Environment]::SetEnvironmentVariable('Path', `$env:Path + ';$InstallDir', 'User')"
        Write-Host ""
    }
} else {
    Write-ColorOutput Green "✓ $InstallDir is already in your PATH"
    Write-Host ""
}

# Verify installation
$ExePath = Join-Path $InstallDir "obsidian.exe"
if (Test-Path $ExePath) {
    try {
        $Version = & $ExePath --version 2>$null
    } catch {
        $Version = "unknown"
    }

    Write-Host "Installation complete!"
    Write-Host ""
    Write-Host "Installed: obsidian $Version"
    Write-Host "Location: $ExePath"
    Write-Host ""
    Write-Host "Try it out with: obsidian --help"
} else {
    Write-ColorOutput Red "✗ Installation verification failed"
    exit 1
}

Write-Host ""
