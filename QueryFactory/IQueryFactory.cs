namespace QueryFactory
{
    public interface IQueryFactory
    {
        string Select(params object[] paramsToExclude);
        string Select<T>(params object[] paramsToExclude) where T : class;
        string Where<T>(object left, object right) where T : class;
        string Join<O>(params object[] paramsToExclude) where O : class;
    }
}
