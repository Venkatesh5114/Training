class Program {
   static void Main () {
      TDeQueue<int> dequeue = new ();
      for(int i = 0; i < 10; i++) dequeue.EnqueueHead (i);
      Console.WriteLine ("DequeueHead");
      for (int i = 0; i < 3; i++) Console.Write(dequeue.DequeueHead ());
      Console.WriteLine ();
      Console.WriteLine ("DequeueTail");
      while(!dequeue.IsEmpty) Console.Write (dequeue.DequeueTail());
   }
}