
[AttributeUsage(AttributeTargets.Class)]
public class TableNameAttribute : Attribute
{
    public string Name { get; set; }
    public TableNameAttribute(string Name)
     => this.Name = Name;
}


[AttributeUsage(AttributeTargets.Property)]
public class ColumnNameAttribute : Attribute
{
    public string Name { get; set; }
    public ColumnNameAttribute(string Name) => this.Name = Name;

}

[AttributeUsage(AttributeTargets.Property)]
public class IgnoreAttribute : Attribute { }


[AttributeUsage(AttributeTargets.Property)]
public class PrimaryKeyAttribute : Attribute
{

    public bool Identity { get; set; }
    public int Seed { get; set; }
    public int Increment { get; set; }



    public PrimaryKeyAttribute(bool identity = true, int seed = 1, int increment = 1)
    {
        Identity = identity;
        Seed = seed;
        Increment = increment;
    }
}