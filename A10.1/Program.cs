

namespace A10._1;

using static EState;

#region class Program -----------------------------------------------------------------------------
class Program {
   #region Method ---------------------------------------------------
   static void Main () {
      string[] inputs = [@"D:\WORK\TRAINING\FILE.TXT", @"C:\A\B\C\D\E\F\G\FILE.TXT",
                         @"CZ:\AB\CD\E.TXT", @"C:README.TXT", @"C:\README.TXT", @"C:\AB\READ.",
                         @"c:\aBc\readme.txt", @"READ.TXT", @"C:\A\B", " ", "abc", @".\ABC",".abc",
                         @"\AB\READ.TXT", @"C:\A.B\DE\R.TXT", @"C:\AB:DC", @"C:\WORK\R.TXT#",
                         @"C:\DIR\ARCHIVE.TAR.GZ", @"C:\WORK23\R.TXT", @"C:\DIR\\FILE.TXT",];
      Console.WriteLine ($" {"Input",-30}| {"Drive",-5}| {"Directory",-15}| {"FileName",-10}| " +
                         $"{"Extension",-10}| {"Status",-10}");
      Console.WriteLine (string.Join ("", Enumerable.Repeat ('-', 100)));
      foreach (string input in inputs) {
         var (drive, dir, fileName, ext, isPassed) = FileParse (input);
         Console.Write ($" {input,-30}| {drive,-5}| {dir,-15}| {fileName,-10}| {ext,-10}| ");
         Console.ForegroundColor = isPassed ? ConsoleColor.Green : ConsoleColor.Red;
         Console.WriteLine ($"{(isPassed ? "Pass" : "Fail"),-10}");
         Console.ResetColor ();
      }
   }
   #endregion

   #region Implementation -------------------------------------------
   static (string, string, string, string, bool) FileParse (string input) {
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
            (E, '\\') => (D, () => (dir, file) = (dir = dir.Length == 0 ? file : dir + ',' + file, "")),
            (E, '.') => (s = dir.Length == 0 ? Z : F, none),
            (F or G, >= 'A' and <= 'Z') => (G, () => ext += c),
            (G, '~') => (H, none),
            _ => (Z, none),
         };
         todo ();
      }
      bool isPass = s == H;
      if (isPass) return (dL, dir, file, ext, isPass);
      return ("-", "-", "-", "-", isPass);
   }
   #endregion
}
#endregion

#region Enum EState -------------------------------------------------------------------------------
enum EState { A, B, C, D, E, F, G, H, Z };
#endregion