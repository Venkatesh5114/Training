// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// Entry point for the wordle game.
// ------------------------------------------------------------------------------------------------

using System.Text;
using static System.Console;

#region class Program -----------------------------------------------------------------------------
class Program {
   #region Methods --------------------------------------------------
   static void Main () {
      OutputEncoding = new UnicodeEncoding ();
      CursorVisible = false;
      try {
         Wordle wordle = new ();
         wordle.Run ();
      } catch (Exception ex) {
         WriteLine ($"Error: {ex.Message}");
      } finally {
         CursorVisible = true;
      }
   }
   #endregion
}
#endregion