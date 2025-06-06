﻿using Castle.Core.Configuration;
using MacroOfExile.Action;
using MacroOfExile.Action.Actions;
using MacroOfExile.Action.Actions.Enums;
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
    internal class SingleClickActionTest
    {
        private Mock<ITarget> _mockTarget;

        [SetUp]
        public void SetUp()
        {
            _mockTarget = new Mock<ITarget>();
        }

        [Test]
        public void Execute_WhenValidAction_TargetIsCalled()
        {
            // Arrange
            var singleClickAction = new SingleClickAction(
                id: "0",
                resolver: null,
                onSuccess: "next",
                onFailure: "fail",
                isLast: false)
            {
                X = new Evaluatebale("100"),
                Y = new Evaluatebale("200"),
                Button = MouseButton.LMB
            };
            _mockTarget.Setup(m => m.GetMilisBetweenActions()).Returns(0);

            // Act
            singleClickAction.Execute(_mockTarget.Object, new MutableDictionaryContext());

            // Assert
            _mockTarget.Verify(t => t.MoveMouse(100, 200), Times.Once);
            _mockTarget.Verify(t => t.SetButtonState((int) MouseButton.LMB, 1), Times.Once);
            _mockTarget.Verify(t => t.SetButtonState((int) MouseButton.LMB, 0), Times.Once);
			_mockTarget.Verify(t => t.GetMilisBetweenActions(), Times.Exactly(2));
		}

        [Test]
        public void Execute_WhenUnknownButton_DoesntThrow()
        {
            // Arrange
            var singleClickAction = new SingleClickAction(
                id: "2",
                resolver: null,
                onSuccess: "next",
                onFailure: "fail",
                isLast: true)
            {
                X = new Evaluatebale("150"),
                Y = new Evaluatebale("150"),
                Button = (MouseButton) 999
            };

            // Act & Assert
            Assert.DoesNotThrow(() => singleClickAction.Execute(_mockTarget.Object, new MutableDictionaryContext()));
        }
    }
}
