using Newtonsoft.Json;

namespace BackEnd
{
    public class TileState
    {
        [JsonProperty("id")]
        public int ID { get; internal set; }
        public StateOfTile State { get;  internal set; }
        public string StateString => State.ToString();
        public MovableItem Movable { get; internal set; }
        public string MovableString => Movable.ToString();
        public TileState(int ID, StateOfTile state)
        {
            this.ID = ID;
            State = state;
        }

        public override bool Equals(object other)
        {
            if(other is TileState)
                return other.GetHashCode() == this.GetHashCode();
            else
                return false;
        }

        public override int GetHashCode()
        {
            return ID*100 + State.GetHashCode()*10 + Movable.GetHashCode();
        }
    }
}
