using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication2
{
    public class scheduled
    {
        DateTime date;
        Recipe current;
        


        public scheduled(DateTime date, Recipe current)
        {
            this.date = date;
            this.current = current;
           
        }

        public DateTime getDate()
        {
            return date;
        }

        public Recipe getrec()
        {
            return current;
        }

        
        
     

    }
}
