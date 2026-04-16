using System.Collections;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using RoomReservationsSystem.Models;

namespace RoomReservationsSystem.Controllers;

[ApiController]
[Route("api/rooms")]
public class RoomsController : ControllerBase
{
    public static List<Room> Rooms =
    [
        new Room()
        {
            Id = 1,
            Name = "Mordor",
            BuildingCode = "36a",
            Capacity = 64,
            Floor = 4,
            HasProjector = true,
            IsActive = false
        },
        new Room()
        {
            Id = 2,
            Name = "Jungle",
            BuildingCode = "36a",
            Capacity = 46,
            Floor = 7,
            HasProjector = false,
            IsActive = true
        },
        new Room()
        {
            Id = 3,
            Name = "Bajo jajo",
            BuildingCode = "36b",
            Capacity = 15,
            Floor = 10,
            HasProjector = false,
            IsActive = false
        },
        new Room()
        {
            Id = 4,
            Name = "Swamp",
            BuildingCode = "29a",
            Capacity = 50,
            Floor = 4,
            HasProjector = true,
            IsActive = true
        },
        new Room()
        {
            Id = 5, 
            Name = "Tux island",
            BuildingCode = "16b",
            Capacity = 10,
            Floor = 2,
            HasProjector = false,
            IsActive = true
        }
    ];
        
    
    [HttpGet]
    public IActionResult GetQueriedRoom([FromQuery] int? minCapacity,[FromQuery] bool? hasProjector,[FromQuery(Name = "activeOnly")] bool? isActive)
    {
        var rooms = Rooms.AsEnumerable();

        if (minCapacity.HasValue) rooms = rooms.Where(r => r.Capacity >= minCapacity);
        if (hasProjector.HasValue) rooms = rooms.Where(r => r.HasProjector == hasProjector);
        if (isActive.HasValue) rooms = rooms.Where(r => r.IsActive == isActive);
        
        return Ok(rooms);
    }

    [HttpGet("{Id:int}")]
    public IActionResult GetRoomById(int id)
    {
        var room = Rooms.FirstOrDefault(r => r.Id == id);

        if (room is null)
            return NotFound($"Nie znaleziono pokoju o Id = {id}");
        
        return Ok(room);
    }

    [HttpGet("building/{buildingCode}")]
    public IActionResult GetRoomsByBuildingCode(string buildingCode)
    {
        var rooms = Rooms.FindAll(r => r.BuildingCode.Equals(buildingCode));
        return Ok(rooms);
    }

    [HttpPost]
    public IActionResult CreateRoom(Room room)
    {
        room.Id = Rooms.Count > 0 ? Rooms.Max(r => r.Id) + 1 : 1;
        Rooms.Add(room);
        return CreatedAtAction(nameof(GetRoomById), new {id = room.Id}, room);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateRoom(int id, [FromBody] Room room)
    {
        var roomChanged = Rooms.FirstOrDefault(r => r.Id == id);

        if (roomChanged is null) return NotFound($"Pokój o id {id} nie został odnaleziony");
        
        roomChanged.Name = room.Name;
        roomChanged.BuildingCode = room.BuildingCode;
        roomChanged.Capacity = room.Capacity;
        roomChanged.Floor = room.Floor;
        roomChanged.HasProjector = room.HasProjector;
        roomChanged.IsActive = room.IsActive;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteRoom(int id)
    {
        var roomToDelete = Rooms.FirstOrDefault(r => r.Id == id);

        if (roomToDelete is null) return NotFound($"Nie znaleziono pokoju o id {id}");
        
        bool areThereFutureReservations = ReservationsController.Reservations.Any(r =>
            r.RoomId == id &&
            r.Date > DateOnly.FromDateTime(DateTime.Now));

        if (areThereFutureReservations)
            return Conflict("Nie można usunąć tego pokoju ponieważ posiada on rezerwacje w przyszłości");
        
        Rooms.Remove(roomToDelete);

        return NoContent();
    }
   
}