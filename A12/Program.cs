// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// Entry point for the wordle game.
// ------------------------------------------------------------------------------------------------

#region class Program -----------------------------------------------------------------------------
class Program {
   #region Methods --------------------------------------------------
   static void Main () {
      Console.CursorVisible = false;
      WordsList word = new ();
      Wordle wordle = new (word);
      wordle.Run ();
      Console.CursorVisible = true;
   }
   #endregion
}
#endregion