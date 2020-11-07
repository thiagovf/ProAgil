using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public class ProAgilRepository : IProAgilRepository
    {
        private readonly ProAgilContext context;
        public ProAgilRepository(ProAgilContext context)
        {
            this.context = context;
            this.context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        // Gerais
        public void Add<T>(T entity) where T : class
        {
            context.Add(entity);
        }
        public void Update<T>(T entity) where T : class
        {
            context.Update(entity);
        }
        public void Delete<T>(T entity) where T : class
        {
            context.Remove(entity);
        }
        public async Task<bool> SaveChangesAssync()
        {
            return (await context.SaveChangesAsync()) > 0;
        }

        // Eventos
        public async Task<Evento[]> GetAllEventosAssync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = context.Eventos
                .Include(c => c.Lotes)
                .Include(c => c.RedesSociais);

            if (includePalestrantes)
            {
                query = query
                    .Include(pe => pe.PalestranteEventos)
                    .ThenInclude(p => p.Palestrante);
            }

            query = query.AsNoTracking()
                .OrderBy(c => c.Id);
            return await query.ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventosAssync(string tema, bool includePalestrantes)
        {
            IQueryable<Evento> query = context.Eventos
                .Include(c => c.Lotes)
                .Include(c => c.RedesSociais);

            if (includePalestrantes)
            {
                query = query
                    .Include(pe => pe.PalestranteEventos)
                    .ThenInclude(p => p.Palestrante);
            }

            query = query.AsNoTracking()
                .OrderBy(c => c.Id)
                .Where(c => c.Tema.ToLower().Contains(tema.ToLower()));
            return await query.ToArrayAsync();
        }

        public async Task<Evento> GetEventoAssync(int eventoId, bool includePalestrantes)
        {
            IQueryable<Evento> query = context.Eventos
                .Include(c => c.Lotes)
                .Include(c => c.RedesSociais);
            if (includePalestrantes)
            {
                query = query
                    .Include(pe => pe.PalestranteEventos)
                    .ThenInclude(p => p.Palestrante);
            }
            query = query.AsNoTracking()
                .OrderByDescending(c => c.DataEvento)
                .Where(c => c.Id == eventoId);
            return await query.FirstOrDefaultAsync();

        }
        // Palestrantes
        public async Task<Palestrante[]> GetAllPalestrantesAssync(string palestranteNome, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = context.Palestrantes
                .Include(p =>  p.RedesSociais);
            if (includeEventos)
            {
                query = query
                    .Include(pe => pe.PalestranteEventos)
                    .ThenInclude(e => e.Evento);
            }
            
            query = query.AsNoTracking()
                .OrderBy(p => p.Nome)
                .Where(p => p.Nome.ToLower().Contains(palestranteNome.ToLower()));
            return await query.ToArrayAsync();
        }

        public async Task<Palestrante> GetPalestranteAssync(int palestranteId, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = context.Palestrantes
                .Include(p => p.RedesSociais);
            if (includeEventos)
            {
                query = query
                    .Include(pe => pe.PalestranteEventos)
                    .ThenInclude(e => e.Evento);
            }

            query = query.AsNoTracking()
                .OrderBy(p => p.Nome)
                .Where(p => p.Id == palestranteId);
            return await query.FirstOrDefaultAsync();
        }

        
    }
}