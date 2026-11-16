// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// Wordle.cs
// Implements the Wordle game by handling user input, word validation, and result display.
// ------------------------------------------------------------------------------------------------

using System.Text;
using static System.Console;

#region class Wordle ------------------------------------------------------------------------------
class Wordle {
   #region Constructor ----------------------------------------------
   /// <summary>Initializes the game with the supplied word list and target word</summary>
   public Wordle (WordsList wordsList) {
      mWordsList = wordsList;
      mWord = wordsList.GuessingWord;
   }
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Runs the Wordle game until the word is guessed or attempts are exhausted</summary>
   public void Run () {
      Display ();
      while (true) {
         CursorTop = mRow;
         string guess = GuessWord ();
         EState state = UpdateState (guess, mWord);
         Display ();
         switch (state) {
            case EState.Invalid:
               PrintMessage ($"{guess} is not a valid word", ConsoleColor.Yellow);
               break;
            case EState.Correct:
               PrintMessage ($"You guessed the word {mWord}!", ConsoleColor.Green);
               return;
            case EState.AttemptsExhausted:
               PrintMessage ($"The word was {mWord}", ConsoleColor.Red);
               return;
         }
      }
   }
   #endregion

   #region Implementation -------------------------------------------
   // Displays the game board and updates the on-screen keyboard
   void Display () {
      OutputEncoding = new UnicodeEncoding ();
      int gamePosition = WindowWidth / 2 - 5;
      CursorTop = 0;
      for (int i = 0; i < ATTEMPTS; i++) {
         CursorLeft = gamePosition;
         for (int j = 0; j < WORDSIZE; j++) {
            int position = i * WORDSIZE + j;
            if (mLetters[position].letter != '\0') {
               var (c, color) = mLetters[position];
               DisplayLetter (c, color);
            } else Write ("\u00b7 ");
         }
         WriteLine ();
      }
      int keyPosition = WindowWidth / 2 - 7;
      CursorLeft = keyPosition - 2;
      WriteLine (new string ('-', 17));
      CursorLeft = keyPosition;
      for (int i = 0; i < 26; i++) {
         char c = (char)('A' + i);
         DisplayLetter (c, mKeyColor.TryGetValue (c, out ConsoleColor color) ? color
                                                                             : ConsoleColor.White);
         if ((i + 1) % KEYPERROW == 0) { WriteLine (); CursorLeft = keyPosition; }
      }
   }

   // Displays a character in the specified foreground color
   void DisplayLetter (char c, ConsoleColor color) {
      ForegroundColor = color;
      Write ($"{c} ");
      ResetColor ();
   }

   // Accepts keyboard input and constructs a five-letter word guess
   string GuessWord () {
      int pos = 0;
      int rowPos = WindowWidth / 2 - 5;
      CursorLeft = rowPos;
      Write ("◌ ");
      StringBuilder guess = new ();
      while (true) {
         var key = ReadKey (true);
         switch (key.Key) {
            case ConsoleKey.Backspace:
               if (pos > 0) {
                  guess.Remove (guess.Length - 1, 1);
                  pos -= 2;
                  CursorLeft = rowPos + pos;
                  Write ("◌ ");
                  if (pos < 8) Write ("\u00b7 ");
                  CursorLeft = rowPos + pos;
               }
               break;
            case ConsoleKey.Enter:
               if (guess.Length == WORDSIZE) return guess.ToString ();
               break;
            default:
               if (char.IsLetter (key.KeyChar) && pos < 10) {
                  char c = char.ToUpper (key.KeyChar);
                  CursorLeft = rowPos + pos;
                  Write ($"{c} ");
                  guess.Append (c);
                  pos += 2;
                  if(pos < 10) Write ("◌ ");
               }
               break;
         }
      }
   }

   // Displays a centered message in the specified foreground color
   void PrintMessage (string message, ConsoleColor color) {
      int x = WindowWidth / 2 - message.Length / 2;
      int y = 12;
      SetCursorPosition (0, y);
      Write (new string (' ', WindowWidth));
      SetCursorPosition (x, y);
      ForegroundColor = color;
      WriteLine (message);
      ResetColor ();
   }

   // Validates the guess, evaluates letter positions, and updates their colors
   EState UpdateState (string guess, string word) {
      if (mWordsList.IsValid (guess)) {
         HashSet<int> matchedPositions = [];
         for (int index = 0; index < WORDSIZE; index++) {
            char ch = guess[index];
            if (ch == word[index]) {
               matchedPositions.Add (index);
               mLetters[mRow * WORDSIZE + index] = new (ch, ConsoleColor.Green);
               UpdateKeyColor (ch, ConsoleColor.Green);
            }
         }
         for (int i = 0; i < WORDSIZE; i++) {
            char guessChar = guess[i];
            int position = mRow * WORDSIZE + i;
            for (int j = 0; j < WORDSIZE; j++) {
               char wordChar = word[j];
               if (guessChar == wordChar) {
                  if (i != j && !matchedPositions.Contains (j)) {
                     mLetters[position] = new (guessChar, ConsoleColor.Blue);
                     UpdateKeyColor (guessChar, ConsoleColor.Blue);
                     matchedPositions.Add (j);
                     break;
                  }
               }
            }
            if (mLetters[position] == default) {
               mLetters[position] = new (guessChar, ConsoleColor.DarkGray);
               UpdateKeyColor (guessChar, ConsoleColor.DarkGray);
            }
         }
         mRow++;
      } else return EState.Invalid;
      if (guess == word) return EState.Correct;
      if (mRow == ATTEMPTS) return EState.AttemptsExhausted;
      return EState.Valid;
   }

   // Updates the keyboard color based on the highest priority color
   void UpdateKeyColor (char c, ConsoleColor color) {
      if (!mKeyColor.TryGetValue (c, out ConsoleColor keyColor) || keyColor is ConsoleColor.Blue
                                                                && color is ConsoleColor.Green)
         mKeyColor[c] = color;
   }
   #endregion

   #region Constants ------------------------------------------------
   const int KEYPERROW = 7;
   const int ATTEMPTS = 6;
   const int WORDSIZE = 5;
   #endregion

   #region Private Data ---------------------------------------------
   LetterColor[] mLetters = new LetterColor[ATTEMPTS * WORDSIZE];
   Dictionary<char, ConsoleColor> mKeyColor = [];
   WordsList mWordsList;
   string mWord;
   int mRow = 0;
   #endregion
}
#endregion

#region Struct LetterColor ------------------------------------------
/// <summary>Stores a character and its associated display color</summary>
record struct LetterColor (char letter, ConsoleColor color);
#endregion
#region Enum EState -------------------------------------------------
enum EState {
   Valid,
   Invalid,
   Correct,
   AttemptsExhausted
}
#endregion