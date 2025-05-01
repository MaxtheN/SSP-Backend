namespace SspUis.DataLayer.EfClasses
{
    public interface IDocument : IDocument<long>
    {
    }

    public interface IDocument<TId>
    {
        TId Id { get; set; }
        //int StatusId { get; set; }
    }
}
