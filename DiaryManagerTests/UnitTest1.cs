using DiaryManager;
using System;
using System.IO;
using Xunit;

namespace DiaryManagerTests
{
    public class UnitTest1
    {
        private readonly string _testFilePath = "test_diary.txt";

        [Fact]
        public void ReadDiaryFile_ShouldReturnCorrectEntries()
        {
            // Arrange
            var testContent = "2023-06-25\nTest Entry 1\n\n2023-06-26\nTest Entry 2\n\n";
            File.WriteAllText(_testFilePath, testContent);
            var diary = new DailyDiary(_testFilePath);

            // Act
            var entries = diary.ReadDiaryFile();

            // Assert
            Assert.Equal(2, entries.Count);
            Assert.Equal(new DateTime(2023, 6, 25), entries[0].Date);
            Assert.Equal("Test Entry 1", entries[0].Content);
            Assert.Equal(new DateTime(2023, 6, 26), entries[1].Date);
            Assert.Equal("Test Entry 2", entries[1].Content);
        }

        [Fact]
        public void AddEntry_ShouldIncreaseEntryCount()
        {
            // Arrange
            File.Delete(_testFilePath);
            var diary = new DailyDiary(_testFilePath);
            var entry = new Entry(DateTime.Now, "Test Entry");

            // Act
            diary.AddEntry(entry);
            var entries = diary.ReadDiaryFile();

            // Assert
            Assert.Single(entries);
        }
    }
}
