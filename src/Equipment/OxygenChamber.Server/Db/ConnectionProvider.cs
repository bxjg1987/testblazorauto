using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OxygenChamber.Server.Db
{
    /// <summary>
    /// 提供全局数据库连接
    /// 启动时以单例注册
    /// </summary>
    public class ConnectionProvider
    {
        private string strConn;
        private object locker = new object();
        private SqlConnection dbConnection;

        //IDbConnection、IDbCommand没有异步方法
        public DbConnection GetConnection()
        {
            if (dbConnection != null && dbConnection.State == ConnectionState.Open)
                return dbConnection;

            lock (locker)
            {
                if (dbConnection != null && dbConnection.State == ConnectionState.Open)
                    return dbConnection;

                dbConnection = new SqlConnection(strConn);
                dbConnection.Open();
                return dbConnection;
            }
        }

        //public SqlConnection Connection { get; private set; }

        public ConnectionProvider(IConfiguration configuration)
        {
            strConn = configuration.GetConnectionString("Default");
        }
    }
}
