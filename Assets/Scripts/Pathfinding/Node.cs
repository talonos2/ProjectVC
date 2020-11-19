namespace PathFind
{
    /**
    * A node in the grid map
    */
    public class Node
    {
        // node starting params
        public bool Walkable { get => penalty != 0.0&&occupiedBy == null;}

        public Soldier occupiedBy;
        public int gridX;
        public int gridY;
        public float penalty;

        // calculated values while finding path
        public int gCost;
        public int hCost;
        public Node parent;

        // create the node
        // _price - how much does it cost to pass this tile. less is better, but 0.0f is for non-walkable.
        // _gridX, _gridY - tile location in grid.
        public Node(float _price, int _gridX, int _gridY)
        {
            penalty = _price;
            gridX = _gridX;
            gridY = _gridY;
        }

        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }

        public override string ToString()
        {
            return "(" + gridX + "," + gridY + ")";
        }
    }
}