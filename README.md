# UBEC - União Brasileira de Educação Católica

Case técnico para processo seletivo - Desenvolvimento de Sistemas.

## Como rodar o projeto do zero

### 1. Backend (.NET + MySQL)

**Pré-requisitos:**
- .NET SDK 8.0 ou 9.0 instalado
- MySQL 8.0 instalado e rodando (serviço iniciado)

**Passos:**

1. Configure a string de conexão no arquivo `appsettings.json`:
```json
"DefaultConnection": "server=localhost;port=3306;database=ubec_solicitacoes;user=root;password=root"

(Altere a senha conforme sua configuração do MySQL)

2. Crie o banco de dados no MySQL:

CREATE DATABASE
ubec_solicitacoes

3. Execute as migrações para criar as tabelas e popular os dados:

dotnet ef migrations add InitialCreate
dotnet ef database update

4. Rode a API:

dotnet run
A API estará disponível em: http://localhost:5103

5. Frontend (React + TypeScript)
Pré-requisitos:

Node.js 18+ instalado

Passos:
1.1. Vá para a pasta do frontend:

cd C:\case-tecnico-ubec\frontend

1.2. Instale as dependências:

npm install

1.3. Rode o projeto:

npm start
O frontend estará disponível em: http://localhost:3000

6. Decisões técnicas:

Service Layer: Separei as regras de negócio (RN01 a RN04) em uma camada de serviço, deixando o controller apenas como roteador. Isso facilita testes e manutenção.

Entity Framework + MySQL: Utilizei ORM para agilizar o desenvolvimento e garantir consistência com o banco de dados relacional.

DTOs (Data Transfer Objects): Criei DTOs para separar a camada de domínio da camada de API, expondo apenas os dados necessários ao frontend.

CORS habilitado: Configurei o CORS para permitir que o frontend (porta 3000) se comunique com o backend (porta 5103) sem bloqueios de segurança.

7. O que ficou de fora ou está frágil nesta versão:

Testes unitários (não implementados, mas seriam o próximo passo);

Paginação na listagem de solicitações;

Design responsivo no frontend (foco foi funcionalidade);

Tratamento global de erros no backend (atualmente é feito caso a caso).

4. Onde você usou IA

Utilizei ChatGPT para gerar o esqueleto inicial dos modelos, DTOs, controller, service e do componente React.
Ajustei manualmente as regras de negócio, o seed do banco de dados, a validação das regras RN01 a RN04 e toda a
configuração do ambiente (MySQL, .NET, CORS) para atender exatamente ao enunciado do case.








