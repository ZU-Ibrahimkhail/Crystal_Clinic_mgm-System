using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Common.Constants;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using System.Data;
using System.Globalization;
using System.Linq.Expressions;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Microsoft.AspNetCore.Http;

namespace Crystal_Clinic_Mgm.Application.Common.Services.Repositories
{
    #region Double Entity Repository implementation
    // double Entity generic dashboard repository
    public class GenericDashboardRepository<TMainEntity, TSubEntity> : IGenericDashboardRepository<TMainEntity, TSubEntity> where TMainEntity : AuditableEntity where TSubEntity : LookAndAuditableEntity
    {
        private readonly ERP_DbContext _DbContext;
        private readonly IStringLocalizer<CommonValidationResource> _localizer;
        readonly Localization Localize;
        private readonly DateTime CurrentDate = DateTime.Now;
        private readonly DateTime WeekStartDate = GeneralHelper.WeekStartDate();
        private DateTime HijriYearStartDate = new PersianCalendar().ToDateTime(new PersianCalendar().GetYear(DateTime.Now), 1, 1, 0, 0, 0, 0);
        private DateTime HijriMonthStartDate = new PersianCalendar().ToDateTime(new PersianCalendar().GetYear(DateTime.Now), new PersianCalendar().GetMonth(DateTime.Now), 1, 0, 0, 0, 0);
        public GenericDashboardRepository(ERP_DbContext dbContext, IStringLocalizer<CommonValidationResource> localizer, IHttpContextAccessor httpContextAccessor)
        {
            _DbContext = dbContext;
            _localizer = localizer;
            Localize = new(httpContextAccessor);

        }
        public async Task<List<BarChartDashboard>?> GetBarChartData(List<Expression<Func<TMainEntity, bool>>> expressionsForFirstStructure, List<string> FirstStructureNameList, CancellationToken cancellationToken, int? mainModule = null, int? subModule = null)
        {
            #region Determining dbcontext for TMainEntity and TSubEntity
            // it's determined by campairing mainModule and subModule to constants application modules
            DbContext mainContext = GetRequiredDbContext(mainModule);
            DbContext subContext = GetRequiredDbContext(subModule);
            #endregion
            #region initializing Variables 
            // data that would be returned
            List<BarChartDashboard> Chart = new();
            // current date for performance
            var CurrentDate = DateTime.Now;
            // listing the Sub Entity
            var Subentity = await subContext.Set<TSubEntity>().Where(x => x.IsDeleted == false).ToListAsync(cancellationToken);
            // foriegn key column name in data base
            string propertyName = GetEntityRelational_FK_PropertyName<TMainEntity, TSubEntity>(mainModule);
            // var for count non null data
            int NotEmptyDataCount = 0;
            #endregion
            #region Date loop for list of DashboardModel
            // loop for data in current day , week, month and year
            for (short i = 0; i < 4; i++)
            {
                // creating a new DashboardModel for adding in Chart list
                var dashboardModel = new BarChartDashboard
                {
                    StructureName = FirstStructureNameList,
                    Data = new(),
                    GroupBy = i switch
                    {
                        0 => _localizer["Day"],
                        1 => _localizer["Week"],
                        2 => _localizer["Month"],
                        _ => _localizer["Year"],
                    },
                    Value = i
                };
                #region Loop for TSubEntity 
                // loop of all Subentity list for comparission of TMainEntity with TSubEntity
                foreach (var entity in Subentity)
                {
                    BarChartData dashboradData = new()
                    {
                        // geting TSubEntity localized name
                        Name = Localize.GetName(entity),
                        Data = new List<int>()
                    };
                    // query for compairing TMainEntity with TSubEntity on MainEntityRelationalPropertyName is equal to ID of TSubEntity
                    // MainEntityRelationalPropertyName is and aurgument that is passed to this method
                    var EntityForCount = mainContext.Set<TMainEntity>().Where(x => x.IsDeleted == false && EF.Property<int>(x, propertyName) == entity.ID);
                    #region Loop of first structure expressions 
                    // loop of all first structure expressions for counting data based on the expression and the date loop (current day , week, month and year)
                    foreach (var expression in expressionsForFirstStructure)
                    {
                        dashboradData.Data.Add(
                        i switch
                        {
                            0 => EntityForCount.Where(expression).Count(x => x.CreatedOn.Date == CurrentDate.Date),
                            1 => EntityForCount.Where(expression).Count(x => x.CreatedOn.Date >= WeekStartDate.Date && x.CreatedOn.Date <= CurrentDate.Date),
                            2 => EntityForCount.Where(expression).Count(x => x.CreatedOn.Year == CurrentDate.Year && x.CreatedOn.Month == CurrentDate.Month),
                            _ => EntityForCount.Where(expression).Count(x => x.CreatedOn.Year == CurrentDate.Year)
                        });
                    }
                    #endregion
                    // adding data to not null count 
                    NotEmptyDataCount += dashboradData.Data.Sum(x => Convert.ToInt32(x));
                    // adding the proccessed dashboradData to dashboardModel
                    dashboardModel.Data.Add(dashboradData);
                }
                #endregion
                // addin the dashboardModel to the list (Chart)
                Chart.Add(dashboardModel);
            }
            #endregion
            if (NotEmptyDataCount == 0)
            {
                return [];
            }
            return Chart;
        }
        public async Task<List<BarChartDashboard>?> GetBarChartData(string language, List<Expression<Func<TMainEntity, bool>>> expressionsForFirstStructure, List<string> FirstStructureNameList, List<int> SubEntityIds, CancellationToken cancellationToken, int? mainModule = null, int? subModule = null)
        {
            #region Determining dbcontext for TMainEntity and TSubEntity
            // it's determined by campairing mainModule and subModule to constants application modules
            DbContext mainContext = GetRequiredDbContext(mainModule);
            DbContext subContext = GetRequiredDbContext(subModule);
            #endregion
            #region initializing Variables 
            // finding the foreign key column name in TMainEntity related to TSubEntity
            string propertyName = GetEntityRelational_FK_PropertyName<TMainEntity, TSubEntity>(mainModule);
            // data that would be returned
            List<BarChartDashboard> Chart = new();
            // listing the Sub Entity
            var Subentity = await subContext.Set<TSubEntity>().Where(x => x.IsDeleted == false && SubEntityIds.Contains(x.ID)).ToListAsync(cancellationToken);
            // var for count non null data
            int NotEmptyDataCount = 0;
            #endregion                                             
            #region Date loop for list of DashboardModel
            // loop for data in current day , week, month and year
            for (short i = 0; i < 4; i++)
            {
                // creating a new DashboardModel for adding in Chart list
                var dashboardModel = new BarChartDashboard
                {
                    StructureName = FirstStructureNameList,
                    Data = new(),
                    GroupBy = i switch
                    {
                        0 => _localizer["Day"],
                        1 => _localizer["Week"],
                        2 => _localizer["Month"],
                        _ => _localizer["Year"],
                    },
                    Value = i
                };
                #region Loop for TSubEntity 
                // loop of all Subentity list for comparission of TMainEntity with TSubEntity
                foreach (var entity in Subentity)
                {
                    BarChartData dashboradData = new()
                    {
                        // geting TSubEntity localized name
                        Name = Localize.GetName(language, entity),
                        Data = new List<int>()
                    };
                    // query for compairing TMainEntity with TSubEntity on MainEntityRelationalPropertyName is equal to ID of TSubEntity
                    var EntityForCount = mainContext.Set<TMainEntity>().Where(x => x.IsDeleted == false && EF.Property<int>(x, propertyName) == entity.ID);
                    #region Loop of first structure expressions 
                    // loop of all first structure expressions for counting data based on the expression and the date loop (current day , week, month and year)
                    foreach (var expression in expressionsForFirstStructure)
                    {
                        dashboradData.Data.Add(
                        i switch
                        {
                            0 => EntityForCount.Where(expression).Count(x => x.CreatedOn.Date == CurrentDate.Date),
                            1 => EntityForCount.Where(expression).Count(x => x.CreatedOn.Date >= WeekStartDate.Date && x.CreatedOn.Date <= CurrentDate.Date),
                            2 => EntityForCount.Where(expression).Count(x => x.CreatedOn.Year == CurrentDate.Year && x.CreatedOn.Month == CurrentDate.Month),
                            _ => EntityForCount.Where(expression).Count(x => x.CreatedOn.Year == CurrentDate.Year)
                        });
                    }
                    #endregion
                    // adding data to not null count 
                    NotEmptyDataCount += dashboradData.Data.Sum(x => Convert.ToInt32(x));
                    // adding the proccessed dashboradData to dashboardModel
                    dashboardModel.Data.Add(dashboradData);
                }
                #endregion
                // addin the dashboardModel to the list (Chart)
                Chart.Add(dashboardModel);
            }
            #endregion
            if (NotEmptyDataCount == 0)
            {
                return [];
            }
            return Chart;
        }
        public async Task<List<BarChartDashboard>?> GetBarChartData(string language, List<Expression<Func<TMainEntity, bool>>> expressionsForFirstStructure, List<Expression<Func<TMainEntity, bool>>> expressionsForSubEntity, List<string> FirstStructureNameList, CancellationToken cancellationToken, int? mainModule = null, int? subModule = null)
        {
            #region Determining dbcontext for TMainEntity and TSubEntity
            // it's determined by campairing mainModule and subModule to constants application modules
            DbContext mainContext = GetRequiredDbContext(mainModule);
            DbContext subContext = GetRequiredDbContext(subModule);
            #endregion
            #region initializing Variables 
            // data that would be returned
            List<BarChartDashboard> Chart = new();
            // listing the Sub Entity
            var Subentity = await subContext.Set<TSubEntity>().Where(x => x.IsDeleted == false).ToListAsync(cancellationToken);
            // var for count non null data
            int NotEmptyDataCount = 0;
            #endregion
            #region Date loop for list of DashboardModel
            // loop for data in current day , week, month and year
            for (short i = 0; i < 4; i++)
            {
                // creating a new DashboardModel for adding in Chart list
                var dashboardModel = new BarChartDashboard
                {
                    StructureName = FirstStructureNameList,
                    Data = new(),
                    GroupBy = i switch
                    {
                        0 => _localizer["Day"],
                        1 => _localizer["Week"],
                        2 => _localizer["Month"],
                        _ => _localizer["Year"],
                    },
                    Value = i
                };
                #region Loop for TSubEntity 
                // loop of all Subentity list for comparission of TMainEntity with TSubEntity
                foreach (var entity in expressionsForSubEntity)
                {
                    BarChartData dashboradData = new()
                    {
                        // geting TSubEntity localized name
                        Name = Localize.GetName(language, Subentity[expressionsForSubEntity.IndexOf(entity)]),
                        Data = new List<int>()
                    };
                    // implementing the exprission for subentities
                    var EntityForCount = mainContext.Set<TMainEntity>().Where(entity);
                    #region Loop of first structure expressions 
                    // loop of all first structure expressions for counting data based on the expression and the date loop (current day , week, month and year)
                    foreach (var expression in expressionsForFirstStructure)
                    {
                        dashboradData.Data.Add(
                        i switch
                        {
                            0 => EntityForCount.Where(expression).Count(x => x.CreatedOn.Date == CurrentDate.Date),
                            1 => EntityForCount.Where(expression).Count(x => x.CreatedOn.Date >= WeekStartDate.Date && x.CreatedOn.Date <= CurrentDate.Date),
                            2 => EntityForCount.Where(expression).Count(x => x.CreatedOn.Year == CurrentDate.Year && x.CreatedOn.Month == CurrentDate.Month),
                            _ => EntityForCount.Where(expression).Count(x => x.CreatedOn.Year == CurrentDate.Year)
                        });
                    }
                    #endregion
                    // adding data to not null count 
                    NotEmptyDataCount += dashboradData.Data.Sum(x => Convert.ToInt32(x));
                    // adding the proccessed dashboradData to dashboardModel
                    dashboardModel.Data.Add(dashboradData);
                }
                #endregion
                // addin the dashboardModel to the list (Chart)
                Chart.Add(dashboardModel);
            }
            #endregion
            if (NotEmptyDataCount == 0)
            {
                return [];
            }
            return Chart;
        }
        public async Task<List<BarChartDashboard>?> GetBarChartData(string language, List<Expression<Func<TMainEntity, bool>>> expressionsForFirstStructure, List<Expression<Func<TMainEntity, bool>>> expressionsForSubEntity, List<int> SubEntityIds, List<string> FirstStructureNameList, CancellationToken cancellationToken, int? mainModule = null, int? subModule = null)
        {
            #region Determining dbcontext for TMainEntity and TSubEntity
            // it's determined by campairing mainModule and subModule to constants application modules
            DbContext mainContext = GetRequiredDbContext(mainModule);
            DbContext subContext = GetRequiredDbContext(subModule);
            #endregion
            #region initializing Variables 
            // data that would be returned
            List<BarChartDashboard> Chart = new();
            // listing the Sub Entity
            var Subentity = await subContext.Set<TSubEntity>().Where(x => x.IsDeleted == false && SubEntityIds.Contains(x.ID)).ToListAsync(cancellationToken);
            // var for count non null data
            int NotEmptyDataCount = 0;
            #endregion
            #region Date loop for list of DashboardModel
            // loop for data in current day , week, month and year
            for (short i = 0; i < 4; i++)
            {
                // creating a new DashboardModel for adding in Chart list
                var dashboardModel = new BarChartDashboard
                {
                    StructureName = FirstStructureNameList,
                    Data = new(),
                    GroupBy = i switch
                    {
                        0 => _localizer["Day"],
                        1 => _localizer["Week"],
                        2 => _localizer["Month"],
                        _ => _localizer["Year"],
                    },
                    Value = i
                };
                #region Loop for TSubEntity 
                // loop of all Subentity list for comparission of TMainEntity with TSubEntity
                foreach (var SubEntityExpression in expressionsForSubEntity)
                {
                    BarChartData dashboradData = new()
                    {
                        // geting TSubEntity localized name
                        Name = Localize.GetName(language, Subentity[expressionsForSubEntity.IndexOf(SubEntityExpression)]),
                        Data = new List<int>()
                    };
                    // implementing the exprission for subentities
                    var EntityForCount = mainContext.Set<TMainEntity>().Where(SubEntityExpression);
                    #region Loop of first structure expressions 
                    // loop of all first structure expressions for counting data based on the expression and the date loop (current day , week, month and year)
                    foreach (var expression in expressionsForFirstStructure)
                    {
                        dashboradData.Data.Add(
                        i switch
                        {
                            0 => EntityForCount.Where(expression).Count(x => x.CreatedOn.Date == CurrentDate.Date),
                            1 => EntityForCount.Where(expression).Count(x => x.CreatedOn.Date >= WeekStartDate.Date && x.CreatedOn.Date <= CurrentDate.Date),
                            2 => EntityForCount.Where(expression).Count(x => x.CreatedOn.Year == CurrentDate.Year && x.CreatedOn.Month == CurrentDate.Month),
                            _ => EntityForCount.Where(expression).Count(x => x.CreatedOn.Year == CurrentDate.Year)
                        });
                    }
                    #endregion
                    // adding data to not null count 
                    NotEmptyDataCount += dashboradData.Data.Sum(x => Convert.ToInt32(x));
                    // adding the proccessed dashboradData to dashboardModel
                    dashboardModel.Data.Add(dashboradData);
                }
                #endregion
                // addin the dashboardModel to the list (Chart)
                Chart.Add(dashboardModel);
            }
            #endregion
            if (NotEmptyDataCount == 0)
            {
                return [];
            }
            return Chart;
        }
        public async Task<List<SpiderDashboard>> GetSpiderDashboardData(string language, Expression<Func<TMainEntity, bool>> expression, CancellationToken cancellationToken, int? mainModule = null, int? subModule = null)
        {
            #region Determining dbcontext for TMainEntity and TSubEntity
            // it's determined by campairing mainModule and subModule to constants application modules
            DbContext mainContext = GetRequiredDbContext(mainModule);
            DbContext subContext = GetRequiredDbContext(subModule);
            #endregion
            #region Variable initialization
            var Subentity = await subContext.Set<TSubEntity>().Where(x => x.IsDeleted == false).ToListAsync(cancellationToken);
            string propertyName = GetEntityRelational_FK_PropertyName<TMainEntity, TSubEntity>(mainModule);
            List<SpiderDashboard> dashboards = new();
            SpiderDashboard dashboard = new()
            {
                TotalCategoryCount = Subentity.Count,
                TotalValueSum = 0,
                Data = new List<SpiderData>()
            };
            #endregion
            #region loop of expressions in MainEntity
            foreach (var entity in Subentity)
            {
                SpiderData data = new()
                {
                    Label = Localize.GetName(language, entity),
                    Value = await mainContext.Set<TMainEntity>().Where(x => EF.Property<int>(x, propertyName) == entity.ID).CountAsync(expression, cancellationToken)
                };
                dashboard.TotalValueSum += data.Value;
                dashboard.Data.Add(data);
            }
            #endregion
            dashboards.Add(dashboard);
            return dashboards;
        }
        public async Task<List<BarChartDashboard>?> GetBarChartData(string language, List<Expression<Func<TMainEntity, bool>>> expressionsForFirstStructure, List<string> FirstStructureNameList, string ForeignKeyColumnName, CancellationToken cancellationToken, int? mainModule = null, int? subModule = null)
        {
            #region Determining dbcontext for TMainEntity and TSubEntity
            // it's determined by campairing mainModule and subModule to constants application modules
            DbContext mainContext = GetRequiredDbContext(mainModule);
            DbContext subContext = GetRequiredDbContext(subModule);
            #endregion
            #region initializing Variables 
            // data that would be returned
            List<BarChartDashboard> Chart = new();
            // listing the Sub Entity
            var Subentity = await subContext.Set<TSubEntity>().Where(x => x.IsDeleted == false).ToListAsync(cancellationToken);
            // foriegn key column name in data base
            string propertyName = ForeignKeyColumnName; //GetEntityRelational_FK_PropertyName<TMainEntity, TSubEntity>(mainModule);
            // var for count non null data
            int NotEmptyDataCount = 0;
            #endregion
            #region Date loop for list of DashboardModel
            // loop for data in current day , week, month and year
            for (short i = 0; i < 4; i++)
            {
                // creating a new DashboardModel for adding in Chart list
                var dashboardModel = new BarChartDashboard
                {
                    StructureName = FirstStructureNameList,
                    Data = new(),
                    GroupBy = i switch
                    {
                        0 => _localizer["Day"],
                        1 => _localizer["Week"],
                        2 => _localizer["Month"],
                        _ => _localizer["Year"],
                    },
                    Value = i
                };
                #region Loop for TSubEntity 
                // loop of all Subentity list for comparission of TMainEntity with TSubEntity
                foreach (var entity in Subentity)
                {
                    BarChartData dashboradData = new()
                    {
                        // geting TSubEntity localized name
                        Name = Localize.GetName(language, entity),
                        Data = new List<int>()
                    };
                    // query for compairing TMainEntity with TSubEntity on MainEntityRelationalPropertyName is equal to ID of TSubEntity
                    // MainEntityRelationalPropertyName is and aurgument that is passed to this method
                    var EntityForCount = mainContext.Set<TMainEntity>().Where(x => x.IsDeleted == false && EF.Property<int>(x, propertyName) == entity.ID);
                    #region Loop of first structure expressions 
                    // loop of all first structure expressions for counting data based on the expression and the date loop (current day , week, month and year)
                    foreach (var expression in expressionsForFirstStructure)
                    {
                        dashboradData.Data.Add(
                        i switch
                        {
                            0 => EntityForCount.Where(expression).Count(x => x.CreatedOn.Date == CurrentDate.Date),
                            1 => EntityForCount.Where(expression).Count(x => x.CreatedOn.Date >= WeekStartDate.Date && x.CreatedOn.Date <= CurrentDate.Date),
                            2 => EntityForCount.Where(expression).Count(x => x.CreatedOn.Year == CurrentDate.Year && x.CreatedOn.Month == CurrentDate.Month),
                            _ => EntityForCount.Where(expression).Count(x => x.CreatedOn.Year == CurrentDate.Year)
                        });
                    }
                    #endregion
                    // adding data to not null count 
                    NotEmptyDataCount += dashboradData.Data.Sum(x => Convert.ToInt32(x));
                    // adding the proccessed dashboradData to dashboardModel
                    dashboardModel.Data.Add(dashboradData);
                }
                #endregion
                // addin the dashboardModel to the list (Chart)
                Chart.Add(dashboardModel);
            }
            #endregion
            if (NotEmptyDataCount == 0)
            {
                return [];
            }
            return Chart;
        }



