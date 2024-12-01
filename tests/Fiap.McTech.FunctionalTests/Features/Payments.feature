#language: pt
#coding:utf-8

Funcionalidade: Gerenciamento de Pagamentos
  Para garantir que os pagamentos sejam processados corretamente
  Como um sistema de pagamento
  Eu quero ser capaz de criar, atualizar e remover pagamentos

  Cenário: Criar um novo pagamento
    Dado que eu tenho os detalhes do pagamento
    Quando eu crio um novo pagamento
    Então o status da resposta deve ser 201 Created
    E o pagamento deve existir no sistema
    E os dados do pagamento devem corresponder ao formato esperado

  Cenário: Criar um pagamento com dados inválidos
    Dado que eu tenho os detalhes do pagamento com dados inválidos
    Quando eu crio um novo pagamento
    Então o status da resposta deve ser 400 BadRequest
    E a mensagem de erro deve ser "Dados do pagamento inválidos"

  Cenário: Atualizar um pagamento existente
    Dado que um pagamento existe no sistema
    E que eu tenho os novos detalhes do pagamento
    Quando eu atualizo o pagamento
    Então o status da resposta deve ser 200 OK
    E os dados do pagamento devem ser atualizados no sistema

  Cenário: Atualizar um pagamento com dados inválidos
    Dado que um pagamento existe no sistema
    E que eu tenho os novos detalhes do pagamento com dados inválidos
    Quando eu atualizo o pagamento
    Então o status da resposta deve ser 400 BadRequest
    E a mensagem de erro deve ser "Dados do pagamento inválidos"

  Cenário: Remover um pagamento existente
    Dado que um pagamento existe no sistema
    Quando eu removo o pagamento
    Então o status da resposta deve ser 204 No Content
    E o pagamento não deve mais existir no sistema

  Cenário: Tentar remover um pagamento inexistente
    Dado que eu tenho um ID de pagamento inexistente
    Quando eu removo o pagamento
    Então o status da resposta deve ser 404 Not Found
    E a mensagem de erro deve ser "Pagamento não encontrado"
