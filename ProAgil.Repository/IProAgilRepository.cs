using System.Threading.Tasks;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public interface IProAgilRepository
    {
        // Geral
         void Add<T>(T entity) where T : class;
         void Update<T>(T entity) where T : class;
         void Delete<T>(T entity) where T : class;
         Task<bool> SaveChangesAssync();

         // Eventos
         Task<Evento[]> GetAllEventosAssync(string tema, bool includePalestrantes);
         Task<Evento[]> GetAllEventosAssync(bool includePalestrantes);
         Task<Evento> GetEventoAssync(int eventoId, bool includePalestrantes);

         // Palestrantes
        Task<Palestrante[]> GetAllPalestrantesAssync(string palestranteNome, bool includeEventos);
         Task<Palestrante> GetPalestranteAssync(int palestranteId, bool includeEventos);

         
    }
}