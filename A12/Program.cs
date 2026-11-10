namespace A12;

using System.Text;
using static System.Console;

class Program {
   static void Main () {
      OutputEncoding = new UnicodeEncoding ();
      Wordle wordle = new ();
      Guess word = new ();
      string crtWord = "CARRY";
      int row = 0;
      CursorVisible = false;
      wordle.Display ();
      while (true) {
         CursorTop = row;
         string guess = GuessWord ();
         EState state = wordle.Run (guess.ToUpper (), crtWord);
         wordle.Display ();
         switch (state) {
            case EState.Valid: row++; break;
            case EState.Invalid:
               PrintMessage ($"{guess} is not a valid word", ConsoleColor.Yellow);
               break;
            case EState.Correct:
               PrintMessage ($"You guessed the word {crtWord}!", ConsoleColor.Green);
               return;
            case EState.AttemptsExhausted:
               PrintMessage ($"The word was {crtWord}", ConsoleColor.Red);
               return;
         }
      }
   }

   static string GuessWord () {
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

   static void PrintMessage (string message, ConsoleColor color) {
      int x = WindowWidth / 2 - message.Length / 2;
      int y = 12;
      SetCursorPosition (x, y);
      Write (new string (' ', WindowWidth));
      SetCursorPosition (x, y);
      ForegroundColor = color;
      WriteLine (message);
      ResetColor ();
   }
}
