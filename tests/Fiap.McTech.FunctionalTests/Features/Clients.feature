#language: pt
#coding:utf-8

Funcionalidade: Gerenciamento de Clientes

  Cen�rio: Criar um novo cliente
    Dado que eu tenho um ID de cliente �nico
    E que eu tenho os detalhes do cliente
    Quando eu crio um novo cliente
    Ent�o o status da resposta deve ser 201 Created
    E o cliente deve existir no sistema
    E os dados do cliente devem corresponder ao formato esperado

  Cen�rio: Criar um cliente com CPF inv�lido
    Dado que eu tenho um ID de cliente �nico
    E que eu tenho os detalhes do cliente com CPF inv�lido
    Quando eu crio um novo cliente
    Ent�o o status da resposta deve ser 400 BadRequest
    E a mensagem de erro deve ser "CPF inv�lido"

  Cen�rio: Atualizar um cliente existente
    Dado que um cliente existe no sistema
    E que eu tenho os novos detalhes do cliente
    Quando eu atualizo o cliente
    Ent�o o status da resposta deve ser 200 OK
    E os dados do cliente devem ser atualizados no sistema

  Cen�rio: Atualizar um cliente com CPF inv�lido
    Dado que um cliente existe no sistema
    E que eu tenho os novos detalhes do cliente com CPF inv�lido
    Quando eu atualizo o cliente
    Ent�o o status da resposta deve ser 400 BadRequest
    E a mensagem de erro deve ser "CPF inv�lido"

  Cen�rio: Remover um cliente existente
    Dado que um cliente existe no sistema
    Quando eu removo o cliente
    Ent�o o status da resposta deve ser 204 No Content
    E o cliente n�o deve mais existir no sistema

  Cen�rio: Tentar remover um cliente inexistente
    Dado que eu tenho um ID de cliente inexistente
    Quando eu removo o cliente
    Ent�o o status da resposta deve ser 404 Not Found
    E a mensagem de erro deve ser "Cliente n�o encontrado"
