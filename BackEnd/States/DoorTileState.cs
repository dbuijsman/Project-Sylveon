
namespace BackEnd
{
    public class DoorTileState : TileState
    {
        public bool IsOpen { get; private set; }
        public DoorTileState(int ID, StateOfTile state, bool isOpen) : base(ID, state)
        {
            IsOpen = isOpen;
        }
        public bool Equals(DoorTileState otherDoor) => Equals((TileState) otherDoor) && IsOpen==otherDoor.IsOpen;
        public override int GetHashCode() => IsOpen ? base.GetHashCode()+50 : base.GetHashCode();
    }
}
