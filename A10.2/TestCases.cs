// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// TestCases.cs
// Testing the double ended queue implementation.
// ------------------------------------------------------------------------------------------------

using static System.Console;

#region class TestCases ---------------------------------------------------------------------------
/// <summary>Tests the double ended queue implementation</summary>
class TestCases {
   #region Methods --------------------------------------------------
   /// <summary>Runs all double ended queue test cases</summary>
   public static void Run () {
      TestEmptyDQueue ();
      TestDQueue ("EnqueueFront and DequeueRear order", count: 4, front: true);
      TestDQueue ("EnqueueRear and DequeueFront order", count: 4, front: false);
      TestMixedOrder ();
      TestWrap ("Circular wrap at rear", skipStart: 4, skipCount: 2, count: 6, front: false);
      TestWrap ("Circular wrap at front", skipStart: 4, skipCount: 2, count: 6, front: true);
      TestDQueue ("Resize using EnqueueRear", count: sData.Length, front: false);
      TestDQueue ("Resize using EnqueueFront", count: sData.Length, front: true);
      TestWrap ("Resize after circular wrapping", skipStart: 4, skipCount: 2, count: sData.Length, front: true);
      TestIsEmpty ();
      TestCount ();
   }
   #endregion

   #region Implementation -------------------------------------------
   // Tests if dequeue operations throw an exception for an empty double ended queue
   static void TestEmptyDQueue () {
      TDQueue<int> q = new ();
      PrintStatus ("DequeueFront on empty DQueue", ThrowsEmpty (q.DequeueFront));
      PrintStatus ("DequeueRear on empty DQueue", ThrowsEmpty (q.DequeueRear));
   }

   // Tests double ended queue insertion and validates the order of removed elements
   static void TestDQueue (string message, int count, bool front) {
      TDQueue<int> q = new ();
      Enqueue (q, 0, count, front);
      bool passed = Compare (q, 0, count, !front);
      PrintStatus (message, passed);
   }

   // Tests insertion and removal from both ends
   static void TestMixedOrder () {
      TDQueue<int> q = new ();
      Enqueue (q, start: 1, count: 5, isFront: false, isMixed: true);
      bool passed = q.DequeueFront () == 40;
      passed &= q.DequeueRear () == 30;
      Enqueue (q, start: 5, count: sData.Length, isFront: false, isMixed: true);
      passed &= q.DequeueFront () == 60;
      passed &= q.DequeueRear () == 50;
      passed &= q.DequeueFront () == 20;
      passed &= q.DequeueRear () == 10;
      PrintStatus ("Mixed Enqueue and Dequeue order", passed);
   }

   // Tests circular buffer wraparound by removing elements and reusing freed positions
   static void TestWrap (string message, int skipStart, int skipCount, int count, bool front) {
      TDQueue<int> q = new ();
      Enqueue (q, 0, skipStart, front);
      for (int i = 0; i < skipCount; i++) {
         if (front) q.DequeueRear ();
         else q.DequeueFront ();
      }
      Enqueue (q, skipStart, count, front);
      bool passed = Compare (q, skipCount, count, !front);
      PrintStatus (message, passed);
   }

   // Tests whether the double ended queue is empty
   static void TestIsEmpty () {
      TDQueue<int> q = new ();
      PrintStatus ("Initially empty", q.IsEmpty);
      q.EnqueueRear (10);
      PrintStatus ("Not empty after EnqueueRear", !q.IsEmpty);
      q.DequeueFront ();
      PrintStatus ("Empty after DequeueFront", q.IsEmpty);
      q.EnqueueFront (20);
      PrintStatus ("Not empty after EnqueueFront", !q.IsEmpty);
      q.DequeueRear ();
      PrintStatus ("Empty after DequeueRear", q.IsEmpty);
   }

   // Tests whether Count is updated correctly
   static void TestCount () {
      TDQueue<int> q = new ();
      bool passed = q.Count == 0;
      Enqueue (q, start: 0, count: 3, isFront: false, isMixed: true);
      passed &= q.Count == 3;
      q.DequeueFront ();
      passed &= q.Count == 2;
      q.DequeueRear ();
      passed &= q.Count == 1;
      q.DequeueFront ();
      passed &= q.Count == 0;
      PrintStatus ("Count updates correctly", passed);
   }

   // Displays the test status
   static void PrintStatus (string name, bool passed) {
      Write ("[");
      ForegroundColor = passed ? ConsoleColor.Green : ConsoleColor.Red;
      Write (passed ? "PASS" : "FAIL");
      ResetColor ();
      WriteLine ($"] {name}");
   }

   // Checks whether a dequeue operation throws on an empty double ended queue
   static bool ThrowsEmpty (Func<int> dequeue) {
      try {
         dequeue ();
      } catch (InvalidOperationException) {
         return true;
      }
      return false;
   }

   // Enqueues elements at the specified end of the double ended queue
   static void Enqueue (TDQueue<int> q, int start, int count, bool isFront, bool isMixed = false) {
      Action<int> enqueue = isFront ? q.EnqueueFront : q.EnqueueRear;
      for (int i = start; i < count; i++) {
         enqueue (sData[i]);
         if (isMixed) {
            isFront = !isFront;
            enqueue = isFront ? q.EnqueueFront : q.EnqueueRear;
         }
      }
   }

   // Compares elements dequeued from the specified end of the double ended queue
   static bool Compare (TDQueue<int> q, int start, int count, bool isFront) {
      bool isPassed = true;
      Func<int> dequeue = isFront ? q.DequeueFront : q.DequeueRear;
      for (int i = start; i < count; i++) isPassed &= dequeue () == sData[i];
      return isPassed && q.IsEmpty;
   }
   #endregion

   #region Private data ---------------------------------------------
   static readonly int[] sData = { 0, 10, 20, 30, 40, 50, 60 };    // Test Values
   #endregion
}
#endregion