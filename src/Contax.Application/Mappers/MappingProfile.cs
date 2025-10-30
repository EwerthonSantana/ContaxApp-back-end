using AutoMapper;
using Contax.Domain.Entities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Contact, ContactDTO>().ReverseMap();
    }
}
