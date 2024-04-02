using System;

// Input
int input = 5;
int output = input
  .Pipe(x => x + 1)
  .Pipe(x => x * 2)
  .Pipe(x => x - 3);

// Output
Console.WriteLine(output);

// Functions
public static TOutput Pipe<TInput, TOutput>( this TInput input, Func<TInput, TOutput> operation) {
  return operation(input);   
}
