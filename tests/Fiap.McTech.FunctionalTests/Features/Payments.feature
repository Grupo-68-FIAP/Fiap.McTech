#language: pt
#coding:utf-8

Funcionalidade: Gerenciamento de Pagamentos
  Para garantir que os pagamentos sejam processados corretamente
  Como um sistema de pagamento
  Eu quero ser capaz de criar, atualizar e remover pagamentos

  Cen�rio: Criar um novo pagamento
    Dado que eu tenho os detalhes do pagamento
    Quando eu crio um novo pagamento
    Ent�o o status da resposta deve ser 201 Created
    E o pagamento deve existir no sistema
    E os dados do pagamento devem corresponder ao formato esperado

  Cen�rio: Criar um pagamento com dados inv�lidos
    Dado que eu tenho os detalhes do pagamento com dados inv�lidos
    Quando eu crio um novo pagamento
    Ent�o o status da resposta deve ser 400 BadRequest
    E a mensagem de erro deve ser "Dados do pagamento inv�lidos"

  Cen�rio: Atualizar um pagamento existente
    Dado que um pagamento existe no sistema
    E que eu tenho os novos detalhes do pagamento
    Quando eu atualizo o pagamento
    Ent�o o status da resposta deve ser 200 OK
    E os dados do pagamento devem ser atualizados no sistema

  Cen�rio: Atualizar um pagamento com dados inv�lidos
    Dado que um pagamento existe no sistema
    E que eu tenho os novos detalhes do pagamento com dados inv�lidos
    Quando eu atualizo o pagamento
    Ent�o o status da resposta deve ser 400 BadRequest
    E a mensagem de erro deve ser "Dados do pagamento inv�lidos"

  Cen�rio: Remover um pagamento existente
    Dado que um pagamento existe no sistema
    Quando eu removo o pagamento
    Ent�o o status da resposta deve ser 204 No Content
    E o pagamento n�o deve mais existir no sistema

  Cen�rio: Tentar remover um pagamento inexistente
    Dado que eu tenho um ID de pagamento inexistente
    Quando eu removo o pagamento
    Ent�o o status da resposta deve ser 404 Not Found
    E a mensagem de erro deve ser "Pagamento n�o encontrado"
