// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// Program to generate a random number between 1 and 100 and let the user guess it.
// ------------------------------------------------------------------------------------------------
class Program {
   static void Main () {
      int num = new Random ().Next (1, 101), guessedNum = 0, count = 0;
      while (guessedNum != num) {
         Console.Write ("Enter your guess (1-100): ");
         guessedNum = ValidGuess ();
         count++;
         if (guessedNum == num) break;
         Console.WriteLine ($"Your guess is too {(guessedNum < num ? "low" : "high")}");
      }
      Console.WriteLine ($"You guessed correctly in {count} tries.");

      // Reads the user input and validates it to be a number between 1 and 100.
      static int ValidGuess () {
         for (; ; ) {
            if (int.TryParse (Console.ReadLine (), out int n) && n >= 1 && n <= 100) return n;
            Console.Write ("Enter a valid number: ");
         }
      }
   }
}

