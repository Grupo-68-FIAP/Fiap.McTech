#language: pt
#coding:utf-8

Funcionalidade: Gerenciamento de Pedidos

  Cen�rio: Criar um novo pedido com cliente autenticado
    Dado que eu tenho um cliente autenticado
    E que eu tenho os detalhes do pedido
    Quando eu crio um novo pedido
    Ent�o o status da resposta deve ser 201 Created
    E o pedido deve existir no sistema
    E os dados do pedido devem corresponder ao formato esperado

  Cen�rio: Criar um novo pedido sem cliente
    Dado que eu tenho os detalhes do pedido sem cliente
    Quando eu crio um novo pedido
    Ent�o o status da resposta deve ser 400 BadRequest
    E a mensagem de erro deve ser "Cliente n�o autenticado"

  Cen�rio: Adicionar item ao pedido existente
    Dado que um pedido existe no sistema
    E que eu tenho os detalhes do item
    Quando eu adiciono o item ao pedido
    Ent�o o status da resposta deve ser 200 OK
    E o item deve ser adicionado ao pedido
    E os dados do pedido devem ser atualizados no sistema

  Cen�rio: Tentar criar um pedido com produto inexistente
    Dado que eu tenho os detalhes do pedido com produto inexistente
    Quando eu crio um novo pedido
    Ent�o o status da resposta deve ser 404 Not Found
    E a mensagem de erro deve ser "Produto n�o encontrado"

  Cen�rio: Tentar criar um pedido com cliente inexistente
    Dado que eu tenho os detalhes do pedido com cliente inexistente
    Quando eu crio um novo pedido
    Ent�o o status da resposta deve ser 404 Not Found
    E a mensagem de erro deve ser "Cliente n�o encontrado"

  Cen�rio: Atualizar um pedido existente
    Dado que um pedido existe no sistema
    E que eu tenho os novos detalhes do pedido
    Quando eu atualizo o pedido
    Ent�o o status da resposta deve ser 200 OK
    E os dados do pedido devem ser atualizados no sistema

  Cen�rio: Remover um pedido existente
    Dado que um pedido existe no sistema
    Quando eu removo o pedido
    Ent�o o status da resposta deve ser 204 No Content
    E o pedido n�o deve mais existir no sistema

  Cen�rio: Tentar remover um pedido inexistente
    Dado que eu tenho um ID de pedido inexistente
    Quando eu removo o pedido
    Ent�o o status da resposta deve ser 404 Not Found
    E a mensagem de erro deve ser "Pedido n�o encontrado"
