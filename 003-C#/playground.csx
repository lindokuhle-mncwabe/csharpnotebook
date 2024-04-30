
using System;
using System.Collections.Immutable;
using System.Text.Json;
using static System.Console;

// -------------------------------------------
// Refactoring from Dependency Injection to 
// Dependency Parameterization
//--------------------------------------------

/*--------------------------------------------------------------*\
  Customer Data and Actions
\*--------------------------------------------------------------*/

public record Customer(Guid CustomerId,
  string FirstName, string LastName, string Email, string Phone
)
{
  public static Customer MapCustomerCreateCommandToCustomer(
    CustomerCreateCommand command
  ) => new Customer(Guid.NewGuid(), 
         command.FirstName, command.LastName, 
         command.Email, command.Phone
       );

  public static CustomerCreateCommand BuildCustomerCreateCommand(
    string firstName, string lastName, string email, string phone
  ) => new CustomerCreateCommand(firstName, 
         lastName, email, phone
       );

  public static (CustomerData, Error) MapCustomerCreateResult(
    Customer customer, Error error
  ) 
  {
    if (error != null)
    { 
      return (null, error);
    }
    var data = new CustomerData(customer.CustomerId, 
      customer.FirstName, customer.LastName, 
      customer.Email, customer.Phone
    );
    return (data, null);
  }
}
public record CustomerCreateCommand(string FirstName,
  string LastName, 
  string Email, 
  string Phone
);
public record CustomerData(Guid CustomerId,
  string FirstName, 
  string LastName, 
  string Email, 
  string Phone
);
public record CustomerReadRequest(Guid CustomerId);

public static ImmutableHashSet<Customer> _customerDb = ImmutableHashSet<Customer>.Empty;

public record CreateCustomerHandler
{
  public static async Task<(CustomerData, Error)> Handle(
    CustomerCreateCommand command,
    CreateCustomerAction createCustomer
  ) 
  {
    var customer = 
      Customer.MapCustomerCreateCommandToCustomer(command);
    var result = Customer.MapCustomerCreateResult(customer, null);
    // Save customer to database
    await createCustomer(customer);
    return await Task.FromResult(result);
  }
}

public record ReadCustomerHandler
{
  public static async Task<(CustomerData, Error)> Handle(
    CustomerReadRequest request,
    ReadCustomerAction readCustomer
  ) 
  {
    var customer = await readCustomer(request);
    var result = Customer.MapCustomerCreateResult(customer, null);
    return await Task.FromResult(result);
  }
}

// Actions Defs
public delegate Task<(Customer, Error)> CreateCustomerAction(
  Customer customer
);
public delegate Task<Customer> ReadCustomerAction(
  CustomerReadRequest request
);

// Actions Handlers
public async static Task<(Customer, Error)> CreateCustomerActionHandlerAsync(
  Customer customer
) 
{
  // depends on data store or api resource
  _customerDb = _customerDb.Add(customer);
  (Customer, Error) result = (customer, null);
  return await Task.FromResult(result);
}
public async static Task<Customer> ReadCustomerActionHandlerAsync(
  CustomerReadRequest request
) 
{
  // depends on data store or api resource
  var customer = _customerDb.FirstOrDefault(c => c.CustomerId == request.CustomerId);
  return await Task.FromResult(customer);
}

/*--------------------------------------------------------------*\
  Shared Data
\*--------------------------------------------------------------*/

public record EmailMessage(string From,
  string To,
  string Subject,
  string Body);

public record Error(string Message);

/*--------------------------------------------------------------*\
  tests to run
\*--------------------------------------------------------------*/
void Describe(string description){}

// Test #4 : CreateCustomerHandler.Handle
Describe(
  @"When CreateCustomerHandler.Handle is called with command 
  FirstName='Jane', 
  LastName='Parker'
  Email='jane.parker@dependencyinjection.com'
  and Phone='123-456-7890'; and passing parameterized dependency
  for storing customer in the database and can be retrieved later; 
  then Customer with correct data should be created & returned."
);

// Arrange
var dataT4 = new {
  FirstName = "Jane",
  LastName = "Parker",
  Email = "jane.parker@dependencyinjection.com",
  Phone = "123-456-7890"
};
CustomerCreateCommand command = Customer.BuildCustomerCreateCommand(
  dataT4.FirstName,
  dataT4.LastName,
  dataT4.Email,
  dataT4.Phone
);

// Act
(CustomerData, Error) sut4 = 
  await CreateCustomerHandler.Handle(command, 
    CreateCustomerActionHandlerAsync
  );

// Assert
var (customerData, _) = sut4; 
var assertPassT4 = (
  customerData.CustomerId != Guid.Empty &&
  customerData.FirstName == command.FirstName &&
  customerData.LastName == command.LastName &&
  customerData.Email == command.Email &&
  customerData.Phone == command.Phone
);

WriteLine("Test #4: {0} --data write", assertPassT4 
  ? "Passed [:-)" : "Failed [:-(");

(CustomerData, Error) sut5 = await ReadCustomerHandler.Handle(
  new CustomerReadRequest(customerData.CustomerId), 
  ReadCustomerActionHandlerAsync
);

var (customerDataB, _) = sut5;
var assertPassT4B = (
  customerDataB.CustomerId == customerData.CustomerId
);

WriteLine("Test #4: {0} --data read", assertPassT4B 
  ? "Passed [:-)" : "Failed [:-(");

