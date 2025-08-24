using Appointment_Scheduler_API.Data;

namespace Appointment_Scheduler_API.Models
{
    public enum AppointmentStatus
    {
        Open,
        Reserved,
        Confirmed
    }

    public partial class Appointment
    {
        public string Patient { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime RegisteredDateTime { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; }

    }
    public partial class Doctor
    {
        //Contstants
        //
        //AppointmentTimeInMinutes:  Length of time for an appointment
        //AppointmentReservationDecayInMinutes:  Length of time for a booked appointment to be confirmed
        //
        public const int AppointmentTimeInMinutes = 15;
        public const int AppointmentReservationDecayInMinutes = 5;
        public string Name { get; set; }

        public List<Appointment> Appointments { get; set; }

        //
        //LoadDoctor
        //Summary:
        //Get Doctor data from Database, also checks for expired bookings
        //
        public Doctor LoadDoctor(string DoctorName)
        {
            Doctor CurrentDoctor = DataHelper.LoadDoctor(DoctorName);

            //Review system for booked appointments that have expired
            foreach(Appointment unconfrimedAppointment in CurrentDoctor.Appointments.Where(obj=> obj.RegisteredDateTime.AddMinutes(AppointmentReservationDecayInMinutes) < DateTime.Now))
            {
                unconfrimedAppointment.AppointmentStatus = AppointmentStatus.Open;
                unconfrimedAppointment.RegisteredDateTime = DateTime.MinValue;
                unconfrimedAppointment.Patient = null;
            }
            DataHelper.SaveDoctor(CurrentDoctor);

            return CurrentDoctor;
        }

        //
        //AddAppointments
        //Summary:
        //Create appointments for every available slot given start and end times
        //
        public List<Appointment> AddAppointments(string DoctorName, DateTime Start, DateTime End)
        {
            List<Appointment> output = new List<Appointment>();
            //Validate Start and End Date are the same day
            if(Start.Date != End.Date)
            {
                throw new Exception("Invalid Date Range");
            }
            //Validate Start Date is before End Date
            if(Start > End)
            {
                throw new Exception("Start Date must come before End");
            }


            //Validate no valid appointments exists within the time span
            Doctor CurrentDoctor = DataHelper.LoadDoctor(DoctorName);
            List<Appointment> existingAppointments = CurrentDoctor.Appointments.Where(obj => obj.StartDateTime >= Start && obj.StartDateTime <= Start).ToList<Appointment>();
            if(existingAppointments.Count > 0)
            {
                throw new Exception("Date Range Already has Appointments");
            }




            TimeSpan Availability = End.Subtract(Start);
            Double AvailabilityInMinutes = Availability.TotalMinutes;

            //Loop over the timespan, creating appointments every AppointmentTimeInMinutes 
            for (int AppointmentsCreated=0; (AppointmentsCreated+1) * AppointmentTimeInMinutes<=AvailabilityInMinutes; AppointmentsCreated++)
            {
                Appointment NewAppointment = new Appointment();

                NewAppointment.StartDateTime = Start.AddMinutes(AppointmentsCreated * AppointmentTimeInMinutes);
                NewAppointment.EndDateTime = Start.AddMinutes((AppointmentsCreated+1) * AppointmentTimeInMinutes);
                NewAppointment.AppointmentStatus = AppointmentStatus.Open;

                CurrentDoctor.Appointments.Add(NewAppointment);
                output.Add(NewAppointment);
            }
            DataHelper.SaveDoctor(CurrentDoctor);

            return output;
        }

        //
        //BookAppointment
        //Summary:
        //Update an appointment to reserved status
        //
        public Appointment BookAppointment(string DoctorName,string Patient, DateTime Start, DateTime End)
        {

            Doctor CurrentDoctor = DataHelper.LoadDoctor(DoctorName);

            Appointment SelectedAppointment = CurrentDoctor.Appointments.Where(obj => obj.StartDateTime == Start && obj.EndDateTime == End).ToList()[0];

            if(SelectedAppointment.AppointmentStatus == AppointmentStatus.Open)
            {
                SelectedAppointment.AppointmentStatus = AppointmentStatus.Reserved;
                SelectedAppointment.RegisteredDateTime = DateTime.Now;
                SelectedAppointment.Patient = Patient;
            }
            else
            {
                SelectedAppointment = null;
            }

            DataHelper.SaveDoctor(CurrentDoctor);

            return SelectedAppointment;
        }

        //
        //ConfirmAppointment
        //Summary:
        //Update a reserved appointment to confirmed status
        //
        public Appointment ConfirmAppointment(string DoctorName, string Patient, DateTime Start, DateTime End)
        {
            Doctor CurrentDoctor = DataHelper.LoadDoctor(DoctorName);

            Appointment SelectedAppointment = CurrentDoctor.Appointments.Where(obj => obj.StartDateTime == Start && obj.EndDateTime == End).ToList()[0];

            if (SelectedAppointment.Patient == Patient && SelectedAppointment.AppointmentStatus == AppointmentStatus.Reserved && SelectedAppointment.RegisteredDateTime.AddMinutes(AppointmentReservationDecayInMinutes)>DateTime.Now)
            {
                SelectedAppointment.AppointmentStatus = AppointmentStatus.Confirmed;
            }
            else
            {
                SelectedAppointment = null;
            }

                DataHelper.SaveDoctor(CurrentDoctor);

            return SelectedAppointment;
        }
    }

}

