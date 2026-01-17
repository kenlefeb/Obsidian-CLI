# Project Overview

**Obsidian CLI** is a command-line interface tool for working with Obsidian (or other markdown-based) knowledge bases. The project is written in C# (.NET 8.0) and provides functionality for managing daily notes, templates, and vault configurations without requiring Obsidian plugins.

## Key Features

- Create daily notes with custom templates
- Manage multiple vaults through configuration
- Template support using Handlebars.NET
- Recurrence patterns for different daily note templates
- Cross-platform support (macOS and Windows)

## Project Structure

```
Obsidian-CLI/
├── src/
│   ├── CLI/                    # Command-line interface implementation
│   │   ├── Configuration/      # Configuration management commands
│   │   ├── DailyNotes/         # Daily note commands and options
│   │   ├── Exceptions/         # Custom exceptions
│   │   ├── Extensions/         # Extension methods
│   │   ├── Global/             # Global options and root command
│   │   └── Program.cs          # Application entry point
│   └── Domain/                 # Domain models and business logic
│       ├── Settings/           # Configuration models
│       ├── DailyNote.cs
│       ├── Note.cs
│       ├── Recurrence.cs
│       ├── Template.cs
│       └── Vault.cs
├── tests/
│   └── obsidian.tests/
└── README.md
```

## Key Technologies

- **Language**: C# / .NET 8.0
- **CLI Framework**: System.CommandLine
- **Templating**: Handlebars.NET
- **License**: Creative Commons BY-SA 4.0

## Common Commands

```bash
# Create a daily note
obsidian daily-note add --date 2025-01-01 --vault vault

# List configuration (planned)
obsidian configuration list --all

# Global options
--verbose    # Enable verbose output
--dry-run    # Preview changes without making them
```

## Architecture Notes

### Command Structure
- Uses System.CommandLine for CLI parsing
- Commands are organized hierarchically (root > command > subcommand)
- Global options (verbose, dry-run) apply across all commands
- Each command has its own Options class for type-safe option handling

### Domain Model
- **Vault**: Represents an Obsidian vault with settings
- **DailyNote**: Extends Note, represents daily notes
- **Template**: Handles Handlebars template processing
- **Recurrence**: Defines patterns for template application
- **VaultSettings**: Configuration for vaults including paths and templates

### Error Handling
- Custom exceptions in CLI/Exceptions/
- Recent improvements to error handling (see commit 785f2ec)
- Dry-run mode for safe operation preview

## Recent Changes

- Added comprehensive exception handling
- Resolved analysis recommendations for code quality
- Improved error condition checks throughout the codebase

## Development Roadmap

1. Enhanced error handling and documentation (in progress)
2. Cross-platform compatibility improvements
3. Recurrence pattern support for templates
4. Template inheritance system
5. CLI-based configuration editing
6. Task support (carry forward uncompleted tasks)

## Configuration

The tool uses a settings file to manage vault configurations. Each vault has:
- Unique ID
- File system path
- Daily note templates
- Template application patterns

## Template System

- Uses Handlebars.NET syntax
- Currently passes `date` data to templates
- Planned support for template inheritance
- Recurrence patterns will allow different templates for different days

## Development Methodology

This project follows **Extreme Programming (XP)** practices:

### Test-Driven Development (TDD)
- **Test-First Approach**: Write tests before writing production code
- **Red-Green-Refactor Cycle**:
  1. Write a failing test that defines the desired behavior
  2. Write the minimal code to make the test pass
  3. Refactor while keeping tests green
- All new features and bug fixes must start with a failing test
- Tests serve as executable specifications and documentation
- Maintain high test coverage as a natural outcome of TDD

### Pair Programming with Claude
- **Claude as Pair Partner**: When working with Claude, treat the interaction as a pair programming session
- **Driver-Navigator Model**:
  - You (human) decide what to build and guide the direction
  - Claude writes code, suggests implementations, and asks clarifying questions
  - Frequent back-and-forth to refine design and implementation
- **Collaborative Design**: Discuss design decisions before implementing
- **Continuous Review**: Claude reviews code as it's written, catching issues early
- **Shared Ownership**: Both partners are responsible for code quality

### XP Workflow with Claude

1. **Discuss the Feature**: Explain what you want to build
2. **Write the Test First**: Start by writing a failing test together
3. **Implement Minimally**: Write just enough code to pass the test
4. **Refactor Together**: Clean up code while tests remain green
5. **Iterate**: Repeat for each small piece of functionality

### Session State Management

To maintain continuity across sessions and context resets, Claude maintains three state files in `.claude/`:

#### State Files

1. **`.claude/context.md`** - Session Goal
   - Contains the high-level goal of the current working session
   - Describes what we're trying to accomplish
   - Updated when starting a new session or changing focus
   - Read first after any context reset

2. **`.claude/todos.md`** - Task Tracking
   - Lists all tasks needed to complete the session goal
   - Tracks completion status (pending/in-progress/completed)
   - Updated as tasks are completed
   - Checked off before memory compaction to preserve progress
   - Read after context reset to resume work

3. **`.claude/insights.md`** - Implementation Learning
   - Iteratively updated after each change is implemented
   - Captures design decisions, patterns discovered, and gotchas
   - Documents why certain approaches were taken
   - Helps avoid repeating mistakes or revisiting settled decisions
   - Provides historical context for future work

#### Workflow with State Files

**Starting a New Session:**
1. Create/update `.claude/context.md` with session goal
2. Create `.claude/todos.md` with initial task list
3. Create `.claude/insights.md` (or clear previous content)

**During Development:**
1. Follow TDD: write test, implement, refactor
2. Check off completed tasks in `.claude/todos.md` immediately
3. Update `.claude/insights.md` after each implementation with learnings
4. Keep state files current before any memory compaction

**After Context Reset:**
1. Read `.claude/context.md` to understand the session goal
2. Read `.claude/todos.md` to see what's been completed and what remains
3. Read `.claude/insights.md` to understand decisions made so far
4. Continue from where we left off

**Ending a Session:**
- Ensure all state files reflect current progress
- All completed tasks are checked off in `.claude/todos.md`
- Final insights are recorded in `.claude/insights.md`
- State files serve as handoff documentation for next session

### Testing Practices

- Tests are located in `tests/obsidian.tests/`
- Use descriptive test names that explain behavior
- Keep tests fast and isolated
- Mock external dependencies
- Test one concept per test method
- Arrange-Act-Assert (AAA) pattern for test structure

### TDD Benefits for This Project

- Ensures vault operations work correctly across platforms
- Validates template rendering with various inputs
- Confirms error handling behaves as expected
- Provides regression safety when refactoring
- Documents expected behavior through executable examples

## Contributing

- See CONTRIBUTING.md for contribution guidelines
- Follow the CODE_OF_CONDUCT.md
- **All contributions must follow TDD**: Include tests that were written first
- Maintain cross-platform compatibility
- Pair with Claude or other developers when possible
- Keep test coverage high through test-first development

## Known Limitations

- Currently requires manual build from source
- Limited to date-based templating (more data planned)
- Recurrence patterns not yet fully implemented
- Task forwarding not yet implemented

## Development Notes

- The tool is designed to work without Obsidian plugins
- Compatible with other markdown systems (e.g., FOAM in VS Code)
- Focus on being lightweight and command-line friendly
- Maintains separation between CLI and Domain layers
