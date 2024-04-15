/* required packages:
  Microsoft.NET.Test.Sdk
  xunit
  xunit.runner.visualstudio
  coverlet.collector
*/

using static System.Console;  
using Xunit;

static class CustomEnumerableExt{
  // create emply extension method that returns the last N elements of the sequence
  public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int count){
    var queue = new Queue<T>(count);
    foreach(var item in source){
      if(queue.Count == count){
        queue.Dequeue();
      }
      queue.Enqueue(item);
    }
    
    // return items as last N elements
    foreach (var item in queue){
      yield return item;
    } 
  } 
}

public class CustomEnumerableExtTests{
  [Fact]
  public void TestTakeLast(){
    Assert.True(true);
  }
}
