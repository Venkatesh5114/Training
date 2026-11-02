// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// Queue.cs
// Queue implementation with circular buffer.
// ------------------------------------------------------------------------------------------------

#region class DeQue -------------------------------------------------------------------------------
/// <summary>Implementation of custom queue that adds and removes elements in FIFO order</summary>
class TDeQueue<T> {
   #region Property -------------------------------------------------
   /// <summary>Checks whether the queue contains no elements</summary>
   public bool IsEmpty => mCount == 0;
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Adds an element to the rear of the queue</summary>
   public void EnqueueTail (T value) {
      if (mCount == mData.Length) Resize ();
      mData[mEnd] = value;
      mEnd = (mEnd + 1) % mData.Length;
      mCount++;
   }

   /// <summary>Adds an element to the front of the queue</summary>
   public void EnqueueHead (T value) {
      if (mCount == mData.Length) Resize ();
      mStart = (mStart + mData.Length - 1) % mData.Length;
      mData[mStart] = value;
      mCount++;
   }

   /// <summary>Removes and returns the element from the front of the queue</summary>
   public T DequeueHead () {
      if (IsEmpty) throw new InvalidOperationException ("Queue Empty");
      T value = mData[mStart];
      mData[mStart] = default!;
      mStart = (mStart + 1) % mData.Length;
      mCount--;
      return value;
   }

   /// <summary>Removes and returns the element from the rear of the queue</summary>
   public T DequeueTail () {
      if (IsEmpty) throw new InvalidOperationException ("Queue Empty");
      T value = mData[mEnd];
      mData[mEnd] = default!;
      mEnd = (mEnd - 1) % mData.Length;
      mCount--;
      return value;
   }
   #endregion

   #region Implementation -------------------------------------------
   // Doubles the queue capacity and rearranges the elements in queue order
   void Resize () {
      T[] temp = new T[mData.Length * 2];
      for (int i = 0; i < mCount; i++) temp[i] = mData[(mStart + i) % mData.Length];
      mData = temp;
      mStart = 0;
      mEnd = mCount;
   }
   #endregion

   #region Fields ---------------------------------------------------
   T[] mData = new T[4];    // Stores the queue elements
   int mEnd = 0;            // Index of the next element to be added
   int mStart = 0;           // Index of the next element to be removed
   int mCount = 0;          // Number of elements currently in the queue
   #endregion
}
#endregion