using System;
using System.Data;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

namespace PublicLibrary
{
    /// <summary>
    /// DataTableEx
    /// 功能：操作DataTable的一些静态方法。
    /// 作者：raoaro
    /// 时间：2018年2月8日
    /// </summary>
    public static class DataTableEx
    {
        /// <summary>
        /// 将DataTable序列化成字节数组
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <returns>如果序列化成功，返回字节数组；否则返回null。</returns>
        public static byte[] Serialize(DataTable dt)
        {
            byte[] bytes = null;
            if (dt != null)
            {
                MemoryStream ms = new MemoryStream();
                BinaryFormatter bf = new BinaryFormatter();
                try
                {
                    bf.Serialize(ms, dt);
                    bytes = ms.ToArray();
                }
                catch { }
                ms.Close();
            }
            return bytes;
        }

        /// <summary>
        /// 将字节数组反序列化为DataTable对象
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns>返回数据表；如果失败，返回null。</returns>
        public static DataTable Deserialize(byte[] bytes)
        {
            DataTable dt = null;
            if (bytes != null && bytes.Length > 0)
            {
                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream ms = new MemoryStream(bytes);
                try
                {
                    dt = (DataTable)bf.Deserialize(ms);
                }
                catch { }
                ms.Close();
            }
            return dt;
        }

