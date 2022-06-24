using Microsoft.EntityFrameworkCore;
using WebAPI.Models;


namespace WebAPI.Data
{
    /// <summary>
    /// Isso � o que precisamos para trabalhar com EF.
    /// DataContext � a representa��o do Db em mem�ria, 
    /// fazendo mapeamento da app em rela��o ao db
    /// DbSet � a representa��o das tabelas em mem�ria, 
    /// sendo respons�veis pelas a��es dentro do db.
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