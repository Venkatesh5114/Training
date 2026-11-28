namespace A14._1;

using static Extensions;

class Program {
   static void Main (string[] args) {
      var words = FileReader ("words.txt");
      var group = words.MyWhere (a => a.Length > 20);
      var crtGrp = words.Where (a => a.Length > 20);
      //foreach(var word in group) Console.WriteLine (word);
      //foreach (var word in crtGrp) Console.WriteLine (word);
      int[] arr = [];
      var sqr = arr.MySelect (a => a * a);
      foreach (int i in sqr) Console.WriteLine (i);
   }
   static IEnumerable<string> FileReader (string filename) {
      string line = "";
      using (var file = File.Open (filename, FileMode.Open))
      using (var sr = new StreamReader (file)) {
         for (; ; ) {
            int n = sr.Read ();
            if (n == -1) break;
            if (n == '\n') { yield return line; line = ""; continue; }
            if (n == '\r') { continue; }
            line += (char)n;
         }
         if (line != "") yield return line;
      }
   }
}


