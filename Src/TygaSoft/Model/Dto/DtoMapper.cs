using AutoMapper;

namespace TygaSoft.Model
{
    public class DtoMapper:IDtoMapper
    {
        private readonly IMapper _mapper;
        public DtoMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDestination TMapper<TDestination,TSource>() where TSource:class,new()
        {
            return _mapper.Map<TDestination>(typeof(TSource));
        }

        public TDestination TMapper<TDestination>(object source)
        {
            return _mapper.Map<TDestination>(source);
        }
    }
}