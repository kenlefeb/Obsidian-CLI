
# Obsidian CLI

This command line interface provides an assortment of features for working with an Obsidian (or other markdown-based) knowledge base from the command line.

While it results in some duplication of functionality, this toolkit is designed to _not_ require any Obsidian plugins, so you really don't even need Obsidian to use this (if you use FOAM, in VS Code, for instance).

## Installation

### Quick Install

**macOS/Linux:**
```bash
# Clone the repository
git clone https://github.com/kenlefeb/Obsidian-CLI.git
cd Obsidian-CLI

# Run the installation script
./install.sh
```

**Windows (PowerShell):**
```powershell
# Clone the repository
git clone https://github.com/kenlefeb/Obsidian-CLI.git
cd Obsidian-CLI

# Run the installation script
.\install.ps1
```

The installation script will:
- ✓ Run all tests to ensure everything works
- ✓ Build the project in Release mode
- ✓ Install the `obsidian` command to your local bin directory
- ✓ Add the installation directory to your PATH (if needed)

**Installation locations:**
- **macOS/Linux:** `~/bin/obsidian`
- **Windows:** `%LOCALAPPDATA%\Programs\obsidian\obsidian.exe`

### Manual Installation

If you prefer to build manually:

```bash
dotnet publish src/CLI/CLI.csproj -c Release -o ./bin
```

Then copy the binaries to a directory in your PATH.

### Uninstall

To uninstall Obsidian CLI:

**macOS/Linux:**
```bash
./uninstall.sh
```

**Windows (PowerShell):**
```powershell
.\uninstall.ps1
```

The uninstall script will remove all installed files and optionally clean up PATH modifications.

## Usage/Examples

```powershell
> obsidian daily-note add --date 2025-01-01 --vault vault
```

This will create a new daily note, using your default daily note template, in the vault with an id of "vault".

## License

[Creative Commons BY-SA 4.0](https://creativecommons.org/licenses/by-sa/4.0/)


## FAQ

#### What is the templating syntax?

I am using Handlebars.NET to implement the templating, so you can visit [their repo](https://github.com/Handlebars-Net/Handlebars.Net) for more details on how to compose a template. Note that, currently, the _only_ data being passed into the template is `date` which represents the date of the note being created.

If there are particular pieces of data that you would find helpful for use in a template, let me know.


## Roadmap

- Polish this MVP to make it more robust, with error handling, better documentation, etc.

- Be more cross-platform-aware. I use Obsidian on MacOS at home, and on Windows at work, so I want this tool to work well on both operating systems.

- Implement the recurrence pattern support so you can use different templates for different days, based on the patters defined in the settings file.

- Implement template "inheritance". My thinking, at the moment, is to just do this by running templates in order from the top-down against the target note, so the "child" templates are just applying themselves over top of "parent" templates.

- Implement configuration editing from the CLI.

- Implement Task support, so when you create a new daily-note, we look at the last daily-note to find all the uncompleted tasks and pull them forward into the new note.



## Running Tests

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run in watch mode
dotnet watch test --project tests/obsidian.tests
```

**Test Coverage:** 47% overall - **Domain: 92% coverage** 🎉
- See [TEST-COVERAGE.md](TEST-COVERAGE.md) for detailed coverage report
- We follow Test-Driven Development (TDD) practices - see [CLAUDE.MD](CLAUDE.MD)

Current test suite:
- ✅ **33 tests passing** - all green!
- ✅ Template & Recurrence classes 100% tested
- ✅ DailyNote class 89% coverage
- ✅ DailyNotes collection 90% coverage
- ✅ **Fixed 3 critical bugs** discovered through TDD

## Contributing

Contributions are always welcome!

See `contributing.md` for ways to get started.

Please adhere to this project's `code of conduct`.

