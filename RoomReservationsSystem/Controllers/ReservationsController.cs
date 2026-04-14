using Microsoft.AspNetCore.Mvc;
using RoomReservationsSystem.Enums;

namespace RoomReservationsSystem.Controllers;

public class ReservationsController : ControllerBase
{
    public int Id { get; set; }
    public string RoomId { get; set; }
    public string OrganizerName { get; set; }
    public string Topic { get; set; }
    public DateOnly Date{ get; set; }
    public TimeOnly StartTime { get; set; } 
    public TimeOnly EndTime { get; set; }
    public Status Status { get; set; }
}