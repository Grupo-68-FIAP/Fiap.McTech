[![.NET](https://github.com/Grupo-68-FIAP/Fiap.McTech/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Grupo-68-FIAP/Fiap.McTech/actions/workflows/dotnet.yml) [![SonarCloud analysis](https://github.com/Grupo-68-FIAP/Fiap.McTech/actions/workflows/sonarcloud.yml/badge.svg)](https://github.com/Grupo-68-FIAP/Fiap.McTech/actions/workflows/sonarcloud.yml)
# McTech - Sistema Gerenciador de Pedidos para Restaurantes

O McTech é um sistema completo de gerenciamento de pedidos desenvolvido especificamente para restaurantes, projetado para simplificar e otimizar os processos de pedidos, desde o momento em que são feitos até a sua conclusão. 
Utilizando tecnologias modernas e práticas arquiteturais, o MeTech oferece uma solução eficiente e escalável para restaurantes de todos os portes.

## Tecnologias Utilizadas
 - Plataforma: .NET 6.0
 - Banco de Dados: Microsoft SQL Server
 - Arquitetura de Software: Hexagonal (ou Ports and Adapters)
 - Padrão Arquitetural Adicional: Arquitetura em Cebola (ou Onion Architecture)

## Funcionalidades Principais

//TODO

## Como Contribuir

1 - Reportar Problemas: Encontrou um bug ou tem uma sugestão de melhoria? Por favor, abra uma issue e descreva detalhadamente o problema ou sua ideia.

2 - Enviar Pull Requests: Se você deseja corrigir um problema ou adicionar uma nova funcionalidade, sinta-se à vontade para enviar um pull request. Certifique-se de seguir as diretrizes de contribuição.

3 - Melhorar a Documentação: A documentação clara e detalhada é essencial. Se você encontrar partes da documentação que podem ser aprimoradas, não hesite em fazer as alterações necessárias.


## Como Configurar e Executar o Projeto

1 - Verifique a instalação do Docker:

```sh
docker --version
```

2 - Verifique a instalação do Docker Compose:

```sh
docker-compose --version
```

3 - Navegue até o diretório do projeto:

```sh
cd Fiap.McTech
```

4 - Construir e iniciar os serviços:

```sh
docker-compose up -d
```

5 - Verificar se os containers estão rodando:

```sh
docker-compose ps
```

6 - Veja a documentação do projeto e teste as API's:

```sh
http://localhost:8080/swagger
```

## Equipe McTech
Agradecemos por considerar contribuir para o MeTech! Se tiver alguma dúvida ou precisar de assistência, não hesite em entrar em contato conosco.
