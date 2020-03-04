import React, { Component } from "react";
import ReactBlockly from "react-blockly";
import SylveonBlocks from "../../blockly/SylveonBlocks";
import "../../css/statement.css";

export class Statement extends Component {
	static displayName = Statement.name;

	static get BLOCKLY_CATEGORIES() {
		return [
			Statement.BLOCKLY_CATEGORY_COMMANDS,
			Statement.BLOCKLY_CATEGORY_FLOW
		];
	}

	static get BLOCKLY_CATEGORY_COMMANDS() {
		return {
			name: "Commands",
			blocks: [
				{ type: "move_forward" },
				{ type: "move_backward" },
				{ type: "rotate" },
				{ type: "pickup" },
				{ type: "drop" }
			]
		};
	}

	static get BLOCKLY_CATEGORY_FLOW() {
		return {
			name: "Flow",
			blocks: [
				{ type: "if_then" },
				{ type: "while_do" },
				{ type: "state_equals" },
				{ type: "state_not_equals" }
			]
		};
	}

	constructor(props) {
		super(props);
		SylveonBlocks.registerBlocks();
		this.state = {
			statementTree: null
		};
	}

	onWorkplaceChanged(workspace) {
		const statementTree = JSON.parse(
			SylveonBlocks.workspaceToJson(workspace)
		);
		this.setState({
			statementTree: statementTree
		});
	}

	onRunButtonClicked() {
		this.props.onRunCode(this.state.statementTree);
	}

	render() {
		return (
			<div id="code-section">
				<div id="blockly">
					<ReactBlockly.BlocklyEditor
						toolboxCategories={Statement.BLOCKLY_CATEGORIES}
						wrapperDivClassName="blockly-wrapper"
						workspaceDidChange={this.onWorkplaceChanged.bind(this)}
					/>
				</div>
				<button
					id="run-button"
					onClick={this.onRunButtonClicked.bind(this)}
				>
					OwO
				</button>
			</div>
		);
	}
}
