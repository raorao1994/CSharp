using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Collections.Generic;

namespace PublicLibrary
{
    /// <summary>
    /// DataAccessOledb
    /// 功能：用于操作excel、foxpro、access文件数据库的类。
    /// 作者：raoaro
    /// 时间：2018年2月8日
    /// 注意：在使用前需要调用Init方法进行初始化。
    /// </summary>
    public static class DataAccessOledb
    {
        //私有静态字段
        private static string _ConnectionString = "";       //连接字符串
        private static bool _IsInit = false;                //是否已经初始化
        private static OledbDatabaseType _DatabaseType;     //oledb数据库类型
        private static string _DbPathName = "";             //数据库文件名（包含路径）

        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                return _ConnectionString;
            }
        }

        /// <summary>
        /// 是否已经初始化
        /// </summary>
        public static bool IsInit
        {
            get
            {
                return _IsInit;
            }
        }

        /// <summary>
        /// oledb数据库类型
        /// </summary>
        public static OledbDatabaseType DatabaseType
        {
            get
            {
                return _DatabaseType;
            }
        }

        /// <summary>
        /// 数据库文件名（含路径）
        /// </summary>
        public static string DbPathName
        {
            get
            {
                return _DbPathName;
            }
        }

        /// <summary>
        /// 初始化oledb数据库连接信息
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="pathName">数据库文件名（含路径）</param>
        /// <returns>返回初始化是否成功</returns>
        public static bool Init(OledbDatabaseType dbType, string pathName)
        {
            if (File.Exists(pathName))
            {
                _DatabaseType = dbType;
                _DbPathName = pathName;
                if (dbType == OledbDatabaseType.Foxpro)
                {
                    _ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Path.GetDirectoryName(pathName) + ";Extended Properties=dBASE IV;User ID=Admin;Password=;";
                }
                else if (dbType == OledbDatabaseType.Excel)
                {
                    _ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathName + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1;\"";
                }
                else if (dbType == OledbDatabaseType.Access)
                {
                    _ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathName + ";User Id=admin;Password=;";
                }
                _IsInit = true;
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 清除数据库连接信息
        /// </summary>
        public static void Uninit()
        {
            _ConnectionString = "";
            _IsInit = false;
            _DbPathName = "";
        }

        /// <summary>
        /// 获取SELECT语句返回的数据表
        /// </summary>
        /// <param name="selectCommandText">SELECT语句</param>
        /// <param name="dt">返回得到的数据表</param>
        /// <param name="result">返回操作的结果（输出参数）</param>
        /// <returns>返回操作是否成功，如果成功，返回true；否则返回false。</returns>
        public static bool GetDataTable(string selectCommandText, DataTable dt, out string result)
        {
            bool bSuccessed = false;
            result = "";
            if (selectCommandText == "")
            {
                bSuccessed = false;
                result = "参数错误：SELECT语句不能为空。";
            }
            else
            {
                if (dt == null)
                {
                    bSuccessed = false;
                    result = "参数错误：dt不能为空。";
                }
                else
                {
                    OleDbConnection conn = new OleDbConnection(_ConnectionString);
                    OleDbDataAdapter da = new OleDbDataAdapter(selectCommandText, conn);
                    try
                    {
                        conn.Open();
                        da.SelectCommand.CommandType = CommandType.Text;
                        da.Fill(dt);
                        bSuccessed = true;
                        result = "已经成功的获取了数据表。";
                    }
                    catch (Exception e)
                    {
                        bSuccessed = false;
                        result = "在访问数据时发生错误。\r\nSELECT语句：" + selectCommandText + "\r\n错误描述：" + e.Message + "\r\n错误源：" + e.Source;
                    }
                    finally
                    {
                        if (conn != null)
                        {
                            if (conn.State != ConnectionState.Closed)
                                conn.Close();
                        }
                    }
                }
            }
            return bSuccessed;
        }

        /// <summary>
        /// 获取SELECT语句返回的数据表
        /// </summary>
        /// <param name="selectCommandText">SELECT语句</param>
        /// <param name="dt">返回得到的数据表</param>
        /// <param name="parameters">跟该SQL语句相对应的参数数组</param>
        /// <param name="result">返回操作的结果（输出参数）</param>
        /// <returns>返回操作是否成功，如果成功，返回true；否则返回false。</returns>
        public static bool GetDataTable(string selectCommandText, DataTable dt, OleDbParameter[] parameters, out string result)
        {
            bool bSuccessed = false;
            result = "";
            if (selectCommandText == "")
            {
                bSuccessed = false;
                result = "参数错误：SELECT语句不能为空。";
            }
            else
            {
                if (dt == null)
                {
                    bSuccessed = false;
                    result = "参数错误：dt不能为空。";
                }
                else
                {
                    OleDbConnection conn = new OleDbConnection(_ConnectionString);
                    OleDbDataAdapter da = new OleDbDataAdapter(selectCommandText, conn);
                    try
                    {
                        conn.Open();
                        da.SelectCommand.CommandType = CommandType.Text;
                        if (parameters != null && parameters.Length > 0)
                            da.SelectCommand.Parameters.AddRange(parameters);
                        da.Fill(dt);
                        bSuccessed = true;
                        result = "已经成功的获取了数据表。";
                    }
                    catch (Exception e)
                    {
                        bSuccessed = false;
                        result = "在访问数据时发生错误。\r\nSELECT语句：" + selectCommandText + "\r\n错误描述：" + e.Message + "\r\n错误源：" + e.Source;
                    }
                    finally
                    {
                        if (conn != null)
                        {
                            if (conn.State != ConnectionState.Closed)
                                conn.Close();
                        }
                    }
                }
            }
            return bSuccessed;
        }

        /// <summary>
        /// 获取SELECT语句返回的数据表
        /// </summary>
        /// <param name="selectCommandText">SELECT语句</param>
        /// <param name="dt">返回得到的数据表</param>
        /// <param name="parameters">跟该SQL语句相对应的参数列表</param>
        /// <param name="result">返回操作的结果（输出参数）</param>
        /// <returns>返回操作是否成功，如果成功，返回true；否则返回false。</returns>
        public static bool GetDataTable(string selectCommandText, DataTable dt, List<OleDbParameter> parameters, out string result)
        {
            return GetDataTable(selectCommandText, dt, parameters.ToArray(), out result);
        }

        /// <summary>
        /// 获取数据库中的某一个值（该方法只返回结果集中的第一个数据）
        /// </summary>
        /// <param name="selectCommandText">SELECT语句</param>
        /// <param name="objValue">返回的数据</param>
        /// <param name="result">返回操作的结果</param>
        /// <returns>返回操作是否成功，如果成功，返回true；否则返回false。</returns>
        public static bool GetOneValue(string selectCommandText, out object objValue, out string result)
        {
            bool bSuccessed = false;
            objValue = null;
            result = "";
            if (selectCommandText == "")
            {
                bSuccessed = false;
                result = "参数错误：SELECT语句不能为空。";
            }
            else
            {
                OleDbConnection conn = new OleDbConnection(_ConnectionString);
                OleDbCommand cmd = new OleDbCommand(selectCommandText, conn);
                try
                {
                    conn.Open();
                    cmd.CommandType = CommandType.Text;
                    objValue = cmd.ExecuteScalar();
                    bSuccessed = true;
                    result = "已经成功的获取了数据。";
                }
                catch (Exception e)
                {
                    bSuccessed = false;
                    result = "在访问数据时发生错误。\r\nSELECT语句：" + selectCommandText + "\r\n错误描述：" + e.Message + "\r\n错误源：" + e.Source;
                }
                finally
                {
                    if (conn != null)
                    {
                        if (conn.State != ConnectionState.Closed)
                            conn.Close();
                    }
                }
            }
            return bSuccessed;
        }

        /// <summary>
        /// 获取数据库中的某一个值（该方法只返回结果集中的第一个数据）
        /// </summary>
        /// <param name="selectCommandText">SELECT语句</param>
        /// <param name="parameters">命令参数数组</param>
        /// <param name="objValue">返回的数据</param>
        /// <param name="result">返回操作的结果</param>
        /// <returns>返回操作是否成功，如果成功，返回true；否则返回false。</returns>
        public static bool GetOneValue(string selectCommandText, OleDbParameter[] parameters, out object objValue, out string result)
        {
            bool bSuccessed = false;
            objValue = null;
            result = "";
            if (selectCommandText == "")
            {
                bSuccessed = false;
                result = "参数错误：SELECT语句不能为空。";
            }
            else
            {
                OleDbConnection conn = new OleDbConnection(_ConnectionString);
                OleDbCommand cmd = new OleDbCommand(selectCommandText, conn);
                try
                {
                    conn.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddRange(parameters);
                    objValue = cmd.ExecuteScalar();
                    bSuccessed = true;
                    result = "已经成功的获取了数据。";
                }
                catch (Exception e)
                {
                    bSuccessed = false;
                    result = "在访问数据时发生错误。\r\nSELECT语句：" + selectCommandText + "\r\n错误描述：" + e.Message + "\r\n错误源：" + e.Source;
                }
                finally
                {
                    if (conn != null)
                    {
                        if (conn.State != ConnectionState.Closed)
                            conn.Close();
                    }
                }
            }
            return bSuccessed;
        }

        /// <summary>
        /// 获取数据库中的某一个值（该方法只返回结果集中的第一个数据）
        /// </summary>
        /// <param name="selectCommandText">SELECT语句</param>
        /// <param name="parameters">命令参数列表</param>
        /// <param name="objValue">返回的数据</param>
        /// <param name="result">返回操作的结果</param>
        /// <returns>返回操作是否成功，如果成功，返回true；否则返回false。</returns>
        public static bool GetOneValue(string selectCommandText, List<OleDbParameter> parameters, out object objValue, out string result)
        {
            return GetOneValue(selectCommandText, parameters.ToArray(), out objValue, out result);
        }

        /// <summary>
        /// 执行SQL语句，并返回受影响的行数。
        /// 注意：这里常见的SQL语句包括INSERT、UPDATE、DELETE等。
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <param name="result">返回操作的结果</param>
        /// <returns>返回操作所影响的行数，如果成功，行数大于0；如果失败，行数为0。</returns>
        public static int ExecuteCommand(string commandText, out string result)
        {
            int recordsAffected = 0;    //受影响的行数
            result = "";
            if (commandText == "")
            {
                result = "参数错误：SQL语句不能为空。";
            }
            else
            {
                OleDbConnection conn = new OleDbConnection(_ConnectionString);
                OleDbCommand cmd = new OleDbCommand(commandText, conn);
                try
                {
                    conn.Open();
                    cmd.CommandType = CommandType.Text;
                    recordsAffected = cmd.ExecuteNonQuery();
                    if (recordsAffected > 0)
                        result = "已经成功的影响了" + recordsAffected + "条记录。";
                }
                catch (Exception e)
                {
                    result = "在执行数据操作时发生错误。\r\nSQL语句：" + commandText + "\r\n错误描述：" + e.Message + "\r\n错误源：" + e.Source;
                }
                finally
                {
                    if (conn != null)
                    {
                        if (conn.State != ConnectionState.Closed)
                            conn.Close();
                    }
                }
            }
            return recordsAffected;
        }

        /// <summary>
        /// 执行SQL语句，并返回受影响的行数。
        /// 注意：这里常见的SQL语句包括INSERT、UPDATE、DELETE等。
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <param name="parameters">参数列表</param>
        /// <param name="result">返回操作的结果</param>
        /// <returns>返回操作所影响的行数，如果成功，行数大于0；如果失败，行数为0。</returns>
        public static int ExecuteCommand(string commandText, List<OleDbParameter> parameters, out string result)
        {
            int recordsAffected = 0;    //受影响的行数
            result = "";
            if (commandText == "")
            {
                result = "参数错误：SQL语句不能为空。";
            }
            else
            {
                OleDbConnection conn = new OleDbConnection(_ConnectionString);
                OleDbCommand cmd = new OleDbCommand(commandText, conn);
                try
                {
                    conn.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddRange(parameters.ToArray());
                    recordsAffected = cmd.ExecuteNonQuery();
                    if (recordsAffected > 0)
                        result = "已经成功的影响了" + recordsAffected + "条记录。";
                }
                catch (Exception e)
                {
                    result = "在执行数据操作时发生错误。\r\nSQL语句：" + commandText + "\r\n错误描述：" + e.Message + "\r\n错误源：" + e.Source;
                }
                finally
                {
                    if (conn != null)
                    {
                        if (conn.State != ConnectionState.Closed)
                            conn.Close();
                    }
                }
            }
            return recordsAffected;
        }

        /// <summary>
        /// 执行SQL语句，并返回受影响的行数。
        /// 注意：这里常见的SQL语句包括INSERT、UPDATE、DELETE等。
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <param name="parameters">参数数组</param>
        /// <param name="result">返回操作的结果</param>
        /// <returns>返回操作所影响的行数，如果成功，行数大于0；如果失败，行数为0。</returns>
        public static int ExecuteCommand(string commandText, OleDbParameter[] parameters, out string result)
        {
            return ExecuteCommand(commandText, new List<OleDbParameter>(parameters), out result);
        }

        /// <summary>
        /// 执行SQL语句，并返回受影响的行数。
        /// 注意：这里常见的SQL语句包括INSERT、UPDATE、DELETE和存储过程。
        /// </summary>
        /// <param name="commandText">SQL语句或者存储过程名</param>
        /// <param name="parameters">参数列表</param>
        /// <param name="result">返回操作的结果</param>
        /// <returns>返回操作所影响的行数，如果成功，行数大于0；如果失败，行数为0。</returns>
        public static int ExecuteCommand(string commandText, List<OleDbParameter> parameters, CommandType commandType, out string result)
        {
            int recordsAffected = 0;    //受影响的行数
            result = "";
            if (commandText == "")
            {
                result = "参数错误：SQL语句不能为空。";
            }
            else
            {
                OleDbConnection conn = new OleDbConnection(_ConnectionString);
                OleDbCommand cmd = new OleDbCommand(commandText, conn);
                try
                {
                    conn.Open();
                    cmd.CommandType = commandType;
                    cmd.Parameters.AddRange(parameters.ToArray());
                    recordsAffected = cmd.ExecuteNonQuery();
                    if (recordsAffected > 0)
                        result = "已经成功的影响了" + recordsAffected + "条记录。";
                }
                catch (Exception e)
                {
                    result = "在执行数据操作时发生错误。\r\nSQL语句：" + commandText + "\r\n错误描述：" + e.Message + "\r\n错误源：" + e.Source;
                }
                finally
                {
                    if (conn != null)
                    {
                        if (conn.State != ConnectionState.Closed)
                            conn.Close();
                    }
                }
            }
            return recordsAffected;
        }

        /// <summary>
        /// 在事务内处理一组命令
        /// </summary>
        /// <param name="commands">命令列表</param>
        /// <param name="result">输出参数，如果失败，返回错误提示</param>
        /// <returns>返回是否执行成功</returns>
        public static bool ExecuteBatchCommands(List<DataAccessCommandOleDb> commands, out string result)
        {
            bool successed = false;
            result = "";
            OleDbConnection conn = new OleDbConnection(_ConnectionString);
            OleDbTransaction trans = null;
            OleDbCommand cmd = new OleDbCommand("", conn);
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                cmd.Transaction = trans;
                int executedCommands = 0;
                foreach (DataAccessCommandOleDb command in commands)
                {
                    cmd.CommandType = command.CommandType;
                    cmd.CommandText = command.CommandText;
                    cmd.Parameters.Clear();
                    if (command.ParamList != null && command.ParamList.Count > 0)
                        cmd.Parameters.AddRange(command.ParamList.ToArray());
                    int recordsAffected = cmd.ExecuteNonQuery();
                    if (command.RecordsAffectedLargeThanZeroIsSuccessful)
                    {
                        //命令影响的行数大于0才算成功
                        if (recordsAffected > 0)
                            executedCommands++;
                        else
                            break;
                    }
                    else
                        executedCommands++;
                }
                if (executedCommands == commands.Count)
                {
                    trans.Commit();
                    successed = true;
                }
                else
                    trans.Rollback();
            }
            catch (Exception e)
            {
                result = "在执行数据操作时发生错误。\r\n错误描述：" + e.Message + "\r\n错误源：" + e.Source;
                try
                {
                    trans.Rollback();
                }
                catch
                {
                }
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
            return successed;
        }

        /// <summary>
        /// 得到oledb库中的表名列表
        /// </summary>
        /// <returns>返回表名列表；如果获取失败，返回null。</returns>
        public static List<string> GetTableNameList()
        {
            List<string> tableNameList = null;
            OleDbConnection conn = new OleDbConnection(_ConnectionString);
            try
            {
                conn.Open();
                DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                {
                    tableNameList = new List<string>(dt.Rows.Count);
                    foreach (DataRow dr in dt.Rows)
                        tableNameList.Add(dr["TABLE_NAME"].ToString());
                }
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
            return tableNameList;
        }

        /// <summary>
        /// 获取数据库中所有表的内容，然后组合到数据集中
        /// </summary>
        /// <returns>返回数据集</returns>
        public static DataSet GetDataSet()
        {
            DataSet ds = new DataSet();
            //将不包含扩展名的文件名，作为数据集名
            string dsName = Path.GetFileNameWithoutExtension(_DbPathName);
            ds.DataSetName = dsName;
            //得到表名列表
            OleDbConnection conn = new OleDbConnection(_ConnectionString);
            try
            {
                conn.Open();
                DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                {
                    OleDbDataAdapter da = new OleDbDataAdapter();
                    da.SelectCommand = new OleDbCommand();
                    da.SelectCommand.Connection = conn;
                    da.SelectCommand.CommandType = CommandType.Text;
                    //依次将每个表添加到数据集
                    foreach (DataRow dr in dt.Rows)
                    {
                        string tableName = dr["TABLE_NAME"].ToString();
                        da.SelectCommand.CommandText = "SELECT * FROM [" + tableName + "]";
                        DataTable dt2 = new DataTable(tableName);
                        da.Fill(dt2);
                        ds.Tables.Add(dt2);
                    }
                }
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
            return ds;
        }
    }

    /// <summary>
    /// Oledb数据库类型
    /// </summary>
    public enum OledbDatabaseType
    {
        Excel,      //电子表格
        Foxpro,     //foxpro
        Access      //access
    }

    /// <summary>
    /// DataAccessCommandOleDb命令：包括命令语句和命令所用的参数列表。
    /// </summary>
    public class DataAccessCommandOleDb
    {
        //公共字段
        public string CommandText;							//命令语句
        public List<OleDbParameter> ParamList;				//参数列表
        public CommandType CommandType;						//命令类型
        public bool RecordsAffectedLargeThanZeroIsSuccessful;	//执行命令所影响的记录数大于0才成功吗？

        //构造函数
        public DataAccessCommandOleDb(string commandText, List<OleDbParameter> paramList, CommandType commandType, bool recordsAffectedLargeThanZeroIsSuccessful)
        {
            CommandText = commandText;
            ParamList = paramList;
            this.CommandType = commandType;
            RecordsAffectedLargeThanZeroIsSuccessful = recordsAffectedLargeThanZeroIsSuccessful;
        }

        public DataAccessCommandOleDb(string commandText, List<OleDbParameter> paramList)
            : this(commandText, paramList, CommandType.Text, true)
        {
        }

        public DataAccessCommandOleDb(string commandText)
            : this(commandText, null)
        {
        }
    }
}