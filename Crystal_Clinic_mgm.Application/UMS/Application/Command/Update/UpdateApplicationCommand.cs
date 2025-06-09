using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.UMS.Application.Command.Update
{
    public class UpdateApplicationCommand : IRequest<JsonResult>
    {
        public int ID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Abbrevation { get; set; } = string.Empty;
        public string IconClass { get; set; } = string.Empty;
        public string DefaultRoute { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        //  public virtual ICollection<Route> Routes { get; set; }

    }
}
