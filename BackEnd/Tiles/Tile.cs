
namespace BackEnd
{
    public class Tile
    {
        private readonly Tile[] neighbours = new Tile[4];
        protected bool _passable;
        public bool Passable
        {
            get
            {
                return _passable && !ContainsMovable;
            }    
            protected set
            {
                _passable = value;
            }
        }
        protected Movable _containedItem;
        public virtual Movable ContainedItem
        {
            get => _containedItem;
            set => _containedItem = value;
        }
        public bool ContainsMovable
        { 
            get
            {
                return ContainedItem != null;
            }
        }

        public Tile()
        {
            Passable = true;
            _containedItem = null;
        }

        public Tile GetNeighbour(Direction dir)
        {
            if (neighbours[(int)dir] != null)
            {
                return neighbours[(int)dir];
            }
            else
            {
                WallTile wall = new WallTile();
                wall.SetNeighbour(this, dir.Opposite());
                return wall;
            }
        }

        public void SetNeighbour(Tile neighbour, Direction dir)
        {
            neighbours[(int)dir] = neighbour;
        }

        public virtual Movable Retrieve()
        {
            Movable item = ContainedItem;
            ContainedItem = null;
            return item;
        }

        public virtual bool DropOnto(Movable item)
        {
            if (!ContainsMovable)
            {
                ContainedItem = item;
                return true;
            }
            return false;
        }
    }
}
