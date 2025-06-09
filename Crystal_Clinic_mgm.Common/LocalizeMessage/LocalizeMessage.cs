using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
namespace Crystal_Clinic_Mgm.Common.LocalizeMessage
{
    public class LocalizeMessage
    {
        public IStringLocalizer<CommonValidationResource> _localizer;
        //---New---
        public string EnglishNameRequiredField = nameof(EnglishNameRequiredField);
        public string DariNameRequiredField = nameof(DariNameRequiredField);
        public string PashtoNameRequiredField = nameof(PashtoNameRequiredField);
        public string CodeRequiredField = nameof(CodeRequiredField);
        public string EnglishNameUniqueField = nameof(EnglishNameUniqueField);
        public string DariNameUniqueField = nameof(DariNameUniqueField);
        public string PashtoNameUniqueField = nameof(PashtoNameUniqueField);
        public string CodeUniqueField = nameof(CodeUniqueField);
        //---------
        public string RequiredField = string.Empty;
        public string UniqueField = string.Empty;
        public string RequiredNumberField = string.Empty;
        public string ValidParentRecord = string.Empty;
        public string ReturnDateCondition = string.Empty;
        public string DocumentUniqueNumberMessage = string.Empty;
        public string TrackingUserNotUpdate = string.Empty;
        public string NotValidId = string.Empty;
        public string NotValidNumber = string.Empty;
        public string NotValidPhoneNumber = string.Empty;
        public string NotCCBranch = string.Empty;
        public string CCDuplicateBranch = string.Empty;
        public string CCDuplicateOrganization = string.Empty;
        public string CCExistBranch = string.Empty;
        public string CCExistOrganization = string.Empty;
        public string CCFromBranch = string.Empty;
        public string CCToBranch = string.Empty;
        public string CCFromToBranch = string.Empty;
        public string CCFromToOrganization = string.Empty;
        public string CCTracking = string.Empty;
        public string GreaterCount = string.Empty;
        public string NoAttachment = string.Empty;
        public string CanNotForwardToSameUser = string.Empty;
        public string DuplicateCard = string.Empty;
        public string IsDuplicate = string.Empty;
        public string DuplicateVisitorTracking = string.Empty;
        public string DownloadReportPath = string.Empty;
        public string ServiceDeadLineByHour = string.Empty;
        public string BigFileOrFileName = string.Empty;
        public string BlockName = string.Empty;
        public string MustBeNull = string.Empty;
        public LocalizeMessage(IStringLocalizer<CommonValidationResource> localizer)
        {
            _localizer = localizer;
            //----New--
            EnglishNameRequiredField = localizer[nameof(EnglishNameRequiredField)] .Value?? "__";
            DariNameRequiredField = localizer[nameof(DariNameRequiredField)] .Value?? "__";
            PashtoNameRequiredField = localizer[nameof(PashtoNameRequiredField)] .Value?? "__";
            CodeRequiredField = localizer[nameof(CodeRequiredField)] .Value?? "__";
            EnglishNameUniqueField = localizer[nameof(EnglishNameUniqueField)] .Value?? "__";
            DariNameUniqueField = localizer[nameof(DariNameUniqueField)] .Value?? "__";
            PashtoNameUniqueField = localizer[nameof(PashtoNameUniqueField)] .Value?? "__";
            CodeUniqueField = localizer[nameof(CodeUniqueField)] .Value?? "__";
            //---------
            RequiredField = localizer[nameof(RequiredField)] .Value?? "__";
            UniqueField = localizer[nameof(UniqueField)] .Value?? "__";
            RequiredNumberField = localizer[nameof(RequiredNumberField)] .Value?? "__";
            ValidParentRecord = localizer[nameof(ValidParentRecord)] .Value?? "__";
            ReturnDateCondition = localizer[nameof(ReturnDateCondition)] .Value?? "__";
            DocumentUniqueNumberMessage = localizer[nameof(DocumentUniqueNumberMessage)] .Value?? "__";
            TrackingUserNotUpdate = localizer[nameof(TrackingUserNotUpdate)] .Value?? "__";
            NotValidId = localizer[nameof(NotValidId)] .Value?? "__";
            NotValidNumber = localizer[nameof(NotValidNumber)] .Value?? "__";
            NotValidPhoneNumber = localizer[nameof(NotValidPhoneNumber)] .Value?? "__";
            NotCCBranch = localizer[nameof(NotCCBranch)] .Value?? "__";
            CCDuplicateBranch = localizer[nameof(CCDuplicateBranch)] .Value?? "__";
            CCDuplicateOrganization = localizer[nameof(CCDuplicateOrganization)] .Value?? "__";
            CCExistBranch = localizer[nameof(CCExistBranch)] .Value?? "__";
            CCExistOrganization = localizer[nameof(CCExistOrganization)] .Value?? "__";
            CCFromBranch = localizer[nameof(CCFromBranch)] .Value?? "__";
            CCToBranch = localizer[nameof(CCToBranch)] .Value?? "__";
            CCFromToBranch = localizer[nameof(CCFromToBranch)] .Value?? "__";
            CCFromToOrganization = localizer[nameof(CCFromToOrganization)] .Value?? "__";
            CCTracking = localizer[nameof(CCTracking)] .Value?? "__";
            GreaterCount = localizer[nameof(GreaterCount)] .Value?? "__";
            NoAttachment = localizer[nameof(NoAttachment)] .Value?? "__";
            CanNotForwardToSameUser = localizer[nameof(CanNotForwardToSameUser)] .Value?? "__";
            DuplicateCard = localizer[nameof(DuplicateCard)] .Value?? "__";
            IsDuplicate = localizer[nameof(IsDuplicate)] .Value?? "__";
            DuplicateVisitorTracking = localizer[nameof(DuplicateVisitorTracking)] .Value?? "__";
            DownloadReportPath = localizer[nameof(DownloadReportPath)] .Value?? "__";
            ServiceDeadLineByHour = localizer[nameof(ServiceDeadLineByHour)] .Value?? "__";
            BigFileOrFileName = localizer[nameof(BigFileOrFileName)] .Value?? "__";
            BlockName = localizer[nameof(BlockName)] .Value?? "__";
            MustBeNull = localizer[nameof(MustBeNull)] .Value?? "__";

        }
    }
}
