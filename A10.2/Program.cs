class Program {
   static void Main () {
      TDeQue<int> dequeue = new ();
      for(int i = 0; i < 10; i++) dequeue.EnqueueRear (i);
      Console.WriteLine ("DequeueHead");
      for (int i = 0; i < 3; i++) Console.Write(dequeue.DequeueRear ());
      Console.WriteLine ();
      Console.WriteLine ("DequeueTail");
      while(!dequeue.IsEmpty) Console.Write (dequeue.DequeueFront());
   }
}