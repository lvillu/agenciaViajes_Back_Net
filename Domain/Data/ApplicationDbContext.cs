﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
  public class ApplicationDbContext : DbContext
  {

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    { }


    public DbSet<User> Users { get; set; }
  }
}


/*
 * Default Project : Domain
 * Add-Migration Nombre de la migracion
 * Update-Database
 * */