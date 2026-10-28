namespace A10._1;

using static EState;

class Program {
   static void Main () {
      Console.WriteLine ("Hello, World!");
   }

   static (string driveLetter, string dir, string fileName, string ext) FileParse(string input) {
      input = input.Trim();
      EState s = A;
      Action none = () => { };
      Action todo;
      string dL = "", dir = "", file = "", ext = "";
      foreach (char c in input) {
         (s, todo) = (s, c) switch {
            (A, >= 'A' and <= 'Z') => (B, () => dL+= c),
            (B, ':') => (C, none),
            (C, '\\') =>  (D, none),
            (D or E, >= 'A' and <= 'Z') => (E, () => dir += c),
            (E, '\\') => ()

         };
      }
      return (dL, dir, file, ext);
   }
}

enum EState { A, B, C, D, E, F , G, H, I , J, Z};
