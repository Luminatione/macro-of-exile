using MacroOfExile.Configuration;
using MacroOfExile.Macro.Context;
using Shared.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MacroOfExile.Action.Actions
{
    internal class InitializeContextFromFileAction(string id, string onSuccess) : Action(id, null, onSuccess, string.Empty)
    {
        public required string FilePath { get; init; }
        public Dictionary<string, string> Variables { get; set; } = [];

        public override void Execute(ITarget target, IContext context, MacroConfiguration? configuration)
        {
            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException($"Context file not found: {FilePath}");
            }

            string json = File.ReadAllText(FilePath);
            Dictionary<string, JsonElement>? data;

            try
            {
                data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);
            }
            catch (JsonException ex)
            {
                throw new InvalidDataException($"Invalid JSON format in context file: {FilePath}", ex);
            }

            if (data == null)
                return;

            foreach (var (key, valueElement) in data)
            {
                string value = valueElement.ValueKind switch
                {
                    JsonValueKind.String => valueElement.GetString() ?? string.Empty,
                    JsonValueKind.Number => valueElement.GetRawText(),
                    JsonValueKind.True => "true",
                    JsonValueKind.False => "false",
                    _ => valueElement.ToString() ?? string.Empty
                };

                Variables[key] = value;
                context.SetVariable(key, value);
            }
        }
    }
}
