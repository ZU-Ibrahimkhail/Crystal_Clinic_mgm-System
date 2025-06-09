using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Domain.Entities;
using System.Linq.Expressions;

namespace Crystal_Clinic_Mgm.Application.Common.Services.IRepositories
{
    #region Double Entity repository 
    public interface IGenericDashboardRepository<TMainEntity, TSubEntity> where TMainEntity : AuditableEntity where TSubEntity : LookAndAuditableEntity
    {
        /// <summary>
        /// Returns the data in the format of BarChart that take multiple parameters
        /// ex :
        /// [{"FirstStructureName": ["SuperAdmin","Edress","SalehMohammad"],"groupBy": "Day","value": 0,"data": [{"name": "پیشنهاد","data": [0,0,0]},{"name": "استعلام","data": [0,0,0]},{ "name": "احکام","data": [0,0,0]},]}]
        /// </summary>
        /// <param name="language">used for localization (en, ps, dr)</param>
        /// <param name="expressionsForFirstStructure"> 
        /// A list of  expression(Expression<Func<TMainEntity, bool>>)  from TMainEntity on a property of  First Structure (ID, level ...etc) 
        /// --> the count of the expression should be the same as the count of FirstStructureNameList
        /// </param>
        /// <param name="FirstStructureNameList">The list of First Structure names to Display ex:(User Name list)--> like above example</param>
        /// <param name="MainEntityRelationalPropertyName">
        /// Name of the Main Entities property to compare with the ID of SubEntity
        /// --> Property name should be valid and exist in TMainEntity Table
        /// </param>
        /// <param name="mainModule">The module of TMainEntity(DMTS,ITSMS,Reception,Archive) it's value is compared with Constants.ApplicationModul</param>
        /// <param name="subModule">The module of TSubEntity(DMTS,ITSMS,Reception,Archive) it's value is compared with Constants.ApplicationModul</param>
        /// <returns></returns>
        public Task<List<BarChartDashboard>?> GetBarChartData(List<Expression<Func<TMainEntity, bool>>> expressionsForFirstStructure, List<string> FirstStructureNameList, CancellationToken cancellationToken, int? mainModule = null, int? subModule = null);
        public Task<List<BarChartDashboard>?> GetBarChartData(string language, List<Expression<Func<TMainEntity, bool>>> expressionsForFirstStructure, List<string> FirstStructureNameList, List<int> SubEntityIds, CancellationToken cancellationToken, int? mainModule = null, int? subModule = null);
        public Task<List<BarChartDashboard>?> GetBarChartData(string language, List<Expression<Func<TMainEntity, bool>>> expressionsForFirstStructure, List<string> FirstStructureNameList, string ForeignKeyColumnName, CancellationToken cancellationToken, int? mainModule = null, int? subModule = null);
        public Task<List<BarChartDashboard>?> GetBarChartData(string language, List<Expression<Func<TMainEntity, bool>>> expressionsForFirstStructure, List<string> FirstStructureNameList, string ForeignKeyColumnName, List<int> SubEntityIds, CancellationToken cancellationToken, int? mainModule = null, int? subModule = null);



        /// <summary>
        /// Returns the data in the format of BarChart that take multiple parameters
        /// ex :
        /// [{"FirstStructureName": ["SuperAdmin","Edress","SalehMohammad"],"groupBy": "Day","value": 0,"data": [{"name": "پیشنهاد","data": [0,0,0]},{"name": "استعلام","data": [0,0,0]},{ "name": "احکام","data": [0,0,0]},]}]
        /// </summary>
        /// <param name="language">used for localization (en, ps, dr)</param>
        /// <param name="expressionsForFirstStructure"> 
        /// A list of  expression(Expression<Func<TMainEntity, bool>>)  from TMainEntity on a property of  First Structure (ID, level ...etc) 
        /// --> the count of the expression should be the same as the count of FirstStructureNameList
        /// </param>
        /// <param name="FirstStructureNameList">The list of First Structure names to Display ex:(User Name list)--> like above example</param>
        /// <param name="expressionsForSubEntity">
        ///A list of expression(Expression<Func<TMainEntity, bool>>)  from TMainEntity on a property of  TSubEntity(ID, level...etc)
        /// --> the count of the expression should be the same as the count of FirstStructureNameList
        /// </param>
        /// <param name="mainModule">The module of TMainEntity(DMTS,ITSMS,Reception,Archive) it's value is compared with Constants.ApplicationModul</param>
        /// <param name="subModule">The module of TSubEntity(DMTS,ITSMS,Reception,Archive) it's value is compared with Constants.ApplicationModul</param>
        /// <returns></returns>
        public Task<List<BarChartDashboard>?> GetBarChartData(string language, List<Expression<Func<TMainEntity, bool>>> expressionsForFirstStructure, List<Expression<Func<TMainEntity, bool>>> expressionsForSubEntity, List<string> FirstStructureNameList, CancellationToken cancellationToken, int? mainModule = null, int? subModule = null);
        public Task<List<BarChartDashboard>?> GetBarChartData(string language, List<Expression<Func<TMainEntity, bool>>> expressionsForFirstStructure, List<Expression<Func<TMainEntity, bool>>> expressionsForSubEntity, List<int> SubEntityIds, List<string> FirstStructureNameList, CancellationToken cancellationToken, int? mainModule = null, int? subModule = null);
        public Task<List<SpiderDashboard>> GetSpiderDashboardData(string language, Expression<Func<TMainEntity, bool>> expression, CancellationToken cancellationToken, int? mainModule = null, int? subModule = null);

    }
    #endregion

    #region Single Entity repository 
    public interface IGenericDashboardRepository<TMainEntity> where TMainEntity : class
    {
        public Task<List<SpiderDashboard>> GetSpiderDashboardData(List<Expression<Func<TMainEntity, bool>>> expressions, List<string> CategoryLabelList, CancellationToken cancellationToken, int? mainModule = null, params Expression<Func<TMainEntity, object>>[]? includes);
    }
    #endregion
}
