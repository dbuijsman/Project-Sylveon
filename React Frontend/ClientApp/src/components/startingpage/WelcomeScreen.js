import React, { useEffect } from "react";
import { Jumbotron, Button, Container } from "reactstrap";

export const WelcomeScreen = props => {
	const name = props.name;
	const startTutorialSession = props.startTutorialSession;

	useEffect(() => {
		localStorage.removeItem("token");
	}, []);

	return (
		<div>
			<Jumbotron fluid>
				<Container fluid>
					<h1 className="display-4">Welkom, {name}!</h1>
					<p className="lead">
						Wanneer je op start drukt volgt een tutorial van het
						sollicitatieprogrammeerspel. De bedoeling is om jouw
						karakter op de finish te laten eindigen aan de hand van
						commando's die jij het geeft. Hierbij is het van belang
						dit met zo min mogelijk commando's te doen.
					</p>
					<p className="lead">Veel geluk pik.</p>
				</Container>
			</Jumbotron>
			<Button
				color="primary start-button"
				onClick={() => startTutorialSession()}
			>
				Start tutorial
			</Button>
		</div>
	);
};
