import React from "react";

import tileImgFloor from "../../../img/game-grid/tile-floor.png";
import tileImgWall from "../../../img/game-grid/tile-wall.png";
import tileImgDoorClose from "../../../img/game-grid/tile-door-close.png";
import tileImgDoorOpen from "../../../img/game-grid/tile-door-open.png";
import tileImgButton from "../../../img/game-grid/tile-button.png";
import tileImgFinish from "../../../img/game-grid/tile-finish.png";

import movableImgBox from "../../../img/game-grid/movable-box.png";
import movableImgPlayerDown from "../../../img/game-grid/movable-player-down.png";
import movableImgPlayerRight from "../../../img/game-grid/movable-player-right.png";
import movableImgPlayerLeft from "../../../img/game-grid/movable-player-left.png";
import movableImgPlayerUp from "../../../img/game-grid/movable-player-up.png";

export const Tile = props => {
	const getTileImgFromStateString = () => {
		switch (props.tile.stateString) {
			case "Empty":
				return tileImgFloor;
			case "Wall":
				return tileImgWall;
			case "Door":
				return props.tile.isOpen ? tileImgDoorOpen : tileImgDoorClose;
			case "Button":
				return tileImgButton;
			case "End":
				return tileImgFinish;
			default:
				throw new Error(
					`Tile state ${props.tile.stateString} is not recognized.`
				);
		}
	};

	const getTopObjectImageFromMovableString = () => {
		switch (props.tile.movableString) {
			case "Box":
				return movableImgBox;
			case "Character South":
				return movableImgPlayerDown;
			case "Character East":
				return movableImgPlayerRight;
			case "Character West":
				return movableImgPlayerLeft;
			case "Character North":
				return movableImgPlayerUp;
			default:
				return;
		}
	};

	const hasTopObject = () => {
		return props.tile.movableString !== "None";
	};

	const renderTopObject = () => {
		return (
			<div
				className="movable tile"
				style={{
					position: "absolute",
					backgroundImage:
						"url(" + getTopObjectImageFromMovableString() + ")"
				}}></div>
		);
	};

	let topObjectRender = null;
	if (hasTopObject()) topObjectRender = renderTopObject();

	return (
		<div
			className="tile"
			style={{
				backgroundImage: "url(" + getTileImgFromStateString() + ")"
			}}>
			{topObjectRender}
		</div>
	);
};
export default Tile;
