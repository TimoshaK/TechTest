using System;
using System.Collections.Generic;
using System.Linq;

namespace L2S3{
    public class Password{
        private string _value;

        

        public string Value => _value;
        public int Length => _value.Length;

        // Замена последнего символа
        public Password ReplaceLastCharacter(char newChar)
        {
            if (string.IsNullOrEmpty(_value))
                return new Password(string.Empty);
            
            char[] chars = _value.ToCharArray();
            chars[^1] = newChar;
            return new Password(new string(chars));
        }
        public Password(string password)
        {
            _value = password ?? string.Empty;
        }
        // Сравнение длин паролей
        public bool IsLongerThan(Password other)
        {
            return this.Length > other.Length;
        }

        public bool IsShorterThan(Password other)
        {
            return this.Length < other.Length;
        }

        // Проверка на равенство паролей
        public bool Equals(Password other)
        {
            if (other is null) return false;
            return this._value == other._value;
        }

        public bool NotEquals(Password other)
        {
            return !this.Equals(other);
        }

        // Сброс пароля на новое значение
        public Password Reset(string newPassword)
        {
            return new Password(newPassword);
        }

        // Проверка на стойкость пароля
        public bool IsStrong()
        {
            if (string.IsNullOrEmpty(_value) || _value.Length < 8)
                return false;

            bool hasUpper = _value.Any(char.IsUpper);
            bool hasLower = _value.Any(char.IsLower);
            bool hasDigit = _value.Any(char.IsDigit);
            bool hasSpecial = _value.Any(ch => !char.IsLetterOrDigit(ch));

            return hasUpper && hasLower && hasDigit && hasSpecial;
        }

        // Проверка на пустой пароль
        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(_value);
        }

        // Проверка допустимой длины
        public bool IsValidLength()
        {
            return this.Length >= 6 && this.Length <= 12;
        }

        public override string ToString()
        {
            return _value;
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"🔐 Пароль: {_value}");
            Console.WriteLine($"📏 Длина: {Length} символов");
            Console.WriteLine($"🛡️  Стойкий: {(IsStrong() ? "✅ Да" : "❌ Нет")}");
            Console.WriteLine($"📋 Допустимая длина: {(IsValidLength() ? "✅ Да" : "❌ Нет")}");
            Console.WriteLine($"🔢 Содержит цифры: {(ContainsDigits() ? "✅ Да" : "❌ Нет")}");
            Console.WriteLine($"✨ Содержит спецсимволы: {(ContainsSpecialCharacters() ? "✅ Да" : "❌ Нет")}");
            Console.WriteLine(new string('-', 40));
        }

        // Методы для проверки содержимого
        public bool ContainsDigits()
        {
            return _value.Any(char.IsDigit);
        }

        public bool ContainsSpecialCharacters()
        {
            return _value.Any(ch => !char.IsLetterOrDigit(ch));
        }
    }
    public static class PasswordExtensions{
        // Метод расширения для string - получение среднего символа
        public static string GetMiddleCharacter(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return "🚫 Пустая строка";

            int middleIndex = text.Length / 2;
            
            if (text.Length % 2 == 0)
            {
                return text.Substring(middleIndex - 1, 2);
            }
            else
            {
                return text[middleIndex].ToString();
            }
        }
    }
}