using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Hangman
{
    class Program
    {
        private const int MINIMUM_WORD_LENGTH = 5;
        private const int MAXIMUM_WORD_LENGTH = 9;


        static void Main(string[] args)
        {
            List<string> words = File.ReadAllLines("data\\dict.txt")
                .Where(line => line.Length >= MINIMUM_WORD_LENGTH && line.Length <= MAXIMUM_WORD_LENGTH).ToList();

            Random randomWordGenerator = new Random();

            string wordToGuess = words[randomWordGenerator.Next(words.Count)].ToLower();

            Puzzle puzzle = new Puzzle(wordToGuess);


            while(true)
            {
                CheckIfLostAndExit(puzzle);
                CheckIfWonAndExit(puzzle);

                Console.WriteLine("Current puzzle is: " + puzzle.getMaskedWord() + "  guessed: " + puzzle.GetGuessedCharacters());
                Console.Write("Enter your guess: ");
                string guess = Console.ReadLine();

                if(guess.Length == 1)
                {
                    HandleGuess(puzzle, guess[0]);
                }
                else
                {
                    Console.WriteLine("Please enter only one character");
                }
            }


        }

        private static void HandleGuess(Puzzle puzzle, char guessedCharacter)
        {
            bool hasCharacterBeenGUessedAlready = PuzzleHandler.HasWordBeenGuessedAlready(puzzle, guessedCharacter);
            bool isguessedCharacterInTheWordToGuess = PuzzleHandler.IsGuessedWordInWordToGuess(puzzle, guessedCharacter);

            if(hasCharacterBeenGUessedAlready)
            {
                Console.WriteLine("You already guessed that character");
            }
            else if (isguessedCharacterInTheWordToGuess)
            {
                Console.WriteLine("The character was in the word, filling it in");
                PuzzleHandler.AddCorrectlyGuessedCharacter(puzzle, guessedCharacter);
                puzzle.AddGuessedCharacter(guessedCharacter);
            }
            else
            {
                Console.WriteLine("The character was not in the word");
                puzzle.AddGuessedCharacter(guessedCharacter);
            }
        }

        private static void CheckIfLostAndExit(Puzzle puzzle)
        {
            if(puzzle.GetNumberOfGuesses() > 7)
            {
                Console.WriteLine("you lose");
                Console.WriteLine("Your word was: " + puzzle.GetWordToGuess());
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        private static void CheckIfWonAndExit(Puzzle puzzle)
        {
            if(!puzzle.GetMaskedWordWithoutSpaces().Any(x => x == '_'))
            {
                Console.WriteLine("You won!");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }
    }
}
