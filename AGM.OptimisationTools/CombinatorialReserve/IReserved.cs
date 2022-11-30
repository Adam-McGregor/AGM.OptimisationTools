namespace AGM.OptimisationTools.CombinatorialReserve;

public interface IReserved
{
    /// <summary>
    /// the ids of the items
    /// </summary>
    public Memory<byte> Ids { get; set; }

    /// <summary>
    /// Specifies whether this has been initialised
    /// </summary>
    public bool Initialised => !Ids.IsEmpty;

    /// <summary>
    /// intialises the reserved object, should only be called by the combinatorialreserve when ranking / getting the Id from a set of ids
    /// Thus Ids should have the correct values when called
    /// </summary>
    public void Initialise();
}
