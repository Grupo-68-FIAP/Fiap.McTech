#language: pt
#coding:utf-8

Funcionalidade: Gerenciamento de Pedidos

  Cenário: Criar um novo pedido com cliente autenticado
    Dado que eu tenho um cliente autenticado
    E que eu tenho os detalhes do pedido
    Quando eu crio um novo pedido
    Então o status da resposta deve ser 201 Created
    E o pedido deve existir no sistema
    E os dados do pedido devem corresponder ao formato esperado

  Cenário: Criar um novo pedido sem cliente
    Dado que eu tenho os detalhes do pedido sem cliente
    Quando eu crio um novo pedido
    Então o status da resposta deve ser 400 BadRequest
    E a mensagem de erro deve ser "Cliente não autenticado"

  Cenário: Adicionar item ao pedido existente
    Dado que um pedido existe no sistema
    E que eu tenho os detalhes do item
    Quando eu adiciono o item ao pedido
    Então o status da resposta deve ser 200 OK
    E o item deve ser adicionado ao pedido
    E os dados do pedido devem ser atualizados no sistema

  Cenário: Tentar criar um pedido com produto inexistente
    Dado que eu tenho os detalhes do pedido com produto inexistente
    Quando eu crio um novo pedido
    Então o status da resposta deve ser 404 Not Found
    E a mensagem de erro deve ser "Produto não encontrado"

  Cenário: Tentar criar um pedido com cliente inexistente
    Dado que eu tenho os detalhes do pedido com cliente inexistente
    Quando eu crio um novo pedido
    Então o status da resposta deve ser 404 Not Found
    E a mensagem de erro deve ser "Cliente não encontrado"

  Cenário: Atualizar um pedido existente
    Dado que um pedido existe no sistema
    E que eu tenho os novos detalhes do pedido
    Quando eu atualizo o pedido
    Então o status da resposta deve ser 200 OK
    E os dados do pedido devem ser atualizados no sistema

  Cenário: Remover um pedido existente
    Dado que um pedido existe no sistema
    Quando eu removo o pedido
    Então o status da resposta deve ser 204 No Content
    E o pedido não deve mais existir no sistema

  Cenário: Tentar remover um pedido inexistente
    Dado que eu tenho um ID de pedido inexistente
    Quando eu removo o pedido
    Então o status da resposta deve ser 404 Not Found
    E a mensagem de erro deve ser "Pedido não encontrado"
