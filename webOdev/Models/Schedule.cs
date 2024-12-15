using System;

namespace webOdev.Models;

public class Schedule
{
    public int Id { get; set; }
    public string Title { get; set; } // Örneğin: "Çocuk Bakımı"
    public DateTime StartTime { get; set; } // Nöbet başlangıç zamanı
    public DateTime EndTime { get; set; } // Nöbet bitiş zamanı
    public string AssignedTo { get; set; } // Asistan veya doktor adı
    public string Department { get; set; } // Departman bilgisi
}
