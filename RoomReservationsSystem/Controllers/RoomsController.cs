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
    public IActionResult GetRooms()
    {
        return Ok(Rooms);
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
}