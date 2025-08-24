using Microsoft.AspNetCore.Mvc;
using Appointment_Scheduler_API.Models;
using System.Linq;

namespace Appointment_Scheduler_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Appointment_SchedulerController : ControllerBase
    {

        private readonly ILogger<Appointment_SchedulerController> _logger;

        public Appointment_SchedulerController(ILogger<Appointment_SchedulerController> logger)
        {
            _logger = logger;
        }

        //
        //GetOpenAppointments
        //Retrieve all open appointments for a specified Doctor
        //
        //
        [HttpGet("GetOpenAppointments")]
        public IEnumerable<Appointment> GetOpenAppointments( String Doctor)
        {
            
            try
            {
                Doctor CurrentDoctor = new Doctor();

                CurrentDoctor = CurrentDoctor.LoadDoctor(Doctor);

                List<Appointment> openAppointments = CurrentDoctor.Appointments.Where(obj => obj.AppointmentStatus == AppointmentStatus.Open).ToList();

                return openAppointments;

            }
            catch(Exception ex) { return null; }
            
        }

        //
        //GetConfirmedAppointments
        //Retrieve all confirmed appointments for a specified Doctor
        //
        //
        [HttpGet("GetConfirmedAppointments")]
        public IEnumerable<Appointment> GetConfirmedAppointments(String Doctor)
        {
            try { 
                Doctor CurrentDoctor = new Doctor();

                CurrentDoctor = CurrentDoctor.LoadDoctor(Doctor);

                List<Appointment> openAppointments = CurrentDoctor.Appointments.Where(obj => obj.AppointmentStatus == AppointmentStatus.Confirmed).ToList();

                return openAppointments;
            }
            catch (Exception ex) { return null; }
        }

        //
        //AddAvailability
        //Add Appointments between start and end times for the specified Doctor
        //
        //
        [HttpPost("AddAvailability")]
        public IEnumerable<Appointment> AddAvailability(String Doctor, DateTime Start, DateTime End)
        {
            try
            {
                Doctor CurrentDoctor = new Doctor();

                List<Appointment> openAppointments = CurrentDoctor.AddAppointments(Doctor, Start, End);

                return openAppointments;
            }
            catch(Exception ex) { return null; }
}
        //
        //BookAppointment
        //Update the specified open appointment to Booked
        //
        //
        [HttpPost( "BookAppointment")]
        public IEnumerable<Appointment> BookAppointment(String Doctor, string Patient, DateTime Start, DateTime End)
        {
            try
            {

                Doctor CurrentDoctor = new Doctor();

                Appointment NewAppointment = CurrentDoctor.BookAppointment(Doctor, Patient, Start, End);

                List<Appointment> BookedAppointment = new List<Appointment>();

                BookedAppointment.Add(NewAppointment);

                return BookedAppointment;
            }
            catch (Exception ex) { return null; }
        }

        //
        //ConfirmBooking
        //Updates the specified Booked appointment to confirmed.
        //
        //
        [HttpPost("ConfirmBooking")]
        public IEnumerable<Appointment> ConfirmBooking(String Doctor, string Patient, DateTime Start, DateTime End)
        {
            try
            {
                Doctor CurrentDoctor = new Doctor();

                Appointment NewAppointment = CurrentDoctor.ConfirmAppointment(Doctor, Patient, Start, End);

                List<Appointment> ConfirmedAppointment = new List<Appointment>();

                ConfirmedAppointment.Add(NewAppointment);

                return ConfirmedAppointment;
            }
            catch (Exception ex) { return null; }

        }


    }
}
