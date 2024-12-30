using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace frontend.logic
{
    public class GameLogic
    {
        public string CipherText { get; private set; } // Texto cifrado que se muestra
        public string EncodedText { get; private set; } // Texto codificado (base para referencia)
        public Dictionary<char, char> SubstitutionMap { get; private set; } // Mapa de sustitución

        public string DecryptedText { get; private set; } // Texto descifrado por el usuario

        private static readonly string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public GameLogic(string plainText)
        {
            SubstitutionMap = new Dictionary<char, char>();
            CipherText = NormalizeText(plainText); // Normalizar texto (sin tildes y en mayúsculas)
            EncodedText = GenerateEncodedText(CipherText); // Generar texto codificado
            DecryptedText = new string('_', CipherText.Length); // Inicializar con '_'
        }

        private string NormalizeText(string text)
        {
            // Quitar tildes y normalizar el texto
            var normalized = text.Normalize(NormalizationForm.FormD);
            var result = new StringBuilder();

            foreach (var c in normalized)
            {
                // Mantener solo los caracteres base
                if (char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    result.Append(c);
                }
            }

            return result.ToString().ToUpper(); // Convertir a mayúsculas
        }

        private string GenerateEncodedText(string text)
        {
            // Generar un mapeo aleatorio para el alfabeto
            var random = new Random();
            var shuffledAlphabet = Alphabet.OrderBy(_ => random.Next()).ToArray();

            // Crear un diccionario de sustitución basado en el alfabeto aleatorio
            var encodingMap = new Dictionary<char, char>();
            for (int i = 0; i < Alphabet.Length; i++)
            {
                encodingMap[Alphabet[i]] = shuffledAlphabet[i];
            }

            // Aplicar el mapeo a todas las letras del texto
            var encodedText = text.Select(c =>
            {
                if (char.IsLetter(c) && Alphabet.Contains(c)) // Solo procesar letras
                    return encodingMap[c]; // Codificar letras
                return c; // Conservar caracteres no alfabéticos
            }).ToArray();

            return new string(encodedText);
        }

        public void SetSubstitution(char cipherChar, char plainChar)
        {
            if (SubstitutionMap.ContainsKey(cipherChar))
            {
                SubstitutionMap[cipherChar] = plainChar;
            }
            else
            {
                SubstitutionMap.Add(cipherChar, plainChar);
            }

            UpdateDecryptedText();
        }

        private void UpdateDecryptedText()
        {
            var decryptedArray = CipherText.ToCharArray();
            for (int i = 0; i < CipherText.Length; i++)
            {
                if (SubstitutionMap.TryGetValue(CipherText[i], out char plainChar))
                {
                    decryptedArray[i] = plainChar; // Sustituir si hay un mapeo
                }
                else
                {
                    decryptedArray[i] = '_'; // Sustituir con '_' si no hay mapeo
                }
            }

            DecryptedText = new string(decryptedArray);
        }
        private string RemoveDiacritics(string text)
        {
            var normalizedText = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in normalizedText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

    }
}
