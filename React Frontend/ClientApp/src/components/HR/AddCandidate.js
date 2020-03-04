import React from "react";
import "../../css/HR.css";

export const AddCandidate = props => {
	const handleKeyPress = event => {
		if (event.key === "Enter") {
			addCandidate(event);
		}
	};

	const addCandidate = event => {
		document.querySelector("#loginerror").innerHTML = " ";
		document.querySelector(".popupButton").style["display"] = "none";
		let credentials = event.target;
		while (credentials.className !== "singleBlock") {
			credentials = credentials.parentNode;
		}
		credentials.querySelector("#noteAdd").innerHTML = " ";
		var name = credentials.querySelector("#username").value;

		fetch("api/HR/candidate", {
			method: "POST",
			headers: {
				"Content-Type": "application/json",
				Authorization: localStorage.getItem("token")
			},
			body: JSON.stringify(name)
		})
			.then(status)
			.then(
				() =>
					(credentials.querySelector("#noteAdd").innerHTML =
						name + " is toegevoegd.")
			)
			.catch(error => {
				const errorMessage = translateErrorStatusCodeToString(error);
				if (error === 400) {
					credentials.querySelector(
						"#noteAdd"
					).innerHTML = errorMessage;
				} else {
					props.onInvalidSession(errorMessage);
				}
			});
	};

	const translateErrorStatusCodeToString = statusCode => {
		if (statusCode === 422) {
			return "Vul eerst een naam in.";
		} else if (statusCode === 401) {
			return "De sessie is verlopen. Log opnieuw in.";
		} else {
			return "Er is iets mis gegaan. Probeer het later opnieuw.";
		}
	};

	const status = response => {
		return new Promise(function(resolve, reject) {
			if (response.status === 200) {
				resolve("OK");
			} else {
				reject(response.status);
			}
		});
	};

	return (
		<article className="singleBlock">
			<div className="login">Voeg kandidaat toe</div>
			<br />
			<div>
				<div className="credentials">
					<div>
						{"Naam kandidaat:"}
						<input
							placeholder="Naam"
							type="string"
							id="username"
							onKeyPress={handleKeyPress}
						/>
					</div>
				</div>
				<div className="note" id="noteAdd">
					{" "}
				</div>
				<button id="loginbutton" onClick={addCandidate}>
					Voeg toe
				</button>
			</div>
		</article>
	);
};
