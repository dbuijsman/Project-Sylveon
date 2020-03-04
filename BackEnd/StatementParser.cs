using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;

namespace BackEnd
{
	public static class StatementParser
	{
		private const string STATEMENT_TYPE_PROP = "type";
		private const string STATEMENT_ACTION_PROP = "action";

		public static bool TryParseStatementTreeJson(JsonElement statementTreeJson, out IEnumerable<Statement> statements)
		{
			try
			{
				statements = ParseStatementArrayJson(statementTreeJson);
				return true;
			}
			catch
			{
				statements = null;
				return false;
			}
		}

		public static Statement ParseStatementJson(JsonElement statementElement)
		{
			string statementType = statementElement.GetProperty(STATEMENT_TYPE_PROP).GetString();
			string statementAction = statementElement.GetProperty(STATEMENT_ACTION_PROP).GetString();
			switch (statementType)
			{
				case "command":
					return ParseCommandStatementJson(statementAction, statementElement);
				case "flow":
					return ParseFlowStatementJson(statementAction, statementElement);
				default:
					throw new JsonException($"Invalid statementType \"{statementType}\".");
			}
		}

		private static IEnumerable<Statement> ParseStatementArrayJson(JsonElement statementArray)
		{
			if (statementArray.ValueKind == JsonValueKind.Array)
			{
				JsonElement.ArrayEnumerator arrayEnumerator = statementArray.EnumerateArray();
				foreach (JsonElement statementJsonElement in arrayEnumerator)
					yield return ParseStatementJson(statementJsonElement);
			}
		}

		private static SingleCommand ParseCommandStatementJson(string statementAction, JsonElement statementElement)
		{
			switch (statementAction)
			{
				case "moveForward":
					return new SingleCommand(Command.MoveForward);
				case "moveBackward":
					return new SingleCommand(Command.MoveBackward);
				case "pickUp":
					return new SingleCommand(Command.PickUp);
				case "drop":
					return new SingleCommand(Command.Drop);
				case "rotate":
					return ParseRotateCommandStatementJson(statementElement);
				default:
					throw new JsonException($"Invalid command action \"{statementAction}\".");
			}
		}

		private static SingleCommand ParseRotateCommandStatementJson(JsonElement statementElement)
		{
			string directionString = statementElement.GetProperty("direction").GetString();

			switch (directionString)
			{
				case "left":
					return new SingleCommand(Command.RotateLeft);
				case "right":
					return new SingleCommand(Command.RotateRight);
				default:
					throw new JsonException($"Invalid rotate direction \"{directionString}\".");
			}
		}

		private static Statement ParseFlowStatementJson(string statementAction, JsonElement statementElement)
		{
			switch (statementAction)
			{
				case "if":
					return ParseIfElseStatementJson(statementElement);
				case "while":
					return ParseWhileStatementJson(statementElement);
				case "equals":
					return null;
				case "not_equals":
					return null;
				default:
					throw new JsonException($"Invalid flow action \"{statementAction}\".");
			}
		}

		private static IfElse ParseIfElseStatementJson(JsonElement statementElement)
		{
			JsonElement trueBlockElement = statementElement.GetProperty("true");
			JsonElement falseBlockElement = statementElement.GetProperty("false");
			JsonElement conditionElement = statementElement.GetProperty("condition");
			Tuple<ConditionParameter, ConditionValue, bool> conditionStatementTuple = ParseConditionStatementJson(conditionElement);
			Statement[] trueBlock = ParseStatementArrayJson(trueBlockElement).ToArray();
			Statement[] falseBlock = ParseStatementArrayJson(falseBlockElement).ToArray();

			return new IfElse(
				conditionStatementTuple.Item1,
				conditionStatementTuple.Item2,
				conditionStatementTuple.Item3,
				trueBlock,
				falseBlock
			);
		}

		private static While ParseWhileStatementJson(JsonElement statementElement)
		{
			JsonElement blockElement = statementElement.GetProperty("content");
			JsonElement conditionElement = statementElement.GetProperty("condition");
			Tuple<ConditionParameter, ConditionValue, bool> conditionStatementTuple = ParseConditionStatementJson(conditionElement);
			Statement[] block = ParseStatementArrayJson(blockElement).ToArray();

			return new While(
				conditionStatementTuple.Item1,
				conditionStatementTuple.Item2,
				conditionStatementTuple.Item3,
				block
			);
		}

		private static Tuple<ConditionParameter, ConditionValue, bool> ParseConditionStatementJson(JsonElement conditionElement)
		{
			if (conditionElement.ValueKind == JsonValueKind.Null)
			{
				return new Tuple<ConditionParameter, ConditionValue, bool>(
					ConditionParameter.TileCurrent,
					ConditionValue.Finish,
					false
				);
			}

			bool shouldInvert = conditionElement.GetProperty("inverse").GetBoolean();
			string targetObjectString = conditionElement.GetProperty("targetObject").GetString();
			string targetStateString = conditionElement.GetProperty("targetState").GetString();

			return new Tuple<ConditionParameter, ConditionValue, bool>(
				ParseConditionParameter(targetObjectString),
				ParseConditionValue(targetStateString),
				!shouldInvert
			);
		}


		private static ConditionParameter ParseConditionParameter(string targetObjectString)
		{
			switch (targetObjectString)
			{
				case "tile_in_front":
					return ConditionParameter.TileFront;
				case "current_tile":
					return ConditionParameter.TileCurrent;
				case "tile_to_left":
					return ConditionParameter.TileLeft;
				case "tile_to_right":
					return ConditionParameter.TileRight;
				case "tile_behind":
					return ConditionParameter.TileBehind;
				default:
					throw new JsonException($"Invalid condition parameter \"{targetObjectString}\".");
			}
		}

		private static ConditionValue ParseConditionValue(string targetStateString)
		{
			switch (targetStateString)
			{
				case "passable":
					return ConditionValue.Passable;
				case "impassable":
					return ConditionValue.Impassable;
				case "is_box":
					return ConditionValue.HasMovable;
				case "is_button":
					return ConditionValue.Button;
				case "is_finish":
					return ConditionValue.Finish;
				default:
					throw new JsonException($"Invalid condition value \"{targetStateString}\".");
			}
		}
	}
}