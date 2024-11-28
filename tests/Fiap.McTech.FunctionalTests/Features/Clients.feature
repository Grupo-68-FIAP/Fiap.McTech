#language: pt
#coding:utf-8

Funcionalidade: Gerenciamento de Clientes

  Cenário: Criar um novo cliente
    Dado que eu tenho um ID de cliente único
    E que eu tenho os detalhes do cliente
    Quando eu crio um novo cliente
    Então o status da resposta deve ser 201 Created
    E o cliente deve existir no sistema
    E os dados do cliente devem corresponder ao formato esperado

  Cenário: Criar um cliente com CPF inválido
    Dado que eu tenho um ID de cliente único
    E que eu tenho os detalhes do cliente com CPF inválido
    Quando eu crio um novo cliente
    Então o status da resposta deve ser 400 BadRequest
    E a mensagem de erro deve ser "CPF inválido"

  Cenário: Atualizar um cliente existente
    Dado que um cliente existe no sistema
    E que eu tenho os novos detalhes do cliente
    Quando eu atualizo o cliente
    Então o status da resposta deve ser 200 OK
    E os dados do cliente devem ser atualizados no sistema

  Cenário: Atualizar um cliente com CPF inválido
    Dado que um cliente existe no sistema
    E que eu tenho os novos detalhes do cliente com CPF inválido
    Quando eu atualizo o cliente
    Então o status da resposta deve ser 400 BadRequest
    E a mensagem de erro deve ser "CPF inválido"

  Cenário: Remover um cliente existente
    Dado que um cliente existe no sistema
    Quando eu removo o cliente
    Então o status da resposta deve ser 204 No Content
    E o cliente não deve mais existir no sistema

  Cenário: Tentar remover um cliente inexistente
    Dado que eu tenho um ID de cliente inexistente
    Quando eu removo o cliente
    Então o status da resposta deve ser 404 Not Found
    E a mensagem de erro deve ser "Cliente não encontrado"