        public async Task<List<BarChartDashboard>?> GetBarChartData(
            string language,
            List<Expression<Func<TMainEntity, bool>>> expressionsForFirstStructure,
            List<string> FirstStructureNameList,
            string ForeignKeyColumnName,
            List<int> SubEntityIds,
            CancellationToken cancellationToken,
            int? mainModule = null,
            int? subModule = null)
        {
            #region Determining dbcontext for TMainEntity and TSubEntity
            // it's determined by campairing mainModule and subModule to constants application modules
            DbContext mainContext = GetRequiredDbContext(mainModule);
            DbContext subContext = GetRequiredDbContext(subModule);
            #endregion
            #region initializing Variables 
            // data that would be returned
            List<BarChartDashboard> Chart = new();
            // listing the Sub Entity
            var Subentity = await subContext.Set<TSubEntity>().Where(x => x.IsDeleted == false && SubEntityIds.Contains(x.ID)).ToListAsync(cancellationToken);
            // foriegn key column name in data base
            string propertyName = ForeignKeyColumnName; //GetEntityRelational_FK_PropertyName<TMainEntity, TSubEntity>(mainModule);
            // var for count non null data
            int NotEmptyDataCount = 0;
            #endregion
            #region Date loop for list of DashboardModel
            // loop for data in current day , week, month and year
            for (short i = 0; i < 4; i++)
            {
                // creating a new DashboardModel for adding in Chart list
                var dashboardModel = new BarChartDashboard
                {
                    StructureName = FirstStructureNameList,
                    Data = new(),
                    GroupBy = i switch
                    {
                        0 => _localizer["Day"],
                        1 => _localizer["Week"],
                        2 => _localizer["Month"],
                        _ => _localizer["Year"],
                    },
                    Value = i
                };
                #region Loop for TSubEntity 
                // loop of all Subentity list for comparission of TMainEntity with TSubEntity
                foreach (var entity in Subentity)
                {
                    BarChartData dashboradData = new()
                    {
                        // geting TSubEntity localized name
                        Name = Localize.GetName(language, entity),
                        Data = new List<int>()
                    };
                    // query for compairing TMainEntity with TSubEntity on MainEntityRelationalPropertyName is equal to ID of TSubEntity
                    // MainEntityRelationalPropertyName is and aurgument that is passed to this method
                    var EntityForCount = mainContext.Set<TMainEntity>().Where(x => x.IsDeleted == false && EF.Property<int>(x, propertyName) == entity.ID);
                    #region Loop of first structure expressions 
                    // loop of all first structure expressions for counting data based on the expression and the date loop (current day , week, month and year)
                    foreach (var expression in expressionsForFirstStructure)
                    {
                        dashboradData.Data.Add(
                        i switch
                        {
                            0 => EntityForCount.Where(expression).Count(x => x.CreatedOn.Date == CurrentDate.Date),
                            1 => EntityForCount.Where(expression).Count(x => x.CreatedOn.Date >= WeekStartDate.Date && x.CreatedOn.Date <= CurrentDate.Date),
                            2 => EntityForCount.Where(expression).Count(x => x.CreatedOn >= HijriMonthStartDate),
                            _ => EntityForCount.Where(expression).Count(x => x.CreatedOn >= HijriYearStartDate)
                        });
                    }
                    #endregion
                    // adding data to not null count 
                    NotEmptyDataCount += dashboradData.Data.Sum(x => Convert.ToInt32(x));
                    // adding the proccessed dashboradData to dashboardModel
                    dashboardModel.Data.Add(dashboradData);
                }
                #endregion
                // addin the dashboardModel to the list (Chart)
                Chart.Add(dashboardModel);
            }
            #endregion
            if (NotEmptyDataCount == 0)
            {
                return [];
            }
            return Chart;
        }


