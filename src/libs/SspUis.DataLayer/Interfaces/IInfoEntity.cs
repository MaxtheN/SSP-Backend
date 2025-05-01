namespace SspUis.DataLayer.Interfaces
{
    public interface IInfoHl : IInfoHl<int>
    {
    }

    /// <summary>
    /// Info or HL
    /// </summary>
    public interface IInfoHl<TId>
    {
        TId Id { get; set; }
        int StateId { get; set; }
    }
}
