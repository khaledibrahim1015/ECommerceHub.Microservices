using Discount.Infrastructure.Configuration.CustomAttributes;
using System.Reflection;

namespace Discount.Infrastructure.Configuration.DatabaseConfigurationManager;

public  class EntityMapper
{
    public string  TableName { get; private set; }
    public PropertyInfo PrimaryKey {  get; private set; }
    public PropertyInfo[] Properties { get; private set; }
    public Dictionary<PropertyInfo , string > ColumnsMappings { get; private set; }

    public EntityMapper(Type entityType)
    {
        TableName = GetTableName(entityType);
        Properties =  entityType.GetProperties()
                    .Where( propInfo => propInfo.GetCustomAttribute<IgnoreAttribute>() == null ).ToArray();

        PrimaryKey = Properties.FirstOrDefault(propInfo => propInfo.GetCustomAttribute<PrimaryKeyAttribute>() != null);
        ColumnsMappings = Properties.ToDictionary(
                         keySelector: prop => prop,
                         prop => prop.GetCustomAttribute<ColumnNameAttribute>().Name ?? prop.Name
                        );
    }

    private string GetTableName(Type type)
    {
        TableNameAttribute? tableAttribute =  type.GetCustomAttribute<TableNameAttribute>();
        return tableAttribute?.Name ?? type.Name + "s";
    }




}
