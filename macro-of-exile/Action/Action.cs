using MacroOfExile.Action.ActionResultResolver;
using MacroOfExile.Action.Actions;
using MacroOfExile.Configuration;
using MacroOfExile.Macro.Context;
using MacroOfExile.Target;
using Shared.Target;
using System.Text.Json.Serialization;

namespace MacroOfExile.Action
{
    [JsonDerivedType(typeof(SingleClickAction), typeDiscriminator: "SingleClick")]
    [JsonDerivedType(typeof(InitializeContextAction), typeDiscriminator: "InitializeContext")]
    [JsonDerivedType(typeof(SetMouseStateAction), typeDiscriminator: "SetMouseState")]
    [JsonDerivedType(typeof(SetKeyStateAction), typeDiscriminator: "SetKeyState")]
    [JsonDerivedType(typeof(WriteAction), typeDiscriminator: "Write")]
    [JsonDerivedType(typeof(DynamicWrite), typeDiscriminator: "DynamicWrite")]
    [JsonDerivedType(typeof(CopyAction), typeDiscriminator: "Copy")]
    [JsonDerivedType(typeof(InitializeContextFromFileAction), typeDiscriminator: "InitializeContextFromFile")]
    [JsonDerivedType(typeof(InterpolateMouseToPositionAction), typeDiscriminator: "InterpolateMouseToPosition")]
    [JsonDerivedType(typeof(BreakAction), typeDiscriminator: "Break")]
    [JsonDerivedType(typeof(SetVariableAction), typeDiscriminator: "SetVariable")]
    [JsonDerivedType(typeof(RunPythonScriptAction), typeDiscriminator: "RunPythonScript")]
    [JsonDerivedType(typeof(ClipboardToVariableAction), typeDiscriminator: "ClipboardToVariable")]
    [JsonDerivedType(typeof(WaitAction), typeDiscriminator: "Wait")]
    public abstract partial class Action(string id, IActionResultResolver? resolver, string onSuccess, string onFailure, bool isLast = false)
    {
        protected Action() : this("0", null, "0", "0") { }

        public virtual string Id { get; } = id;
        public virtual bool IsLast { get; } = isLast;
        public virtual IActionResultResolver? Resolver { get; } = resolver;
        public virtual string OnSuccess { get; } = onSuccess;
        public virtual string OnFailure { get; } = onFailure;

        public abstract void Execute(ITarget target, IContext context, MacroConfiguration? configuration);

        public virtual string GetNext(ITarget target, IContext context)
        {
            return (Resolver?.IsSuccess(target, context) ?? true) ? OnSuccess : OnFailure;
        }
    }
}
