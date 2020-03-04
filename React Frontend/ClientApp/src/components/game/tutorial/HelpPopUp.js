import React, { useState } from "react";
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from "reactstrap";

export const HelpPopUp = props => {
	const { buttonLabel } = props;

	const [modal, setModal] = useState(false);

	const toggle = () => setModal(!modal);

	return (
		<div>
			<Button color="info" onClick={toggle}>
				{buttonLabel}
			</Button>
			<Modal isOpen={modal} toggle={toggle} className="modal-xl">
				<ModalHeader toggle={toggle}>Tutorial</ModalHeader>
				<ModalBody>
					Interesante informatie over de applicatie wat van nut kan
					zijn tot het succesvol uitvoeren van sollicitatie
					gerelateerde programmeer-opdrachten.
				</ModalBody>
				<ModalFooter>
					<Button color="primary" onClick={toggle}>
						Close
					</Button>
				</ModalFooter>
			</Modal>
		</div>
	);
};
