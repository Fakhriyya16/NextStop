
using AutoMapper;
using Domain.Entities;
using Service.DTOs.Accounts;
using Service.DTOs.Blogs;
using Service.DTOs.Categories;
using Service.DTOs.Cities;
using Service.DTOs.Countries;
using Service.DTOs.Places;
using Service.DTOs.Tags;

namespace Service.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Country, CountryDto>().ForMember(dest => dest.Cities, opt => opt.MapFrom(m => m.Cities.Select(m => m.Name).ToList()));
            CreateMap<CountryCreateDto, Country>();
            CreateMap<CountryEditDto, Country>();

            CreateMap<City, CityDto>().ForMember(dest => dest.Country, opt => opt.MapFrom(m => m.Country.Name))
                                      .ForMember(dest => dest.Places, opt => opt.MapFrom(m=>m.Places.Select(m=>m.Name).ToList()));
            CreateMap<CityCreateDto, City>();
            CreateMap<CityEditDto, City>();

            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryEditDto, Category>();
            CreateMap<Category, CategoryDto>().ForMember(dest => dest.Places,opt => opt.MapFrom(m=>m.Places.Select(m=>m.Name).ToList()));

            CreateMap<TagCreateDto, Tag>();
            CreateMap<TagEditDto, Tag>();
            CreateMap<Tag, TagDto>()
                 .ForMember(dest => dest.Places, opt => opt.MapFrom(src => src.PlaceTags.Select(pt => pt.Place).ToList()));
                 

            CreateMap<PlaceCreateDto, Place>().ForMember(dest => dest.Images, opt => opt.Ignore());
            CreateMap<PlaceEditDto, Place>();
            CreateMap<Place, PlaceDto>().ForMember(dest => dest.City, opt => opt.MapFrom(m => m.City.Name))
                                        .ForMember(dest => dest.Category, opt => opt.MapFrom(m => m.Category.Name))
                                        .ForMember(dest => dest.Country, opt => opt.MapFrom(m => m.City.Country.Name))
                                        .ForMember(dest => dest.Reviews, opt => opt.MapFrom(m => m.Reviews.Select(r => r.Comment).ToList()))
                                        .ForMember(dest => dest.Images, opt => opt.MapFrom(m => m.Images.Select(i => i.ImageUrl).ToList()))
                                        .ForMember(dest => dest.Tags, opt => opt.MapFrom(m => m.PlaceTags.Select(pt => pt.Tag.Name).ToList()));

            CreateMap<RegisterDto, AppUser>();
            CreateMap<LoginDto, AppUser>();
            CreateMap<UserUpdateDto, AppUser>();
            CreateMap<AppUser, UserDetailDto>().ForMember(dest => dest.Reviews, opt => opt.MapFrom(m=>m.Reviews.Select(m=>m.Comment).ToList()))
                                               .ForMember(dest => dest.Blogs, opt => opt.MapFrom(m=>m.Blogs.Select(m=>m.Title).ToList()))
                                               .ForMember(dest => dest.Favorites, opt => opt.MapFrom(m=>m.Favorites.Select(m=>m.Place.Name).ToList()))
                                               .ForMember(dest => dest.SubscriptionType, opt => opt.MapFrom(m => m.Subscription.SubscriptionType));

            CreateMap<AppUser, UserDto>().ForMember(dest => dest.SubscriptionType, opt => opt.MapFrom(m => m.Subscription.SubscriptionType));

            CreateMap<Blog, BlogDto>().ForMember(dest => dest.PublishDate, opt => opt.MapFrom(m=>m.CreatedDate.ToString("dd.MM.yyyy")))
                                      .ForMember(dest => dest.Author, opt => opt.MapFrom(m=>m.AppUser.Name + " " + m.AppUser.Surname))
                                      .ForMember(dest => dest.Images, opt => opt.MapFrom(m=>m.BlogImages.Select(m=>m.ImageUrl).ToList()));
            CreateMap<BlogCreateDto, Blog>();
            CreateMap<BlogEditDto, Blog>();
        }
    }
}
