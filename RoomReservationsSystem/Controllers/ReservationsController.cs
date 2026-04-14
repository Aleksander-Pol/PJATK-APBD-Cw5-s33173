using Microsoft.AspNetCore.Mvc;
using RoomReservationsSystem.Enums;
using RoomReservationsSystem.Models;

namespace RoomReservationsSystem.Controllers;

[ApiController]
[Route("api/reservations")]
public class ReservationsController : ControllerBase
{
    public static List<Reservation> Reservations =
    [
        new Reservation()
        {
            Id = 1,
            RoomId = 1,
            OrganizerName = "Jan Długosz",
            Topic = "Political and economical state" +
                    "of the world",
            Date = new DateOnly(2018, 12, 30),
            StartTime = new TimeOnly(21,13),
            EndTime = new TimeOnly(22, 28),
            Status = Status.Confirmed
        },
        new Reservation()
        {
            Id = 1,
            RoomId = 2,
            OrganizerName = "Jan Bralczyk",
            Topic = "The Fall of Constantinopole's Influence" +
                    " on LeBron James' Legacy",
            Date = new DateOnly(2019,3, 25),
            StartTime = new TimeOnly(18,13),
            EndTime = new TimeOnly(20, 23),
            Status = Status.Confirmed
        }
    ];

    [HttpGet]
    public IActionResult GetQueriedReservation([FromQuery] string? date, [FromQuery] string? status,
        [FromQuery] int? roomId)
    {
        var reservations = Reservations.AsEnumerable();

        if (date is not null) reservations = reservations.Where(r => r.Date.ToString("yyyy-MM-dd").Equals(date));
        if (status is not null)
            reservations =
                reservations.Where(r => r.Status.ToString().ToUpperInvariant().Equals(status.ToUpperInvariant()));
        if (roomId.HasValue) reservations = reservations.Where(r => r.RoomId == roomId);

        return Ok(reservations);
    }
}