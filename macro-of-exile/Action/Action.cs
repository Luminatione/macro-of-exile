using MacroOfExile.Action.ActionResultResolver;
using MacroOfExile.Action.Actions;
using MacroOfExile.Target;
using System.Text.Json.Serialization;

namespace MacroOfExile.Action
{
    [JsonDerivedType(typeof(SingleClickAction), typeDiscriminator: "SingleClick")]
    public abstract class Action(string id, IActionResultResolver resolver, string onSuccess, string onFailure, bool isLast = false)
    {
        public string Id { get; } = id;
        public bool IsLast { get; } = isLast;
        public IActionResultResolver Resolver { get; } = resolver;
        public string OnSuccess { get; } = onSuccess;
        public string OnFailure { get; } = onFailure;

        public abstract void Execute(ITarget target);

        public string GetNext(ITarget target)
        {
            return Resolver.IsSuccess(target) ? OnSuccess : OnFailure;
        }
    }
}
