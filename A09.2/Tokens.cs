// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// Entry point to execute predefined test cases and evaluate interactive user input.
// ------------------------------------------------------------------------------------------------

namespace Eval;

#region Class Token -------------------------------------------------------------------------------
/// <summary></summary>
class Token { }
#endregion

#region Class TNumber -----------------------------------------------------------------------------
/// <summary></summary>
abstract class TNumber : Token {
   #region Property -------------------------------------------------
   public abstract double Value { get; }
   #endregion
}
#endregion

#region Class TLiteral ----------------------------------------------------------------------------
/// <summary></summary>
class TLiteral (double value) : TNumber {
   #region Property -------------------------------------------------
   public override double Value => mValue;
   #endregion

   #region Method ---------------------------------------------------
   /// <summary></summary>
   public override string ToString () => $"literal : {Value}";
   #endregion

   #region Field ----------------------------------------------------
   readonly double mValue = value;
   #endregion
}
#endregion

#region Class TVariable ---------------------------------------------------------------------------
/// <summary></summary>
class TVariable (Evaluator eval, string name) : TNumber() {
   #region Properties -----------------------------------------------
   public string Name { get; } = name;
   public override double Value => mEval.GetVariable (Name);
   #endregion

   #region Method ---------------------------------------------------
   /// <summary></summary>
   public override string ToString () => $"Variable : {Name}";
   #endregion

   #region Field ----------------------------------------------------
   readonly Evaluator mEval = eval;
   #endregion
}
#endregion

#region Class TOperator ---------------------------------------------------------------------------
/// <summary></summary>
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
/// <summary></summary>
class TOpUnary (Evaluator eval, string op) : TOperator(eval) {
   #region Properties -----------------------------------------------
   public override int Priority => 6 + mEval.BasePriority;
   public string Op { get; } = op;
   #endregion

   #region Methods --------------------------------------------------
   /// <summary></summary>
   public double Apply (double num) {
      return Op switch {
         "+" => num,
         "-" => -num,
         "++" => num + 1,
         "--" => num - 1,
         _ => throw new InvalidOperationException (),
      };
   }

   /// <summary></summary>
   public override string ToString () => $"Unary : {Op}";
   #endregion
}
#endregion

#region Class TOpBinary ---------------------------------------------------------------------------
/// <summary></summary>
class TOpBinary (Evaluator eval, char op) : TOperator(eval) {
   #region Properties -----------------------------------------------
   public override int Priority => sPriority[Op] + mEval.BasePriority;
   public char Op { get; } = op;
   #endregion

   #region Methods --------------------------------------------------
   /// <summary></summary>
   public override string ToString () => $"Operator : {Op}";

   /// <summary></summary>
   public double Apply (double a, double b)
      => Op switch {
         '+' => a + b,
         '-' => a - b,
         '*' => a * b,
         '/' => a / b,
         '^' => Math.Pow (a, b),
         _ => throw new InvalidOperationException (),
      };
   #endregion

   #region Field ----------------------------------------------------
   static Dictionary<char, int> sPriority = new () {
      ['+'] = 1, ['-'] = 1, ['*'] = 2, ['/'] = 2, ['^'] = 3, ['='] = 4,
   };
   #endregion
}
#endregion

#region Class TOpFunc -----------------------------------------------------------------------------
/// <summary></summary>
class TOpFunc (Evaluator eval, string func) : TOperator(eval) {
   #region Properties -----------------------------------------------
   public string Func { get; } = func;
   public override int Priority => 5 + mEval.BasePriority;
   #endregion

   #region Methods --------------------------------------------------
   /// <summary></summary>
   public override string ToString () => $"Function : {Func}";

   /// <summary></summary>
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
         _ => throw new NotImplementedException (),
      };
      double D2R (double f) => f * Math.PI / 180;
      double R2D (double f) => f * 180 / Math.PI;
   }
   #endregion
}
#endregion

#region Class TEnd --------------------------------------------------------------------------------
/// <summary></summary>
class TEnd : Token {
   #region Method ---------------------------------------------------
   /// <summary></summary>
   public override string ToString () => "end";
   #endregion
}
#endregion

#region Class TError ------------------------------------------------------------------------------
/// <summary></summary>
class TError (string msg) : Token {
   #region Property -------------------------------------------------
   public string Message { get; } = msg;
   #endregion

   #region Method ---------------------------------------------------
   /// <summary></summary>
   public override string ToString () => $"Error : {Message}";
   #endregion
}
#endregion

#region Class TPunctuation ------------------------------------------------------------------------
/// <summary></summary>
class TPunctuation (char punct) : Token {
   #region Property -------------------------------------------------
   public char Punct { get; } = punct;
   #endregion

   #region Method ---------------------------------------------------
   /// <summary></summary>
   public override string ToString () => $"Punctuation : {Punct}";
   #endregion
}
#endregion