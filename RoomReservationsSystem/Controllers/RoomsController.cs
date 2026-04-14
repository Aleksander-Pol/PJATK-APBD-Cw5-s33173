using System.Collections;
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
        Rooms.Add(room);
        return Ok(Rooms);
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

        Rooms.Remove(roomToDelete);

        return Ok(Rooms);
    }
   
}