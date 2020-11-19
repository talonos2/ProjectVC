/*** 
* Provide simple path-finding algorithm with support in penalties.
* Heavily based on code from this tutorial: https://www.youtube.com/watch?v=mZfyt03LDH4
* This is just a Unity port of the code from the tutorial + option to set penalty + nicer API.
*
* Original Code author: Sebastian Lague.
* Modifications & API by: Ronen Ness.
* Further modifications made by Brennan "Talonos" Smith.
* Since: 2016.
*/
using UnityEngine;
using System.Collections.Generic;

/***
 * Brennan Here: I got this code online, then
 * modified it so that the A* algorithm could take in a set of possible target positions instead of
 * a single one. This is because finding a path to each enemy and then seeing which is closest
 * took way too long. Now the pathfinding algorithm is only run once.
 * 
 * Also, I changed it to use cardinal directions instead of also using diagonals.
 */

namespace PathFind
{
    /**
    * Main class to find the best path from A to B.
    * Use like this:
    * Grid grid = new Grid(width, height, tiles_costs);
    * List<Point> path = Pathfinding.FindPath(grid, from, to);
    */
    public class Pathfinding
    {
        // The API you should use to get path
        // grid: grid to search in.
        // startPos: starting position.
        // targetPos: ending position.
        public static List<Vector2Int> FindPath(ChessGrid grid, Vector2Int startPos, List<Vector2Int> PossibleTargets)
        {
            // find path
            List<Node> nodes_path = _ImpFindPath(grid, startPos, PossibleTargets);

            // convert to a list of points and return
            List<Vector2Int> ret = new List<Vector2Int>();
            if (nodes_path != null)
            {
                foreach (Node node in nodes_path)
                {
                    ret.Add(new Vector2Int(node.gridX, node.gridY));
                }
                return ret;
            }
            else
            {
                return null;
            }
        }

        // internal function to find path, don't use this one from outside
        private static List<Node> _ImpFindPath(ChessGrid grid, Vector2Int startPos, List<Vector2Int> possibleTargets)
        {
            foreach (Node n in grid.nodes)
            {
                n.gCost = 0;
                n.hCost = 0;
                n.parent = null;
            }

            Node startNode = grid.nodes[startPos.x, startPos.y];
            List<Node> targetNodes = new List<Node>();
            foreach (Vector2Int targetPos in possibleTargets)
            {
                targetNodes.Add(grid.nodes[targetPos.x, targetPos.y]);
            }

            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                    {
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (targetNodes.Contains(currentNode))
                {
                    return RetracePath(grid, startNode, currentNode);
                }

                List<Node> neighbors = grid.GetNeighbours(currentNode);
                neighbors.Shuffle();
                foreach (Node neighbour in neighbors)
                {
                    if (!neighbour.Walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) * (int)(10.0f * neighbour.penalty);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNodes);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }

            return null;
        }

        private static List<Node> RetracePath(ChessGrid grid, Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            path.Reverse();
            return path;
        }

        private static int GetDistance(Node nodeA, List<Node> possibleNodesB)
        {
            int toReturn = int.MaxValue;
            foreach (Node nodeB in possibleNodesB)
            {
                int distance = GetDistance(nodeA, nodeB);
                if (distance < toReturn)
                {
                    toReturn = distance;
                }
            }
            return toReturn;
        }

        private static int GetDistance(Node nodeA, Node nodeB)
        {
                int distance;
                int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
                int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

                if (dstX > dstY)
                {
                    return distance = 14 * dstY + 10 * (dstX - dstY);
                }
                else
                {
                    return distance = 14 * dstX + 10 * (dstY - dstX);
                }
        }
    }

}