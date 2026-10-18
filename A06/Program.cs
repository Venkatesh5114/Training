// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// A06: Program to solve the N-Queens problem and display all possible and unique solutions.
// ------------------------------------------------------------------------------------------------

using static System.Console;

#region Program -----------------------------------------------------
class Program {
   #region Methods --------------------------------------------------
   static void Main () {
      List<int[]> solutions = [];
      int[] rows = new int[N];
      Write ("Would you like to see (A)ll solutions? Press any other key for unique solutions:");
      ConsoleKey response = ReadKey ().Key;
      bool showUniqueSolution = response != ConsoleKey.A;
      Write ("Processing...");
      FindSolutions (0);
      Clear ();
      DisplaySolution ();

      // Finds all possible N-Queens solutions
      void FindSolutions (int row) {
         for (int col = 0; col < N; col++)
            if (IsSafe (row, col)) {
               rows[row] = col;
               if (row == N - 1) AddSolution ([.. rows]);
               else FindSolutions (row + 1);
            }

         // Checks whether a queen can be safely placed at the given position.
         bool IsSafe (int row, int col) {
            for (int i = 0; i < row; i++) {
               int placedCol = rows[i];
               if (placedCol == col || Math.Abs (col - placedCol) == row - i) return false;
            }
            return true;
         }

         // Adds the solution directly or only if it is unique, based on the selected mode.
         void AddSolution (int[] solution) {
            if (!showUniqueSolution || IsUnique (solution)) solutions.Add (solution);
         }

         // Checks whether an equivalent solution exists through rotations or reflections.
         bool IsUnique (int[] solution) {
            for (int i = 0; i < ROTATIONS; i++) {
               solution = Rotate (solution);
               if (SolutionExists (solution) || SolutionExists (Mirror (solution))) return false;
            }
            return true;
         }

         // Checks whether the given solution already exists.
         bool SolutionExists (int[] solution) => solutions.Any (x => x.SequenceEqual (solution));

         // Rotates the solution by 90 degrees.
         int[] Rotate (int[] solution) {
            int[] rotated = new int[N];
            for (int row = 0; row < N; row++) rotated[solution[row]] = N - 1 - row;
            return rotated;
         }

         // Creates a mirrored version of the given solution.
         int[] Mirror (int[] solution) => [.. solution.Reverse ()];
      }

      // Displays the solutions and allows navigation between them.
      void DisplaySolution () {
         OutputEncoding = new System.Text.UnicodeEncoding ();
         int currSoln = 0, totalSolns = solutions.Count;
         Clear ();
         while (currSoln < totalSolns) {
            Clear ();
            WriteLine ($"\nSolution {currSoln + 1} of {totalSolns} ");
            DisplayBoard (solutions[currSoln]);
            Write ("\nPress \u2192 to see the next solution... " +
               "\nPress \u2190 to see the previous solution... \nPress any key to exit... ");
            switch (ReadKey (true).Key) {
               case ConsoleKey.RightArrow: currSoln = Math.Min (currSoln + 1, totalSolns - 1); break;
               case ConsoleKey.LeftArrow: currSoln = Math.Max (currSoln - 1, 0); break;
               default: return;
            }
         }
      }

      // Displays the board for the given solution.
      void DisplayBoard (int[] solution) {
         WriteLine (Border (TOP));
         for (int i = 0; i < N; i++) {
            int placedQueen = solution[i];
            Write (VERTICAL);
            for (int j = 0; j < N; j++) {
               Write (placedQueen == j ? QUEEN : EMPTY);
               Write (VERTICAL);
            }
            WriteLine ();
            if (i < N - 1) WriteLine (Border (MIDDLE));
         }
         WriteLine (Border (BOTTOM));

         // Builds a horizontal border line from the given corner and joint characters.
         string Border (string pattern) {
            string border = pattern[0].ToString ();
            for (int i = 1; i <= N; i++)
               border += HORIZONTAL + (i == N ? pattern[2] : pattern[1]);
            return border;
         }
      }
   }
   #endregion

   #region const ----------------------------------------------------
   const string TOP = "┌┬┐";
   const string MIDDLE = "├┼┤";
   const string BOTTOM = "└┴┘";
   const string VERTICAL = "│";
   const string HORIZONTAL = "────";
   const string EMPTY = "    ";
   const string QUEEN = " ♕  ";
   const int ROTATIONS = 4;
   const int N = 8;                   // Number of Queens
   #endregion
}
#endregion