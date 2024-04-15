
using System;
using static System.Console;

public static IEnumerable<T> TakeSequence<T>(
    this IEnumerable<T> source, int count) {
  var queue = new Queue<T>(count);
  foreach(var item in source) {
    if(queue.Count == count) {
      queue.Dequeue();
    }
    queue.Enqueue(item);
  }
  foreach (var item in queue) {
    yield return item; // lazyly return the items
  }
}

/*---------------------------------------------------------------*\
  Given Empty Sequence when N=0 or N=5 return Empty Sequence
\*---------------------------------------------------------------*/

// Expected: Empty
WriteLine((Enumerable.Empty<int>()).TakeSequence(0));

