#language: pt
#coding:utf-8

Funcionalidade: Gerenciamento de Carrinho de Compras

  Cen�rio: Criar um novo carrinho com cliente autenticado
    Dado que eu tenho um cliente autenticado
    E que eu tenho os detalhes do carrinho
    Quando eu crio um novo carrinho
    Ent�o o status da resposta deve ser 201 Created
    E o carrinho deve existir no sistema
    E os dados do carrinho devem corresponder ao formato esperado

  Cen�rio: Criar um novo carrinho sem cliente
    Dado que eu tenho os detalhes do carrinho sem cliente
    Quando eu crio um novo carrinho
    Ent�o o status da resposta deve ser 201 Created
    E o carrinho deve existir no sistema
    E os dados do carrinho devem corresponder ao formato esperado

  Cen�rio: Adicionar produto ao carrinho existente
    Dado que um carrinho existe no sistema
    E que eu tenho os detalhes do produto
    Quando eu adiciono o produto ao carrinho
    Ent�o o status da resposta deve ser 200 OK
    E o produto deve ser adicionado ao carrinho
    E os dados do carrinho devem ser atualizados no sistema

  Cen�rio: Atualizar a quantidade de um item no carrinho
    Dado que um carrinho com itens existe para um cliente
    Quando eu atualizo a quantidade de um item no carrinho
    Ent�o o status da resposta deve ser 200 OK
    E a quantidade do item deve ser atualizada no carrinho

  Cen�rio: Tentar criar um carrinho com produto inexistente
    Dado que eu tenho os detalhes do carrinho com produto inexistente
    Quando eu crio um novo carrinho
    Ent�o o status da resposta deve ser 404 Not Found
    E a mensagem de erro deve ser "Produto n�o encontrado"

  Cen�rio: Tentar criar um carrinho com cliente inexistente
    Dado que eu tenho os detalhes do carrinho com cliente inexistente
    Quando eu crio um novo carrinho
    Ent�o o status da resposta deve ser 404 Not Found
    E a mensagem de erro deve ser "Cliente n�o encontrado"