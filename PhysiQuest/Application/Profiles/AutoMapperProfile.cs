using Application.Badges.DTO;
using Application.Quests.DTO;
using Application.UserBadges.DTO;
using Application.UserQuests.DTO;
using Application.Users.DTO;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<UserBadge, UserBadgeDTO>().ReverseMap();
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

            //CreateMap<CreateAndUpdateQuestDTO, Quest>()
            //    .ForMember(dest => dest.Id, opt => opt.Ignore())
            //    .ForMember(dest => dest.PropNameToIgnore, opt => opt.Ignore());
            //CreateMap<Quest, CreateAndUpdateQuestDTO>()
            //    .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            //    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));


        }
    }
}
