import React, { useState, useEffect, useCallback } from "react";
import { Redirect, Link } from "react-router-dom";
import { AddCandidate } from "./AddCandidate";
import { Results } from "./Statistics/Results";
import { useInterval } from "../../hooks/useInterval";
import "../../css/HR.css";

export const MainPage = () => {
	const [token, setToken] = useState(localStorage.getItem("token"));
	const [isValid, setIsValid] = useState(null);
	const [lastCheck, setLastCheck] = useState(null);
	const [newFinishedIDs, setNewFinishedIds] = useState([]);
	const [atResults, setAtResults] = useState(false);
	const [lastID, setLastID] = useState(0);
	const NewIDCheckFrequency = 20000;

	const getLastID = useCallback(async () => {
		await fetch("api/statistics/lastFinished", {
			method: "GET",
			headers: {
				"Content-Type": "application/json",
				Authorization: token
			}
		}).then(async response => {
			if (response.status === 200) {
				setLastID(await response.text());
			}
		});
	}, [setLastID, token]);

	const getNewFinished = useCallback(async () => {
		let time = "";
		if (lastCheck !== null) {
			time = "?time=" + lastCheck;
		}
		await fetch("api/statistics/newFinished" + time, {
			method: "GET",
			headers: {
				"Content-Type": "application/json",
				Authorization: localStorage.getItem("token")
			}
		})
			.then(status)
			.then(data => {
				if (data.iDs.length > 0) {
					var arrayID = newFinishedIDs;
					data.iDs.forEach(id => {
						arrayID.push(id);
					});
					setNewFinishedIds(arrayID);
				}
				setLastCheck(data.time);
			})
			.catch(async error => {
				if (error.status === 404) {
					setLastCheck(await error.text());
				}
			});
		await getLastID();
	}, [setNewFinishedIds, setLastCheck, getLastID, lastCheck, newFinishedIDs]);

	useEffect(() => {
		getNewFinished();
	}, [getNewFinished]);

	useEffect(() => {
		const storedToken = localStorage.getItem("token");
		if (storedToken !== token) {
			setToken(storedToken);
		}
	}, [token]);

	useEffect(() => {
		const validate = async () => {
			await fetch("api/HR/validate", {
				method: "GET",
				headers: {
					"Content-Type": "application/json",
					Authorization: token
				}
			}).then(response => {
				if (response.status === 200) {
					setIsValid(true);
				} else {
					setIsValid(false);
					setToken(null);
				}
			});
		};
		token !== null ? validate() : setIsValid(false);
	}, [token, setIsValid]);

	useEffect(() => {
		document.querySelector("#loginerror").innerHTML = " ";
		document.querySelector(".popupButton").style["display"] = "none";
	}, [token]);

	useInterval(async () => {
		await getNewFinished();
	}, NewIDCheckFrequency);

	const status = response => {
		return new Promise(function(resolve, reject) {
			if (response.status === 200) {
				resolve(response.json());
			} else {
				reject(response);
			}
		});
	};

	const removeIDFromNewFinishedIDs = id => {
		let arrayIDs = newFinishedIDs;
		const index = arrayIDs.indexOf(id);
		if (index > -1) {
			arrayIDs.splice(index, 1);
		}
		setNewFinishedIds(arrayIDs);
	};

	const toLogin = error => {
		document.querySelector("#loginerror").innerHTML = "Oeps! " + error;
		document.querySelector(".popupButton").style["display"] = "unset";
	};
	const changePage = () => {
		document.querySelector("#loginerror").innerHTML = " ";
		document.querySelector(".popupButton").style["display"] = "none";
		setAtResults(!atResults);
	};
	const getPage = () => {
		if (isValid) {
			if (atResults) {
				let id = lastID;
				if (newFinishedIDs.length > 0) {
					id = newFinishedIDs[0];
				}
				return (
					<div>
						<button className="upperButton" onClick={changePage}>
							Voeg kandidaat toe
						</button>
						<Results
							onInvalidSession={toLogin}
							id={id}
							lastID={lastID}
							onSeen={removeIDFromNewFinishedIDs}
						/>
					</div>
				);
			} else {
				return (
					<div>
						{newResults()}
						<button className="upperButton" onClick={changePage}>
							Naar resultaten{" "}
						</button>
						<AddCandidate onInvalidSession={toLogin} />
					</div>
				);
			}
		}
	};

	const newResults = () => {
		if (isValid) {
			if (newFinishedIDs.length > 0) {
				return <div className="badge">{newFinishedIDs.length}</div>;
			} else {
				return <div />;
			}
		}
	};
	return (
		<div>
			{getPage()}
			<div className="error" id="loginerror">
				{" "}
			</div>
			<Link to="HR/login" className="popupButton">
				<button id="loginbutton">Naar login</button>
			</Link>
			{!isValid && isValid !== null && token === null && (
				<Redirect to="/HR/login" />
			)}
		</div>
	);
};
