using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lab1.RezervacijeProstora
{
    public class DataLoader
    {
        public async Task SimulirajUcitavanjePodatakaAsync()
        {
            Console.WriteLine("Učitavanje podataka je započelo...");
            await Task.Delay(1500);
            Console.WriteLine("Učitavanje podataka je završilo.");
        }
    }
}