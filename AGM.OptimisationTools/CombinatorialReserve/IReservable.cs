namespace AGM.OptimisationTools.CombinatorialReserve;

public interface IReservable<T> where T : struct
{
    public IReserved? Reserved { get; set; }
}