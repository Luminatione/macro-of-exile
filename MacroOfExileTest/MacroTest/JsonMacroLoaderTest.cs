using MacroOfExile.Action;
using MacroOfExile.Action.ActionResultResolver;
using MacroOfExile.Action.Actions;
using MacroOfExile.Configuration;
using MacroOfExile.Macro;
using MacroOfExile.Macro.MacroLoader;
using Moq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MacroOfExileTest.MacroTest
{
    [TestFixture]
    internal class JsonMacroLoaderTests
    {
        [Test]
        public void CreateMacro_ReturnsValidMacro_WhenFileIsValidJson()
        {
            // Arrange
            var mockFileContent = "{ \"Actions\": [{ \"$type\": \"SingleClick\", \"Id\": \"0\", \"OnSuccess\": \"2\", \"OnFailure\": \"3\", \"X\": 100, \"Y\": 200, \"Button\": 0} ] }";
            var configuration = new MacroConfiguration();
            var mockResolver = new Mock<IActionResultResolver>();
            var actions = new List<MacroOfExile.Action.Action>(){ new SingleClickAction("0", mockResolver.Object, "2", "3") {Button = 0, X = 100, Y = 200 } };
            var mockMacro = new Macro(actions, configuration);

            var configurationProviderMock = new Mock<IConfigurationProvider>();
            configurationProviderMock.Setup(cp => cp.GetConfiguration()).Returns(configuration);

            var fileSystemMock = new Mock<IFileSystem>();
            fileSystemMock.Setup(fs => fs.File.ReadAllText(It.IsAny<string>())).Returns(mockFileContent);

            var loader = new JsonMacroLoader("mock_macro.json", configurationProviderMock.Object, fileSystemMock.Object);

            // Act
            var result = loader.CreateMacro();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Actions, Has.Count.EqualTo(1));
                Assert.That(Utility.Utility.PublicInstancePropertiesEqual(result.Actions[0], actions[0], "Resolver"));
                Assert.That(result.MacroConfiguration, Is.EqualTo(configuration));
            });
        }

        [Test]
        public void CreateMacro_ThrowsJsonException_WhenFileIsInvalidJson()
        {
            // Arrange
            var invalidJson = "This is not valid JSON";

            var configurationProviderMock = new Mock<IConfigurationProvider>();
            var fileSystemMock = new Mock<IFileSystem>();
            fileSystemMock.Setup(fs => fs.File.ReadAllText(It.IsAny<string>())).Returns(invalidJson);

            var loader = new JsonMacroLoader("mock_macro.json", configurationProviderMock.Object, fileSystemMock.Object);

            // Act & Assert
            Assert.Throws(Is.InstanceOf<JsonException>(), () => loader.CreateMacro());
        }

        [Test]
        public void CreateMacro_ThrowsException_WhenFileDoesNotExist()
        {
            // Arrange
            var configurationProviderMock = new Mock<IConfigurationProvider>();
            var fileSystemMock = new Mock<IFileSystem>();
            fileSystemMock.Setup(fs => fs.File.ReadAllText(It.IsAny<string>())).Throws<FileNotFoundException>();

            var loader = new JsonMacroLoader("non_existent_macro.json", configurationProviderMock.Object, fileSystemMock.Object);

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => loader.CreateMacro());
        }
    }

}
