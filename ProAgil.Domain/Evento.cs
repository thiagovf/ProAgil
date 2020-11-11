using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProAgil.Domain
{
    public class Evento
    {
        public int Id { get; set; }
        [Required (ErrorMessage="O campo {0} deve ser preenchido.")]
        [StringLength (100, MinimumLength=3, ErrorMessage="Local deve ter entre 3 e 100 caracteres.")]
        public string Local { get; set; }
        public DateTime DataEvento { get; set; }
        [Required (ErrorMessage="O {0} deve ser preenchido.")]
        public string Tema { get; set; }
        [Range(1, 120000, ErrorMessage = "Qtd de pessoas é entre 2 e 120.000.")]
        public int QtdPessoas { get; set; }
        public string ImagemURL { get; set; }
        [Phone]
        public string Telefone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public List<Lote> Lotes { get; set; }
        public List<RedeSocial> RedesSociais { get; set; }
        public List<PalestranteEvento> PalestranteEventos { get; set; }

    }
}