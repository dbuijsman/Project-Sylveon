import React, { useState, useEffect } from "react";
import "../../../css/SkipButton.css";

export function StatisticsButtons(props) {
	const [classNamesPrevious, setClassNamesPrevious] = useState(null);
	const [classNamesNext, setClassNamesNext] = useState(null);
	useEffect(() => {
		if (props.disabledPrevious) {
			setClassNamesPrevious(["skipButton", "disabled"].join(" "));
		} else {
			setClassNamesPrevious("skipButton");
		}
	}, [props.disabledPrevious]);

	useEffect(() => {
		if (props.disabledNext) {
			setClassNamesNext(["skipButton", "disabled"].join(" "));
		} else {
			setClassNamesNext("skipButton");
		}
	}, [props.disabledNext]);

	function previousButtonClicked() {
		props.onClickPrevious();
	}
	function nextButtonClicked() {
		props.onClickNext();
	}
	return (
		<div>
			<button
				disabled={props.disabledPrevious}
				className={classNamesPrevious}
				onClick={previousButtonClicked}>
				Vorige kandidaat
			</button>
			<button
				disabled={props.disabledNext}
				className={classNamesNext}
				onClick={nextButtonClicked}>
				Volgende kandidaat
			</button>
		</div>
	);
}
