namespace A12;

using System.Text;
using static System.Console;

class Program {
   static void Main () {
      Wordle wordle = new ();
      Guess word = new ();
      string crtWord = word.RandomWord ();
      int row = 0;
      CursorVisible = false;
      while (true) {
         wordle.Display ();
         CursorTop = row;
         string guess = GuessWord ();
         EState state = wordle.Run (guess.ToUpper (), crtWord);
         if (state == EState.Valid) row++;
         if (state == EState.Correct || state == EState.AttemptsExhausted) {
            CursorVisible = true;
            break;
         }
      }
   }

   static string GuessWord () {
      int pos = 0;
      int rowPos = WindowWidth / 2 - 5;
      CursorLeft = rowPos;
      StringBuilder guess = new ();
      while (true) {
         _ = DisplayCursor (CursorLeft, CursorTop);
         ConsoleKey key = ReadKey (true).Key;
         switch (key) {
            case ConsoleKey.Backspace:
               if (pos > 0) {
                  guess.Remove (guess.Length - 1, 1);
                  pos -= 2;
                  CursorLeft = rowPos + pos;
                  Write (". ");
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
               }
               break;
         }
      }
   }

   static ConsoleKey DisplayCursor (int x, int y) {
      OutputEncoding = new UnicodeEncoding ();
      bool showCursor = true;
      while (true) {
         SetCursorPosition (x, y);
         Write (showCursor ? '◌': ' ');
         showCursor = !showCursor;
         Thread.Sleep (500);
      }
   }
}
