using Soenneker.DataTables.Attributes.Searchable;
using Soenneker.DataTables.Dtos.Column;
using Soenneker.Extensions.Char;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Serialization;
using Soenneker.DataTables.Attributes.Orderable;

namespace Soenneker.DataTables.Extensions.Types;

/// <summary>
/// A collection of helpful Type extension methods relating to DataTable.js
/// </summary>
public static class DataTablesTypesExtension
{
    /// <summary>
    /// Converts the public instance properties of a type into a list of <see cref="DataTableColumn"/> objects
    /// for use with DataTables.js, using the <see cref="JsonPropertyNameAttribute"/> if present.
    /// </summary>
    /// <param name="type">The type to reflect and extract column names from.</param>
    /// <returns>
    /// A list of <see cref="DataTableColumn"/> where each column corresponds to a public property that is not marked with <see cref="JsonIgnoreAttribute"/>.
    /// The column name will use the value of <see cref="JsonPropertyNameAttribute"/> if present, or fall back to a camelCase version of the property name.
    /// </returns>
    /// <example>
    /// For a class:
    /// <code>
    /// public class Contact
    /// {
    ///     [JsonPropertyName("full_name")]
    ///     public string Name { get; set; }
    ///
    ///     public string Email { get; set; }
    ///
    ///     [JsonIgnore]
    ///     public string InternalId { get; set; }
    /// }
    /// </code>
    /// The result will contain two columns: "full_name" and "email".
    /// </example>
    public static List<DataTableColumn> ToDataTableColumns(this Type type)
    {
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var columns = new List<DataTableColumn>(properties.Length);

        foreach (PropertyInfo prop in properties)
        {
            if (prop.GetCustomAttribute<JsonIgnoreAttribute>() is not null)
                continue;

            var jsonAttr = prop.GetCustomAttribute<JsonPropertyNameAttribute>();
            string? name = jsonAttr?.Name;

            if (name is null)
            {
                string propName = prop.Name;

                name = string.Create(propName.Length, propName, static (span, s) =>
                {
                    span[0] = s[0].ToLowerInvariant();
                    s.AsSpan(1).CopyTo(span[1..]);
                });
            }

            bool searchable = prop.IsDefined(typeof(DataTableSearchableAttribute), inherit: false);
            bool orderable = prop.IsDefined(typeof(DataTableOrderableAttribute), inherit: false);

            var column = new DataTableColumn
            {
                Data = name,
                Searchable = searchable,
                Orderable = orderable
            };

            columns.Add(column);
        }

        return columns;
    }
}