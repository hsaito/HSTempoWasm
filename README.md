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

## License

This repository is licensed under the [MIT](LICENSE) license.
