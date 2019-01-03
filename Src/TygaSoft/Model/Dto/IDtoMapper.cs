namespace TygaSoft.Model
{
    public interface IDtoMapper
    {
        TDestination TMapper<TDestination>(object source);

        TDestination TMapper<TDestination,TSource>() where TSource:class,new();
    }
}