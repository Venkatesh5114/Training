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
      TestDeque ("EnqueueFront and DequeueRear order", 0, 4, EState.Front);
      TestDeque ("EnqueueRear and DequeueFront order", 0, 4, EState.Rear);
      TestMixedOrder ();
      TestWrap ("Circular wrap at tail", 0, 4, 2, 6, EState.Rear);
      TestWrap ("Circular wrap at head", 0, 4, 2, 6, EState.Front);
      TestDeque ("Resize using EnqueueRear", 0, sData.Length, EState.Rear);
      TestDeque ("Resize using EnqueueFront", 0, sData.Length, EState.Front);
      TestWrap ("Resize after circular wrapping", 0, 4, 2, sData.Length, EState.Front);
      TestIsEmpty ();
      TestCount ();
   }

   // Tests if dequeue operations throw an exception for an empty deque
   static void TestEmptyDeque () {
      TDeque<int> deque = new ();
      bool headPassed = ThrowsEmpty (deque.DequeueFront);
      bool tailPassed = ThrowsEmpty (deque.DequeueRear);
      PrintStatus ("DequeueHead on empty deque", headPassed);
      PrintStatus ("DequeueTail on empty deque", tailPassed);
   }

   static void TestDeque (string message, int start, int end, EState state) {
      TDeque<int> deque = new ();
      Enqueue(deque, start, end, state);
      state = state == EState.Front ? EState.Rear : EState.Front;
      bool passed = Compare (deque, start, end, state);
      PrintStatus(message, passed);
   }

   // Tests insertion and removal from both ends
   static void TestMixedOrder () {
      TDeque<int> deque = new ();
      Enqueue(deque, 1, 5, EState.Rear, true);
      bool passed = deque.DequeueFront () == 40;
      passed &= deque.DequeueRear () == 30;
      Enqueue (deque, 5, sData.Length, EState.Rear, true);
      passed &= deque.DequeueFront () == 60;
      passed &= deque.DequeueRear () == 50;
      passed &= deque.DequeueFront () == 20;
      passed &= deque.DequeueRear () == 10;
      PrintStatus ("Mixed Enqueue and Dequeue order", passed);
   }

   static void TestWrap (string message, int start, int skipStart, int skipCount, int end, EState state) {
      TDeque<int> deque = new ();
      Enqueue (deque, start, skipStart, state);
      bool isFront = state == EState.Front;
      for(int i = 0; i < skipCount; i++) {
         if(isFront) deque.DequeueRear ();
         else deque.DequeueFront ();
      }
      Enqueue (deque, skipStart, end, state);
      state = isFront ? EState.Rear : EState.Front;
      bool passed = Compare (deque, start + skipCount, end, state);
      PrintStatus (message, passed);
   }

   // Tests whether the deque is empty
   static void TestIsEmpty () {
      TDeque<int> deque = new ();
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
      TDeque<int> deque = new ();
      bool passed = deque.Count == 0;
      Enqueue (deque, 0, 3, EState.Rear, true);
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

   // Checks whether a dequeue operation throws on an empty queue
   static bool ThrowsEmpty (Func<int> dequeue) {
      try {
         dequeue ();
      } catch (InvalidOperationException) {
         return true;
      }
      return false;
   }

   // Enqueues elements at the specified end of the deque
   static void Enqueue (TDeque<int> deque, int start, int end, EState state, bool isMixed = false) {
      bool isFront = state == EState.Front;
      Action<int> enqueue = isFront ? deque.EnqueueFront : deque.EnqueueRear;
      for (int i = start; i < end; i++) {
         enqueue (sData[i]);
         if(isMixed) {
            isFront = !isFront;
            enqueue = isFront ? deque.EnqueueFront : deque.EnqueueRear;
         }
      }
   }

   // Compares elements dequeued from the specified end of the deque
   static bool Compare (TDeque<int> deque, int start, int end, EState state) {
      bool isPassed = true;
      Func<int> dequeue = state == EState.Front ? deque.DequeueFront : deque.DequeueRear;
      for (int i = start; i < end; i++) isPassed &= dequeue () == sData[i];
      return isPassed;
   }
   #endregion

   #region Private data ---------------------------------------------
   static readonly int[] sData = { 0, 10, 20, 30, 40, 50, 60 };    // Test Values
   #endregion

   #region Enum EState ----------------------------------------------
   enum EState { Front, Rear };
   #endregion
}
#endregion