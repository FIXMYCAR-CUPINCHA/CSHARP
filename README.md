# SentinelTrack API

**SentinelTrack** é uma API REST desenvolvida em .NET 8 com Entity Framework Core, criada para gerenciar o fluxo de **motos** em **pátios**. O projeto simula um sistema de controle de operações para uma empresa de aluguel de motos, com regras de capacidade, alocação e gerenciamento de veículos.

A aplicação oferece endpoints organizados para cadastro, listagem, atualização e exclusão de pátios, motos e usuários, incluindo validações e filtros. O projeto foi desenvolvido como parte de um desafio técnico proposto pela **Mottu**.

**Justificativa do domínio:** Essas entidades traduzem diretamente o cenário da Mottu:  
controlar a infraestrutura (pátios), gerenciar os ativos (motocicletas) e registrar usuários operacionais (usuários).

## Rotas Disponíveis

---

### Pátios (`/api/v1/yards`)

- **GET /api/v1/yards** — Lista os pátios, com filtros opcionais via query params:

  | Query Param  | Tipo    | Descrição                                    | Exemplo         |
  |--------------|---------|----------------------------------------------|-----------------|
  | capacityMin  | integer | Filtra pátios com capacidade mínima          | `/api/v1/yards?capacityMin=10` |
  | capacityMax  | integer | Filtra pátios com capacidade máxima           | `/api/v1/yards?capacityMax=50` |

- **GET /api/v1/yards/{id}** — Busca pátio pelo ID.

- **POST /api/v1/yards** — Cria um novo pátio.

- **PUT /api/v1/yards/{id}** — Atualiza um pátio existente.

- **DELETE /api/v1/yards/{id}** — Remove um pátio.

---

### Motos (`/api/v1/motos`)

- **GET /api/v1/motos** — Lista motos, com filtros opcionais via query params:

  | Query Param | Tipo    | Descrição                        | Exemplo               |
  |-------------|---------|----------------------------------|-----------------------|
  | yardId      | GUID    | Filtra motos pelo ID do pátio     | `/api/v1/motos?yardId=abc123` |
  | plate       | string  | Filtra motos pela placa             | `/api/v1/motos?plate=ABC1D123`       |

- **GET /api/v1/motos/{id}** — Busca moto pelo ID.

- **POST /api/v1/motos** — Cria uma nova moto.

- **PUT /api/v1/motos/{id}** — Atualiza uma moto existente.

- **DELETE /api/v1/motos/{id}** — Remove uma moto.

---

### Usuários (`/api/v1/users`)

- **GET /api/v1/users** — Lista usuários, com filtros opcionais via query params:

  | Query Param | Tipo    | Descrição                        | Exemplo               |
  |-------------|---------|----------------------------------|-----------------------|
  | email       | string  | Filtra usuário pelo email        | `/api/v1/users?email=email@gmail.com`       |

- **GET /api/v1/users/{id}** — Busca usuário pelo ID.

- **POST /api/v1/users** — Cria um novo usuário.

- **PUT /api/v1/users/{id}** — Atualiza um usuário existente.

- **DELETE /api/v1/users/{id}** — Remove um usuário.

---

### Auth (`/api/v1/auth`)

- **POST /api/v1/auth/token** — Gera um token de autenticação.

---

## Instruções de Execução

1. Clone o repositório:
   ```bash
   git clone https://github.com/FIXMYCAR-CUPINCHA/CSHARP.git
   cd CSHARP/

2. Faça o restore:
   ```bash
   dotnet restore

3. Build o projeto com o comando:
   ```bash
   dotnet build

4. Rode o projeto com o comando:
   ```bash
   cd SentinelTrack/
   dotnet run

5. Caso o Swagger não abra sozinho acesse em:
   https://localhost:5000/swagger/index.html

---

## Autenticação
Não se esqueça de acessar o endpoint para pegar o token:

https://localhost:5000/api/v1/auth/token

---

## Testes Automatizados com xUnit
Rodar os testes:
```
dotnet test
```

---

## Url para Health Checks

https://localhost:5000/health

---

## 👥 INTEGRANTES DO GRUPO

- RM555323 - Thomaz Oliveira Vilas Boas Bartol
- RM556089 - Vinicius Souza Carvalho
- RM556972 - Gabriel Duarte Pinto

---

## 📌 Exemplos de Uso (cURL)

> Usando http para não dar erro nas requisições

### Listar usuários

```powershell
curl -X GET "http://localhost:5000/api/v1/users?page=1&pageSize=10"
```

### Criando um usuário

```powershell
curl -X POST "http://localhost:5000/api/v1/users" -H "Content-Type: application/json" -d '{
  "Name": "Maria Souza",
  "email": "maria.souza@example.com", 
  "Role": "admin"
}'
```

### Atualizando um usuário

```powershell
curl -X PUT "http://localhost:5000/api/v1/users/790d6edb-c9d3-4f28-8cdc-ffefc7a1e726" \
-H "Content-Type: application/json" \
-d '{
  "Name": "Maria Souza Atualizada",
  "email": "maria.nova@example.com",
  "Role": "admin"
}'
```

### Listando todos os pátios

```powershell
curl -X GET "http://localhost:5000/api/v1/yards?page=1&pageSize=10"
```

