namespace A10._1;

using static EState;

#region class Program -----------------------------------------------------------------------------
class Program {
   #region Method ---------------------------------------------------
   static void Main () {
      string[] inputs = [@"Cz:\abc\def\r.txt", @"C:\abc\def\readme.txt", @"C:\README.TXT", @"C:\abc\.bcf",
                         @"C:\abc\bcf.", @"Readme.txt", @"C:\abc\def", @"C:\abc d", @"\abcd\Readme.txt", " ",
                         @"C:\ab.c\def\r.txt", @"C:\abc:d", @".\abc", ".abc", "abc", @"C:\abc6\def\r.txt",
                         @"C:\DIR\ARCHIVE.TAR.GZ", @"C:\work\r.txt", @"C:\\work~\\r.txt~",
                         @"C:\A\B\C\D\E\F\G\FILE.TXT", @"C:\DIR\\FILE.TXT",@"D:\WORK\TRAINING\FILE.TXT"];
      Console.WriteLine ($"{"Input"}");
      foreach(string input in inputs) {
         Console.WriteLine (FileParse(input));
      }
   }
   #endregion

   #region Implementation -------------------------------------------
   static (string, string, string, string,bool ) FileParse (string input) {
      input = input.Trim () + "~";
      EState s = A;
      Action none = () => { };
      Action todo;
      string dL = "", dir = "", file = "", ext = "";
      foreach (char c in input) {
         (s, todo) = (s, c) switch {
            (A, >= 'A' and <= 'Z') => (B, () => dL += c),
            (B, ':') => (C, none),
            (C, '\\') => (D, none),
            (D or E, >= 'A' and <= 'Z') => (E, () => file += c),
            (E, '\\') => (F, () => (dir, file) = (dir += file, "")),
            (F or G, >= 'A' and <= 'Z') => (G, () => file += c),
            (G, '\\') => (F, () => (dir, file) = (dir = dir + "," + file, "")),
            (G, '.') => (H, none),
            (H or I, >= 'A' and <= 'Z') => (I, () => ext += c),
            (I, '~') => (J, none),
            _ => (Z, none),
         };
         todo ();
      }
      bool isPass = s == J;
      if (isPass) return (dL, dir, file, ext, isPass);
      return ("", "", "", "", isPass);
   }
   #endregion
}
#endregion

#region Enum EState -------------------------------------------------------------------------------
enum EState { A, B, C, D, E, F, G, H, I, J, Z };
#endregion