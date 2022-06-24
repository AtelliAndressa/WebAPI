using Microsoft.EntityFrameworkCore;
using WebAPI.Models;


namespace WebAPI.Data
{
    /// <summary>
    /// Isso é o que precisamos para trabalhar com EF.
    /// DataContext é a representação do Db em memória, 
    /// fazendo mapeamento da app em relação ao db
    /// DbSet é a representação das tabelas em memória, 
    /// sendo responsáveis pelas ações dentro do db.
    /// </summary>
    public class DataContext : DbContext
    {
        //construtor da class
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<User> User { get; set; }

    }
}