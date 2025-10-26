using MacroOfExile.Action.Actions.Enums;
using MacroOfExile.Action.Actions;
using MacroOfExile.Action;
using MacroOfExile.Exceptions;
using MacroOfExile.Macro.Context;
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
	internal class SetButtonStateActionTest
	{
		private Mock<ITarget> _mockTarget;

		[SetUp]
		public void SetUp()
		{
			_mockTarget = new Mock<ITarget>();
		}

		[Test]
		public void Execute_WhenValidInputs_TargetIsCalled()
		{
			// Arrange
			var setButtonStateAction = new SetMouseStateAction(
				id: "3",
				resolver: null,
				onSuccess: "next",
				onFailure: "fail",
				isLast: false)
			{
				X = new Evaluatebale("300"),
				Y = new Evaluatebale("400"),
				Button = MouseButton.RMB,
				State = 1
			};

			// Act
			setButtonStateAction.Execute(_mockTarget.Object, new MutableDictionaryContext(), null);

			// Assert
			_mockTarget.Verify(t => t.SetMouseState((int)MouseButton.RMB, 1, 300, 400), Times.Once);
		}

		[Test]
		public void Execute_WhenInvalidButton_DoesntThrow()
		{
			// Arrange
			var setButtonStateAction = new SetMouseStateAction(
				id: "4",
				resolver: null,
				onSuccess: "next",
				onFailure: "fail",
				isLast: true)
			{
				X = new Evaluatebale("500"),
				Y = new Evaluatebale("600"),
				Button = (MouseButton)999,
				State = 0
			};

			// Act & Assert
			Assert.DoesNotThrow(() => setButtonStateAction.Execute(_mockTarget.Object, new MutableDictionaryContext(), null));
		}

		[Test]
		public void Execute_WhenXIsInvalid_ThrowsUncastableVariableException()
		{
			// Arrange
			var setButtonStateAction = new SetMouseStateAction(
				id: "5",
				resolver: null,
				onSuccess: "next",
				onFailure: "fail",
				isLast: false)
			{
				X = new Evaluatebale("not_a_number"),
				Y = new Evaluatebale("100"),
				Button = MouseButton.LMB,
				State = 1
			};

			// Act & Assert
			Assert.Throws<UncastableVariableException>(() => setButtonStateAction.Execute(_mockTarget.Object, new MutableDictionaryContext(), null));
		}

		[Test]
		public void Execute_WhenYIsInvalid_ThrowsUncastableVariableException()
		{
			// Arrange
			var setButtonStateAction = new SetMouseStateAction(
				id: "6",
				resolver: null,
				onSuccess: "next",
				onFailure: "fail",
				isLast: false)
			{
				X = new Evaluatebale("50"),
				Y = new Evaluatebale("bad_value"),
				Button = MouseButton.LMB,
				State = 0
			};

			// Act & Assert
			Assert.Throws<UncastableVariableException>(() => setButtonStateAction.Execute(_mockTarget.Object, new MutableDictionaryContext(), null));
		}
	}
}
