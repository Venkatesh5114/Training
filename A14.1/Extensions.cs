public static class Extensions {
   public static IEnumerable<T> MyWhere<T>(this IEnumerable<T> able, Func<T, bool> predicate) {
      foreach (var item in able) if (predicate (item)) yield return item;
   }

   public static IEnumerable<U> MySelect<T, U> (this IEnumerable<T> able, Func<T, U> transform) {
      foreach (var item in able) yield return transform (item);
   }
}