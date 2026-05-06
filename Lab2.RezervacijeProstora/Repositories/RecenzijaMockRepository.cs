using Lab2.RezervacijeProstora.Models;
using System.Linq;

namespace Lab2.RezervacijeProstora.Repositories
{
    public class RecenzijaMockRepository
    {
        public List<Recenzija> GetAll()
        {
            return MockDataStore.Recenzije;
        }

        public Recenzija? GetById(int id)
        {
            return MockDataStore.Recenzije.FirstOrDefault(r => r.Id == id);
        }
    }
}