using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using platformAthletic.Model;
using platformAthletic.Models.ViewModels;


namespace platformAthletic.Mappers
{
    public class CommonMapper : IMapper
    {
        static CommonMapper()
        {
            MapperCollection.LoginUserMapper.Init();
            MapperCollection.UserMapper.Init();
            MapperCollection.BillingInfoMapper.Init();
            MapperCollection.CellMapper.Init();
            MapperCollection.EquipmentMapper.Init();
            MapperCollection.FeatureCatalogMapper.Init();
            MapperCollection.FeatureTextMapper.Init();
            MapperCollection.FieldPositionMapper.Init();
            MapperCollection.InvoiceMapper.Init();
            MapperCollection.PageMapper.Init();
            MapperCollection.PaymentDetailMapper.Init();
            MapperCollection.PillarTypeMapper.Init();
            MapperCollection.PromoActionMapper.Init();
            MapperCollection.PromoCodeMapper.Init();
            MapperCollection.RoleMapper.Init();
            MapperCollection.SBCValueMapper.Init();
            MapperCollection.SeasonMapper.Init();
            MapperCollection.StateMapper.Init();
            MapperCollection.TeamMapper.Init();
            MapperCollection.TrainingMapper.Init();
            MapperCollection.TrainingDayMapper.Init();
            MapperCollection.TrainingSetMapper.Init();
            MapperCollection.UserAttendanceMapper.Init();
            MapperCollection.UserEquipmentMapper.Init();
            MapperCollection.UserPillarMapper.Init();
            MapperCollection.UserRoleMapper.Init();
            MapperCollection.UserSeasonMapper.Init();
            MapperCollection.CycleMapper.Init();
            MapperCollection.PhaseMapper.Init();
            MapperCollection.WeekMapper.Init();
            MapperCollection.TrainingEquipmentMapper.Init();
            MapperCollection.DayMapper.Init();
            MapperCollection.TrainingDayCellMapper.Init();
            MapperCollection.VideoMapper.Init();
            MapperCollection.FileMapper.Init();
            MapperCollection.FaqMapper.Init();
            MapperCollection.BannerPlaceMapper.Init();
            MapperCollection.BannerMapper.Init();
            MapperCollection.FeedbackMapper.Init();
            MapperCollection.AboutMapper.Init();
            MapperCollection.GalleryMapper.Init();
            MapperCollection.PostMapper.Init();
            MapperCollection.AphorismMapper.Init();
            MapperCollection.MacrocycleMapper.Init();
            MapperCollection.GroupMapper.Init();
            MapperCollection.PeopleSayingMapper.Init();
            MapperCollection.LevelMapper.Init();
            MapperCollection.SportMapper.Init();
            MapperCollection.UserVideoMapper.Init();
        }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            return Mapper.Map(source, sourceType, destinationType);
        }
    }
}