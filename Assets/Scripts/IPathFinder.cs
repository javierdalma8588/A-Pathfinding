using System.Collections.Generic;

/// <summary>
/// Interface to handle the path finding 
/// </summary>
public interface IPathFinder
{
    IList<ICell> FindPathOnMap(ICell cellStart, ICell cellEnd);
}

