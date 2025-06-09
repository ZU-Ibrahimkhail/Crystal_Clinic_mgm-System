using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Common.CommonResponseLocalization;

namespace Crystal_Clinic_Mgm.Common.Message
{
    public class Message : IMessage
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string messageText = "";
        private readonly IStringLocalizer<CommonResponseResource> _localizer;
        public Message(
            IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<CommonResponseResource> localizer)
        {
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }
        #region Transaction success messages implementation
        public JsonResult Saved(object? obj = null)
        {
            messageText = _localizer.GetString("SavedSuccessfully");
            return Success(Constants.Constants.ResponsCodes.SAVED, obj);
        }
        public JsonResult Update(object? obj)
        {
            messageText = _localizer.GetString("UpdateSuccessfully");
            return Success(Constants.Constants.ResponsCodes.UPDATED, obj);
        }
        public JsonResult Delete(object? obj)
        {
            messageText = _localizer.GetString("DeleteSuccessfully");
            return Success(Constants.Constants.ResponsCodes.DELETED, obj);
        }
        public JsonResult Remove(object? obj)
        {
            messageText = _localizer.GetString("RemoveRecordSuccessfully");
            return Success(Constants.Constants.ResponsCodes.DELETED, obj);
        }
        #endregion
        //private functions user can't access these functions
        #region PrivateMethod
        private JsonResult Success(int code, object? obj = null)
        {
            return new JsonResult(new { code = code, message = messageText, data = obj });
        }
        private JsonResult Error(int code, object? obj = null)
        {

            _httpContextAccessor.HttpContext!.Response.StatusCode = code;
            return new JsonResult(new { code = code, message = messageText, data = obj });
        }
        #endregion
        #region Validation error message implementation
        public JsonResult ValidationError(int code, object? obj = null)
        {
            messageText = "Validation Error !";
            _httpContextAccessor.HttpContext!.Response.StatusCode = code;
            return new JsonResult(new { code = code, message = messageText, error = obj });
        }
        public JsonResult CheckValidationError(List<ValidationFailure> validation)
        {
            IDictionary<string, string> d = new Dictionary<string, string>();
            foreach (var c in validation)
            {
                d.Add(new KeyValuePair<string, string>(c.PropertyName, c.ErrorMessage));
            }
            return ValidationError(Constants.Constants.ResponsCodes.Validation_Error, d);
        }       
        public JsonResult ValidationErrorAuthentication(IdentityResult validation)
        {
            IDictionary<string, string> d = new Dictionary<string, string>();
            foreach (var c in validation.Errors)
            {
                d.Add(new KeyValuePair<string, string>(c.Code, c.Description));
            }
            return ValidationError(Constants.Constants.ResponsCodes.Validation_Error, d);
        }

