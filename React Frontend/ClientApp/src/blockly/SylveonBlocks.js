import Blockly from "blockly";

class SylveonBlocks
{
    static get COMMAND_COLOR() { return 0; }
    static get FLOW_COLOR() { return 225; }

    static registerBlocks()
    {
        SylveonBlocks.registerBlock("move_forward", SylveonBlocks.moveForwardAction, (block) => "{\"type\":\"command\",\"action\":\"moveForward\"}");
        SylveonBlocks.registerBlock("move_backward", SylveonBlocks.moveBackwardAction, (block) => "{\"type\":\"command\",\"action\":\"moveBackward\"}");
        SylveonBlocks.registerBlock("rotate", SylveonBlocks.rotateAction, (block) => {
            const direction = block.getFieldValue("direction");
            return "{\"type\":\"command\",\"action\":\"rotate\",\"direction\":\"" + direction + "\"}";
        });
        SylveonBlocks.registerBlock("pickup", SylveonBlocks.pickupAction, (block) => "{\"type\":\"command\",\"action\":\"pickUp\"}");
        SylveonBlocks.registerBlock("drop", SylveonBlocks.dropAction, (block) => "{\"type\":\"command\",\"action\":\"drop\"}");
        SylveonBlocks.registerBlock("if_then", SylveonBlocks.ifFlow, (block) => {
            const condition = Blockly.JavaScript.statementToCode(block, "condition");
            const true_content = Blockly.JavaScript.statementToCode(block, "true_action");
            const false_content = Blockly.JavaScript.statementToCode(block, "false_action");

            return "{\"type\":\"flow\",\"action\":\"if\",\"condition\":"
                + (condition ? condition : "null")
                + ",\"true\":["
                + true_content
                + "],\"false\":["
                + false_content
                + "]}";
        });
        SylveonBlocks.registerBlock("while_do", SylveonBlocks.whileFlow, (block) => {
            const condition = Blockly.JavaScript.statementToCode(block, "condition");
            const content = Blockly.JavaScript.statementToCode(block, "action");

            return "{\"type\":\"flow\",\"action\":\"while\",\"condition\":"
                + (condition ? condition : "null")
                + ",\"content\":["
                + content
                + "]}";
        });
        SylveonBlocks.registerBlock("state_equals", SylveonBlocks.flowStatementStateEquals, (block) => {
            const object = block.getFieldValue("flow_statement_object");
            const objectState = block.getFieldValue("flow_statement_object_state");

            return "{\"type\":\"flow\",\"action\":\"equals\",\"inverse\":false,\"targetObject\":\""
                + object
                + "\",\"targetState\":\""
                + objectState
                + "\"}";
        });
        SylveonBlocks.registerBlock("state_not_equals", SylveonBlocks.flowStatementStateNotEquals, (block) => {
            const object = block.getFieldValue("flow_statement_object");
            const objectState = block.getFieldValue("flow_statement_object_state");

            return "{\"type\":\"flow\",\"action\":\"not_equals\",\"inverse\":true,\"targetObject\":\""
                + object
                + "\",\"targetState\":\""
                + objectState
                + "\"}";
        });
    }

    static registerBlock(name, object, writeCode)
    {
        Blockly.Blocks[name] = {
            init: function() {
                this.jsonInit(object);
            }
        }

        Blockly.JavaScript[name] = function(block)
        {
            let json = writeCode(block);
            if(block.getNextBlock() !== null)
                json += ",";
            return json;
        }
    }

    static workspaceToJson(workspace)
    {
        if(workspace.getTopBlocks().length === 1)
            return "[" + (Blockly.JavaScript.workspaceToCode(workspace).replace(";","").replace("\n","")) + "]";
        else
            return "null";
    }

    static clearWorkspace() 
    {
        Blockly.mainWorkspace.clear();
    }

    static get moveForwardAction()
    {
        return {
            message0: "Move forward",
            previousStatement: null,
            nextStatement: null,
            colour: SylveonBlocks.COMMAND_COLOR,
        }
    }

    static get moveBackwardAction()
    {
        return {
            message0: "Move backward",
            previousStatement: null,
            nextStatement: null,
            colour: SylveonBlocks.COMMAND_COLOR,
        }
    }

    static get rotateAction()
    {
        return {
            message0: "Rotate %1",
            previousStatement: null,
            nextStatement: null,
            colour: SylveonBlocks.COMMAND_COLOR,
            args0: [
                {
                    type: "field_dropdown",
                    name: "direction",
                    options: [
                        [ "left", "left" ],
                        [ "right", "right" ],
                    ]
                }
            ]
        }
    }

    static get pickupAction()
    {
        return {
            message0: "Pick up",
            previousStatement: null,
            nextStatement: null,
            colour: SylveonBlocks.COMMAND_COLOR,
        }
    }

    static get dropAction()
    {
        return {
            message0: "Drop",
            previousStatement: null,
            nextStatement: null,
            colour: SylveonBlocks.COMMAND_COLOR,
        }
    }

    static get ifFlow()
    {
        return {
            message0: "If %1 then %2 else %3",
            previousStatement: null,
            nextStatement: null,
            colour: SylveonBlocks.FLOW_COLOR,
            args0: [
                {
                    type: "input_value",
                    name: "condition",
                    check: [ "flow_statement" ],
                },
                {
                    type: "input_statement",
                    name: "true_action",
                },
                {
                    type: "input_statement",
                    name: "false_action",
                }
            ]
        }
    }

    static get whileFlow()
    {
        return {
            message0: "While %1 do %2",
            previousStatement: null,
            nextStatement: null,
            colour: SylveonBlocks.FLOW_COLOR,
            args0: [
                {
                    type: "input_value",
                    name: "condition",
                    check: [ "flow_statement" ],
                },
                {
                    type: "input_statement",
                    name: "action",
                }
            ]
        }
    }

    static get flowObjectDropdown()
    {
        return {
            type: "field_dropdown",
            name: "flow_statement_object",
            options: [
                [ "tile in front", "tile_in_front" ],
                [ "current tile", "current_tile" ],
                [ "tile to the left", "tile_to_left" ],
                [ "tile to the right", "tile_to_right" ],
                [ "tile behind", "tile_behind" ],
            ]
        }
    }

    static get flowObjectStateDropdown()
    {
        return {
            type: "field_dropdown",
            name: "flow_statement_object_state",
            options: [
                [ "passable", "passable" ],
                [ "impassable", "impassable" ],
                [ "moveable", "is_box" ],
                [ "a button", "is_button" ],
                [ "the finish", "is_finish" ],
            ]
        }
    }

    static get flowStatementStateEquals()
    {
        return {
            message0: "%1 is %2",
            colour: SylveonBlocks.FLOW_COLOR,
            args0: [
                SylveonBlocks.flowObjectDropdown,
                SylveonBlocks.flowObjectStateDropdown,
            ],
            output: "flow_statement"
        }
    }

    static get flowStatementStateNotEquals()
    {
        return {
            message0: "%1 is not %2",
            colour: SylveonBlocks.FLOW_COLOR,
            args0: [
                SylveonBlocks.flowObjectDropdown,
                SylveonBlocks.flowObjectStateDropdown,
            ],
            output: "flow_statement"
        }
    }
}

export default SylveonBlocks;