        public async Task<List<BarChartDashboard>> GetBarChart(string language,
            List<object> FirstStructureRecords,
            Func<object, string> CallerMethod,
            Expression<Func<TMainEntity, object, bool>> ExpressionForMainEntityAccordingFirstStructure,
            string FirstStructurePrimaryKeysForignKeyNameInMainEntity,
            List<int> ListOfSubEntityIds,
            Expression<Func<TMainEntity, object, bool>> ExpressionForMainEntityAccordingSubEntity,
            int? mainModule = null,
            int? subModule = null
            )
        {
            #region Determining dbcontext for TMainEntity and TSubEntity
            // it's determined by campairing mainModule and subModule to constants application modules
            DbContext mainContext = GetRequiredDbContext(mainModule);
            DbContext subContext = GetRequiredDbContext(subModule);
            #endregion
            #region initializing Variables 
            // data that would be returned
            List<BarChartDashboard> Chart = new();
            // listing the Sub Entity
            var Subentity = await subContext.Set<TSubEntity>().Where(x => x.IsDeleted == false && ListOfSubEntityIds.Contains(x.ID)).ToListAsync();
            // foriegn key column name in data base
            string propertyName = FirstStructurePrimaryKeysForignKeyNameInMainEntity;
            //GetEntityRelational_FK_PropertyName<TMainEntity, TSubEntity>(mainModule);
            // var for count non null data
            int NotEmptyDataCount = 0;
            #endregion
            #region Date loop for list of DashboardModel
            for (short i = 0; i < 4; i++)
            {
                var dashboardModel = new BarChartDashboard
                {
                    StructureName = FirstStructureRecords.Select(x => CallerMethod.Invoke(x)).ToList(),
                    Data = new(),
                    GroupBy = i switch
                    {
                        0 => _localizer["Day"],
                        1 => _localizer["Week"],
                        2 => _localizer["Month"],
                        _ => _localizer["Year"],
                    },
                    Value = i
                };
                string SubEntityForiegnKeyNameInMainEntity = GetEntityRelational_FK_PropertyName<TMainEntity, TSubEntity>(mainModule);
                #region Loop for TSubEntity 
                foreach (var entity in Subentity)
                {
                    BarChartData dashboradData = new()
                    {
                        Name = Localize.GetName(language, entity),
                        Data = new List<int>()
                    };
                    var EntityForCount = mainContext.Set<TMainEntity>().Where(x => x.IsDeleted == false && EF.Property<int>(x, propertyName) == entity.ID);
                    #region Loop of first structure expressions 
                    foreach (var expression in FirstStructureRecords)
                    {
                        dashboradData.Data.Add(DashboardFilter(
                            ExpressionForMainEntityAccordingFirstStructure,
                            ExpressionForMainEntityAccordingSubEntity,
                            propertyName,
                            i,
                            SubEntityForiegnKeyNameInMainEntity,
                            EntityForCount));
                    }
                    #endregion
                    // adding data to not null count 
                    NotEmptyDataCount += dashboradData.Data.Sum(x => Convert.ToInt32(x));
                    // adding the proccessed dashboradData to dashboardModel
                    dashboardModel.Data.Add(dashboradData);
                }
                #endregion
                // addin the dashboardModel to the list (Chart)
                Chart.Add(dashboardModel);
            }
            #endregion
            if (NotEmptyDataCount == 0)
            {
                return [];
            }
            return Chart;
        }

