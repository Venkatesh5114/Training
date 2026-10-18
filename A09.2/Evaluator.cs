namespace Eval;

class EvalException (string message) : Exception (message) { }
class Evaluator () {
   public double Evaluate (string input) {
      Reset ();
      Tokenizer tokenizer = new (this, input);
      List<Token> tokens = [];
      for (; ; ) {
         var token = tokenizer.GetNext ();
         if (token is TEnd) break;
         if (token is TError err) throw new EvalException (err.Message);
         tokens.Add (token);
      }
      // Check if this is a variable assignment
      TVariable? tVar = null;
      if (tokens.Count > 1 && tokens[0] is TVariable tvar && tokens[1] is TOpBinary bin && bin.Op == '=') {
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

   void ApplyOperator () {
      var op = mOperators.Pop ();
      if (op is TOpUnary uni) {
         if (mOperands.Count == 0) Error("Too few operands");
         else {
            double f = mOperands.Pop ();
            mOperands.Push (uni.Apply (f));
         }
      }
      if (op is TOpBinary bin) {
         if (mOperands.Count < 2) Error ("Too few operands");
         double f1 = mOperands.Pop (), f2 = mOperands.Pop ();
         mOperands.Push (bin.Apply (f2, f1));
      }
      if (op is TOpFunc func) {
         if (mOperands.Count < 1) Error ("Too few operands");
         double f = mOperands.Pop ();
         mOperands.Push (func.Apply (f));
      }
   }
   void Process (Token token) {
      switch (token) {
         case TNumber num:
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
   public int BasePriority { get; private set; }

   void Reset () {
      mOperands.Clear ();
      mOperators.Clear ();
      BasePriority = 0;
   }

   void Error (string s) => throw new EvalException (s);
   public bool OkToPush (TOperator op) {
      if (mOperators.Count == 0) return true;
      TOperator prev = mOperators.Peek ();
      return prev.FinalPriority < op.FinalPriority || prev is TOpUnary;
   }

   public double GetVariable (string name) {
      if (mVariables.TryGetValue (name, out double value)) return value;
      Error ($"Unknown variable {name}");
      return 0;
   }

   Stack<double> mOperands = new ();
   Stack<TOperator> mOperators = new ();
   Dictionary<string, double> mVariables = [];
}