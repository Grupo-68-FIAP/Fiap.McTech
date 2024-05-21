using AutoMapper;
using Fiap.McTech.Application.Dtos.Clients;
using Fiap.McTech.Domain.Entities.Clients;

namespace Fiap.McTech.CrossCutting.Ioc.Mappers.Profiles
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<Client, ClientOutputDto>();
            CreateMap<ClientInputDto, Client>()
                .ForMember(dest => dest.Cpf, opt => opt.MapFrom(src => new Domain.ValuesObjects.Cpf(src.Cpf)))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => new Domain.ValuesObjects.Email(src.Email)));
        }
    }
}
