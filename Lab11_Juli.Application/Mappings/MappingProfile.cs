using AutoMapper;
using Lab11_Juli.Application.DTOs;
using Lab11_Juli.Infrastructure.Data;

namespace Lab11_Juli.Application.Mappings;

public class MappingProfile: Profile
{
  public MappingProfile()
  {
    CreateMap<RoleDto, Role>().ReverseMap();
  }  
}