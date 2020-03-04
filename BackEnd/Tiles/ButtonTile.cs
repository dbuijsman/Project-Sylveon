
namespace BackEnd
{
    public class ButtonTile : Tile
    {
        public DoorTile door { get; }

        public ButtonTile(DoorTile door) : base()
        {
            this.door = door;
        }

        public override Movable ContainedItem
        {
            set
            {
                _containedItem = value;
                UpdateDoorState();
            }
        }

        private void UpdateDoorState()
        {
            if (ContainedItem == null)
            {
                door.Close();
            }
            else
            {
                door.Open();
            }
        }
    }
}
