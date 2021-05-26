using Ardalis.SmartEnum;
using System;
using System.Reflection;
using System.Text;

namespace QueryFactory
{
    public class QueryFactory<T> : IQueryFactory where T : class
    {
        private readonly StringBuilder select;
        private readonly StringBuilder join;
        private readonly Type instanceType = typeof(T);

        public QueryFactory()
        {
            select = new StringBuilder();
            join = new StringBuilder();
        }

        public string Select(params object[] paramsToExclude)
        {
            select.Append("SELECT ");
            PropertyInfo[] props = instanceType.GetProperties();
            string entityName = instanceType.Name.ToCamelCase();
            for (int i = 0; i < props.Length; i++)
            {
                select.AppendLine($"{entityName}.{props[i].Name.ToCamelCase()} AS {props[i].Name}");
                if (i < props.Length - 1)
                    select.Append(",");
            }
            string queryy = select.ToString();
            return queryy;
        }

        public string Select<O>(params object[] paramsToExclude) where O : class
        {
            select.Append("SELECT ");
            Type localType = typeof(O);
            PropertyInfo[] props = localType.GetProperties();
            string entityName = localType.Name.ToCamelCase();
            for (int i = 0; i < props.Length; i++)
            {
                select.AppendLine($"{entityName}.{props[i].Name.ToCamelCase()} AS {props[i].Name}");
                if (i < props.Length - 1)
                    select.Append(",");
            }
            string queryy = select.ToString();
            return queryy;
        }

        public string Join<O>(params object[] paramsToExclude) where O : class
        {
            PropertyInfo[] props = typeof(O).GetProperties();
            for (int i = 0; i < props.Length; i++)
            {
                select.AppendLine($"{props[i].Name.ToCamelCase()} AS {props[i].Name}");
                if (i < props.Length - 1)
                    select.Append(",");
            }
            string queryy = select.ToString();
            return queryy;
        }

        public string Where<U>(object left, object right) where U : class
        {
            return "asd";
        }
    }

    public class JoinType : SmartEnum<JoinType>
    {
        public static readonly JoinType InnerJoin = new JoinType("INNER JOIN", 1);
        public static readonly JoinType LeftJoin = new JoinType("LEFT JOIN", 2);
        public static readonly JoinType RightJoin = new JoinType("RIGHT JOIN", 3);
        public static readonly JoinType FullJoin = new JoinType("FULL JOIN", 4);

        public JoinType(string name, int value) : base(name, value)
        {
        }
    }

    public class QueryFactoryResult
    {
        public Type InitialSource { get; set; }
        private readonly StringBuilder _select = new StringBuilder();
        private readonly StringBuilder _join = new StringBuilder();
        private readonly StringBuilder _where = new StringBuilder();
        public StringBuilder Select { get => _select; }
        public StringBuilder Join { get => _join; }
        public StringBuilder Where { get => _where; }
    }

    public static class QueryFactory
    {
        //ToDo: se o AS tiver que ser outro novo...
        private static readonly QueryFactoryResult queryFactoryResult = new QueryFactoryResult();

        public static QueryFactoryResult Select(Type source, params object[] paramsToExclude)
        {
            queryFactoryResult.InitialSource = source;
            queryFactoryResult.Select.Append("SELECT ");
            PropertyInfo[] props = source.GetProperties();
            string entityName = source.Name.ToCamelCase();
            for (int i = 0; i < props.Length; i++)
            {
                queryFactoryResult.Select.AppendLine($" {entityName}.{props[i].Name.ToCamelCase()} AS {props[i].Name} ");
                if (i < props.Length - 1)
                    queryFactoryResult.Select.Append(",");
            }
            string queryy = queryFactoryResult.Select.ToString();
            return queryFactoryResult;
        }

        public static QueryFactoryResult Join<T>(this T item, JoinType joinType, Type entity, string key, bool addToSelectParams)
        {
            string entityName = entity.Name.ToCamelCase();
            if (addToSelectParams)
            {
                PropertyInfo[] props = entity.GetProperties();
                queryFactoryResult.Select.Append(",");
                for (int i = 0; i < props.Length; i++)
                {
                    queryFactoryResult.Select.AppendLine($" {entityName}.{props[i].Name.ToCamelCase()} AS {props[i].Name} ");
                    if (i < props.Length - 1)
                        queryFactoryResult.Select.Append(",");
                }
            }
            queryFactoryResult.Join.Append($"{joinType.Name} {entityName} ON {entityName}.{key}");
            string queryy = queryFactoryResult.Select.ToString();
            string joinn = queryFactoryResult.Join.ToString();
            return queryFactoryResult;
        }

        public static QueryFactoryResult On<T>(this T item, string entityName, string key)
        {
            queryFactoryResult.Join.AppendLine($" = {entityName}.{key} ");
            string queryy = queryFactoryResult.Select.ToString();
            string joinn = queryFactoryResult.Join.ToString();
            return queryFactoryResult;
        }

        public static QueryFactoryResult Where<T>(this T item, string condition)
        {
            queryFactoryResult.Where.Append($" WHERE {condition} ");
            return queryFactoryResult;
        }

        public static QueryFactoryResult And<T>(this T item, string condition)
        {
            queryFactoryResult.Where.Append($" AND {condition} ");
            return queryFactoryResult;
        }

        public static QueryFactoryResult Or<T>(this T item, string condition)
        {
            queryFactoryResult.Where.Append($" OR {condition} ");
            return queryFactoryResult;
        }

        public static string Create<T>(this T item)
        {
            return @$"{queryFactoryResult.Select} 
                    FROM {queryFactoryResult.InitialSource.Name.ToCamelCase()}
                         {queryFactoryResult.Join}
                         {queryFactoryResult.Where}";
        }
    }
}
