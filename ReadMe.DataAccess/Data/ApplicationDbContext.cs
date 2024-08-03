using Microsoft.EntityFrameworkCore;
using ReadMe.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadMe.DataAccess.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext( DbContextOptions<ApplicationDbContext> options ): base(options)
        {
        }

       public DbSet<Category> categories {  get; set; }
    }
}
