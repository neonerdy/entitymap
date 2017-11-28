# Entitymap

## How to Use

Open Connection

```
DataSource dataSource = new DataSource();
dataSource.Provider = "System.Data.SqlClient";
dataSource.ConnectionString = "Data Source=XERIS\\SQLEXPRESS;"
 + "Initial Catalog=Northwind;Integrated Security=True";
IEntityManager em = EntityManagerFactory.CreateInstance(dataSource);

```

ExecuteReader()

```
IDataReader rdr=em.ExecuteReader("SELECT * FROM Customers");
while (rdr.Read())
{
 Console.WriteLine(rdr["CustomerId"]);
 Console.WriteLine(rdr["CompanyName"]);
}

```

ExecuteNonQuery()

```
string sql="INSERT INTO Customers (CustomerId,CompanyName) VALUES "
 + "('MSFT','Microsoft')";
em.ExecuteNonQuery(sql);

```

ExecuteObject()

```
string sql="SELECT * FROM Customers ";
Customer cust = em.ExecuteObject<Customer>(sql, new Customer Mapper());
Console.WriteLine(cust.CustomerId);
Console.WriteLine(cust.CompanyName);

```

ExecuteList()

```
string sql="SELECT * FROM Customers ";
List<Customer> custs = em.ExecuteList< Customer >(sql, new CustomerMapper());
foreach (Customer cust in custs)
{
 Console.WriteLine(cust.CustomerId);
 Console.WriteLine(cust.CompanyName);
}

```