        public JsonResult CheckCCValidationError(List<ValidationFailure> validation)
        {
            validation = validation.DistinctBy(x => x.PropertyName).ToList();
            IDictionary<string, object?> d = new Dictionary<string, object?>();
            var keyname = validation.Where(x => x.PropertyName.Contains('[')).Select(x => x.PropertyName.Split("[")[0]).Distinct().ToList();
            foreach (var v in validation.Where(x => !x.PropertyName.Contains('[')))
            {
                d.Add(v.PropertyName, v.ErrorMessage);
            }

            foreach (var key in keyname)
            {
                var indexcount = validation.Where(x => x.PropertyName.StartsWith(key)).Select(x => int.Parse(x.PropertyName.Split("[")[1].Split("]")[0])).Max();
                var Docs = new List<Dictionary<string, string>>();
                for (int k = 0; k <= indexcount; k++)
                {
                    Docs.Add(new Dictionary<string, string>());
                }
                for (int i = 0; i < Docs.Count; i++)
                {
                    foreach (var v in validation)
                    {
                        string name = v.PropertyName.Split(".").Last();
                        int index = int.Parse(v.PropertyName.Split("[")[1].Split("]")[0]);
                        if (index == i && v.PropertyName.StartsWith(key))
                        {
                            Docs[i].Add(name.Replace("[" + index + "]", ""), v.ErrorMessage);
                        }
                    }
                }
                d.Add(new KeyValuePair<string, object?>(key, Docs));
            }


            return ValidationError(Constants.Constants.ResponsCodes.Validation_Error, d);
        }
        #endregion
        #region Error
        public JsonResult RecordNotFound(object? obj = null)
        {
            messageText = _localizer.GetString("RecordNotFound");
            return Error(Constants.Constants.ErrorCodes.NotFound, messageText);
        }
        public JsonResult IdErrorMessage(object? obj = null)
        {
            messageText = _localizer.GetString("IdNotValid");
            return Error(Constants.Constants.ErrorCodes.IdLessThenError, messageText);
        }
        public JsonResult InternalServerError(object? obj = null)
        {
            messageText = _localizer.GetString("DatabaseError");
            return new JsonResult(new { code = 1, message = messageText, error = obj });
        }
        public JsonResult InternalSystemError(object? obj = null)
        {
            messageText = _localizer.GetString("SystemError");
            return Error(Constants.Constants.ErrorCodes.BadRequest, obj);
        }
        #endregion
        public JsonResult Process(int ProcessId)
        {
            int constantReturnCode = 0;
            if (ProcessId is > 0)
            {
                if (ProcessId == Constants.Constants.ProcessStatus.Sent)
                {
                    messageText = _localizer.GetString("YourDoucForwardedToNextStep");
                    constantReturnCode = Constants.Constants.ProcessStatus.Sent;
                }
                if (ProcessId == Constants.Constants.ProcessStatus.Pending)
                {
                    messageText = _localizer.GetString("YouPutDocInPendingProcess");
                    constantReturnCode = Constants.Constants.ProcessStatus.Pending;
                }
                if (ProcessId == Constants.Constants.ProcessStatus.InProcess)
                {
                    messageText = _localizer.GetString("YouPutDocUnderProcess");
                    constantReturnCode = Constants.Constants.ProcessStatus.InProcess;
                }
                if (ProcessId == Constants.Constants.ProcessStatus.DocumentProcessEnd)
                {
                    messageText = _localizer.GetString("DocProcessEnd");
                    constantReturnCode = Constants.Constants.ProcessStatus.DocumentProcessEnd;
                }
                if (ProcessId == Constants.Constants.ProcessStatus.Reject)
                {
                    messageText = _localizer.GetString("DocRejectAndForwardedToPreviousEmployee");
                    constantReturnCode = Constants.Constants.ProcessStatus.Reject;
                }
                if (ProcessId == Constants.Constants.ProcessStatus.Approve)
                {
                    messageText = _localizer.GetString("YourRequestApproved");
                    constantReturnCode = Constants.Constants.ProcessStatus.Approve;
                }
            }
            else
            {
                messageText = _localizer.GetString("YouDidNotEnterValidIdPleaseTryAgain");
                //messageText = "You are not entered valide Id please try again  ";
            }

            return Success(constantReturnCode, ProcessId);
        }
        //Followin Method Used for to show downloadReport path
        public JsonResult DownloadReportPathMessage(string ReportPath, string ErrorMessage)
        {
            if (ErrorMessage=="")
            {
                messageText = _localizer.GetString("DownloadedReportPath");
                return Success(
                    Constants.Constants.ResponsCodes.ReportDownloaded,
                    ReportPath);
            }
            else
            {
                messageText = ErrorMessage;// _localizer.GetString("DownloadedReportPath");
                return Error(
                    Constants.Constants.ErrorCodes.NotFound,
                    ReportPath);
            }
           
        }

      

        public class DocumentCCModel
        {
            public string? Organization_Or_Branch_Id { get; set; }
            public string? DocumentNumber { get; set; }
        }

        public class CCValidation
        {
            public List<Dictionary<string, string>>? Branch { get; set; }

        }
    }
}
