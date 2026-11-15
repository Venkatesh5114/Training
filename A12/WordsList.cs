#region class Words -------------------------------------------------------------------------------
class WordsList {
   #region Properties -----------------------------------------------
   /// <summary>Returns a randomly selected word from the guessing word list</summary>
   public string GuessingWord => mGuess[Random.Shared.Next (mGuess.Length)];
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Checks whether the given word exists in the valid word list</summary>
   public bool IsValid(string word) => mValidWords.Contains (word);
   #endregion

   #region Private Data ---------------------------------------------
   readonly string[] mValidWords = File.ReadAllLines ("Validate.txt");
   readonly string[] mGuess = File.ReadAllLines ("guess.txt");
   #endregion
}
#endregion