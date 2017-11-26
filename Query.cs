
/*===========================================================
 * 
 * EntitMap 
 * 
 * Lightweight Data Access Layer Framework
 *
 * Version      : 2.0
 * Author       : Ariyanto
 * E-Mail       : neonerdy@gmail.com
 * Last Updated : 06/28/2011
 *  
 * 
 * © 2009, Under Apache Licence 
 * 
 *==========================================================
 */

using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

namespace EntityMap
{

    public class Query
    {
        private StringBuilder q = new StringBuilder();
        private string tableName;
        private string columns;
        private string[] fields;

       
        public Query()
        {
            q.Append("SELECT");
        }

        public Query Select(string columns)
        {
            this.columns = columns;
            q.Append(" " + columns);
            return this;
        }

        //for INSERT and UPDATE

        public Query Select(string[] fields)
        {
            this.fields = fields;
            return this;
        }


        public Query From(string tableName)
        {
            if (fields == null)
            {
                if (columns != null)
                {
                    q.Append(" FROM " + tableName);
                }
                else
                {
                    q.Append(" * FROM " + tableName);
                }
            }

            this.tableName = tableName;
            return this;
        }

        public Query GroupBy(string columnName)
        {
            q.Append(" GROUP BY " + columnName);
            return this;
        }

        public Query Having(string columnName)
        {
            q.Append(" HAVING " + columnName);
            return this;
        }


        public Query Where(string columnName)
        {
            q.Append(" WHERE " + columnName);
            return this;
        }

        public Query OrderBy(string columnOrder)
        {
            q.Append(" ORDER BY " + columnOrder);
          
            return this;
        }

        public Query And(string columnName)
        {
            q.Append(" AND " + columnName);
            return this;
        }

        public Query Or(string columnName)
        {
            q.Append(" OR " + columnName);
            return this;
        }


        public Query Equal(object value)
        {
            if (value.GetType() == typeof(string) 
                || value.GetType() == typeof(Guid)
                || value.GetType() == typeof(DateTime)
                || value.GetType() == typeof(Boolean)
                || value.GetType() == typeof(Char))
            {
                q.Append(" = '" + value + "'");
            }
            else
            {
                q.Append(" = " + value);
            }
            return this;
        }


        public Query NotEqual(object value)
        {
            if (value.GetType() == typeof(string)
               || value.GetType() == typeof(Guid)
               || value.GetType() == typeof(DateTime)
               || value.GetType() == typeof(Boolean)
               || value.GetType() == typeof(Char))
            {
                q.Append(" <> '" + value + "'");
            }
            else
            {
                q.Append(" <> " + value);
            }
            return this;
        }

        public Query Like(string value)
        {
            q.Append(" LIKE " + "'" + value + "'");
            return this;
        }

        public Query NotLike(string value)
        {
            q.Append(" NOT LIKE '" + value + "'");
            return this;
        }

        public Query LessThan(object value)
        {
            q.Append(" < " + value);
            return this;
        }

        public Query LessEqualThan(object value)
        {
            q.Append(" <= " + value);
            return this;
        }


        public Query GreaterThan(object value)
        {
            q.Append(" > " + value);
            return this;
        }

        public Query GreaterEqualThan(object value)
        {
            q.Append(" >= " + value);
            return this;
        }

        public Query Between(object from, object to)
        {
            if (from.GetType() == typeof(DateTime)
                && to.GetType() == typeof(DateTime))
            {
                q.Append(" BETWEEN '" + from + "' AND '" + to + "'");
            }
            else
            {
                q.Append(" BETWEEN " + from + " AND " + to);
            }
            return this;
        }

        public Query NotBetween(object from, object to)
        {
            if (from.GetType() == typeof(DateTime)
                && to.GetType() == typeof(DateTime))
            {
                q.Append(" NOT BETWEEN '" + from + "' AND '" + to + "'");
            }
            else
            {
                q.Append(" NOT BETWEEN " + from + " AND " + to);
            }
            return this;
        }

        public Query In(object[] values)
        {
            string s = "";
            string str = "";

            for (int i = 0; i <= values.Length - 1; i++)
            {
                if (values.GetType() == typeof(string[])
                    || values.GetType() == typeof(Guid[])
                    || values.GetType() == typeof(DateTime[])
                    || values.GetType() == typeof(Char[])
                    || values.GetType() ==  typeof(Boolean[]))
                {
                    s = s + "'" + values[i] + "',";
                    str = s.Substring(0, s.Length - 1);
                }
                else
                {
                    s = s + values[i] + ",";
                    str = s.Substring(0, s.Length - 1);
                }
            }

            q.Append(" IN (" + str + ")");

            return this;

        }


