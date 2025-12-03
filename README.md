# PomoDojo

A cross-platform productivity application leveraging the Pomodoro Technique to enhance focus management and prevent burnout.

## Overview

PomoDojo is designed to help users improve their time management and concentration by implementing structured focus intervals with automated break reminders. The application targets students, professionals, and individuals with ADHD or other attention-focusing challenges.

## Features (Scope)

The following features are planned for implementation:

- **Timer Controls**: Start, Pause, Resume, and Stop functionality
- **Default Pomodoro Timer**: Standard timer with configurable break durations
- **Customizable Timer Lengths**: User-adjustable focus and break intervals
- **Automatic Long Break**: Triggers after a set number of Pomodoro cycles
- **Auto-Start Next Session**: Optional automatic session progression
- **Notification Alerts**: Audio/visual alerts when a session ends

## Architecture

PomoDojo implements a **Multi-Tier Architecture** with clear separation of concerns:

```
┌─────────────────────────────────────────┐
│           Frontend Layer                │
│    C# Desktop UI (WPF/WinForms)         │
│    Timer • Settings • Statistics        │
├─────────────────────────────────────────┤
│           Interface Layer               │
│            P/Invoke                     │
│    Platform Invocation Services         │
├─────────────────────────────────────────┤
│            Logic Layer                  │
│      C++ Native DLL (Unmanaged)         │
│  Timer Logic • State Management         │
│  Sessions • Configuration • Notifications│
├─────────────────────────────────────────┤
│            Data Layer                   │
│    Database • File Storage              │
│    Preferences • Logs                   │
└─────────────────────────────────────────┘
```

### Technology Stack

| Layer | Technology |
|-------|------------|
| Frontend | C# (WPF/WinForms) |
| Interface | P/Invoke (Platform Invocation Services) |
| Backend | C++ Native DLL |
| Data Storage | Database + File System |

## Core Components

### C++ Backend Modules

**Timer Management**
- **Timer Core**: Countdown operations, start/pause/resume/stop functionality
- **Synchronization Handler**: Timing precision within 200ms tolerance
- **Time Utilities**: Timestamp generation and duration formatting

**Session Control**
- **Session Manager**: Focus and break session lifecycles, history tracking
- **State Controller**: Finite state machine for application states
- **Transition Handler**: Manages session type transitions

**Data Management**
- **Configuration Manager**: User preferences with validation
- **Data Persistence**: Database operations for session storage

**System Services**
- **Notification Manager**: System notifications for session events
- **Event Dispatcher**: Event propagation across components
- **Error Handler**: Centralized logging and recovery

### C# Presentation Components

- **Main Window Controller**: Primary navigation and view coordination
- **Timer Display**: Visual timer with progress indicators
- **Settings View**: Configuration interface for timer durations and preferences
- **Notification System**: Alerts for session transitions with response handling

### Interface Layer (P/Invoke)

P/Invoke (Platform Invocation Services) enables the C# frontend to call functions exported from the native C++ DLL:

- **Timer Functions**: Start, pause, resume, stop timer operations
- **Session Functions**: Session lifecycle management
- **Configuration Functions**: Get/set user preferences
- **Event Callbacks**: Delegates for timer events and notifications
- **Data Marshalling**: Automatic type conversion between managed and native code

## Data Flow

1. User interactions in the C# UI trigger commands
2. Commands pass through P/Invoke to native C++ DLL functions
3. The C++ core processes business logic and state changes
4. State updates and events propagate back via callbacks
5. UI reflects the new state to the user

## Design Patterns

- **Model-View-Presenter (MVP)**: Clean separation between timing engine and presentation
- **Finite State Machine**: Application state management
- **Event-Driven Architecture**: Asynchronous communication between layers
- **P/Invoke Interop**: Native DLL calls for C++/C# language interoperability

## Interface Design Principles

- **Event-Driven Communication**: Asynchronous events prevent blocking
- **Type Safety**: Strong typing ensures data integrity across language boundaries
- **Performance Optimization**: Zero-copy transfers where possible

## User Workflow

### Focus Session Flow
```
Start Focus Session → Display Countdown → Session Complete? 
    ↓ No                                      ↓ Yes
    └──────────────────────────────────────→ Alert User → Start Break? 
                                                              ↓ No → End Session
                                                              ↓ Yes → Move to Break
```

### Break Session Flow
```
Start Break → Display Countdown → Break Complete? 
    ↓ No                              ↓ Yes
    └────────────────────────────→ Alert User → Start Next Session?
                                                    ↓ No → End Session
                                                    ↓ Yes → Next PomoDojo Session
```

## Glossary

| Term | Definition |
|------|------------|
| **Pomodoro Technique** | Time management method using focused work intervals followed by breaks |
| **Focus Session** | A timed work period (default: 25 minutes) |
| **Short Break** | A brief rest period between focus sessions (default: 5 minutes) |
| **Super Break** | An extended break triggered after multiple Pomodoro intervals |
| **Session Cycle** | A complete set of focus sessions and breaks |

## Future Development

Planned features for future releases:
- In-application event scheduler
- Calendar integration (Google Calendar, iOS Calendar)
- Cross-platform mobile support

## Team

**Prepared for:**  
Texas A&M University - Victoria  
Professor Wenjuan Huang  
COSC 4320 – Fall 2025

**Developers:**
- Ablasse Kingcaid-Ouedraogo
- Sebastian Reyna
- Praise Fisher-Afolabi

## License

This project is developed as part of academic coursework at Texas A&M University - Victoria.

---

*Document Version: November 8, 2025*