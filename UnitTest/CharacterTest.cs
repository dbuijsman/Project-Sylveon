using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackEnd;

namespace UnitTest
{
    [TestClass]
    public class CharacterTest
    {
        [TestMethod]
        public void MoveForwardUpdatesPositionTest()
        {
            Tile characterStartPosition = new Tile();
            Tile tileInFrontOfPlayer = new Tile();
            characterStartPosition.SetNeighbour(tileInFrontOfPlayer, Direction.North);
            Character character = new Character(characterStartPosition, Direction.North);
            character.ExecuteCommand(Command.MoveForward);
            Assert.AreEqual(tileInFrontOfPlayer, character.Position);
        }

        [TestMethod]
        public void MoveBackwardUpdatesPositionTest()
        {
            Tile characterStartPosition = new Tile();
            Tile tileBehindPlayer = new Tile();
            characterStartPosition.SetNeighbour(tileBehindPlayer, Direction.East);
            Character character = new Character(characterStartPosition, Direction.West);
            character.ExecuteCommand(Command.MoveBackward);
            Assert.AreEqual(tileBehindPlayer, character.Position);
        }

        [TestMethod]
        public void RotateLeftUpdatesDirectionTest()
        {
            Character character = new Character(new Tile(), Direction.North);
            character.ExecuteCommand(Command.RotateLeft);
            Assert.AreEqual(Direction.West, character.Direction);
        }

        [TestMethod]
        public void RotateRightUpdatesDirectionTest()
        {
            Character character = new Character(new Tile(), Direction.East);
            character.ExecuteCommand(Command.RotateRight);
            Assert.AreEqual(Direction.South, character.Direction);
        }

        [TestMethod]
        public void CanNotWalkThroughWallTest()
        {
            Tile characterStartPosition = new Tile();
            Tile tileInFrontOfPlayer = new WallTile();
            characterStartPosition.SetNeighbour(tileInFrontOfPlayer, Direction.North);
            Character character = new Character(characterStartPosition, Direction.North);
            character.ExecuteCommand(Command.MoveForward);
            Assert.AreEqual(characterStartPosition, character.Position);
        }

        [TestMethod]
        public void CanWalkThroughOpenDoorTest()
        {
            Tile characterStartPosition = new Tile();
            DoorTile tileInFrontOfPlayer = new DoorTile();
            tileInFrontOfPlayer.Open();
            characterStartPosition.SetNeighbour(tileInFrontOfPlayer, Direction.North);
            Character character = new Character(characterStartPosition, Direction.North);
            character.ExecuteCommand(Command.MoveForward);
            Assert.AreEqual(tileInFrontOfPlayer, character.Position);
        }
        
        [TestMethod]
        public void CanNotWalkThroughClosedDoorTest()
        {
            Tile characterStartPosition = new Tile();
            Tile tileInFrontOfPlayer = new DoorTile();
            characterStartPosition.SetNeighbour(tileInFrontOfPlayer, Direction.North);
            Character character = new Character(characterStartPosition, Direction.North);
            character.ExecuteCommand(Command.MoveForward);
            Assert.AreEqual(characterStartPosition, character.Position);
        }
        
        [TestMethod]
        public void CanNotWalkThroughBoxTest()
        {
            Tile characterStartPosition = new Tile();
            Tile tileInFrontOfPlayer = new Tile();
            tileInFrontOfPlayer.ContainedItem = new Box();
            characterStartPosition.SetNeighbour(tileInFrontOfPlayer, Direction.North);
            Character character = new Character(characterStartPosition, Direction.North);
            character.ExecuteCommand(Command.MoveForward);
            Assert.AreEqual(characterStartPosition, character.Position);
        }

        [TestMethod]
        public void RetrieveBoxFromTileTest()
        {
            Tile tile = new Tile();
            Tile tileNeighbour = new Tile();
            Movable item = new Box();
            tileNeighbour.ContainedItem = item;
            tile.SetNeighbour(tileNeighbour, Direction.North);
            Character character = new Character(tile, Direction.North);
            character.ExecuteCommand(Command.PickUp);

            Assert.AreEqual(item, character.HeldItem);
            Assert.IsNull(tileNeighbour.ContainedItem);
            Assert.IsFalse(tileNeighbour.ContainsMovable);
        }

