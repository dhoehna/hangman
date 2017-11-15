module Main where

import Control.Monad (forever)
import Data.Char (toLower)
import Data.Maybe (isJust, fromJust)
import Data.List (intersperse)
import System.Exit (exitSuccess)
import System.Random (randomRIO)

type WordList = [String]

allWords :: IO WordList
allWords = do
  dictionary <- readFile "data/dict.txt"
  return (lines dictionary)
  
  
mininumWordLength :: Int
mininumWordLength = 5

maiximumWordLength :: Int
maiximumWordLength = 9

gameWords :: IO WordList
gameWords = do
  allTheWords <- allWords
  return (filter gameLength allTheWords)
  where gameLength w =
         let l = length (w :: String)
         in l > mininumWordLength && l < maiximumWordLength

randomWord :: WordList -> IO String
randomWord wordList = do
  randomIndex <- randomRIO (0 , (length wordList) - 1)
  return $ wordList !! randomIndex
  
randomWord' :: IO String
randomWord' = gameWords >>= randomWord

bindRandomWords :: IO String
bindRandomWords = gameWords >>= randomWord
  
  
  
data Puzzle = Puzzle String [Maybe Char] [Char]

instance Show Puzzle where
  show (Puzzle _ discovered guessed) =
    (intersperse ' ' $ fmap renderPuzzleChar discovered)
    ++ " Guessed so far: " ++ guessed
    
freshPuzzle :: String -> Puzzle
freshPuzzle wordToGuess = Puzzle wordToGuess (fmap (\x -> Nothing) wordToGuess) []

charInWord :: Puzzle -> Char -> Bool
charInWord (Puzzle wordToGuess _ _) characterGuessed = elem characterGuessed wordToGuess

alreadyGuessed :: Puzzle -> Char -> Bool
alreadyGuessed (Puzzle _ _ alreadyGuessed) guessedCharacter = 
    elem guessedCharacter alreadyGuessed

renderPuzzleChar :: Maybe Char -> Char
renderPuzzleChar maybeCharToShow = if (isJust maybeCharToShow) then (fromJust maybeCharToShow) else '_'

fillInCharacter :: Puzzle -> Char -> Puzzle
fillInCharacter (Puzzle word filledInSoFar s) c = Puzzle word newFilledInSoFar (c : s)
    where newFilledInSoFar = zipWith (zipper c) word filledInSoFar
          zipper guessed wordChar guessChar =
            if wordChar == guessed
            then Just wordChar
            else guessChar
            
handleGuess :: Puzzle -> Char -> IO Puzzle
handleGuess puzzle guess = do
    putStrLn $ "Your guess was: " ++ [guess]
    case (charInWord puzzle guess, alreadyGuessed puzzle guess) of
        (_, True) -> do
            putStrLn "You already guessed that character.  pick something else"
            return puzzle
        (True, _) -> do
            putStrLn "This character was in the word.  Filling in the word accordingly"
            return (fillInCharacter puzzle guess)
        (False, _) -> do
            putStrLn "This character was not in the word. Try again"
            return (fillInCharacter puzzle guess)
            
gameOver :: Puzzle -> IO()
gameOver (Puzzle wordToGuess _ guessed) =
    if (length guessed) > 7 then
        do putStrLn "you lose"
           putStrLn $ "The word was: " ++ wordToGuess
           exitSuccess
    else return ()
    
gameWin :: Puzzle -> IO()
gameWin (Puzzle _ filledInSoFar _) =
    if all isJust filledInSoFar then
        do putStrLn "You win!"
           exitSuccess
    else return()
    
runGame :: Puzzle -> IO()
runGame puzzle = forever $ do
    gameOver puzzle
    gameWin puzzle
    putStrLn $ "Current puzzle is: " ++ show puzzle
    putStr "Guess a letter: "
    guess <- getLine
    case guess of
        [c] -> handleGuess puzzle c >>= runGame
        _ -> putStrLn "Your guess must be a single character"

main :: IO ()
main = do
  word <- randomWord'
  let puzzle = freshPuzzle (fmap toLower word)
  runGame puzzle
