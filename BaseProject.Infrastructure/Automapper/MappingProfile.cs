using Domain.Concrete;
using Domain.Dto;
using Profile = AutoMapper.Profile;

namespace BaseProject.Infrastructure.Automapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Like, LikeDto>().ReverseMap();
        CreateMap<Comment, CommentDto>().ReverseMap();
        CreateMap<Education, EducationDto>().ReverseMap();
        CreateMap<Follower, FollowerDto>().ReverseMap();
        CreateMap<Reply, ReplyDto>().ReverseMap();
        CreateMap<SavedPost, SavedSharingDto>().ReverseMap();
        CreateMap<Sharing, SharingDto>().ReverseMap();
        CreateMap<User, UserProfileDto>().ReverseMap();
        CreateMap<SubCategory, SubCategoryDto>().ReverseMap();
        CreateMap<Education, EducationDto>().ReverseMap();
        CreateMap<WorkExpeirence, ExperienceDto>().ReverseMap();
        CreateMap<Education, EducationDto>().ReverseMap();
    }
}