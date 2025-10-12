using MacroOfExile.Action.Actions;
using MacroOfExile.Macro.Context;
using Moq;
using Shared.KeyState;
using Shared.Target;
using Shared.VirtualKeys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static MacroOfExile.Action.Actions.SetKeyStateAction;

namespace MacroOfExileTest.ActionTest
{
    [TestFixture]
    internal class SetKeyStateActionTest
    {
        private Mock<ITarget> _mockTarget;

        [SetUp]
        public void SetUp()
        {
            _mockTarget = new Mock<ITarget>();
        }

        [Test]
        public void Execute_WhenValidKeyAndState_TargetIsCalled()
        {
            // Arrange
            List<KeyState> keyStates = new List<KeyState> { new KeyState(Shared.VirtualKeys.VirtualKey.A, 1) };
            var setKeyStateAction = new SetKeyStateAction(
                id: "1",
                resolver: null,
                onSuccess: "next",
                onFailure: "fail",
                isLast: false)
            {
                KeyStates = keyStates
            };

            // Act
            setKeyStateAction.Execute(_mockTarget.Object, new MutableDictionaryContext());

            // Assert
            _mockTarget.Verify(t => t.SetKeysState(keyStates), Times.Once);
        }

        [Test]
        public void Execute_WhenInvalidKey_DoesNotThrow()
        {
            // Arrange
            List<KeyState> keyStates = new List<KeyState> { new KeyState((VirtualKey) 999, 0) };

            var setKeyStateAction = new SetKeyStateAction(
                id: "2",
                resolver: null,
                onSuccess: "next",
                onFailure: "fail",
                isLast: true)
            {
                KeyStates = keyStates
            };

            // Act & Assert
            Assert.DoesNotThrow(() => setKeyStateAction.Execute(_mockTarget.Object, new MutableDictionaryContext()));
        }
    }
}
