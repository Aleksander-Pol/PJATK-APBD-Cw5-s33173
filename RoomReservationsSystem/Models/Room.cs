using System.ComponentModel.DataAnnotations;

namespace RoomReservationsSystem.Models;

public class Room
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    [Required]
    public string BuildingCode { get; set; }
    public int Floor { get; set; }
    
    [Range(0, int.MaxValue, ErrorMessage = "Capacity has to be positive")]
    public int Capacity { get; set; }
    public bool HasProjector { get; set; }
    public bool IsActive { get; set; }
}