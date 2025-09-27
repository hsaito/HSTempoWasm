# HSTempoWasm

This repository contains the source code for HSTempoWasm, a Blazor WebAssembly interval/tempo finder.

## Prerequisite

- [.NET SDK 9.0.100](https://dotnet.microsoft.com/download/dotnet/9.0) or later supporting [Blazor](https://blazor.net)

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

You need to disable the default CodeQL setup in your repository settings:

1. Go to your repository on GitHub
2. Navigate to **Settings** → **Code security and analysis**
3. Find **CodeQL analysis** section
4. If "Default setup" is enabled, click **Configure** → **Advanced**
5. This will disable default setup and allow the custom workflow to run

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
