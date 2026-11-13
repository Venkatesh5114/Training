#region class Words -------------------------------------------------------------------------------
class WordsList {
   #region Properties -----------------------------------------------
   public string GuessingWord => mGuess[new Random ().Next (mGuess.Length)];
   public string[] ValidWords => mValidWords;
   #endregion

   #region Methods --------------------------------------------------
   public bool IsValid(string word) => mValidWords.Contains (word);
   #endregion

   #region Private Data ---------------------------------------------
   readonly string[] mValidWords = File.ReadAllLines ("Validate.txt");
   readonly string[] mGuess = File.ReadAllLines ("guess.txt");
   #endregion
}
#endregion