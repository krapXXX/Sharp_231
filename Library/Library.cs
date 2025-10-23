using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Library
{
    public class Library
    {
        private List<Literature> Funds { get; set; } = [];

        public Library()
        {
            Funds.Add(new Book
            {
                Author = "D. Knuth",
                Title = "The art of programming",
                Publisher = "Київ, Наукова Думка",
                Year = 2000
            });



            Funds.Add(new Book
            {


                Author = "J. Richterr",
                Title = "CLR via C#",
                Publisher = "Microsoft Press",
                Year = 2013
            });

            Funds.Add(new Journal
            {
                Title = "ArgC & ArgV",
                Number = "2(113), 2000",
                Publisher = "https://journals.ua/technology/argc-argv/"
            });

            Funds.Add(new Newspaper
            {
                Title="Урядовий кур'єр",
                Date="23.10.2025",
                Publisher="Газета Кабінету Міністрів України"
            });

            Funds.Add(new Booklet
            {
                Title ="New Opening!",
                Subject = "Sweet Restaurant",
                Author ="PopShop"
            });

        }


        public void PrintCatalog()
        {
            foreach (Literature literature in Funds)
            {
                Console.WriteLine(literature.GetCard());
            }
        }
    }

}
