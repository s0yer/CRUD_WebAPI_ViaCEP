
## Objetivo

ApiWeb de acesso a ApiExterna Viacep.
Consome ApiExterna Trazendo endereco.
Valida Cep no servico de ApiExterna.


# Arquitetura e especificações

- .net 8
- database MySql
- Api Externa ViaCep - https://viacep.com.br/
- Servicos de CRUD
- EF Core
- Controller -> Service -> Data 

# Estrutura Projeto

-Service
-Controller
-Data
-Models
-Migrations
-Testes


# Configuração do Banco de Dados

executar o MySql Localhost, ou subir container docker_compose com MySql:

docker version
docker compose up -d

# Criando migrations no database ( cria modelo identico ao Models do projeto )

dotnet tool install --global dotnet-ef

> no diretorio do projeto

dotnet ef migrations add InitialCreate
dotnet ef database update

# Executando o projeto

dotnet run ou compilar no Visual Studio ( IIS express .net 8 )

# Validacoes

Formato para entrada do cep na mascara ex. "88032005"
Nao cadastra Cep ja cadastrado