        public Query NotIn(object[] values)
        {
            string s = "";
            string str = "";

            for (int i = 0; i <= values.Length - 1; i++)
            {
                if (values.GetType() == typeof(string[])
                     || values.GetType() == typeof(Guid[])
                     || values.GetType() == typeof(DateTime[])
                     || values.GetType() == typeof(Char[])
                     || values.GetType() == typeof(Boolean[]))
                {
                    s = s + "'" + values[i] + "',";
                    str = s.Substring(0, s.Length - 1);
                }
                else
                {
                    s = s + values[i] + ",";
                    str = s.Substring(0, s.Length - 1);
                }
            }

            q.Append(" NOT IN (" + str + ")");

            return this;

        }

        public Query InnerJoin(string joinTable, string joinTableKey,
            string mainTableKey)
        {

            q.Append(" INNER JOIN " + joinTable + " ON " + joinTableKey 
               + " = " + mainTableKey);

            return this;
        }

        public Query OuterJoin(string joinTable, string joinTableKey,
            string mainTableKey)
        {
            q.Append(" OUTER JOIN " + joinTable + " ON " + joinTableKey
                + " = " + mainTableKey);
            return this;
        }


        public Query RightJoin(string joinTable, string joinTableKey,
            string mainTableKey)
        {
            q.Append(" RIGHT JOIN " + joinTable + " ON " + joinTableKey
                + " = " + mainTableKey);

            return this;
        }


        public Query LeftJoin(string joinTable, string joinTableKey,
            string mainTableKey)
        {
            q.Append(" LEFT JOIN " + joinTable + " ON " + joinTableKey
                + " = " + mainTableKey);

            return this;
        }


        public Query Insert(object[] values)
        {
            string sql = "INSERT INTO " + this.tableName + " (";

            for (int i = 0; i <= this.fields.Length - 1; i++)
            {
                if (i == this.fields.Length - 1)
                {
                    sql = sql + this.fields[i] + ")";
                }
                else
                {
                    sql = sql + this.fields[i] + ",";
                }
            }

            sql = sql + " VALUES (";

            for (int i = 0; i <= values.Length - 1; i++)
            {
                if (i == values.Length - 1)
                {
                    if (values[i].GetType() == typeof(string) || values[i].GetType() == typeof(char) ||
                     values[i].GetType() == typeof(DateTime) || values[i].GetType() == typeof(Guid) ||
                     values[i].GetType()==typeof(Boolean))
                    {
                        sql = sql + "'" + values[i] + "')";
                    }
                    else
                    {
                        sql = sql + values[i] + ")";
                    }
                }
                else
                {
                    if (values[i].GetType() == typeof(string) || values[i].GetType() == typeof(char) ||
                     values[i].GetType() == typeof(DateTime) || values[i].GetType() == typeof(Guid) ||
                     values[i].GetType()==typeof(Boolean))             
                    {
                        sql = sql + "'" + values[i] + "',";
                    }
                    else
                    {
                        sql = sql + values[i] + ",";
                    }
                }
            }

            q.Remove(0, 6);
            q.Append(sql);

            return this;
        }


        public Query Update(object[] values)
        {
            string sql = "UPDATE " + tableName + " SET ";

            for (int i = 0; i <= this.fields.Length - 1; i++)
            {
                if (i == fields.Length - 1)
                {
                    if (values[i].GetType() == typeof(string) || values[i].GetType() == typeof(char) ||
                        values[i].GetType() == typeof(DateTime) || values[i].GetType() == typeof(Guid) ||
                        values[i].GetType() == typeof(Boolean))      
                  
                    {
                        sql = sql + this.fields[i] + "='" + values[i] + "'";
                    }
                    else
                    {
                        sql = sql + this.fields[i] + "=" + values[i] + "";
                    }
                }
                else
                {
                    if (values[i].GetType() == typeof(string) || values[i].GetType() == typeof(char) ||
                       values[i].GetType() == typeof(DateTime) || values[i].GetType() == typeof(Guid) ||
                       values[i].GetType() == typeof(Boolean))  
                    {
                        sql = sql + this.fields[i] + "='" + values[i] + "',";
                    }
                    else
                    {
                        sql = sql + this.fields[i] + "=" + values[i] + ",";
                    }
                }
            }

            q.Remove(0, 6);
            q.Append(sql);

            return this;
        }

        public Query Delete()
        {
            string sql = "DELETE FROM " + this.tableName;
            q.Remove(0, 14+ this.tableName.Length);
            q.Append(sql);

            return this;
        }

        public string ToSql()
        {
            return q.ToString();
        }



    }
}
