// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// Evaluator.cs
// Evaluates expressions with operator precedence, parentheses, functions, and variable
// assignments while handling errors
// ------------------------------------------------------------------------------------------------

namespace Eval;

#region class EvalException -----------------------------------------------------------------------
/// <summary>Defines a custom exception class for handling evaluation errors</summary>
/// <param name="message">Specifies the error message describing the evaluation error</param>
class EvalException (string message) : Exception (message) { }
#endregion

#region class Evaluator ---------------------------------------------------------------------------
/// <summary>Evaluates expressions with operator precedence, parentheses, functions, and variable
/// assignments while handling errors</summary>
class Evaluator () {
   #region Property -------------------------------------------------
   public int BasePriority { get; private set; }
   #endregion

   #region Method ---------------------------------------------------
   /// <summary>Evaluates a mathematical expression and returns its result</summary>
   /// <param name="input">The expression to evaluate</param>
   /// <returns>The evaluated result</returns>
   public double Evaluate (string input) {
      Reset ();
      Tokenizer tokenizer = new (this, input);
      for (; ; ) {
         var token = tokenizer.GetNext ();
         if (token is TEnd) break;
         if (token is TError err) throw new EvalException (err.Message);
         tokens.Add (token);
      }
      // Identify a variable assignment and remove the variable and '=' tokens from the expression
      if (tokens.Count > 1 && tokens[0] is TVariable tvar && tokens[1] is TOpBinary bin
                                                                          && bin.Op == '=') {
         tVar = tvar;
         tokens.RemoveRange (0, 2);
      }
      foreach (var token in tokens) Process (token);
      while (mOperators.Count > 0) ApplyOperator ();
      if (BasePriority != 0) Error ("Mismatched paranthesis");
      if (mOperators.Count > 0) Error ("Too many operators");
      if (mOperands.Count > 1) Error ("Too many operands");
      double f = mOperands.Pop ();
      if (tVar != null) mVariables[tVar.Name] = f;
      return f;
   }
   #endregion

   #region Implementation -------------------------------------------
   // Applies the operator to the available operands
   void ApplyOperator () {
      var op = mOperators.Pop ();
      if (op is TOpUnary uni) {
         if (mOperands.Count == 0) Error ("Too few operands");
         else {
            double f = mOperands.Pop ();
            if ((tTemp is not null && (uni.Op is "++" or "--")) || (uni.Op is "+" or "-")) {
               double ans = uni.Apply (f);
               if (tTemp is not null && (uni.Op is "++" or "--")) mVariables[tTemp.Name] = ans;
               mOperands.Push (ans);
            } else Error ("Invalid Expression");
         }
      }
      if (op is TOpBinary bin) {
         if(bin.Op == '=') Error("Unexpected '=' operator");
         if (mOperands.Count < 2) Error ("Too few operands");
         else {
            double f1 = mOperands.Pop (), f2 = mOperands.Pop ();
            mOperands.Push (bin.Apply (f2, f1));
         }
      }
      if (op is TOpFunc func) {
         if (mOperands.Count < 1) Error ("Too few operands");
         else {
            double f = mOperands.Pop ();
            mOperands.Push (func.Apply (f));
         }
      }
   }

   // Processes a token and updates the operand or operator stacks
   void Process (Token token) {
      switch (token) {
         case TNumber num:
            if (num is TVariable var) tTemp = var;
            mOperands.Push (num.Value);
            break;
         case TOperator op:
            op.FinalPriority = op.Priority + BasePriority;
            while (!OkToPush (op)) ApplyOperator ();
            mOperators.Push (op);
            break;
         case TPunctuation p:
            BasePriority += p.Punct == '(' ? 10 : -10;
            break;
         default: Error ($"Unknown token: {token}"); break;
      }
   }

   // Resets the evaluator state before evaluating a new expression
   void Reset () {
      tokens.Clear ();
      mOperands.Clear ();
      mOperators.Clear ();
      BasePriority = 0;
      tVar = null;
      tTemp = null;
   }

   // Throws an evaluation exception with the specified message
   void Error (string s) => throw new EvalException (s);

   // Determines whether the operator can be pushed onto the operator stack
   public bool OkToPush (TOperator op) {
      if (mOperators.Count == 0) return true;
      TOperator prev = mOperators.Peek ();
      return prev.FinalPriority < op.FinalPriority ||
             (prev.FinalPriority == op.FinalPriority && op is TOpBinary { Op: '^' }) ||
             (prev is TOpUnary && op is TOpFunc) ||
             (prev is TOpUnary or TOpFunc && op is TOpBinary { Op: '^' } &&
             prev.FinalPriority - op.FinalPriority < 10);
   }

   // Retrieves the value of the specified variable
   public double GetVariable (string name) {
      if (mVariables.TryGetValue (name, out double value)) return value;
      Error ($"Unknown variable {name}");
      return 0;
   }
   #endregion

   #region Private Data ---------------------------------------------
   List<Token> tokens = [];                           // Stores the tokens
   Stack<double> mOperands = new ();                  // Stores the operands
   Stack<TOperator> mOperators = new ();              // Stores the operators
   Dictionary<string, double> mVariables = [];        // Stores variable names and their values
   TVariable? tVar = null;                            // Check if this is a variable assignment
   TVariable? tTemp = null;                           // Temporary variable for processing
   #endregion
}
#endregion