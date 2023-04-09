using Application.Badges.DTO;
using Application.Quests.DTO;
using Application.UserBadges.DTO;
using Application.UserQuests.DTO;
using Application.Users.DTO;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            //CreateMap<UserBadge, UserBadgeDTO>().ReverseMap();
            CreateMap<UserQuest, UserQuestDTO>().ReverseMap();
            CreateMap<Badge, BadgeDTO>().ReverseMap();
            CreateMap<Quest, QuestDTO>().ReverseMap();
            CreateMap<Quest, CreateAndUpdateQuestDTO>().ReverseMap();
            CreateMap<User, UserWithBadgesDTO>().IncludeBase<User, UserDTO>().ReverseMap();
            CreateMap<User, UserWithQuestsDTO>().IncludeBase<User, UserDTO>().ReverseMap();

            CreateMap<CreateAndUpdateDTO, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Points, opt => opt.Ignore())
                .ForMember(dest => dest.Tokens, opt => opt.Ignore());
            CreateMap<User, CreateAndUpdateDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<UserBadge, UserBadgeDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.BadgeName, opt => opt.MapFrom(src => src.Badge.Name));

            CreateMap<CreateAndUpdateBadgeDTO, Badge>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
