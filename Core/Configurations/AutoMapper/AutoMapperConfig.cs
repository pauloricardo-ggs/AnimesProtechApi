using AutoMapper;
using Core.Configurations.AutoMapper;

namespace Core.Configurations;

public class AutoMapperConfig
{
    public static MapperConfiguration RegisterMappings()
    {
        return new MapperConfiguration(autoMapperConfig =>
        {
            autoMapperConfig.AddProfile(new DtoToCommandMappingProfile());
            autoMapperConfig.AddProfile(new EntityToDtoMappingProfile());
        });
    }
}