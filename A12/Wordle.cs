using System.Drawing;
using System.Text;
using static System.Console;

#region class Wordle ------------------------------------------------------------------------------
class Wordle {
   #region Constructor ----------------------------------------------
   public Wordle (WordsList wordsList) {
      mWordsList = wordsList;
      mWord = wordsList.GuessingWord;
   }
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Runs the game by evaluating the guess and displaying the results</summary>
   public void Run () {
      int row = 0;
      CursorVisible = false;
      Display ();
      while (true) {
         CursorTop = row;
         string guess = GuessWord ();
         EState state = UpdateState (guess.ToUpper (), mWord);
         Display ();
         switch (state) {
            case EState.Valid: row++; break;
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
      int mGame = WindowWidth / 2 - 5;
      CursorTop = 0;
      int count = mLetters.Count;
      for (int i = 0; i < ATTEMPTS; i++) {
         CursorLeft = mGame;
         for (int j = 0; j < LETTERS; j++) {
            if (i * 5 + j < count) {
               var (c, color) = mLetters[i * 5 + j];
               DisplayLetter (c, color);
            } else Write ("\u00b7 ");
         }
         WriteLine ();
      }
      int mKey = WindowWidth / 2 - 7;
      CursorLeft = mKey - 2;
      WriteLine (new string ('-', 17));
      CursorLeft = mKey;
      for (int i = 0; i < 26; i++) {
         char c = (char)('A' + i);
         DisplayLetter (c, mKeyColor.TryGetValue (c, out ConsoleColor color) ? color
                                                                             : ConsoleColor.White);
         if ((i + 1) % KEYPERROW == 0) { WriteLine (); CursorLeft = mKey; }
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
                  Write ("\u25cc ");
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
                  if (pos < 10) Write ("◌ ");
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
         List<(char letter, int index)> guessedChar = [];
         for (int index = 0; index < WORDSIZE; index++) {
            char ch = guess[index];
            if (ch == word[index]) guessedChar.Add ((ch, index));
         }
         for (int i = 0; i < WORDSIZE; i++) {
            char c1 = guess[i];
            bool added = false;
            for (int j = 0; j < WORDSIZE; j++) {
               char c2 = word[j];
               if (c1 == c2) {
                  if (i == j) {
                     mLetters.Add ((c1, ConsoleColor.Green));
                     UpdateKeyColor (c1, ConsoleColor.Green);
                     added = true;
                     break;
                  }
                  if (i != j && !guessedChar.Contains ((c1, i)) && !guessedChar.Contains ((c1, j))) {
                     mLetters.Add ((c1, ConsoleColor.Blue));
                     UpdateKeyColor (c1, ConsoleColor.Blue);
                     guessedChar.Add ((c1, j));
                     added = true;
                     break;
                  }
               }
            }
            if (!added) {
               mLetters.Add ((c1, ConsoleColor.DarkGray));
               UpdateKeyColor (c1, ConsoleColor.DarkGray);
            }
         }
      } else return EState.Invalid;
      if (guess == word) return EState.Correct;
      if (mLetters.Count == ATTEMPTS * LETTERS) return EState.AttemptsExhausted;
      return EState.Valid;
   }

   void UpdateKeyColor (char c, ConsoleColor color) {
      if (mKeyColor.TryGetValue (c, out ConsoleColor keyColor)) {
         if (keyColor is ConsoleColor.DarkGray && color is ConsoleColor.Green or ConsoleColor.Blue)
            mKeyColor[c] = color;
         else if (keyColor is ConsoleColor.Blue && color is ConsoleColor.Green)
            mKeyColor[c] = color;
      } else {
         mKeyColor[c] = color;
      }
   }
   #endregion

   #region Constants ------------------------------------------------
   const int KEYPERROW = 7;
   const int LETTERS = 5;
   const int ATTEMPTS = 6;
   const int WORDSIZE = 5;
   #endregion

   #region Private Data ---------------------------------------------
   List<(char letter, ConsoleColor color)> mLetters = [];
   Dictionary<char, ConsoleColor> mKeyColor = [];
   WordsList mWordsList;
   string mWord;
   #endregion

   #region Enum EState -------------------------------------------------
   enum EState {
      Valid,
      Invalid,
      Correct,
      AttemptsExhausted
   }
   #endregion
}
#endregion