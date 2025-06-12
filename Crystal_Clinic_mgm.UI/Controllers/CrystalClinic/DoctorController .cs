using Crystal_Clinic_Mgm.Application.CrystalClinic.Doctors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers.CrystalClinic
{
    [Authorize]
    public class DoctorController : BaseController
    {

        #region Create Doctor
        [HttpPost("create")]
        public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorCommand command)
        {
            var doctorId = await Mediator.Send(command);
            return Ok(new { DoctorId = doctorId });
        }
        #endregion

        #region Update Doctor
        [HttpPut("update")]
        public async Task<IActionResult> UpdateDoctor([FromBody] UpdateDoctorCommand command)
        {
            var doctorId = await Mediator.Send(command);
            return Ok(new { DoctorId = doctorId });
        }
        #endregion

        #region Delete Doctor
        [HttpDelete("delete/{doctorId:int}")]
        public async Task<IActionResult> DeleteDoctor(int doctorId)
        {
            var success = await Mediator.Send(new DeleteDoctorCommand { DoctorId = doctorId });
            return success ? Ok("Doctor deleted.") : NotFound("Doctor not found.");
        }
        #endregion

        #region Get Doctor Details
        [HttpGet("details/{doctorId:int}")]
        public async Task<IActionResult> GetDoctorDetails(int doctorId)
        {
            var doctorDetails = await Mediator.Send(new GetDoctorDetailsQuery { DoctorId = doctorId });
            return Ok(doctorDetails);
        }
        #endregion

        #region Get Doctor List
        [HttpGet("list")]
        public async Task<IActionResult> GetDoctorList([FromQuery] GetDoctorListQuery query)
        {
            var doctorList = await Mediator.Send(query);
            return Ok(doctorList);
        }
        #endregion
    }
}
