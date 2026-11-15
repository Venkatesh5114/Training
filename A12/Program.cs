#region class Program -----------------------------------------------------------------------------
class Program {
   #region Methods --------------------------------------------------
   static void Main () {
      WordsList word = new ();
      Wordle wordle = new (word);
      wordle.Run ();
   }
   #endregion
}
#endregion