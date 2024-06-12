using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class to handle the implementation of the A* pathfinding algorithm this inheriting from the IPathFinder interface
/// </summary>
public class PathFinder : IPathFinder
{
    /// <summary>
    /// This is the method that finds the path on the map from start to end cell using the A* algorithm
    /// </summary>
    /// <param name="cellStart"></param>
    /// <param name="cellEnd"></param>
    /// <returns></returns>
    public IList<ICell> FindPathOnMap(ICell cellStart, ICell cellEnd, IMap map)
    {
        map = (Map) GridManager.Instance.MapInterface;
        
        // Initialize the open set (priority queue) and closed set (hash set)
        var openSet = new PriorityQueue<Node>();
        var closedSet = new HashSet<Node>();

        var startNode = new Node(cellStart);
        var endNode = new Node(cellEnd);

        // Add the start node to the open set with its initial F cost
        openSet.Enqueue(startNode, startNode.FCost);

        // Main loop to process nodes until the open set is empty
        while (openSet.Count > 0)
        {
            // Dequeue the node with the lowest F cost
            var currentNode = openSet.Dequeue();

            // If the current node is the end node, retrace the path and return it
            if (currentNode.Cell == endNode.Cell)
            {
                return RetracePath(startNode, currentNode);
            }

            // Add the current node to the closed set
            closedSet.Add(currentNode);

            // Iterate through the neighbors of the current node
            foreach (var neighborCell in map.GetNeighbors(currentNode.Cell))
            {
                var neighborNode = new Node(neighborCell);

                // Skip the neighbor if it's already in the closed set
                if (closedSet.Contains(neighborNode))
                {
                    continue;
                }

                // Calculate tentative G cost for the neighbor
                float tentativeGCost = currentNode.GCost + GetDistance(currentNode.Cell, neighborNode.Cell);

                // If the tentative G cost is lower or the neighbor is not in the open set
                if (tentativeGCost < neighborNode.GCost || !openSet.Contains(neighborNode))
                {
                    // Update the neighbor node's costs and set its parent
                    neighborNode.GCost = tentativeGCost;
                    neighborNode.HCost = GetDistance(neighborNode.Cell, endNode.Cell);
                    neighborNode.Parent = currentNode;

                    // Add the neighbor to the open set if it's not already there
                    if (!openSet.Contains(neighborNode))
                    {
                        openSet.Enqueue(neighborNode, neighborNode.FCost);
                    }
                }
            }
        }

        // After looping the map if there is no possible path then we return null path
        return null;
    }

    // This method calculates the distance between two cells using Manhattan distance for a hexagonal grid
    private float GetDistance(ICell cellA, ICell cellB)
    {
        int dx = Mathf.Abs(cellA.X - cellB.X);
        int dz = Mathf.Abs(cellA.Z - cellB.Z);
        return dx + dz - Mathf.Min(dx, dz);
    }

    // This method retraces the path from end node to start node
    private IList<ICell> RetracePath(Node startNode, Node endNode)
    {
        var path = new List<ICell>();
        var currentNode = endNode;

        // Follow the parent links from the end node to the start node
        while (currentNode != startNode)
        {
            path.Add(currentNode.Cell);
            currentNode = currentNode.Parent;
        }

        // Reverse the path to get the correct order from start to end
        path.Reverse();
        return path;
    }

    // Nested class representing a node in the pathfinding algorithm
    private class Node
    {
        public ICell Cell { get; }
        public Node Parent { get; set; }
        public float GCost { get; set; } = float.MaxValue;// Cost from start to this node
        public float HCost { get; set; }// Heuristic cost from this node to the end
        public float FCost => GCost + HCost;// Total cost

        // Constructor to initialize the node with a cell
        public Node(ICell cell)
        {
            Cell = cell;
        }

        // Override Equals method for comparing nodes
        public override bool Equals(object obj)
        {
            return obj is Node node && Cell.X == node.Cell.X && Cell.Z == node.Cell.Z;
        }

        // Override GetHashCode method for hashing nodes
        public override int GetHashCode()
        {
            return (Cell.X, Cell.Z).GetHashCode();
        }
    }

    // Nested class representing a priority queue for nodes
    private class PriorityQueue<T>
    {
        private List<(T item, float priority)> elements = new List<(T item, float priority)>();

        // Property to get the count of elements in the queue
        public int Count => elements.Count;

        // Method to add an item with a priority to the queue
        public void Enqueue(T item, float priority)
        {
            elements.Add((item, priority));
        }

        // Method to remove and return the item with the lowest value
        public T Dequeue()
        {
            var bestIndex = 0;
            for (var i = 0; i < elements.Count; i++)
            {
                if (elements[i].priority < elements[bestIndex].priority)
                {
                    bestIndex = i;
                }
            }

            var bestItem = elements[bestIndex].item;
            elements.RemoveAt(bestIndex);
            return bestItem;
        }

        // Method to check if an item is in the queue
        public bool Contains(T item)
        {
            return elements.Any(e => e.item.Equals(item));
        }
    }
}


