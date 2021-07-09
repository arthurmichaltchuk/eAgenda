using eAgenda.Controladores.CompromissoModule;
using eAgenda.Controladores.ContatoModule;
using eAgenda.Controladores.Shared;
using eAgenda.Dominio.CompromissoModule;
using eAgenda.Dominio.ContatoModule;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAgenda.Tests.CompromissoModule
{
    [TestClass]
    public class ControladorCompromissoTest
    {
        ControladorCompromisso controlador = null;
        ControladorContato controladorContato = null;

        public ControladorCompromissoTest()
        {
            controladorContato = new ControladorContato();
            controlador = new ControladorCompromisso();
            Db.Update("DELETE FROM [TBCOMPROMISSO]");
            Db.Update("DELETE FROM [TBCONTATO]");
        }

        //INSERÇÂO
        [TestMethod]
        public void DeveInserir_Compromisso()
        {
            //arrange
            var novoContato = new Contato("José Pedro", "jose.pedro@gmail.com", "321654987", "JP Ltda", "Dev");
            var novoCompromisso = new Compromisso("Assunto", "Local", "Link", new DateTime(2002, 1, 1), new TimeSpan(08, 00, 00), new TimeSpan(09, 00, 00), novoContato);

            //action
            controladorContato.InserirNovo(novoContato);
            controlador.InserirNovo(novoCompromisso);

            //assert
            var CompromissoEncontrado = controlador.SelecionarPorId(novoCompromisso.Id);
            CompromissoEncontrado.Should().Be(novoCompromisso);
        }

        [TestMethod]
        public void DeveInserir_Compromisso_Sem_Contato()
        {
            //arrange
            var novoCompromisso = new Compromisso("Assunto", "Local", "Link", new DateTime(2002, 1, 1), new TimeSpan(08, 00, 00), new TimeSpan(09, 00, 00), null);

            //action
            controlador.InserirNovo(novoCompromisso);

            //assert
            var CompromissoEncontrado = controlador.SelecionarPorId(novoCompromisso.Id);
            CompromissoEncontrado.Should().Be(novoCompromisso);
        }

        [TestMethod]
        public void DeveRetornar_HoraInvalida()
        {
            //arrange
            var c1 = new Compromisso("Assunto", "Local", "Link", new DateTime(2002, 1, 1), new TimeSpan(14, 00, 00), new TimeSpan(16, 00, 00), null);
            controlador.InserirNovo(c1);

            //action
            bool resultado = controlador.ValidarConflitos(new DateTime(2002, 1, 1), new TimeSpan(15, 00, 00), new TimeSpan(16, 00, 00));

            //assert
            resultado.Should().Be(false);
        }

        //SELEÇÂO

        [TestMethod]
        public void DeveSelecionar_TodosContatos()
        {
            //arrange
            var c1 = new Compromisso("Assunto1", "Local", "Link", new DateTime(2002, 1, 1), new TimeSpan(08, 00, 00), new TimeSpan(09, 00, 00), null);
            controlador.InserirNovo(c1);

            var c2 = new Compromisso("Assunto2", "Local", "Link", new DateTime(2002, 1, 1), new TimeSpan(08, 00, 00), new TimeSpan(09, 00, 00), null);
            controlador.InserirNovo(c2);

            var c3 = new Compromisso("Assunto3", "Local", "Link", new DateTime(2002, 1, 1), new TimeSpan(08, 00, 00), new TimeSpan(09, 00, 00), null);
            controlador.InserirNovo(c3);

            //action
            var compromissos = controlador.SelecionarTodos();

            //assert
            compromissos.Should().HaveCount(3);
            compromissos[0].Assunto.Should().Be("Assunto1");
            compromissos[1].Assunto.Should().Be("Assunto2");
            compromissos[2].Assunto.Should().Be("Assunto3");
        }

        [TestMethod]
        public void DeveSelecionar_TodosContatosPassados()
        {
            //arrange
            var c1 = new Compromisso("Assunto1", "Local", "Link", new DateTime(2003, 1, 1), new TimeSpan(08, 00, 00), new TimeSpan(09, 00, 00), null);
            controlador.InserirNovo(c1);

            var c2 = new Compromisso("Assunto2", "Local", "Link", new DateTime(2001, 1, 1), new TimeSpan(08, 00, 00), new TimeSpan(09, 00, 00), null);
            controlador.InserirNovo(c2);

            var c3 = new Compromisso("Assunto3", "Local", "Link", new DateTime(2001, 1, 1), new TimeSpan(08, 00, 00), new TimeSpan(09, 00, 00), null);
            controlador.InserirNovo(c3);

            //action
            var compromissos = controlador.SelecionarCompromissosPassados(new DateTime(2002, 1, 1));

            //assert
            compromissos.Should().HaveCount(2);
            compromissos[0].Assunto.Should().Be("Assunto2");
            compromissos[1].Assunto.Should().Be("Assunto3");
        }

        [TestMethod]
        public void DeveSelecionar_TodosContatosFuturos()
        {
            //arrange
            var c1 = new Compromisso("Assunto1", "Local", "Link", new DateTime(2002, 2, 1), new TimeSpan(08, 00, 00), new TimeSpan(09, 00, 00), null);
            controlador.InserirNovo(c1);

            var c2 = new Compromisso("Assunto2", "Local", "Link", new DateTime(2002, 4, 1), new TimeSpan(08, 00, 00), new TimeSpan(09, 00, 00), null);
            controlador.InserirNovo(c2);

            var c3 = new Compromisso("Assunto3", "Local", "Link", new DateTime(2002, 5, 1), new TimeSpan(08, 00, 00), new TimeSpan(09, 00, 00), null);
            controlador.InserirNovo(c3);

            var c4 = new Compromisso("Assunto4", "Local", "Link", new DateTime(2002, 7, 1), new TimeSpan(08, 00, 00), new TimeSpan(09, 00, 00), null);
            controlador.InserirNovo(c4);

            var c5 = new Compromisso("Assunto5", "Local", "Link", new DateTime(2002, 9, 1), new TimeSpan(08, 00, 00), new TimeSpan(09, 00, 00), null);
            controlador.InserirNovo(c5);

            //action
            var compromissos = controlador.SelecionarCompromissosFuturos(new DateTime(2002, 1, 1), new DateTime(2002, 6, 1));

            //assert
            compromissos.Should().HaveCount(3);
            compromissos[0].Assunto.Should().Be("Assunto1");
            compromissos[1].Assunto.Should().Be("Assunto2");
            compromissos[2].Assunto.Should().Be("Assunto3");
        }

        //EDIÇÂO

        [TestMethod]
        public void DeveAtualizar_Compromisso()
        {
            //arrange
            var compromisso = new Compromisso("Assunto", "Local", "Link", new DateTime(2002, 2, 1), new TimeSpan(08, 00, 00), new TimeSpan(09, 00, 00), null);
            controlador.InserirNovo(compromisso);

            var novoCompromisso = new Compromisso("NovoAssunto", "NovoLocal", "NovoLink", new DateTime(2000, 2, 1), new TimeSpan(10, 00, 00), new TimeSpan(11, 00, 00), null);

            //action
            controlador.Editar(compromisso.Id, novoCompromisso);

            //assert
            Compromisso compromissoAtualizado = controlador.SelecionarPorId(compromisso.Id);
            compromissoAtualizado.Should().Be(novoCompromisso);
        }

        [TestMethod]
        public void DeveVerificar_DataInvalida_Compromisso()
        {
            //arrange
            var c1 = new Compromisso("Assunto1", "Local", "Link", new DateTime(2002, 2, 1), new TimeSpan(06, 00, 00), new TimeSpan(07, 00, 00), null);
            controlador.InserirNovo(c1);

            var c2 = new Compromisso("Assunto2", "Local", "Link", new DateTime(2002, 2, 1), new TimeSpan(10, 00, 00), new TimeSpan(11, 00, 00), null);
            controlador.InserirNovo(c2);

            var novoCompromisso = new Compromisso("NovoAssunto", "NovoLocal", "NovoLink", new DateTime(2002, 2, 1), new TimeSpan(10, 10, 00), new TimeSpan(10, 40, 00), null);

            //action
            string resultado = controlador.Editar(c1.Id, novoCompromisso);

            //assert
            resultado.Should().Be("Hora inválida! tente novamente...");
        }

        [TestMethod]
        public void DeveAtualizar_AdicionarContato_Compromisso()
        {
            //arrange
            var compromisso = new Compromisso("Assunto", "Local", "Link", new DateTime(2002, 2, 1), new TimeSpan(08, 00, 00), new TimeSpan(09, 00, 00), null);
            controlador.InserirNovo(compromisso);
            var novoContato = new Contato("José Pedro", "jose.pedro@gmail.com", "321654987", "JP Ltda", "Dev");
            controladorContato.InserirNovo(novoContato);

            var novoCompromisso = new Compromisso("NovoAssunto", "NovoLocal", "NovoLink", new DateTime(2000, 2, 1), new TimeSpan(10, 00, 00), new TimeSpan(11, 00, 00), novoContato);

            //action
            controlador.Editar(compromisso.Id, novoCompromisso);

            //assert
            Compromisso compromissoAtualizado = controlador.SelecionarPorId(compromisso.Id);
            compromissoAtualizado.Should().Be(novoCompromisso);
        }

        [TestMethod]
        public void DeveAtualizar_RetirarContato_Compromisso()
        {
            //arrange

            var novoContato = new Contato("José Pedro", "jose.pedro@gmail.com", "321654987", "JP Ltda", "Dev");
            var compromisso = new Compromisso("Assunto", "Local", "Link", new DateTime(2002, 2, 1), new TimeSpan(08, 00, 00), new TimeSpan(09, 00, 00), novoContato);

            controladorContato.InserirNovo(novoContato);
            controlador.InserirNovo(compromisso);

            var novoCompromisso = new Compromisso("NovoAssunto", "NovoLocal", "NovoLink", new DateTime(2000, 2, 1), new TimeSpan(10, 00, 00), new TimeSpan(11, 00, 00), null);

            //action
            controlador.Editar(compromisso.Id, novoCompromisso);

            //assert
            Compromisso compromissoAtualizado = controlador.SelecionarPorId(compromisso.Id);
            compromissoAtualizado.Should().Be(novoCompromisso);
        }

        //EXCLUIR
        [TestMethod]
        public void DeveExcluir_Contato()
        {
            //arrange            
            var compromisso = new Compromisso("Assunto", "Local", "Link", new DateTime(2002, 2, 1), new TimeSpan(08, 00, 00), new TimeSpan(09, 00, 00), null);
            controlador.InserirNovo(compromisso);

            //action            
            controlador.Excluir(compromisso.Id);

            //assert
            Compromisso compromissoEncontrado = controlador.SelecionarPorId(compromisso.Id);
            compromissoEncontrado.Should().BeNull();
        }
    }
}
