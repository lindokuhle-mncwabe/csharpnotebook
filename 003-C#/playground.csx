
using System;
using System.Collections.Immutable;
using System.Text.Json;
using static System.Console;

#nullable enable

/*--------------------------------------------------------------
  Diddling
--------------------------------------------------------------*/
public sealed record Either<T, TError>
{
  public T? Value { get; }
  public TError? Error { get; }

  private Either(T value, TError error)
  {
      Value = value;
      Error = error;
  }

  public static Either<T, TError> Ok(T value) 
    => new(value, default!);

  public static Either<T, TError> Fail(TError error) 
    => new(default!, error);

  public TResult Match<TResult>(Func<T, TResult> ok, 
    Func<TError, TResult> fail
  ) => Error != null 
    ? fail(Error)
    : ok(Value!); 
}

public static Either<int, string> Divide(int dividend, 
  int divisor
) => divisor == 0 
  ? Either<int, string>.Fail("Divisor cannot be zero") 
  : Either<int, string>.Ok(dividend / divisor);

Either<int, string> result = Divide(10, 0);
WriteLine(result);

string output = result.Match(
    value => $"Result: {value}",
    error => $"Result: {error}"
);

WriteLine(output);

/*--------------------------------------------------------------
  Test #1 : 
--------------------------------------------------------------*/
/*{
  Describe(
    @"When Customer.BuildCustomerCreateCommand is called with 
    firstName='Jane', 
    lastName='Parker'
    email='jane.parker@dependencyinjection.com'
    and phone='123-456-7890'
    then CustomerCreateCommand with correct data should be returned."
  );

  // Arrange
  var data = new {
    FirstName = "Jane",
    LastName = "Parker",
    Email = "jane.parker@dependencyinjection.com",
    Phone = "123-456-7890"
  };

  // Act
  CustomerCreateCommand sut = Customer.BuildCustomerCreateCommand(
    data.FirstName,
    data.LastName,
    data.Email,
    data.Phone
  );

  // Assert
  var assertPass = (
    sut.FirstName == data.FirstName &&
    sut.LastName == data.LastName &&
    sut.Email == data.Email &&
    sut.Phone == data.Phone
  );

  WriteLine("Test #1: {0}", assertPass 
    ? "Passed [:-)" : "Failed [:-(");

  // Output
  if (!assertPass) 
  {
    WriteLine(
      @"[Actual: 
      FirstName = {0}, 
      LastName  = {1}, 
      Email     = {2}, 
      Phone     = {3}]", 
      sut.FirstName, 
      sut.LastName, 
      sut.Email, 
      sut.Phone
    );
  }
}

*/
