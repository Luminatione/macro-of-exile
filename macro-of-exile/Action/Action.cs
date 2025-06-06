﻿using MacroOfExile.Action.ActionResultResolver;
using MacroOfExile.Action.Actions;
using MacroOfExile.Macro.Context;
using MacroOfExile.Target;
using Shared.Target;
using System.Text.Json.Serialization;

namespace MacroOfExile.Action
{
    [JsonDerivedType(typeof(SingleClickAction), typeDiscriminator: "SingleClick")]
    [JsonDerivedType(typeof(InitializeContextAction), typeDiscriminator: "InitializeContext")]
    [JsonDerivedType(typeof(SetButtonStateAction), typeDiscriminator: "SetButtonState")]
    [JsonDerivedType(typeof(SetKeyStateAction), typeDiscriminator: "SetKeyState")]
    public abstract partial class Action(string id, IActionResultResolver? resolver, string onSuccess, string onFailure, bool isLast = false)
    {
        protected Action() : this("0", null, "0", "0") { }

        public virtual string Id { get; } = id;
        public virtual bool IsLast { get; } = isLast;
        public virtual IActionResultResolver? Resolver { get; } = resolver;
        public virtual string OnSuccess { get; } = onSuccess;
        public virtual string OnFailure { get; } = onFailure;

        public abstract void Execute(ITarget target, IContext context);

        public virtual string GetNext(ITarget target)
        {
            return (Resolver?.IsSuccess(target) ?? true) ? OnSuccess : OnFailure;
        }
    }
}
