// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// WordsList.cs
// Manages the word lists used for word validation and random word selection.
// ------------------------------------------------------------------------------------------------

#region class WordsList ---------------------------------------------------------------------------
class WordsList {
   #region Properties -----------------------------------------------
   /// <summary>Returns a randomly selected word from the guessing word list</summary>
   public string GuessingWord => mGuess[Random.Shared.Next (mGuess.Length)];
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Checks whether the given word exists in the valid word list</summary>
   public bool IsValid (string word) => mValidWords.Contains (word);
   #endregion

   #region Private Data ---------------------------------------------
   readonly string[] mValidWords = File.ReadAllLines ("Validate.txt");
   readonly string[] mGuess = File.ReadAllLines ("guess.txt");
   #endregion
}
#endregion