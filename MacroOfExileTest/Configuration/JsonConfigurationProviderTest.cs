using MacroOfExile.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MacroOfExileTest.Configuration
{
    [TestFixture]
    internal class JsonConfigurationProviderTest
    {
        [Test]
        public void GetConfiguration_ReturnsValidConfiguration_WhenFileExistsAndIsValid()
        {
            // Arrange
            var mockFileContent = "{ \"MinShortDelay\": 0, \"MaxShortDelay\": 1 }";
            var fileMock = new Mock<IFileSystem>();
            fileMock.Setup(f => f.File.ReadAllText(It.IsAny<string>()))
                .Returns(mockFileContent);
            var provider = new JsonConfigurationProvider(fileMock.Object)
            {
                ConfigurationFilename = "mock_configuration.json",
            };

            // Act
            var result = provider.GetConfiguration();

            // Assert
            Assert.That(result, Is.Not.EqualTo(null));
            Assert.Multiple(() =>
            {
                Assert.That(result.MinShortDelay, Is.EqualTo(0));
                Assert.That(result.MaxShortDelay, Is.EqualTo(1));
            });
        }

        [Test]
        public void GetConfiguration_ReturnsDefaultConfiguration_WhenFileIsEmpty()
        {
            var fileMock = new Mock<IFileSystem>();
            fileMock.Setup(f => f.File.ReadAllText(It.IsAny<string>()))
                .Returns(string.Empty);
            var provider = new JsonConfigurationProvider(fileMock.Object)
            {
                ConfigurationFilename = "mock_configuration.json"
            };

            // Act & Assert
            Assert.Throws(Is.InstanceOf<JsonException>(), () => provider.GetConfiguration());
        }

        [Test]
        public void GetConfiguration_ThrowsException_WhenFileIsInvalidJson()
        {
            // Arrange
            var invalidJson = "This is not valid JSON";
            var fileMock = new Mock<IFileSystem>();
            fileMock.Setup(f => f.File.ReadAllText(It.IsAny<string>()))
                .Returns(invalidJson);
            var provider = new JsonConfigurationProvider(fileMock.Object)
            {
                ConfigurationFilename = "mock_configuration.json"
            };

            // Act & Assert
            Assert.Throws(Is.InstanceOf<JsonException>(), () => provider.GetConfiguration());
        }

        public interface IFileWrapper
        {
            string ReadAllText(string path);
        }
    }
}
