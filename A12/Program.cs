namespace A12;

using static System.Console;

class Program {
   static void Main () {
      Wordle wordle = new ();
      Guess word = new ();
      string crtWord = word.RandomWord ();
      for (; ; ) {
         Clear ();
         wordle.Display ();
         CursorLeft = WindowWidth / 2 - 20;
         CursorTop = 15;
         Write ("Enter a word: ");
         string guess = ReadLine () ?? "";
         wordle.Run (guess.ToUpper (), crtWord);
         if (guess == "") break;
      }
   }
}
