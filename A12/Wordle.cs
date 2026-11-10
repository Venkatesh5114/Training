using static System.Console;

class Wordle {
   public EState Run (string guess, string word) {
      string[] validate = File.ReadAllLines ("Validate.txt");
      if (validate.Contains (guess)){
         List<char> guessedChar = [];
         List<(char, int)> parChar = [];
         for (int i = 0; i < guess.Length; i++) {
            char c1 = guess[i];
            bool added = false;
            for (int j = 0; j < word.Length; j++) {
               char c2 = word[j];
               if (c1 == c2 && i == j) {
                  words.Add ((c1, ConsoleColor.Green));
                  added = true;
                  guessedChar.Add (c1);
                  break;
               } else if (c1 == c2 && i != j && !guessedChar.Contains (c1) && !parChar.Contains((c1, j))) {
                  words.Add ((c1, ConsoleColor.Blue));
                  parChar.Add ((c1, j));
                  added = true;
                  break;
               }
            }
            if (!added) words.Add ((c1, ConsoleColor.DarkGray));
         }
      }
      else return EState.Invalid;
      if (guess == word) return EState.Correct;
      if (words.Count == ATTEMPTS * LETTERS) return EState.AttemptsExhausted;
      return EState.Valid;
   }

   public void Display () {
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

   static void DisplayLetter (char c, ConsoleColor color) {
      ForegroundColor = color;
      Write ($"{c} ");
      ResetColor ();
   }

   const int KEYPERROW = 7;
   const int LETTERS = 5;
   const int ATTEMPTS = 6;
   List<(char letter, ConsoleColor color)> words = [];
}

enum EState {
   Valid,
   Invalid,
   Correct,
   AttemptsExhausted
}