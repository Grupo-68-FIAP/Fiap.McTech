﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (https://www.specflow.org/).
//      SpecFlow Version:3.9.0.0
//      SpecFlow Generator Version:3.9.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Fiap.McTech.FunctionalTests.Features
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class GerenciamentoDeCarrinhoDeComprasFeature : object, Xunit.IClassFixture<GerenciamentoDeCarrinhoDeComprasFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = ((string[])(null));
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "Cart.feature"
#line hidden
        
        public GerenciamentoDeCarrinhoDeComprasFeature(GerenciamentoDeCarrinhoDeComprasFeature.FixtureData fixtureData, Fiap_McTech_FunctionalTests_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("pt"), "Features", "Gerenciamento de Carrinho de Compras", "  Cen�rio: Criar um novo carrinho com cliente autenticado\r\n    Dado que eu tenho " +
                    "um cliente autenticado\r\n    E que eu tenho os detalhes do carrinho\r\n    Quando e" +
                    "u crio um novo carrinho\r\n    Ent�o o status da resposta deve ser 201 Created\r\n  " +
                    "  E o carrinho deve existir no sistema\r\n    E os dados do carrinho devem corresp" +
                    "onder ao formato esperado\r\n\r\n  Cen�rio: Criar um novo carrinho sem cliente\r\n    " +
                    "Dado que eu tenho os detalhes do carrinho sem cliente\r\n    Quando eu crio um nov" +
                    "o carrinho\r\n    Ent�o o status da resposta deve ser 201 Created\r\n    E o carrinh" +
                    "o deve existir no sistema\r\n    E os dados do carrinho devem corresponder ao form" +
                    "ato esperado\r\n\r\n  Cen�rio: Adicionar produto ao carrinho existente\r\n    Dado que" +
                    " um carrinho existe no sistema\r\n    E que eu tenho os detalhes do produto\r\n    Q" +
                    "uando eu adiciono o produto ao carrinho\r\n    Ent�o o status da resposta deve ser" +
                    " 200 OK\r\n    E o produto deve ser adicionado ao carrinho\r\n    E os dados do carr" +
                    "inho devem ser atualizados no sistema\r\n\r\n  Cen�rio: Atualizar a quantidade de um" +
                    " item no carrinho\r\n    Dado que um carrinho com itens existe para um cliente\r\n  " +
                    "  Quando eu atualizo a quantidade de um item no carrinho\r\n    Ent�o o status da " +
                    "resposta deve ser 200 OK\r\n    E a quantidade do item deve ser atualizada no carr" +
                    "inho\r\n\r\n  Cen�rio: Tentar criar um carrinho com produto inexistente\r\n    Dado qu" +
                    "e eu tenho os detalhes do carrinho com produto inexistente\r\n    Quando eu crio u" +
                    "m novo carrinho\r\n    Ent�o o status da resposta deve ser 404 Not Found\r\n    E a " +
                    "mensagem de erro deve ser \"Produto n�o encontrado\"\r\n\r\n  Cen�rio: Tentar criar um" +
                    " carrinho com cliente inexistente\r\n    Dado que eu tenho os detalhes do carrinho" +
                    " com cliente inexistente\r\n    Quando eu crio um novo carrinho\r\n    Ent�o o statu" +
                    "s da resposta deve ser 404 Not Found\r\n    E a mensagem de erro deve ser \"Cliente" +
                    " n�o encontrado\"", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        public virtual void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        void System.IDisposable.Dispose()
        {
            this.TestTearDown();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                GerenciamentoDeCarrinhoDeComprasFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                GerenciamentoDeCarrinhoDeComprasFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
