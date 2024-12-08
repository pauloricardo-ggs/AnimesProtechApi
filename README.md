
# Projeto AnimesProtech com CQRS, Filtros Avançados e Logs Persistentes

Este projeto é uma API que utiliza o padrão **CQRS (Command Query Responsibility Segregation)** com o Mediator pattern (**MediatR**). Ele inclui autenticação via **JWT (JSON Web Token)**, suporte a filtros avançados para listagem e registro de logs persistentes no banco de dados **PostgreSQL**.

---

## Configuração Inicial

### Configuração do Arquivo `.env`
- Duplique o arquivo `.sample_env` e renomeie a cópia para `.env`.
- Preencha as variáveis no arquivo `.env` com os dados necessários, como configurações do banco de dados e autenticação JWT.

### Aplicando as Migrations

Após inserir os dados do banco de dados no arquivo `.env`,  você deve aplicar as migrations para criar as tabelas necessárias. Use o seguinte comando:
```bash
dotnet ef database update -s Core -p DataAccess
```
Antes de iniciar a aplicação, você deve aplicar as migrations no banco de dados para criar as tabelas necessárias. Use o seguinte 
---

## Filtros de Listagem

A API suporta filtros avançados no formato `key[operation]=value`. Estes filtros podem ser usados em endpoints de listagem para realizar consultas mais precisas.

### Operações Suportadas
| **Operação** | **Descrição**                                                                 |
|--------------|-------------------------------------------------------------------------------|
| `like`       | Realiza uma busca parcial (similar ao `LIKE` no SQL). Exemplo: `name[like]=John` busca por nomes que contenham "John". |
| `eq`         | Verifica igualdade. Exemplo: `status[eq]=active` busca por itens com status "active". |
| `ne`         | Verifica desigualdade. Exemplo: `status[ne]=inactive` busca por itens com status diferente de "inactive". |
| `lte`        | Verifica se o valor é menor ou igual. Exemplo: `price[lte]=100` busca por itens com preço até 100. |
| `lt`         | Verifica se o valor é menor que. Exemplo: `age[lt]=18` busca por itens com idade menor que 18. |
| `gte`        | Verifica se o valor é maior ou igual. Exemplo: `rating[gte]=4.5` busca por itens com avaliação maior ou igual a 4.5. |
| `gt`         | Verifica se o valor é maior que. Exemplo: `views[gt]=1000` busca por itens com mais de 1000 visualizações. |

---

## Autenticação via JWT

O projeto utiliza **JWT (JSON Web Token)** para autenticação. Você deve incluir o token de autenticação no cabeçalho de cada requisição protegida:

- **Formato do Cabeçalho**:
`Authorization: Bearer`

Certifique-se de gerar um token válido ao autenticar-se no sistema.

---

## Logs Importantes

Os logs considerados importantes são persistidos no banco de dados PostgreSQL para rastreamento e auditoria. As informações registradas incluem:

- ID da requisição
- Rota acessada
- Dados da requisição e resposta
- Código HTTP retornado
- Data e hora do log
- Tipo da requisição

---

## Banco de Dados

O banco de dados utilizado no projeto é **PostgreSQL**. 

---

## Padrão CQRS com MediatR

O projeto adota o padrão **CQRS** (Command Query Responsibility Segregation) com a biblioteca **MediatR**. Este padrão separa responsabilidades de **comandos** (escrita) e **consultas** (leitura), garantindo maior flexibilidade e manutenibilidade do código.

---

## Tecnologias Utilizadas

- **.NET 9** para a API.
- **PostgreSQL** como banco de dados.
- **JWT** para autenticação.
- **Serilog** para gerenciamento e persistência de logs.
- **CQRS com MediatR** para organização e separação de responsabilidades.

---

## Como Utilizar

Após configurar o arquivo `.env` e inicializar o banco de dados, inicie a API e utilize ferramentas como **Postman** ou **Swagger UI**  para explorar os endpoints e testar as funcionalidades.