// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// Tokenizer.cs
// Parses the input expression and generates tokens for evaluation
// ------------------------------------------------------------------------------------------------

namespace Eval;

#region Class Tokenizer ---------------------------------------------------------------------------
/// <summary>Parses the input expression and generates tokens for evaluation</summary>
class Tokenizer {
   #region Constructor ----------------------------------------------
   public Tokenizer (Evaluator eval, string input) {
      mText = input;
      mN = 0;
      mEval = eval;
   }
   #endregion

   #region Method ---------------------------------------------------
   /// <summary>Returns the next token from the input expression</summary>
   public Token GetNext () => mPrevToken = PrevToken ();
   #endregion

   #region Implementations ------------------------------------------
   // Reads the next token based on the current character
   Token PrevToken () {
      while (mN < mText.Length) {
         char c = mText[mN++];
         switch (c) {
            case '+' or '-':
               if ((mPrevToken is null or TPunctuation { Punct: '(' } or TOperator) &&
                    mPrevToken is not TNumber) return UnaryOp ();
               else return new TOpBinary (mEval, c);
            case '*' or '/' or '^' or '=': return new TOpBinary (mEval, c);
            case ' ': continue;
            case >= '0' and <= '9': return Literal ();
            case '(' or ')': return new TPunctuation (c);
            case (>= 'a' and <= 'z') or (>= 'A' and <= 'Z'): return Identifier ();
            default: return new TError ($"Unexpected character {c}");
         }
      }
      return new TEnd ();
   }

   // Extracts and converts a numeric literal from the input
   Token Literal () {
      int start = mN - 1;
      while (mN < mText.Length) {
         char ch = mText[mN++];
         if (!(char.IsDigit (ch) || ch == '.')) { mN--; break; }
      }
      string number = mText[start..mN];
      double num = double.Parse (number);
      return new TLiteral (num);
   }

   // Extracts an identifier and determines whether it is a function or variable
   Token Identifier () {
      int start = mN - 1;
      while (mN < mText.Length) {
         char ch = mText[mN++];
         if (!(char.IsDigit (ch) || char.IsLetter (ch))) { mN--; break; }
      }
      string identifier = mText[start..mN];
      if (mFuncs.Contains (identifier)) return new TOpFunc (mEval, identifier);
      else return new TVariable (mEval, identifier);
   }

   // Identifies and returns a unary operator, including repeated operators
   Token UnaryOp () {
      char currOp = mText[mN - 1];
      char nextOp = mN < mText.Length ? mText[mN] : '\0';
      if (currOp == nextOp) {
         mN++;
         return new TOpUnary (mEval, $"{currOp}{nextOp}");
      }
      return new TOpUnary (mEval, currOp.ToString ());
   }
   #endregion

   #region Fields ---------------------------------------------------
   readonly string mText;        // The input text we're parsing through
   readonly Evaluator mEval;     // The evaluator that owns this
   int mN;                       // Position within the text
   Token? mPrevToken;
   readonly string[] mFuncs = { "sin", "cos", "tan", "sqrt", "log", "exp", "asin", "acos", "atan" };
   #endregion
}
#endregion
