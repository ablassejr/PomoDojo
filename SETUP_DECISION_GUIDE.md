# PomoDojo Setup Decision Guide

This guide will walk you through all the decisions needed to set up the PomoDojo project structure and code skeleton. Use this with Claude Code CLI by answering each question in sequence.

---

## Section 1: Project Foundation

### 1.1 Build System for C++ Core
**Question:** Which build system do you want to use for the C++ core?

**Options:**
- **CMake** (Recommended) - Industry standard, cross-platform, widely supported
- **Meson** - Modern, fast, Python-based
- **Premake** - Lua-based, simpler than CMake
- **Make** - Traditional, Linux-focused

**Your Answer:** `[BUILD_SYSTEM]`

**Impact:** This determines the build configuration files and compilation workflow.

---

### 1.2 C++ Standard Version
**Question:** Which C++ standard version do you want to target?

**Options:**
- **C++17** - Stable, widely supported
- **C++20** - Modern features (concepts, modules, ranges)
- **C++23** - Latest standard (limited compiler support)

**Your Answer:** `[CPP_STANDARD]`

**Impact:** Affects available language features and compiler requirements.

---

### 1.3 C# Target Framework
**Question:** Which .NET version do you want to target for the C# UI?

**Options:**
- **.NET 8** (Recommended) - Latest LTS version
- **.NET 7** - Recent version
- **.NET 6** - Previous LTS version
- **.NET Framework 4.8** - Windows-only legacy

**Your Answer:** `[DOTNET_VERSION]`

**Impact:** Determines available C# features and platform support.

---

## Section 2: User Interface Framework

### 2.1 C# UI Framework Selection
**Question:** Which UI framework do you want to use for the cross-platform C# interface?

**Options:**
- **Avalonia UI** (Recommended) - XAML-based, truly cross-platform, modern
- **MAUI** - Microsoft's official cross-platform framework
- **WPF** - Windows-only, mature, XAML-based
- **WinForms** - Windows-only, simpler, older
- **Uno Platform** - XAML-based, runs on multiple platforms

**Your Answer:** `[UI_FRAMEWORK]`

**Impact:** Determines UI capabilities, platform support, and development approach.

---

### 2.2 UI Architecture Pattern
**Question:** Which architecture pattern do you want to use for the UI layer?

**Options:**
- **MVVM (Model-View-ViewModel)** (Recommended for XAML frameworks)
- **MVC (Model-View-Controller)**
- **MVP (Model-View-Presenter)**
- **Simple code-behind** (For prototyping)

**Your Answer:** `[UI_PATTERN]`

**Impact:** Affects code organization and testability of UI components.

---

## Section 3: C++ and C# Interoperability

### 3.1 Interop Mechanism
**Question:** How do you want C# to communicate with the C++ core?

**Options:**
- **C++/CLI** - Direct C++ wrapper for .NET (Windows-focused)
- **P/Invoke with C API** (Recommended) - C# calls C functions that wrap C++ code
- **COM Interop** - Component Object Model (Windows-only)
- **gRPC** - Network-based communication (overkill for desktop app)
- **Native AOT with C exports** - Modern .NET 7+ approach

**Your Answer:** `[INTEROP_METHOD]`

**Impact:** Determines how the C++ core and C# UI communicate and share data.

---

### 3.2 Shared Data Serialization
**Question:** How do you want to serialize data between C++ and C#?

**Options:**
- **JSON** - Human-readable, flexible
- **Protocol Buffers** - Efficient, schema-based
- **Direct memory structures** - Fastest, requires careful management
- **Simple primitive types** - Easiest to start

**Your Answer:** `[SERIALIZATION]`

**Impact:** Affects performance and ease of data sharing between components.

---

## Section 4: Core C++ Architecture

### 4.1 Timer Implementation
**Question:** Which timer mechanism do you want to use in the C++ core?

**Options:**
- **std::chrono with condition variables** (Recommended) - Modern C++, portable
- **Boost.Asio** - Powerful, requires Boost dependency
- **Platform-specific APIs** - Best performance, not portable
- **Simple thread sleep loops** - Easiest, less precise

