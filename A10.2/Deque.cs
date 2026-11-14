// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// TDQueue.cs
// Double Ended Queue implementation with circular buffer.
// ------------------------------------------------------------------------------------------------

#region class TDQueue -----------------------------------------------------------------------------
/// <summary>Implementation of custom double ended queue using circular buffer</summary>
class TDQueue<T> {
   #region Properties -----------------------------------------------
   /// <summary>Checks whether the double ended queue contains no elements</summary>
   public bool IsEmpty => mCount == 0;

   /// <summary>Gets the number of elements in the double ended queue</summary>
   public int Count => mCount;
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Adds an element to the rear of the queue</summary>
   public void EnqueueRear (T value) => Enqueue (value, EState.Rear);

   /// <summary>Adds an element to the front of the queue</summary>
   public void EnqueueFront (T value) => Enqueue (value, EState.Front);

   /// <summary>Removes and returns the element from the front of the queue</summary>
   public T DequeueFront () => Dequeue (EState.Front);

   /// <summary>Removes and returns the element from the rear of the queue</summary>
   public T DequeueRear () => Dequeue (EState.Rear);
   #endregion

   #region Implementation -------------------------------------------
   // Doubles the queue capacity and rearranges the elements in queue order
   void Resize () {
      var temp = new T[mData.Length * 2];
      for (int i = 0; i < mCount; i++) temp[i] = mData[(mStart + i) % mData.Length];
      (mData, mStart, mEnd) = (temp, 0, mCount);
   }

   // Inserts an element at the specified end of the double ended queue
   void Enqueue (T value, EState state) {
      if (mCount == mData.Length) Resize ();
      int capacity = mData.Length;
      if (state is EState.Rear) {
         mData[mEnd] = value;
         mEnd = (mEnd + 1) % capacity;
      } else {
         mStart = (mStart + capacity - 1) % capacity;
         mData[mStart] = value;
      }
      mCount++;
   }

   // Removes and returns an element from the specified end of the double ended queue
   T Dequeue (EState state) {
      if (IsEmpty) throw new InvalidOperationException ("Queue Empty");
      int capacity = mData.Length;
      T value;
      if (state is EState.Rear) {
         mEnd = (mEnd + capacity - 1) % capacity;
         value = mData[mEnd];
         mData[mEnd] = default!;
      } else {
         value = mData[mStart];
         mData[mStart] = default!;
         mStart = (mStart + 1) % capacity;
      }
      mCount--;
      return value;
   }
   #endregion

   #region Private Data ---------------------------------------------
   T[] mData = new T[INITCAPACITY];    // Stores the queue elements
   int mEnd = 0;            // Index of the next element to be added or removed at the rear
   int mStart = 0;          // Index of the next element to be added or removed at the front
   int mCount = 0;          // Number of elements currently in the queue
   const int INITCAPACITY = 4;
   #endregion

   #region Enum EState ----------------------------------------------
   enum EState { Front, Rear }
   #endregion
}
#endregion