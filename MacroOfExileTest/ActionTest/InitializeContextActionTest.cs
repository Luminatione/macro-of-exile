using MacroOfExile.Action.Actions;
using MacroOfExile.Macro.Context;
using MacroOfExile.Target;
using Moq;
using Shared.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExileTest.ActionTest
{
    [TestFixture]
    internal class InitializeContextActionTest
    {
        private readonly Mock<IContext> context = new();
        private readonly Mock<ITarget> target = new();

        [Test]
        public void Execute_WhenContextProvided_ContextCalled()
        {
            //Arrange
            InitializeContextAction action = new("0", "1");
            action.Variables.Add("a", "1");
            action.Variables.Add("b", "2");

            //Act
            action.Execute(target.Object, context.Object, null);

            //Assert
            context.Verify(c => c.SetVariable(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [Test]
        public void Execute_WhenNoContextProvided_ContextNotCalled()
        {
            //Arrange
            InitializeContextAction action = new("0", "1");

            //Act
            action.Execute(target.Object, context.Object, null);

            //Assert
            context.Verify(c => c.SetVariable(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
        }
    }
}
