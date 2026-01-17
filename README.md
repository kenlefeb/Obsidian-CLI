
# Obsidian CLI

This command line interface provides an assortment of features for working with an Obsidian (or other markdown-based) knowledge base from the command line.

While it results in some duplication of functionality, this toolkit is designed to _not_ require any Obsidian plugins, so you really don't even need Obsidian to use this (if you use FOAM, in VS Code, for instance).

## Installation

Currently, you'll need to clone the repo and build it yourself. Once you have the resulting binaries, just make sure the Obsidian.exe is in your path.
    
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

**Test Coverage:** 37% overall (Phase 3 partial - bug discovered)
- Domain package: **74% coverage** ✅
- See [TEST-COVERAGE.md](TEST-COVERAGE.md) for detailed coverage report
- We follow Test-Driven Development (TDD) practices - see [CLAUDE.MD](CLAUDE.MD)

Current test suite:
- ✅ 29 tests passing
- ✅ Template & Recurrence classes fully tested (Phase 1)
- ✅ DailyNote class 84% coverage with integration tests (Phase 2)
- ⚠️ DailyNotes collection partial (Phase 3 - discovered critical bug in enumeration)

## Contributing

Contributions are always welcome!

See `contributing.md` for ways to get started.

Please adhere to this project's `code of conduct`.

