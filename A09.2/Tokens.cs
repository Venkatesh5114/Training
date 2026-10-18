namespace Eval;

#region Class Token -------------------------------------------------------------------------------
class Token { }
#endregion

#region Class TNumber -----------------------------------------------------------------------------
abstract class TNumber : Token {
   #region Method ---------------------------------------------------
   public abstract double Value { get; }
   #endregion
}
#endregion

#region Class TOperator ---------------------------------------------------------------------------
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

#region Class TLiteral ----------------------------------------------------------------------------
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
class TVariable : TNumber {
   #region Constructor ----------------------------------------------
   public TVariable (Evaluator eval, string name) => (Name, mEval) = (name, eval);
   #endregion

   #region Properties
   public string Name { get; }
   #endregion

   #region Method ---------------------------------------------------
   public override double Value => mEval.GetVariable (Name);
   public override string ToString () => $"Variable : {Name}";
   #endregion

   #region Field ----------------------------------------------------
   readonly Evaluator mEval;
   #endregion
}
#endregion

#region Class TOpUnary ----------------------------------------------------------------------------
class TOpUnary : TOperator {
   #region Constructor ----------------------------------------------
   public TOpUnary (Evaluator eval, string op) : base (eval) => Op = op;
   #endregion

   #region Properties -----------------------------------------------
   public override int Priority => 5 + mEval.BasePriority;
   public string Op { get; }
   #endregion

   #region Methods --------------------------------------------------
   public double Apply (double num) {
      return Op switch {
         "+" => num,
         "-" => -num,
         "++" => num + 1,
         "--" => num - 1,
         _ => throw new InvalidOperationException (),
      };
   }
   public override string ToString () => $"Unary : {Op}";
   #endregion
}
#endregion
class TOpBinary : TOperator {
   public TOpBinary (Evaluator eval, char op) : base (eval) => Op = op;
   public override int Priority => sPriority[Op] + mEval.BasePriority;
   static Dictionary<char, int> sPriority = new () {
      ['+'] = 1, ['-'] = 1, ['*'] = 2, ['/'] = 2, ['^'] = 3, ['='] = 4,
   };
   public char Op { get; }
   public override string ToString () => $"Operator : {Op}";
   public double Apply (double a, double b)
      => Op switch {
         '+' => a + b,
         '-' => a - b,
         '*' => a * b,
         '/' => a / b,
         '^' => Math.Pow (a, b),
         _ => throw new InvalidOperationException (),
      };
}
class TOpFunc : TOperator {
   public TOpFunc (Evaluator eval, string func) : base (eval) => Func = func;
   public string Func { get; }
   public override string ToString () => $"Function : {Func}";
   public override int Priority => 6 + mEval.BasePriority;
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
}
class TEnd : Token {
   public override string ToString () => "end";

}
class TError : Token {
   public TError (string msg) => Message = msg;
   public string Message { get; }
   public override string ToString () => $"Error : {Message}";
}
class TPunctuation : Token {
   public TPunctuation (char p) => Punct = p;
   public char Punct { get; }
   public override string ToString () => $"Punctuation : {Punct}";
}