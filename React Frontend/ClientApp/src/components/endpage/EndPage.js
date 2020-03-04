import React from "react";
import { Jumbotron, Container } from "reactstrap";

export const EndPage = () => {
	return (
		<div>
			<Jumbotron fluid>
				<Container fluid>
					<h1 className="display-3">Je bent klaar!</h1>
					<p>Lekker gewerkt pik.</p>
				</Container>
			</Jumbotron>
		</div>
	);
};
