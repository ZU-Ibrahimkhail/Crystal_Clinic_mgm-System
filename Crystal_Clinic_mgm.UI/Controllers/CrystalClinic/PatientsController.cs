using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Crystal_ClinicServices.Patients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers.CrystalClinic
{
    [Authorize]
    [RBAC]
    public class PatientsController : BaseController
    {
        


        [HttpGet("{patientId}")]
        public async Task<IActionResult> GetPatientById(int patientId)
        {
            var patient = await Mediator.Send(new GetPatientByIdQuery { PatientId = patientId });
            if (patient == null)
                return NotFound();
            return Ok(patient);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPatients([FromQuery] GetAllPatientsQuery query)
        {
            var patients = await Mediator.Send(query);
            return Ok(patients);
        }

        [HttpPut("{patientId}")]
        public async Task<IActionResult> UpdatePatient(int patientId, [FromBody] UpdatePatientCommand command)
        {
            if (patientId != command.PatientId)
                return BadRequest("Patient ID mismatch");

            var result = await Mediator.Send(command);
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{patientId}")]
        public async Task<IActionResult> DeletePatient(int patientId)
        {
            var result = await Mediator.Send(new DeletePatientCommand { PatientId = patientId });
            if (!result)
                return NotFound();
            return NoContent();
        }
    }

}
