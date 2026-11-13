using System.ComponentModel.DataAnnotations;
using System.Text;
using static System.Console;

class Wordle {
   public Wordle(WordsList wordsList) {
      mValidWords = wordsList.ValidWords;
      mWord = wordsList.GuessingWord;
   }
   public void Run () {
      Evaluate ();
   }

   void Display () {
      OutputEncoding = new UnicodeEncoding ();
      int mGame = WindowWidth / 2 - 5;
      CursorTop = 0;
      int count = words.Count;
      for (int i = 0; i < ATTEMPTS; i++) {
         CursorLeft = mGame;
         for (int j = 0; j < LETTERS; j++) {
            if (i * 5 + j < count) {
               var (c, color) = words[i * 5 + j];
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
         if (words.Contains ((c, ConsoleColor.Green))) DisplayLetter (c, ConsoleColor.Green);
         else if (words.Contains ((c, ConsoleColor.Blue))) DisplayLetter (c, ConsoleColor.Blue);
         else if (words.Contains ((c, ConsoleColor.DarkGray))) DisplayLetter (c, ConsoleColor.DarkGray);
         else DisplayLetter (c, ConsoleColor.White);
         if ((i + 1) % KEYPERROW == 0) { WriteLine (); CursorLeft = mKey; }
      }
   }

   void DisplayLetter (char c, ConsoleColor color) {
      ForegroundColor = color;
      Write ($"{c} ");
      ResetColor ();
   }

   string GuessWord () {
      int pos = 0;
      int rowPos = WindowWidth / 2 - 5;
      CursorLeft = rowPos;
      Write ("◌ ");
      StringBuilder guess = new ();
      while (true) {
         ConsoleKey key = ReadKey (true).Key;
         switch (key) {
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
               if (pos == 10) return guess.ToString ();
               break;
            default:
               if (char.IsLetter ((char)key) && pos < 10) {
                  char c = char.ToUpper ((char)key);
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

   void PrintMessage (string message, ConsoleColor color) {
      int x = WindowWidth / 2 - message.Length / 2;
      int y = 12;
      Write (new string (' ', WindowWidth));
      SetCursorPosition (x, y);
      SetCursorPosition (x, y);
      ForegroundColor = color;
      WriteLine (message);
      ResetColor ();
   }

   void Evaluate () {
      int row = 0;
      CursorVisible = false;
      Display ();
      while (true) {
         CursorTop = row;
         mGuess = GuessWord ();
         EState state = UpdateState (mGuess.ToUpper (), mWord);
         Display ();
         switch (state) {
            case EState.Valid: row++; break;
            case EState.Invalid:
               PrintMessage ($"{mGuess} is not a valid word", ConsoleColor.Yellow);
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

   EState UpdateState (string guess, string word) {
      if (mValidWords.Contains (guess)) {
         List<char> guessedChar = [];
         List<(char letter, int index)> parChar = [];
         for (int index = 0; index < WORDSIZE; index++)
            if (guess[index] == word[index]) guessedChar.Add (guess[index]);
         for (int i = 0; i < WORDSIZE; i++) {
            char c1 = guess[i];
            bool added = false;
            for (int j = 0; j < WORDSIZE; j++) {
               char c2 = word[j];
               if (c1 == c2) {
                  if (i == j) {
                     words.Add ((c1, ConsoleColor.Green));
                     added = true;
                     break;
                  }
                  if (i != j && !guessedChar.Contains (c1) && !parChar.Contains ((c1, j))) {
                     words.Add ((c1, ConsoleColor.Blue));
                     parChar.Add ((c1, j));
                     added = true;
                     break;
                  }
               }
            }
            if (!added) words.Add ((c1, ConsoleColor.DarkGray));
         }
      } else return EState.Invalid;
      if (guess == word) return EState.Correct;
      if (words.Count == ATTEMPTS * LETTERS) return EState.AttemptsExhausted;
      return EState.Valid;
   }
   const int KEYPERROW = 7;
   const int LETTERS = 5;
   const int ATTEMPTS = 6;
   const int WORDSIZE = 5;
   List<(char letter, ConsoleColor color)> words = [];

   #region Private Data ---------------------------------------------
   string? mGuess;
   string mWord;
   string[] mValidWords;
   #endregion
}

enum EState {
   Valid,
   Invalid,
   Correct,
   AttemptsExhausted
}