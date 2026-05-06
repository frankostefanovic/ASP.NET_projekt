using Lab2.RezervacijeProstora.Models;
using System.Linq;

namespace Lab2.RezervacijeProstora.Repositories
{
    public class LokacijaMockRepository
    {
        public List<Lokacija> GetAll()
        {
            return MockDataStore.Lokacije;
        }

        public Lokacija? GetById(int id)
        {
            return MockDataStore.Lokacije.FirstOrDefault(l => l.Id == id);
        }
    }
}