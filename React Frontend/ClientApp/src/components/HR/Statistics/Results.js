import React, { useState, useEffect, useCallback } from "react";
import { Jumbotron, Container } from "reactstrap";
import { Statistics } from "./Statistics";
import { StatisticsButtons } from "./StatisticsButtons";
import "../../../css/HR.css";

export const Results = props => {
	const [name, setName] = useState(null);
	const [firstID, setFirstID] = useState(1);
	const [lastID, setLastID] = useState(props.lastID);
	const [id, setID] = useState(props.id);

	const getLastID = useCallback(async () => {
		const id = localStorage.getItem("token");
		await fetch("api/statistics/lastFinished", {
			method: "GET",
			headers: {
				"Content-Type": "application/json",
				Authorization: id
			}
		})
			.then(status)
			.then(data => {
				setLastID(data);
				setID(data);
			});
	}, [setLastID, setID]);

	useEffect(() => {
		if (id === 0) {
			getLastID();
			setID(lastID);
		}
		props.onSeen(id);
		fetch("api/candidate/" + id)
			.then(statusToJSON)
			.then(data => {
				setName(data.name);
			});
	}, [id, getLastID, setID, lastID, props]);

	const getPreviousID = async () => {
		document.querySelector("#loginerror").innerHTML = " ";
		document.querySelector(".popupButton").style["display"] = "none";
		await fetch("api/statistics/previousFinished?ID=" + id, {
			method: "GET",
			headers: {
				"Content-Type": "application/json",
				Authorization: localStorage.getItem("token")
			}
		})
			.then(status)
			.then(setID)
			.catch(error => {
				if (error === 404) {
					setFirstID(id);
				} else {
					props.onInvalidSession(
						translateErrorStatusCodeToString(error)
					);
				}
			});
	};
	const getNextID = async () => {
		document.querySelector("#loginerror").innerHTML = " ";
		document.querySelector(".popupButton").style["display"] = "none";
		await fetch("api/statistics/nextFinished?ID=" + id, {
			method: "GET",
			headers: {
				"Content-Type": "application/json",
				Authorization: localStorage.getItem("token")
			}
		})
			.then(status)
			.then(setID)
			.catch(error => {
				if (error === 404) {
					setLastID(id);
				} else {
					props.onInvalidSession(
						translateErrorStatusCodeToString(error)
					);
				}
			});
	};
	const status = async response => {
		return await new Promise(async function(resolve, reject) {
			if (response.status === 200) {
				resolve(await response.text());
			} else {
				reject(response.status);
			}
		});
	};
	const statusToJSON = response => {
		return new Promise(function(resolve, reject) {
			if (response.status === 200) {
				resolve(response.json());
			} else {
				reject(response.status);
			}
		});
	};
	const translateErrorStatusCodeToString = statusCode => {
		if (statusCode === 401) {
			return "De sessie is verlopen. Log opnieuw in.";
		} else {
			return "Er is iets mis gegaan. Probeer het later opnieuw.";
		}
	};
	if (id === 0) {
		return <div />;
	}
	return (
		<div>
			<Jumbotron fluid>
				<Container fluid>
					<h1 className="display-3">Statistieken {name}</h1>
					<Statistics key={id} id={id} />
					<StatisticsButtons
						disabledPrevious={id === firstID}
						disabledNext={id === lastID}
						onClickPrevious={getPreviousID}
						onClickNext={getNextID}
					/>
				</Container>
			</Jumbotron>
		</div>
	);
};
