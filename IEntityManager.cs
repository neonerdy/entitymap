
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
    public interface IEntityManager : IDisposable
    {
        #region Data Access Helper

        DbConnection Connection
        {
            get;
        }

        IDataReader ExecuteReader(string sql);
        IDataReader ExecuteReader(DbCommandWrapper dbCommandWrapper);

        int ExecuteNonQuery(string sql);
        int ExecuteNonQuery(string sql, Transaction tx);
        int ExecuteNonQuery(DbCommandWrapper dbCommandWrapper);
        int ExecuteNonQuery(DbCommandWrapper dbCommandWrapper, Transaction tx);

        DataSet ExecuteDataSet(string sql);
        DataSet ExecuteDataSet(DbCommandWrapper dbCommandWrapper);

        object ExecuteScalar(string sql);
        object ExecuteScalar(string sql, Transaction tx);
        object ExecuteScalar(DbCommandWrapper dbCommandWrapper);
        object ExecuteScalar(DbCommandWrapper dbCommandWrapper, Transaction tx);

        T ExecuteObject<T>(string sql, IDataMapper<T> rowMapper);
        T ExecuteObject<T>(string sql, IDataMapper<T> dataMapper, Transaction tx);
        T ExecuteObject<T>(DbCommandWrapper dbCommandWrapper, IDataMapper<T> dataMapper);
        T ExecuteObject<T>(DbCommandWrapper dbCommandWrapper, IDataMapper<T> dataMapper,
            Transaction tx);

        List<T> ExecuteList<T>(string sql, IDataMapper<T> rowMapper);
        List<T> ExecuteList<T>(string sql, IDataMapper<T> rowMapper,
            int index, int size);
        List<T> ExecuteList<T>(string sql, IDataMapper<T> dataMapper, Transaction tx);
        List<T> ExecuteList<T>(DbCommandWrapper dbCommandWrapper, IDataMapper<T> dataMapper);

        List<T> ExecuteList<T>(DbCommandWrapper dbCommandWrapper, IDataMapper<T> dataMapper,
            Transaction tx);
        List<T> ExecuteList<T>(DbCommandWrapper dbCommandWrapper, IDataMapper<T> dataMapper,
            int index, int size);

        DbCommandWrapper CreateCommand(CommandWrapperType cmdWrapperType, string query);
        Transaction BeginTransaction();
        DbCommand CreateCommand();

        #endregion
    }
}
