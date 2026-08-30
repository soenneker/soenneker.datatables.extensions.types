[![](https://img.shields.io/nuget/v/soenneker.datatables.extensions.types.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.datatables.extensions.types/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.datatables.extensions.types/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.datatables.extensions.types/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.datatables.extensions.types.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.datatables.extensions.types/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.datatables.extensions.types/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.datatables.extensions.types/actions/workflows/codeql.yml)

# Soenneker.DataTables.Extensions.Types

Builds DataTables column definitions from a .NET row type, including JSON names and optional `DataTableColumnAttribute` configuration.

## Installation

```bash
dotnet add package Soenneker.DataTables.Extensions.Types
```

## Define a row

```csharp
using System.Text.Json.Serialization;
using Soenneker.DataTables.Attributes.Column;

public sealed class CustomerRow
{
    [DataTableColumn(Title = "Customer", Searchable = true, Orderable = true, Order = 0)]
    public required string Name { get; init; }

    [JsonPropertyName("created_at")]
    [DataTableColumn(Title = "Created", Orderable = true, Width = "12rem", Order = 1)]
    public DateTimeOffset CreatedAt { get; init; }

    [JsonIgnore]
    public string? InternalNote { get; init; }
}
```

## Generate columns

```csharp
using Soenneker.DataTables.Extensions.Types;

List<DataTableColumn> columns = typeof(CustomerRow).ToDataTableColumns();
```

The example produces columns whose `Data` values are `name` and `created_at`. Explicit `JsonPropertyName` values win; otherwise the CLR property name is converted with `JsonNamingPolicy.CamelCase`.

`DataTableColumnAttribute.Data` can override the data source entirely. Other attribute values such as title, visibility, CSS class, searchability, ordering, responsive priority, and width are copied to the resulting DTO.

## Selection and ordering

- Public instance properties with public getters are included.
- Indexers and properties ignored by `System.Text.Json` are excluded. `[JsonIgnore(Condition = JsonIgnoreCondition.Never)]` remains included.
- Columns with an explicit non-negative `Order` come first, in ascending order.
- Columns without an explicit order retain their reflection order after explicitly ordered columns.

Make sure the serializer used for row data follows the same names. A custom naming policy can differ from the camel-case fallback; add `JsonPropertyName` in that case.
