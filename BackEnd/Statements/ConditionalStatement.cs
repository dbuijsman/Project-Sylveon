
namespace BackEnd
{
    enum ConditionalStatementName
    {
        While, DoWhile, IfElse, Repeat
    }
    public abstract class ConditionalStatement : Statement
    {
		private protected readonly ConditionParameter _parameter;
		private protected readonly ConditionValue _value;
		private protected readonly bool _isTrue;
		private protected StatementBlock _statements;
        public ConditionalStatement(ConditionParameter parameter, ConditionValue value, bool isTrue){
            this.Condition = new ConditionEntity(parameter,value,isTrue);
            this._parameter = parameter;
            this._value = value;
            this._isTrue = isTrue;
        }
    }
}
