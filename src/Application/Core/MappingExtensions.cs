using AutoMapper;

namespace Application.Core
{
    public static class MappingExtensions
    {
        public static IMappingExpression<TSource, TDestination> MapOnlyIfChanged<TSource, TDestination>(this IMappingExpression<TSource, TDestination> map)
        {
            try {
                map.ForAllMembers(source =>
                {
                    source.Condition((sourceObject, destObject, sourceProperty, destProperty) =>
                    {
                        if (sourceProperty == null)
                            return !(destProperty == null);
                        return !sourceProperty.Equals(destProperty);
                    });
                });
            }
            catch (Exception ex) {

                var a = ex;
                    }
            
            return map;
        }
    }
}
