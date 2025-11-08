using SentinelTrack.Application.Interfaces;
using SentinelTrack.Infrastructure.Context;
using SentinelTrack.Domain.Entities;

namespace SentinelTrack.Infrastructure.Data;

public class DataSeeder : IDataSeeder
{
    private readonly AppDbContext _db;

    public DataSeeder(AppDbContext db)
    {
        _db = db;
    }

    public void Seed()
    {
        if (_db.Yards.Any()) return;

        var y1 = new Yard { Id = Guid.NewGuid(), Name = "Pátio Central", Address = "Av. Paulista, 1000", PhoneNumber = "+55 11 4002-8922", Capacity = 150 };
        _db.Yards.Add(y1);
        _db.Users.Add(new User { Id = Guid.NewGuid(), Name = "Admin", Email = "admin@sentineltrack.com", Role = "admin" });
        _db.Motos.Add(new Moto { Id = Guid.NewGuid(), Plate = "ABC1D23", Model = "Honda CG 160", Color = "Preta", YardId = y1.Id });
        _db.SaveChanges();
    }
}