using MacroOfExile.Macro.Context;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExile.Action
{
    public class Evaluatebale(string expression)
    {
        public string Expression { get; set; } = expression;

        public virtual string GetValue(IContext context)
        {
            Dictionary<string, string> variableValues = [];
            Expression.Split(" ").Where(t => t.StartsWith('$')).ToList().ForEach(t => variableValues.TryAdd(t, context.GetVariable(t.Substring(1))));
            string result = new(Expression);
            variableValues.Keys.ToList().ForEach(k => result = result.Replace(k, variableValues[k].ToString()));
            return CSharpScript.EvaluateAsync(result).Result?.ToString() ?? string.Empty;
        }
    }
}
