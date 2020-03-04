
namespace BackEnd
{
    public class DoorTile : ImpassableTile
    {
        public DoorTile() : base()
        { 
        }

        public virtual void Open()
        {
            Passable = true;
        }

        public virtual void Close()
        {
            Passable = false;
        }

        public bool IsOpen => Passable;
        public bool IsClosed => !Passable;
    }
}
