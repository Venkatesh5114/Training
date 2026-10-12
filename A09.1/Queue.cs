// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// Queue.cs
// Queue implementation with circular buffer.
// ------------------------------------------------------------------------------------------------

#region Queue -------------------------------------------------------------------------------------
/// <summary>Implementation of custom queue that adds and removes elements in FIFO order</summary>
class TQueue<T> {
   #region Property -------------------------------------------------
   /// <summary>Checks whether the queue contains no elements</summary>
   public bool IsEmpty => mCount == 0;
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Adds an element to the rear of the queue</summary>
   public void Enqueue (T value) {
      if (mCount == mData.Length) Resize ();
      mData[mPos] = value;
      mPos = (mPos + 1) % mData.Length;
      mCount++;
   }

   /// <summary>Removes and returns the element from the front of the queue</summary>
   public T Dequeue () {
      if (IsEmpty) throw new InvalidOperationException ("Queue Empty");
      T value = mData[mFree];
      mData[mFree] = default!;
      mFree = (mFree + 1) % mData.Length;
      mCount--;
      return value;
   }
   #endregion

   #region Implementation -------------------------------------------
   // Doubles the queue capacity and rearranges the elements in queue order
   void Resize () {
      T[] temp = new T[mData.Length * 2];
      for (int i = 0; i < mCount; i++) temp[i] = mData[(mFree + i) % mData.Length];
      mData = temp;
      mFree = 0;
      mPos = mCount;
   }
   #endregion

   #region Fields ---------------------------------------------------
   T[] mData = new T[4];    // Stores the queue elements
   int mPos = 0;            // Index of the next element to be added
   int mFree = 0;           // Index of the next element to be removed
   int mCount = 0;          // Number of elements currently in the queue
   #endregion
}
#endregion