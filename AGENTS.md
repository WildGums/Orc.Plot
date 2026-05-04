# Orc.Plot

Orc.Plot is a WPF control library that extends the functionality of [OxyPlot](http://www.oxyplot.org/). It provides enhanced plot controls, animations, and easing functions built on top of OxyPlot and integrated with the WildGums Orc ecosystem.

Orc.Plot consists of the following projects:

- `Orc.Plot` — Core library with WPF controls, animations, and easing functions.
- `Orc.Plot.Example` — Example WPF application demonstrating library features.
- `Orc.Plot.Tests` — Automated tests including public API verification.

---

## Critical Rules (Read First)

These rules are **non-negotiable**. Violating them causes broken builds, crashes, or downstream breakage.

### 1. Never Edit Generated Files

Files matching `*.generated.cs` are auto-generated.

- **NEVER** manually edit these files

### 2. ABI / API Stability

This project maintains stable ABI / API. Breaking changes break downstream apps.

| Allowed | Never |
|---------|-------|
| Add new overloads | Modify existing signatures |
| Add new methods | Remove public APIs |
| Add new classes | Change return types |

### 3. Tests Are Mandatory

**Building alone is NOT sufficient.** Run tests before claiming completion (see [Commands](#commands)).

The public API snapshot (`PublicApiFacts.Orc_Plot_HasNoBreakingChanges_Async.verified.txt`) must be updated whenever intentional public API changes are made. Never update it to suppress a test failure caused by an unintended change.

### 4. Branch Protection (COMPLIANCE REQUIRED)

**Direct commits to protected branches are a policy violation.**

| Repository | Protected Branches |
|------------|-------------------|
| Orc.Plot | `master` |
| Orc.Plot | `develop` |

**Required workflow:**

1. **Create a feature branch FIRST** — Use naming convention: `feature/issue-NNNN-description`
2. **Make all commits on the feature branch** — Never commit directly to protected branches
3. **Submit a Pull Request** — Target the `develop` branch; changes must be reviewed before merging

```bash
# CORRECT — Always create a feature branch first
git checkout -b feature/issue-1234-fix-description

# NEVER DO THIS — Policy violation
git checkout develop && git commit  # FORBIDDEN

# NEVER DO THIS — Policy violation
git checkout master && git commit  # FORBIDDEN
```

---

## Commands

Single source of truth for all commands:

| Task | Command |
|------|---------|
| **Build** | `dotnet cake --target=build` |
| **Test** | `dotnet cake --target=test` |
| **Build and test** | `dotnet cake --target=buildandtest` |

---

## Architecture & Directories

### Layer Overview

```
Orc.Plot           => WPF controls and animations built on top of OxyPlot
Orc.Plot.Example   => Sample WPF application
Orc.Plot.Tests     => NUnit test project
```

### Directory Guide

| Directory / File | Editable? | Notes |
|-----------------|-----------|-------|
| `*.generated.cs` | No | Leave as-is; auto-generated |
| `deployment/` | No | Deployment / build scripts |
| `src/Orc.Plot/` | Yes | Main library source |
| `src/Orc.Plot/Controls/` | Yes | WPF controls (e.g., `PlotView`) |
| `src/Orc.Plot/Animations/` | Yes | Animation support and easing functions |
| `src/Orc.Plot.Example/` | Yes | Example application |
| `src/Orc.Plot.Tests/` | Yes | Tests |
| `src/Orc.Plot.Tests/*.verified.txt` | Yes | Public API snapshots — update only for intentional API changes |

### Key Namespaces

| Namespace | Purpose |
|-----------|---------|
| `Orc.Plot` | Core controls (`PlotView`) |
| `Orc.Plot.Animations` | Animation engine, easing functions, and settings |

### XMLNS

Use the WildGums XMLNS in XAML:

```xml
xmlns:orcplot="http://schemas.wildgums.com/orc/plot"
```

---

## Writing Code

### Anti-Patterns (Never Do This)

| Anti-Pattern | Why |
|-------------|-----|
| Modifying method signatures | ABI breaking |
| Manual edits to `*.generated.cs` | Overwritten on regenerate |
| Using default parameters in public APIs | ABI breaking |
| **Skipping failing tests** | **Unacceptable — tests must pass** |
| Updating the public API snapshot to hide an unintended API change | Masks a breaking change |

---

## Testing & Debugging

### Running Tests

```bash
dotnet cake --target=test
```

### Tests MUST Pass

> **NON-NEGOTIABLE:** Tests must PASS before claiming completion.
>
> - Do NOT skip failing tests
> - Do NOT claim completion if tests fail
> - Do NOT use `SkipException` to work around failures

### Writing Tests

1. Use NUnit to write tests
2. Create a Facts class for a feature
3. Combine Pascal / Snake case for test methods (e.g. `PlotView_Renders_WithValidData`)

```csharp
[Test]
public void PlotView_Renders_WithValidData()
{
    var plotModel = new PlotModel { Title = "Test" };

    Assert.That(plotModel.Title, Is.EqualTo("Test"));
}
```

**Philosophy:** Tests FAIL when wrong, never skip (except missing hardware).

### Updating the Public API Snapshot

When intentional public API changes are made, update the verified snapshot:

```
src/Orc.Plot.Tests/PublicApiFacts.Orc_Plot_HasNoBreakingChanges_Async.verified.txt
```

Delete the `.verified.txt` file and run the tests once so that VerifyNUnit regenerates it from the new API surface. Commit the updated snapshot alongside the API change.

```bash
# Windows (PowerShell)
Remove-Item src\Orc.Plot.Tests\PublicApiFacts.Orc_Plot_HasNoBreakingChanges_Async.verified.txt
dotnet cake --target=test

# Linux / macOS
rm src/Orc.Plot.Tests/PublicApiFacts.Orc_Plot_HasNoBreakingChanges_Async.verified.txt
dotnet cake --target=test
```

### Debugging Methodology

1. **Establish baseline** — What's the known-good state?
2. **One change at a time** — Verify each change before proceeding
3. **Track changes in a table** — Log what you changed and the result
4. **Platform differences are signals** — If X works and Y fails, the difference IS the answer
5. **Revert if worse** — Don't pile fixes on top of failures

---

## Further Reading

| Topic | Document |
|-------|----------|
| Contributing guidelines | `CONTRIBUTING.md` |
| Documentation portal | https://opensource.wildgums.com |
| OxyPlot documentation | https://oxyplot.readthedocs.io |
