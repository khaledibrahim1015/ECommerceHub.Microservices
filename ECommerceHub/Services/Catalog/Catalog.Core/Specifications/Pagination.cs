namespace Catalog.Core.Specifications;

public class Pagination<T> where T:class
{
    public int PageIndex { get; init; }
    public int PageSize { get; init; }
    public long Count { get; init; }
    public IReadOnlyList<T> Data { get; init; }


    public Pagination(int pageIndex , int pageSize , int count , IReadOnlyList<T>data)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        Count = count;
        Data = data;
    }
    public Pagination()
    {
        
    }


}
