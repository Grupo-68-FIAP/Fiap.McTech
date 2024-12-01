#language: pt
#coding:utf-8

Funcionalidade: Gerenciamento de Carrinho de Compras

  Cenário: Criar um novo carrinho com cliente autenticado
    Dado que eu tenho um cliente autenticado
    E que eu tenho os detalhes do carrinho
    Quando eu crio um novo carrinho
    Então o status da resposta deve ser 201 Created
    E o carrinho deve existir no sistema
    E os dados do carrinho devem corresponder ao formato esperado

  Cenário: Criar um novo carrinho sem cliente
    Dado que eu tenho os detalhes do carrinho sem cliente
    Quando eu crio um novo carrinho
    Então o status da resposta deve ser 201 Created
    E o carrinho deve existir no sistema
    E os dados do carrinho devem corresponder ao formato esperado

  Cenário: Adicionar produto ao carrinho existente
    Dado que um carrinho existe no sistema
    E que eu tenho os detalhes do produto
    Quando eu adiciono o produto ao carrinho
    Então o status da resposta deve ser 200 OK
    E o produto deve ser adicionado ao carrinho
    E os dados do carrinho devem ser atualizados no sistema

  Cenário: Atualizar a quantidade de um item no carrinho
    Dado que um carrinho com itens existe para um cliente
    Quando eu atualizo a quantidade de um item no carrinho
    Então o status da resposta deve ser 200 OK
    E a quantidade do item deve ser atualizada no carrinho

  Cenário: Tentar criar um carrinho com produto inexistente
    Dado que eu tenho os detalhes do carrinho com produto inexistente
    Quando eu crio um novo carrinho
    Então o status da resposta deve ser 404 Not Found
    E a mensagem de erro deve ser "Produto não encontrado"

  Cenário: Tentar criar um carrinho com cliente inexistente
    Dado que eu tenho os detalhes do carrinho com cliente inexistente
    Quando eu crio um novo carrinho
    Então o status da resposta deve ser 404 Not Found
    E a mensagem de erro deve ser "Cliente não encontrado"