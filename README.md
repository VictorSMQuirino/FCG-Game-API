# FIAP Cloud Games

## Descri��o
A FIAP Cloud Games (FCG) segue sua evolu��o! Nesta fase, o foco ser� 
a migra��o para microsservi�os, a otimiza��o da busca de jogos e a ado��o de 
solu��es serverless para efici�ncia operacional.
O desafio desta fase foi estruturado para aplicar os conhecimentos 
adquiridos nas disciplinas da fase, como Elasticsearch, Serverless, API 
Gateway, Microsservi�os, Arquitetura de Software e Monitoramento e Acesso.

## Tecnologias Utilizadas

| Tecnologia            | Descri��o                                      |
|-----------------------|-----------------------------------------------|
| **Backend**           | .NET 8                                       |
| **Banco de Dados**    | PostgreSQL                                   |
| **ORM**               | Entity Framework Core                        |
| **Documenta��o da API** | Swagger (OpenAPI)                           |
| **Testes**            | xUnit  |

### Pr�-requisitos

- **.NET 8 SDK**: Dispon�vel em [Download .NET](https://dotnet.microsoft.com/download/dotnet/8.0).
- **PostgreSQL**: Servidor de banco de dados instalado e em execu��o.

### Instala��o

1. Clone o reposit�rio do projeto:
   ```bash
   git clone https://github.com/VictorSMQuirino/Fiap-Cloud-Games.git
   ```
2. Navegue at� o diret�rio do projeto:
   ```bash
   cd Fiap-Cloud-Games
   ```
3. Restaure os pacotes NuGet:
   ```bash
   dotnet restore
   ```

### Configura��o do Banco de Dados

1. Certifique-se de que o PostgreSQL est� instalado e em execu��o.
2. Crie um novo banco de dados, por exemplo, `fcg_db_games_`.
3. Atualize a string de conex�o e outras informa��es nos arquivos `appsettings.json` ou `appsettings.Development.json` do projeto `FIAP_CloudGames.API`:
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

4. Navegue at� o diret�rio do projeto `FIAP_CloudGames.Infrastructure`:
   ```bash
   cd src\FCG_Games.Infrastructure
   ```

5. Aplique as migra��es do Entity Framework Core:
   ```bash
   dotnet ef database update -s ..\FCG_Games.API\FCG_Games.API.csproj
   ```

## Licen�a

Este projeto est� licenciado sob a [MIT License](https://opensource.org/licenses/MIT).

## Autores

- V�ctor Quirino

## Agradecimentos

- � FIAP, pela oportunidade de aprendizado e desenvolvimento do projeto.
- � comunidade .NET, por fornecer recursos e documenta��o extensos.