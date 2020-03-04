import React, { useState, useEffect } from "react";
import { Button } from "reactstrap";
import { Redirect } from "react-router-dom";
import { LevelOverview } from "./LevelOverview";
import { Header } from "../header/Header";

export const Overview = () => {
	const [levels, setLevels] = useState([]);
	const [isFinished, setFinished] = useState(false);

	useEffect(() => {
		getOverview();
	}, []);

	const getOverview = async () => {
		await fetch("api/session/getOverview", {
			method: "GET",
			headers: {
				"Content-Type": "application/json",
				Authorization: localStorage.getItem("sessionID")
			}
		})
			.then(response => response.json())
			.then(data => {
				setLevels(data.levels);
			});
	};

	const toggleFinishedState = async () => {
		await fetch("api/session/endSession", {
			method: "POST",
			headers: {
				"Content-Type": "application/json",
				Authorization: localStorage.getItem("sessionID")
			}
		}).then(response => {
			setFinished(response.status === 200);
			if (response.status === 200) localStorage.removeItem("sessionID");
		});
	};

	const redirectToAnalysationPage = () => {
		if (isFinished) {
			return <Redirect to="/results" />;
		}
	};

	return (
		<div>
			{redirectToAnalysationPage()}
			<Header />
			{levels.map((key, index) => (
				<LevelOverview key={index} info={key} />
			))}
			<Button
				color="primary"
				className="finalize-button"
				onClick={() => toggleFinishedState()}
			>
				Vermoord session
			</Button>
		</div>
	);
};