        [TestMethod]
        public void RetrieveNothingWhenTileHasNoMovableTest()
        {
            Tile tile = new Tile();
            Tile tileNeighbour = new Tile();
            tile.SetNeighbour(tileNeighbour, Direction.North);
            Character character = new Character(tile, Direction.North);
            character.ExecuteCommand(Command.PickUp);
            Assert.IsNull(character.HeldItem);
        }

        [TestMethod]
        public void DropOntoTileTest()
        {
            Tile tile = new Tile();
            Tile tileNeighbour = new Tile();
            tile.SetNeighbour(tileNeighbour, Direction.North);
            Movable item = new Box();
            Character character = new Character(tile, Direction.North);
            character.HeldItem = item;

            character.ExecuteCommand(Command.Drop);

            Assert.AreEqual(item, tileNeighbour.ContainedItem);
            Assert.IsTrue(tileNeighbour.ContainsMovable);
            Assert.IsNull(character.HeldItem);
        }

        [TestMethod]
        public void CanNotDropOntoBoxTest()
        {
            Tile tile = new Tile();
            Tile tileNeighbour = new Tile();
            Movable itemFront = new Box();
            tileNeighbour.ContainedItem = itemFront;
            tile.SetNeighbour(tileNeighbour, Direction.North);
            Character character = new Character(tile, Direction.North);
            Movable itemCharacter = new Box();
            character.HeldItem = itemCharacter;
            character.ExecuteCommand(Command.Drop);

            Assert.AreEqual(itemFront, tileNeighbour.ContainedItem);
            Assert.AreEqual(itemCharacter, character.HeldItem);
        }

        [TestMethod]
        public void CannotDropOntoWallTest()
        {
            Tile tile = new Tile();
            Tile wall = new WallTile();
            tile.SetNeighbour(wall, Direction.North);
            Movable item = new Box();
            Character character = new Character(tile, Direction.North);
            character.HeldItem = item;
            character.ExecuteCommand(Command.Drop);

            Assert.IsFalse(wall.ContainsMovable);
            Assert.IsNull(wall.ContainedItem);
            Assert.AreEqual(item, character.HeldItem);
        }

        [TestMethod]
        public void DroppingBoxOntoButtonOpensDoorTest()
        {
            Tile tile = new Tile();
            DoorTile door = new DoorTile();
            ButtonTile button = new ButtonTile(door);
            tile.SetNeighbour(button, Direction.North);
            Character character = new Character(tile, Direction.North);
            Movable item = new Box();
            character.HeldItem = item;
            character.ExecuteCommand(Command.Drop);

            Assert.IsTrue(door.IsOpen);
            Assert.IsTrue(button.ContainsMovable);
            Assert.AreEqual(item, button.ContainedItem);
            Assert.IsNull(character.HeldItem);
        }

        [TestMethod]
        public void TakingItemFromButtonClosesDoorTest()
        {
            Tile tile = new Tile();
            DoorTile door = new DoorTile();
            ButtonTile button = new ButtonTile(door);
            Movable item = new Box();
            button.ContainedItem = item;
            tile.SetNeighbour(button, Direction.North);
            Character character = new Character(tile, Direction.North);
            character.ExecuteCommand(Command.PickUp);

            Assert.IsTrue(door.IsClosed);
            Assert.IsFalse(button.ContainsMovable);
            Assert.AreEqual(item, character.HeldItem);
        }

        [TestMethod]
        public void CanNotDropOntoOpenDoorTest()
        {
            Tile tile = new Tile();
            DoorTile door = new DoorTile();
            door.Open();
            tile.SetNeighbour(door, Direction.North);
            Character character = new Character(tile, Direction.North);
            Movable item = new Box();
            character.HeldItem = item;
            character.ExecuteCommand(Command.Drop);

            Assert.IsFalse(door.ContainsMovable);
            Assert.AreEqual(item, character.HeldItem);
        }
    }
}
