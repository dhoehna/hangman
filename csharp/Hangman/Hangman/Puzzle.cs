using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman
{
    public class Puzzle
    {
        private string wordToGuess;
        private char[] maskedWordToGuess;
        private List<char> guessedCharacters;

        public Puzzle(string wordToGuess)
        {
            this.wordToGuess = wordToGuess;
            maskedWordToGuess = new char[wordToGuess.Length];
            guessedCharacters = new List<char>();

            for(int maskedCharIndex = 0; maskedCharIndex < maskedWordToGuess.Length; maskedCharIndex++)
            {
                maskedWordToGuess[maskedCharIndex] = '_';
            }
        }

        public string getMaskedWord()
        {
            StringBuilder builder = new StringBuilder();

            foreach(char maskedCharacter in maskedWordToGuess)
            {
                builder.Append(maskedCharacter);
                builder.Append(' ');
            }

            return builder.ToString();
        }

        public void AddGuessedCharacter(char guessedCharacter)
        {
            this.guessedCharacters.Add(guessedCharacter);
        }

        public char[] GetMaskedWordWithoutSpaces()
        {
            return maskedWordToGuess;
        }

        public int GetNumberOfGuesses()
        {
            return guessedCharacters.Count;
        }

        public string GetWordToGuess()
        {
            return wordToGuess;
        }

        public string GetGuessedCharacters()
        {
            StringBuilder builder = new StringBuilder();

            foreach(char guessedCharacter in guessedCharacters)
            {
                builder.Append(guessedCharacter);
            }

            return builder.ToString();
        }
    }
}
