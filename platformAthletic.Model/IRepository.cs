using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace platformAthletic.Model
{
    public interface IRepository
    {
        IQueryable<T> GetTable<T>() where T : class;

        #region BillingInfo
        
        IQueryable<BillingInfo> BillingInfoes { get; }
        
        bool CreateBillingInfo(BillingInfo instance);

        BillingInfo UpdateBillingInfo(BillingInfo instance);

        bool CancelAutoDebit(int idBillingInfo);

        bool RemoveBillingInfo(int idBillingInfo);
        
        #endregion 

        #region Cell
        
        IQueryable<Cell> Cells { get; }
        
        bool CreateCell(Cell instance);
        
        bool UpdateCell(Cell instance);
        
        bool RemoveCell(int idCell);
        
        #endregion 

        #region Cycle

        IQueryable<Cycle> Cycles { get; }

        bool CreateCycle(Cycle instance);

        bool UpdateCycle(Cycle instance);

        bool RemoveCycle(int idCycle);

        #endregion 

        #region Day

        IQueryable<Day> Days { get; }

        bool CreateDay(Day instance);

        bool UpdateDay(Day instance);

        bool RemoveDay(int idDay);

        #endregion 

        #region Equipment
        
        IQueryable<Equipment> Equipments { get; }
        
        bool CreateEquipment(Equipment instance);
        
        bool UpdateEquipment(Equipment instance);
        
        bool RemoveEquipment(int idEquipment);
        
        #endregion 

        #region FeatureCatalog
        
        IQueryable<FeatureCatalog> FeatureCatalogs { get; }
        
        bool CreateFeatureCatalog(FeatureCatalog instance);
        
        bool UpdateFeatureCatalog(FeatureCatalog instance);
        
        bool RemoveFeatureCatalog(int idFeatureCatalog);

        bool MoveFeatureCatalog(int id, int placeBefore);
        
        #endregion 

        #region FeatureText
        
        IQueryable<FeatureText> FeatureTexts { get; }
        
        bool CreateFeatureText(FeatureText instance);
        
        bool UpdateFeatureText(FeatureText instance);
        
        bool RemoveFeatureText(int idFeatureText);

        bool ChangeParentFeatureText(int id, int idParent);

        bool MoveFeatureText(int id, int placeBefore);
        
        #endregion 

        #region FieldPosition
        
        IQueryable<FieldPosition> FieldPositions { get; }
        
        bool CreateFieldPosition(FieldPosition instance);
        
        bool UpdateFieldPosition(FieldPosition instance);
        
        bool RemoveFieldPosition(int idFieldPosition);
        
        #endregion 

        #region Invoice
        
        IQueryable<Invoice> Invoices { get; }
        
        bool CreateInvoice(Invoice instance);
        
        bool UpdateInvoice(Invoice instance);
        
        bool RemoveInvoice(int idInvoice);
        
        #endregion 

        #region Page
        
        IQueryable<Page> Pages { get; }
        
        bool CreatePage(Page instance);
        
        bool UpdatePage(Page instance);
        
        bool RemovePage(int idPage);
        
        #endregion 

        #region PaymentDetail

        IQueryable<PaymentDetail> PaymentDetails { get; }

        bool CreatePaymentDetail(PaymentDetail instance);

        bool ProcessPaymentDetail(PaymentDetail instance);

        bool RemovePaymentDetail(int idPaymentDetail);

        bool SetResultPaymentDetail(int idPaymentDetail, string result);

        #endregion 

        #region Phase

        IQueryable<Phase> Phases { get; }

        bool CreatePhase(Phase instance);

        bool UpdatePhase(Phase instance);

        bool RemovePhase(int idPhase);

        #endregion 

        #region PillarType
        
        IQueryable<PillarType> PillarTypes { get; }
        
        bool CreatePillarType(PillarType instance);
        
        bool UpdatePillarType(PillarType instance);
        
        bool RemovePillarType(int idPillarType);
        
        #endregion 

        #region PromoAction

        IQueryable<PromoAction> PromoActions { get; }

        bool CreatePromoAction(PromoAction instance);

        bool UpdatePromoAction(PromoAction instance);

        bool RemovePromoAction(int idPromoAction);

        bool ChangeStatePromoAction(int idPromoAction, bool open);

        #endregion

        #region PromoCode

        IQueryable<PromoCode> PromoCodes { get; }

        bool CreatePromoCode(PromoCode instance);

        bool UsePromoCode(string referralCode); 

        bool RemovePromoCode(int idPromoCode);

        bool GeneratePromoCodes(int promoActionId, int quantity, string referralCode);

        double GetDiscountByPromoCode(int idPromoCode, double totalPrice, PromoAction.TargetEnum target);

        bool ValidatePromoCode(string referralCode, PromoAction.TargetEnum target);

        #endregion 

        #region Role
        
        IQueryable<Role> Roles { get; }
        
        bool CreateRole(Role instance);
        
        bool RemoveRole(int idRole);
        
        #endregion 

        #region SBCValue
        
        IQueryable<SBCValue> SBCValues { get; }

        bool UpdateSbcValue(SBCValue instance);

        bool RemoveSbcValue(int idSbcValue);

        #endregion 

        #region Season
        
        IQueryable<Season> Seasons { get; }
        
        bool CreateSeason(Season instance);
        
        bool UpdateSeason(Season instance);
        
        bool RemoveSeason(int idSeason);
        
        #endregion 

        #region State
        
        IQueryable<State> States { get; }
        
        bool CreateState(State instance);
        
        bool UpdateState(State instance);
        
        bool RemoveState(int idState);
        
        #endregion 

        #region Team
        
        IQueryable<Team> Teams { get; }
        
        bool CreateTeam(Team instance);
        
        bool UpdateTeam(Team instance);

        bool UpdateTeamCount(Team instance);
        bool RemoveTeam(int idTeam);
        
        #endregion 

        #region Training
        
        IQueryable<Training> Trainings { get; }
        
        bool CreateTraining(Training instance);
        
        bool UpdateTraining(Training instance);
        
        bool RemoveTraining(int idTraining);
        
        #endregion 

        #region TrainingDay

        IQueryable<TrainingDay> TrainingDays { get; }

        bool CreateTrainingDay(TrainingDay instance);

        bool UpdateTrainingDay(TrainingDay instance);

        bool RemoveTrainingDay(int idTrainingDay);

        #endregion 

        #region TrainingEquipment
        
        IQueryable<TrainingEquipment> TrainingEquipments { get; }
        
        bool CreateTrainingEquipment(TrainingEquipment instance);
        
        bool UpdateTrainingEquipment(TrainingEquipment instance);
        
        bool RemoveTrainingEquipment(int idTrainingEquipment);
        
        #endregion 

        #region TrainingSet
        
        IQueryable<TrainingSet> TrainingSets { get; }
        
        bool CreateTrainingSet(TrainingSet instance);
        
        bool UpdateTrainingSet(TrainingSet instance);
        
        bool RemoveTrainingSet(int idTrainingSet);
        
        #endregion 

        #region User
        
        IQueryable<User> Users { get; }
        
        bool CreateUser(User instance);
        
        bool UpdateUser(User instance);

        bool CreateAdminUser(User instance);

        bool UpdateAdminUser(User instance);

        bool UpdateFullUser(User instance);

        bool UpdateManageUser(User instance);

        bool SetUserColors(User instance);

        bool UpdatePaidTillUser(User instance);

        User GetUser(string email);

        bool OnlineUser(int idUser);

        User Login(string email, string password);

        bool ActivateUser(User instance);

        bool ResendRegister(User instance);

        bool ChangePassword(User instance);

        bool SetSbcValue(int idUser, SBCValue.SbcType type, double value);

        bool RemoveUser(int idUser);

        bool ChangeGroup(User instance);

        bool SetAttendance(int idUser, bool attendance, int idUserSeason);

        bool VisitGettingStarted(int idUser);

        bool SetUserField(int idUser, User.FieldType fieldType, string value);

        bool SetFieldPosition(int idUser, int fieldPositionID);

        bool ResetAttendance(User instance);

        bool ResetProgress(User instance);

        bool SetProgressDate(User instance);
        #endregion 


        #region UserAttendance
        
        IQueryable<UserAttendance> UserAttendances { get; }
        
        bool CreateUserAttendance(UserAttendance instance);
        
        bool RemoveUserAttendance(int idUserAttendance);
        
        #endregion 

        #region UserEquipment
        
        IQueryable<UserEquipment> UserEquipments { get; }
        
        bool CreateUserEquipment(UserEquipment instance);
        
        bool UpdateUserEquipment(UserEquipment instance);
        
        bool RemoveUserEquipment(int idUserEquipment);
        
        #endregion 

        #region UserPillar
        
        IQueryable<UserPillar> UserPillars { get; }
        
        bool CreateUserPillar(UserPillar instance);
        
        bool RemoveUserPillar(int idUserPillar);
        
        #endregion 

        #region UserRole
        
        IQueryable<UserRole> UserRoles { get; }
        
        bool CreateUserRole(UserRole instance);
        
        bool RemoveUserRole(int idUserRole);
        
        #endregion 

        #region UserSeason
        
        IQueryable<UserSeason> UserSeasons { get; }
        
        bool CreateUserSeason(UserSeason instance);
        
        bool UpdateUserSeason(UserSeason instance);
        
        bool RemoveUserSeason(int idUserSeason);
        
        #endregion 

        #region Week
        
        IQueryable<Week> Weeks { get; }
        
        bool CreateWeek(Week instance);
        
        bool UpdateWeek(Week instance);
        
        bool RemoveWeek(int idWeek);
        
        #endregion 

        #region TrainingDayCell
        
        IQueryable<TrainingDayCell> TrainingDayCells { get; }
        
        bool CreateTrainingDayCell(TrainingDayCell instance);
        
        bool UpdateTrainingDayCell(TrainingDayCell instance);
        
        bool RemoveTrainingDayCell(int idTrainingDayCell);
        
        #endregion 

        #region Setting
        
        IQueryable<Setting> Settings { get; }

        void SaveSetting(Setting instance);

        bool RemoveSetting(int idSetting);
        
        #endregion 

        #region Video
        
        IQueryable<Video> Videos { get; }
        
        bool CreateVideo(Video instance);
        
        bool UpdateVideo(Video instance);
        
        bool RemoveVideo(int idVideo);
        
        #endregion 

        #region File
        
        IQueryable<File> Files { get; }
        
        bool CreateFile(File instance);
        
        bool UpdateFile(File instance);
        
        bool RemoveFile(int idFile);
        
        #endregion 

        #region Faq
        
        IQueryable<Faq> Faqs { get; }
        
        bool CreateFaq(Faq instance);
        
        bool UpdateFaq(Faq instance);
        
        bool RemoveFaq(int idFaq);

        bool MoveFaq(int id, int placeBefore);
        
        #endregion 

        #region BannerPlace
        
        IQueryable<BannerPlace> BannerPlaces { get; }
        
        bool CreateBannerPlace(BannerPlace instance);
        
        bool UpdateBannerPlace(BannerPlace instance);
        
        bool RemoveBannerPlace(int idBannerPlace);
        
        #endregion 

        #region Banner
        
        IQueryable<Banner> Banners { get; }
        
        bool CreateBanner(Banner instance);
        
        bool UpdateBanner(Banner instance);
        
        bool RemoveBanner(int idBanner);
        
        #endregion 

        #region About
        
        IQueryable<About> Abouts { get; }
        
        bool CreateAbout(About instance);
        
        bool UpdateAbout(About instance);
        
        bool RemoveAbout(int idAbout);
        
        #endregion 

        #region Feedback
        
        IQueryable<Feedback> Feedbacks { get; }
        
        bool CreateFeedback(Feedback instance);

        bool ReadFeedback(Feedback instance);
        
        bool RemoveFeedback(int idFeedback);
        
        #endregion 

        #region Gallery
        
        IQueryable<Gallery> Galleries { get; }
        
        bool CreateGallery(Gallery instance);
        
        bool UpdateGallery(Gallery instance);
        
        bool RemoveGallery(int idGallery);
        
        #endregion 

        #region Post
        
        IQueryable<Post> Posts { get; }
        
        bool CreatePost(Post instance);
        
        bool UpdatePost(Post instance);
        
        bool RemovePost(int idPost);
        
        #endregion 

        #region Aphorism
        
        IQueryable<Aphorism> Aphorisms { get; }
        
        bool CreateAphorism(Aphorism instance);
        
        bool UpdateAphorism(Aphorism instance);
        
        bool RemoveAphorism(int idAphorism);
        
        #endregion 

        #region FailedMail
        
        IQueryable<FailedMail> FailedMails { get; }
        
        bool CreateFailedMail(FailedMail instance);
        
        FailedMail PopFailedMail();
        
        bool RemoveFailedMail(int idFailedMail);
        
        #endregion 

        #region Macrocycle
        
        IQueryable<Macrocycle> Macrocycles { get; }
        
        bool CreateMacrocycle(Macrocycle instance);
        
        bool UpdateMacrocycle(Macrocycle instance);

        bool UpdateMacrocycleName(Macrocycle instance);
       // bool RemoveMacrocycle(int idMacrocycle);
        
        #endregion 

        #region Group
        
        IQueryable<Group> Groups { get; }
        
        bool CreateGroup(Group instance);
        
        bool UpdateGroup(Group instance);
        
        bool RemoveGroup(int idGroup);
        
        #endregion 

        #region Schedule
        
        IQueryable<Schedule> Schedules { get; }
        
        bool CreateSchedule(Schedule instance);
        
        bool UpdateSchedule(Schedule instance);
        
        bool RemoveSchedule(int idSchedule);

        bool ResetSchedule(int idTeam, int? groupID);
        
        #endregion 

        #region PersonalSchedule
        
        IQueryable<PersonalSchedule> PersonalSchedules { get; }
        
        bool CreatePersonalSchedule(PersonalSchedule instance);
        
        bool UpdatePersonalSchedule(PersonalSchedule instance);
        
        bool RemovePersonalSchedule(int idPersonalSchedule);

        bool ResetPersonalSchedule(int idUser);

        #endregion 

        #region PagePart
        
        IQueryable<PagePart> PageParts { get; }
        
        bool SavePagePart(PagePart instance);
        
        #endregion 

        #region PeopleSaying
        
        IQueryable<PeopleSaying> PeopleSayings { get; }
        
        bool CreatePeopleSaying(PeopleSaying instance);
        
        bool UpdatePeopleSaying(PeopleSaying instance);
        
        bool RemovePeopleSaying(int idPeopleSaying);
        
        #endregion 
    }
}