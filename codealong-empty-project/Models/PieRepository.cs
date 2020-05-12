using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace codealong_pie_project.Models
{
    public class PieRepository : IPieRepository
    {
        private readonly AppDbContext appDbContext;
        public PieRepository(AppDbContext appDb)
        {
            appDbContext = appDb;
        }

        public IEnumerable<Pie> AllPies { 
        get
            {
                return appDbContext.Pies.Include(c => c.Category); //inkluderar pie-propertyt Category i hämtningen av dbsettet Pies
            }
        }

        public IEnumerable<Pie> PiesOfTheWeek {
            get
            {
                return appDbContext.Pies.Include(c => c.Category).Where(p=>p.IsPieOfTheWeek); 
                //inkluderar bara pajer som isPieOfTheWeek = true i hämtningen av dbsettet Pies
            }

        }

        public Pie GetPieById(int pieId)
        {
            return appDbContext.Pies.FirstOrDefault(p => p.PieId == pieId);
            //Returnerar första pajen vars id matchar
        }
    }
}
