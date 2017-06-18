using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication2
{
    public class Agenda
    {
        public List<scheduled> events = new List<scheduled>();

        public int eventsum()
        {
            return events.Count;
        }

        public scheduled getScheduled(int number)
        {
            return events.ElementAt(number);
        }

        public Agenda()
        {
            DateTime date;
            int recipeIndex;
            scheduled eventToAdd;
            /*
            date = new DateTime(2014, 12, 7);
            recipeIndex = 2;
            eventToAdd= new scheduled(date, recipeIndex);
            events.Add(eventToAdd);
             */


        }
    }
}
