# Installation Guide

This guide covers installation, configuration, and uninstallation of Obsidian CLI.

## Quick Start

### Install

**macOS/Linux:**
```bash
git clone https://github.com/kenlefeb/Obsidian-CLI.git
cd Obsidian-CLI
./install.sh
```

**Windows:**
```powershell
git clone https://github.com/kenlefeb/Obsidian-CLI.git
cd Obsidian-CLI
.\install.ps1
```

### Verify Installation

After installation, verify it works:

```bash
obsidian --version
obsidian --help
```

## Installation Details

### What Gets Installed

The installation script:
1. Runs all 33 tests to ensure quality (92% Domain coverage)
2. Builds the project in Release mode
3. Detects your platform (osx-arm64, osx-x64, linux-x64, linux-arm64, win-x64, win-x86)
4. Installs binaries and dependencies to:
   - **macOS/Linux:** `~/bin/obsidian-cli/`
   - **Windows:** `%LOCALAPPDATA%\Programs\obsidian\`
5. Creates a symlink at `~/bin/obsidian` (macOS/Linux only)
6. Adds `~/bin` to PATH if needed (macOS/Linux) or adds installation directory (Windows)

### Installed Files

**macOS/Linux:**
```
~/bin/
├── obsidian -> ~/bin/obsidian-cli/obsidian  (symlink)
└── obsidian-cli/
    ├── obsidian
    ├── obsidian.dll
    ├── Domain.dll
    ├── Handlebars.dll
    ├── settings.json
    ├── obsidian.runtimeconfig.json
    ├── obsidian.deps.json
    ├── [various .NET runtime DLLs]
    └── files/
        └── [template files]
```

**Windows:**
```
%LOCALAPPDATA%\Programs\obsidian\
├── obsidian.exe
├── obsidian.dll
├── Domain.dll
├── Handlebars.dll
├── settings.json
├── obsidian.runtimeconfig.json
├── obsidian.deps.json
├── [various .NET runtime DLLs]
└── files/
    └── [template files]
```

### Requirements

- **.NET 10 SDK** or later
- **macOS:** 10.15+ (Intel or Apple Silicon)
- **Linux:** Any modern distribution (x64 or ARM64)
- **Windows:** Windows 10+ (x64 or x86)

### PATH Configuration

#### macOS/Linux

If the installation directory isn't in your PATH, add it to your shell config:

**Bash** (`~/.bashrc` or `~/.bash_profile`):
```bash
export PATH="$PATH:$HOME/bin"
```

**Zsh** (`~/.zshrc`):
```bash
export PATH="$PATH:$HOME/bin"
```

**Fish** (`~/.config/fish/config.fish`):
```fish
set -gx PATH $PATH $HOME/bin
```

Then reload your shell:
```bash
source ~/.bashrc  # or ~/.zshrc, etc.
```

#### Windows

The PowerShell install script automatically adds the installation directory to your user PATH. After installation, restart your terminal for the changes to take effect.

To verify PATH is set correctly:
```powershell
$env:Path -split ';' | Select-String obsidian
```

## Manual Installation

If you prefer to build and install manually:

### Build

```bash
dotnet publish src/CLI/CLI.csproj \
  --configuration Release \
  --runtime <RID> \
  --output ./publish
```

Replace `<RID>` with your platform:
- `osx-arm64` - macOS Apple Silicon
- `osx-x64` - macOS Intel
- `linux-x64` - Linux x64
- `linux-arm64` - Linux ARM64
- `win-x64` - Windows x64
- `win-x86` - Windows x86

### Install

Copy all files from `./publish/` to a directory:

```bash
# macOS/Linux
mkdir -p ~/bin/obsidian-cli
cp -r ./publish/* ~/bin/obsidian-cli/
chmod +x ~/bin/obsidian-cli/obsidian

# Create symlink for easy access
ln -s ~/bin/obsidian-cli/obsidian ~/bin/obsidian

# Make sure ~/bin is in your PATH
export PATH="$PATH:$HOME/bin"

# Windows (PowerShell)
New-Item -ItemType Directory -Path $env:LOCALAPPDATA\Programs\obsidian -Force
Copy-Item .\publish\* $env:LOCALAPPDATA\Programs\obsidian\ -Recurse
```

## Configuration

After installation, you need to configure Obsidian CLI with your vault settings. See the main README for configuration details.

Quick start:
```bash
obsidian config list --all
```

## Uninstallation

To remove Obsidian CLI:

**macOS/Linux:**
```bash
cd Obsidian-CLI
./uninstall.sh
```

**Windows:**
```powershell
cd Obsidian-CLI
.\uninstall.ps1
```

The uninstall script will:
- Remove the symlink at `~/bin/obsidian` (macOS/Linux)
- Remove the installation directory (`~/bin/obsidian-cli` or Windows equivalent)
- Remove all installed files and dependencies
- Remove from PATH (Windows only - macOS/Linux users should manually edit shell config if needed)

### Manual Uninstall

**macOS/Linux:**
```bash
# Remove symlink
rm ~/bin/obsidian

# Remove installation directory
rm -rf ~/bin/obsidian-cli

# Optionally remove the PATH export from your shell config file if you added it
```

**Windows:**
```powershell
Remove-Item "$env:LOCALAPPDATA\Programs\obsidian" -Recurse
# Also remove from PATH via System Properties > Environment Variables
```

## Troubleshooting

### "dotnet: command not found"

Install the .NET SDK from: https://dotnet.microsoft.com/download

### "obsidian: command not found" (after installation)

**macOS/Linux:**
1. Verify the symlink exists: `ls -la ~/bin/obsidian`
2. Verify the installation directory exists: `ls ~/bin/obsidian-cli/`
3. Check if `~/bin` is in your PATH: `echo $PATH`
4. Restart your terminal
5. If still not working, add `~/bin` to PATH manually (see PATH Configuration above)

**Windows:**
1. Verify the file exists: `dir $env:LOCALAPPDATA\Programs\obsidian\obsidian.exe`
2. Check if the directory is in your PATH: `$env:Path`
3. Restart your terminal
4. If still not working, add the directory to PATH manually (see PATH Configuration above)

### "Permission denied" (macOS/Linux)

Make sure the executable bit is set:
```bash
chmod +x ~/bin/obsidian-cli/obsidian
```

### Tests fail during installation

The installation script will ask if you want to continue anyway. However, we recommend investigating test failures before installing:

```bash
dotnet test tests/obsidian.tests/obsidian.tests.csproj --verbosity normal
```

### Platform detection issues

If the install script doesn't detect your platform correctly, you can manually specify the runtime identifier in the publish command.

## Updates

To update to the latest version:

```bash
cd Obsidian-CLI
git pull
./install.sh  # Will overwrite the existing installation
```

## Development Installation

If you're contributing to the project, you may want to use `dotnet run` instead of installing:

```bash
cd src/CLI
dotnet run -- --help
```

Or create an alias:
```bash
alias obsidian-dev='dotnet run --project ~/src/Obsidian-CLI/src/CLI/CLI.csproj --'
```
