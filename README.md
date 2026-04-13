# Korp_Teste_Pedro_Lima_de_Carvalho


## Tecnologias Utilizadas

![HTML5](https://img.shields.io/badge/html5-%23E34F26.svg?style=for-the-badge&logo=html5&logoColor=white)
![CSS3](https://img.shields.io/badge/css3-%231572B6.svg?style=for-the-badge&logo=css3&logoColor=white)
![Angular](https://img.shields.io/badge/angular-%23DD0031.svg?style=for-the-badge&logo=angular&logoColor=white) 
![RxJS](https://img.shields.io/badge/rxjs-%23B7178C.svg?style=for-the-badge&logo=reactivex&logoColor=white)
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![SQLite](https://img.shields.io/badge/sqlite-%2307405e.svg?style=for-the-badge&logo=sqlite&logoColor=white)

## Descrição

Este projeto é uma aplicação fullstack com microsserviços para gerenciamento de produtos e notas fiscais. O projeto foi desenvolvido com Angular no frontend e ASP.NET Core no backend.

## Arquitetura

O sistema foi dividido em dois microsserviços:

### Product Service

Responsável por:

- Cadastro de produtos
- Atualização de produtos
- Controle de estoque

### Billing Service (Invoices)

Responsável por:

- Criação de notas fiscais
- Adição de itens à nota
- Fechamento de notas
- Comunicação com o serviço de produtos

### Integração entre serviços

- Comunicação via HTTP entre Billing Service e Product Service
- Atualização de estoque ao fechar a nota

### Tratamento de Falhas

- Simulação de falha no serviço de estoque
- Tratamento de erros com respostas HTTP apropriadas:
    - 400 (Bad Request)
    - 404 (Not Found)
    - 409 (Conflict)
    - 500 (Internal Server Error)


### Concorrência

- Controle de concorrência no estoque
- Prevenção de inconsistência em cenários simultâneos


### Idempotência

- Operação de fechamento de nota protegida contra múltiplas execuções
- Evita duplicação de efeitos no sistema


## Frontend

### Componentes

- **Product Form**: Cadastro e edição de produtos
- **Product List**: Listagem de produtos
- **Invoice List**: Gerenciamento de notas fiscais


### RxJS

Utilizado para:

- Requisições HTTP assíncronas
- Manipulação de respostas com `subscribe()`


### Ciclo de Vida Angular

- `ngOnInit`: utilizado para carregar dados iniciais (produtos e notas)


### Validações

- Campos obrigatórios
- Mensagens de erro exibidas ao usuário
- Tratamento de erros via `alert()`


## Backend

### ASP.NET Core

- Criação de APIs REST
- Controllers para Products e Invoices


### Entity Framework Core

- Mapeamento ORM
- Manipulação de dados no SQLite


### LINQ

Utilizado para consultas como:

- Busca por ID
- Verificação de existência (`Any`)
- Cálculo de valores (`Max`)


## Regras de Negócio

- Não é possível adicionar itens em notas fechadas
- Não é possível fechar nota com estoque insuficiente
- Não é permitido código de produto duplicado
- Produto deve existir para ser adicionado à nota


##  Como executar o projeto

### Backend

1. Acesse cada projeto (Products e Billing)
2. Execute:

```bash
dotnet run
```

3. Acesse Swagger:

- Products: https://localhost:7076/swagger
- Invoices: https://localhost:7094/swagger


### Frontend

1. Instale dependências:

```bash
npm install
```

2. Execute:

```bash
ng serve
```

3. Acesse:

```text
http://localhost:4200
```