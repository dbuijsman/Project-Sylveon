
namespace BackEnd
{
    public class CharacterState
    {
        public Direction DirectionCharacter { get; }
        public string DirectionCharacterString => DirectionCharacter.ToString();
        public TileState Tile { get; internal set; }

        public CharacterState(Direction direction)
        {
            this.DirectionCharacter = direction;
        }

        public override bool Equals(object other)
        {
            if(other is CharacterState)
                return other.GetHashCode() == this.GetHashCode();
            else
                return false;
        }

        public override int GetHashCode()
        {
            return DirectionCharacter.GetHashCode() + Tile.GetHashCode()*10;
        }
    }
}
