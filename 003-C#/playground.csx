
using System;
using System.Collections.Immutable;
using System.Text.Json;
using static System.Console;

#nullable enable

/*--------------------------------------------------------------
  Functional Programming in C# - Part 1
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

public abstract record NameType;
internal record FullName(string FirstName, 
  string LastName
) : NameType;
internal record Mononym(string Name) : NameType;

public record Name
{
  public static Either<NameType, string> Create(string firstName, 
    string lastName
  ) => string.IsNullOrWhiteSpace(firstName) || 
       string.IsNullOrWhiteSpace(lastName)
    ? Either<NameType, string>.Fail("First and last name cannot be empty")
    : Either<NameType, string>.Ok(new FullName(firstName, lastName));

  public static Either<NameType, string> Create(string name)
    => string.IsNullOrWhiteSpace(name)
      ? Either<NameType, string>.Fail("Name cannot be empty")
      : Either<NameType, string>.Ok(new Mononym(name));  
}

string output = Name.Create("Jane", "Parker").Match(
  value => value switch
  {
    FullName name => $"Full Name: {name.FirstName} {name.LastName}",
    Mononym name => $"Mononym: {name.Name}",
    _ => "Unknown"
  },
  error => error
);

WriteLine(output);
WriteLine("---------------------------------------------");

public record Book(string Title, string[] Authors)
{
  public static Either<Book, string> Create(string title, 
    params string[] authors
  ) => string.IsNullOrWhiteSpace(title)
    ? Either<Book, string>.Fail("Title cannot be empty")
    : Either<Book, string>.Ok(new Book(title, authors));
}

string book = Book.Create(
  "Dependency Injection Principles, Practices, and Patterns", 
  "Mark Seemann" 
  //"Steven van Deursen"
).Match(
  value => $"Book: {value.Title} by {string.Join(", ", value.Authors)}",
  error => error
);

WriteLine(book);
WriteLine("---------------------------------------------");

public static Either<int, string> Divide(int dividend, 
  int divisor
) => divisor == 0 
  ? Either<int, string>.Fail("Divisor cannot be zero") 
  : Either<int, string>.Ok(dividend / divisor);

string answer = Divide(10, 2).Match(
    value => $"Result: {value}",
    error => $"Result: {error}"
);

WriteLine(answer);

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
