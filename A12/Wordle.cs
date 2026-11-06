using System.Security.Cryptography;
using static System.Console;

class Wordle {
   public void Run (string guess, string word) {
      string[] validate = File.ReadAllLines ("Validate.txt");
      if (!validate.Contains (guess)) Print ($"{guess} is not a valid word", ConsoleColor.Yellow);
      else {
         for (int i = 0; i < guess.Length; i++) {
            char c1 = guess[i];
            bool added = false;
            for (int j = 0; j < word.Length; j++) {
               char c2 = word[j];
               if (c1 == c2 && i == j) {
                  words.Add ((c1, ConsoleColor.Green));
                  added = true;
                  break;
               } else if (c1 == c2 && i != j) {
                  words.Add ((c1, ConsoleColor.Blue));
                  added = true;
                  break;
               }
            }
            if (!added) words.Add ((c1, ConsoleColor.DarkGray));
         }
      }
   }

   public void Display () {
      //Clear ();
      for (int i = 0; i < ATTEMPTS; i++) {
         CursorLeft = mGame;
         for (int j = 0; j < LETTERS; j++) {
            if (i * 5 + j < words.Count ()) {
               var (c, color) = words[i * 5 + j];
               DisplayLetter (c, color);
            } else Write (". ");
         }
         WriteLine ();
      }
      CursorLeft = mKey - 2;
      WriteLine (new string ('-', 17));
      CursorLeft = mKey;
      for (int i = 0; i < 26; i++) {
         Write ($"{(char)('A' + i)} ");
         if ((i + 1) % KEYPERROW == 0) { WriteLine (); CursorLeft = mKey; }
      }
      //ReadKey (true);
   }

   void Print (string message, ConsoleColor color) {
      CursorLeft = WindowWidth / 2 - message.Length / 2;
      CursorTop = 12;
      ForegroundColor = color;
      WriteLine (message);
      ResetColor ();
   }

   void DisplayLetter (char c, ConsoleColor color) {
      ForegroundColor = color;
      Write ($"{c} ");
      ResetColor ();
   }

   int mGame = WindowWidth / 2 - 5;
   int mKey = WindowWidth / 2 - 7;
   const int KEYPERROW = 7;
   const int LETTERS = 5;
   const int ATTEMPTS = 6;
   List<(char, ConsoleColor)> words = [];
}

