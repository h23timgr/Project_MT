using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Xunit;
using Moq;
using OODP;
using OODP.Interfaces;

namespace _4iradTests
{
    public class SaveLoadTests
    {
        private Stack<Tuple<int, int, bool>> CreateDummyMoveHistory()
        {
            var moves = new Stack<Tuple<int, int, bool>>();
            moves.Push(new Tuple<int, int, bool>(0, 0, true));
            moves.Push(new Tuple<int, int, bool>(1, 1, false));
            return moves;
        }

        [Fact]
        public void SaveGame_ShouldCreateValidJsonFile()
        {
            //Arrange
            var logger = new DummyLoggerService();
            var fileServiceMock = new Mock<IFileService>();
            var saveService = new GameSaveService(logger, fileServiceMock.Object);
            var moves = CreateDummyMoveHistory();
            string filePath = "dummyFile.json";

            //Act
            saveService.SaveGame(filePath, moves);

            //Assert
            fileServiceMock.Verify(fs => fs.WriteAllText(filePath, It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void SaveGame_ShouldContainCorrectData()
        {
            //Arrange
            var logger = new DummyLoggerService();
            var fileServiceMock = new Mock<IFileService>();
            var saveService = new GameSaveService(logger, fileServiceMock.Object);
            var moves = CreateDummyMoveHistory();
            string filePath = "dummyFile.json";
            string capturedJson = null;

            fileServiceMock
                .Setup(fs => fs.WriteAllText(filePath, It.IsAny<string>()))
                .Callback<string, string>((path, contents) => capturedJson = contents);

            //Act
            saveService.SaveGame(filePath, moves);

            //Assert
            Assert.False(string.IsNullOrEmpty(capturedJson));
            var deserialized = JsonConvert.DeserializeObject<List<Tuple<int, int, bool>>>(capturedJson);
            Assert.NotNull(deserialized);
            Assert.Equal(moves.Count, deserialized.Count);
        }

        [Fact]
        public void LoadGame_ShouldLoadSameGameState()
        {
            //Arrange
            var logger = new DummyLoggerService();
            var fileServiceMock = new Mock<IFileService>();
            var moves = new List<Tuple<int, int, bool>>
            {
                new Tuple<int, int, bool>(0, 0, true),
                new Tuple<int, int, bool>(1, 1, false)
            };
            string jsonString = JsonConvert.SerializeObject(moves);
            string filePath = "dummyFile.json";

            fileServiceMock.Setup(fs => fs.Exists(filePath)).Returns(true);
            fileServiceMock.Setup(fs => fs.ReadAllText(filePath)).Returns(jsonString);

            var saveService = new GameSaveService(logger, fileServiceMock.Object);

            //Act
            var loadedMoves = saveService.LoadGame(filePath);

            //Assert
            Assert.Equal(moves.Count, loadedMoves.Count);
        }

        [Fact]
        public void LoadGame_WithCorruptFile_ShouldReturnEmptyList()
        {
            //Arrange
            var logger = new DummyLoggerService();
            var fileServiceMock = new Mock<IFileService>();
            string filePath = "corruptFile.json";

            fileServiceMock.Setup(fs => fs.Exists(filePath)).Returns(true);
            fileServiceMock.Setup(fs => fs.ReadAllText(filePath)).Returns("INVALID_JSON");

            var saveService = new GameSaveService(logger, fileServiceMock.Object);

            //Act
            var loadedMoves = saveService.LoadGame(filePath);

            //Assert
            Assert.Empty(loadedMoves);
        }

        [Fact]
        public void LoadGame_FileNotFound_ShouldReturnEmptyList()
        {
            //Arrange
            var logger = new DummyLoggerService();
            var fileServiceMock = new Mock<IFileService>();
            string filePath = "nonexistentFile.json";

            fileServiceMock.Setup(fs => fs.Exists(filePath)).Returns(false);

            var saveService = new GameSaveService(logger, fileServiceMock.Object);

            //Act
            var loadedMoves = saveService.LoadGame(filePath);

            //Assert
            Assert.Empty(loadedMoves);
        }

        [Fact]
        public void SaveGame_ShouldOverwriteExistingFile()
        {
            //Arrange
            var logger = new DummyLoggerService();
            var fileServiceMock = new Mock<IFileService>();
            var saveService = new GameSaveService(logger, fileServiceMock.Object);
            var moves = CreateDummyMoveHistory();
            string filePath = "dummyFile.json";

            //Act
            saveService.SaveGame(filePath, moves);
            saveService.SaveGame(filePath, moves);

            //Assert
            fileServiceMock.Verify(fs => fs.WriteAllText(filePath, It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public void SaveGame_ShouldLogError_WhenWriteFails()
        {
            var loggerMock = new Mock<ILoggerService>();
            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock
                .Setup(fs => fs.WriteAllText(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new IOException("Diskfel!"));

            var saveService = new GameSaveService(loggerMock.Object, fileServiceMock.Object);
            var moves = new Stack<Tuple<int, int, bool>>();

            // Act
            saveService.SaveGame("dummy.json", moves);

            // Assert
            loggerMock.Verify(l => l.Log(It.Is<string>(s => s.Contains("Error i spelsparning"))), Times.Once);
        }

        [Fact]
        public void LoadGame_ShouldLogError_WhenReadFails()
        {
            var loggerMock = new Mock<ILoggerService>();
            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(fs => fs.Exists(It.IsAny<string>())).Returns(true);
            fileServiceMock
                .Setup(fs => fs.ReadAllText(It.IsAny<string>()))
                .Throws(new IOException("Diskfel!"));

            var saveService = new GameSaveService(loggerMock.Object, fileServiceMock.Object);

            // Act
            var result = saveService.LoadGame("dummy.json");

            // Assert
            Assert.Empty(result);
            loggerMock.Verify(l => l.Log(It.Is<string>(s => s.Contains("Error för att ladda spel"))), Times.Once);
        }


        //Dummy Logger
        public class DummyLoggerService : ILoggerService
        {
            public void Log(string message) { }
        }
    }
}
