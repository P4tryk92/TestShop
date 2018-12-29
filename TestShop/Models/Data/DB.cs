using System.Data.Entity;

namespace TestShop.Models.Data
{
    public class Db :DbContext
    {
        public DbSet<PageDTO> Pages { get; set; }
        public DbSet<SidebarDTO> Sidebar { get; set; }
    }
}