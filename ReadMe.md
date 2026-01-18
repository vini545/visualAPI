# VisualAPI

API RESTful em ASP.NET Core (.NET 9) com PostgreSQL, JWT e testes unitários.

---

## Requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)
- PostgreSQL (será configurado via Docker Compose)

---

## Executando o sistema localmente

## Clonar o repositório

```bash
git clone https://github.com/vini545/visualAPI.git
cd visualAPI


Antes de rodar a aplicação, configure as seguintes variáveis no `appsettings.json` ou via variáveis de ambiente:
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=visualAPIDB;Username=postgres;Password=SUA_SENHA_AQUI"
},
"Jwt": {
  "Key": "SUA_CHAVE_SECRETA",
  "Issuer": "VisualAPI",
  "Audience": "VisualAPIUsers"
}

substitua SUA_SENHA_AQUI e SUA_CHAVE_SECRETA pelos valores corretos antes de rodar a aplicação


Rodar com Docker Compose:
    docker-compose up --build

A API estará disponível em: http://localhost:8080

-- Testes --
    No terminal, dentro da pasta do projeto de testes (visualAPI.Tests):
        dotnet test

API já vem com Swagger configurado. Para testar:
        http://localhost:8080/swagger/index.html
        
Para logar utilizar ou criar um usuario novo
Password = "senha123",
UserName = "visualAPI"



Para endpoints protegidos por JWT:

Clique em Authorize

Informe Bearer <SEU_TOKEN>
obtido no endpoint api/auth/login


-----------------------------------------------------------------------------------------
Endpoints Principais
Pessoa

GET /pessoa - Lista todas as pessoas

POST /pessoa - Cria nova pessoa

PATCH /pessoa/{id} - Atualiza informações parciais (nome)

DELETE /pessoa/{id} - Deleta pessoa

Conta

GET /conta - Lista todas as contas

GET /conta/{id}/saldo - Consulta saldo

POST /conta/{id}/credito - Adiciona crédito

POST /conta/{id}/debito - Debita valor

DELETE /conta/{id} - Deleta conta


Observações

DTOs (PessoaDTO, ContaDTO, ContaOperacaoDTO) são usados para evitar ciclos na serialização JSON.

Migrations do EF criam automaticamente o banco e tabelas.

Para desenvolvimento, o Swagger é automaticamente habilitado em Development.
