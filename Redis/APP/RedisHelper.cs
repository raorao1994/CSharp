using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP
{
    public class RedisHelper
    {
        #region 
        //执行顺序---静态字段---静态构造函数---构造函数
        private static ConnectionMultiplexer redis;

        static RedisHelper()
        {
            if (redis == null || redis.IsConnected)
            {
                string val = ConfigurationManager.AppSettings["WriteServerConStr"];
                val = val+","+ConfigurationManager.AppSettings["ReadServerConStr"];
                redis = ConnectionMultiplexer.Connect(val);
            }
        }

        #endregion

        #region redis 字符串（string）操作

        /// <summary>
        /// 设置指定键的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool StringSet(string key, string value)
        {
            return redis.GetDatabase().StringSet(key, value);
        }

        /// <summary>
        /// 获取指定键的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object StringGet(string key)
        {
            return redis.GetDatabase().StringGet(key);
        }

        /// <summary>
        /// 获取存储在键上的字符串的子字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static object StringGet(string key, int start, int end)
        {
            return redis.GetDatabase().StringGetRange(key, start, end);
        }

        /// <summary>
        /// 设置键的字符串值并返回其旧值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object StringGetAndSet(string key, string value)
        {
            return redis.GetDatabase().StringGetSet(key, value);
        }

        /// <summary>
        /// 返回在键处存储的字符串值中偏移处的位值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static bool StringGetBit(string key, long offset)
        {
            return redis.GetDatabase().StringGetBit(key, offset);
        }

        /// <summary>
        /// 获取所有给定键的值
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static List<object> StringMultiGet(string[] keys)
        {
            List<object> list = new List<object>();
            for (int i = 0; i < keys.Length; i++)
            {
                list.Add(redis.GetDatabase().StringGet(keys[i]));
            }
            return list;
        }

        /// <summary>
        /// 存储在键上的字符串值中设置或清除偏移处的位
        /// </summary>
        /// <param name="key"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool StringSetBit(string key, long offset)
        {
            return redis.GetDatabase().StringSetBit(key, offset, true);
        }

        /// <summary>
        /// 使用键和到期时间来设置值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static bool StringSet(string key, string value, TimeSpan expiry)
        {
            return redis.GetDatabase().StringSet(key, value, expiry);
        }

        /// <summary>
        /// 设置键的值，仅当键不存在时
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void StringSetIfAbsent(string key, string value)
        {
            if (redis.GetDatabase().StringGet(key) == RedisValue.Null)
            {
                redis.GetDatabase().StringSet(key, value);
            }
        }

        /// <summary>
        /// 在指定偏移处开始的键处覆盖字符串的一部分
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">值</param>
        /// <param name="offset">偏移量</param>
        /// <returns></returns>
        public static object StringSet(string key, long offset, string value)
        {
            return redis.GetDatabase().StringSetRange(key, offset, value);
        }

        /// <summary>
        /// 获取存储在键中的值的长度
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public static long StringSize(string key)
        {
            return redis.GetDatabase().StringLength(key);
        }

        /// <summary>
        /// 为多个键分别设置它们的值
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static void StringMultiSet(Dictionary<string, string> dic)
        {
            foreach (KeyValuePair<string, string> key in dic)
            {
                redis.GetDatabase().StringSet(key.Key, key.Value);
            }
        }

        /// <summary>
        /// 为多个键分别设置它们的值，仅当键不存在时
        /// </summary>
        /// <param name="keys">键值集合</param>
        /// <returns></returns>
        public static void StringMultiSetIfAbsent(Dictionary<string, string> dic)
        {
            foreach (KeyValuePair<string, string> key in dic)
            {
                //判断键值是否存在
                if (redis.GetDatabase().StringGet(key.Key) == RedisValue.Null)
                {
                    redis.GetDatabase().StringSet(key.Key, key.Value);
                }
            }
        }

        /// <summary>
        /// 将键的整数值按给定的数值增加
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">给定的数值</param>
        /// <returns></returns>
        public static double StringIncrement(string key, double value)
        {
            return redis.GetDatabase().StringIncrement(key, value);
        }

        /// <summary>
        /// 在key键对应值的右面追加值value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long StringAppend(string key, string value)
        {
            return redis.GetDatabase().StringAppend(key, value);
        }

        /// <summary>
        /// 删除某个键值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool StringDelete(string key)
        {
            return false;
        }

        #endregion

        #region redis 哈希/散列/字典（Hash）操作

        /// <summary>
        /// 删除指定的哈希字段
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static bool HashDelete(string key, string field)
        {
            return redis.GetDatabase().HashDelete(key, field);
        }

        /// <summary>
        /// 判断是否存在散列字段
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public static bool HashHasKey(string key, string field)
        {
            return redis.GetDatabase().HashExists(key, field);
        }

        /// <summary>
        /// 获取存储在指定键的哈希字段的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static object HashGet(string key, string field)
        {
            return redis.GetDatabase().HashGet(key, field);
        }

        /// <summary>
        /// 获取存储在指定键的哈希中的所有字段和值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Dictionary<string, object> HashGetAll(string key)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            var collection = redis.GetDatabase().HashGetAll(key);
            foreach (var item in collection)
            {
                dic.Add(item.Name, item.Value);
            }
            return dic;
        }

        /// <summary>
        /// 将哈希字段的浮点值按给定数值增加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value">给定的数值</param>
        /// <returns></returns>
        public static double HashIncrement(string key, string field, double value)
        {
            return redis.GetDatabase().HashIncrement(key, field, value);
        }

        /// <summary>
        /// 获取哈希中的所有字段
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string[] HashKeys(string key)
        {
            return redis.GetDatabase().HashKeys(key).ToStringArray();
        }

        /// <summary>
        /// 获取散列中的字段数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long HashSize(string key)
        {
            return redis.GetDatabase().HashLength(key);
        }

        /// <summary>
        /// 获取所有给定哈希字段的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashKeys"></param>
        /// <returns></returns>
        public static List<object> HashMultiGet(string key, List<string> hashKeys)
        {
            List<object> result = new List<object>();
            foreach (string field in hashKeys)
            {
                result.Add(redis.GetDatabase().HashGet(key, field));
            }
            return result;
        }

        /// <summary>
        /// 为多个哈希字段分别设置它们的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static void HashPutAll(string key, Dictionary<string, string> dic)
        {
            List<HashEntry> list = new List<HashEntry>();
            for (int i = 0; i < dic.Count; i++)
            {
                KeyValuePair<string, string> param = dic.ElementAt(i);
                list.Add(new HashEntry(param.Key, param.Value));
            }
            redis.GetDatabase().HashSet(key, list.ToArray());
        }

        /// <summary>
        /// 设置散列字段的字符串值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void HashPut(string key, string field, string value)
        {
            redis.GetDatabase().HashSet(key, new HashEntry[] { new HashEntry(field, value) });
        }

        /// <summary>
        /// 仅当字段不存在时，才设置散列字段的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fiels"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void HashPutIfAbsent(string key, string field, string value)
        {
            if (!HashHasKey(key, field))
            {
                redis.GetDatabase().HashSet(key, new HashEntry[] { new HashEntry(field, value) });
            }
        }

        /// <summary>
        /// 获取哈希中的所有值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string[] HashValues(string key)
        {
            return redis.GetDatabase().HashValues(key).ToStringArray();
        }

        /// <summary>
        /// redis中获取指定键的值并返回对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetHashValue<T>(string key)
        {
            HashEntry[] array = redis.GetDatabase().HashGetAll(key);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            for (int i = 0; i < array.Length; i++)
            {
                dic.Add(array[i].Name, array[i].Value);
            }
            string strJson = JsonConvert.SerializeObject(dic);
            return JsonConvert.DeserializeObject<T>(strJson);
        }

        /// <summary>
        /// 把指定对象存储在键值为key的redis中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="key"></param>
        public static void SetHashValue<T>(T t, string key)
        {
            string strJson = JsonConvert.SerializeObject(t);
            Dictionary<string, string> param = JsonConvert.DeserializeObject<Dictionary<string, string>>(strJson);
            HashPutAll(key, param);
        }

        #endregion

        #region redis 列表（List）操作

        /// <summary>
        /// 从左向右存压栈
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ListLeftPush(string key, string value)
        {
            return redis.GetDatabase().ListLeftPush(key, value);
        }

        /// <summary>
        /// 从左出栈
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object ListLeftPop(string key)
        {
            return redis.GetDatabase().ListLeftPop(key);
        }

        /// <summary>
        /// 队/栈长
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long ListSize(string key)
        {
            return redis.GetDatabase().ListLength(key);
        }

        /// <summary>
        /// 范围检索,返回List
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static string[] ListRange(string key, int start, int end)
        {
            return redis.GetDatabase().ListRange(key, start, end).ToStringArray();
        }

        /// <summary>
        /// 移除key中值为value的i个,返回删除的个数；如果没有这个元素则返回0 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="i"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ListRemove(string key, string value)
        {
            return redis.GetDatabase().ListRemove(key, value);
        }

        /// <summary>
        /// 检索
        /// </summary>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static object ListIndex(string key, long index)
        {
            return redis.GetDatabase().ListGetByIndex(key, index);
        }

        /// <summary>
        /// 赋值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void ListSet(string key, int index, string value)
        {
            redis.GetDatabase().ListSetByIndex(key, index, value);
        }

        /// <summary>
        /// 裁剪,删除除了[start,end]以外的所有元素 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static void ListTrim(string key, int start, int end)
        {
            redis.GetDatabase().ListTrim(key, start, end);
        }

        /// <summary>
        /// 将源key的队列的右边的一个值删除，然后塞入目标key的队列的左边，返回这个值
        /// </summary>
        /// <param name="sourceKey"></param>
        /// <param name="destinationKey"></param>
        /// <returns></returns>
        public static object ListRightPopAndLeftPush(string sourceKey, string destinationKey)
        {
            return redis.GetDatabase().ListRightPopLeftPush(sourceKey, destinationKey);
        }

        #endregion

        #region redis 集合（Set）操作

        /// <summary>
        /// 集合添加元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetAdd(string key, string value)
        {
            redis.GetDatabase().SetAdd(key, value);
        }

        /// <summary>
        /// 集合组合操作
        /// </summary>
        /// <param name="point">操作标示：0--并集；1--交集；2--差集</param>
        /// <param name="firstKey">第一个集合的键值</param>
        /// <param name="secondKey">第二个集合的键值</param>
        public static string[] SetCombine(int point, string firstKey, string secondKey)
        {
            RedisValue[] array;
            switch (point)
            {
                case 0:
                    array = redis.GetDatabase().SetCombine(SetOperation.Union, firstKey, secondKey);
                    break;
                case 1:
                    array = redis.GetDatabase().SetCombine(SetOperation.Intersect, firstKey, secondKey);
                    break;
                case 2:
                    array = redis.GetDatabase().SetCombine(SetOperation.Difference, firstKey, secondKey);
                    break;
                default:
                    array = new RedisValue[0];
                    break;
            }
            return array.ToStringArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetContains(string key, string value)
        {
            return redis.GetDatabase().SetContains(key, value);
        }

        /// <summary>
        /// 返回对应键值集合的长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long SetLength(string key)
        {
            return redis.GetDatabase().SetLength(key);
        }

        /// <summary>
        /// 根据键值返回集合中所有的value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string[] SetMembers(string key)
        {
            return redis.GetDatabase().SetMembers(key).ToStringArray();
        }

        /// <summary>
        /// 将成员从源集移动到目标集
        /// </summary>
        /// <param name="sourceKey">源集key</param>
        /// <param name="destinationKey">目标集key</param>
        /// <param name="value"></param>
        public static bool SetMove(string sourceKey, string destinationKey, string value)
        {
            return redis.GetDatabase().SetMove(sourceKey, destinationKey, value);
        }

        /// <summary>
        /// 移除集合中指定键值随机元素
        /// </summary>
        /// <param name="key"></param>
        public static string SetPop(string key)
        {
            return redis.GetDatabase().SetPop(key);
        }

        /// <summary>
        /// 返回集合中指定键值随机元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string SetRandomMember(string key)
        {
            return redis.GetDatabase().SetRandomMember(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        public static string[] SetRandomMembers(string key, long count)
        {
            return redis.GetDatabase().SetRandomMembers(key, count).ToStringArray();
        }

        /// <summary>
        /// 移除集合中指定key值和value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetRemove(string key, string value)
        {
            redis.GetDatabase().SetRemove(key, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public static void SetScan(string key)
        {
            redis.GetDatabase().SetScan(key);
        }

        #endregion

        #region redis 有序集合（sorted set）操作

        public static void Method(string key, string value, double score)
        {
            redis.GetDatabase().SortedSetAdd(key, new SortedSetEntry[] { new SortedSetEntry(value, score) });
        }

        #endregion
    }
}
