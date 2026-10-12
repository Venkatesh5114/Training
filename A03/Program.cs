// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Trumpf Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// Program to find valid words from a given set of letters and
// calculate their scores based on specific rules.
// ------------------------------------------------------------------------------------------------
using static System.Console;

class Program {
   static void Main () {
      const int PANGRAMBONUS = 7, MINLENGTH = 4;
      char[] letters = ['U', 'X', 'A', 'L', 'T', 'N', 'E'];
      Dictionary<string, (int Score, bool IsPangram)> result = [];
      foreach (string word in File.ReadLines ("words.txt"))
         if (IsValid (word)) result[word] = GetScoreandPangramStatus (word);
      ConsoleColor previousColor = ForegroundColor;
      foreach (var ans in result.OrderByDescending (x => x.Value.Score).ThenBy (x => x.Key)) {
         var (Score, IsPangram) = ans.Value;
         if (IsPangram) ForegroundColor = ConsoleColor.Green;
         WriteLine ($"{Score, 3} {ans.Key}");
         // Restores the foreground color only if it was changed.
         if (ForegroundColor != previousColor) ForegroundColor = previousColor;
      }
      WriteLine ("----");
      WriteLine ($"{result.Sum (x => x.Value.Score), 3} Total");

      // Checks validity of the word
      bool IsValid (string word)
         => word.Length >= MINLENGTH && word.Contains (letters[0]) && word.All (letters.Contains);

      // Calculates the score of the word and determines whether it is a pangram.
      (int, bool) GetScoreandPangramStatus (string word) {
         bool isPangram = letters.All (word.Contains);
         int length = word.Length;
         int score = length == MINLENGTH ? 1 : (isPangram ? length + PANGRAMBONUS : length);
         return (score, isPangram);
      }
   }
}