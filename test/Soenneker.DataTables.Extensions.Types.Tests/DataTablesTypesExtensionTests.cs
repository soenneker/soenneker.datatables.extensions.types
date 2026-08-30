using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Soenneker.DataTables.Attributes.Column;
using Soenneker.DataTables.Dtos.Column;
using Soenneker.Tests.Unit;

namespace Soenneker.DataTables.Extensions.Types.Tests;

public sealed class DataTablesTypesExtensionTests : UnitTest
{
    [Test]
    public void Conversion_matches_serialized_property_names_and_excludes_non_row_properties()
    {
        List<DataTableColumn> columns = typeof(Row).ToDataTableColumns();

        if (columns.Count != 3 ||
            columns[0].Data as string != "created_at" ||
            columns[0].Title != "Created" ||
            columns[1].Data as string != "fullName" ||
            columns[2].Data as string != "alwaysIncluded")
        {
            throw new InvalidOperationException("The generated columns did not match the row's serializable properties.");
        }
    }

    private sealed class Row
    {
        public string? FullName { get; init; }

        [JsonPropertyName("created_at")]
        [DataTableColumn(Title = "Created", Order = 0)]
        public DateTimeOffset CreatedAt { get; init; }

        [JsonIgnore]
        public string? Secret { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public string? AlwaysIncluded { get; init; }

        public string? SetterOnly { set { } }

        public string this[int index] => index.ToString();
    }
}
