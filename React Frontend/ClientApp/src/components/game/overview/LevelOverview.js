import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import "../../../css/overview.css";
import solvedIcon from "../../../img/solved.png";
import unsolvedIcon from "../../../img/unsolved.png";

export function LevelOverview(props) {
	const [levelNumber, setLevelNumber] = useState(0);
	const [solved, setSolved] = useState(false);
	const [numberOfLines, setNumberOfLines] = useState(0);
	const [par, setPar] = useState(0);
	useEffect(() => {
		setLevelNumber(props.info.levelNumber);
		setSolved(props.info.solved);
		if (props.info.solved) {
			setNumberOfLines(props.info.lines);
			setPar(props.info.par);
		}
	}, [
		props.info.levelNumber,
		props.info.lines,
		props.info.solved,
		props.info.par
	]);
	let levelIcon;
	if (solved) {
		levelIcon = solvedIcon;
	} else {
		levelIcon = unsolvedIcon;
	}
	return (
		<article className="levelOverview">
			<Link
				to={"/gamesession/" + levelNumber}
				className="textLevelOverview"
			>
				{levelNumber}:
				{solved
					? "Solved in " +
					  numberOfLines +
					  " lines. Par is " +
					  par +
					  "."
					: "Not solved"}
				<img
					className="levelIcon"
					src={levelIcon}
					alt={solved ? "solved" : "unsolved"}
				/>
			</Link>
		</article>
	);
}
