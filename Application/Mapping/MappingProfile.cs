using Application.DTOs.Producto;
using AutoMapper;
using Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            //DTO Producto a ProductoVerDTO

            CreateMap<Producto, ProductoVerDTO>()
                .ForMember(dto => dto.id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dto => dto.name, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dto => dto.image, opt => opt.MapFrom(src => src.Imagen));

            //DTO ProductoVerDTO a Producto
            CreateMap<ProductoCrearDTO, Producto>()
              .ForMember(dto => dto.Nombre, opt => opt.MapFrom(src => src.name))
              .ForMember(dto => dto.Imagen, opt => opt.MapFrom(src => src.image));
        }
    }
}
