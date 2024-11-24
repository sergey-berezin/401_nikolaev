using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticSquares;
using Microsoft.EntityFrameworkCore;

namespace GeneticApplication
{
    public class PopulationEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public int num1 { get; set; }
        public int num2 { get; set; }
        public int num3 { get; set; }
        public ICollection<IndividualEntity> Individuals { get; set; }
    }

    public class IndividualEntity
    {
        public int Id { get; set; }
        public PopulationEntity Population { get; set; }
        public ICollection<GenEntity> Gens { get; set; }
    }
    public class GenEntity
    {
        public int Id { get; set; }
        public IndividualEntity Individual { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Size { get; set; }
    }
    class LibraryContext : DbContext
    {
        public DbSet<IndividualEntity> AllIndividuals { get; set; }
        public DbSet<GenEntity> AllGens { get; set;}
        public DbSet<PopulationEntity> AllPopulations { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder o)
            => o.UseSqlite("Data Source=GeneticLibrary.db");
    }
}