        private int DashboardFilter(Expression<Func<TMainEntity, object, bool>> ExpressionForMainEntityAccordingFirstStructure, Expression<Func<TMainEntity, object, bool>> ExpressionForMainEntityAccordingSubEntity, string propertyName, short i, string SubEntityForiegnKeyNameInMainEntity, IQueryable<TMainEntity> EntityForCount)
        {
            return
                                    i switch
                                    {
                                        0 => EntityForCount.
                                        Where(x => ExpressionForMainEntityAccordingFirstStructure.Compile()(x, EF.Property<object>(x, propertyName))).
                                        Where(x => ExpressionForMainEntityAccordingSubEntity.Compile()(x, EF.Property<object>(x, SubEntityForiegnKeyNameInMainEntity))).
                                        Count(x => GetDateFilterPredicate(i, x.CreatedOn, CurrentDate.Date)),

                                        1 => EntityForCount.
                                        Where(x => ExpressionForMainEntityAccordingFirstStructure.Compile()(x, EF.Property<object>(x, propertyName))).
                                        Where(x => ExpressionForMainEntityAccordingSubEntity.Compile()(x, EF.Property<object>(x, SubEntityForiegnKeyNameInMainEntity))).
                                        Count(x => GetDateFilterPredicate(i, x.CreatedOn, WeekStartDate.Date, CurrentDate.Date)),

                                        2 => EntityForCount.
                                        Where(x => ExpressionForMainEntityAccordingFirstStructure.Compile()(x, EF.Property<object>(x, propertyName))).
                                        Where(x => ExpressionForMainEntityAccordingSubEntity.Compile()(x, EF.Property<object>(x, SubEntityForiegnKeyNameInMainEntity))).
                                        Count(x => GetDateFilterPredicate(i, x.CreatedOn, CurrentDate.Year, CurrentDate.Month)),

                                        _ => EntityForCount.
                                        Where(x => ExpressionForMainEntityAccordingFirstStructure.Compile()(x, EF.Property<object>(x, propertyName))).
                                        Where(x => ExpressionForMainEntityAccordingSubEntity.Compile()(x, EF.Property<object>(x, SubEntityForiegnKeyNameInMainEntity))).
                                        Count(x => GetDateFilterPredicate(i, x.CreatedOn, CurrentDate.Year))
                                    };
        }
        private bool GetDateFilterPredicate(short i, DateTime createdOn, params object[] dateParams)
        {
            return i switch
            {
                0 => createdOn.Date == (DateTime)dateParams[0],
                1 => createdOn.Date >= (DateTime)dateParams[0] && createdOn.Date <= (DateTime)dateParams[1],
                2 => createdOn.Year == (int)dateParams[0] && createdOn.Month == (int)dateParams[1],
                _ => createdOn.Year == (int)dateParams[0]
            };
        }

