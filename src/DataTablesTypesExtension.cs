using Soenneker.DataTables.Dtos.Column;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Serialization;
using Soenneker.DataTables.Attributes.Column;

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

            var colAttr = prop.GetCustomAttribute<DataTableColumnAttribute>();

            var column = new DataTableColumn();

            if (colAttr != null)
            {
                column.Data = colAttr.Data;
                column.Title = colAttr.Title;
                column.Width = colAttr.Width;
                column.ClassName = colAttr.ClassName;
                column.CellType = colAttr.CellType;
                column.ContentPadding = colAttr.ContentPadding;
                column.DefaultContent = colAttr.DefaultContent;
                column.Name = colAttr.Name;
                column.OrderData = colAttr.OrderData;
                column.OrderDataType = colAttr.OrderDataType;
                column.OrderSequence = colAttr.OrderSequence;
                column.Type = colAttr.Type;
                column.Footer = colAttr.Footer;
                column.AriaTitle = colAttr.AriaTitle;

                // Sentinel-based assignments for value types
                if (!colAttr.Visible)
                    column.Visible = false;

                if (colAttr.Searchable)
                    column.Searchable = true;

                if (colAttr.Orderable)
                    column.Orderable = true;

                if (colAttr.ResponsivePriority != -1)
                    column.ResponsivePriority = colAttr.ResponsivePriority;

                if (colAttr.Order != -1)
                    column.Order = colAttr.Order;
            }

            if (column.Data == null)
            {
                var jsonProp = prop.GetCustomAttribute<JsonPropertyNameAttribute>();
                column.Data = jsonProp?.Name ?? prop.Name;
            }

            columns.Add(column);
        }

        return columns;
    }
}