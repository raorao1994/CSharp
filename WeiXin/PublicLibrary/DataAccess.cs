using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;

namespace PublicLibrary
{
    /// <summary>
    /// DataAccess
    /// 功能：SQL SERVER数据库访问类。
    /// 作者：raoaro
    /// 时间：2018年2月8日
    /// 注意：在使用前，需要设置connectionString。
    /// </summary>
    public static class DataAccess
    {
        //静态字段
        /// <summary>
        /// 将NText字段的尺寸设定为100M
        /// </summary>
        public static int NTextSize = 100000000;
        /// <summary>
        /// 默认的SQL SERVER数据库连接字符串，在使用前需要设定
        /// </summary>
        public static string connectionString;
        /// <summary>
        /// 默认BulkCopy操作的操时时间设定为60秒
        /// </summary>
        public static int BulkCopyTimeout = 60;

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
                    SqlConnection conn = new SqlConnection(connectionString);
                    SqlDataAdapter da = new SqlDataAdapter(selectCommandText, conn);
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
        /// 从指定的服务器获取SELECT语句返回的数据表
        /// </summary>
        /// <param name="strConn">数据库连接字符串</param>
        /// <param name="selectCommandText">SELECT语句</param>
        /// <param name="dt">返回得到的数据表</param>
        /// <param name="parameters">跟该SQL语句相对应的参数数组</param>
        /// <param name="result">返回操作的结果（输出参数）</param>
        /// <returns>返回操作是否成功，如果成功，返回true；否则返回false。</returns>
        public static bool GetDataTable(string strConn, string selectCommandText, DataTable dt, SqlParameter[] parameters, out string result)
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
                    SqlConnection conn = new SqlConnection(strConn);
                    SqlDataAdapter da = new SqlDataAdapter(selectCommandText, conn);
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
                        if (da != null && da.SelectCommand != null && da.SelectCommand.Parameters.Count > 0)
                            da.SelectCommand.Parameters.Clear();
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
        /// 从指定的多个服务器获取SELECT语句返回的数据表，并合并数据表。
        /// 注意：在执行过程中忽略单台服务器的错误；该方法可能会将dt置为null。
        /// </summary>
        /// <param name="connectionStringList">数据库连接字符串</param>
        /// <param name="selectCommandText">SELECT语句</param>
        /// <param name="dt">返回得到的数据表</param>
        /// <param name="parameters">跟该SQL语句相对应的参数数组</param>
        /// <param name="result">返回操作的结果（输出参数）</param>
        /// <returns>返回成功获取到数据的服务器数目。</returns>
        public static int GetDataTable(List<string> connectionStringList, string selectCommandText, ref DataTable dt, List<SqlParameter> parameters, out string result)
        {
            int successedServerCount = 0;
            result = "";
            if (selectCommandText == "")
            {
                result = "参数错误：SELECT语句不能为空。";
            }
            else
            {
                if (connectionStringList == null || connectionStringList.Count == 0)
                {
                    result = "数据库连接字符串列表不能为空。";
                }
                else
                {
                    dt = null;
                    foreach (string connectionString in connectionStringList)
                    {
                        DataTable dtTemp = new DataTable();
                        if (GetDataTable(connectionString, selectCommandText, dtTemp, (parameters != null && parameters.Count > 0) ? parameters.ToArray() : null, out result))
                        {
                            if (dtTemp != null)
                            {
                                if (DataTableEx.Merge(ref dt, dtTemp))
                                    successedServerCount++;
                            }
                        }
                    }
                }
            }
            return successedServerCount;
        }

        /// <summary>
        /// 从指定的多个服务器获取SELECT语句返回的数据表，并汇总数据表。
        /// 注意：在执行过程中忽略单台服务器的错误；该方法可能会将dt置为null；除了关键列之外，要求其他列都是整型。
        /// </summary>
        /// <param name="connectionStringList">数据库连接字符串</param>
        /// <param name="selectCommandText">SELECT语句</param>
        /// <param name="dt">返回得到的数据表</param>
        /// <param name="parameters">跟该SQL语句相对应的参数数组</param>
        /// <param name="result">返回操作的结果（输出参数）</param>
        /// <param name="keyColumnName">关键列名</param>
        /// <returns>返回成功获取到数据的服务器数目。</returns>
        public static int GetDataTable(List<string> connectionStringList, string selectCommandText, ref DataTable dt, List<SqlParameter> parameters, out string result, string keyColumnName)
        {
            List<string> keyColumnNames = new List<string>(1);
            keyColumnNames.Add(keyColumnName);
            return GetDataTable(connectionStringList, selectCommandText, ref dt, parameters, out result, keyColumnNames);
        }

