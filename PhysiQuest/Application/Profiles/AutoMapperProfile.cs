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
        }
    }
}
