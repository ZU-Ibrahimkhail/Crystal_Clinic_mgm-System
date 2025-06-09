namespace Crystal_Clinic_Mgm.Application.UMS.ViewModel
{
    public class ControllerActionsViewModel
    {
        public string Controller { get; set; } = string.Empty;
        public bool IsGlobal { get; set; }
        public string Action { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string ActionCategory { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
