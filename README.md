# EntityMap

EntityMap is lightweight data acees framework that implement repository pattern, data mapper patern, and fluent interface.

## How to Use

Open Connection

```
DataSource dataSource = new DataSource();
dataSource.Provider = "System.Data.SqlClient";
dataSource.ConnectionString = "Data Source=XERIS\\SQLEXPRESS;Initial Catalog=Northwind;Integrated Security=True";
IEntityManager em = EntityManagerFactory.CreateInstance(dataSource);

```

### ExecuteReader()

```
IDataReader rdr=em.ExecuteReader("SELECT * FROM Customers");
while (rdr.Read())
{
   Console.WriteLine(rdr["CustomerId"]);
   Console.WriteLine(rdr["CompanyName"]);
}

```

### ExecuteNonQuery()

```
string sql="INSERT INTO Customers (CustomerId,CompanyName) VALUES ('MSFT','Microsoft')";
em.ExecuteNonQuery(sql);

```

### ExecuteObject()

```
string sql="SELECT * FROM Customers ";
Customer cust = em.ExecuteObject<Customer>(sql, new Customer Mapper());
Console.WriteLine(cust.CustomerId);
Console.WriteLine(cust.CompanyName);

```

### ExecuteList()

```
string sql="SELECT * FROM Customers ";
List<Customer> custs = em.ExecuteList<Customer>(sql, new CustomerMapper());
foreach (Customer cust in custs)
{
   Console.WriteLine(cust.CustomerId);
   Console.WriteLine(cust.CompanyName);
}

```

### Customer Model

```
public class Customer
{
    public string CustomerId { get; set; }
    public string CompanyName { get; set; }
    public string ContactName { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
}


```


### Customer Mapper

```
public class CustomerMapper : IDataMapper<Customer>
{
   public Customer Map(IDataReader rdr)
   {
       Customer customer = new Customer();
 
       customer.CustomerId = rdr["CustomerId"] is DBNull ? string.Empty : (string)rdr["CustomerId"];
       customer.CompanyName = rdr["CompanyName"] is DBNull ? string.Empty : (string)rdr["CompanyName"];
       customer.ContactName = rdr["ContactName"] is DBNull ? string.Empty : (string)rdr["ContactName"];
       customer.Address = rdr["Address"] is DBNull ? string.Empty : (string)rdr["Address"];
       customer.Phone = rdr["Phone"] is DBNull ? string.Empty : (string)rdr["Phone"];
 
       return customer;
    }
}



```

### Fluent Query

```
Query q = new Query().From("Customers").Where("CustomerId").Equal("ALFKI");
Console.WriteLine(q.GetSql());

```

```
string[] fields = {"CustomerId","CompanyName"};
object[] values = {"MSFT","Microsoft"};

int result = new Query().Select(fields).From("Customers").Insert(values).ExecuteNonQuery();

string[] fields = {"CompanyName"};
object[] values = {"Microsoft Corporation"};

int result = new Query().Select(fields).From("Customers").Update(values).Where("CustomerId").Equal("MSFT").ExecuteNonQuery();

```