if (!assertPassT4) 
{
  WriteLine(
    @"[Actual: 
    FirstName  = {0}, 
    LastName   = {1}, 
    Email      = {2}, 
    Phone      = {3},
    CustomerId = {4}]", 
    customerData.FirstName, 
    customerData.LastName, 
    customerData.Email, 
    customerData.Phone,
    customerData.CustomerId);
}
/*
// Test #3 : CreateCustomerHandler.Handle
Describe(
  @"When CreateCustomerHandler.Handle is called with command 
  FirstName='Jane', 
  LastName='Parker'
  Email='jane.parker@dependencyinjection.com'
  and Phone='123-456-7890'
  then Customer with correct data should be created & returned."
);

// Arrange
var dataT3 = new {
  FirstName = "Jane",
  LastName = "Parker",
  Email = "jane.parker@dependencyinjection.com",
  Phone = "123-456-7890"
};
CustomerCreateCommand command = Customer.BuildCustomerCreateCommand(
  dataT3.FirstName,
  dataT3.LastName,
  dataT3.Email,
  dataT3.Phone);

// Act
(CustomerData, Error) sut3 = 
  await CreateCustomerHandler.Handle(command);

// Assert
var (customerData, _) = sut3;
var assertPassT3 = (
  customerData.CustomerId != Guid.Empty &&
  customerData.FirstName == command.FirstName &&
  customerData.LastName == command.LastName &&
  customerData.Email == command.Email &&
  customerData.Phone == command.Phone
);

WriteLine("Test #3: {0}", assertPassT3 
  ? "Passed [:-)" : "Failed [:-(");

if (!assertPassT3) 
{
  WriteLine(
    @"[Actual: 
    FirstName  = {0}, 
    LastName   = {1}, 
    Email      = {2}, 
    Phone      = {3},
    CustomerId = {4}]", 
    customerData.FirstName, 
    customerData.LastName, 
    customerData.Email, 
    customerData.Phone,
    customerData.CustomerId);
}

// Test #2 : Customer.MapCustomerCreateCommandToCustomer
Describe(
  @"When CustomerMapper.MapCustomerCreateCommandToCustomer is called with 
  firstName='Jane', 
  lastName='Parker'
  email='jane.parker@dependencyinjection.com'
  and phone='123-456-7890'
  then Customer with correct data should be returned."
);

// Arrange
var dataT2 = new {
  FirstName = "Jane",
  LastName = "Parker",
  Email = "jane.parker@dependencyinjection.com",
  Phone = "123-456-7890"
};
CustomerCreateCommand command = Customer.BuildCustomerCreateCommand(
  dataT2.FirstName,
  dataT2.LastName,
  dataT2.Email,
  dataT2.Phone
);

// Act
Customer sutT2 = Customer.MapCustomerCreateCommandToCustomer(command);

// Assert
var assertPassT2 = (
  sutT2.FirstName == command.FirstName &&
  sutT2.LastName == command.LastName &&
  sutT2.Email == command.Email &&
  sutT2.Phone == command.Phone
);

WriteLine("Test #2: {0}", assertPassT2 
  ? "Passed [:-)" : "Failed [:-(");

if (!assertPassT2) 
{
  WriteLine(
    @"[Actual: 
    FirstName = {0}, 
    LastName  = {1}, 
    Email     = {2}, 
    Phone     = {3}]", 
    sutT2.FirstName, 
    sutT2.LastName, 
    sutT2.Email, 
    sutT2.Phone
  );
}

// Test #1 : Customer.BuildCustomerCreateCommand
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
*/



/*
public delegate Task<Customer> ReadCustomerHandler(CustomerRead request);
public delegate Task UpdateCustomerHandler(Customer customer);
public delegate Task SendEmailMessageHandler(EmailMessage message);
public delegate Task TwillioEmailMessageSender(EmailMessage message);


public async static Task<Customer> CreateCustomerAsync(Customer customer) {
  var newCustomer = new Customer(
    1,
    "Jane",
    "Parker",
    "jane.parker@dependencyinjection.com",
    "123-456-7890"
  );
  
  return await Task.FromResult(newCustomer);
}

ImmutableHashSet<Customer> list = ImmutableHashSet<Customer>.Empty;
async static void PersistNewCustomerAsync(Customer newCustomer
, PersistNewCustomerHandler persistCustomer) {
  persistCustomer(newCustomer);
  WriteLine($"New customer created: {newCustomer}");
  await Task.CompletedTask;
}

async static void UpdateCustomer(Customer newCustomer
,DbReadCustomerAsync readCustomer
,DbUpdateCustomerAsync updateCustomer
,TwillioSendEmailMessageAsync sendEmailMessage) {
  // retrieving the existing customer
  // update the customer if changed
  // send an email about the change
  await Task.CompletedTask;
}

// Command Handlers | Query Handlers
public async Task<(Customer, Error)> CreateCustomerHandler(
  MapCustomerCreateAction mapCustomerCreate,
  CreateCustomerAction createCustomer) 
{
  // depends on service actions
  (Customer, Error) result = (null, null);
  return await Task.FromResult(result);
}

public delegate Task<Customer> MapCustomerCreateAction();
public async static Task<(Customer, Error)> MapCustomerCreateAsync() 
{
  (Customer, Error) result = (null, null);
  return await Task.FromResult(result);
}

public delegate Task<(Customer, Error)> CreateCustomerAction(Customer customer);

// Service Actions
public async static Task<(Customer, Error)> CreateCustomerAsync(
  Customer customer) 
{
  // depends on data store action or api resource
  (Customer, Error) result = (null, null);
  return await Task.FromResult(result);
}

// Data Store Actions
public async static Task DataStoreCreateCustomer(Customer customer) {
  await Task.CompletedTask;
}

*/
