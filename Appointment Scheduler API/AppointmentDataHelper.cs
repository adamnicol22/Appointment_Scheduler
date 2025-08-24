using Appointment_Scheduler_API.Models;

namespace Appointment_Scheduler_API.Data
{
    public class DataHelper
    {

        public static Dictionary<string, Doctor> DoctorStore = new Dictionary<string, Doctor>();
        //LoadDoctor
        //Summary:
        //Load Doctor information into Memory, if Doctor information is not found, a new Doctor object is created
        //
        public static Doctor LoadDoctor(String DoctorName)
        {

            if (DoctorStore.ContainsKey(DoctorName)) return DoctorStore.GetValueOrDefault(DoctorName);
            else
            {
                Doctor NewDoc = new Doctor();
                NewDoc.Name= DoctorName;

                NewDoc.Appointments = new List<Appointment>();

                return NewDoc;
            }

        }
        //SaveDoctor
        //Summary:
        //Save the Doctor Information in memory
        //
        public static string SaveDoctor(Doctor CurrentDoctor)
        {  
           if(DoctorStore.ContainsKey(CurrentDoctor.Name)) DoctorStore.Remove(CurrentDoctor.Name);

            DoctorStore.Add(CurrentDoctor.Name, CurrentDoctor);

            return "Success";
        }

    }
}
