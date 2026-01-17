#!/bin/bash
# Uninstallation script for Obsidian CLI

set -e  # Exit on error

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Configuration
INSTALL_DIR="${HOME}/bin/obsidian-cli"
BIN_DIR="${HOME}/bin"

echo -e "${GREEN}Obsidian CLI Uninstallation${NC}"
echo "=========================================="
echo ""

# Check if obsidian is installed
if [ ! -f "${INSTALL_DIR}/obsidian" ]; then
    echo -e "${YELLOW}Obsidian CLI is not installed in ${INSTALL_DIR}${NC}"
    exit 0
fi

# Get version before uninstalling
VERSION=$(${INSTALL_DIR}/obsidian --version 2>/dev/null || echo "unknown")
echo "Found: obsidian ${VERSION}"
echo "Location: ${INSTALL_DIR}"
echo ""

# Confirm uninstallation
read -p "Are you sure you want to uninstall Obsidian CLI? (y/N) " -n 1 -r
echo
if [[ ! $REPLY =~ ^[Yy]$ ]]; then
    echo "Uninstallation cancelled."
    exit 0
fi

# Remove symlink
echo "Removing Obsidian CLI files..."

if [ -L "${BIN_DIR}/obsidian" ]; then
    rm "${BIN_DIR}/obsidian"
    echo -e "${GREEN}✓ Removed symlink: ${BIN_DIR}/obsidian${NC}"
fi

# Remove installation directory
if [ -d "${INSTALL_DIR}" ]; then
    rm -rf "${INSTALL_DIR}"
    echo -e "${GREEN}✓ Removed installation directory: ${INSTALL_DIR}${NC}"
fi

echo ""
echo -e "${GREEN}✓ Obsidian CLI has been uninstalled successfully${NC}"
echo ""
echo "Note: ${BIN_DIR} is still in your PATH."
echo "If you added ${BIN_DIR} specifically for Obsidian CLI and don't use it for other tools,"
echo "you may want to remove it from your shell configuration file."
