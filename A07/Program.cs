// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Trumpf Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// Program to convert string to Double
// ------------------------------------------------------------------------------------------------

using static System.Console;

#region Program -----------------------------------------------------------------------------------
class Program {
   static void Main () {
      string[] testCases = ["10.54e23.4e3", "-1.546234e-4", "0", "0.0", "12345", "0.00000325",
         "10.54e2", "1.5e2.5", "1.e3", "1.e+3", "12.54e3.", "12.", "12.e1", ".325", " +625 ",
         "6.25e0", "6.0e0", "6.25e-1", "+6.25e1", "*6.25", "10.625","15a1", "1.567*2", "+-12",
         "12.-5", ".e1", "-0.325", "  12.456    ", "12e1000", "-4.231"];
      WriteLine ($"{"Input", 15} {"CustomValue", 30} {"Original Value", 30}");
      foreach (string testCase in testCases) {
         double customValue = DoubleParse (testCase);
         double originalValue = double.TryParse (testCase, out double val) ? val : double.NaN;
         Write ($"{testCase, 15}");
         Write ($"{customValue, 30}");
         WriteLine ($"{originalValue, 30}");
      }
   }

   #region Implementation -------------------------------------------
   // Parses the input string and returns a double value.
   static double DoubleParse (string input) {
      input = input.Trim ();
      int index = 0, len = input.Length, sign = 1;
      // Validates that the input is not empty
      if (len == 0) return double.NaN;
      // Handles the double starting with '+' or '-'
      double result = 0;
      var (signedDigit, digitCount) = ParseSignedDigits (true);
      result += signedDigit;
      // Ensures that at least one digit is present before the decimal point
      if (index < len && digitCount == 0) return double.NaN;
      // Checks for a decimal point
      if (index < len && input[index] == '.') {
         index++;
         var (dec, decDigits) = ParseSignedDigits (false);
         // Ensures that the decimal point is followed by digits
         if (decDigits == 0) return double.NaN;
         result += dec / Math.Pow (10, decDigits);
      }
      // Checks for an exponent
      if (index < len && (input[index] == 'E' || input[index] == 'e')) {
         index++;
         var (expo, expoDigits) = ParseSignedDigits (true);
         // Ensures that the exponent contains digits
         if (expoDigits == 0) return double.NaN;
         result *= Math.Pow (10, expo);
      }
      // Rejects the input when unprocessed characters remain
      if (index < len) return double.NaN;
      return result;

      // Extracts consecutive digits and returns the number along with its digit count and sign
      (int signedDigits, int count) ParseSignedDigits (bool parseSign) {
         int digits = 0, count = 0;
         if (parseSign && index < len && (input[index] == '+' || input[index] == '-')) {
            sign = input[index] == '-' ? -1 : 1;
            index++;
         }
         while (index < len && char.IsDigit (input[index])) {
            digits = digits * 10 + (input[index] - '0');
            count++;
            index++;
         }
         return (digits * sign, count);
      }
   }
   #endregion
}
#endregion