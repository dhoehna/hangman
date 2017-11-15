using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman
{
    public static class PuzzleHandler
    {
        public static bool HasWordBeenGuessedAlready(Puzzle puzzle, char guessedCharacter)
        {
            return puzzle.getMaskedWord().Any(x => x == guessedCharacter);
        }

        public static bool IsGuessedWordInWordToGuess(Puzzle puzzle, char guessedCharacter)
        {
            return puzzle.GetWordToGuess().Any(x => x == guessedCharacter);
        }

        public static void AddCorrectlyGuessedCharacter(Puzzle puzzle, char guessedCharacter)
        {
            for(int index = 0; index < puzzle.GetWordToGuess().Length; index++)
            {
                if(puzzle.GetWordToGuess()[index] == guessedCharacter)
                {
                    puzzle.GetMaskedWordWithoutSpaces()[index] = guessedCharacter;
                }
            }
        }
    }
}
