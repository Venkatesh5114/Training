// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// Wordle.cs
// Implements the Wordle game by handling user input, word validation and result display.
// ------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Text;
using static System.Console;

#region class Wordle ------------------------------------------------------------------------------
class Wordle {
   #region Constructor ----------------------------------------------
   /// <summary>Initializes the game with the supplied word list and target word</summary>
   public Wordle () {
      mSelectWords = ReadWordList ("guess.txt");
      mValidWords = [.. ReadWordList ("Validate.txt").Concat (mSelectWords)];
   }
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Runs the Wordle game until the word is guessed or attempts are exhausted</summary>
   public void Run () {
      Display ();
      string word = SelectWord ();
      bool isValid = true;
      while (true) {
         string guess = ReadGuess (isValid);
         EState state = UpdateState (guess, word);
         switch (state) {
            case EState.Valid: isValid = true; break;
            case EState.Invalid:
               isValid = false;
               PrintMessage ($"{guess} is not a valid word", ConsoleColor.Yellow);
               break;
            case EState.Correct:
               PrintMessage ($"You guessed the word {word}!", ConsoleColor.Green);
               return;
            case EState.AttemptsExhausted:
               PrintMessage ($"The word was {word}", ConsoleColor.Red);
               return;
         }
         if (isValid) Display ();
      }
   }
   #endregion

   #region Implementation -------------------------------------------
   // Displays the game board and updates the on-screen keyboard
   void Display () {
      int gamePosition = WindowWidth / 2 - 5;
      CursorTop = 0;
      for (int i = 0; i < ATTEMPTS; i++) {
         CursorLeft = gamePosition;
         for (int j = 0; j < WORDSIZE; j++) {
            int position = i * WORDSIZE + j;
            if (mLetters[position].letter != default) {
               var (c, color) = mLetters[position];
               DisplayLetter (c, color);
            } else Write (mDot);
         }
         WriteLine ();
      }
      int keyPosition = WindowWidth / 2 - 7;
      CursorLeft = keyPosition - 2;
      WriteLine (new string (mLineChar, 17));
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
   string ReadGuess (bool isPrevValid) {
      int cursorPos = WindowWidth / 2 + WORDSIZE;
      if (isPrevValid) {
         cursorPos = WindowWidth / 2 - WORDSIZE;
         inputChar.Clear ();
      }
      SetCursorPosition (cursorPos, mRow);
      if (isPrevValid) Write (mCircle);
      while (true) {
         var key = ReadKey (true);
         switch (key.Key) {
            case ConsoleKey.Backspace or ConsoleKey.LeftArrow:
               if (inputChar.Length > 0) {
                  inputChar.Remove (inputChar.Length - 1, 1);
                  cursorPos -= 2;
                  CursorLeft = cursorPos;
                  Write (mCircle);
                  if (inputChar.Length < WORDSIZE - 1) Write (mDot);
                  CursorLeft = cursorPos;
               }
               break;
            case ConsoleKey.Enter:
               if (inputChar.Length == WORDSIZE) return inputChar.ToString ();
               break;
            default:
               if (char.IsLetter (key.KeyChar) && inputChar.Length < 5) {
                  char c = char.ToUpper (key.KeyChar);
                  CursorLeft = cursorPos;
                  Write ($"{c} ");
                  inputChar.Append (c);
                  cursorPos += 2;
                  if (inputChar.Length < WORDSIZE) Write (mCircle);
               }
               break;
         }
      }
   }

   // Selects a random word from the available words
   string SelectWord () => mSelectWords[Random.Shared.Next (mSelectWords.Length)];

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
      if (mValidWords.Contains (guess)) {
         // Stores the positions of letters already matched in the target word.
         HashSet<int> matchedPositions = [];
         // Identify letters that match at the correct positions
         for (int index = 0; index < WORDSIZE; index++) {
            char ch = guess[index];
            if (ch == word[index]) {
               matchedPositions.Add (index);
               mLetters[mRow * WORDSIZE + index] = new (ch, ConsoleColor.Green);
               UpdateKeyColor (ch, ConsoleColor.Green);
            }
         }
         // Identify letters present in the word but at incorrect positions
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
            // Assigns the guessed character and updates its key color if the position is empty
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

   // Reads words from the specified embedded text file and returns them as an array
   string[] ReadWordList (string name) {
      var assembly = Assembly.GetExecutingAssembly ();
      var resourceName = assembly.GetManifestResourceNames ()
         .First (resourse => resourse.EndsWith (name));
      using var stream = assembly.GetManifestResourceStream (resourceName)!;
      using var reader = new StreamReader (stream);
      string[] words = reader.ReadToEnd ()
                             .Split (["\r\n", "\r", "\n"], StringSplitOptions.RemoveEmptyEntries);
      return words;
   }

   #region Private Data ---------------------------------------------
   LetterColor[] mLetters = new LetterColor[ATTEMPTS * WORDSIZE];
   Dictionary<char, ConsoleColor> mKeyColor = [];
   HashSet<string> mValidWords;
   string[] mSelectWords;
   int mRow = 0;
   char mLineChar = '-';
   string mCircle = "◌ ";
   string mDot = "\u00b7 ";
   StringBuilder inputChar = new ();
   #endregion

   #region Constants ------------------------------------------------
   const int KEYPERROW = 7;
   const int ATTEMPTS = 6;
   const int WORDSIZE = 5;
   #endregion
}
#endregion

#region Struct LetterColor ------------------------------------------
/// <summary>Stores a character and its associated display color</summary>
record struct LetterColor (char letter, ConsoleColor color);
#endregion

#region Enum EState -------------------------------------------------
enum EState {
   Valid,               // Guess is valid but does not match the target word
   Invalid,             // Guess is not present in the word list
   Correct,             // Guess matches the target word
   AttemptsExhausted    // Maximum number of attempts has been reached
}
#endregion