using NtierDemo.Core.DataAccess.AdoNet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NtierDemo.Core.Helpers.DataAccess.AdoNet
{
    public class AdoSqlQueryHelper : IHelper
    {
        /// <summary>
        /// Sql 'SELECT' query for all columns
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns>Query String</returns>
        public string Select(string tableName) => $"SELECT * FROM {tableName} ";

        /// <summary>
        /// Sql 'SELECT' query with column names
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columNames"></param>
        /// <returns>Query String</returns>
        public string Select(string tableName, params string[] columnNames)
        {
            string _columnNames = string.Join(", ", columnNames);
            return $"SELECT {_columnNames} FROM {tableName} ";
        }

        /// <summary>
        /// Sql 'INSERT' query for all columns
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="values"></param>
        /// <param name="output"></param>
        /// <returns>Query String</returns>
        public string Insert(string tableName, params string[] values)
        {
            string _values = string.Join(", ", values);
            return $"INSERT INTO {tableName} VALUES ({_values})";
        }

        /// <summary>
        /// Sql 'INSERT' query with column names
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="values"></param>
        /// <returns>Query String</returns>
        public string Insert(string tableName, string[] columnNames, params string[] values)
        {
            string _columnNames = string.Join(", ", columnNames);
            string _values = string.Join(", ", values.Select(x => x.Replace(x, $"'{x}'")));
            return $"INSERT INTO {tableName} ({_columnNames}) VALUES ({_values})";
        }

        public string Update(string tableName, IDictionary<string, object> columnNameValues, string condition)
        {
            int i = 0;
            string _keyValues = string.Empty;

            foreach (KeyValuePair<string, object> item in columnNameValues)
            {
                _keyValues += $"{item.Key} = {(typeof(string) == item.Value.GetType() ? "'" : string.Empty)}{item.Value}{(typeof(string) == item.Value.GetType() ? "'" : string.Empty)}";
                if (i != columnNameValues.Count - 1) _keyValues += ", ";
                i++;
            }
            return $"UPDATE {tableName} SET {_keyValues} {condition}";
        }


        /// <summary>
        /// Sql 'DELETE' query with condition param
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="condition"></param>
        /// <returns>Query String</returns>
        public string Delete(string tableName, string condition) => $"DELETE FROM {tableName} {condition}";

        /// <summary>
        /// Sql 'AS' keyword
        /// </summary>
        /// <param name="variableName"></param>
        /// <returns>Query String</returns>
        public string As(string variableName) => $"AS {variableName} ";

        /// <summary>
        /// Sql 'JOIN' clause methods
        /// </summary>
        public enum JoinMethods
        {
            INNER, LEFT, RIGHT, FULL
        }

        /// <summary>
        /// Sql 'JOIN' clause
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="joinMethods"></param>
        /// <returns>Query String</returns>
        public string Join(string tableName, JoinMethods joinMethods = JoinMethods.INNER) => $"{joinMethods} JOIN {tableName} ";

        /// <summary>
        /// Sql 'ON' keyword
        /// </summary>
        /// <param name="queryParam"></param>
        /// <param name="conditionParam"></param>
        /// <param name="conditionOperation"></param>
        /// <param name="and"></param>
        /// <param name="or"></param>
        /// <returns>Query String</returns>
        public string On(string queryParam, string conditionParam, string conditionOperation = "=", bool and = false, bool or = false) =>
            $"ON {queryParam} {conditionOperation} {conditionParam} ";

        /// <summary>
        /// Sql 'Where' clause
        /// </summary>
        /// <param name="queryParam"></param>
        /// <param name="conditionParam"></param>
        /// <param name="conditionOperation"></param>
        /// <param name="and"></param>
        /// <param name="or"></param>
        /// <returns>Query String</returns>
        public string Where(string queryParam, string conditionParam, string conditionOperation = "=", bool and = false, bool or = false) =>
            $"WHERE {queryParam} {conditionOperation} {conditionParam} {(and ? "AND" : string.Empty)} {(or ? "OR" : string.Empty)} ";

        /// <summary>
        /// Sql 'ORDER BY' keyword methods
        /// </summary>
        public enum OrderByMethods
        {
            ASC, DESC
        }

        /// <summary>
        /// Sql 'ORDER BY' keyword
        /// </summary>
        /// <param name="orderByMethods"></param>
        /// <param name="conditionParams"></param>
        /// <returns></returns>
        public string OrderBy(OrderByMethods orderByMethods = OrderByMethods.ASC, params string[] conditionParams)
        {
            string _conditionParams = string.Join(", ", conditionParams);
            return $"ORDER BY {_conditionParams} {orderByMethods}";
        }

    }
}
