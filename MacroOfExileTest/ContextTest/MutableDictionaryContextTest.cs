using MacroOfExile.Exceptions;
using MacroOfExile.Macro.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExileTest.ContextTest
{
    [TestFixture]
    internal class MutableDictionaryContextTest
    {
        [Test]
        public void GetVariable_WhenVariableIsStored_ReturnVariable()
        {
            //Arrange
            MutableDictionaryContext context = new();
            string variableName = "foo", variableValue = "bar";
            context.SetVariable(variableName, variableValue);

            //Act & Assert
            Assert.That(context.GetVariable(variableName), Is.EqualTo(variableValue));
        }

        [Test]
        public void GetVariable_WhenVariableIsNotStored_Throws()
        {
            //Arrange
            MutableDictionaryContext context = new();

            //Act & Assert
            Assert.Throws(Is.InstanceOf<UnknownVariableException>(), () => context.GetVariable("foo"));
        }

        [Test]
        public void SetVariable_WhenVariableIsAlreadySet_OverwriteCurrentValue()
        {
            //Arrange
            MutableDictionaryContext context = new();
            string variableName = "foo", variableValue = "bar";
            context.SetVariable(variableName, variableValue);

            //Act & Assert
            Assert.DoesNotThrow(() => context.SetVariable(variableName, variableValue + "abc"));
            Assert.That(context.GetVariable(variableName), Is.EqualTo(variableValue + "abc"));
        }

        [Test]
        public void ModifyVariable_WhenVariableIsSet_ModifyVariable()
        {
            //Arrange
            MutableDictionaryContext context = new();
            string variableName = "foo", variableValue = "bar";
            context.SetVariable(variableName, variableValue);
            context.ModifyVariable(variableName, v => v += "abc");

            //Act & Assert
            Assert.That(context.GetVariable(variableName), Is.EqualTo(variableValue + "abc"));
        }

        [Test]
        public void ModifyVariable_WhenVariableIsNotSet_Throws()
        {
            //Arrange
            MutableDictionaryContext context = new();

            //Act & Assert
            Assert.Throws(Is.InstanceOf<UnknownVariableException>(), () => context.ModifyVariable("foo", v => v));
        }
    }
}
