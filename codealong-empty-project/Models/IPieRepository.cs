using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace codealong_pie_project.Models
{
    public interface IPieRepository
    {
        IEnumerable<Pie> AllPies { get; }
        IEnumerable<Pie> PiesOfTheWeek { get; }
        Pie GetPieById(int pieId);
    }
}
