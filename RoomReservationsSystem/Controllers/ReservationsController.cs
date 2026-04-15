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
                Id = 2,
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
        public IActionResult GetQueriedReservation([FromQuery] DateOnly? date, [FromQuery] Status? status,
            [FromQuery] int? roomId)
        {
            var reservations = Reservations.AsEnumerable();

            if (date is not null) reservations = reservations.Where(r => r.Date.ToString("yyyy-MM-dd").Equals(date));
            if (status is not null)
                reservations =
                    reservations.Where(r => r.Status == status);
            
            if (roomId.HasValue) reservations = reservations.Where(r => r.RoomId == roomId);

            return Ok(reservations);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetReservationById(int id)
        {
            var reservation = Reservations.FirstOrDefault(r => r.Id == id);

            return Ok(reservation);
        }

        [HttpPost]
        public IActionResult CreateNewReservation(Reservation reservation)
        {
            var room = RoomsController.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);

            if (room is null)
                return NotFound($"Nie znaleziono pokoju o id {reservation.RoomId}");

            if (!room.IsActive)
                return BadRequest($"Nie można stworzyć rezerwacji na nieaktywny pokój");

            bool isThereConflict = Reservations.Any(r =>
                r.RoomId == reservation.RoomId &&
                r.Date == reservation.Date &&
                r.StartTime < reservation.EndTime &&
                r.EndTime > reservation.StartTime);

            if (isThereConflict)
                return Conflict("Rezerwacja nakłada się czasowo z inną rezerwacją");
                
            reservation.Id = Reservations.Count > 0 ? Reservations.Max(r => r.Id) + 1 : 1;
            Reservations.Add(reservation);

            return CreatedAtAction(nameof(GetReservationById), new { id = reservation.Id }, reservation);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateReservation(int id,[FromBody] Reservation reservation)
        {
            var newReservation = Reservations.FirstOrDefault(r => r.Id == id);

            if (newReservation is null) return NotFound($"Rezerwacja o id {id} nie została odnaleziona");

            newReservation.Id = reservation.Id;
            newReservation.RoomId = reservation.RoomId;
            newReservation.Date = reservation.Date;
            newReservation.StartTime = reservation.StartTime;
            newReservation.EndTime = reservation.EndTime;
            newReservation.OrganizerName = reservation.OrganizerName;
            newReservation.Topic = reservation.Topic;
            newReservation.Status = reservation.Status;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRoom(int id)
        {
            var reservationToDelete = Reservations.FirstOrDefault(r => r.Id == id);

            if (reservationToDelete is null) return NotFound($"Nie odnaleziono rezerwacji o id {id}");

            Reservations.Remove(reservationToDelete);

            return Ok(Reservations);
        }
    }