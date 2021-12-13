using IEFCore.Domain;
using IEFCore.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            using var db = new Data.ApplicationContext();

            var migrationPending = db.Database.GetPendingMigrations().Any();

            if (migrationPending) db.Database.Migrate();

            /* 
            if (db.Database.GetPendingMigrations().Any()) db.Database.Migrate();
            */

            //InserirDados();

            //InserirDadosMassa();

            //ConsultaClientes();

            //CadastrarPedido();

            //ConsultarPedidoCarregamentoAdiantado();

            //AtualizarDados();

            RemoverDados();

            /*
            Get-Help EntityFrameworkCore
            Add-Migration [Nome]
            Update-Database
            Remove-Migration [Nome] / null = ultima migration criada
            Script-Migration [Nome Arquivo] / -i criar Script Idempotente
            
             */
        }

        private static void InserirDados()
        {
            var produto = new Produto
            {
                Descricao = "My Product",
                CodigoBarras = "1234567891234",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaRevenda,
                Ativo = true
            };

            using var db = new Data.ApplicationContext();

            //db.Produtos.Add(produto); 

            //db.Set<Produto>().Add(produto);

            //db.Entry(produto).State = EntityState.Added;

            db.Add(produto);

            var dados = db.SaveChanges();

            if (dados > 0) Console.WriteLine($"Linhas Afetadas: {dados}");

        }
        private static void InserirDadosMassa()
        {
            /*
            var produto = new Produto
            {
                Descricao = "My Product",
                CodigoBarras = "1234567891234",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaRevenda,
                Ativo = true
            };

            var cliente = new Cliente
            {
                Nome = "João Stahl",
                CEP = "14700000",
                Cidade = "Bebedouro",
                Estado = "SP",
                Telefone = "17900000000"
            };
            */

            var listaClientes = new[]
            {
                new Cliente
                {
                     Nome = "João Stahl",
                    CEP = "14500000",
                    Cidade = "Bebedouro",
                    Estado = "SP",
                    Telefone = "17900000000"
                },
                new Cliente
                {
                     Nome = "João Girardi",
                    CEP = "14600000",
                    Cidade = "Bebedouro",
                    Estado = "SP",
                    Telefone = "17900000000"
                },
                new Cliente
                {
                     Nome = "João Vitor",
                    CEP = "14400000",
                    Cidade = "Bebedouro",
                    Estado = "SP",
                    Telefone = "17900000000"
                }
            };

            using var db = new Data.ApplicationContext();

            //db.AddRange(produto, cliente);
            
            //db.Clientes.AddRange(listaClientes);

            //db.Set<Cliente>().AddRange(listaClientes);

            var dados = db.SaveChanges();

            Console.WriteLine($"Linhas Afetadas: {dados}");

        }

        private static void ConsultaClientes()
        {
            using var db = new Data.ApplicationContext();

            //var querySintaxe = (from c in db.Clientes where c.Id > 0 select c).ToList();
            
            var queryMetodo = db.Clientes
                .AsNoTracking()
                .Where(c => c.Id > 0)
                .OrderBy(c => c.Nome)
                .ToList();

            foreach (var cliente in queryMetodo)
            {
                Console.WriteLine($"Consultado o cliente Id: {cliente.Id}");

                //db.Clientes.Find(cliente.Id);

                db.Clientes.FirstOrDefault(c => c.Id == cliente.Id);
            }
            
        }
        private static void CadastrarPedido()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Clientes.FirstOrDefault();
            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now.AddDays(1),
                Observacao = "Novo Pedido",
                Status = StatusPedido.Finalizado,
                TipoFrete = TipoFrete.SemFrete,
                Itens = new List<PedidoItem>
                {
                    new PedidoItem
                    {
                        ProdutoId = produto.Id,
                        Desconto = 2,
                        Quantidade = 2,
                        Valor = 20
                    }
                }
            };

            db.Pedidos.Add(pedido);

            db.SaveChanges();
        }

        private static void ConsultarPedidoCarregamentoAdiantado()
        {
            using var db = new Data.ApplicationContext();

            var pedidos = db.Pedidos
                .Include(p => p.Itens)
                    .ThenInclude(p => p.Produto)
                .Include("Clientes")
                .ToList();

            Console.WriteLine(pedidos.Count);
        }

        private static void AtualizarDados()
        {
            using var db = new Data.ApplicationContext();

            //var cliente = db.Clientes.Find(1);

            var cliente = new Cliente
            {
                Id = 1
            };

            db.Attach(cliente);

            var clienteDesconectado = new
            {
                Nome = "João",
                Telefone = "17123451234"
            };

            db.Entry(cliente).CurrentValues.SetValues(clienteDesconectado);

            db.Entry(cliente).State = EntityState.Modified;

            //db.Clientes.Update(cliente);

            db.SaveChanges();
        }
        private static void RemoverDados()
        {
            using var db = new Data.ApplicationContext();

            //var cliente = db.Clientes.OrderByDescending(c => c.Id).FirstOrDefault();

            /*
           db.Clientes.Remove(cliente);

           db.Remove(cliente);

           db.Entry(cliente).State = EntityState.Deleted;
           */

            var clienteDesconectado = new Cliente
            {
                Id = 3
            };

            db.Entry(clienteDesconectado).State = EntityState.Deleted;           

            db.SaveChanges();
        }

    }
}
