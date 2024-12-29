namespace frontend.logic
{
    public class GameLogic
    {
        public string CipherText { get; private set; }
        public Dictionary<char, char> SubstitutionMap { get; private set; }
        public string DecryptedText { get; private set; }

        public GameLogic(string cipherText)
        {
            CipherText = cipherText;
            SubstitutionMap = new Dictionary<char, char>();
            DecryptedText = new string('_', cipherText.Length); // Inicializa con '_'
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
                    decryptedArray[i] = plainChar; // Sustituye si hay un mapeo
                }
                else
                {
                    decryptedArray[i] = '_'; // Sustituye con '_' si no hay mapeo
                }
            }

            DecryptedText = new string(decryptedArray);
        }
    }
}