        /// <summary>
        /// 从指定的多个服务器获取SELECT语句返回的数据表，并汇总数据表。
        /// 注意：在执行过程中忽略单台服务器的错误；该方法可能会将dt置为null；除了关键列之外，要求其他列都是整型。
        /// </summary>
        /// <param name="connectionStringList">数据库连接字符串</param>
        /// <param name="selectCommandText">SELECT语句</param>
        /// <param name="dt">返回得到的数据表</param>
        /// <param name="parameters">跟该SQL语句相对应的参数数组</param>
        /// <param name="result">返回操作的结果（输出参数）</param>
        /// <param name="keyColumnNames">关键列名</param>
        /// <returns>返回成功获取到数据的服务器数目。</returns>
        public static int GetDataTable(List<string> connectionStringList, string selectCommandText, ref DataTable dt, List<SqlParameter> parameters, out string result, List<string> keyColumnNames)
        {
            int successedServerCount = 0;
            result = "";
            if (selectCommandText == "")
            {
                result = "参数错误：SELECT语句不能为空。";
            }
            else
            {
                if (connectionStringList == null || connectionStringList.Count == 0)
                {
                    result = "数据库连接字符串列表不能为空。";
                }
                else if (keyColumnNames == null || keyColumnNames.Count == 0)
                {
                    result = "关键列列表不能为空。";
                }
                else
                {
                    dt = null;
                    foreach (string connectionString in connectionStringList)
                    {
                        DataTable dtTemp = new DataTable();
                        if (GetDataTable(connectionString, selectCommandText, dtTemp, (parameters != null && parameters.Count > 0) ? parameters.ToArray() : null, out result))
                        {
                            if (dtTemp != null)
                            {
                                if (DataTableEx.Summarize(ref dt, dtTemp, keyColumnNames))
                                    successedServerCount++;
                            }
                        }
                    }
                }
            }
            return successedServerCount;
        }

