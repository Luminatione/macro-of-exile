using MacroOfExile.Exceptions;
using MacroOfExile.Macro;
using MacroOfExile.Macro.Context;
using MacroOfExile.Target;
using Moq;
using Shared.Target;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExileTest.MacroTest
{
    [TestFixture]
    internal class MacroExecutorTest
    {
        private Mock<ITarget> _mockTarget;
        private MacroExecutor _macroExecutor;

        [SetUp]
        public void SetUp()
        {
            _mockTarget = new Mock<ITarget>();
            _macroExecutor = new MacroExecutor(_mockTarget.Object, new MutableDictionaryContext());
        }

        [Test]
        public void Execute_ShouldExecuteSingleAction_WhenActionIsLast()
        {
            // Arrange
            var mockAction = new Mock<MacroOfExile.Action.Action>();
            mockAction.Setup(a => a.Id).Returns("0");
            mockAction.Setup(a => a.IsLast).Returns(true);

            var macro = new Macro([mockAction.Object]);

            // Act
            _macroExecutor.Execute(macro);

            // Assert
            mockAction.Verify(a => a.Execute(_mockTarget.Object, It.IsAny<IContext>()), Times.Once);
            mockAction.Verify(a => a.GetNext(It.IsAny<ITarget>()), Times.Never);
        }

        [Test]
        public void Execute_ShouldExecuteMultipleActions_InCorrectSequence()
        {
            // Arrange
            var mockAction1 = new Mock<MacroOfExile.Action.Action>();
            mockAction1.Setup(a => a.Id).Returns("0");
            mockAction1.Setup(a => a.IsLast).Returns(false);
            mockAction1.Setup(a => a.GetNext(It.IsAny<ITarget>())).Returns("1");
            mockAction1.Setup(a => a.OnSuccess).Returns("1");

            var mockAction2 = new Mock<MacroOfExile.Action.Action>();
            mockAction2.Setup(a => a.Id).Returns("1");
            mockAction2.Setup(a => a.IsLast).Returns(true);

            var macro = new Macro([mockAction1.Object, mockAction2.Object]);

            // Act
            _macroExecutor.Execute(macro);

            // Assert
            mockAction1.Verify(a => a.Execute(_mockTarget.Object, It.IsAny<IContext>()), Times.Once);
            mockAction1.Verify(a => a.GetNext(It.IsAny<ITarget>()), Times.Once);
            mockAction2.Verify(a => a.Execute(_mockTarget.Object, It.IsAny<IContext>()), Times.Once);
            mockAction2.Verify(a => a.GetNext(It.IsAny<ITarget>()), Times.Never);
        }

        [Test]
        public void Execute_ShouldThrowException_WhenActionNotFound()
        {
            // Arrange
            var mockAction = new Mock<MacroOfExile.Action.Action>();
            mockAction.Setup(a => a.Id).Returns("0");
            mockAction.Setup(a => a.IsLast).Returns(false);
            mockAction.Setup(a => a.GetNext(It.IsAny<ITarget>())).Returns("1");

            var macro = new Macro([mockAction.Object]);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _macroExecutor.Execute(macro));
        }

        [Test]
        public void Execute_ShouldNotExecute_WhenMacroHasNoActions()
        {
            // Arrange
            var macro = new Macro([]);

            // Act & Assert
            Assert.Throws<MissingFirstMacroElementException>(() => _macroExecutor.Execute(macro));
        }
    }
}
