using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace Crystal_Clinic_Mgm.Common.Message
{
    public interface IMessage
    {
        #region Transaction success messages
        JsonResult Saved(object? obj = null);
        JsonResult Update(object? obj = null);
        JsonResult Delete(object? obj = null);
        JsonResult Remove(object? obj = null);
        #endregion
        #region Error messages
        JsonResult RecordNotFound(object? obj = null);
        JsonResult IdErrorMessage(object? obj = null);
        JsonResult InternalServerError(object? obj = null);
        JsonResult InternalSystemError(object? obj = null);
        #endregion
        #region Validation Error
        JsonResult ValidationError(int code, object? obj = null);
        JsonResult CheckValidationError(List<ValidationFailure> validation);
        JsonResult CheckCCValidationError(List<ValidationFailure> validation);
        JsonResult ValidationErrorAuthentication(IdentityResult validation);

        #endregion
        //Following Method decalared for Business Process in DMTS like Process, Pending,Reject etc.
        JsonResult Process(int ProcessId);
        //Followin Method Used for to show downloadReport path
        JsonResult DownloadReportPathMessage(string ReportPath,string ErrorMessage);
    }
}
