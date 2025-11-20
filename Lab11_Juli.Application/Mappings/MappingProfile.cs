using AutoMapper;
using Lab11_Juli.Application.DTOs;
using Lab11_Juli.Infrastructure.Data;

namespace Lab11_Juli.Application.Mappings;

public class MappingProfile: Profile
{
  public MappingProfile()
  {
    CreateMap<Role, RoleDto>().ReverseMap();
    CreateMap<Role, RoleGetDto>().ReverseMap();
    CreateMap<Ticket, TicketDto>().ReverseMap();
    CreateMap<Ticket, CreateTicketDto>().ReverseMap();
    CreateMap<UpdateTicketDto, Ticket>().ReverseMap();
    
    CreateMap<RegisterUserDto, User>()
      .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // se genera con BCrypt
      .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now));

    CreateMap<User, AuthResponseDto>()
      .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
      .ForMember(dest => dest.Role, opt => opt.Ignore());
  }  
}