        /// <summary>
        /// 从数据表中移除列名列表中的列
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <param name="columnList">需要被移除的列名列表</param>
        /// <returns>返回成功移除列的数目</returns>
        public static int RemoveDataColumn(DataTable dt, List<string> columnList)
        {
            int count = 0;
            if (dt != null && dt.Columns != null && dt.Columns.Count > 0 && columnList != null && columnList.Count > 0)
            {
                foreach (string columnName in columnList)
                {
                    if (dt.Columns.Contains(columnName))
                    {
                        DataColumn dc = dt.Columns[columnName];
                        if (dt.Columns.CanRemove(dc))
                        {
                            dt.Columns.Remove(dc);
                            count++;
                        }
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// 从数据表中移除列名列表中的列
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <param name="columnList">需要被移除的列名列表</param>
        /// <returns>返回成功移除列的数目</returns>
        public static int RemoveDataColumn(DataTable dt, string[] columnList)
        {
            if (columnList.Length > 0)
                return RemoveDataColumn(dt, new List<string>(columnList));
            else
                return 0;
        }

        /// <summary>
        /// 比较两个数据表的表结构是否相同；
        /// 比较过程如下：
        /// （1）比较列数目是否相同；
        /// （2）比较每列的列名及数据类型是否相同。
        /// </summary>
        /// <param name="dt1">数据表1</param>
        /// <param name="dt2">数据表2</param>
        /// <param name="needSameColumnOrder">是否需要列的次序保持一致</param>
        /// <returns>返回比较的结果。</returns>
        public static bool CompareTableStructure(DataTable dt1, DataTable dt2, bool needSameColumnOrder)
        {
            if (dt1 == null && dt2 == null)
                return true;
            if ((dt1 == null && dt2 != null) || (dt1 != null && dt2 == null))
                return false;
            if (dt1 != null && dt2 != null)
            {
                if (dt1.Columns.Count != dt2.Columns.Count)     //列数目不同
                    return false;
                for (int i = 0; i < dt1.Columns.Count; i++)
                {
                    if (needSameColumnOrder)
                    {
                        if (dt1.Columns[i].ColumnName != dt2.Columns[i].ColumnName || dt1.Columns[i].DataType != dt2.Columns[i].DataType)   //列名或者列数据类型不一致
                            return false;
                    }
                    else
                    {
                        DataColumn dc = dt1.Columns[i];
                        if (!dt2.Columns.Contains(dc.ColumnName))   //数据表2中不包含数据表1中的某列
                            return false;
                        if (dc.DataType != dt2.Columns[dc.ColumnName].DataType) //列的数据类型不同
                            return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 合并数据表：将dtSource表中的数据行添加到dtDest表的行之后
        /// </summary>
        /// <param name="dtDest">目标数据表</param>
        /// <param name="dtSource">源数据表</param>
        /// <returns>返回是否合并成功。</returns>
        public static bool Merge(ref DataTable dtDest, DataTable dtSource)
        {
            if (dtSource == null || dtSource.Rows == null || dtSource.Rows.Count == 0)
                return false;
            if (dtDest == null)
                dtDest = dtSource.Copy();
            else
            {
                if (!CompareTableStructure(dtDest, dtSource, false))    //如果表结构不一致，返回
                    return false;
                foreach (DataRow drSource in dtSource.Rows)
                {
                    DataRow drNew = dtDest.NewRow();
                    for (int i = 0; i < dtDest.Columns.Count; i++)
                        drNew[i] = drSource[dtDest.Columns[i].ColumnName];  //drNew[i] = drSource[i];
                    dtDest.Rows.Add(drNew);
                }
            }
            dtDest.AcceptChanges();
            return true;
        }

        /// <summary>
        /// 从指定数据表中查找特定值所在的数据行
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <param name="columnName">被查找的列名</param>
        /// <param name="value">被查找的值</param>
        /// <returns>如果成功找到了值所在的数据行，返回数据行；否则返回null。</returns>
        public static DataRow FindDataRow(DataTable dt, string columnName, object value)
        {
            if (dt != null)
            {
                int columnIndex = dt.Columns.IndexOf(columnName);
                return FindDataRow(dt, columnIndex, value);
            }
            else
                return null;
        }

        /// <summary>
        /// 从指定数据表中查找特定值所在的数据行
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <param name="columnName">被查找的列名</param>
        /// <param name="value">被查找的值</param>
        /// <returns>如果成功找到了值所在的数据行，返回数据行的索引；否则返回-1。</returns>
        public static int FindDataRowIndex(DataTable dt, string columnName, object value)
        {

            if (dt != null)
            {
                int columnIndex = dt.Columns.IndexOf(columnName);
                return FindDataRowIndex(dt, columnIndex, value);
            }
            else
                return -1;
        }

        /// <summary>
        /// 从指定数据表中查找特定值所在的数据行
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <param name="columnIndex">被查找的列索引</param>
        /// <param name="value">被查找的值</param>
        /// <returns>如果成功找到了值所在的数据行，返回数据行；否则返回null。</returns>
        public static DataRow FindDataRow(DataTable dt, int columnIndex, object value)
        {
            int rowIndex = FindDataRowIndex(dt, columnIndex, value);
            if (rowIndex != -1)
                return dt.Rows[rowIndex];
            else
                return null;
        }

        /// <summary>
        /// 从指定数据表中查找特定值所在的数据行
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <param name="columnIndex">被查找的列索引</param>
        /// <param name="value">被查找的值</param>
        /// <returns>如果成功找到了值所在的数据行，返回数据行的索引；否则返回-1。</returns>
        public static int FindDataRowIndex(DataTable dt, int columnIndex, object value)
        {
            int rowIndex = -1;
            if (dt != null && dt.Rows != null && dt.Rows.Count > 0 && columnIndex >= 0 && columnIndex < dt.Columns.Count)
            {
                Type type = value.GetType();
                if (dt.Columns[columnIndex].DataType == type)
                {
                    if (type.IsValueType || type == typeof(string))
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i][columnIndex].ToString() == value.ToString())
                            {
                                rowIndex = i;
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i][columnIndex] == value)
                            {
                                rowIndex = i;
                                break;
                            }
                        }
                    }
                }
            }
            return rowIndex;
        }

        /// <summary>
        /// 从指定数据表中查找特定值所在的数据行
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <param name="columnNameAndValues">关键列及值</param>
        /// <returns>如果成功找到了值所在的数据行，返回数据行；否则返回null。</returns>
        public static DataRow FindDataRow(DataTable dt, Dictionary<int, object> columnIndexAndValues)
        {
            int rowIndex = FindDataRowIndex(dt, columnIndexAndValues);
            if (rowIndex != -1)
                return dt.Rows[rowIndex];
            else
                return null;
        }

        /// <summary>
        /// 从指定数据表中查找特定值所在的数据行
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <param name="columnIndexAndValues">关键列及值</param>
        /// <returns>如果成功找到了值所在的数据行，返回数据行的索引；否则返回-1。</returns>
        public static int FindDataRowIndex(DataTable dt, Dictionary<int, object> columnIndexAndValues)
        {
            int rowIndex = -1;
            if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
            {
                //检查关键列是否都有效
                int columnCount = dt.Columns.Count;
                foreach (KeyValuePair<int, object> pair in columnIndexAndValues)
                {
                    if (pair.Key < 0 || pair.Key >= columnCount)    //索引是否在范围内
                        return -1;
                    if (dt.Columns[pair.Key].DataType != pair.Value.GetType())  //类型是否相同
                        return -1;
                }
                //依次在每行中查找
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    bool match = true;
                    DataRow dr = dt.Rows[i];
                    foreach (KeyValuePair<int, object> pair in columnIndexAndValues)
                    {
                        Type type = pair.Value.GetType();
                        if (type.IsValueType || type == typeof(string))
                        {
                            if (dr[pair.Key].ToString() != pair.Value.ToString())
                            {
                                match = false;
                                break;
                            }
                        }
                        else
                        {
                            if (dr[pair.Key] != pair.Value)
                            {
                                match = false;
                                break;
                            }
                        }
                    }
                    if (match)
                    {
                        rowIndex = i;
                        break;
                    }
                }
            }
            return rowIndex;
        }

        /// <summary>
        /// 从指定数据表中查找特定值所在的数据行
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <param name="columnNameAndValues">关键列及值</param>
        /// <returns>如果成功找到了值所在的数据行，返回数据行；否则返回null。</returns>
        public static DataRow FindDataRow(DataTable dt, Dictionary<string, object> columnNameAndValues)
        {
            int rowIndex = FindDataRowIndex(dt, columnNameAndValues);
            if (rowIndex != -1)
                return dt.Rows[rowIndex];
            else
                return null;
        }

        /// <summary>
        /// 从指定数据表中查找特定值所在的数据行
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <param name="columnIndexAndValues">关键列及值</param>
        /// <returns>如果成功找到了值所在的数据行，返回数据行的索引；否则返回-1。</returns>
        public static int FindDataRowIndex(DataTable dt, Dictionary<string, object> columnNameAndValues)
        {
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
                return -1;
            //先将关键列名转换成列号，然后再查找
            Dictionary<int, object> columnIndexAndValues = new Dictionary<int, object>(columnNameAndValues.Count);
            foreach (KeyValuePair<string, object> pair in columnNameAndValues)
            {
                int columnIndex = dt.Columns.IndexOf(pair.Key);
                if (columnIndex >= 0)
                    columnIndexAndValues.Add(columnIndex, pair.Value);
                else
                    return -1;
            }
            return FindDataRowIndex(dt, columnIndexAndValues);
        }

        /// <summary>
        /// 汇总数据表：对于dtSource与dtDest共有的数据行，将dtSource表中的数据累加到dtDest表；对于dtSource中多出的数据行，添加到dtDest表之中；对于dtDest中多出的数据行，保持原样。
        /// 注：要求dtDest和dtSource有相同的表结构，并且除了关键列之外的其他列数据类型均为整型。
        /// </summary>
        /// <param name="dtDest">目标数据表</param>
        /// <param name="dtSource">源数据表</param>
        /// <param name="keyColumnName">关键列名</param>
        /// <returns>返回是否汇总成功。</returns>
        public static bool Summarize(ref DataTable dtDest, DataTable dtSource, string keyColumnName)
        {
            if (dtSource == null || dtSource.Rows == null || dtSource.Rows.Count == 0)
                return false;
            if (dtDest == null)
                dtDest = dtSource.Copy();
            else
            {
                if (!CompareTableStructure(dtDest, dtSource, false))    //如果表结构不一致，返回
                    return false;
                if (!dtDest.Columns.Contains(keyColumnName))            //如果表中不包含关键列，返回
                    return false;
                foreach (DataRow drSource in dtSource.Rows)
                {
                    DataRow drDest = FindDataRow(dtDest, keyColumnName, drSource[keyColumnName]);
                    if (drDest != null)
                    {
                        //如果在dtDest中找到了数据行，累加数据
                        for (int i = 0; i < dtDest.Columns.Count; i++)
                        {
                            string columnName = dtDest.Columns[i].ColumnName;
                            if (columnName != keyColumnName)
                                drDest[i] = (Convert.IsDBNull(drDest[i]) ? 0 : (int)drDest[i]) + (Convert.IsDBNull(drSource[columnName]) ? 0 : (int)drSource[columnName]);
                        }
                    }
                    else
                    {
                        //如果没有找到区域对应的行，创建新行
                        DataRow drNew = dtDest.NewRow();
                        for (int i = 0; i < dtDest.Columns.Count; i++)
                        {
                            string columnName = dtDest.Columns[i].ColumnName;
                            drNew[i] = drSource[columnName];
                        }
                        dtDest.Rows.Add(drNew);
                    }
                }
            }
            dtDest.AcceptChanges();
            return true;
        }

        /// <summary>
        /// 汇总数据表：对于dtSource与dtDest共有的数据行，将dtSource表中的数据累加到dtDest表；对于dtSource中多出的数据行，添加到dtDest表之中；对于dtDest中多出的数据行，保持原样。
        /// </summary>
        /// <param name="dtDest">目标数据表</param>
        /// <param name="dtSource">源数据表</param>
        /// <param name="keyColumnIndex">关键列索引</param>
        /// <returns>返回是否汇总成功。</returns>
        public static bool Summarize(ref DataTable dtDest, DataTable dtSource, int keyColumnIndex)
        {
            if (dtSource != null && keyColumnIndex >= 0 && keyColumnIndex < dtSource.Columns.Count)
            {
                string keyColumnName = dtSource.Columns[keyColumnIndex].ColumnName;
                return Summarize(ref dtDest, dtSource, keyColumnName);
            }
            else
                return false;
        }

        /// <summary>
        /// 汇总数据表：对于dtSource与dtDest共有的数据行，将dtSource表中的数据累加到dtDest表；对于dtSource中多出的数据行，添加到dtDest表之中；对于dtDest中多出的数据行，保持原样。
        /// 注：要求dtDest和dtSource有相同的表结构，并且除了关键列之外的其他列数据类型均为整型。
        /// </summary>
        /// <param name="dtDest">目标数据表</param>
        /// <param name="dtSource">源数据表</param>
        /// <param name="keyColumnName">关键列名</param>
        /// <returns>返回是否汇总成功。</returns>
        public static bool Summarize(ref DataTable dtDest, DataTable dtSource, List<string> keyColumnNames)
        {
            if (dtSource == null || dtSource.Rows == null || dtSource.Rows.Count == 0)
                return false;
            if (dtDest == null)
                dtDest = dtSource.Copy();
            else
            {
                if (!CompareTableStructure(dtDest, dtSource, false))    //如果表结构不一致，返回
                    return false;
                foreach (string keyColumnName in keyColumnNames)
                {
                    if (!dtDest.Columns.Contains(keyColumnName))        //如果表中不包含关键列，返回
                        return false;
                }
                foreach (DataRow drSource in dtSource.Rows)
                {
                    //查找关键列在源表中的值
                    Dictionary<string, object> columnNameAndValues = new Dictionary<string, object>();
                    foreach (string keyColumnName in keyColumnNames)
                        columnNameAndValues.Add(keyColumnName, drSource[keyColumnName]);
                    //在目标表中查找匹配的数据行
                    DataRow drDest = FindDataRow(dtDest, columnNameAndValues);
                    if (drDest != null)
                    {
                        //如果在dtDest中找到了数据行，累加数据
                        for (int i = 0; i < dtDest.Columns.Count; i++)
                        {
                            string columnName = dtDest.Columns[i].ColumnName;
                            if (!keyColumnNames.Contains(columnName))
                                drDest[i] = (Convert.IsDBNull(drDest[i]) ? 0 : (int)drDest[i]) + (Convert.IsDBNull(drSource[columnName]) ? 0 : (int)drSource[columnName]);
                        }
                    }
                    else
                    {
                        //如果没有找到区域对应的行，创建新行
                        DataRow drNew = dtDest.NewRow();
                        for (int i = 0; i < dtDest.Columns.Count; i++)
                        {
                            string columnName = dtDest.Columns[i].ColumnName;
                            drNew[i] = drSource[columnName];
                        }
                        dtDest.Rows.Add(drNew);
                    }
                }
            }
            dtDest.AcceptChanges();
            return true;
        }

        /// <summary>
        /// 汇总数据表：对于dtSource与dtDest共有的数据行，将dtSource表中的数据累加到dtDest表；对于dtSource中多出的数据行，添加到dtDest表之中；对于dtDest中多出的数据行，保持原样。
        /// </summary>
        /// <param name="dtDest">目标数据表</param>
        /// <param name="dtSource">源数据表</param>
        /// <param name="keyColumnIndex">关键列索引</param>
        /// <returns>返回是否汇总成功。</returns>
        public static bool Summarize(ref DataTable dtDest, DataTable dtSource, List<int> keyColumnIndexes)
        {
            if (dtSource != null)
            {
                List<string> keyColumnNames = new List<string>(keyColumnIndexes.Count);
                int columnCount = dtSource.Columns.Count;
                foreach (int keyColumnIndex in keyColumnIndexes)
                {
                    if (keyColumnIndex >= 0 && keyColumnIndex < columnCount)
                        keyColumnNames.Add(dtSource.Columns[keyColumnIndex].ColumnName);
                    else
                        return false;
                }
                return Summarize(ref dtDest, dtSource, keyColumnNames);
            }
            else
                return true;
        }
    }
}
