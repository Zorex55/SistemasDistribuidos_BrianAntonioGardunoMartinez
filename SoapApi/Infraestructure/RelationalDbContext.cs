using Microsoft.EntityFrameworkCore;
using SoapApi.Infraestructure.Entities;

namespace SoapApi.Infraestructure;

public class RelationalDbContext : DbContext{
    public RelationalDbContext(DbContextOptions<RelationalDbContext> options) : base(options){
        
    }

    public DbSet<UserEntity> Users {get; set;}
}