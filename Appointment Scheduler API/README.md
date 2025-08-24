To Run the API (Requires Visual Studio)

Step 1: Download the Project

Step 2: Open the "Appointment Scheduler API.sln" file

Step 3: Press the run button.  This will open a console window identifing the listening address, 
	This is typically http://localhost:5230

Endpooints run as the follows:

GET: http://localhost:5230/api/Appointment_Scheduler/GetOpenAppointments?Doctor={Doctor}

	Gets any open appointments for the specified Doctor

	Parameters:
		Doctor: String, Name of the Doctor

	Returns:
		List of Open Appointments


GET: http://localhost:5230/api/Appointment_Scheduler/GetConfirmedAppointments?Doctor={Doctor}

	Gets any confirmed appointments for the specified Doctor

	Parameters:
		Doctor: String, Name of the Doctor

	Returns:
		List of Closed Appointments


POST: http://localhost:5230/api/Appointment_Scheduler/AddAvailability?Doctor={Doctor}&Start={StartDate}&End={EndDate}

	Add appointments to the specified Doctor

	Parameters:
		Doctor: String, Name of the Doctor
		Start: DateTime, Starting Datetime for appointment block
		End: DateTime, Ending Datetime for appointment block

	Returns:
		List of Appointments created with call


POST: http://localhost:5230/api/Appointment_Scheduler/BookAppointment?Doctor={Doctor}&Patient={Patient}Start={StartDate}&End={EndDate}

	Books the specific appointment for the Patient

	Parameters:
		Doctor: String, Name of the Doctor
		Patieent: String, Name of the Patient
		Start: DateTime, Starting Datetime for appointment
		End: DateTime, Ending Datetime for appointment

	Returns:
		Booked Appointment


POST: http://localhost:5230/api/Appointment_Scheduler/ConfirmBooking?Doctor={Doctor}&Patient={Patient}Start={StartDate}&End={EndDate}

	Confirmes the specified appointment for the Patient

	Parameters:
		Doctor: String, Name of the Doctor
		Patieent: String, Name of the Patient
		Start: DateTime, Starting Datetime for appointment
		End: DateTime, Ending Datetime for appointment

	Returns:
		Confirmed Appointment


All endpoints return no data on failure states

Tests

	Tests were run using POSTMAN application, tests are run from a blank system, this requires a restart between tests

Test 1

POST: http://localhost:5230/api/Appointment_Scheduler/AddAvailability?Doctor=Jon&Start=2025-09-01 17:00:00&End=2025-09-01 18:00:00
		Expectation:  Returns Successfully

GET: http://localhost:5230/api/Appointment_Scheduler/GetOpenAppointments?Doctor=Jon
		Expectation:  Returns Successfully

POST: http://localhost:5230/api/Appointment_Scheduler/BookAppointment?Doctor=Jon&Patient=Adam&Start=2025-09-01 17:00:00&End=2025-09-01 17:15:00
		Expectation:  Returns Successfully

POST: http://localhost:5230/api/Appointment_Scheduler/ConfirmBooking?Doctor=Jon&Patient=Adam&Start=2025-09-01 17:00:00&End=2025-09-01 17:15:00
		Expectation:  Returns Successfully

GET: http://localhost:5230/api/Appointment_Scheduler/GetConfirmedAppointments?Doctor=Jon
		Expectation:  Returns Successfully


Test 2

POST: http://localhost:5230/api/Appointment_Scheduler/AddAvailability?Doctor=Jon&Start=2025-09-01 17:00:00&End=2025-09-01 18:00:00
		Expectation:  Returns Successfully

POST: http://localhost:5230/api/Appointment_Scheduler/AddAvailability?Doctor=Jon&Start=2025-09-01 17:00:00&End=2025-09-01 18:00:00
		Expectation:  Failure

Test 3

POST: http://localhost:5230/api/Appointment_Scheduler/AddAvailability?Doctor=Jon&Start=2025-09-01 17:00:00&End=2025-09-01 18:00:00
		Expectation:  Returns Successfully

POST: http://localhost:5230/api/Appointment_Scheduler/BookAppointment?Doctor=Jon&Patient=Adam&Start=2025-09-01 17:00:00&End=2025-09-01 17:15:00
		Expectation:  Returns Successfully

		Wait more than 5 minutes

POST: http://localhost:5230/api/Appointment_Scheduler/ConfirmBooking?Doctor=Jon&Patient=Adam&Start=2025-09-01 17:00:00&End=2025-09-01 17:15:00
		Expectation:  Failure

Test 4

GET: http://localhost:5230/api/Appointment_Scheduler/GetOpenAppointments?Doctor=Jon
		Expectation:  Failure

Test 5

GET: http://localhost:5230/api/Appointment_Scheduler/GetConfirmedAppointments?Doctor=Jon
		Expectation:  Failure

Test 6

POST: http://localhost:5230/api/Appointment_Scheduler/AddAvailability?Doctor=Jon&Start=2025-09-01 17:00:00&End=2025-08-01 18:00:00
		Expectation:  Failure

Test 7

POST: http://localhost:5230/api/Appointment_Scheduler/AddAvailability?Doctor=Jon&Start=2025-09-01 17:00:00&End=2025-09-01 15:00:00
		Expectation:  Failure

Test 8
POST: http://localhost:5230/api/Appointment_Scheduler/AddAvailability?Doctor=Jon&Start=2025-09-01 17:00:00&End=2025-09-01 18:00:00
		Expectation:  Returns Successfully

POST: http://localhost:5230/api/Appointment_Scheduler/BookAppointment?Doctor=Jon&Patient=Adam&Start=2025-09-01 17:00:00&End=2025-09-01 17:15:00
		Expectation:  Returns Successfully

POST: http://localhost:5230/api/Appointment_Scheduler/BookAppointment?Doctor=Jon&Patient=Matt&Start=2025-09-01 17:00:00&End=2025-09-01 17:15:00
		Expectation:  Failure