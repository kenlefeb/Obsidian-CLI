#!/bin/bash
# Installation script for Obsidian CLI
# This script builds and installs the Obsidian CLI tool to your local system

set -e  # Exit on error

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Configuration
INSTALL_DIR="${HOME}/bin/obsidian-cli"
PROJECT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
CLI_PROJECT="${PROJECT_DIR}/src/CLI/CLI.csproj"

echo -e "${GREEN}Obsidian CLI Installation${NC}"
echo "=========================================="
echo ""

# Check if .NET SDK is installed
if ! command -v dotnet &> /dev/null; then
    echo -e "${RED}Error: .NET SDK not found!${NC}"
    echo "Please install .NET 10 SDK from: https://dotnet.microsoft.com/download"
    exit 1
fi

# Display .NET version
DOTNET_VERSION=$(dotnet --version)
echo -e "✓ Found .NET SDK version: ${GREEN}${DOTNET_VERSION}${NC}"

# Check if CLI project exists
if [ ! -f "${CLI_PROJECT}" ]; then
    echo -e "${RED}Error: CLI project not found at ${CLI_PROJECT}${NC}"
    exit 1
fi

# Determine runtime identifier
case "$(uname -s)" in
    Darwin*)
        if [ "$(uname -m)" = "arm64" ]; then
            RID="osx-arm64"
        else
            RID="osx-x64"
        fi
        ;;
    Linux*)
        if [ "$(uname -m)" = "aarch64" ]; then
            RID="linux-arm64"
        else
            RID="linux-x64"
        fi
        ;;
    *)
        echo -e "${RED}Error: Unsupported operating system${NC}"
        exit 1
        ;;
esac

echo -e "✓ Detected platform: ${GREEN}${RID}${NC}"
echo ""

# Run tests before installing
echo "Running tests..."
if dotnet test "${PROJECT_DIR}/tests/obsidian.tests/obsidian.tests.csproj" --verbosity quiet --nologo; then
    echo -e "${GREEN}✓ All tests passed${NC}"
else
    echo -e "${RED}✗ Tests failed${NC}"
    echo ""
    read -p "Continue with installation anyway? (y/N) " -n 1 -r
    echo
    if [[ ! $REPLY =~ ^[Yy]$ ]]; then
        echo "Installation cancelled."
        exit 1
    fi
fi

echo ""
echo "Building Obsidian CLI..."

# Clean previous build
dotnet clean "${CLI_PROJECT}" --configuration Release --nologo --verbosity quiet > /dev/null 2>&1

# Build and publish
dotnet publish "${CLI_PROJECT}" \
    --configuration Release \
    --runtime "${RID}" \
    --self-contained false \
    --output "${PROJECT_DIR}/publish" \
    --nologo \
    --verbosity quiet

if [ $? -ne 0 ]; then
    echo -e "${RED}✗ Build failed${NC}"
    exit 1
fi

echo -e "${GREEN}✓ Build completed successfully${NC}"
echo ""

# Create install directory if it doesn't exist
if [ ! -d "${INSTALL_DIR}" ]; then
    echo "Creating installation directory: ${INSTALL_DIR}"
    mkdir -p "${INSTALL_DIR}"
fi

# Copy all files to installation directory
echo "Installing to ${INSTALL_DIR}..."
cp -r "${PROJECT_DIR}/publish/"* "${INSTALL_DIR}/"
chmod +x "${INSTALL_DIR}/obsidian"

# Create symlink in ~/bin for easy access
BIN_DIR="${HOME}/bin"
if [ ! -d "${BIN_DIR}" ]; then
    mkdir -p "${BIN_DIR}"
fi

# Remove old symlink if it exists
if [ -L "${BIN_DIR}/obsidian" ] || [ -f "${BIN_DIR}/obsidian" ]; then
    rm "${BIN_DIR}/obsidian"
fi

# Create new symlink
ln -s "${INSTALL_DIR}/obsidian" "${BIN_DIR}/obsidian"
echo -e "${GREEN}✓ Created symlink: ${BIN_DIR}/obsidian -> ${INSTALL_DIR}/obsidian${NC}"

echo -e "${GREEN}✓ Installation completed successfully${NC}"
echo ""

# Check if ~/bin is in PATH
if [[ ":$PATH:" != *":${BIN_DIR}:"* ]]; then
    echo -e "${YELLOW}⚠ Warning: ${BIN_DIR} is not in your PATH${NC}"
    echo ""
    echo "To add it to your PATH, add this line to your shell configuration file:"
    echo ""

    # Detect shell
    if [ -n "$ZSH_VERSION" ]; then
        SHELL_CONFIG="~/.zshrc"
    elif [ -n "$BASH_VERSION" ]; then
        SHELL_CONFIG="~/.bashrc"
    else
        SHELL_CONFIG="~/.profile"
    fi

    echo -e "  ${GREEN}export PATH=\"\$PATH:${BIN_DIR}\"${NC}"
    echo ""
    echo "Add this to: ${SHELL_CONFIG}"
    echo ""
    echo "Then run: source ${SHELL_CONFIG}"
    echo ""
else
    echo -e "${GREEN}✓ ${BIN_DIR} is already in your PATH${NC}"
    echo ""
fi

# Verify installation
if [ -x "${INSTALL_DIR}/obsidian" ]; then
    VERSION=$(${INSTALL_DIR}/obsidian --version 2>/dev/null || echo "unknown")
    echo "Installation complete!"
    echo ""
    echo "Installed: obsidian ${VERSION}"
    echo "Location: ${INSTALL_DIR}/obsidian"
    echo ""
    echo "Try it out with: obsidian --help"
else
    echo -e "${RED}✗ Installation verification failed${NC}"
    exit 1
fi
