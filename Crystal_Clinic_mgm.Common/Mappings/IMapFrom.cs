using AutoMapper;

namespace Crystal_Clinic_Mgm.Common.Mappings;

public interface IMapFrom<T>
{
    void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
}