        /// <summary>
        /// 获取SELECT语句返回的数据表
        /// </summary>
        /// <param name="selectCommandText">SELECT语句</param>
        /// <param name="dt">返回得到的数据表</param>
        /// <param name="parameters">跟该SQL语句相对应的参数数组</param>
        /// <param name="result">返回操作的结果（输出参数）</param>
        /// <returns>返回操作是否成功，如果成功，返回true；否则返回false。</returns>
        public static bool GetDataTable(string selectCommandText, DataTable dt, SqlParameter[] parameters, out string result)
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
                    SqlConnection conn = new SqlConnection(connectionString);
                    SqlDataAdapter da = new SqlDataAdapter(selectCommandText, conn);
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
                        if (da != null && da.SelectCommand != null && da.SelectCommand.Parameters.Count > 0)
                            da.SelectCommand.Parameters.Clear();
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
        public static bool GetDataTable(string selectCommandText, DataTable dt, List<SqlParameter> parameters, out string result)
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
                SqlConnection conn = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand(selectCommandText, conn);
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
        public static bool GetOneValue(string selectCommandText, SqlParameter[] parameters, out object objValue, out string result)
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
                SqlConnection conn = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand(selectCommandText, conn);
                try
                {
                    conn.Open();
                    cmd.CommandType = CommandType.Text;
                    if (parameters != null && parameters.Length > 0)
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
                    if (cmd != null && cmd.Parameters.Count > 0)
                        cmd.Parameters.Clear();
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
        /// <param name="strConn">数据库连接字符串</param>
        /// <param name="selectCommandText">SELECT语句</param>
        /// <param name="parameters">命令参数数组</param>
        /// <param name="objValue">返回的数据</param>
        /// <param name="result">返回操作的结果</param>
        /// <returns>返回操作是否成功，如果成功，返回true；否则返回false。</returns>
        public static bool GetOneValue(string strConn, string selectCommandText, SqlParameter[] parameters, out object objValue, out string result)
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
                SqlConnection conn = new SqlConnection(strConn);
                SqlCommand cmd = new SqlCommand(selectCommandText, conn);
                try
                {
                    conn.Open();
                    cmd.CommandType = CommandType.Text;
                    if (parameters != null && parameters.Length > 0)
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
                    if (cmd != null && cmd.Parameters.Count > 0)
                        cmd.Parameters.Clear();
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
        public static bool GetOneValue(string selectCommandText, List<SqlParameter> parameters, out object objValue, out string result)
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
                SqlConnection conn = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand(commandText, conn);
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
                    if (cmd != null && cmd.Parameters.Count > 0)
                        cmd.Parameters.Clear();
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
        public static int ExecuteCommand(string commandText, List<SqlParameter> parameters, out string result)
        {
            int recordsAffected = 0;    //受影响的行数
            result = "";
            if (commandText == "")
            {
                result = "参数错误：SQL语句不能为空。";
            }
            else
            {
                SqlConnection conn = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand(commandText, conn);
                try
                {
                    conn.Open();
                    cmd.CommandType = CommandType.Text;
                    if (parameters != null && parameters.Count > 0)
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
                    if (cmd != null && cmd.Parameters.Count > 0)
                        cmd.Parameters.Clear();
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
        public static int ExecuteCommand(string commandText, SqlParameter[] parameters, out string result)
        {
            return ExecuteCommand(commandText, new List<SqlParameter>(parameters), out result);
        }

        /// <summary>
        /// 执行SQL语句，并返回受影响的行数。
        /// 注意：这里常见的SQL语句包括INSERT、UPDATE、DELETE和存储过程。
        /// </summary>
        /// <param name="commandText">SQL语句或者存储过程名</param>
        /// <param name="parameters">参数列表</param>
        /// <param name="result">返回操作的结果</param>
        /// <returns>返回操作所影响的行数，如果成功，行数大于0；如果失败，行数为0。</returns>
        public static int ExecuteCommand(string commandText, List<SqlParameter> parameters, CommandType commandType, out string result)
        {
            int recordsAffected = 0;    //受影响的行数
            result = "";
            if (commandText == "")
            {
                result = "参数错误：SQL语句不能为空。";
            }
            else
            {
                SqlConnection conn = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand(commandText, conn);
                try
                {
                    conn.Open();
                    cmd.CommandType = commandType;
                    if (parameters != null && parameters.Count > 0)
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
                    if (cmd != null && cmd.Parameters.Count > 0)
                        cmd.Parameters.Clear();
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
        /// 注意：这里常见的SQL语句包括INSERT、UPDATE、DELETE和存储过程。
        /// </summary>
        /// <param name="strConn">数据库连接字符串</param>
        /// <param name="commandText">SQL语句或者存储过程名</param>
        /// <param name="parameters">参数列表</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="result">返回操作的结果</param>
        /// <returns>返回操作所影响的行数，如果成功，行数大于0；如果失败，行数为0。</returns>
        public static int ExecuteCommand(string strConn, string commandText, List<SqlParameter> parameters, CommandType commandType, out string result)
        {
            int recordsAffected = 0;    //受影响的行数
            result = "";
            if (commandText == "")
            {
                result = "参数错误：SQL语句不能为空。";
            }
            else
            {
                SqlConnection conn = new SqlConnection(strConn);
                SqlCommand cmd = new SqlCommand(commandText, conn);
                try
                {
                    conn.Open();
                    cmd.CommandType = commandType;
                    if (parameters != null && parameters.Count > 0)
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
                    if (cmd != null && cmd.Parameters.Count > 0)
                        cmd.Parameters.Clear();
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
        /// 注意：这里常见的SQL语句包括INSERT、UPDATE、DELETE和存储过程。
        /// </summary>
        /// <param name="connectionStringList">数据库连接字符串列表</param>
        /// <param name="dac">命令</param>
        /// <param name="needAllDone">是否需要所有服务器都执行成功才提交事务</param>
        /// <param name="result">返回操作的结果</param>
        /// <returns>返回在每个数据库上操作所影响的行数，如果成功，行数大于0；如果失败，行数为0。</returns>
        public static List<int> ExecuteCommand(List<string> connectionStringList, DataAccessCommand dac, bool needAllDone, out string result)
        {
            result = "";
            if (connectionStringList == null || connectionStringList.Count == 0)
            {
                result = "数据库连接字符串列表为空。";
                return null;
            }
            //定义变量
            int serverCount = connectionStringList.Count;
            List<SqlConnection> connList = new List<SqlConnection>(serverCount);
            List<SqlTransaction> transList = new List<SqlTransaction>(serverCount);
            List<SqlCommand> cmdList = new List<SqlCommand>(serverCount);
            List<int> recordsAffectedList = new List<int>(serverCount);
            bool transCommit = true;        //是否提交事务
            //分别在每台服务器上执行命令
            for (int i = 0; i < serverCount; i++)
            {
                connList.Add(null);
                transList.Add(null);
                cmdList.Add(null);
                recordsAffectedList.Add(0);
                try
                {
                    if (string.IsNullOrEmpty(connectionStringList[i]))
                    {
                        result = string.Format("第{0}个数据库连接字符串为空。", i);
                    }
                    else
                    {
                        connList[i] = new SqlConnection(connectionStringList[i]);
                        connList[i].Open();
                        transList[i] = connList[i].BeginTransaction();
                        cmdList[i] = new SqlCommand(dac.CommandText, connList[i], transList[i]);
                        cmdList[i].CommandType = dac.CommandType;
                        if (dac.ParamList != null && dac.ParamList.Count > 0)
                            cmdList[i].Parameters.AddRange(dac.ParamList.ToArray());
                        recordsAffectedList[i] = cmdList[i].ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    result = "在服务器（" + connectionStringList[i] + "）执行数据操作时发生错误。\r\nSQL语句：" + dac.CommandText + "\r\n错误描述：" + e.Message + "\r\n错误源：" + e.Source;
                }
                finally
                {
                    if (cmdList[i] != null && cmdList[i].Parameters.Count > 0)
                        cmdList[i].Parameters.Clear();  //为了反复利用参数，某个命令对象在用完之后需要清除参数
                }
                if (needAllDone)
                {
                    if (dac.RecordsAffectedLargeThanZeroIsSuccessful && recordsAffectedList[i] <= 0)
                    {
                        transCommit = false;
                        break;
                    }
                }
            }
            //提交还是回滚事务
            foreach (SqlTransaction trans in transList)
            {
                if (trans != null)
                {
                    if (transCommit)
                        trans.Commit();
                    else
                        trans.Rollback();
                }
            }
            foreach (SqlConnection conn in connList)
            {
                if (conn != null && conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return recordsAffectedList;
        }

        /// <summary>
        /// 在事务内处理一组命令
        /// </summary>
        /// <param name="commands">命令列表</param>
        /// <param name="result">输出参数，如果失败，返回错误提示</param>
        /// <returns>返回是否执行成功</returns>
        public static bool ExecuteBatchCommands(List<DataAccessCommand> commands, out string result)
        {
            bool successed = false;
            result = "";
            SqlConnection conn = new SqlConnection(connectionString);
            SqlTransaction trans = null;
            SqlCommand cmd = new SqlCommand("", conn);
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                cmd.Transaction = trans;
                int executedCommands = 0;
                foreach (DataAccessCommand command in commands)
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
                if (cmd != null && cmd.Parameters.Count > 0)
                    cmd.Parameters.Clear();
                if (conn != null)
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
            return successed;
        }

        /// <summary>
        /// 在事务内处理一组命令
        /// </summary>
        /// <param name="strConn">数据库连接字符串</param>
        /// <param name="commands">命令列表</param>
        /// <param name="result">输出参数，如果失败，返回错误提示</param>
        /// <returns>返回是否执行成功</returns>
        public static bool ExecuteBatchCommands(string strConn, List<DataAccessCommand> commands, out string result)
        {
            bool successed = false;
            result = "";
            SqlConnection conn = new SqlConnection(strConn);
            SqlTransaction trans = null;
            SqlCommand cmd = new SqlCommand("", conn);
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                cmd.Transaction = trans;
                int executedCommands = 0;
                foreach (DataAccessCommand command in commands)
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
                if (cmd != null && cmd.Parameters.Count > 0)
                    cmd.Parameters.Clear();
                if (conn != null)
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
            return successed;
        }

        /// <summary>
        /// 在多个服务器上执行一组命令。
        /// 注意：这里常见的SQL语句包括INSERT、UPDATE、DELETE和存储过程。
        /// </summary>
        /// <param name="connectionStringList">数据库连接字符串列表</param>
        /// <param name="dacList">命令列表</param>
        /// <param name="needAllDone">是否需要所有服务器都执行成功才提交事务</param>
        /// <param name="result">返回操作的结果</param>
        /// <returns>返回在每个服务器上是否执行成功。</returns>
        public static List<bool> ExecuteBatchCommands(List<string> connectionStringList, List<DataAccessCommand> dacList, bool needAllDone, out string result)
        {
            result = "";
            if (connectionStringList == null || connectionStringList.Count == 0)
            {
                result = "数据库连接字符串列表为空。";
                return null;
            }
            if (dacList == null || dacList.Count == 0)
            {
                result = "命令列表为空。";
                return null;
            }
            //定义变量
            int serverCount = connectionStringList.Count;
            List<SqlConnection> connList = new List<SqlConnection>(serverCount);
            List<SqlTransaction> transList = new List<SqlTransaction>(serverCount);
            List<SqlCommand> cmdList = new List<SqlCommand>(serverCount);
            List<bool> successList = new List<bool>(serverCount);
            List<bool> transCommitList = new List<bool>(serverCount);        //是否在某个服务器提交事务
            bool allDone = true;                                            //是否所有的服务器都执行成功
            //分别在每台服务器上执行命令
            for (int i = 0; i < serverCount; i++)
            {
                connList.Add(null);
                transList.Add(null);
                cmdList.Add(null);
                successList.Add(false);
                transCommitList.Add(false);
                int executedCommands = 0;
                successList[i] = false;
                try
                {
                    if (string.IsNullOrEmpty(connectionStringList[i]))
                    {
                        result = string.Format("第{0}个数据库连接字符串为空。", i);
                    }
                    else
                    {
                        connList[i] = new SqlConnection(connectionStringList[i]);
                        connList[i].Open();
                        transList[i] = connList[i].BeginTransaction();
                        cmdList[i] = new SqlCommand("", connList[i], transList[i]);
                        foreach (DataAccessCommand dac in dacList)
                        {
                            cmdList[i].CommandText = dac.CommandText;
                            cmdList[i].CommandType = dac.CommandType;
                            if (dac.ParamList != null && dac.ParamList.Count > 0)
                            {
                                cmdList[i].Parameters.Clear();
                                cmdList[i].Parameters.AddRange(dac.ParamList.ToArray());
                            }
                            int recordsAffected = cmdList[i].ExecuteNonQuery();
                            if (dac.RecordsAffectedLargeThanZeroIsSuccessful)
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
                        if (executedCommands == dacList.Count)
                        {
                            transCommitList[i] = true;
                            successList[i] = true;
                        }
                        else
                        {
                            transCommitList[i] = false;
                            successList[i] = false;
                        }
                    }
                }
                catch (Exception e)
                {
                    result = "在服务器（" + connectionStringList[i] + "）执行数据操作时发生错误。\r\n错误描述：" + e.Message + "\r\n错误源：" + e.Source;
                }
                finally
                {
                    if (cmdList[i] != null && cmdList[i].Parameters.Count > 0)
                        cmdList[i].Parameters.Clear();  //为了反复利用参数，某个命令对象在用完之后需要清除参数
                }
                if (needAllDone)
                {
                    if (successList[i] == false)
                    {
                        allDone = false;
                        break;
                    }
                }
            }
            //提交还是回滚事务
            for (int i = 0; i < connList.Count; i++)
            {
                if (needAllDone && allDone == false)
                    transCommitList[i] = false;
                if (transList[i] != null)
                {
                    if (transCommitList[i])
                        transList[i].Commit();
                    else
                        transList[i].Rollback();
                }
                if (connList[i] != null && connList[i].State != ConnectionState.Closed)
                {
                    connList[i].Close();
                    connList[i] = null;
                }
            }

            return successList;
        }

        /// <summary>
        /// 用BulkCopy的形式将数据表的内容全部保存到数据库中去
        /// </summary>
        /// <param name="strConn">数据库连接字符串</param>
        /// <param name="dtSrc">源数据表</param>
        /// <param name="destTableName">数据库中的目标表名</param>
        /// <param name="columnMappings">列映射列表，如果为null或者Count为0，表示使用默认列映射</param>
        /// <param name="transType">事务类型</param>
        /// <param name="options">选项</param>
        /// <param name="timeout">操时时间（单位：秒）</param>
        /// <returns>返回保存是否成功</returns>
        public static bool BulkCopy(string strConn, DataTable dtSrc, string destTableName, List<SqlBulkCopyColumnMapping> columnMappings, BulkCopyTransType transType, SqlBulkCopyOptions options, int timeout)
        {
            bool bSuccessed = false;
            if (dtSrc != null && dtSrc.Rows != null && dtSrc.Rows.Count > 0 && destTableName != null && destTableName != "" && timeout > 0)
            {
                SqlConnection conn = null;
                SqlTransaction trans = null;
                SqlBulkCopy sbc = null;
                try
                {
                    //对不同的事务类型使用不同的构造函数
                    if (transType == BulkCopyTransType.None)
                    {
                        sbc = new SqlBulkCopy(strConn, options);
                    }
                    else if (transType == BulkCopyTransType.Internal)
                    {
                        sbc = new SqlBulkCopy(strConn, options | SqlBulkCopyOptions.UseInternalTransaction);
                    }
                    else
                    {
                        conn = new SqlConnection(strConn);
                        conn.Open();
                        trans = conn.BeginTransaction();
                        sbc = new SqlBulkCopy(conn, options, trans);
                    }
                    //设置操时时间
                    sbc.BulkCopyTimeout = timeout;
                    //设置数据库中的目标表名
                    sbc.DestinationTableName = destTableName;
                    //设置列映射
                    if (columnMappings != null && columnMappings.Count > 0)
                    {
                        foreach (SqlBulkCopyColumnMapping columnMapping in columnMappings)
                        {
                            sbc.ColumnMappings.Add(columnMapping);
                        }
                    }
                    sbc.WriteToServer(dtSrc);
                    if (trans != null)
                        trans.Commit();
                    bSuccessed = true;
                }
                catch
                {
                    bSuccessed = false;
                    if (trans != null)
                        trans.Rollback();
                }
                finally
                {
                    if (sbc != null)
                        sbc.Close();
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
        /// 用BulkCopy的形式将数据表的内容全部保存到数据库中去
        /// </summary>
        /// <param name="dtSrc">源数据表</param>
        /// <param name="destTableName">数据库中的目标表名</param>
        /// <param name="columnMappings">列映射列表，如果为null或者Count为0，表示使用默认列映射</param>
        /// <param name="transType">事务类型</param>
        /// <param name="options">选项</param>
        /// <param name="timeout">操时时间（单位：秒）</param>
        /// <returns>返回保存是否成功</returns>
        public static bool BulkCopy(DataTable dtSrc, string destTableName, List<SqlBulkCopyColumnMapping> columnMappings, BulkCopyTransType transType, SqlBulkCopyOptions options, int timeout)
        {
            bool bSuccessed = false;
            if (dtSrc != null && dtSrc.Rows != null && dtSrc.Rows.Count > 0 && destTableName != null && destTableName != "" && timeout > 0)
            {
                SqlConnection conn = null;
                SqlTransaction trans = null;
                SqlBulkCopy sbc = null;
                try
                {
                    //对不同的事务类型使用不同的构造函数
                    if (transType == BulkCopyTransType.None)
                    {
                        sbc = new SqlBulkCopy(DataAccess.connectionString, options);
                    }
                    else if (transType == BulkCopyTransType.Internal)
                    {
                        sbc = new SqlBulkCopy(DataAccess.connectionString, options | SqlBulkCopyOptions.UseInternalTransaction);
                    }
                    else
                    {
                        conn = new SqlConnection(DataAccess.connectionString);
                        conn.Open();
                        trans = conn.BeginTransaction();
                        sbc = new SqlBulkCopy(conn, options, trans);
                    }
                    //设置操时时间
                    sbc.BulkCopyTimeout = timeout;
                    //设置数据库中的目标表名
                    sbc.DestinationTableName = destTableName;
                    //设置列映射
                    if (columnMappings != null && columnMappings.Count > 0)
                    {
                        foreach (SqlBulkCopyColumnMapping columnMapping in columnMappings)
                        {
                            sbc.ColumnMappings.Add(columnMapping);
                        }
                    }
                    sbc.WriteToServer(dtSrc);
                    if (trans != null)
                        trans.Commit();
                    bSuccessed = true;
                }
                catch
                {
                    bSuccessed = false;
                    if (trans != null)
                        trans.Rollback();
                }
                finally
                {
                    if (sbc != null)
                        sbc.Close();
                    if (conn != null)
                    {
                        if (conn.State != ConnectionState.Closed)
                            conn.Close();
                    }
                }
            }
            return bSuccessed;
        }
        //以下是几个BulkCopy的不同重载
        public static bool BulkCopy(DataTable dtSrc, string destTableName)
        {
            return BulkCopy(dtSrc, destTableName, null, BulkCopyTransType.None, SqlBulkCopyOptions.Default, BulkCopyTimeout);
        }
        public static bool BulkCopy(DataTable dtSrc, string destTableName, List<SqlBulkCopyColumnMapping> columnMappings)
        {
            return BulkCopy(dtSrc, destTableName, columnMappings, BulkCopyTransType.None, SqlBulkCopyOptions.Default, BulkCopyTimeout);
        }
        public static bool BulkCopy(DataTable dtSrc, string destTableName, List<SqlBulkCopyColumnMapping> columnMappings, BulkCopyTransType transType)
        {
            return BulkCopy(dtSrc, destTableName, columnMappings, transType, SqlBulkCopyOptions.Default, BulkCopyTimeout);
        }
        public static bool BulkCopy(DataTable dtSrc, string destTableName, List<SqlBulkCopyColumnMapping> columnMappings, BulkCopyTransType transType, SqlBulkCopyOptions options)
        {
            return BulkCopy(dtSrc, destTableName, columnMappings, transType, options, BulkCopyTimeout);
        }

        /// <summary>
        /// 得到某个表中符合条件的记录数目
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="filter">筛选条件</param>
        /// <param name="distinct">是否只统计不重复的记录</param>
        /// <param name="result">输出参数，返回错误信息</param>
        /// <returns>返回记录数目；如果发生错误，返回-1。</returns>
        public static int GetRowCount(string tableName, string filter, bool distinct, out string result)
        {
            int rowCount = -1;
            string selectCommandText = "SELECT " + (distinct ? "DISTINCT" : "") + " COUNT(*) FROM [" + tableName + "]";
            if (filter != "")
                selectCommandText += " WHERE " + filter;
            object objValue;
            if (DataAccess.GetOneValue(selectCommandText, out objValue, out result))
            {
                if (!Convert.IsDBNull(objValue))
                    rowCount = (int)objValue;
            }
            return rowCount;
        }
        public static int GetRowCount(string tableName, string filter, out string result)
        {
            return GetRowCount(tableName, filter, false, out result);
        }
        public static int GetRowCount(string tableName, string filter)
        {
            string result;
            return GetRowCount(tableName, filter, false, out result);
        }
        public static int GetRowCount(string tableName)
        {
            string result;
            return GetRowCount(tableName, "", false, out result);
        }

        /// <summary>
        /// 使用分页存储过程获取符合条件的分页数据表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKey">主键字段名</param>
        /// <param name="fields">字段列表</param>
        /// <param name="filter">过滤条件（不包含WHERE）</param>
        /// <param name="group">分组字段列表（不包含GROUP BY）</param>
        /// <param name="sort">排序字段（不包含ORDER BY）</param>
        /// <param name="pageNumber">页号</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>返回数据表；如果获取失败，返回null。</returns>
        public static DataTable GetDataTable(string tableName, string primaryKey, string fields, string filter, string group, string sort, int pageNumber, int pageSize)
        {
            //查询语句：这里使用分页查询存储过程
            string commandText = "EXECUTE SP_Pagination @Tables,@PrimaryKey,@Sort,@CurrentPage,@PageSize,@Fields,@Filter,@Group";
            //参数列表
            List<SqlParameter> paramList = new List<SqlParameter>(5);
            SqlParameter p;
            p = new SqlParameter("@Tables", SqlDbType.NVarChar, 1000);
            p.Value = tableName;
            paramList.Add(p);
            p = new SqlParameter("@PrimaryKey", SqlDbType.NVarChar, 100);
            p.Value = primaryKey;
            paramList.Add(p);
            p = new SqlParameter("@Sort", SqlDbType.NVarChar, 200);
            p.Value = sort;
            paramList.Add(p);
            p = new SqlParameter("@CurrentPage", SqlDbType.Int, 4);
            p.Value = pageNumber;
            paramList.Add(p);
            p = new SqlParameter("@PageSize", SqlDbType.Int, 4);
            p.Value = pageSize;
            paramList.Add(p);
            p = new SqlParameter("@Fields", SqlDbType.NVarChar, 1000);
            p.Value = fields;
            paramList.Add(p);
            p = new SqlParameter("@Filter", SqlDbType.NVarChar, 1000);
            p.Value = filter;
            paramList.Add(p);
            p = new SqlParameter("@Group", SqlDbType.NVarChar, 1000);
            p.Value = group;
            paramList.Add(p);
            //定义其他变量
            string result;
            DataTable dt = new DataTable();
            //执行查询
            if (!DataAccess.GetDataTable(commandText, dt, paramList, out result))
                dt = null;
            return dt;
        }

        /// <summary>
        /// 使用分页存储过程获取符合条件的分页数据表
        /// </summary>
        /// <param name="strConn">数据库连接字符串</param>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKey">主键字段名</param>
        /// <param name="fields">字段列表</param>
        /// <param name="filter">过滤条件（不包含WHERE）</param>
        /// <param name="group">分组字段列表（不包含GROUP BY）</param>
        /// <param name="sort">排序字段（不包含ORDER BY）</param>
        /// <param name="pageNumber">页号</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>返回数据表；如果获取失败，返回null。</returns>
        public static DataTable GetDataTable(string strConn, string tableName, string primaryKey, string fields, string filter, string group, string sort, int pageNumber, int pageSize)
        {
            //查询语句：这里使用分页查询存储过程
            string commandText = "EXECUTE SP_Pagination @Tables,@PrimaryKey,@Sort,@CurrentPage,@PageSize,@Fields,@Filter,@Group";
            //参数列表
            List<SqlParameter> paramList = new List<SqlParameter>(5);
            SqlParameter p;
            p = new SqlParameter("@Tables", SqlDbType.NVarChar, 1000);
            p.Value = tableName;
            paramList.Add(p);
            p = new SqlParameter("@PrimaryKey", SqlDbType.NVarChar, 100);
            p.Value = primaryKey;
            paramList.Add(p);
            p = new SqlParameter("@Sort", SqlDbType.NVarChar, 200);
            p.Value = sort;
            paramList.Add(p);
            p = new SqlParameter("@CurrentPage", SqlDbType.Int, 4);
            p.Value = pageNumber;
            paramList.Add(p);
            p = new SqlParameter("@PageSize", SqlDbType.Int, 4);
            p.Value = pageSize;
            paramList.Add(p);
            p = new SqlParameter("@Fields", SqlDbType.NVarChar, 1000);
            p.Value = fields;
            paramList.Add(p);
            p = new SqlParameter("@Filter", SqlDbType.NVarChar, 1000);
            p.Value = filter;
            paramList.Add(p);
            p = new SqlParameter("@Group", SqlDbType.NVarChar, 1000);
            p.Value = group;
            paramList.Add(p);
            //定义其他变量
            string result;
            DataTable dt = new DataTable();
            //执行查询
            if (!DataAccess.GetDataTable(strConn, commandText, dt, null, out result))
                dt = null;
            return dt;
        }
    }


    /// <summary>
    /// BulkCopy操作的事务类型
    /// </summary>
    public enum BulkCopyTransType
    {
        None,		//不使用事务
        Internal,	//使用内部事务：对每一个复制批次使用事务
        External	//使用外部事务：对整个复制使用批次
    }

    /// <summary>
    /// DataAccessCommand命令：包括命令语句和命令所用的参数列表。
    /// </summary>
    public class DataAccessCommand
    {
        //公共字段
        public string CommandText;							//命令语句
        public List<SqlParameter> ParamList;				//参数列表
        public CommandType CommandType;						//命令类型
        public bool RecordsAffectedLargeThanZeroIsSuccessful;	//执行命令所影响的记录数大于0才成功吗？

        //构造函数
        public DataAccessCommand(string commandText, List<SqlParameter> paramList, CommandType commandType, bool recordsAffectedLargeThanZeroIsSuccessful)
        {
            CommandText = commandText;
            ParamList = paramList;
            this.CommandType = commandType;
            RecordsAffectedLargeThanZeroIsSuccessful = recordsAffectedLargeThanZeroIsSuccessful;
        }

        public DataAccessCommand(string commandText, List<SqlParameter> paramList)
            : this(commandText, paramList, CommandType.Text, true)
        {
        }

        public DataAccessCommand(string commandText)
            : this(commandText, null)
        {
        }
    }
}