**Your Answer:** `[TIMER_IMPL]`

**Impact:** Affects timing precision and cross-platform compatibility.

---

### 4.2 State Management
**Question:** How do you want to manage the Pomodoro timer state in C++?

**Options:**
- **State Machine pattern** (Recommended) - Clear, maintainable
- **Simple enum-based states** - Lightweight, good for simple cases
- **Observer pattern** - Event-driven approach
- **Finite State Machine library** - Formal FSM implementation

**Your Answer:** `[STATE_MANAGEMENT]`

**Impact:** Determines code structure for workflow logic.

---

### 4.3 Configuration Storage
**Question:** How do you want to store user settings and configuration?

**Options:**
- **JSON files** (Recommended) - Human-readable, easy to edit
- **XML files** - More verbose, widely supported
- **INI files** - Simple, traditional
- **SQLite database** - Structured, supports complex queries
- **Registry (Windows) / Preferences (macOS/Linux)** - Platform-specific

**Your Answer:** `[CONFIG_STORAGE]`

**Impact:** Affects how settings are persisted and loaded.

---

## Section 5: Testing Strategy

### 5.1 C++ Testing Framework
**Question:** Which testing framework do you want for C++ unit tests?

**Options:**
- **Google Test (gtest)** (Recommended) - Industry standard, feature-rich
- **Catch2** - Header-only, modern syntax
- **doctest** - Lightweight, fast compilation
- **Boost.Test** - Part of Boost, comprehensive

**Your Answer:** `[CPP_TEST_FRAMEWORK]`

**Impact:** Determines testing capabilities and test code style.

---

### 5.2 C# Testing Framework
**Question:** Which testing framework do you want for C# unit tests?

**Options:**
- **xUnit** (Recommended) - Modern, widely used in .NET
- **NUnit** - Mature, feature-rich
- **MSTest** - Microsoft's framework, integrated with Visual Studio

**Your Answer:** `[CSHARP_TEST_FRAMEWORK]`

**Impact:** Affects test structure and runner integration.

---

### 5.3 Integration Testing
**Question:** Do you want to set up integration tests for the C++/C# interface?

**Options:**
- **Yes** - Test the complete flow through both layers
- **No** - Focus on unit tests only initially
- **Later** - Set up infrastructure but write tests later

**Your Answer:** `[INTEGRATION_TESTS]`

**Impact:** Determines if integration test projects are created.

---

## Section 6: Dependency Management

### 6.1 C++ Dependency Manager
**Question:** How do you want to manage C++ dependencies?

**Options:**
- **vcpkg** (Recommended) - Microsoft's C++ package manager
- **Conan** - Modern, widely used
- **Git submodules** - Manual but simple
- **Manual** - Download and configure manually

**Your Answer:** `[CPP_DEPS]`

**Impact:** Affects how third-party C++ libraries are managed.

---

### 6.2 C# Dependency Manager
**Question:** How do you want to manage C# dependencies?

**Options:**
- **NuGet** (Standard) - Built into .NET ecosystem
- **Paket** - Alternative with better dependency resolution

**Your Answer:** `[CSHARP_DEPS]`

**Impact:** Typically NuGet is standard for .NET projects.

---

## Section 7: Development Tools

### 7.1 IDE/Editor Support
**Question:** Which development environments will your team use?

**Options (select multiple):**
- **Visual Studio** - Full-featured Windows IDE
- **Visual Studio Code** - Lightweight, cross-platform
- **Rider** - JetBrains IDE for .NET
- **CLion** - JetBrains IDE for C++
- **Command line only** - Terminal-based workflow

**Your Answer:** `[IDE_SUPPORT]` (can be multiple)

**Impact:** Determines which project files to generate (.sln, .vscode, etc.)

---

### 7.2 Code Formatting
**Question:** Do you want to enforce code formatting standards?

**Options:**
- **Yes with config files** - Set up .clang-format and .editorconfig
- **Yes without config** - Document standards in README
- **No** - Let developers use their preferences

**Your Answer:** `[CODE_FORMATTING]`

**Impact:** Affects code consistency across the team.

---

