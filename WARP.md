# WARP.md

This file provides guidance to WARP (warp.dev) when working with code in this repository.

## Project Overview

PomoDojo is a cross-platform Pomodoro timer application using a **hybrid C++/C# architecture**:
- **C++ Native DLL**: Core timer logic, state management, and business rules
- **C# WPF/WinForms**: Desktop UI and presentation layer
- **P/Invoke**: Interface layer enabling C# to call native C++ functions

The project follows a **Multi-Tier Architecture** with strict separation between presentation (C#), business logic (C++), and data persistence layers.

## Project Status

⚠️ **Early Development Stage**: This repository currently contains architectural documentation only. Source code implementation has not yet begun.

## Planned Directory Structure

When implementing the codebase, follow this organization:

```
PomoDojo/
├── src/
│   ├── backend/          # C++ native DLL
│   │   ├── timer/        # Timer core, synchronization
│   │   ├── session/      # Session manager, state controller
│   │   ├── data/         # Configuration, persistence
│   │   └── services/     # Notifications, events, error handling
│   ├── frontend/         # C# desktop application
│   │   ├── Views/        # WPF/WinForms UI components
│   │   ├── Controllers/  # View coordination and navigation
│   │   └── Interop/      # P/Invoke declarations and marshalling
│   └── shared/           # Common headers and interfaces
├── tests/
│   ├── backend/          # C++ unit tests
│   └── frontend/         # C# unit tests
└── docs/                 # Additional documentation
```

## Build System

### C++ Backend (Native DLL)

**Recommended Build System**: CMake or Visual Studio solution (.sln)

```bash
# CMake approach (cross-platform)
mkdir build && cd build
cmake ..
cmake --build .

# Or Visual Studio
msbuild PomoDojo.sln /p:Configuration=Release
```

### C# Frontend

```bash
# Build the C# project
dotnet build

# Run the application
dotnet run --project src/frontend/PomoDojo.csproj

# Run tests
dotnet test
```

## Architecture Guidelines

### C++ Backend Modules

**Timer Management**
- Maintain timing precision within 200ms tolerance
- Use high-resolution timers for countdown operations
- Implement thread-safe synchronization for timer state

**Session Control**
- Implement finite state machine for session states (Focus → Short Break → Long Break)
- Track session history for statistics
- Handle automatic transitions between session types

**Data Management**
- Validate all configuration changes before persisting
- Use database for session history and logs
- Store user preferences with fallback defaults

**System Services**
- Use platform-native notification APIs
- Implement event dispatcher for cross-component communication
- Centralize error logging with severity levels

### C# Frontend Responsibilities

- **UI only**: No business logic in presentation layer
- All timer operations must call through P/Invoke to C++ backend
- Handle UI events and marshal data to/from native code
- Display notifications and respond to timer callbacks

### P/Invoke Interface Layer

Critical considerations when implementing interop:
- Export C++ functions with `extern "C"` and appropriate calling conventions
- Use `[DllImport]` attributes in C# with correct marshalling hints
- Handle memory management carefully (who allocates, who frees)
- Pass structured data as pointers or use COM-style interfaces
- Register C# delegates as callbacks for asynchronous events

Example pattern:
```cpp
// C++ side
extern "C" __declspec(dllexport) int StartTimer(int durationSeconds);
```

```csharp
// C# side
[DllImport("PomoDojo.dll", CallingConvention = CallingConvention.Cdecl)]
public static extern int StartTimer(int durationSeconds);
```

## Design Patterns in Use

- **Model-View-Presenter (MVP)**: Separates UI from business logic
- **Finite State Machine**: Manages application state transitions
- **Event-Driven Architecture**: Asynchronous communication via callbacks
- **P/Invoke Interop**: Cross-language boundary between C# and C++

## Key Development Principles

### Language Boundaries
- **C++ owns**: Timer logic, session state, configuration validation, data persistence
- **C# owns**: UI rendering, user input handling, view coordination
- **Never mix**: Keep business logic out of C#, keep UI concerns out of C++

### Error Handling
- C++ should log errors and return error codes through P/Invoke
- C# should catch exceptions and display user-friendly messages
- Centralize logging in the C++ error handler module

### Performance Considerations
- Minimize P/Invoke calls in tight loops
- Use zero-copy transfers where possible (pass pointers instead of copying data)
- Profile timer precision to ensure 200ms tolerance is maintained

### Testing Strategy
- Unit test C++ modules independently before integration
- Test P/Invoke marshalling with sample data
- Integration tests for full user workflows
- UI tests should verify state display accuracy

## Pomodoro Technique Implementation

**Session Types**:
- **Focus Session**: 25 minutes (default, configurable)
- **Short Break**: 5 minutes (after each focus session)
- **Long Break**: 15-30 minutes (after 4 focus sessions)

**State Transitions**:
```
Idle → Focus → Short Break → Focus → ... → Long Break → Idle
```

**Critical Requirements**:
- Session cannot be skipped without explicit user action
- Timer must persist across brief application interruptions
- Notifications must fire reliably at session boundaries
- User must confirm break transitions (configurable auto-start)

## Academic Context

This is a coursework project for:
- Texas A&M University - Victoria
- COSC 4320 – Fall 2025
- Professor Wenjuan Huang

Development team:
- Ablasse Kingcaid-Ouedraogo
- Sebastian Reyna
- Praise Fisher-Afolabi

## Related Documentation

- See `README.md` for complete feature scope and architecture diagrams
- Refer to the glossary in README.md for Pomodoro technique terminology
