using Xunit;
using Arkanoid.Game;
using Arkanoid.Entities;
using System.Collections.Generic;

namespace Arkanoid.Tests
{
    public class BlockFactoryTests
    {
        [Theory]
        [InlineData('0', null)]              // 0 должен возвращать null
        [InlineData('1', BlockType.Normal, 1)]    // 1 создает нормальный блок с 1 HP
        [InlineData('2', BlockType.Normal, 2)]    // 2 создает нормальный блок с 2 HP
        [InlineData('3', BlockType.Normal, 3)]    // 3 создает нормальный блок с 3 HP
        [InlineData('4', BlockType.Normal, 4)]    // 4 создает нормальный блок с 4 HP
        [InlineData('5', BlockType.Normal, 5)]    // 5 создает нормальный блок с 5 HP
        [InlineData('U', BlockType.Unbreakable, 1)] // U создает несокрушимый блок
        [InlineData('u', BlockType.Unbreakable, 1)] // Проверяем регистронезависимость
        [InlineData('A', BlockType.Normal, 1)]    // Неизвестный символ создает блок с 1 HP
        public void CreateBlockFromChar_ShouldCreateCorrectBlockType(
            char blockChar, BlockType? expectedType, int expectedHitPoints = 0)
        {
            // Arrange
            var position = new Vector2(100, 50);
            var size = new Vector2(50, 20);
            float spacing = 5;

            // Act
            var block = BlockFactory.CreateBlockFromChar(blockChar, position, size, spacing);

            // Assert для символа '0' (должен возвращать null)
            if (blockChar == '0')
            {
                Assert.Null(block);
                return;
            }

            // Assert для всех остальных символов
            Assert.NotNull(block);
            Assert.Equal(position, block.Position);
            Assert.Equal(size, block.Size);

            // Проверяем тип блока и количество HP
            if (expectedType.HasValue)
            {
                Assert.Equal(expectedType.Value, block.Type);
                Assert.Equal(expectedHitPoints, block.HitPoints);
            }

            // Дополнительные проверки для несокрушимых блоков
            if (blockChar == 'U' || blockChar == 'u')
            {
                Assert.Equal(BlockType.Unbreakable, block.Type);
                Assert.Equal(1, block.HitPoints);
            }
        }

        [Theory]
        [InlineData('1', '1')]  // Нормальный блок с 1 HP
        [InlineData('2', '2')]  // Нормальный блок с 2 HP
        [InlineData('3', '3')]  // Нормальный блок с 3 HP
        [InlineData('U', 'U')]  // Несокрушимый блок
        public void GetCharFromBlock_ShouldReturnCorrectChar(char createChar, char expectedChar)
        {
            // Arrange
            var position = new Vector2(100, 50);
            var size = new Vector2(50, 20);
            float spacing = 5;

            // Создаем блок
            var block = BlockFactory.CreateBlockFromChar(createChar, position, size, spacing);

            // Act
            var resultChar = BlockFactory.GetCharFromBlock(block);

            // Assert
            Assert.Equal(expectedChar, resultChar);
        }

        [Fact]
        public void GetCharFromBlock_WithNullBlock_ShouldReturnZero()
        {
            // Проверяем обработку null-блока

            // Arrange
            Block nullBlock = null;

            // Act
            var resultChar = BlockFactory.GetCharFromBlock(nullBlock);

            // Assert
            Assert.Equal('0', resultChar);
        }

        [Fact]
        public void BlockCreationAndCharConversion_ShouldBeReversible()
        {
            // Проверяем, что создание блока и преобразование в символ - обратимые операции

            // Arrange
            var testCases = new List<char> { '1', '2', '3', '4', '5', 'U' };
            var position = new Vector2(100, 50);
            var size = new Vector2(50, 20);
            float spacing = 5;

            foreach (var testChar in testCases)
            {
                // Act - создаем блок из символа
                var block = BlockFactory.CreateBlockFromChar(testChar, position, size, spacing);

                // Act - преобразуем блок обратно в символ
                var resultChar = BlockFactory.GetCharFromBlock(block);

                // Assert
                Assert.Equal(char.ToUpper(testChar), resultChar);
            }
        }

        [Theory]
        [InlineData(BlockType.Normal, 1, '1')]
        [InlineData(BlockType.Normal, 2, '2')]
        [InlineData(BlockType.Normal, 3, '3')]
        [InlineData(BlockType.Normal, 4, '4')]
        [InlineData(BlockType.Normal, 5, '5')]
        [InlineData(BlockType.Normal, 10, '1')] // Больше 5 HP - возвращает '1' как default
        [InlineData(BlockType.Unbreakable, 1, 'U')]
        public void GetCharFromBlock_WithDirectBlock_ShouldReturnCorrectChar(
            BlockType blockType, int hitPoints, char expectedChar)
        {
            // Проверяем преобразование различных типов блоков в символы

            // Arrange
            var block = new Block(
                new Vector2(100, 50),
                new Vector2(50, 20),
                blockType,
                hitPoints
            );

            // Act
            var resultChar = BlockFactory.GetCharFromBlock(block);

            // Assert
            Assert.Equal(expectedChar, resultChar);
        }
    }
}