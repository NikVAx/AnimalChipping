using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using WebApi.DTOs.Account;
using WebApi.DTOs.Animal;
using WebApi.DTOs.AnimalType;
using WebApi.DTOs.LocationPoint;
using WebApi.DTOs.VisitedLocationPoint;

namespace WebApi.AutoMapper
{
    public class DtoMappingProfile : Profile
    {
        public DtoMappingProfile()
        {
            CreateMap<Animal, GetAnimalDto>()
                .ForMember(dest => dest.AnimalTypes, opts =>
                    opts.MapFrom(src => src.AnimalTypes.Select(x => x.Id)))
                .ForMember(dest => dest.VisitedLocations, opts =>
                    opts.MapFrom(src => src.VisitedLocations.Select(x => x.Id)));
            CreateMap<CreateAnimalDto, Animal>();
            CreateMap<AnimalFilterDto, AnimalFilter>();

            CreateMap<LocationFilterDto, LocationFilter>();

            CreateMap<LocationPoint, GetLocationPointDto>();
            CreateMap<CreateLocationPointDto, LocationPoint>();
            
            CreateMap<AnimalType, GetAnimalTypeDto>();
            CreateMap<CreateAnimalTypeDto, AnimalType>();

            CreateMap<AccountFilterDto, AccountFilter>();
            CreateMap<Account, GetAccountDto>();
            CreateMap<RegisterAccountDto, Account>();
        }
        
    }
}
