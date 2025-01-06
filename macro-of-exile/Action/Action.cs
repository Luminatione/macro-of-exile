using MacroOfExile.Action.ActionResultResolver;
using MacroOfExile.Target;

namespace MacroOfExile.Action
{
    public abstract class Action(string id, IActionResultResolver resolver, bool isLast = false)
    {
        public string Id { get; } = id;
        public bool IsLast { get; } = isLast;

        public abstract void Execute(ITarget target);

        public string GetNext(ITarget target) => resolver.GetResult(target);
    }
}
