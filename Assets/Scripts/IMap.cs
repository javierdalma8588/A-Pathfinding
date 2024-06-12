using System.Collections.Generic;

/// <summary>
/// Interface for the map (Add all the map variables and functions here)
/// </summary>
public interface IMap
{
    Cell StartCell{ get; set; }
    Cell EndCell{ get; set; }
    void AddCell(Cell cell);
    void SetAllCellsWalkable();
    IEnumerable<ICell> GetNeighbors(ICell cell);
}