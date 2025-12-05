# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

PomoDojo is a cross-platform productivity application leveraging the Pomodoro Technique to enhance focus management and prevent burnout. The system employs a **Multi-Tier Architecture** with clear separation of concerns across three primary layers:

- **C++ Core Engine** (15+ program units): High-precision timing logic, state management, and data persistence using SQLite
- **C# Presentation Layer** (5+ view components): Cross-platform desktop UI using WPF/WinForms with MVVM pattern
- **C++/CLI Interoperability Layer** (5 adapter components): Event-driven bridge between managed and unmanaged code

The app delivers accurate timing (less than 200ms drift per session), automated break reminders, and productivity tracking for students, professionals, and individuals with ADHD.

## Architecture

### Multi-Tier Architecture

PomoDojo implements a **Multi-Tier Architecture** following the Model-View-Presenter (MVP) pattern:

```
Frontend Layer (C# UI)
    ↕
Interface Layer (C++/CLI Adapter)
    ↕
Logic Layer (C++ Core Engine)
    ↕
Data Layer (SQLite + File Storage)
```

### Architectural Decisions

**Decision: C++/CLI Interoperability (ADR-0002)**
- Primary interop strategy using C++/CLI for Windows
- Fallback to P/Invoke for Linux/macOS if needed
- Event-driven communication between layers
- Minimal marshaling overhead with type-safe data conversion

**Decision: SQLite for Data Persistence**
- Session history and statistics stored in SQLite database
- User preferences stored in configuration files
- Enables efficient querying and data export

**Decision: WPF/WinForms for Desktop UI**
- Cross-platform desktop interface
- MVVM pattern for clean separation
- Native look and feel per platform

### C++ Core Engine Structure (15 program units)

**Timer Management (3 units)**
- Timer Core: Countdown operations, start/pause/resume/stop functionality
- Synchronization Handler: Precision timing with drift measurement (<200ms tolerance)
- Time Utilities: Timestamp generation, duration formatting

**Session Control (3 units)**
- Session Manager: Orchestrates focus/break session lifecycles
- State Controller: Finite state machine for application states
- Transition Handler: Manages transitions between sessions

**Data Management (2 units)**
- Configuration Manager: User preferences with validation
- Data Persistence: SQLite database operations for session storage

**System Services (3 units)**
- Notification Manager: Queues and triggers system notifications
- Event Dispatcher: Event propagation and listener registration
- Error Handler: Centralized logging and recovery

**Utilities (4 units)**
- Input Handler: Validation for timer durations and user input
- Session Models: Data structures for sessions
- Statistics Calculator: Productivity metrics
- Health Monitor: System health checks

### C# Presentation Layer (5 view components)

- **Main Window Controller**: Primary container managing navigation
- **Timer Display**: Visual timer with progress indicators
- **Settings View**: Configuration interface for preferences
- **Statistics Dashboard**: Productivity metrics with charts and data export
- **Notification System**: Overlay notifications for session transitions

### C++/CLI Interoperability Layer (5 adapter components)

- **Timer Adapter**: Synchronizes timer-related data between C++ and C#
- **Session Adapter**: Manages session operations across language boundary
- **Configuration Adapter**: Handles configuration management interface
- **Event Marshaller**: Converts native callbacks to .NET events
- **Data Compatibility Manager**: Type conversions and data marshaling

## Development Commands

*To be added when build system is configured*

The project currently has no build configuration. When implementing:
- Document C++ build commands (likely CMake or Makefile)
- Document C# build commands (likely dotnet CLI or MSBuild)
- Document how to build the interface layer
- Document how to run the complete application
- Document testing procedures for both C++ and C# components

## Target Users & Use Cases

- Students managing study schedules
- Professionals balancing deep work and meetings
- Individuals with ADHD who benefit from structured focus aids

Core workflow: Timed work sessions → Automatic break reminders → Return to work prompts

## Future Roadmap

- Event scheduler for planning focus sessions
- Calendar integrations (Google Calendar, Microsoft Outlook)
- Multi-platform support expansion
