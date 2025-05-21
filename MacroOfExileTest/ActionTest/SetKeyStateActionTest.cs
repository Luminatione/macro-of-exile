using MacroOfExile.Action.Actions;
using MacroOfExile.Macro.Context;
using Moq;
using Shared.Target;
using System;
using System.Collections.Generic;
using System.Linq;
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
			var setKeyStateAction = new SetKeyStateAction(
				id: "1",
				resolver: null,
				onSuccess: "next",
				onFailure: "fail",
				isLast: false)
			{
				Key = VirtualKey.A,
				State = 1
			};

			// Act
			setKeyStateAction.Execute(_mockTarget.Object, new MutableDictionaryContext());

			// Assert
			_mockTarget.Verify(t => t.SetKeyState((int) VirtualKey.A, 1), Times.Once);
		}

		[Test]
		public void Execute_WhenInvalidKey_DoesntThrow()
		{
			// Arrange
			var setKeyStateAction = new SetKeyStateAction(
				id: "2",
				resolver: null,
				onSuccess: "next",
				onFailure: "fail",
				isLast: true)
			{
				Key = (VirtualKey) 999,
				State = 0
			};

			// Act & Assert
			Assert.DoesNotThrow(() => setKeyStateAction.Execute(_mockTarget.Object, new MutableDictionaryContext()));
		}
	}
}