### 7.3 Static Analysis
**Question:** Do you want to set up static analysis tools?

**Options:**
- **Yes** - Set up clang-tidy for C++, Roslyn analyzers for C#
- **Later** - Add after initial development
- **No** - Skip static analysis

**Your Answer:** `[STATIC_ANALYSIS]`

**Impact:** Affects code quality enforcement.

---

## Section 8: Version Control & CI/CD

### 8.1 Git Ignore Configuration
**Question:** Which build artifacts do you want to ignore in Git?

**Options:**
- **Standard** - Ignore build/, bin/, obj/, etc.
- **Minimal** - Only ignore compiler outputs
- **Comprehensive** - Ignore IDE files, OS files, build artifacts

**Your Answer:** `[GITIGNORE_LEVEL]`

**Impact:** Determines what gets committed to the repository.

---

### 8.2 CI/CD Platform
**Question:** Do you want to set up continuous integration?

**Options:**
- **GitHub Actions** - Integrated with GitHub
- **Azure Pipelines** - Microsoft's CI/CD platform
- **GitLab CI** - If using GitLab
- **Later** - Set up after initial development
- **No** - Manual builds only

**Your Answer:** `[CI_PLATFORM]`

**Impact:** Determines if CI configuration files are created.

---

### 8.3 Build Configurations
**Question:** Which build configurations do you want to support initially?

**Options (select multiple):**
- **Debug** - Development builds with symbols
- **Release** - Optimized production builds
- **RelWithDebInfo** - Release with debug symbols
- **MinSizeRel** - Minimized binary size

**Your Answer:** `[BUILD_CONFIGS]` (can be multiple)

**Impact:** Affects CMake/build system configuration.

---

## Section 9: Documentation

### 9.1 API Documentation
**Question:** How do you want to document the code?

**Options:**
- **Doxygen** - C++ API documentation generator
- **XML comments** - C# inline documentation
- **Both** - Document both C++ and C#
- **Markdown only** - Simple README files
- **None** - Code comments only

**Your Answer:** `[API_DOCS]`

**Impact:** Determines documentation generation setup.

---

### 9.2 User Documentation
**Question:** What level of user documentation do you want to start with?

**Options:**
- **Comprehensive** - README, CONTRIBUTING, architecture docs
- **Basic** - README with setup and usage instructions
- **Minimal** - Just README
- **None** - Code only

**Your Answer:** `[USER_DOCS]`

**Impact:** Determines which documentation files are created.

---

## Section 10: Project Structure Preferences

### 10.1 Directory Organization
**Question:** How do you want to organize the project directories?

**Options:**
- **Monorepo** - All code in one repository with separate folders
- **Separate repos** - C++ core and C# UI in different repositories
- **Hybrid** - Monorepo with submodules for dependencies

**Your Answer:** `[REPO_STRUCTURE]`

**Impact:** Fundamental project organization decision.

---

### 10.2 Source Organization Style
**Question:** How do you want to organize source files within each component?

**Options:**
- **Flat** - All source files in src/, all headers in include/
- **Component-based** - Group by feature (timer/, config/, ui/)
- **Layer-based** - Group by layer (core/, services/, presentation/)

**Your Answer:** `[SOURCE_ORG]`

**Impact:** Affects folder structure and file navigation.

---

## Section 11: Feature Decisions

### 11.1 Notification System
**Question:** How do you want to implement timer notifications?

**Options:**
- **OS native** - Platform-specific notification APIs
- **In-app only** - Show alerts within the application
- **Both** - Native notifications + in-app alerts
- **Later** - Implement after core functionality

**Your Answer:** `[NOTIFICATIONS]`

**Impact:** Affects platform-specific code requirements.

---

### 11.2 Audio Alerts
**Question:** Do you want audio alerts when timers complete?

**Options:**
- **Yes** - Include audio playback capability
- **No** - Visual notifications only
- **Later** - Add after initial release

**Your Answer:** `[AUDIO_ALERTS]`

**Impact:** Determines if audio libraries are needed.

---

### 11.3 Data Persistence
**Question:** What data do you want to persist between sessions?

