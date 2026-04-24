using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data.DTOs;
using api.Data.Models;
using AutoMapper;

namespace api.Data.Mappers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            _ = CreateMap<ProductModel, ProductCreateDto>().ReverseMap();

            _ = CreateMap<ProductModel, ProductDto>().ReverseMap();
        }
    }
}