        #region Helper Methodes 
        // findes the relational foreign key in T1 that is mapped with primary key of T2
        public string GetEntityRelational_FK_PropertyName<TEntityFK, TEntityPK>(int? mainModule) where TEntityFK : class where TEntityPK : class
        {
            #region Determining dbcontext for TMainEntity and TSubEntity
            // it's determined by campairing mainModule 
            DbContext mainContext = GetRequiredDbContext(mainModule);
            #endregion
            return mainContext.Model.FindEntityType(typeof(TEntityFK))?.GetDeclaredForeignKeys()
                                    .FirstOrDefault(x => x?.PrincipalEntityType?.Name == typeof(TEntityPK).FullName)?.Properties
                                    .FirstOrDefault()?.Name ?? string.Empty;
        }
        // get the correct Dbcontext instance based on module you pass
        public DbContext GetRequiredDbContext(int? Module)
        {
            return Module switch
            {
                _ => _DbContext
            };
        }

        // get the Name of entity T in DataBase
        //public string GetTableNameInDb<T>(int? Module) where T : class
        //{
        //    var mapping = GetRequiredDbContext(Module).Model.FindEntityType(typeof(T));
        //    return mapping?.GetSchema() + "." + mapping?.GetTableName();
        //}
        #endregion
    }
    #endregion
    #region Single Entity Repository implementation
    // single Entity Generic Dashboard repository
    public class GenericDashboardRepository<TMainEntity> : IGenericDashboardRepository<TMainEntity> where TMainEntity : class
    {
        private readonly ERP_DbContext _DbContext;
        private readonly UMS_DbContext _UMSDbContext;

