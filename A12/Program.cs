namespace A12;

using System.Text;
using static System.Console;

class Program {
   static void Main () {
      WordsList word = new ();
      Wordle wordle = new (word);
      wordle.Run ();
   }
}
