using DynamicLoopCompany.Components.Mapping.Profiles;
using AutoMapper;

namespace DynamicLoopCompany.Components.Mapping
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(configuration =>
            {
                configuration.AddProfile(new MasterMap());
                configuration.AddProfile(new HomeMap());
                configuration.AddProfile(new ContentMap());
                configuration.AddProfile(new SubMenuMap());
                configuration.AddProfile(new ContentInteractiveImageMap());
            });
        }
    }
}
