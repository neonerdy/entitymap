
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
using System.Data.Common;
using System.Data;
using System.Configuration;

namespace EntityMap
{
    public class EntityManager : IEntityManager, IDisposable
    {
        private DbConnection conn;
        private DataSource dataSource;
        private string sql;
        private string storedProcName;
      
        #region Constructors


        public EntityManager(DataSource dataSource)
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


        public DbConnection Connection
        {
            get { return conn; }
        }


        #endregion

        #region Data Access Helper


        public IDataReader ExecuteReader(string sql)
        {
            try
            {
                DbHelper dbHelper = new DbHelper(conn);
                return dbHelper.ExecuteReader(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public IDataReader ExecuteReader(DbCommandWrapper dbCommandWrapper)
        {
            IDataReader rdr = null;
            try
            {
                DbHelper dbHelper = new DbHelper(conn);

                if (sql.Equals(""))
                {
                    rdr = dbHelper.ExecuteReader(dbCommandWrapper, true,
                        storedProcName, false, null);
                 }
                else
                {
                    rdr = dbHelper.ExecuteReader(dbCommandWrapper, false,
                        sql, false, null);
                 }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rdr;
        }



        public int ExecuteNonQuery(string sql)
        {
            int result = 0;
            try
            {
                DbHelper dbHelper = new DbHelper(conn);
                result = dbHelper.ExecuteNonQuery(sql, false, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


        public int ExecuteNonQuery(string sql, Transaction tx)
        {
            DbHelper dbHelper = null;
            int result = 0;

            try
            {
                if (tx == null)
                {
                    dbHelper = new DbHelper(conn);
                }
                else
                {
                    dbHelper = new DbHelper(tx.GetConnection());
                }

                result = dbHelper.ExecuteNonQuery(sql, true, tx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }



        public int ExecuteNonQuery(DbCommandWrapper dbCommandWrapper)
        {
            int result = 0;
            try
            {
                DbHelper dbHelper = new DbHelper(conn);

                if (sql.Equals(""))
                {
                    result = dbHelper.ExecuteNonQuery(dbCommandWrapper, true,
                        storedProcName, false, null);
                }
                else
                {
                    result = dbHelper.ExecuteNonQuery(dbCommandWrapper, false,
                        sql, false, null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }



        public int ExecuteNonQuery(DbCommandWrapper dbCommandWrapper, Transaction tx)
        {
            int result = 0;
            DbHelper dbHelper = null;

            try
            {
                if (tx == null)
                {
                    throw new ArgumentNullException("tx");
                }
                else
                {
                    dbHelper = new DbHelper(tx.GetConnection());
                }

                if (sql.Equals(""))
                {
                    result = dbHelper.ExecuteNonQuery(dbCommandWrapper, true, storedProcName, true, tx);
                }
                else
                {
                    result = dbHelper.ExecuteNonQuery(dbCommandWrapper, false, sql, true, tx);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


        public DataSet ExecuteDataSet(string sql)
        {
            DataSet dataSet = null;

            try
            {
                DbHelper dbHelper = new DbHelper(conn);
                dataSet = dbHelper.ExecuteDataSet(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dataSet;
        }


        public DataSet ExecuteDataSet(DbCommandWrapper dbCommandWrapper)
        {
            DataSet dataSet = null;
            try
            {
                DbHelper dbHelper = new DbHelper(conn);

                if (sql.Equals(""))
                {
                    dataSet = dbHelper.ExecuteDataSet(dbCommandWrapper, true, storedProcName);
                }
                else
                {
                    dataSet = dbHelper.ExecuteDataSet(dbCommandWrapper, false, sql);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dataSet;
        }


        public object ExecuteScalar(string sql)
        {
            object result = null;
            try
            {
                DbHelper dbHelper = new DbHelper(conn);
                result = dbHelper.ExecuteScalar(sql, false, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


        public object ExecuteScalar(string sql, Transaction tx)
        {
            object result = null;
            try
            {
                DbHelper dbHelper = new DbHelper(conn);
                result = dbHelper.ExecuteScalar(sql, true, tx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


        public object ExecuteScalar(DbCommandWrapper dbCommandWrapper)
        {
            object result = null;
            try
            {
                DbHelper dbHelper = new DbHelper(conn);
                if (sql.Equals(""))
                {
                    result = dbHelper.ExecuteScalar(dbCommandWrapper, true, storedProcName, false, null);
                }
                else
                {
                    result = dbHelper.ExecuteScalar(dbCommandWrapper, false, sql, false, null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


        public object ExecuteScalar(DbCommandWrapper dbCommandWrapper, Transaction tx)
        {
            object result = null;
            try
            {
                DbHelper dbHelper = new DbHelper(conn);

                if (sql.Equals(""))
                {
                    result = dbHelper.ExecuteScalar(dbCommandWrapper, true, storedProcName, true, tx);
                }
                else
                {
                    result = dbHelper.ExecuteScalar(dbCommandWrapper, false, sql, true, tx);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


        public T ExecuteObject<T>(string sql, IDataMapper<T> dataMapper)
        {
            T obj = default(T);
            try
            {
                DbHelper dbHelper = new DbHelper(conn);
                obj = dbHelper.ExecuteObject(sql, dataMapper, false, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return obj;
        }




        public T ExecuteObject<T>(string sql, IDataMapper<T> dataMapper, Transaction tx)
        {
            T obj = default(T);
            try
            {
                DbHelper dbHelper = new DbHelper(conn);
                obj = dbHelper.ExecuteObject(sql, dataMapper, true, tx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return obj;
        }


        public T ExecuteObject<T>(DbCommandWrapper dbCommandWrapper,
           IDataMapper<T> dataMapper)
        {
            T obj = default(T);

            try
            {
                DbHelper dbHelper = new DbHelper(conn);
                if (sql.Equals(""))
                {
                    obj = dbHelper.ExecuteObject<T>(dbCommandWrapper, true,
                        storedProcName, dataMapper, false, null);
                }
                else
                {
                    obj = dbHelper.ExecuteObject<T>(dbCommandWrapper, false,
                        sql, dataMapper, false, null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return obj;
        }



        public T ExecuteObject<T>(DbCommandWrapper dbCommandWrapper,
          IDataMapper<T> dataMapper, Transaction tx)
        {
            T obj = default(T);
            try
            {
                DbHelper dbHelper = new DbHelper(conn);
                if (sql.Equals(""))
                {
                    obj = dbHelper.ExecuteObject<T>(dbCommandWrapper, true,
                        storedProcName, dataMapper, true, tx);
                }
                else
                {
                    obj = dbHelper.ExecuteObject<T>(dbCommandWrapper, false,
                        sql, dataMapper, true, tx);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return obj;
        }



        public List<T> ExecuteList<T>(string sql, IDataMapper<T> dataMapper)
        {
            List<T> list = null;
            try
            {
                DbHelper dbHelper = new DbHelper(conn);
                list = dbHelper.ExecuteList<T>(sql, dataMapper, false, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }


        public List<T> ExecuteList<T>(string sql, IDataMapper<T> dataMapper, Transaction tx)
        {
            List<T> list = null;
            try
            {
                DbHelper dbHelper = new DbHelper(conn);
                list = dbHelper.ExecuteList<T>(sql, dataMapper, true, tx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }



        public List<T> ExecuteList<T>(string sql, IDataMapper<T> dataMapper,
            int index, int size)
        {
            List<T> list = null;

            try
            {
                DbHelper dbHelper = new DbHelper(dataSource);
                list = dbHelper.ExecuteList<T>(sql, dataMapper, index, size);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }



        public List<T> ExecuteList<T>(DbCommandWrapper dbCommandWrapper,
              IDataMapper<T> dataMapper)
        {
            List<T> list = null;

            try
            {
                DbHelper dbHelper = new DbHelper(conn);
                if (sql.Equals(""))
                {
                    list = dbHelper.ExecuteList<T>(dbCommandWrapper, true,
                        storedProcName, dataMapper, false, null);
                }
                else
                {
                    list = dbHelper.ExecuteList<T>(dbCommandWrapper, false,
                        sql, dataMapper, false, null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }




        public List<T> ExecuteList<T>(DbCommandWrapper dbCommandWrapper,
            IDataMapper<T> dataMapper, Transaction tx)
        {
            List<T> list = null;
            try
            {
                DbHelper dbHelper = new DbHelper(conn);
                if (sql.Equals(""))
                {
                    list = dbHelper.ExecuteList<T>(dbCommandWrapper, true,
                        storedProcName, dataMapper, true, tx);
                }
                else
                {
                    list = dbHelper.ExecuteList<T>(dbCommandWrapper, false,
                        sql, dataMapper, true, tx);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }



        public List<T> ExecuteList<T>(DbCommandWrapper dbCommandWrapper,
              IDataMapper<T> dataMapper, int index, int size)
        {
            List<T> list = null;

            try
            {
                DbHelper dbHelper = new DbHelper(dataSource);
                if (sql.Equals(""))
                {
                    list = dbHelper.ExecuteList<T>(dbCommandWrapper, true,
                        storedProcName, dataMapper, index, size);
                }
                else
                {
                    list = dbHelper.ExecuteList<T>(dbCommandWrapper, false,
                        sql, dataMapper, index, size);
               }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }



        public DbCommandWrapper CreateCommand(CommandWrapperType cmdWrapperType, string query)
        {
            if (string.IsNullOrEmpty(query))
                throw new Exception("Sql or stored procedure is empty");

            DbCommandWrapper dbCommandWrapper = new DbCommandWrapper();
            switch (cmdWrapperType)
            {
                case CommandWrapperType.Text:
                    this.sql = query;
                    this.storedProcName = "";
                    break;

                case CommandWrapperType.StoredProcedure:
                    this.storedProcName = query;
                    this.sql = "";
                    break;
                default:
                    throw new Exception("ComandWrapperType is null");
            }

            return dbCommandWrapper;

        }


        public DbCommand CreateCommand()
        {
            return conn.CreateCommand();
        }

        public Transaction BeginTransaction()
        {
            if (conn == null || conn.State == ConnectionState.Closed)
            {
                conn = ConnectionFactory.CreateConnection(dataSource);
            }

            if (conn == null)
                throw new Exception("Connection is null");

            return new Transaction(conn);
        }


        #endregion


        #region IDisposable Members

        public void Dispose()
        {
            if (conn != null)
            {
                conn.Dispose();
            }

        }

        #endregion
    }
}
