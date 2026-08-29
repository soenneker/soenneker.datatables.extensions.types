[![](https://img.shields.io/nuget/v/soenneker.datatables.extensions.types.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.datatables.extensions.types/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.datatables.extensions.types/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.datatables.extensions.types/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.datatables.extensions.types.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.datatables.extensions.types/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.datatables.extensions.types/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.datatables.extensions.types/actions/workflows/codeql.yml)

# Soenneker.DataTables.Extensions.Types

A collection of helpful Type extension methods relating to DataTable.js.

## Install

```bash
dotnet add package Soenneker.DataTables.Extensions.Types
```

## Quick start

```csharp
using Soenneker.DataTables.Extensions.Types;

Type type = /* obtain from your application */;
var result = type.ToDataTableColumns();
```

Converts the public instance properties of a type into a list of `DataTableColumn` objects for use with DataTables.js, using the `JsonPropertyNameAttribute` if present.

## What you get

- `DataTablesTypesExtension` — A collection of helpful Type extension methods relating to DataTable.js.

## API at a glance

| API | What it does | Result / important behavior |
| --- | --- | --- |
| `DataTablesTypesExtension.ToDataTableColumns(type)` | Converts the public instance properties of a type into a list of `DataTableColumn` objects for use with DataTables.js, using the `JsonPropertyNameAttribute` if present. | A list of `DataTableColumn` where each column corresponds to a public property that is not marked with `JsonIgnoreAttribute`. The column name will use the value of `JsonPropertyNameAttribute` if present, or fall back to a camelCase version of the property name. |
