import React, { useState } from "react";
import { Redirect } from "react-router-dom";
import "../../css/HR.css";

export const Login = props => {
	const [loggedIn, setLoggedIn] = useState(false);
	const [username, setUsername] = useState("");
	const [password, setPassword] = useState("");

	const handleKeyPress = event => {
		if (event.key === "Enter") {
			login(event);
		}
	};

	const handleUsernameChange = event => {
		setUsername(event.target.value);
	};

	const handlePasswordChange = event => {
		setPassword(event.target.value);
	};

	const login = async event => {
		fetch("api/HR/login", {
			method: "POST",
			headers: {
				"Content-Type": "application/json"
			},
			body: JSON.stringify({ username: username, password: password })
		})
			.then(status)
			.then(token => {
				localStorage.setItem("token", token);
				setLoggedIn(true);
			})
			.catch(error => {
				document.querySelector("#loginerror").innerHTML =
					"Oeps! " + error;
			});
	};

	const translateErrorStatusCodeToString = statusCode => {
		if (statusCode === 400) {
			return "Alle velden moeten ingevuld worden.";
		} else if (statusCode === 401) {
			return "De combinatie van gebruikersnaam en wachtwoord is niet correct.";
		} else {
			return "Er is iets mis gegaan. Probeer het later opnieuw.";
		}
	};

	const status = response => {
		return new Promise((resolve, reject) => {
			if (response.status === 200) {
				resolve(response.json());
			} else {
				reject(translateErrorStatusCodeToString(response.status));
			}
		});
	};

	const redirectToHR = () => {
		if (loggedIn) {
			return <Redirect to="/HR" />;
		}
	};

	return (
		<article className="singleBlock">
			{redirectToHR()}
			<div className="login">Login</div>
			<br />
			<div>
				<div className="credentials">
					<div>
						{"Gebruikersnaam:"}
						<input
							placeholder="Gebruikersnaam"
							type="string"
							id="username"
							value={username}
							onKeyPress={handleKeyPress}
							onChange={event => handleUsernameChange(event)}
						/>
					</div>
					<div>
						{"Wachtwoord:"}
						<input
							placeholder="Wachtwoord"
							type="password"
							id="password"
							className="password"
							value={password}
							onKeyPress={handleKeyPress}
							onChange={event => handlePasswordChange(event)}
						/>
					</div>
				</div>
				<button id="loginbutton" onClick={login}>
					{"Log in"}
				</button>
				<div className="error" id="loginerror"></div>
			</div>
		</article>
	);
};
