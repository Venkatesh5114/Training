// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// A02.2: Program to guess a number thought of by the user between 1 and 100 by determining it's
// binary digits from least significant bit to most significant bit.
// ------------------------------------------------------------------------------------------------

using static System.Console;

#region Program -----------------------------------------------------
class Program {
   #region Methods --------------------------------------------------
   static void Main () {
      for (; ; ) {
         WriteLine ($"Think of a number between {MINVALUE} and {MAXVALUE}");
         int guess = 0;
         for (int divisor = 1; divisor <= MAXVALUE; divisor *= BINARYBASE) {
            Display ($"The number when divided by {divisor * BINARYBASE,DISPWIDTH} is remainder " +
                     $"{guess,DISPWIDTH} (Y)es or (N)o: ", ConsoleColor.Yellow);
            ConsoleKey response;
            while ((response = ReadKey (true).Key) is not (ConsoleKey.Y or ConsoleKey.N)) { }
            WriteLine (response);
            if (response == ConsoleKey.N) guess += divisor;
         }
         if (guess >= MINVALUE && guess <= MAXVALUE) {
            Display ($"I guessed it, your number is {guess}", ConsoleColor.Green);
            break;
         }
         Display ("Hints are inconsistent. Press any key to try again or Esc to exit.",
                  ConsoleColor.Red);
         if (ReadKey (true).Key == ConsoleKey.Escape) break;
         Clear ();
      }
   }

   // Displays the message in the specified color.
   static void Display (string message, ConsoleColor color) {
      ForegroundColor = color;
      Write (message);
      ResetColor ();
   }
   #endregion

   #region const ----------------------------------------------------
   const int MINVALUE = 1;
   const int MAXVALUE = 100;
   const int DISPWIDTH = 3;
   const int BINARYBASE = 2;
   #endregion
}
#endregion