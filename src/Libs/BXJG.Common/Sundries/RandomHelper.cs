using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace BXJG.Common
{
    public static class RandomHelper
    {
        private const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
        private const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Numbers = "0123456789";
        private const string SafeSpecials = "!@#$%^&*()-_=+[]{}|;,.<>?~";

        public static string GetRandomString(
            int length = 8,
            bool includeLowercase = true,
            bool includeUppercase = true,
            bool includeNumber = true,
            bool includeSpecial = false)
        {
            // 参数验证
            if (length <= 0)
                throw new ArgumentException("Length must be greater than zero.", nameof(length));
            if (length > 100)
                throw new ArgumentException("Length must not exceed 100.", nameof(length));

            // 收集启用的字符集
            var charSets = new List<string>(4);
            if (includeLowercase) charSets.Add(Lowercase);
            if (includeUppercase) charSets.Add(Uppercase);
            if (includeNumber) charSets.Add(Numbers);
            if (includeSpecial) charSets.Add(SafeSpecials);

            int charSetsCount = charSets.Count;
            if (charSetsCount == 0)
                throw new ArgumentException("At least one character set must be included.");

            // 检查长度是否满足最小要求
            if (length < charSetsCount)
                throw new ArgumentException($"Length must be at least {charSetsCount} when including {charSetsCount} character sets.");

            using var rng = RandomNumberGenerator.Create();
            var result = new char[length];

            // 步骤1：确保每个字符集至少包含一个字符
            for (int i = 0; i < charSetsCount; i++)
            {
                Span<byte> buffer = stackalloc byte[4];
                rng.GetBytes(buffer);
                uint randomUint = BitConverter.ToUInt32(buffer);
                int index = (int)(randomUint % (uint)charSets[i].Length);
                result[i] = charSets[i][index];
            }

            // 步骤2：创建字符池并填充剩余字符
            string charPool = string.Concat(charSets);
            int remaining = length - charSetsCount;

            if (remaining > 0)
            {
                Span<byte> buffer = stackalloc byte[remaining];
                rng.GetBytes(buffer);

                for (int i = 0; i < remaining; i++)
                {
                    int index = buffer[i] % charPool.Length;
                    result[charSetsCount + i] = charPool[index];
                }
            }

            // 步骤3：高效洗牌 (Fisher-Yates with 4-byte randomness)
            for (int i = length - 1; i > 0; i--)
            {
                Span<byte> buffer = stackalloc byte[4];
                rng.GetBytes(buffer);
                uint randomUint = BitConverter.ToUInt32(buffer);
                int j = (int)(randomUint % (i + 1));

                (result[i], result[j]) = (result[j], result[i]);
            }

            return new string(result);
        }
    }
}