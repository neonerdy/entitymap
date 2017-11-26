
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
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace EntityMap
{
    public class DbHelper
    {
        private DbConnection conn;
        private DataSource dataSource;
     
        public DbHelper(DbConnection conn)
        {
            if (conn == null)
                throw new Exception("Connection is null");

            this.conn = conn;
        }


        public DbHelper(DataSource dataSource)
        {
            if (dataSource == null)
                throw new Exception("Data Source is null");

            try
            {
                this.dataSource = dataSource;
                if (conn == null)
                {
                    conn = ConnectionFactory.CreateConnection(dataSource);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Transaction BeginTransaction()
        {
            return new Transaction(conn);
        }

        public int ExecuteNonQuery(string sql, bool isTransaction, Transaction tx)
        {
            if (string.IsNullOrEmpty(sql))
                throw new Exception("Sql is empty");

            int result = 0;
            DbCommand cmd = null;
            try
            {
                cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                if (isTransaction)
                {
                    cmd.Transaction = tx.GetTransaction();
                }

                cmd.CommandText = sql;
                result = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
            }

            return result;
        }


        public int ExecuteNonQuery(DbCommandWrapper cmdWrapper,
            bool isStoredProc, string sql, bool isTransaction, Transaction tx)
        {

            if (cmdWrapper == null)
                throw new Exception("DbCommandWrapper is null");

            if (string.IsNullOrEmpty(sql))
                throw new Exception("Sql or stored procedure is empty");

            int result = 0;
            DbCommand cmd = null;

            try
            {
                cmd = conn.CreateCommand();
                cmdWrapper.Command = cmd;

                if (isTransaction)
                {
                    cmd.Transaction = tx.GetTransaction();
                }
                if (isStoredProc)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    cmd.CommandType = CommandType.Text;
                }
                cmd.CommandText = sql;
                AddParameters(cmd, cmdWrapper);
                result = cmd.ExecuteNonQuery();
                cmdWrapper.PopulateOutputParameters();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
            }

            return result;
        }


        private DbParameter CreateDbParameter(DbCommand cmd, ParameterClause param)
        {
            DbParameter sqlParam = cmd.CreateParameter();

            sqlParam.ParameterName = param.Name;
            sqlParam.DbType = param.DbType;
            sqlParam.Value = ReferenceEquals(param.Argument, null) ? DBNull.Value : param.Argument;
            sqlParam.Direction = param.Direction;
            sqlParam.FixParameterRange();

            return sqlParam;
        }

        private void AddParameters(DbCommand cmd, DbCommandWrapper cmdWrapper)
        {
            foreach (ParameterClause param in cmdWrapper.Parameters)
            {
                cmd.Parameters.Add(CreateDbParameter(cmd, param));
            }
        }

        public IDataReader ExecuteReader(string sql)
        {
            if (string.IsNullOrEmpty(sql))
                throw new Exception("Sql is empty");

            DbDataReader reader = null;
            DbCommand cmd = null;
            try
            {
                cmd = conn.CreateCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                reader = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
            }
            return reader;
        }



        public IDataReader ExecuteReader(DbCommandWrapper cmdWrapper,
           bool isStoredProc, string sql, bool isTransaction, Transaction tx)
        {
            if (cmdWrapper == null)
                throw new Exception("DbCommandWrapper is null");

            if (string.IsNullOrEmpty(sql))
                throw new Exception("Sql or stored procedure is empty");

            DbCommand cmd = null;
            IDataReader rdr = null;
            try
            {
                cmd = conn.CreateCommand();
                cmdWrapper.Command = cmd;

                if (isStoredProc)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    cmd.CommandType = CommandType.Text;
                }

                cmd.CommandText = sql;
                if (isTransaction)
                {
                    cmd.Transaction = tx.GetTransaction();
                }
                AddParameters(cmd, cmdWrapper);
                rdr = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
            }

            return rdr;
        }



        public DataSet ExecuteDataSet(string sql)
        {
            if (string.IsNullOrEmpty(sql))
                throw new Exception("Sql is empty");

            DataSet dataSet = null;
            DbCommand cmd = null;
            DbDataAdapter da = null;

            try
            {
                DbProviderFactory factory = DbProviderFactories.GetFactory(dataSource.Provider);
                da = factory.CreateDataAdapter();
                cmd = conn.CreateCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;

                dataSet = new DataSet();
                da.SelectCommand = cmd;
                da.Fill(dataSet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (da != null) da.Dispose();
                if (cmd != null) cmd.Dispose();
            }
            return dataSet;
        }


        public DataSet ExecuteDataSet(DbCommandWrapper cmdWrapper,
           bool isStoredProc, string sql)
        {

            if (cmdWrapper == null)
                throw new Exception("DbCommandWrapper is null");

            if (string.IsNullOrEmpty(sql))
                throw new Exception("Sql or stored procedure is empty");

            DataSet dataSet = null;
            DbCommand cmd = null;
            DbDataAdapter da = null;

            try
            {
                DbProviderFactory factory = DbProviderFactories.GetFactory(dataSource.Provider);
                da = factory.CreateDataAdapter();
                cmd.Connection = conn;

                cmd = conn.CreateCommand();
                if (isStoredProc)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    cmd.CommandType = CommandType.Text;
                }
                cmd.CommandText = sql;

                dataSet = new DataSet();

                AddParameters(cmd, cmdWrapper);

                da.SelectCommand = cmd;
                da.Fill(dataSet);
            }
            catch (Exception ex)
            {
                 throw ex;
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
            }

            return dataSet;
        }



        public object ExecuteScalar(string sql, bool isTransaction, Transaction tx)
        {
            if (string.IsNullOrEmpty(sql))
                throw new Exception("Sql is empty");

            object result = null;
            DbCommand cmd = null;

            try
            {
                cmd = conn.CreateCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;

                if (isTransaction)
                {
                    cmd.Transaction = tx.GetTransaction();
                }

                cmd.CommandText = sql;
                result = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                 throw ex;
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
            }
            return result;
        }



        public object ExecuteScalar(DbCommandWrapper cmdWrapper,
           bool isStoredProc, string sql, bool isTransaction, Transaction tx)
        {

            if (cmdWrapper == null)
                throw new Exception("DbCommandWrapper is null");

            if (string.IsNullOrEmpty(sql))
                throw new Exception("Sql or stored procedure is empty");

            object result = null;
            DbCommand cmd = null;
            try
            {
                cmd = conn.CreateCommand();
                cmdWrapper.Command = cmd;

                if (isTransaction)
                {
                    cmd.Transaction = tx.GetTransaction();
                }

                if (isStoredProc)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    cmd.CommandType = CommandType.Text;
                }

                cmd.CommandText = sql;
                AddParameters(cmd, cmdWrapper);
                result = cmd.ExecuteScalar();

                cmdWrapper.PopulateOutputParameters();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
            }

            return result;
        }



        public T ExecuteObject<T>(string sql, IDataMapper<T> rowMapper,
            bool isTransaction, Transaction tx)
        {
            if (string.IsNullOrEmpty(sql))
                throw new Exception("Sql is empty");

            if (rowMapper == null)
                throw new Exception("Mapper is null");

            T obj = default(T);
            IDataReader rdr = null;
            DbCommand cmd = null;

            try
            {
                cmd = conn.CreateCommand();
                if (isTransaction)
                {
                    cmd.Transaction = tx.GetTransaction();
                }
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    obj = rowMapper.Map(rdr);
                }
            }
            catch (Exception ex)
            {
                 throw ex;
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (rdr != null) rdr.Close();
            }

            return obj;
        }


        public T ExecuteObject<T>(DbCommandWrapper cmdWrapper, bool isStoredProc,
           string sql, IDataMapper<T> dataMapper, bool isTransaction, Transaction tx)
        {

            if (cmdWrapper == null)
                throw new Exception("DbCommandWrapper is null");

            if (string.IsNullOrEmpty(sql))
                throw new Exception("Sql or stored procedure is empty");

            if (dataMapper == null)
                throw new Exception("Mapper is null");

            T obj = default(T);
            DbCommand cmd = null;

            try
            {
                cmd = conn.CreateCommand();
                cmdWrapper.Command = cmd;

                if (isTransaction)
                {
                    cmd.Transaction = tx.GetTransaction();
                }
                if (isStoredProc)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    cmd.CommandType = CommandType.Text;
                }

                cmd.CommandText = sql;

                AddParameters(cmd, cmdWrapper);

                using (IDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        obj = dataMapper.Map(rdr);
                    }
                }

                cmdWrapper.PopulateOutputParameters();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
            }
            return obj;
        }


        public List<T> ExecuteList<T>(string sql, IDataMapper<T> rowMapper,
            int index, int size)
        {
            if (string.IsNullOrEmpty(sql))
                throw new Exception("Sql is empty");

            if (rowMapper == null)
                throw new Exception("Mapper is null");

            List<T> list = new List<T>();
            IDataReader rdr = null;
            DataSet dataSet = null;
            DbCommand cmd = null;
            DbDataAdapter da = null;

            try
            {
                DbProviderFactory factory = DbProviderFactories.GetFactory(dataSource.Provider);
                da = factory.CreateDataAdapter();
                cmd = conn.CreateCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;

                dataSet = new DataSet();
                da.SelectCommand = cmd;
                da.Fill(dataSet, index, size, typeof(T).ToString());

                rdr = dataSet.Tables[typeof(T).ToString()].CreateDataReader();

                while (rdr.Read())
                {
                    list.Add(rowMapper.Map(rdr));
                }
            }
            catch (Exception ex)
            {
                  throw ex;
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (da != null) da.Dispose();
                if (dataSet != null) dataSet.Dispose();
                if (rdr != null) rdr.Close();
            }

            return list;
        }



        public List<T> ExecuteList<T>(string sql, IDataMapper<T> rowMapper,
            bool isTransaction, Transaction tx)
        {
            if (string.IsNullOrEmpty(sql))
                throw new Exception("Sql is empty");

            if (rowMapper == null)
                throw new Exception("Mapper is null");

            List<T> list = new List<T>();
            IDataReader rdr = null;
            DbCommand cmd = null;

            try
            {
                cmd = conn.CreateCommand();
                if (isTransaction)
                {
                    cmd.Transaction = tx.GetTransaction();
                }

                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    list.Add(rowMapper.Map(rdr));
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (rdr != null) rdr.Close();
            }
            return list;
        }



        public List<T> ExecuteList<T>(DbCommandWrapper cmdWrapper, bool isStoredProc,
              string sql, IDataMapper<T> dataMapper, bool isTransaction, Transaction tx)
        {

            if (cmdWrapper == null)
                throw new Exception("DbCommandWrapper is null");

            if (string.IsNullOrEmpty(sql))
                throw new Exception("Sql or stored procedure is empty");

            if (dataMapper == null)
                throw new Exception("Mapper is null");

            List<T> list = new List<T>();
            DbCommand cmd = null;
            IDataReader rdr = null;

            try
            {
                cmd = conn.CreateCommand();
                cmdWrapper.Command = cmd;

                if (isTransaction)
                {
                    cmd.Transaction = tx.GetTransaction();
                }
                if (isStoredProc)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    cmd.CommandType = CommandType.Text;
                }

                cmd.CommandText = sql;
                AddParameters(cmd, cmdWrapper);

                if (rdr != null) rdr.Close();

                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(dataMapper.Map(rdr));
                }
                cmdWrapper.PopulateOutputParameters();
            }
            catch (Exception ex)
            {
                 throw ex;
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (rdr != null) rdr.Close();
            }
            return list;
        }




        public List<T> ExecuteList<T>(DbCommandWrapper cmdWrapper, bool isStoredProc,
              string sql, IDataMapper<T> dataMapper, int index, int size)
        {
            if (cmdWrapper == null)
                throw new Exception("DbCommandWrapper is null");

            if (string.IsNullOrEmpty(sql))
                throw new Exception("Sql or stored procedure is empty");

            if (dataMapper == null)
                throw new Exception("Mapper is null");

            List<T> list = new List<T>();
            IDataReader rdr = null;
            DataSet dataSet = null;
            DbCommand cmd = null;
            DbDataAdapter da = null;

            try
            {
                DbProviderFactory factory = DbProviderFactories.GetFactory(dataSource.Provider);
                da = factory.CreateDataAdapter();
                cmd = conn.CreateCommand();
                cmdWrapper.Command = cmd;
                cmd.Connection = conn;

                if (isStoredProc)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    cmd.CommandType = CommandType.Text;
                }

                cmd.CommandText = sql;

                dataSet = new DataSet();
                da.SelectCommand = cmd;
                da.Fill(dataSet, index, size, typeof(T).ToString());

                AddParameters(cmd, cmdWrapper);

                rdr = dataSet.Tables[typeof(T).ToString()].CreateDataReader();

                while (rdr.Read())
                {
                    list.Add(dataMapper.Map(rdr));
                }
                cmdWrapper.PopulateOutputParameters();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (da != null) da.Dispose();
                if (dataSet != null) dataSet.Dispose();
                if (rdr != null) rdr.Close();
            }
            return list;
        }


    }

    static class DbParameterExtension
    {
        /// <summary>
        /// Fix parameter range. 
        /// 
        /// For example, SqlDatetime has smaller data range than System.DateTime
        /// So passing DateTime.MinValue to Sql Command  with parameter SqlDateTime 
        /// will cause value overflow.
        /// This method will convert the value from DateTime.MinValue to SqlDateTime.MinValue
        /// and other dataType appropriately.
        /// </summary>
        /// <param name="param"></param>
        public static void FixParameterRange(this DbParameter param)
        {
            if (param.DbType == DbType.DateTime && param.Value is System.DateTime)
            {
                DateTime value = (DateTime)param.Value;
                if (DateTime.MinValue.Equals(value))
                {
                    param.Value = System.Data.SqlTypes.SqlDateTime.MinValue;
                }
                else if (DateTime.MaxValue.Equals(value))
                {
                    param.Value = System.Data.SqlTypes.SqlDateTime.MaxValue;
                }
            }
            //add other DbType here
        }
    }

}
