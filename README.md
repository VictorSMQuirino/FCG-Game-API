# FIAP Cloud Games

## Descrição
A FIAP Cloud Games (FCG) segue sua evolução! Nesta fase, o foco será 
a migração para microsserviços, a otimização da busca de jogos e a adoção de 
soluções serverless para eficiência operacional.
O desafio desta fase foi estruturado para aplicar os conhecimentos 
adquiridos nas disciplinas da fase, como Elasticsearch, Serverless, API 
Gateway, Microsserviços, Arquitetura de Software e Monitoramento e Acesso.

## Tecnologias Utilizadas

| Tecnologia            | Descrição                                      |
|-----------------------|-----------------------------------------------|
| **Backend**           | .NET 8                                       |
| **Banco de Dados**    | PostgreSQL                                   |
| **ORM**               | Entity Framework Core                        |
| **Documentação da API** | Swagger (OpenAPI)                           |
| **Testes**            | xUnit  |

### Pré-requisitos

- **.NET 8 SDK**: Disponível em [Download .NET](https://dotnet.microsoft.com/download/dotnet/8.0).
- **PostgreSQL**: Servidor de banco de dados instalado e em execução.

### Instalação

1. Clone o repositório do projeto:
   ```bash
   git clone https://github.com/VictorSMQuirino/Fiap-Cloud-Games.git
   ```
2. Navegue até o diretório do projeto:
   ```bash
   cd Fiap-Cloud-Games
   ```
3. Restaure os pacotes NuGet:
   ```bash
   dotnet restore
   ```

### Configuração do Banco de Dados

1. Certifique-se de que o PostgreSQL está instalado e em execução.
2. Crie um novo banco de dados, por exemplo, `fcg_db_games_`.
3. Atualize a string de conexão e outras informações nos arquivos `appsettings.json` ou `appsettings.Development.json` do projeto `FIAP_CloudGames.API`:
```json
{
    "ConnectionStrings": {
        "DefaultConnection": "Host=localhost;Port=5432;Database=fcg_db_games_;Username=seu_usuario;Password=sua_senha"
    },
    "Jwt": {
        "Key": "sua_chave_secreta",
        "Issuer": "seu_emissor",
        "Audience": "sua_audiencia",
        "ExpireMinutes": 30
    },
    "Elasticsearch": {
        "Uri": "default",
        "Index": "default",
        "ApiKey": "default"
    },
    "Jwt": {
        "Key": "default",
        "Issuer": "default",
        "Audience": "default",
        "ExpireMinutes": 30
    },
    "PaymentsApi": {
        "Url":  "default"
    }
}
``` 

4. Navegue até o diretório do projeto `FIAP_CloudGames.Infrastructure`:
   ```bash
   cd src\FCG_Games.Infrastructure
   ```

5. Aplique as migrações do Entity Framework Core:
   ```bash
   dotnet ef database update -s ..\FCG_Games.API\FCG_Games.API.csproj
   ```

## Licença

Este projeto está licenciado sob a [MIT License](https://opensource.org/licenses/MIT).

## Autores

- Víctor Quirino

## Agradecimentos

- À FIAP, pela oportunidade de aprendizado e desenvolvimento do projeto.
- À comunidade .NET, por fornecer recursos e documentação extensos.