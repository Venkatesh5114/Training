// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// Evaluates mathematical expressions interactively and through predefined test cases.
// ------------------------------------------------------------------------------------------------

namespace Eval;

#region class Program -----------------------------------------------------------------------------
/// <summary>Runs the expression evaluator and its test cases</summary>
class Program () {
   #region Method ---------------------------------------------------
   /// <summary>Starts interactive mode or test cases based on user input</summary>
   static void Main () {
      Print ("Enter (I)nteractive mode or (T)estcases or any other key for exit: ");
      switch (Console.ReadKey ().Key) {
         case ConsoleKey.I: Console.Clear (); InteractiveMode (); break;
         case ConsoleKey.T: Console.Clear (); TestCases (); break;
         default: return;
      }
   }
   #endregion

   #region Implementation -------------------------------------------
   // Evaluates user-entered expressions and displays results or errors
   static void InteractiveMode () {
      var evaluate = new Evaluator ();
      for (; ; ) {
         Print ("> ");
         string input = Console.ReadLine () ?? "";
         if (input == null || input == "") break;
         try {
            double result = evaluate.Evaluate (input);
            Print ($"{result:G15} \n", ConsoleColor.Green);
         } catch (Exception e) {
            Print ($"{e.Message} \n", ConsoleColor.Red);
         }
      }
   }

   // Executes predefined test cases and displays their results and status
   static void TestCases () {
      var evaluate = new Evaluator ();
      var tests = new (string Input, double Expected)[] {
         // Unary operators
         // Valid cases
         ("a = 10", 10),
         ("+a", 10),
         ("-a", -10),
         ("++a", 11),
         ("a", 11),
         ("--a", 10),
         ("a", 10),
         // Invalid cases
         ("++5", NAN),
         // Arithmetic operators
         // Valid cases
         ("2+3", 5),
         ("10-4", 6),
         ("4*5", 20),
         ("20/4", 5),
         ("2^3", 8),
         ("10--4", 14),
         ("10+-4", 6),
         // Invalid cases
         ("5++++2", NAN),
         ("3+", NAN),
         ("3 + 2 3", NAN),
         ("5/0", NAN),
         // Operator precedence
         // Valid cases
         ("2+3*4", 14),
         ("(2+3)*4", 20),
         ("10-3-2", 5),
         ("20/5/2", 2),
         ("2^3^2", 512),
         ("-2^2", -4),
         ("(-2)^2", 4),
         // Invalid cases
         ("2 + (3* 4", NAN),
         ("2  (3* 4)", NAN),
         // Mathematical functions
         ("sqrt 16", 4),
         ("sin 30", 0.5),
         ("cos(60)", 0.5),
         ("log(1)", 0),
         ("exp(0)", 1),
         ("asin(0.5)", 30),
         ("acos(0.5)", 60),
         ("cos(-60)", 0.5),
         ("-cos(60)", -0.5),
         // Invalid cases
         ("abs(-23)", NAN),
         // Variable assignment and retrieval
         ("x=10", 10),
         ("x+5", 15),
         ("x = x*2", 20),
         ("x", 20),
         // Invalid cases
         ("x + y", NAN)
      };
      Print ($"{"Expression",WIDTH3} | {"Expected",WIDTH2} | {"Actual",WIDTH2} | " +
                         $" {"Status",WIDTH1} | Error Message (if NaN) \n");
      Print ($"{DashedLines} \n");
      foreach (var (Input, Expected) in tests) {
         bool iPass;
         try {
            double actual = evaluate.Evaluate (Input);
            iPass = Math.Abs (actual - Expected) < TOLERANCE;
            Print ($"{Input,WIDTH3} | {Expected,WIDTH2:G15} | {actual,WIDTH2:G15} | ");
            Print ($"{(iPass ? "PASS" : "FAIL"),WIDTH1} ", iPass ? ConsoleColor.Green
                                                                : ConsoleColor.Red);
            Print (" | \n");
         } catch (Exception e) {
            iPass = double.IsNaN (Expected);
            Print ($"{Input,WIDTH3} | {$"{Expected}",WIDTH2} | {$"{NAN}",WIDTH2} |");
            Print ($" {(iPass ? "PASS" : "FAIL"),WIDTH1} ", iPass ? ConsoleColor.Green
                                                                : ConsoleColor.Red);
            Print (" |");
            Print ($" {e.Message} \n", ConsoleColor.Red);
         }
      }
      Print ($"{DashedLines} \n");
   }

   // Prints the text in the specified color or the default console color if none is provided.
   static void Print (string text, ConsoleColor? color = null) {
      ConsoleColor iDefaultColor = Console.ForegroundColor;
      Console.ForegroundColor = color ?? iDefaultColor;
      Console.Write (text);
      Console.ForegroundColor = iDefaultColor;
   }
   #endregion

   #region Private data ---------------------------------------------
   static readonly string DashedLines = new ('-', 84);
   const int WIDTH1 = -8;
   const int WIDTH2 = -12;
   const int WIDTH3 = -15;
   const double NAN = double.NaN;
   const double TOLERANCE = 1e-6;
   #endregion
}
#endregion