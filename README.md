<div align="center">

# 🎯 PooSee

### **Multi-Session Browser Testing Utility for Windows**

*A compact, polished desktop tool for legitimate website and session testing.*

[![Platform](https://img.shields.io/badge/platform-Windows%2010%20%7C%2011-0078D4?style=flat-square&logo=windows)]()
[![Framework](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet)]()
[![UI](https://img.shields.io/badge/UI-WPF-68217A?style=flat-square)]()
[![Automation](https://img.shields.io/badge/Automation-Playwright-2EAD33?style=flat-square&logo=playwright)]()
[![License](https://img.shields.io/badge/license-MIT-blue?style=flat-square)]()

**Developed by [MiSFiT-SeCuRiTY](https://github.com/MiSFiT-SeCuRiTY)**

</div>

---

## 📖 Table of Contents

- [What is PooSee?](#-what-is-poosee)
- [Features](#-features)
- [Screenshots](#-screenshots)
- [Requirements](#-requirements)
- [Installation](#-installation)
- [Quick Start](#-quick-start)
- [How It Works](#-how-it-works)
- [User Interface Guide](#-user-interface-guide)
- [Settings Reference](#-settings-reference)
- [Presets System](#-presets-system)
- [Data Storage](#-data-storage)
- [Architecture](#-architecture)
- [FAQ](#-faq)
- [Troubleshooting](#-troubleshooting)
- [Building from Source](#-building-from-source)
- [Project Structure](#-project-structure)
- [Security & Safety](#-security--safety)
- [Credits](#-credits)
- [License](#-license)

---

## 🎯 What is PooSee?

**PooSee** is a small, focused Windows desktop utility designed for **legitimate website and session testing**.

You give it one or more URLs. For each URL, you set:
- A **minimum** refresh interval (in seconds)
- A **maximum** refresh interval (in seconds)

Then you tell PooSee how many browser **sessions** to launch. Each session:
- Opens in its own **isolated browser profile** (its own cookies, cache, storage)
- Loads its assigned URL in its own tab
- Waits a **random amount of time between the min and max you set**
- Reloads the same tab — no new tab is created
- Repeats forever, until you press **STOP ALL**

PooSee is useful when you need to:
- 🧪 **QA a website's behavior over time** — check that pages reload cleanly, sessions don't leak, memory doesn't balloon
- 🔍 **Verify session isolation** — confirm that two browser contexts don't share cookies or local storage
- 📊 **Monitor page stability** — run a URL for hours under repeated reloads to spot intermittent issues
- 🎓 **Test your own web apps** — simulate multiple users loading your site in parallel
- 🔐 **Security research on properties you own** — study session handling, cookie scoping, and refresh behavior

### 🚫 What PooSee is **not**

PooSee is **not** designed for, and does **not** include:

- ❌ CAPTCHA solving or bypassing
- ❌ Fingerprint spoofing or evasion
- ❌ Anti-bot detection bypassing
- ❌ Proxy rotation for evasion
- ❌ Fake account creation
- ❌ Artificial traffic, views, likes, or engagement
- ❌ Credential harvesting or cookie theft
- ❌ Stealth automation
- ❌ Any mechanism intended to bypass website restrictions

It uses **ordinary, transparent browser behavior** — the same kind of traffic a normal visitor would produce. If a website blocks normal visits, PooSee will be blocked too. That's by design.

---

## ✨ Features

<table>
<tr>
<td width="50%" valign="top">

### 🌐 Multi-URL Jobs
- Add any number of URL jobs
- Each job has its own **min/max refresh interval**
- Enable/disable individual jobs without deleting them
- Optional friendly name for each URL

### 🪟 Isolated Sessions
- Every session gets its own **browser context**
- Separate cookies, local storage, session storage
- Separate profile folder on disk
- Sessions don't leak data between each other

### ⏱️ Independent Timers
- Each session runs its own async refresh loop
- Random delay between min and max
- No two sessions ever sync up
- Uses `async/await` + `CancellationToken` (no `Thread.Sleep`)

</td>
<td width="50%" valign="top">

### 🔍 Smart Browser Detection
- Auto-detects **Google Chrome**
- Auto-detects **Microsoft Edge**
- Auto-detects **Chromium**
- Reads Windows registry + standard install paths
- Shows detected version

### 💾 Presets
- Save your URL configuration as a named preset
- Load presets with one click
- Rename and delete presets
- Presets stored locally, portable between machines

### 🎨 Modern Dark UI
- Custom window chrome with rounded corners
- Charcoal backgrounds, electric-blue accent
- Dark / Light / System theme
- Minimal, uncluttered layout

### 🔔 System Tray
- Minimize to tray on close
- Right-click menu: Show / Start / Stop All / Settings / Exit
- Runs silently in the background

</td>
</tr>
</table>

### 📝 Additional Features

- **Resource-aware session count** — recommends a safe number based on your CPU cores and available RAM
- **Always-on-top mode** — pin the window above all others
- **Automatic logging** — daily rolling logs written via Serilog
- **Session history** — every run recorded to JSON for later analysis
- **Auto-cleanup** — temporary profiles deleted on stop
- **Last-config restore** — remembers your URLs and session count between launches
- **Window state persistence** — remembers size and position

---

## 📸 Screenshots

### Main Window

<img width="880" height="971" alt="Screenshot 2026-10-02 141628" src="https://github.com/user-attachments/assets/24bfe3ea-024e-493d-8064-f482414cedb6" />

### Add URL Job Dialog

<img width="555" height="693" alt="Screenshot 2026-10-02 145111" src="https://github.com/user-attachments/assets/649e4757-1e61-4a00-b8d3-d8aa37416e17" />

### Settings Window

<img width="686" height="781" alt="Screenshot 2026-10-02 145141" src="https://github.com/user-attachments/assets/09add80f-18f3-498e-aaba-2f8d4ff2a7c4" />
<img width="672" height="772" alt="Screenshot 2026-10-02 145149" src="https://github.com/user-attachments/assets/fbebee1f-5ddc-40f4-af16-f220d1eb3e50" />

### Running Sessions

<img width="846" height="445" alt="Screenshot 2026-10-02 145316" src="https://github.com/user-attachments/assets/bb3fc630-d999-4e11-86cd-5a88e350a080" />

---

## 💻 Requirements

| Item | Requirement |
|---|---|
| **OS** | Windows 10 or Windows 11 (x64) |
| **Architecture** | x64 (ARM64 build also available) |
| **Runtime** | **None** — the release is self-contained |
| **Browser** | Google Chrome, Microsoft Edge, or Chromium (any recent version) |
| **Disk** | ~200 MB for the app + ~150 MB for Playwright's Chromium driver |
| **RAM** | 2 GB minimum, 4 GB+ recommended for 5+ sessions |

> 💡 **No .NET installation required.** PooSee ships with the .NET runtime bundled into the executable.

---

## 🚀 Installation

### Step 1 — Download

Download the latest release from:

```
https://github.com/MiSFiT-SeCuRiTY/PooSee/releases
```
Pick the file named like:

```
PooSee.zip
```
### Step 2 — Extract

Extract the ZIP to any folder you like — for example:

```
C:\PooSee\
```
**Do not run PooSee directly from inside the ZIP.** Windows Explorer's ZIP viewer will run it, but Playwright can't find its driver files. Always extract first.

After extraction, you'll have:

```
C:\PooSee
├── PooSee.exe ← the app
├── .playwright\ ← Playwright driver (required)
├── playwright.ps1 ← one-time browser installer
└── README.md ← this file
````
### Step 3 — One-time browser installation

PooSee uses **Microsoft Playwright** to control browsers. Playwright needs a small driver download the very first time.

1. Open the extracted folder
2. **Shift + Right-click** inside the folder → **Open PowerShell window here**
   - On Windows 11: right-click → **Open in Terminal**
3. Run:

```powershell
.\playwright.ps1 install chromium
````
This downloads \~150 MB of Chromium driver files into:
```
%USERPROFILE%\AppData\Local\ms-playwright\
```
Wait for it to finish. It only needs to run once.

### Step 4 — Run PooSee

Double-click `PooSee.exe`.

> ⚠️ **Windows SmartScreen warning** — because PooSee is unsigned, Windows may show "Windows protected your PC". Click **More info** → **Run anyway**. This is normal for unsigned open-source apps.

PooSee opens. You're ready.

---

## 🎬 Quick Start

Let's run your first test in 60 seconds.

### 1️⃣ Add a URL

Click **Add URL** in the main window. A dialog appears:
```
URL:              https://example.com
Friendly name:    (optional)
Minimum interval: 30 seconds
Maximum interval: 60 seconds
Enabled:          ☑
```
Click **Save**. The URL appears in the **URL JOBS** panel.

### 2️⃣ Set the session count

The **Sessions:** box at the top-right of the window shows a suggested number. For your first test, keep it small — `2` or `3`.

> 💡 **Recommended:** value shows what PooSee thinks your PC can comfortably handle based on CPU and RAM.

### 3️⃣ Start

Click **START**.

PooSee will:

1. Validate your URLs
2. Detect a browser
3. Launch Playwright
4. Open one isolated browser window per session
5. Navigate each one to your URL
6. Start the refresh timers

### 4️⃣ Watch

The **SESSIONS** panel populates:
```
● Session 01   https://example.com   Running   43s
● Session 02   https://example.com   Running   17s
```
The `43s` / `17s` are countdowns — the seconds remaining until the next reload.

Each session reloads its **same tab** when its timer fires. No new tabs, ever.

### 5️⃣ Stop

Click **STOP ALL**. PooSee:

1. Cancels all timers
2. Closes all browser pages
3. Closes all contexts
4. Closes the browser
5. Deletes temporary profile folders
6. Writes a session history entry

Browsers close cleanly. No orphan processes are left behind.

---

## 🧠 How It Works

### The Big Picture
```
                      ┌──────────────────────┐
                      │     PooSee.exe       │
                      │  (WPF Desktop App)   │
                      └──────────┬───────────┘
                                 │
                    ┌────────────┴────────────┐
                    │                         │
             ┌──────▼──────┐         ┌────────▼────────┐
             │  Browser    │         │  Session        │
             │  Service    │◄────────┤  Manager        │
             │ (Playwright)│         │                 │
             └──────┬──────┘         └────────┬────────┘
                    │                         │
                    │              ┌──────────┴─────────┐
                    │              │                    │
             ┌──────▼──────┐  ┌────▼─────┐      ┌───────▼──────┐
             │  Browser    │  │ Session 1│      │  Session N   │
             │  Process    │  │ Context  │ ...  │  Context     │
             └─────────────┘  │ + Page   │      │  + Page      │
                              └────┬─────┘      └──────┬───────┘
                                   │                   │
                                   ▼                   ▼
                              ┌─────────┐         ┌─────────┐
                              │  Timer  │         │  Timer  │
                              │ 20–40s  │         │ 90–150s │
                              └─────────┘         └─────────┘
```
### The Lifecycle

#### 🔵 Start

1. **Validation** — checks every URL is valid HTTP/HTTPS, intervals are positive, min ≤ max
2. **Browser detection** — finds Chrome / Edge / Chromium on disk
3. **Config snapshot** — saves current URL list + session count to disk for next launch
4. **Playwright startup** — `Playwright.CreateAsync()` initialises the driver
5. **Browser launch** — one shared browser process opens
6. **Session distribution** — sessions assigned to URL jobs via round-robin
7. **Per-session setup** (for each):
   - Create isolated profile folder: `Profiles\temp\session-001\`
   - Create isolated browser context: `browser.NewContextAsync()`
   - Open a new page (tab): `context.NewPageAsync()`
   - Navigate to the assigned URL
8. **Start refresh loops** — each session's async timer kicks off

#### 🔁 Refresh Loop (per session)
```
┌──────────────────────────────────┐
│ 1. Pick random delay             │
│    between min and max seconds   │
├──────────────────────────────────┤
│ 2. await Task.Delay(delay, ct)   │
│    (cancellable)                 │
├──────────────────────────────────┤
│ 3. page.ReloadAsync()            │
│    (same tab — no new tab!)      │
├──────────────────────────────────┤
│ 4. Update UI: LastRefresh,       │
│    RefreshCount, NextRefresh     │
├──────────────────────────────────┤
│ 5. Loop back to step 1           │
└──────────────────────────────────┘
```
Uses `Task.Delay(..., cancellationToken)` — **never** `Thread.Sleep`. The UI thread stays responsive. Cancellation is instant when you press STOP ALL.

#### 🔴 Stop All

1. **Cancel** — cancel the shared `CancellationTokenSource`
2. **Wait** — await each session's loop task to finish
3. **Close pages** — `page.CloseAsync()`
4. **Close contexts** — `context.CloseAsync()`
5. **Close browser** — `browser.CloseAsync()`
6. **Delete profiles** — removes `Profiles\temp\session-*` folders
7. **Write history** — appends a record to `history.json`
8. **Update UI** — status returns to Idle

This sequence is **safe to call at any time**, even if some sessions failed to start.

### Session Distribution Logic

Sessions are distributed across URL jobs using a **deterministic round-robin** strategy.

Given **3 URL jobs** and **6 sessions**:

| **Session** | **Assigned URL** |
| :---------- | :--------------- |
| 01          | URL 1            |
| 02          | URL 2            |
| 03          | URL 3            |
| 04          | URL 1            |
| 05          | URL 2            |
| 06          | URL 3            |

The pattern repeats. This gives a predictable, understandable spread. Only **enabled** jobs participate.

### Browser Isolation

Even though PooSee launches **one** browser process for performance, each session gets its own **isolated context**:

- Separate cookies
- Separate localStorage / sessionStorage
- Separate cache
- Separate profile folder on disk
- Separate pages

This is done through Playwright's `Browser.NewContextAsync()` — the same mechanism Chrome Incognito uses internally.

The profile folder on disk (`Profiles\temp\session-XXX\`) is created fresh for each run and deleted after Stop All, unless you've turned on **Persistent Profiles** in Settings.

### Resource Recommendation

PooSee computes a safe session count from your system:
```
recommended = min(cpu_logical_cores, floor(ram_gb / 1.5))
recommended = clamp(recommended, 1, 10)
```
On a 4-core, 8 GB machine → `min(4, 5) = 4`
On a 16-core, 32 GB machine → `min(16, 21) = 10` (capped)

You can always override this in the Sessions box.

---

## 🖥️ User Interface Guide

### Main Window
```
┌─────────────────────────────────────────────────────────────────┐
│ PooSee · Browser Session Testing Utility          [On top] — ▢ ✕ │  ← Custom title bar
├─────────────────────────────────────────────────────────────────┤
│ ● Browser: Google Chrome        Sessions: [ 4 ] Rec: 5          │  ← Status header
│   121.0.6167.85 · C:\Program Files\Google\Chrome\Application... │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  URL JOBS                                          [ + Add URL ] │  ← URL list
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ ☑ example.com — Main site          20–40s   [Edit][Rem] │  │
│  │ ☑ dashboard.internal — QA          90–150s  [Edit][Rem] │  │
│  └──────────────────────────────────────────────────────────┘  │
│                                                                  │
│  SESSIONS                                                        │  ← Session grid
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ Session 01  ● https://example.com   Running     43s     │  │
│  │ Session 02  ● https://example.com   Running     17s     │  │
│  │ Session 03  ● https://dashboard...  Running     08s     │  │
│  │ Session 04  ● https://dashboard...  Running     112s    │  │
│  └──────────────────────────────────────────────────────────┘  │
│                                                                  │
├─────────────────────────────────────────────────────────────────┤
│ Idle             [Presets] [Settings] [About]   [START] [STOP] │  ← Footer
└─────────────────────────────────────────────────────────────────┘
```
### Element Reference

| **Element**     | **Purpose**                                            |
| :-------------- | :----------------------------------------------------- |
| **On top**      | Toggle always-on-top                                   |
| **Browser:**    | Shows detected browser + version + path                |
| **Sessions:**   | Override the session count (1 – maximum from settings) |
| **Rec:**        | The auto-recommended session count                     |
| **Add URL**     | Opens the URL job editor                               |
| **URL row**     | Checkbox = enable/disable · `Edit` / `Remove` buttons  |
| **Session row** | ID · status dot · URL · status text · countdown        |
| **Status dots** | 🟢 Running · 🟡 Starting · 🔴 Failed · ⚪ Idle          |
| **START**       | Begins all sessions                                    |
| **STOP ALL**    | Cancels and closes everything                          |

### Status Indicators

| **Color**     | **Meaning**                    |
| :------------ | :----------------------------- |
| 🟢 **Green**  | Session is running normally    |
| 🟡 **Yellow** | Session is starting up         |
| 🔴 **Red**    | Session failed (check the log) |
| ⚪ **Grey**    | Session is idle or stopped     |

---

## ⚙️ Settings Reference

Open **Settings** from the footer.

### 🌐 Browser

| **Setting**                     | **Description**                                                                       |
| :------------------------------ | :------------------------------------------------------------------------------------ |
| **Automatic browser selection** | If on, PooSee picks the first available browser. If off, uses your preferred channel. |
| **Detected browsers**           | Read-only list of what PooSee found on your system                                    |

### 📁 Profile Location

| **Setting**                                   | **Description**                                                                             |
| :-------------------------------------------- | :------------------------------------------------------------------------------------------ |
| **Base folder**                               | Where PooSee creates temporary browser profiles. Default: `%LocalAppData%\PooSee\Profiles\` |
| **Cleanup temporary profiles after each run** | If on (default), session profiles are deleted after STOP ALL. If off, they persist.         |
| **Use persistent profiles**                   | If on, profiles survive between runs. Not recommended for most testing.                     |

### 🔢 Default Sessions

| **Setting**                               | **Description**                                                |
| :---------------------------------------- | :------------------------------------------------------------- |
| **Automatically recommend session count** | If on, PooSee uses the resource-based recommendation           |
| **Manual default**                        | If recommendation is off, use this number for new launches     |
| **Maximum sessions**                      | Hard cap. Prevents accidental resource exhaustion. Default: 20 |

### 🚀 Startup Behavior

| **Setting**                                  | **Description**                                        |
| :------------------------------------------- | :----------------------------------------------------- |
| **Start minimized**                          | Launch window minimized instead of normal              |
| **Start in system tray**                     | Launch with no window — only the tray icon             |
| **Restore previous configuration on launch** | Load the URLs and session count from your last session |
| **Restore previous window state**            | Remember size and position                             |
| **Minimize to system tray on close**         | Clicking ✕ hides the window instead of exiting         |
| **Always on top**                            | Pin above all other windows                            |

### 🎨 Appearance

| **Setting** | **Description**       |
| :---------- | :-------------------- |
| **Theme**   | Dark / Light / System |

---

## 💾 Presets System

Presets let you save your complete URL configuration as a named snapshot.

### Creating a preset

1. Configure your URLs as you'd like them saved
2. Click **Presets** in the footer
3. Click **Save current as preset**
4. Enter a name (e.g., "Client QA — Production")
5. Click **OK**

### Loading a preset

1. Click **Presets**
2. Click a preset in the list to select it
3. Click **Load**
4. Your URL JOBS panel is replaced with the preset's configuration
5. The preset window closes automatically

### Renaming / deleting

- Select a preset → click **Rename** → enter a new name → OK
- Select a preset → click **Delete** → confirm (no prompt currently — be careful)

### Where presets are stored
```
%LocalAppData%\PooSee\Presets\presets.json
```
You can back this file up, copy it to another machine, or commit it to source control if you want to share configurations with a team.

---

## 📂 Data Storage

All PooSee data lives under:
```
%LocalAppData%\PooSee\
```
> **Quick access:** press **Win + R**, type `%LocalAppData%\PooSee`, press Enter.

### Folder layout
```
%LocalAppData%\PooSee\
│
├── Settings\
│   └── appsettings.json       ← all user preferences
│
├── Presets\
│   └── presets.json           ← saved URL configurations
│
├── History\
│   └── history.json           ← session history (last 200 entries)
│
├── Logs\
│   └── poosee-2025-10-02.log  ← daily rolling logs (14 days retained)
│
└── Profiles\
    ├── temp\                  ← temporary profiles, auto-cleaned
    │   ├── session-001\
    │   ├── session-002\
    │   └── ...
    └── persistent\            ← only if "Persistent Profiles" is on
        ├── session-001\
        └── ...
```
### Resetting PooSee

To reset everything to defaults:

1. Exit PooSee (tray → Exit)
2. Delete `%LocalAppData%\PooSee\`
3. Relaunch

Or delete only specific parts:

- **Reset settings only** — delete `Settings\appsettings.json`
- **Reset presets only** — delete `Presets\presets.json`
- **Reset history only** — delete `History\history.json`

### Viewing logs

Logs are plain text. Open with any text editor:
```
%LocalAppData%\PooSee\Logs\poosee-2025-10-02.log
```
Example:
```
2025-10-02 17:30:02 [INF] Application started
2025-10-02 17:30:03 [INF] Browser detected: Google Chrome (121.0.6167.85)
2025-10-02 17:30:04 [INF] Configuration validated
2025-10-02 17:30:05 [INF] Starting Playwright. Browser: Google Chrome
2025-10-02 17:30:06 [INF] Browser launched.
2025-10-02 17:30:06 [DBG] Profile created: ...\session-001
2025-10-02 17:30:07 [INF] Session 01 started → https://example.com
2025-10-02 17:30:07 [INF] Session 02 started → https://example.com
2025-10-02 17:31:02 [DBG] Session 01 refreshed (#1)
2025-10-02 17:31:14 [DBG] Session 02 refreshed (#1)
2025-10-02 17:35:12 [INF] All sessions stopped.
2025-10-02 17:35:12 [INF] Application exiting
```
---

## 🏗️ Architecture

PooSee follows **MVVM** (Model–View–ViewModel) with **dependency injection**.
```
┌────────────────────────────────────────────────────────────┐
│                          Views                             │
│  MainWindow · UrlJobWindow · SettingsWindow · PresetWindow │
│                        (XAML)                              │
└──────────────────┬─────────────────────────────────────────┘
                   │ data-binding
                   ▼
┌────────────────────────────────────────────────────────────┐
│                       ViewModels                           │
│  MainViewModel · SettingsViewModel · PresetViewModel ·     │
│  UrlJobViewModel                                           │
└──────────────────┬─────────────────────────────────────────┘
                   │ method calls
                   ▼
┌────────────────────────────────────────────────────────────┐
│                        Services                            │
│  BrowserService · BrowserDetector · SessionManager ·       │
│  RefreshScheduler · ProfileManager · PresetManager ·       │
│  HistoryService · LogService · SettingsService ·           │
│  SystemResourceService · TrayService                       │
└──────────────────┬─────────────────────────────────────────┘
                   │
                   ▼
┌────────────────────────────────────────────────────────────┐
│                         Models                             │
│  UrlJob · SessionInfo · SessionConfiguration ·             │
│  BrowserInfo · AppSettings · Preset                        │
└────────────────────────────────────────────────────────────┘
```
### Layer responsibilities

| **Layer**          | **Responsibility**                                            | **Never does**              |
| :----------------- | :------------------------------------------------------------ | :-------------------------- |
| **Views**          | Render XAML, capture user input, forward to ViewModel         | Contain business logic      |
| **ViewModels**     | Expose bindable properties, commands, orchestrate services    | Talk to Playwright directly |
| **Services**       | Business logic: browser control, scheduling, storage, logging | Touch the UI                |
| **Models**         | Plain data                                                    | Contain behaviour           |
| **Infrastructure** | Cross-cutting helpers (paths, commands, JSON, DI)             | Business logic              |

### Key design decisions

#### Why MVVM?

- Clean separation of UI from logic
- Testable ViewModels without spinning up a browser
- XAML changes don't touch C#
- C# changes don't touch XAML

#### Why Dependency Injection?

- `Microsoft.Extensions.DependencyInjection`
- Every service registered in `App.xaml.cs`
- Constructor injection everywhere
- Easy to swap implementations (e.g., a mock browser detector for tests)

#### Why Playwright?

- Modern, actively maintained
- Cross-browser support
- First-class .NET API
- Clean async model
- Context isolation built in
- Better than Selenium for this use case

#### Why async/await everywhere?

- The UI thread must stay responsive even with 10+ sessions
- Every browser operation is async
- Every delay uses `Task.Delay(..., ct)`
- Cancellation via `CancellationTokenSource`

#### Why not `Thread.Sleep`?

Because it blocks a thread. With 10 sessions each sleeping for minutes, you'd waste 10 threads. `await Task.Delay` returns the thread to the pool immediately.

### Session manager internals

`SessionManager` is the orchestrator. Simplified pseudocode:
```
StartAsync(config):
    Start browser
    AssignedJobs = RoundRobin.Distribute(sessionCount, jobs)
    
    for i in 0..sessionCount:
        session = SessionInfo(...)
        profile = ProfileManager.CreateProfile(i)
        context = browserService.CreateContext(profile)
        page = context.NewPage()
        page.Goto(job.Url)
        loopTask = Task.Run(() => Scheduler.Run(...))
        runtimes.Add(session, context, page, loopTask)
    
    raise SessionsChanged

StopAllAsync():
    cts.Cancel()
    for runtime in runtimes:
        await runtime.LoopTask
        await runtime.Page.Close()
        await runtime.Context.Close()
        ProfileManager.CleanupAfterSession()
    await browserService.Stop()
    raise SessionsChanged
```
### Scheduler internals
```
RunAsync(min, max, onRefresh, onNextScheduled, ct):
    while not cancelled:
        delay = random(min, max)
        onNextScheduled(now + delay)
        await Task.Delay(delay, ct)
        await onRefresh(ct)
```
Each session gets its own `RunAsync` call.

---

## ❓ FAQ

### General

**Q: What's the difference between a URL job and a session?**
A URL job is a *configuration* — a URL plus its min/max interval. A session is a *live browser context* that runs a URL job. One URL job can be assigned to multiple sessions.

**Q: Can two sessions share the same URL?**
Yes. If you have 3 URL jobs and 6 sessions, two sessions will run each URL (round-robin).

**Q: Does PooSee use my everyday Chrome profile?**
No. PooSee launches Chrome with a *fresh, isolated profile* per session. Your normal browsing, cookies, and logins are untouched.

**Q: Can I log into a website once and have all sessions use that login?**
No — that would defeat isolation. Each session starts fresh. If you need login persistence, use the **Persistent Profiles** setting (Settings → Profile Location) so profiles survive between runs.

**Q: Does PooSee visit any URLs I didn't configure?**
No. It only loads the URLs you explicitly enter.

**Q: Does PooSee send any data to a server?**
No. PooSee makes zero network calls to any server of ours — there is none. The only network traffic is the browser's own traffic to the URLs you configured.

### Sessions

**Q: How many sessions can I run?**
Technically dozens, but practically: each session is a Chrome context, and each uses 100–300 MB of RAM. On a 16 GB machine, \~10–15 sessions is comfortable. PooSee recommends a number based on your system specs.

**Q: Why does my page refresh at different intervals?**
By design. Each refresh picks a **random** delay between your min and max. This is intended for realistic testing — two sessions should never sync up.

**Q: Can I make it refresh at exactly the same interval?**
Set min = max. E.g., min 30, max 30.

**Q: Why does a new browser tab not open on refresh?**
Because that would be wrong. PooSee **reloads the existing tab** via `page.ReloadAsync()`. New tabs are only created when a session starts.

**Q: What happens if a page fails to load?**
That session is marked **Failed** (red dot), an error is logged, and other sessions continue unaffected.

### Presets

**Q: Can I share presets with my team?**
Yes. Send them the file:
```
%LocalAppData%\PooSee\Presets\presets.json
```
They drop it in the same location on their machine.

**Q: Can a preset contain multiple URLs?**
Yes — that's the whole point. A preset captures the whole URL JOBS list.

### Settings

**Q: Where are my settings stored?**
`%LocalAppData%\PooSee\Settings\appsettings.json`

**Q: How do I reset everything?**
Exit PooSee, delete `%LocalAppData%\PooSee\`, relaunch.

**Q: What does "Cleanup temporary profiles after each run" do?**
When on (default), PooSee deletes the browser profile folders it created for the run, once STOP ALL is pressed. This keeps your disk clean. Turn it off if you're debugging and want to inspect the profile data afterwards.

### System Tray

**Q: Why does PooSee keep running after I close the window?**
Because **Minimize to tray on close** is enabled by default. This lets your sessions keep running while the window is hidden.

**Q: How do I fully exit PooSee?**
Right-click the tray icon → **Exit**.

**Q: Where is the tray icon?**
Bottom-right of your taskbar, near the clock. If it's hidden, click the **⌃** arrow to reveal it.

### Technical

**Q: Which browsers does PooSee support?**
Google Chrome, Microsoft Edge, and Chromium. All three are Chromium-based, and PooSee uses Playwright's Chromium driver.

**Q: Does PooSee install its own browser?**
Yes — Playwright bundles a version of Chromium that it manages separately from your system Chrome. When you first run `playwright.ps1 install chromium`, that's what's downloaded.

**Q: Why does PooSee need a one-time browser install?**
Playwright ships as a driver + a small node runtime. The browser binaries themselves are downloaded on demand to keep the initial ZIP smaller.

**Q: Can I run PooSee without Chrome installed?**
Yes — Playwright's bundled Chromium works standalone. But browser detection in PooSee looks for system Chrome/Edge/Chromium by default. You can point it at the Playwright binary in Settings if needed.

**Q: Does PooSee work offline?**
Once browsers are installed, PooSee works fine offline. It only needs internet to reach your target URLs (which are, of course, external).

---

## 🛠️ Troubleshooting

### 🚨 "Driver not found: C:...\node.exe"

**Cause:** Playwright's driver wasn't extracted from the ZIP, or the app was run directly from inside the ZIP file.

**Fix:**

1. Right-click the ZIP → **Extract All** → extract to a real folder (e.g. `C:\PooSee\`)
2. Confirm `.playwright\` exists inside the extracted folder
3. Run `PooSee.exe` from that extracted folder

### 🚨 "Executable doesn't exist at ...chromium-XXXX\chrome.exe"

**Cause:** Playwright's browser binaries aren't installed.

**Fix:** Open PowerShell in the PooSee folder and run:

powershell

```
.\playwright.ps1 install chromium
```
Or copy the `%USERPROFILE%\AppData\Local\ms-playwright\` folder from a machine where it works to the same location on this machine.

### 🚨 "No browser detected"

**Cause:** PooSee couldn't find Chrome, Edge, or Chromium.

**Fix:** Install one of:

- [Google Chrome](https://www.google.com/chrome/)
- [Microsoft Edge](https://www.microsoft.com/edge) — usually preinstalled on Windows 10/11

Then relaunch PooSee.

### 🚨 "Windows protected your PC" (SmartScreen)

**Cause:** PooSee isn't code-signed.

**Fix:** Click **More info** → **Run anyway**. This appears because Microsoft charges for signing certificates. For a small utility, unsigned binaries are common.

### 🚨 A session fails to navigate

**Common causes:**

- URL requires a login
- URL requires a corporate VPN
- URL is down
- Network is disconnected
- Page uses an unsupported TLS version

Check the log file for the exact reason. Other sessions continue running.

### 🚨 App is slow / UI freezes

**Cause:** Too many sessions for your system.

**Fix:**

1. Reduce session count
2. Or check the log for a single session hogging CPU
3. Make sure you're not running PooSee on battery saver mode

### 🚨 Profile folders not cleaned up

**Cause:** Chrome or Edge may still be holding file locks after being told to close.

**Fix:** PooSee retries 4 times with short delays. If folders persist, exit PooSee, wait 10 seconds, and manually delete `%LocalAppData%\PooSee\Profiles\temp\`.

### 🚨 Log file is huge

**Cause:** Verbose debug logging.

**Fix:** The log rolls daily and retains 14 days. Older files are auto-deleted. If you want fewer logs, reduce verbosity by editing `LogService.cs` and rebuilding.

### 🚨 "Cannot create profile" errors

**Cause:** Disk full or permissions issue on `%LocalAppData%`.

**Fix:** Change the **Profile Location** in Settings to a folder you have full control over, e.g. `C:\PooSeeProfiles\`.

### 🚨 Session runs but page never refreshes

**Cause:** Possible if the page has beforeunload handlers that block reload, or an extension interferes.

**Fix:**

1. Try `https://example.com` — if that works, the problem is site-specific
2. Check the log — Playwright logs the exact error
3. Try a different browser in Settings

---

## 🧱 Building from Source

### Prerequisites

- **Visual Studio 2022** (17.8+) — [download](https://visualstudio.microsoft.com/)
  - During install, tick **".NET desktop development"**
- **.NET 10 SDK** — [download](https://dotnet.microsoft.com/download/dotnet/10.0)

Verify:

powershell

```
dotnet --version
```
Should print `10.x.x`.

### Clone and build

powershell

```
git clone https://github.com/MiSFiT-SeCuRiTY/PooSee.git
cd PooSee\src\PooSee
dotnet build
```
Or open `PooSee.sln` in Visual Studio and press **F6**.

### Run from source

powershell

```
dotnet run --project .\PooSee.csproj
```
Or press **F5** in Visual Studio.

### Install Playwright browsers

Once from the project folder:

powershell

```
dotnet build
& ".\bin\Debug\net10.0-windows\playwright.ps1" install chromium
```
### Run tests

powershell

```
dotnet test
```
Unit tests cover URL validation, interval math, session distribution, and preset serialization.

---

## 📤 Publishing a Release

To produce a standalone single-file EXE:

powershell

```
dotnet publish .\PooSee.csproj `
  -c Release `
  -r win-x64 `
  --self-contained true `
  --output C:\Dev\PooSee\publish `
  /p:PublishSingleFile=true `
  /p:IncludeNativeLibrariesForSelfExtract=true `
  /p:EnableCompressionInSingleFile=true
```
Output lands in `C:\Dev\PooSee\publish\`:
```
PooSee.exe              ← single-file app (~99 MB)
.playwright\            ← Playwright driver
playwright.ps1          ← browser installer helper
PooSee.pdb              ← debug symbols (safe to delete)
```
### Trim for distribution

The `.playwright\node\` folder contains driver binaries for macOS and Linux too. Delete everything except `win32_x64\`:

powershell

```
$keep = "C:\Dev\PooSee\publish\.playwright\node\win32_x64"
Get-ChildItem "C:\Dev\PooSee\publish\.playwright\node" -Directory |
    Where-Object { $_.FullName -ne $keep } |
    Remove-Item -Recurse -Force
```
### Create the release ZIP

powershell

```
Compress-Archive -Path "C:\Dev\PooSee\publish\*" `
                 -DestinationPath "C:\Dev\PooSee\PooSee-v1.0.0-win-x64.zip" `
                 -Force
```
Upload the resulting ZIP to GitHub Releases.

### ARM64 build (optional)

Repeat the publish command with `-r win-arm64` and a separate `--output` folder.

---

## 📁 Project Structure
```
PooSee/
├── PooSee.sln
├── Directory.Build.props
├── README.md
├── LICENSE
├── .gitignore
│
└── PooSee/
    ├── PooSee.csproj
    ├── App.xaml                  ← resource dictionary wiring
    ├── App.xaml.cs               ← DI container, startup, tray wiring
    │
    ├── Assets/
    │   ├── app.ico               ← application icon
    │   └── logo.svg              ← vector logo
    │
    ├── Infrastructure/
    │   ├── AppPaths.cs           ← resolves %LocalAppData%\PooSee\...
    │   ├── ObservableObject.cs   ← INotifyPropertyChanged base
    │   ├── RelayCommand.cs       ← ICommand (sync) + generic variant
    │   ├── AsyncRelayCommand.cs  ← ICommand (async)
    │   ├── JsonFileStore.cs      ← atomic JSON read/write
    │   └── ThemeManager.cs       ← dark/light/system switcher
    │
    ├── Models/
    │   ├── UrlJob.cs
    │   ├── SessionConfiguration.cs
    │   ├── SessionInfo.cs
    │   ├── BrowserInfo.cs
    │   ├── AppSettings.cs
    │   ├── Preset.cs
    │   └── SessionHistoryEntry.cs
    │
    ├── Services/
    │   ├── ILogService.cs / LogService.cs
    │   ├── ISystemResourceService.cs / SystemResourceService.cs
    │   ├── IBrowserDetector.cs / BrowserDetector.cs
    │   ├── IProfileManager.cs / ProfileManager.cs
    │   ├── ISettingsService.cs / SettingsService.cs
    │   ├── IPresetManager.cs / PresetManager.cs
    │   ├── IHistoryService.cs / HistoryService.cs
    │   ├── ISessionDistributionStrategy.cs / RoundRobinStrategy.cs
    │   ├── IBrowserService.cs / BrowserService.cs
    │   ├── IRefreshScheduler.cs / RefreshScheduler.cs
    │   ├── ISessionManager.cs / SessionManager.cs
    │   ├── ITrayService.cs / TrayService.cs
    │
    ├── ViewModels/
    │   ├── MainViewModel.cs
    │   ├── SettingsViewModel.cs
    │   ├── PresetViewModel.cs
    │   └── UrlJobViewModel.cs
    │
    ├── Views/
    │   ├── MainWindow.xaml(.cs)
    │   ├── UrlJobWindow.xaml(.cs)
    │   ├── SettingsWindow.xaml(.cs)
    │   ├── PresetWindow.xaml(.cs)
    │   ├── NamePromptWindow.xaml(.cs)
    │   └── AboutWindow.xaml(.cs)
    │
    └── Resources/
        ├── Colors.xaml           ← palette
        ├── Styles.xaml           ← buttons, textboxes, checkboxes
        ├── Icons.xaml            ← vector icon geometries
        └── Templates.xaml        ← data templates
```
---

## 🔒 Security & Safety

### Design principles

PooSee was built **explicitly** to be a transparent testing tool. It does not contain code whose purpose is to deceive, evade, or attack.

### What PooSee will never do

- Collect telemetry or analytics
- Send data to any server other than the URLs you configure
- Read or modify files outside `%LocalAppData%\PooSee\` (or your custom profile folder)
- Interact with other browser processes
- Kill processes it didn't start
- Modify your system registry, startup items, or services
- Install anything outside its own folder and the Playwright browser cache

### Your data

Everything is local. You can inspect every file:

- `Settings\appsettings.json` — plain JSON
- `Presets\presets.json` — plain JSON
- `History\history.json` — plain JSON
- `Logs\*.log` — plain text

There is no encryption, no obfuscation, and no secrets.

### Responsible use

Use PooSee only on websites you own or are explicitly authorized to test. Respect `robots.txt`, terms of service, and rate limits. Do not use it for artificial traffic, ad fraud, or anything that would violate a website's rules.

If you're testing a third-party site, get written permission first. When in doubt, don't.

---

## 🙏 Credits

**PooSee** was designed and developed by:

<div align="center">

### **MiSFiT-SeCuRiTY**

[https://img.shields.io/badge/GitHub-MiSFiT--SeCuRiTY-181717?style=for-the-badge&logo=github](https://img.shields.io/badge/GitHub-MiSFiT--SeCuRiTY-181717?style=for-the-badge\&logo=github)

</div>

### Built with

- [**.NET 10**](https://dotnet.microsoft.com/) — application runtime
- [**WPF**](https://learn.microsoft.com/dotnet/desktop/wpf/) — desktop UI framework
- [**Microsoft Playwright**](https://playwright.dev/dotnet/) — browser automation
- [**Serilog**](https://serilog.net/) — logging
- [**Microsoft.Extensions.DependencyInjection**](https://learn.microsoft.com/dotnet/core/extensions/dependency-injection) — DI container
- [**WinForms NotifyIcon**](https://learn.microsoft.com/dotnet/api/system.windows.forms.notifyicon) — system tray integration

### Trademark notice

PooSee is **not affiliated with** Google, Microsoft, Chrome, Edge, Playwright, or any of the above projects. All trademarks belong to their respective owners.

---

<div align="center">
⭐ If PooSee is useful to you, consider starring the repo ⭐

Made with ❤️ by MiSFiT-SeCuRiTY

</div> ```
