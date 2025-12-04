# HSTempoWasm

This repository contains the source code for HSTempoWasm, a Blazor WebAssembly interval/tempo finder.

## Prerequisite

- [.NET SDK 10.0.100](https://dotnet.microsoft.com/download/dotnet/10.0) or later supporting [Blazor](https://blazor.net)

## Using HSTempoWasm

### The Official Instance

The official instance of this application can be found at [https://hstempo.hidekisaito.com](https://hstempo.hidekisaito.com).

## Running the Code

Run with the following:

```
dotnet run --project HSTempoWasm/HSTempoWasm.csproj
```

A [Dockerfile](HSTempoWasm/Dockerfile) is provided for running this as a container.

## Running Unit Tests

Unit tests are located in the `HSTempoWasm.Tests` project. To run all tests:

```
dotnet test HSTempoWasm.Tests/HSTempoWasm.Tests.csproj
```

## CodeQL Setup

This repository uses a custom CodeQL analysis workflow. If you encounter issues, here are the common fixes:

### Error: "CodeQL analyses from advanced configurations cannot be processed when the default setup is enabled"

**CRITICAL:** This error means GitHub's default CodeQL setup is still enabled. You **MUST** disable it:

#### Step-by-Step Fix:
1. **Go to**: https://github.com/hsaito/HSTempoWasm/settings/security_analysis
2. **Find**: "Code scanning" section 
3. **Look for**: "CodeQL analysis" (should show "Default setup: Active")
4. **Click**: "Configure" button next to CodeQL analysis
5. **Change**: From "Default" to "Advanced" 
6. **Confirm**: The change (this disables default setup)
7. **Re-run**: Your GitHub Actions workflow

#### Verification:
- After the change, the CodeQL section should show "Advanced setup: Active"
- The custom workflow should then work without upload errors

#### Alternative:
If you can't change settings immediately, you can:
- Rename `codeql-alternative.yml.disabled` to `codeql-alternative.yml`
- Disable the main `codeql-analysis.yml` temporarily
- Use manual trigger: Actions → CodeQL Analysis (Alternative) → Run workflow

### Error: "Resource not accessible by integration"

This is a permissions issue. Ensure that:

1. **Actions permissions** are set correctly:
   - Go to **Settings** → **Actions** → **General**
   - Under "Workflow permissions", select **Read and write permissions**
   - Check **Allow GitHub Actions to create and approve pull requests**

2. **Code security permissions** are enabled:
   - Go to **Settings** → **Code security and analysis**
   - Ensure **Code scanning alerts** are enabled

3. **Repository visibility**: Code scanning works better with public repositories or GitHub Advanced Security enabled for private repositories.

The workflow has been updated to use CodeQL Action v3 (the latest version) with enhanced permissions.

## License

This repository is licensed under the [MIT](LICENSE) license.