**Options:**
- **Settings only** - Just user preferences
- **Settings + session state** - Resume interrupted sessions
- **Settings + session + history** - Track all completed pomodoros
- **Full analytics** - Detailed productivity metrics

**Your Answer:** `[DATA_PERSISTENCE]`

**Impact:** Affects storage requirements and data models.

---

## Section 12: Deployment

### 12.1 Target Platforms (Priority Order)
**Question:** Which platforms do you want to support initially? (Rank in order)

**Options:**
- Windows
- macOS
- Linux

**Your Answer:** `[PLATFORM_1]`, `[PLATFORM_2]`, `[PLATFORM_3]`

**Impact:** Determines platform-specific code and testing priorities.

---

### 12.2 Distribution Method
**Question:** How do you plan to distribute the application?

**Options:**
- **Installer packages** - MSI (Windows), DMG (macOS), DEB/RPM (Linux)
- **Portable executable** - Standalone executable
- **App stores** - Microsoft Store, Mac App Store
- **Source only** - Users build from source
- **Multiple** - Support several distribution methods

**Your Answer:** `[DISTRIBUTION]`

**Impact:** Affects packaging and installation scripts needed.

---

## Section 13: Logging & Debugging

### 13.1 Logging Framework (C++)
**Question:** Which logging framework do you want for the C++ core?

**Options:**
- **spdlog** (Recommended) - Fast, modern, header-only
- **Boost.Log** - Comprehensive, part of Boost
- **Custom simple logger** - Minimal dependencies
- **Standard output only** - No logging framework

**Your Answer:** `[CPP_LOGGING]`

**Impact:** Affects debugging capabilities and diagnostics.

---

### 13.2 Logging Framework (C#)
**Question:** Which logging framework do you want for the C# UI?

**Options:**
- **Serilog** (Recommended) - Structured logging
- **NLog** - Mature, flexible
- **Microsoft.Extensions.Logging** - Built-in .NET logging
- **Console only** - No logging framework

**Your Answer:** `[CSHARP_LOGGING]`

**Impact:** Affects debugging and error tracking.

---

### 13.3 Error Handling Strategy
**Question:** How do you want to handle errors across the C++/C# boundary?

**Options:**
- **Error codes** - Return codes checked in C#
- **Exceptions with translation** - C++ exceptions converted to C# exceptions
- **Result types** - Monadic result objects
- **Callbacks** - Error callbacks from C++ to C#

**Your Answer:** `[ERROR_HANDLING]`

**Impact:** Determines error handling patterns in code.

---

## Section 14: Performance & Optimization

### 14.1 Performance Profiling
**Question:** Do you want to set up performance profiling tools?

**Options:**
- **Yes** - Include profiling hooks from the start
- **Later** - Add when performance issues arise
- **No** - Not needed

**Your Answer:** `[PROFILING]`

**Impact:** Determines if profiling code is included.

---

### 14.2 Memory Management
**Question:** What memory management approach for the C++ core?

**Options:**
- **Modern C++ (smart pointers)** (Recommended) - RAII, shared_ptr, unique_ptr
- **Manual management** - Traditional new/delete
- **Custom allocators** - Optimized memory pools
- **Mix** - Smart pointers with some manual management

**Your Answer:** `[MEMORY_MGMT]`

**Impact:** Affects code safety and modern C++ usage.

---

## Section 15: Initial Feature Set

### 15.1 MVP Features
**Question:** Which features do you want in the initial implementation?

**Options (select multiple):**
- **Basic Pomodoro timer** (25 min work, 5 min break)
- **Customizable intervals** (User-defined work/break times)
- **Session counter** (Track number of pomodoros)
- **Long break** (Extended break after N pomodoros)
- **Pause/Resume**
- **Skip session**
- **Manual task list**
- **Statistics tracking**
- **Settings persistence**

**Your Answer:** `[MVP_FEATURES]` (list selected features)

**Impact:** Determines initial class structure and interfaces.

---

## Summary Template

Once you've answered all questions, provide this summary to Claude Code CLI:

