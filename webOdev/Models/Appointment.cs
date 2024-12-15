namespace webOdev.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int DoctorId { get; set; } // Hangi doktorla randevu
        public int AssistantId { get; set; } // Hangi asistanla randevu
        public DateTime Date { get; set; } // Tarih (Gün)
        public TimeSpan Time { get; set; } // Saat (10:00, 11:00 gibi)
        public string PatientName { get; set; } // Hasta adı
    }
}
