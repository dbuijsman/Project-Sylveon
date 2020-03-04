import React, { useEffect, useState } from "react";
import {
	Jumbotron,
	Button,
	Container,
	Form,
	FormGroup,
	Label,
	Input
} from "reactstrap";

export const CanditateSelectionPage = props => {
	const [dropdownValue, setDropdownValue] = useState("kieskandidaat");
	const [availableCandidateOptions, setAvailableCandidateOptions] = useState(
		null
	);
	const [availableCandidates, setAvailableCandidates] = useState(null);

	const selectedCandidateNameCallBack = props.selectedCandidateNameCallBack;

	useEffect(() => {
		const getAllUnstartedCandidates = () => {
			fetch("api/candidate/getUnstartedCandidates", {
				method: "GET",
				headers: {
					"Content-Type": "application/json",
					Authorization: localStorage.getItem("token")
				}
			})
				.then(response => response.json())
				.then(data => {
					setAvailableCandidates(data);
					let candidateOptions = data.map((candidate, index) => {
						return (
							<option value={candidate.id} key={index}>
								{candidate.name}
							</option>
						);
					});
					setAvailableCandidateOptions(candidateOptions);
				});
		};
		getAllUnstartedCandidates();
	}, []);

	const handleCandidateSelection = () => {
		localStorage.setItem("sessionID", dropdownValue);
		const selectedCandidateName = availableCandidates.map(candidate => {
			let name = "";
			if (candidate.id === dropdownValue) {
				name = candidate.name;
			}
			return name;
		});
		selectedCandidateNameCallBack(selectedCandidateName);
	};

	return (
		<div>
			<Jumbotron fluid>
				<Container fluid>
					<h1 className="display-2">Kies kandidaat</h1>
					<Form>
						<FormGroup>
							<Label for="selectCandidate">
								Selecteer kandidaat
							</Label>
							<Input
								type="select"
								name="select"
								id="candidateSelect"
								onChange={e => {
									setDropdownValue(e.target.value);
								}}
								value={dropdownValue}
							>
								<option disabled value="kieskandidaat">
									Kies kandidaat
								</option>
								{availableCandidateOptions}
							</Input>
						</FormGroup>
					</Form>

					<Button
						color="primary start-button"
						onClick={() => handleCandidateSelection()}
					>
						Selecteer kandidaat
					</Button>
				</Container>
			</Jumbotron>
		</div>
	);
};
