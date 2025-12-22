using Arkanoid.Entities;
using System;

namespace Arkanoid.Game
{
    public static class BlockFactory
    {
        public static Block CreateBlockFromChar(char blockChar, Vector2 position, Vector2 size, float spacing)
        {
            switch (char.ToUpper(blockChar))
            {
                case '0':
                    return null;

                case '1':
                    return new Block(position, size, BlockType.Normal, 1);

                case '2':
                    return new Block(position, size, BlockType.Normal, 2);

                case '3':
                    return new Block(position, size, BlockType.Normal, 3);

                case '4':
                    return new Block(position, size, BlockType.Normal, 4);

                case '5':
                    return new Block(position, size, BlockType.Normal, 5);

                case 'U':
                    return new Block(position, size, BlockType.Unbreakable, 1);

                default:
                    return new Block(position, size, BlockType.Normal, 1);
            }
        }

        public static char GetCharFromBlock(Block block)
        {
            if (block == null) return '0';

            if (block.Type == BlockType.Unbreakable) return 'U';

            return block.HitPoints switch
            {
                1 => '1',
                2 => '2',
                3 => '3',
                4 => '4',
                5 => '5',
                _ => '1'
            };
        }
    }
}