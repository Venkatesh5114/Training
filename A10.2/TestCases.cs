// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// TestCases.cs
// Testing the double ended queue implementation.
// ------------------------------------------------------------------------------------------------

using static System.Console;

#region TestCases ---------------------------------------------------------------------------------
/// <summary>Tests the generic queue implementation</summary>
class TestCases {
   #region Implementation -------------------------------------------
   // Runs all deque test cases
   public static void Run () {
      TestEmptyDeque ();
      TestHeadOrder ();
      TestTailOrder ();
      TestMixedOrder ();
      TestHeadWrap ();
      TestTailWrap ();
      TestResize ();
      TestHeadResize ();
      TestWrappedResize ();
      TestIsEmpty ();
      TestCount ();
   }

   // Tests if dequeue operations throw an exception for an empty deque
   static void TestEmptyDeque () {
      TDeQue<int> deque = new ();
      bool headPassed = false, tailPassed = false;

      try {
         deque.DequeueFront ();
      } catch (Exception) {
         headPassed = true;
      }

      try {
         deque.DequeueRear ();
      } catch (Exception) {
         tailPassed = true;
      }

      PrintStatus ("DequeueHead on empty deque", headPassed);
      PrintStatus ("DequeueTail on empty deque", tailPassed);
   }

   // Tests insertion at tail and removal from head
   static void TestHeadOrder () {
      TDeQue<int> deque = new ();
      foreach (int value in sData) deque.EnqueueRear (value);

      bool passed = true;
      for (int i = 0; i < sData.Length; i++)
         passed &= deque.DequeueFront () == sData[i];

      PrintStatus ("EnqueueTail and DequeueHead order", passed);
   }

   // Tests insertion at head and removal from tail
   static void TestTailOrder () {
      TDeQue<int> deque = new ();
      foreach (int value in sData) deque.EnqueueFront (value);

      bool passed = true;
      for (int i = 0; i < sData.Length; i++)
         passed &= deque.DequeueRear () == sData[i];

      PrintStatus ("EnqueueHead and DequeueTail order", passed);
   }

   // Tests insertion and removal from both ends
   static void TestMixedOrder () {
      TDeQue<int> deque = new ();

      deque.EnqueueRear (10);
      deque.EnqueueFront (20);
      deque.EnqueueRear (30);
      deque.EnqueueFront (40);

      bool passed = deque.DequeueFront () == 40;
      passed &= deque.DequeueRear () == 30;

      deque.EnqueueRear (50);
      deque.EnqueueFront (60);

      passed &= deque.DequeueFront () == 60;
      passed &= deque.DequeueRear () == 50;
      passed &= deque.DequeueFront () == 20;
      passed &= deque.DequeueRear () == 10;

      PrintStatus ("Mixed Enqueue and Dequeue order", passed);
   }

   // Tests circular wrapping when inserting at tail
   static void TestHeadWrap () {
      TDeQue<int> deque = new ();

      for (int i = 0; i < 4; i++)
         deque.EnqueueRear (sData[i]);

      deque.DequeueFront ();
      deque.DequeueFront ();

      deque.EnqueueRear (sData[4]);
      deque.EnqueueRear (sData[5]);

      bool passed = true;
      for (int i = 2; i < 6; i++)
         passed &= deque.DequeueFront () == sData[i];

      PrintStatus ("Circular wrap at tail", passed);
   }

   // Tests circular wrapping when inserting at head
   static void TestTailWrap () {
      TDeQue<int> deque = new ();

      for (int i = 0; i < 4; i++)
         deque.EnqueueFront (sData[i]);

      deque.DequeueRear ();
      deque.DequeueRear ();

      deque.EnqueueFront (sData[4]);
      deque.EnqueueFront (sData[5]);

      bool passed = true;
      for (int i = 2; i < 6; i++)
         passed &= deque.DequeueRear () == sData[i];

      PrintStatus ("Circular wrap at head", passed);
   }

   // Tests resizing when elements are inserted at tail
   static void TestResize () {
      TDeQue<int> deque = new ();

      for (int i = 0; i < sData.Length; i++)
         deque.EnqueueRear (sData[i]);

      bool passed = deque.Count == sData.Length;

      for (int i = 0; i < sData.Length; i++)
         passed &= deque.DequeueFront () == sData[i];

      PrintStatus ("Resize using EnqueueTail", passed);
   }

   // Tests resizing when elements are inserted at head
   static void TestHeadResize () {
      TDeQue<int> deque = new ();

      for (int i = 0; i < sData.Length; i++)
         deque.EnqueueFront (sData[i]);

      bool passed = deque.Count == sData.Length;

      for (int i = sData.Length - 1; i >= 0; i--)
         passed &= deque.DequeueFront () == sData[i];

      PrintStatus ("Resize using EnqueueHead", passed);
   }

   // Tests resizing when elements are stored across the circular boundary
   static void TestWrappedResize () {
      TDeQue<int> deque = new ();

      for (int i = 0; i < 4; i++)
         deque.EnqueueRear (sData[i]);

      deque.DequeueFront ();
      deque.DequeueFront ();

      for (int i = 4; i < sData.Length; i++)
         deque.EnqueueRear (sData[i]);

      bool passed = deque.Count == 5;

      for (int i = 2; i < sData.Length; i++)
         passed &= deque.DequeueFront () == sData[i];
      PrintStatus ("Resize after circular wrapping", passed);
   }

   // Tests whether the deque is empty
   static void TestIsEmpty () {
      TDeQue<int> deque = new ();
      PrintStatus ("Initially empty", deque.IsEmpty);
      deque.EnqueueRear (10);
      PrintStatus ("Not empty after EnqueueTail", !deque.IsEmpty);

      deque.DequeueFront ();
      PrintStatus ("Empty after DequeueHead", deque.IsEmpty);

      deque.EnqueueFront (20);
      PrintStatus ("Not empty after EnqueueHead", !deque.IsEmpty);

      deque.DequeueRear ();
      PrintStatus ("Empty after DequeueTail", deque.IsEmpty);
   }

   // Tests whether Count is updated correctly
   static void TestCount () {
      TDeQue<int> deque = new ();

      bool passed = deque.Count == 0;

      deque.EnqueueRear (10);
      deque.EnqueueFront (20);
      deque.EnqueueRear (30);
      passed &= deque.Count == 3;

      deque.DequeueFront ();
      passed &= deque.Count == 2;

      deque.DequeueRear ();
      passed &= deque.Count == 1;

      deque.DequeueFront ();
      passed &= deque.Count == 0;

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
   #endregion

   #region Private data ---------------------------------------------
   static readonly int[] sData = { 0, 10, 20, 30, 40, 50, 60 };    // Test Values
   #endregion
}
#endregion