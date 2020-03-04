using System.Collections.Generic;
using MongoDB.Entities;
using MongoDB.Entities.Core;

namespace BackEnd
{
    public class ConditionEntity : Entity
    {
		public string Parameter;
		public string Value;
		public bool IsTrue;
        public ConditionEntity(ConditionParameter parameter, ConditionValue value, bool isTrue){
            this.Parameter = parameter.ToString();
            this.Value = value.ToString();
            this.IsTrue = isTrue;
        }
        public List<object> ToObjectList()
        {
            List<object> listOfProperties = new List<object>();
            listOfProperties.Add((ConditionParameter) ConditionParameter.Parse(typeof(ConditionParameter),Parameter));
            listOfProperties.Add((ConditionValue) ConditionValue.Parse(typeof(ConditionValue),Value));
            listOfProperties.Add((bool)IsTrue);
            return listOfProperties;
        }
    }
}