```
Please set up the PomoDojo project with the following configuration:

BUILD SYSTEM:
- C++ Build System: [BUILD_SYSTEM]
- C++ Standard: [CPP_STANDARD]
- .NET Version: [DOTNET_VERSION]

USER INTERFACE:
- Framework: [UI_FRAMEWORK]
- Pattern: [UI_PATTERN]

INTEROP:
- Method: [INTEROP_METHOD]
- Serialization: [SERIALIZATION]

C++ CORE:
- Timer: [TIMER_IMPL]
- State Management: [STATE_MANAGEMENT]
- Config Storage: [CONFIG_STORAGE]
- Memory Management: [MEMORY_MGMT]

TESTING:
- C++ Framework: [CPP_TEST_FRAMEWORK]
- C# Framework: [CSHARP_TEST_FRAMEWORK]
- Integration Tests: [INTEGRATION_TESTS]

DEPENDENCIES:
- C++ Manager: [CPP_DEPS]
- C# Manager: [CSHARP_DEPS]

DEVELOPMENT:
- IDE Support: [IDE_SUPPORT]
- Code Formatting: [CODE_FORMATTING]
- Static Analysis: [STATIC_ANALYSIS]

CI/CD:
- Platform: [CI_PLATFORM]
- Build Configs: [BUILD_CONFIGS]

DOCUMENTATION:
- API Docs: [API_DOCS]
- User Docs: [USER_DOCS]

STRUCTURE:
- Repository: [REPO_STRUCTURE]
- Source Organization: [SOURCE_ORG]

FEATURES:
- Notifications: [NOTIFICATIONS]
- Audio: [AUDIO_ALERTS]
- Persistence: [DATA_PERSISTENCE]

DEPLOYMENT:
- Platforms: [PLATFORM_1], [PLATFORM_2], [PLATFORM_3]
- Distribution: [DISTRIBUTION]

LOGGING:
- C++ Logging: [CPP_LOGGING]
- C# Logging: [CSHARP_LOGGING]
- Error Handling: [ERROR_HANDLING]

PERFORMANCE:
- Profiling: [PROFILING]

MVP FEATURES:
[MVP_FEATURES]

Please create:
1. Complete directory structure
2. Build configuration files
3. Code skeleton with proper namespaces and class stubs
4. Interface definitions for C++/C# interop
5. Basic project files (.sln, .csproj, CMakeLists.txt, etc.)
6. Configuration templates
7. README with build and run instructions
8. Git configuration (.gitignore)
```

---

## Next Steps After Setup

After Claude Code CLI generates the project structure, you should:

1. **Verify the build**
   - Build the C++ core
   - Build the C# UI
   - Verify interop layer compiles

2. **Run initial tests**
   - Ensure test frameworks are working
   - Run example tests

3. **Review documentation**
   - Check README for accuracy
   - Update any project-specific details

4. **Set up your development environment**
   - Open project in your chosen IDE
   - Configure debugger
   - Set up source control

5. **Plan first iteration**
   - Identify first feature to implement
   - Create development branch
   - Start coding!

---

## Example Prompt for Claude Code CLI

Here's an example of how to use this guide:

```
I'm setting up PomoDojo based on the decision guide. Here are my choices:

BUILD: CMake, C++20, .NET 8
UI: Avalonia UI with MVVM
INTEROP: P/Invoke with C API
C++ CORE: std::chrono timer, State Machine pattern, JSON config, smart pointers
TESTING: Google Test, xUnit, Yes to integration tests
DEPS: vcpkg for C++, NuGet for C#
DEV: Visual Studio Code, yes to clang-format, yes to static analysis
CI/CD: GitHub Actions, Debug and Release configs
DOCS: Both Doxygen and XML comments, Comprehensive user docs
STRUCTURE: Monorepo, Component-based organization
FEATURES: OS native notifications, yes to audio, full analytics
DEPLOYMENT: Priority - Windows, macOS, Linux; Distribution - Multiple methods
LOGGING: spdlog for C++, Serilog for C#, exceptions with translation
PROFILING: Later
MVP: Basic Pomodoro, Customizable intervals, Long break, Pause/Resume, Settings persistence

Please create the complete project structure and code skeleton.
```
