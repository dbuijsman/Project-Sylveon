import React from "react";

export const Timer = props => {
	return (
		<div>
			<div className="timer-container">
				<div className="timer">
					{props.minutes + ":" + props.seconds}
				</div>
			</div>
		</div>
	);
};
