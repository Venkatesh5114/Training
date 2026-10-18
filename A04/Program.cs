// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// Program to find the occurence of all letters in word.txt and display the top 7 letters with
// their occurences.
// ------------------------------------------------------------------------------------------------

using static System.Console;
class Program {
   static void Main () {
      Dictionary<char, int> freq = [];
      foreach (string word in File.ReadLines ("words.txt"))
         foreach (char c in word)
            freq[c] = (freq.TryGetValue (c, out int value)) ? ++value : 1;
      WriteLine ("Seven most frequently occuring letters and their occurences");
      foreach (var ch in freq.OrderByDescending (a => a.Value).Take (7))
         WriteLine ($"{ch.Key, 2} : {ch.Value}");
   }
}