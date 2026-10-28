// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// Tokens.cs
// Implements token classes for representing literals, variables, operators, functions, and errors
// ------------------------------------------------------------------------------------------------

namespace Eval;

#region class Token -------------------------------------------------------------------------------
/// <summary>Base class for all tokens used in expression evaluation</summary>
class Token { }
#endregion

#region class TNumber -----------------------------------------------------------------------------
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

   #region Private data ---------------------------------------------
   readonly double mValue = value;
   #endregion
}
#endregion

#region class TVariable ---------------------------------------------------------------------------
/// <summary>Represents a variable whose value is retrieved from the evaluator</summary>
class TVariable (Evaluator eval, string name) : TNumber () {
   #region Property -------------------------------------------------
   public string Name { get; } = name;
   public override double Value => mEval.GetVariable (Name);
   #endregion

   #region Method ---------------------------------------------------
   public override string ToString () => $"Variable : {Name}";
   #endregion

   #region Private data ---------------------------------------------
   readonly Evaluator mEval = eval;
   #endregion
}
#endregion

#region class TOperator ---------------------------------------------------------------------------
/// <summary>Abstract base class for operators, providing priority and evaluator access</summary>
abstract class TOperator (Evaluator eval) : Token {
   #region Property -------------------------------------------------
   public abstract int Priority { get; }
   public int FinalPriority { get; set; }
   #endregion

   #region Private data ---------------------------------------------
   readonly protected Evaluator mEval = eval;
   #endregion
}
#endregion

#region class TOpUnary ----------------------------------------------------------------------------
/// <summary>Represents unary operators such as "+", "-", "++", and "--"</summary>
class TOpUnary (Evaluator eval, string op) : TOperator (eval) {
   #region Property -------------------------------------------------
   public override int Priority => 3 + mEval.BasePriority;
   public string Op { get; } = op;
   #endregion

   #region Method ---------------------------------------------------
   public double Apply (double num) {
      return Op switch {
         "+" => num,
         "-" => -num,
         "++" => num + 1,
         "--" => num - 1,
         _ => throw new InvalidOperationException ($"Unknown Unary Operator '{Op}'"),
      };
   }

   public override string ToString () => $"Unary : {Op} Priority: {Priority}";
   #endregion
}
#endregion

#region class TOpBinary ---------------------------------------------------------------------------
/// <summary>Represents binary operators such as '+', '-', '*', '/', '^', and '='</summary>
class TOpBinary (Evaluator eval, char op) : TOperator (eval) {
   #region Property -------------------------------------------------
   public override int Priority => sPriority[Op] + mEval.BasePriority;
   public char Op { get; } = op;
   #endregion

   #region Method ---------------------------------------------------
   public double Apply (double a, double b)
      => Op switch {
         '+' => a + b,
         '-' => a - b,
         '*' => a * b,
         '/' => b == 0 ? throw new EvalException ("Divide by zero") : a / b,
         '^' => Math.Pow (a, b),
         _ => throw new EvalException ($"Unknown Operator '{Op}'"),
      };

   public override string ToString () => $"Operator : {Op} Priority: {Priority}";
   #endregion

   #region Private data ---------------------------------------------
   static Dictionary<char, int> sPriority = new () {
      ['+'] = 1, ['-'] = 1, ['*'] = 2, ['/'] = 2, ['^'] = 5, ['='] = 0,
   };
   #endregion
}
#endregion

#region class TOpFunc -----------------------------------------------------------------------------
/// <summary>Represents mathematical functions</summary>
class TOpFunc (Evaluator eval, string func) : TOperator (eval) {
   #region Property -------------------------------------------------
   public string Func { get; } = func;
   public override int Priority => 4 + mEval.BasePriority;
   #endregion

   #region Method ---------------------------------------------------
   public double Apply (double f) {
      return Func switch {
         "sin" => Math.Sin (D2R (f)),
         "cos" => Math.Cos (D2R (f)),
         "tan" => Math.Tan (D2R (f)),
         "log" => f > 0 ? Math.Log(f) : throw new EvalException("Expected number greater than 0") ,
         "exp" => Math.Exp (f),
         "sqrt" => f > 0 ? Math.Sqrt (f) : throw new EvalException ("Expected positive number"),
         "asin" => R2D (Math.Asin (f)),
         "acos" => R2D (Math.Acos (f)),
         "atan" => R2D (Math.Atan (f)),
         _ => throw new EvalException ($"Unknown function '{Func}'"),
      };
      double D2R (double f) => f * Math.PI / 180;
      double R2D (double f) => f * 180 / Math.PI;
   }

   public override string ToString () => $"Function : {Func} Priority: {Priority}";
   #endregion
}
#endregion

#region class TEnd --------------------------------------------------------------------------------
/// <summary>Represents the end of an expression</summary>
class TEnd : Token {
   #region Method ---------------------------------------------------
   public override string ToString () => "end";
   #endregion
}
#endregion

#region class TError ------------------------------------------------------------------------------
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

#region class TPunctuation ------------------------------------------------------------------------
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