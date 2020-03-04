
namespace BackEnd
{
    public class ButtonTileState : TileState
    {
        public DoorTileState Door { get; internal set; }
        public ButtonTileState(int ID, StateOfTile state) : base(ID, state)
        {
        }
        public bool Equals(ButtonTileState otherButton) => Equals((TileState) otherButton) && Door.Equals(otherButton.Door);
        public override int GetHashCode() => base.GetHashCode()*100 + Door.GetHashCode();
    }
}
