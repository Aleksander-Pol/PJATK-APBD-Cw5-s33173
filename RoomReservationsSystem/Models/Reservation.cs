using System.ComponentModel.DataAnnotations;
using RoomReservationsSystem.Enums;

namespace RoomReservationsSystem.Models;

public class Reservation
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    
    [Required]
    public string OrganizerName { get; set; }
   
    [Required]
    public string Topic { get; set; }
    public DateOnly Date{ get; set; }
    public TimeOnly StartTime { get; set; } 
    
    [IsTimeAfter(nameof(StartTime))]
    public TimeOnly EndTime { get; set; }
    public Status Status { get; set; }
}