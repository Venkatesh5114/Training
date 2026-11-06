class Guess {
   public string RandomWord () {
      string[] guess = File.ReadAllLines ("guess.txt");
      Random random = new Random ();
      int index = random.Next (guess.Length);
      return guess [index];
   }
}