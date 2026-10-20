// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// Entry point to execute predefined test cases and evaluate interactive user input.
// ------------------------------------------------------------------------------------------------

namespace Eval;

#region Class Program -----------------------------------------------------------------------------
/// <summary> </summary>
class Program () {
   #region Method ---------------------------------------------------
   static void Main () {
      var evaluate = new Evaluator ();
      for (; ; ) {
         Console.Write ("> ");
         string input = Console.ReadLine () ?? "";
         if (input == null || input == "") break;
         try {
            Console.ForegroundColor = ConsoleColor.Green;
            double result = evaluate.Evaluate (input);
            Console.WriteLine (result);
         } catch (Exception e) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine (e.Message);
         }
         Console.ResetColor ();
      }
   }
   #endregion
}
#endregion