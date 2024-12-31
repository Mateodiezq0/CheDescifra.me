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
        public Dictionary<char, char> SubstitutionMap { get; private set; } // Mapa de sustitución del usuario
        public Dictionary<char, char> OriginalSubstitutionMap { get; set; } // Mapa original de sustitución

        public string DecryptedText { get; private set; } // Texto descifrado por el usuario

        //private static readonly string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public GameLogic(string plainText)
        {
            // Normalizar el texto plano para eliminar tildes y caracteres especiales
            CipherText = NormalizeText(plainText); 

            // Generar el texto codificado y el mapa original de sustitución
            OriginalSubstitutionMap = GenerateOriginalSubstitutionMap(CipherText, out string encodedText);
            EncodedText = encodedText;

            // Inicializar el mapa de sustitución del usuario solo con las letras relevantes
            SubstitutionMap = OriginalSubstitutionMap.Keys.ToDictionary(key => key, _ => ' ');

            // Inicializar el texto descifrado con espacios
            DecryptedText = new string(' ', CipherText.Length);
        }

        public bool IsGameWon()
        {
            foreach (var pair in OriginalSubstitutionMap)
            {
                // Verificar si el mapa del usuario contiene la relación inversa
                if (!SubstitutionMap.TryGetValue(pair.Value, out char originalKey) || originalKey != pair.Key)
                {
                    return false; // Si no coincide, el juego no está ganado
                }
            }

            return true; // Todos los pares coinciden
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

        private Dictionary<char, char> GenerateOriginalSubstitutionMap(string text, out string encodedText)
        {
            // Extraer las letras únicas del texto
            var uniqueChars = new HashSet<char>(text.Where(char.IsLetter));

            // Generar un alfabeto aleatorio limitado a las letras únicas del texto
            var random = new Random();
            var shuffledChars = uniqueChars.OrderBy(_ => random.Next()).ToArray();

            // Crear el mapa original de sustitución
            var originalMap = new Dictionary<char, char>();
            int index = 0;
            foreach (var c in uniqueChars)
            {
                originalMap[c] = shuffledChars[index++];
            }

            // Generar el texto codificado usando el mapa de sustitución
            var encodedArray = text.Select(c =>
            {
                if (originalMap.ContainsKey(c)) // Solo procesar letras en el mapa
                    return originalMap[c]; // Codificar la letra
                return c; // Conservar caracteres no alfabéticos
            }).ToArray();

            encodedText = new string(encodedArray);
            return originalMap;
        }

        public void SetSubstitution(char cipherChar, char plainChar)
        {
            if (SubstitutionMap.ContainsKey(cipherChar))
            {
                SubstitutionMap[cipherChar] = plainChar;
            }
            else
            {
                throw new ArgumentException($"El carácter '{cipherChar}' no es válido para esta partida.");
            }

            UpdateDecryptedText();
        }

        private void UpdateDecryptedText()
        {
            // Construir el texto descifrado basado en el mapa de sustitución
            var decryptedArray = CipherText.Select(c =>
            {
                if (SubstitutionMap.TryGetValue(c, out char plainChar) && plainChar != ' ')
                {
                    return plainChar; // Sustituir con el carácter mapeado
                }
                return ' '; // Si no hay mapeo, usar un espacio
            }).ToArray();

            DecryptedText = new string(decryptedArray);
        }

    }
}
