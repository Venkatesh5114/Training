// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// Tokens.cs
// Implements token classes for representing literals, variables, operators, functions, and errors
// ------------------------------------------------------------------------------------------------

namespace Eval;

#region Class Token -------------------------------------------------------------------------------
/// <summary>Base class for all tokens used in expression evaluation</summary>
class Token { }
#endregion

#region Class TNumber -----------------------------------------------------------------------------
/// <summary>Abstract base class for numeric tokens that provide a numerical value</summary>
abstract class TNumber : Token {
   #region Property -------------------------------------------------
   public abstract double Value { get; }
   #endregion
}
#endregion

#region Class TLiteral ----------------------------------------------------------------------------
/// <summary>Represents a numeric literal with a fixed value</summary>
class TLiteral (double value) : TNumber {
   #region Property -------------------------------------------------
   public override double Value => mValue;
   #endregion

   #region Method ---------------------------------------------------
   public override string ToString () => $"literal : {Value}";
   #endregion

   #region Field ----------------------------------------------------
   readonly double mValue = value;
   #endregion
}
#endregion

#region Class TVariable ---------------------------------------------------------------------------
/// <summary>Represents a variable whose value is retrieved from the evaluator</summary>
class TVariable (Evaluator eval, string name) : TNumber () {
   #region Properties -----------------------------------------------
   public string Name { get; } = name;
   public override double Value => mEval.GetVariable (Name);
   #endregion

   #region Method ---------------------------------------------------
   public override string ToString () => $"Variable : {Name}";
   #endregion

   #region Field ----------------------------------------------------
   readonly Evaluator mEval = eval;
   #endregion
}
#endregion

#region Class TOperator ---------------------------------------------------------------------------
/// <summary>Abstract base class for operators, providing priority and evaluator access</summary>
abstract class TOperator (Evaluator eval) : Token {
   #region Properties -----------------------------------------------
   public abstract int Priority { get; }
   public int FinalPriority { get; set; }
   #endregion

   #region Field ----------------------------------------------------
   readonly protected Evaluator mEval = eval;
   #endregion
}
#endregion

#region Class TOpUnary ----------------------------------------------------------------------------
/// <summary>Represents unary operators such as "+", "-", "++", and "--"</summary>
class TOpUnary (Evaluator eval, string op) : TOperator (eval) {
   #region Properties -----------------------------------------------
   public override int Priority => 6;
   public string Op { get; } = op;
   #endregion

   #region Methods --------------------------------------------------
   public double Apply (double num) {
      return Op switch {
         "+" => num,
         "-" => -num,
         "++" => num + 1,
         "--" => num - 1,
         _ => throw new InvalidOperationException ($"Unknown Unary Operator '{Op}'"),
      };
   }

   public override string ToString () => $"Unary : {Op}";
   #endregion
}
#endregion

#region Class TOpBinary ---------------------------------------------------------------------------
/// <summary>Represents binary operators such as '+', '-', '*', '/', '^', and '='</summary>
class TOpBinary (Evaluator eval, char op) : TOperator (eval) {
   #region Properties -----------------------------------------------
   public override int Priority => sPriority[Op];
   public char Op { get; } = op;
   #endregion

   #region Methods --------------------------------------------------
   public double Apply (double a, double b)
      => Op switch {
         '+' => a + b,
         '-' => a - b,
         '*' => a * b,
         '/' => b == 0 ? throw new InvalidOperationException ("Divide by zero") : a / b,
         '^' => Math.Pow (a, b),
         _ => throw new InvalidOperationException ($"Unknown Operator '{Op}'"),
      };

   public override string ToString () => $"Operator : {Op}";
   #endregion

   #region Field ----------------------------------------------------
   static Dictionary<char, int> sPriority = new () {
      ['+'] = 1, ['-'] = 1, ['*'] = 2, ['/'] = 2, ['^'] = 3, ['='] = 4,
   };
   #endregion
}
#endregion

#region Class TOpFunc -----------------------------------------------------------------------------
/// <summary>Represents mathematical functions</summary>
class TOpFunc (Evaluator eval, string func) : TOperator (eval) {
   #region Properties -----------------------------------------------
   public string Func { get; } = func;
   public override int Priority => 5;
   #endregion

   #region Methods --------------------------------------------------
   public double Apply (double f) {
      return Func switch {
         "sin" => Math.Sin (D2R (f)),
         "cos" => Math.Cos (D2R (f)),
         "tan" => Math.Tan (D2R (f)),
         "log" => Math.Log (f),
         "exp" => Math.Exp (f),
         "sqrt" => Math.Sqrt (f),
         "asin" => R2D (Math.Asin (f)),
         "acos" => R2D (Math.Acos (f)),
         "atan" => R2D (Math.Atan (f)),
         _ => throw new NotImplementedException ($"Unknown function '{Func}'"),
      };
      double D2R (double f) => f * Math.PI / 180;
      double R2D (double f) => f * 180 / Math.PI;
   }

   public override string ToString () => $"Function : {Func}";
   #endregion
}
#endregion

#region Class TEnd --------------------------------------------------------------------------------
/// <summary>Represents the end of an expression</summary>
class TEnd : Token {
   #region Method ---------------------------------------------------
   public override string ToString () => "end";
   #endregion
}
#endregion

#region Class TError ------------------------------------------------------------------------------
/// <summary>Represents an error token containing an error message</summary>
class TError (string msg) : Token {
   #region Property -------------------------------------------------
   public string Message { get; } = msg;
   #endregion

   #region Method ---------------------------------------------------
   public override string ToString () => $"Error : {Message}";
   #endregion
}
#endregion

#region Class TPunctuation ------------------------------------------------------------------------
/// <summary>Represents punctuation characters such as '(' and ')' used in expressions</summary>
class TPunctuation (char punct) : Token {
   #region Property -------------------------------------------------
   public char Punct { get; } = punct;
   #endregion

   #region Method ---------------------------------------------------
   public override string ToString () => $"Punctuation : {Punct}";
   #endregion
}
#endregion