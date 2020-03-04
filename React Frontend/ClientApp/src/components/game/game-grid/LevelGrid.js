import React, { useState, useEffect, useCallback } from "react";
import Tile from "./Tile";
import "../../../css/LevelGrid.css";

export const LevelGrid = props => {
	const [width, setWidth] = useState("");
	const [tiles, setTiles] = useState([]);

	const addRowOfWalls = useCallback((tiles, width) => {
		const extraWall = JSON.parse(
			'{"id":-1,"state":1,"stateString":"Wall","movable":0,"movableString":"None"}'
		);
		for (let index = 0; index < width + 2; index++) {
			tiles[tiles.length] = extraWall;
		}
	}, []);

	const addRowOfTiles = useCallback(
		(newTiles, originalTiles, width, startIndex) => {
			const extraWall = JSON.parse(
				'{"id":-1,"state":1,"stateString":"Wall","movable":0,"movableString":"None"}'
			);
			newTiles[newTiles.length] = extraWall;
			for (let index = 0; index < width; index++) {
				newTiles[newTiles.length] = originalTiles[startIndex + index];
			}
			newTiles[newTiles.length] = extraWall;
			return startIndex + width;
		},
		[]
	);

	const placeCharacter = useCallback((character, allTiles) => {
		allTiles[character.tile.id].movableString =
			"Character " + character.directionCharacterString;
	}, []);

	useEffect(() => {
		let puzzleWidth = props.puzzle.puzzleWidth;
		let widthStr = "";
		for (let i = 0; i < puzzleWidth + 2; i++) {
			widthStr += "auto ";
		}
		let allTiles = props.puzzle.puzzleTiles;
		placeCharacter(props.puzzle.character, allTiles);

		let startIndex = 0;
		let newTiles = [];
		addRowOfWalls(newTiles, puzzleWidth);
		while (startIndex < allTiles.length) {
			startIndex = addRowOfTiles(
				newTiles,
				allTiles,
				puzzleWidth,
				startIndex
			);
		}
		addRowOfWalls(newTiles, puzzleWidth);
		setWidth(widthStr);
		setTiles(newTiles);
	}, [
		setWidth,
		setTiles,
		props,
		placeCharacter,
		addRowOfWalls,
		addRowOfTiles
	]);

	return (
		<div>
			<div
				className="game-grid-container"
				style={{
					display: "grid",
					gridTemplateColumns: width
				}}>
				{tiles.map((key, index) => (
					<Tile key={index} tile={key} />
				))}
			</div>
			<span>Level complete: {props.isComplete.toString()}</span>
		</div>
	);
};
