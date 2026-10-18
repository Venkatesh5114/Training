// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// Program to guess a number thought of by the user between 1 and 100 using binary search.
// ------------------------------------------------------------------------------------------------

using static System.Console;

#region Program -----------------------------------------------------
class Program {

   #region Methods --------------------------------------------------
   static void Main () {
      int low = MINVALUE, high = MAXVALUE;
      WriteLine ($"Think of a number between {low} and {high}: ");
      WriteLine ("H: If the number is greater than guess\nL: If the number is less than guess" +
                 "\nY: If guessed correctly ");
      while (low <= high) {
         int guess = low + (high - low) / 2;
         Display ($"Is your number {guess,3} (Y)es (H)igh (L)ow: ", ConsoleColor.Yellow);
         ConsoleKey response;
         do response = ReadKey (true).Key;
         while (response is not (ConsoleKey.Y or ConsoleKey.L or ConsoleKey.H));
         WriteLine (response);
         switch (response) {
            case ConsoleKey.Y:
               Display ($"I guessed it, your number is {guess}.", ConsoleColor.Green);
               return;
            case ConsoleKey.H: low = guess + 1; break;
            case ConsoleKey.L: high = guess - 1; break;
         }
      }
      Display ("Hints are inconsistent. Please Try again", ConsoleColor.Red);
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
   #endregion
}
#endregion