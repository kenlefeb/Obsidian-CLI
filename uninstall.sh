#!/bin/bash
# Uninstallation script for Obsidian CLI

set -e  # Exit on error

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Configuration
INSTALL_DIR="${HOME}/bin"

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

# Remove obsidian binary and related files
echo "Removing Obsidian CLI files..."

# Remove the main executable
if [ -f "${INSTALL_DIR}/obsidian" ]; then
    rm "${INSTALL_DIR}/obsidian"
    echo -e "${GREEN}✓ Removed obsidian executable${NC}"
fi

# Remove DLL files
rm -f "${INSTALL_DIR}/"*.dll 2>/dev/null && echo -e "${GREEN}✓ Removed DLL files${NC}"

# Remove .pdb files
rm -f "${INSTALL_DIR}/"*.pdb 2>/dev/null && echo -e "${GREEN}✓ Removed debug files${NC}"

# Remove configuration files
rm -f "${INSTALL_DIR}/obsidian.runtimeconfig.json" 2>/dev/null
rm -f "${INSTALL_DIR}/obsidian.deps.json" 2>/dev/null
rm -f "${INSTALL_DIR}/settings.json" 2>/dev/null
echo -e "${GREEN}✓ Removed configuration files${NC}"

# Remove files directory if it exists
if [ -d "${INSTALL_DIR}/files" ]; then
    rm -rf "${INSTALL_DIR}/files"
    echo -e "${GREEN}✓ Removed files directory${NC}"
fi

echo ""
echo -e "${GREEN}✓ Obsidian CLI has been uninstalled successfully${NC}"
echo ""
echo "Note: ${INSTALL_DIR} is still in your PATH."
echo "If you don't use this directory for other tools, you may want to remove it from your PATH."
