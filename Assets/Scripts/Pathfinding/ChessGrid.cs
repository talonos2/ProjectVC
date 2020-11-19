using UnityEngine;
using System.Collections.Generic;
using System;

namespace PathFind
{

    /// <summary>
    /// Represents a grid upon which to base the pathfinding algorithm. I got this code online, then
    /// modified it so that the A* algorithm could take in a set of possible target positions instead of
    /// A single one. This is because finding a path to each enemy and then seeing which is closest
    /// took way too long. Now the pathfinding algorithm is only run once.
    /// </summary>
    public class ChessGrid
    {
        public Node[,] nodes;
        int gridSizeX, gridSizeY;

        /**
        * Create a new grid with tile prices.
        * width: grid width.
        * height: grid height.
        * tiles_costs: 2d array of floats, representing the cost of every tile.
        *               0.0f = unwalkable tile.
        *               1.0f = normal tile.
        */
        public ChessGrid(int width, int height, float[,] tiles_costs)
        {
            gridSizeX = width;
            gridSizeY = height;
            nodes = new Node[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    nodes[x, y] = new Node(tiles_costs[x, y], x, y);

                }
            }
        }

        /**
        * Create a new grid of just walkable / unwalkable.
        * width: grid width.
        * height: grid height.
        * walkable_tiles: the tilemap. true for walkable, false for blocking.
        */
        public ChessGrid(int width, int height, bool[,] walkable_tiles)
        {
            gridSizeX = width;
            gridSizeY = height;
            nodes = new Node[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    nodes[x, y] = new Node(walkable_tiles[x, y] ? 1.0f : 0.0f, x, y);
                }
            }
        }

        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (Math.Abs(x+y)!=1)
                        continue;

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;

                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    {
                        neighbours.Add(nodes[checkX, checkY]);
                    }
                }
            }

            return neighbours;
        }

        internal Vector3 GridToWorldSpace(Vector2Int position)
        {
            return new Vector3(position.x - gridSizeX * .5f, 0, position.y - gridSizeY * .5f);
        }
    }
}