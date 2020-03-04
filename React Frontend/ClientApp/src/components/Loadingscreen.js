import React from "react";
import { Spinner } from "reactstrap";

export const Loadingscreen = () => {
	return (
		<div>
			Loading...
			<Spinner
				style={{
					width: "3rem",
					height: "3rem"
				}}
				type="grow"
				color="success"
			/>
		</div>
	);
};
