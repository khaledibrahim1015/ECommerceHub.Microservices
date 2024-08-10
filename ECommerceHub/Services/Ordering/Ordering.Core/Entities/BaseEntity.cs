namespace Ordering.Core.Entities;

public abstract  class BaseEntity
{
    public int Id { get; protected set; }
    public string? CreatedBy  { get; protected set; }
    public DateTime? CreatedDate { get; protected set; }
    public string?  LastModifiedBy { get; protected set; }
    public DateTime? LastModifiedDate { get; protected set; }


}
