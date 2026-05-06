using Lab2.RezervacijeProstora.Models;
using System.Linq;

namespace Lab2.RezervacijeProstora.Repositories
{
    public class RezervacijaMockRepository
    {
        public List<Rezervacija> GetAll()
        {
            return MockDataStore.Rezervacije;
        }

        public Rezervacija? GetById(int id)
        {
            return MockDataStore.Rezervacije.FirstOrDefault(r => r.Id == id);
        }
    }
}