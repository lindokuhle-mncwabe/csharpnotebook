using System;

// refactoring into method chaining
int FactorialRefactor1(int n) {
  if (n < 0) {
    throw new ArgumentException("n must be non-negative");
  } else {
    var result = 1;
    if (n > 1) {
      for(int i = 2; i <= n; i++) {
        result *= i;
      }
    }
    return result;
  }
}

int FactorialRefactor2(int n) {
  if (n < 0) throw new ArgumentException("n must be non-negative");
  if (n == 0) return 1;
  var result = 1;
  for(int i = 2; i <= n; i++) {
    result *= i;
  }
}

int FactorialRefactor3(int n) {
  return n < 0 ? throw new ArgumentException("n must be non-negative") 
    : n == 0 ? 1 
    : Enumerable.Range(1, n).Aggregate((a, b) => a * b);
}
