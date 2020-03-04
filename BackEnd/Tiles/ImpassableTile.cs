
namespace BackEnd
{
    public abstract class ImpassableTile : Tile
    {
        public ImpassableTile() : base()
        {
            Passable = false;
        }
        public override Movable Retrieve()
        {
            return null;
        }

        public override bool DropOnto(Movable item)
        {
            return false;
        }
    }
}
