using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using platformAthletic.Model;
using platformAthletic.Models.ViewModels;
using platformAthletic.Models.ViewModels.User;
using platformAthletic.Helpers;

namespace platformAthletic.Mappers
{
    public static class MapperCollection
    {
        public static class LoginUserMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<User, LoginViewModel>();
                Mapper.CreateMap<LoginViewModel, User>();
            }
        }

        public static class UserMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<User, AdminUserView>()
                    .ForMember(dest => dest.ConfirmPassword, opt => opt.MapFrom(p => p.Password));

                Mapper.CreateMap<AdminUserView, User>();

                Mapper.CreateMap<User, UserView>();
                Mapper.CreateMap<UserView, User>();

                Mapper.CreateMap<User, TeamUserView>()
                    .ForMember(dest => dest.Team, opt => opt.MapFrom(p => p.OwnTeam))
                    .ForMember(dest => dest.FullName, opt => opt.MapFrom(p => p.FirstName));

                Mapper.CreateMap<TeamUserView, User>()
                    .ForMember(dest => dest.Team, opt => opt.Ignore())
                    .ForMember(dest => dest.FirstName, opt => opt.MapFrom(p => p.FullName));

                Mapper.CreateMap<User, IndividualUserView>()
                    .ForMember(dest => dest.PrimaryColor, opt => opt.MapFrom(p => p.RealPrimaryColor))
                    .ForMember(dest => dest.SecondaryColor, opt => opt.MapFrom(p => p.RealSecondaryColor));

                Mapper.CreateMap<IndividualUserView, User>()
                    .ForMember(dest => dest.PrimaryColor, opt => opt.MapFrom(p => p.PrimaryColor))
                    .ForMember(dest => dest.SecondaryColor, opt => opt.MapFrom(p => p.SecondaryColor));

                Mapper.CreateMap<RegisterIndividualView, User>()
                    .ForMember(dest => dest.BillingInfo, opt => opt.MapFrom(p => p.BillingInfo))
                    .ForMember(dest => dest.PlayerOfTeamID, opt => opt.Ignore());

                Mapper.CreateMap<RegisterTeamView, User>()
                    .ForMember(dest => dest.Team, opt => opt.Ignore())
                    .ForMember(dest => dest.PlayerOfTeamID, opt => opt.Ignore());


                Mapper.CreateMap<User, TeamBillingUserView>()
                    .ForMember(dest => dest.BillingInfo, opt => opt.MapFrom(p => p.BillingInfo))
                    .ForMember(dest => dest.Invoice, opt => opt.MapFrom(p => p.Invoice))
                    .AfterMap((src, dest) =>
                    {
                        if (dest.BillingInfo == null)
                        {
                            dest.BillingInfo = new BillingInfoView()
                            {
                                UserID = src.ID
                            };
                        }
                    })
                    .AfterMap((src, dest) =>
                    {
                        if (dest.Invoice == null)
                        {
                            dest.Invoice = new InvoiceView()
                            {
                                UserID = src.ID
                            };
                        }
                    });

                Mapper.CreateMap<User, IndividualBillingUserView>()
                    .ForMember(dest => dest.BillingInfo, opt => opt.MapFrom(p => p.BillingInfo))
                     .AfterMap((src, dest) =>
                    {
                        if (dest.BillingInfo == null)
                        {
                            dest.BillingInfo = new BillingInfoView()
                            {
                                UserID = src.ID
                            };
                        }
                    });
                Mapper.CreateMap<User, PlayerUserView>();
                Mapper.CreateMap<PlayerUserView, User>();

                Mapper.CreateMap<User, PlayerView>();
                Mapper.CreateMap<PlayerView, User>();

                Mapper.CreateMap<User, UserInfoView>()
                    .ForMember(p => p.Birthday, opt => opt.MapFrom(p => p.Birthday ?? DateTime.Now.Current()))
                    .ForMember(p => p.Sports, opt => opt.MapFrom(p => p.UserFieldPositions.Select(r => new UserInfoView.SportInfo()
                    {
                        FieldPositionID = r.FieldPositionID,
                        SportID = r.SportID
                    })))
                     .AfterMap((src, dest) =>
                     {
                         while (dest.Sports.Count < 3)
                         {
                             dest.Sports.Add(new UserInfoView.SportInfo());
                         };
                     });
                Mapper.CreateMap<UserInfoView, User>();
            }
        }

        
        public static class BillingInfoMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<BillingInfo, BillingInfoView>()
                    .ForMember(dest => dest.ExpirationMonth, opt => opt.MapFrom(p => p.ExparationDate.Month))
                    .ForMember(dest => dest.ExpirationYear, opt => opt.MapFrom(p => p.ExparationDate.Year))
                    .ForMember(dest => dest.CVC, opt => opt.Ignore());

                Mapper.CreateMap<BillingInfoView, BillingInfo>()
                    .ForMember(dest => dest.ExparationDate, opt => opt.MapFrom(p => new DateTime(p.ExpirationYear, p.ExpirationMonth, 1)));
        	}
        }

        
        public static class CellMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Cell, CellView>();
        		Mapper.CreateMap<CellView, Cell>();
        	}
        }
        
        public static class EquipmentMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Equipment, EquipmentView>();
        		Mapper.CreateMap<EquipmentView, Equipment>();
        	}
        }

        
        public static class FeatureCatalogMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<FeatureCatalog, FeatureCatalogView>();
        		Mapper.CreateMap<FeatureCatalogView, FeatureCatalog>();
        	}
        }

        
        public static class FeatureTextMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<FeatureText, FeatureTextView>();
        		Mapper.CreateMap<FeatureTextView, FeatureText>();
        	}
        }

        
        public static class FieldPositionMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<FieldPosition, FieldPositionView>();
        		Mapper.CreateMap<FieldPositionView, FieldPosition>();
        	}
        }

        
        public static class InvoiceMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Invoice, InvoiceView>();
        		Mapper.CreateMap<InvoiceView, Invoice>();
        	}
        }

        
        public static class PageMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Page, PageView>();
        		Mapper.CreateMap<PageView, Page>();
        	}
        }

        
        public static class PaymentDetailMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<PaymentDetail, PaymentDetailView>();
        		Mapper.CreateMap<PaymentDetailView, PaymentDetail>();
        	}
        }

        
        public static class PillarTypeMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<PillarType, PillarTypeView>();
        		Mapper.CreateMap<PillarTypeView, PillarType>();
        	}
        }

        
        public static class PromoActionMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<PromoAction, PromoActionView>();
        		Mapper.CreateMap<PromoActionView, PromoAction>();
        	}
        }

        
        public static class PromoCodeMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<PromoCode, PromoCodeView>();
        		Mapper.CreateMap<PromoCodeView, PromoCode>();
        	}
        }

        
        public static class RoleMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Role, RoleView>();
        		Mapper.CreateMap<RoleView, Role>();
        	}
        }

        
        public static class SBCValueMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<SBCValue, SBCValueView>();
        		Mapper.CreateMap<SBCValueView, SBCValue>();
        	}
        }

        
        public static class SeasonMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Season, SeasonView>();
        		Mapper.CreateMap<SeasonView, Season>();
        	}
        }

        
        public static class StateMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<State, StateView>();
        		Mapper.CreateMap<StateView, State>();
        	}
        }

        
        public static class TeamMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Team, TeamView>();
        		Mapper.CreateMap<TeamView, Team>();
        	}
        }

        
        public static class TrainingMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Training, TrainingView>();
        		Mapper.CreateMap<TrainingView, Training>();
        	}
        }

        
        public static class TrainingDayMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<TrainingDay, TrainingDayView>();
        		Mapper.CreateMap<TrainingDayView, TrainingDay>();
        	}
        }
        
        public static class TrainingSetMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<TrainingSet, TrainingSetView>();
        		Mapper.CreateMap<TrainingSetView, TrainingSet>();
        	}
        }

        
        public static class UserAttendanceMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<UserAttendance, UserAttendanceView>();
        		Mapper.CreateMap<UserAttendanceView, UserAttendance>();
        	}
        }

        
        public static class UserEquipmentMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<UserEquipment, UserEquipmentView>();
        		Mapper.CreateMap<UserEquipmentView, UserEquipment>();
        	}
        }

        
        public static class UserPillarMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<UserPillar, UserPillarView>();
        		Mapper.CreateMap<UserPillarView, UserPillar>();
        	}
        }

        
        public static class UserRoleMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<UserRole, UserRoleView>();
        		Mapper.CreateMap<UserRoleView, UserRole>();
        	}
        }

        
        public static class UserSeasonMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<UserSeason, UserSeasonView>();
        		Mapper.CreateMap<UserSeasonView, UserSeason>();
        	}
        }

        
        public static class CycleMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Cycle, CycleView>();
        		Mapper.CreateMap<CycleView, Cycle>();
        	}
        }

        
        public static class PhaseMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Phase, PhaseView>();
        		Mapper.CreateMap<PhaseView, Phase>();
        	}
        }

        
        public static class WeekMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Week, WeekView>();
        		Mapper.CreateMap<WeekView, Week>();
        	}
        }

        
        public static class TrainingEquipmentMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<TrainingEquipment, TrainingEquipmentView>();
        		Mapper.CreateMap<TrainingEquipmentView, TrainingEquipment>();
        	}
        }

        
        public static class DayMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Day, DayView>();
        		Mapper.CreateMap<DayView, Day>();
        	}
        }

        
        public static class TrainingDayCellMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<TrainingDayCell, TrainingDayCellView>();
        		Mapper.CreateMap<TrainingDayCellView, TrainingDayCell>();
        	}
        }

        
        public static class VideoMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Video, VideoView>();
        		Mapper.CreateMap<VideoView, Video>();
        	}
        }

        
        public static class FileMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<File, FileView>();
        		Mapper.CreateMap<FileView, File>();
        	}
        }

        
        public static class FaqMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Faq, FaqView>();
        		Mapper.CreateMap<FaqView, Faq>();
        	}
        }

        
        public static class BannerPlaceMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<BannerPlace, BannerPlaceView>();
        		Mapper.CreateMap<BannerPlaceView, BannerPlace>();
        	}
        }

        
        public static class BannerMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Banner, BannerView>();
        		Mapper.CreateMap<BannerView, Banner>();
        	}
        }

        
        public static class FeedbackMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Feedback, FeedbackView>();
        		Mapper.CreateMap<FeedbackView, Feedback>();
        	}
        }

        
        public static class AboutMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<About, AboutView>();
        		Mapper.CreateMap<AboutView, About>();
        	}
        }

        
        public static class GalleryMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Gallery, GalleryView>();
        		Mapper.CreateMap<GalleryView, Gallery>();
        	}
        }

        
        public static class PostMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Post, PostView>();
        		Mapper.CreateMap<PostView, Post>();
        	}
        }

        
        public static class AphorismMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Aphorism, AphorismView>();
        		Mapper.CreateMap<AphorismView, Aphorism>();
        	}
        }

        
        public static class MacrocycleMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Macrocycle, MacrocycleView>();
        		Mapper.CreateMap<MacrocycleView, Macrocycle>();
        	}
        }

        
        public static class GroupMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Group, GroupView>();
        		Mapper.CreateMap<GroupView, Group>();
        	}
        }

        
        public static class PeopleSayingMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<PeopleSaying, PeopleSayingView>();
        		Mapper.CreateMap<PeopleSayingView, PeopleSaying>();
        	}
        }


        public static class LevelMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<Level, LevelView>();
                Mapper.CreateMap<LevelView, Level>();
            }
        }

        public static class SportMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<Sport, SportView>();
                Mapper.CreateMap<SportView, Sport>();
            }
        }

        public static class UserVideoMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<UserVideo, UserVideoView>();
                Mapper.CreateMap<UserVideoView, UserVideo>();
            }
        }
    }
}