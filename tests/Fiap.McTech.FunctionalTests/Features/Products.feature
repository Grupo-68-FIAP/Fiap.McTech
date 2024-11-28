#language: pt
#coding:utf-8

Funcionalidade: Gerenciamento de Produtos

  Cenário: Criar um novo produto
    Dado que eu inseri "Product 1" como o nome do produto
    E que eu inseri 10 como o valor do produto
    E que eu inseri "Product description" como a descrição do produto
    E que eu inseri "img" como a imagem do produto
    E que eu selecionei "Snack" como a categoria do produto
    Quando eu crio o produto
    Então o produto deve ser criado com sucesso

  Cenário: Criar um produto com valor negativo
    Dado que eu inseri "Product 2" como o nome do produto
    E que eu inseri -5 como o valor do produto
    E que eu inseri "Product description" como a descrição do produto
    E que eu inseri "img" como a imagem do produto
    E que eu selecionei "Snack" como a categoria do produto
    Quando eu crio o produto
    Então a criação do produto deve falhar com "Invalid product value"

  Cenário: Remover um produto existente
    Dado que um produto existe no sistema
    Quando eu removo o produto
    Então o produto não deve mais existir no sistema
    E o status da resposta deve ser 204 No Content

  Cenário: Atualizar um produto existente
    Dado que um produto existe no sistema
    Quando eu atualizo o produto com novos dados
    Então o produto deve ser atualizado com sucesso
    E o status da resposta deve ser 200 OK