        public GenericDashboardRepository(ERP_DbContext dbContext, UMS_DbContext uMSDbContext)
        {
            _DbContext = dbContext;
            _UMSDbContext = uMSDbContext;
        }

        public async Task<List<SpiderDashboard>> GetSpiderDashboardData(List<Expression<Func<TMainEntity, bool>>> expressions, List<string> CategoryLabelList, CancellationToken cancellationToken, int? mainModule = null, params Expression<Func<TMainEntity, object>>[]? includes)
        {
            #region Determining dbcontext for TMainEntity and TSubEntity
            // it's determined by campairing mainModule and subModule to constants application modules
            DbContext mainContext = mainModule switch
            {
                Constants.ApplicationModule.UMS => _UMSDbContext,
                _ => _DbContext
            };
            #endregion
            #region Variable initialization
            List<SpiderDashboard> dashboards = new();
            SpiderDashboard dashboard = new()
            {
                TotalCategoryCount = CategoryLabelList.Count,
                TotalValueSum = 0,
                Data = new List<SpiderData>()
            };
            #endregion
            #region loop of expressions in MainEntity
            foreach (var expression in expressions)
            {
                SpiderData data = new()
                {
                    Label = CategoryLabelList[expressions.IndexOf(expression)],
                    Value = includes != null ?
                    await includes!.Aggregate(mainContext.Set<TMainEntity>().Where(expression), (current, includeProperty) => current.Include(includeProperty)).CountAsync(cancellationToken)
                    : await mainContext.Set<TMainEntity>().CountAsync(expression, cancellationToken)
                };
                dashboard.TotalValueSum += data.Value;
                dashboard.Data.Add(data);
            }
            #endregion
            dashboards.Add(dashboard);
            return dashboards;
        }
    }
    #endregion
}
