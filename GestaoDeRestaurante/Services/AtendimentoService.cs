using GestaoDeRestaurante.Data;
using GestaoDeRestaurante.DTOs.Atendimento;
using GestaoDeRestaurante.Enums;
using GestaoDeRestaurante.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeRestaurante.Services
{
    public class AtendimentoService
    {
        private readonly AppDbContext _context;

        public AtendimentoService(AppDbContext context)
        {
            _context = context;
        }

        // 🔹 CREATE
        public async Task<AtendimentoResponseDTO> CriarAtendimento(AtendimentoRequestDTO dto)
        {
            Atendimento atendimento;

            // 🔥 Decide qual tipo criar
            if (dto.TipoAtendimento == TipoAtendimento.AtendimentoPresencial)
            {
                atendimento = new AtendimentoPresencial();
            }
            else if (dto.TipoAtendimento == TipoAtendimento.DeliveryProprio)
            {
                atendimento = new AtendimentoDeliveryProprio
                {
                    ObservacaoEntrega = dto.ObservacaoEntrega
                };
            }
            else // DeliveryAplicativo
            {
                atendimento = new AtendimentoDeliveryAplicativo
                {
                    NomeAplicativo = dto.NomeAplicativo ?? "App"
                };
            }

            atendimento.TipoAtendimento = dto.TipoAtendimento;
            atendimento.DataHora = DateTime.Now;
            atendimento.TaxaEntrega = 0; // será calculado no Pedido

            _context.Atendimentos.Add(atendimento);
            await _context.SaveChangesAsync();

            return new AtendimentoResponseDTO
            {
                Id = atendimento.Id,
                TipoAtendimento = atendimento.TipoAtendimento,
                DataHora = atendimento.DataHora,
                TaxaEntrega = atendimento.TaxaEntrega
            };
        }

        // 🔹 GET ALL
        public async Task<List<AtendimentoResponseDTO>> ListarAtendimentos()
        {
            var atendimentos = await _context.Atendimentos.ToListAsync();

            return atendimentos.Select(a => new AtendimentoResponseDTO
            {
                Id = a.Id,
                TipoAtendimento = a.TipoAtendimento,
                DataHora = a.DataHora,
                TaxaEntrega = a.TaxaEntrega
            }).ToList();
        }

        // 🔹 GET BY ID
        public async Task<AtendimentoResponseDTO> BuscarAtendimentoPorId(int id)
        {
            var atendimento = await _context.Atendimentos.FindAsync(id);

            if (atendimento == null)
                throw new Exception("Atendimento não encontrado.");

            return new AtendimentoResponseDTO
            {
                Id = atendimento.Id,
                TipoAtendimento = atendimento.TipoAtendimento,
                DataHora = atendimento.DataHora,
                TaxaEntrega = atendimento.TaxaEntrega
            };
        }

        public async Task<AtendimentoResponseDTO> AtualizarAtendimento(int id, AtendimentoRequestDTO dto)
        {
            var atendimento = await _context.Atendimentos.FindAsync(id);
            if (atendimento == null)
                throw new Exception("Atendimento nao encontrado.");

            atendimento.TipoAtendimento = dto.TipoAtendimento;

            if (atendimento is AtendimentoDeliveryProprio deliveryProprio)
                deliveryProprio.ObservacaoEntrega = dto.ObservacaoEntrega;
            else if (atendimento is AtendimentoDeliveryAplicativo deliveryApp)
                deliveryApp.NomeAplicativo = dto.NomeAplicativo ?? "App";

            await _context.SaveChangesAsync();

            return new AtendimentoResponseDTO
            {
                Id = atendimento.Id,
                TipoAtendimento = atendimento.TipoAtendimento,
                DataHora = atendimento.DataHora,
                TaxaEntrega = atendimento.TaxaEntrega
            };
        }

        // 🔹 DELETE (opcional)
        public async Task<bool> DeletarAtendimento(int id)
        {
            var atendimento = await _context.Atendimentos.FindAsync(id);

            if (atendimento == null)
                throw new Exception("Atendimento não encontrado.");

            _context.Atendimentos.Remove(atendimento);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}