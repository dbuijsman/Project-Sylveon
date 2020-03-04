import React, { useState, useEffect, useCallback } from "react";
import { Timer } from "./Timer";
import "../../../css/Header.css";
import { useInterval } from "../../../hooks/useInterval";

export const Header = props => {
	const [millisecondsRemaining, setMillisecondsRemaining] = useState(1200000);
	const [minutes, setMinutes] = useState("20");
	const [seconds, setSeconds] = useState("00");
	const timerTickDelay = 1000;
	const timerBackendSyncDelay = 10000;

	const sessionIsOver = useCallback(
		input => {
			if (props.hasTimer) {
				props.sessionIsOverCallback(input);
			}
		},
		[props]
	);

	const getRemainingTime = useCallback(async () => {
		const id = localStorage.getItem("sessionID");
		await fetch("api/session/remainingTime", {
			method: "GET",
			headers: {
				"Content-Type": "application/json",
				Authorization: id
			}
		})
			.then(status)
			.then(milliseconds => {
				setMillisecondsRemaining(milliseconds);
			})
			.catch(async error => {
				if (error === 410) {
					setMillisecondsRemaining(0);
					await fetch("api/session/endSession", {
						method: "POST",
						headers: {
							"Content-Type": "application/json",
							Authorization: localStorage.getItem("sessionID")
						}
					}).then(() => {
						sessionIsOver(true);
					});
				}
			});
	}, [setMillisecondsRemaining, sessionIsOver]);

	useInterval(() => {
		if (props.hasTimer) {
			tickTimer();
		}
	}, timerTickDelay);

	useInterval(() => {
		if (props.hasTimer) {
			getRemainingTime();
		}
	}, timerBackendSyncDelay);

	useEffect(() => {
		if (props.hasTimer) {
			getRemainingTime();
		}
	}, [getRemainingTime, props.hasTimer]);

	const timer = () => {
		if (props.hasTimer && !isNaN(millisecondsRemaining)) {
			return <Timer minutes={minutes} seconds={seconds} key="timer" />;
		}
		return;
	};

	const tickTimer = async () => {
		if (millisecondsRemaining <= 0) {
			setTime("00", "00", 0);
			return;
		}

		let minutes = Math.floor(millisecondsRemaining / 60000);
		let seconds = Math.floor(millisecondsRemaining / 1000) % 60;

		if (minutes < 10) minutes = "0" + minutes;
		if (seconds < 10) seconds = "0" + seconds;

		setTime(minutes, seconds, millisecondsRemaining);
	};

	const setTime = (minutes, seconds, millisecondsRemaining) => {
		setMinutes(minutes);
		setSeconds(seconds);
		setMillisecondsRemaining(millisecondsRemaining - 1000);
	};

	const status = response => {
		return new Promise(function(resolve, reject) {
			if (response.status === 200) {
				resolve(response.json());
			} else {
				reject(response.status);
			}
		});
	};

	return (
		<div>
			<div className="header-container">
				<div className="header-content">{timer()}</div>
			</div>
		</div>
	);
};
