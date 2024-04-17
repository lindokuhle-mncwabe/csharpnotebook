
using System;
using static System.Console;

// Refactoring Impure Out Parameter Feature

// Impure Function
public static void Math01(int a, int b, 
                          out int sum, 
                          out int dif, 
                          out decimal pro, 
                          out decimal quo) {
  sum = a + b;
  dif = a - b;
  pro = (decimal)a * b;
  quo = (decimal)a / b;
}
// What is the problem with Math01 function?
// 1. Input parameters act as output parameters
// 2. Input parameters are mutated in the function
// 3. Void function has not output so it is not testable
// 4. The function relies on mutation

Math01(5, 10, 
       out int sum, 
       out int dif, 
       out decimal pro, 
       out decimal quo);
WriteLine("Sum : {0}, Dif :{1} , Pro :{2} , Quo: {3}", 
          sum, dif, pro, quo);

// Let's fix this:
// 1st thing: I want to remove use of out parameters and 
// define a type to store the output
public record CalcData02(int Sum, int Dif, decimal Pro, decimal Quo);
public static CalcData02 Math02(int a, int b) =>
  new CalcData02(a - b, a + b, (decimal)a * b, (decimal)a / b);
// What is the problem with Math03 function?
// 1. I can swap Sum and Dif and the compiler will not complain
// 2. I can also swap Pro and Quo and the compiler will not complain
CalcData02 calcData02 = Math02(5, 10);
WriteLine("Sum : {0}, Dif :{1} , Pro :{2} , Quo: {3}", 
          calcData02.Sum, 
          calcData02.Dif, 
          calcData02.Pro, 
          calcData02.Quo);
// Line below  error CS8852: Init-only property or indexer 
// 'calcData02.Sub' can only be assigned in an object initialize
// calcData02.Sub = 30; 

// 2nd thing: I want the compiler to enforce the order in the 
// MathResult object
public record CalcData03(Sum Sum, 
                         Dif Dif, 
                         Pro Pro, 
                         Quo Quo);
public static CalcData03 Math03(int a, int b) =>  
  new CalcData03(Sum.Calc(a, b), 
                 Dif.Calc(a, b), 
                 Pro.Calc(a, b), 
                 Quo.Calc(a, b));
// What is the problem with Math03 function? Nothing!
public record Sum(int Value) {
  public static Sum Calc(int a, int b) => new(a + b);
}
public record Dif(int Value) {
  public static Dif Calc(int a, int b) => new(a - b);
}
public record Pro(decimal Value) { 
  public static Pro Calc(decimal a, decimal b) => 
    new((decimal)a * b);
}
public record Quo(decimal Value) { 
  public static Quo Calc(decimal a, decimal b) => 
    new((decimal)a / b);
}
CalcData03 calcData03 = Math03(5, 10);
WriteLine("Sum : {0}, Dif :{1} , Pro :{2} , Quo: {3}", 
          calcData03.Sum.Value, 
          calcData03.Dif.Value, 
          calcData03.Pro.Value, 
          calcData03.Quo.Value);

/*------------------------------------------*
  tests to run
\*------------------------------------------*/

// Test# 1
// WriteLine("Given Input when str1='a' and str2='b'-> Smaller");
// WriteLine("Test #1: {0}", 
//   (StringCompareResult.Smaller.ToString()         // expected
//   ==
//   CompareTwoStringsPurely("a", "b").ToString())  // actual
//   == true ? "Passed" : "Failed!");

// WriteLine("-------------------------------------------------------");
