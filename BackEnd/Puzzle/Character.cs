using System;

namespace BackEnd
{
    public class Character : ICharacter
    {
        public Direction Direction { get; private set; }
        public Tile Position { get; private set; }
        public Movable HeldItem { get; set; }

        public Character(Tile position, Direction direction)
        {
            Position = position;
            Direction = direction;
            HeldItem = null;
        }

        public void ExecuteCommand(Command command)
        {
            switch (command)
            {
                case Command.MoveForward:
                    MoveForward();
                    break;
                case Command.MoveBackward:
                    MoveBackward();
                    break;
                case Command.RotateLeft:
                    RotateLeft();
                    break;
                case Command.RotateRight:
                    RotateRight();
                    break;
                case Command.PickUp:
                    PickUp();
                    break;
                case Command.Drop:
                    Drop();
                    break;
                default: return;
            }
        }

        protected virtual void RotateLeft()
        {
            Direction = Direction.Left();
        }

        protected virtual void RotateRight()
        {
            Direction = Direction.Right();
        }

        protected virtual bool MoveForward()
        {
            Tile tileInFront = Position.GetNeighbour(Direction);
            if (tileInFront.Passable)
            {
                Position = tileInFront;
                return true;
            }
            return false;
        }
        protected virtual bool MoveBackward()
        {
            Tile tileBehind = Position.GetNeighbour(Direction.Opposite());
            if (tileBehind.Passable)
            {
                Position = tileBehind;
                return true;
            }
            return false;
        }

        protected virtual bool PickUp()
        {
            Tile tileInFront = Position.GetNeighbour(Direction);
            if (HeldItem == null && tileInFront.ContainsMovable)
            {
                HeldItem = tileInFront.Retrieve();
                return HeldItem != null;
            }
            return false;
        }

        protected virtual bool Drop()
        {
            Tile tileInFront = Position.GetNeighbour(Direction);
            if (HeldItem != null && tileInFront.Passable)
            {
                if (tileInFront.DropOnto(HeldItem))
                {
                    HeldItem = null;
                    return true;
                }
            }
            return false;
        }

        public bool CheckCondition(ConditionParameter parameter, ConditionValue value)
        {
            Tile tileToCheck;
            switch (parameter)
            {
                case ConditionParameter.TileFront:
                    tileToCheck = Position.GetNeighbour(Direction);
                    break;
                case ConditionParameter.TileLeft:
                    tileToCheck = Position.GetNeighbour(Direction.Left());
                    break;
                case ConditionParameter.TileRight:
                    tileToCheck = Position.GetNeighbour(Direction.Right());
                    break;
                case ConditionParameter.TileBehind:
                    tileToCheck = Position.GetNeighbour(Direction.Opposite());
                    break;
                case ConditionParameter.TileCurrent:
                    tileToCheck = Position;
                    break;
                default:
                    throw new NotImplementedException();
            }

            switch (value)
            {
                case ConditionValue.Passable:
                    return tileToCheck.Passable;
                case ConditionValue.Impassable:
                    return !tileToCheck.Passable;
                case ConditionValue.Button:
                    return tileToCheck is ButtonTile;
                case ConditionValue.HasMovable:
                    return tileToCheck.ContainsMovable;
                case ConditionValue.Finish:
                    return tileToCheck is FinishTile;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
