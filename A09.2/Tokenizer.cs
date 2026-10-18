namespace Eval;

class Tokenizer {
   public Tokenizer (Evaluator eval, string input) {
      mText = input;
      mN = 0;
      mEval = eval;
   }
   readonly string mText;        // The input text we're parsing through
   readonly Evaluator mEval;     // The evaluator that owns this
   int mN;                       // Position within the text
   Token? mPrevToken;

   public Token GetNext () => mPrevToken = PrevToken ();

   Token PrevToken () {
      while (mN < mText.Length) {
         char c = mText[mN++];
         switch (c) {
            case '+' or '-':
               if (mPrevToken is null or TPunctuation { Punct: '(' } or TOpBinary { Op: '=' }) return GetUnaryOp ();
               else return new TOpBinary (mEval, c);
            case '*' or '/' or '^' or '=': return new TOpBinary (mEval, c);
            case ' ': continue;
            case >= '0' and <= '9': return GetLiteral ();
            case '(' or ')': return new TPunctuation (c);
            case (>= 'a' and <= 'z') or (>= 'A' and <= 'Z'): return GetIdentifier ();
            default: return new TError ($"Unexpected character {c}");
         }
      }
      return new TEnd ();
   }


   Token GetLiteral () {
      int start = mN - 1;
      while (mN < mText.Length) {
         char ch = mText[mN++];
         if (!(char.IsDigit (ch) || ch == '.')) { mN--; break; }
      }
      string number = mText[start..mN];
      double num = double.Parse (number);
      return new TLiteral (num);
   }

   Token GetIdentifier () {
      int start = mN - 1;
      while (mN < mText.Length) {
         char ch = mText[mN++];
         if (!(char.IsDigit (ch) || char.IsLetter (ch))) { mN--; break; }
      }
      string identifier = mText[start..mN];
      if (mFuncs.Contains (identifier)) return new TOpFunc (mEval, identifier);
      else return new TVariable (mEval, identifier);
   }

   Token GetUnaryOp () {
      char currOp = mText[mN - 1];
      char nextOp = mN < mText.Length ? mText[mN] : '\0';
      if (currOp == nextOp) {
         mN++;
         return new TOpUnary (mEval, $"{currOp}{nextOp}");
      }
      return new TOpUnary (mEval, currOp.ToString ());
   }
   readonly string[] mFuncs = { "sin", "cos", "tan", "sqrt", "log", "exp", "asin", "acos", "atan" };
}