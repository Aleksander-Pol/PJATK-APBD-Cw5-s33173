using RoomReservationsSystem.Enums;

namespace RoomReservationsSystem.Models;

public class Reservation
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public string OrganizerName { get; set; }
    public string Topic { get; set; }
    public DateOnly Date{ get; set; }
    public TimeOnly StartTime { get; set; } 
    public TimeOnly EndTime { get; set; }
    public Status Status { get; set; }
}