# SchoolApp — guia de arquitetura

Projeto de estudo de uma API escolar em **ASP.NET Core / .NET 10**, com Entity Framework Core e PostgreSQL. Este README explica o código existente e serve como material de consulta durante a evolução do projeto.

## Estado atual

As cinco entidades possuem repositórios, DTOs, interfaces de serviço e serviços CRUD registrados na injeção de dependência. A camada HTTP expõe cursos, turmas, usuários, matrículas e notas, além do exemplo `WeatherForecastController`. Ainda não há autenticação, autorização por perfil ou paginação. Há middleware global de exceções, com a limitação de status HTTP descrita adiante.

## Como as camadas se relacionam

```mermaid
flowchart LR
    HTTP[Cliente HTTP] --> API[School.API: controller]
    API --> APP[School.Application: serviço e DTOs]
    APP --> CONTRACT[School.Domain: interface de repositório]
    DATA[School.Infra.Data: implementação EF Core] -. implementa .-> CONTRACT
    DATA --> DB[(PostgreSQL)]
    IOC[School.Infra.Ioc] -. registra implementações .-> API
```

O fluxo HTTP acima é implementado pelos controllers escolares, que recebem DTOs e chamam os serviços por suas interfaces, sem acessar diretamente a persistência.

A separação é uma arquitetura em camadas com inversão de dependência: a aplicação conhece contratos de persistência, sem depender do EF Core. Isso facilita testar regras sem banco e trocar a implementação de armazenamento. As entidades atuais são classes de dados; as regras implementadas ficam nos serviços.

| Projeto/pasta | Responsabilidade e exemplos |
| --- | --- |
| `School.API` | Entrada HTTP e inicialização da aplicação. |
| `School.API/Controllers` | Receber requisições, chamar serviços e definir respostas HTTP. Contém Course, SchoolClass, User, Enrollment, Grade e WeatherForecast. |
| `School.API/Program.cs` | Montar o container de dependências e o pipeline de middleware; registrar controllers, infraestrutura e OpenAPI. |
| `School.API/Properties` | Perfis locais de execução, quando presentes. |
| `School.API/appsettings*.json` | Configurações por ambiente. Credenciais reais devem ficar fora do Git. |
| `School.Application/DTO` | Contratos de entrada e saída, separados por entidade e operação. |
| `School.Application/Interfaces` | Contratos dos casos de uso, como `ICourseService`. |
| `School.Application/Services` | Mapeamento explícito entre entidades e DTOs e coordenação dos repositórios. ServiceValidation permanece disponível, mas não é chamado pelos serviços atuais. |
| `School.Domain/Entities` | Modelo escolar: Course, SchoolClass, User, Enrollment e Grade. |
| `School.Domain/Interfaces` | Contratos dos repositórios. Definem o que a aplicação precisa armazenar e consultar. |
| `School.Infra.Data/Context` | `ApplicationDbContext`: sessão de acesso ao banco, DbSets e aplicação dos mapeamentos. |
| `School.Infra.Data/EntitiesConfiguration` | Fluent API: chaves, limites, obrigatoriedade, relacionamentos e comportamento de exclusão física. |
| `School.Infra.Data/Repositories` | Consultas e gravações com EF Core. `SaveChangesAsync` efetiva as alterações no banco. |
| `School.Infra.Data/Migrations` | Histórico versionado da estrutura do banco. O snapshot representa o último modelo gerado. |
| `School.Infra.Ioc` | Liga interfaces às implementações e configura o provedor PostgreSQL. |
| `School.Application.Tests` | Verificações executáveis dos serviços com repositórios em memória, sem banco e sem framework de testes externo. |
| `SchoolApp.sln` | Agrupa os projetos para a IDE e os comandos de compilação. |
| `bin` e `obj` | Arquivos gerados na compilação/restauração; não são código de negócio. |

Referências de projeto: `Application → Domain`; `Infra.Data → Domain`; `Infra.Ioc → Application + Infra.Data`; `API → Infra.Ioc`. O domínio não precisa conhecer HTTP ou EF Core.

## Modelo escolar

- **Course**: curso com nome e descrição; possui turmas.
- **SchoolClass**: turma vinculada a um curso por `CourseId`. Matrículas usam SchoolClassId e a navegação SchoolClass; cursos e o DbContext usam a coleção SchoolClasses.
- **User**: usuário com nome, e-mail, perfil e dados derivados da senha.
- **Enrollment**: matrícula que associa usuário e turma, com início, expiração e indicador de atividade.
- **Grade**: nota vinculada à matrícula e ao curso, com data de avaliação e aprovação.

`Id` é a chave primária. Campos como `CourseId` são chaves estrangeiras. Propriedades de navegação como `Course` e `Enrollments` permitem representar os relacionamentos em objetos; elas não são automaticamente carregadas por todas as consultas. Os DTOs comuns retornam campos escalares; os DTOs detalhados de turma e matrícula incluem objetos de leitura dos relacionamentos.

## Entidade, DTO, interface, serviço e repositório

Uma **entidade** representa o modelo persistido. Um **DTO** representa os dados aceitos ou devolvidos por uma operação. Por exemplo, `CoursePostDto` recebe nome e descrição; `CoursePutDto` também exige ID; `CourseGetDto` devolve a identificação gerada. Os DTOs não expõem `Deleted` nem as coleções de navegação.

Uma **interface** define um contrato. `ICourseService` descreve os casos de uso; `ICourseRepository` descreve o acesso aos dados. O **serviço** decide se a operação é válida e converte os dados. O **repositório** usa o contexto para persistir. O controller de cursos depende de `ICourseService`, sem instanciar repositórios ou contextos manualmente.

### Métodos dos serviços

Os serviços seguem o padrão de EnrollmentService: recebem os DTOs, criam entidades com inicializadores, chamam o repositório e montam explicitamente o DTO de resposta em cada operação.

| Método | Comportamento |
| --- | --- |
| GetByIdAsync(id) | Consulta o repositório e retorna DTO; lança NotFoundException quando ausente. Turmas e matrículas retornam DTOs detalhados com relacionamentos. |
| GetAllAsync() | Mapeia os registros devolvidos pelo repositório para uma lista de DTOs, sem filtro adicional no serviço. Matrículas usam EnrollmentGetDetailDto. |
| AddAsync(dto) | Cria a entidade a partir do DTO e retorna os valores persistidos. |
| UpdateAsync(dto) | Cria uma entidade com os campos de entrada e delega a atualização ao repositório. Usuários recebem o ID separadamente: UpdateAsync(userId, dto). |
| DeleteAsync(id) | Delega a exclusão ao repositório e retorna DTO; lança NotFoundException quando ausente. |

Task e await representam operações assíncronas de acesso à persistência. As respostas são construídas com os dados devolvidos pelo repositório, inclusive IDs gerados.

### Mapeamento e regras atuais

- Os serviços não executam Data Annotations, normalização com Trim ou validação geral de IDs e vínculos. Na camada HTTP, [ApiController] executa a validação das anotações dos DTOs e retorna 400 quando inválidos; chamadas diretas aos serviços não as executam automaticamente.
- Turmas associam CourseId; a consulta individual retorna SchoolClassGetDetailDto com o curso. O repositório carrega essa navegação com Include.
- Matrículas geram EnrollmentDate em UTC e começam com IsActive igual a true. O serviço de referência recebe apenas o repositório de matrículas e retorna usuário e turma nas consultas detalhadas. O repositório carrega essas navegações.
- Na atualização, notas derivam CourseId pela matrícula e sua turma (SchoolClassId). A ausência desses vínculos gera NotFoundException. A criação atual gera DateGrade com DateTime.Now e define IsApproved quando GradeValue é maior ou igual a 60.
- Usuários recebem perfil Student na criação. UpdateAsync recebe userId separado do DTO. A criação gera hash HMACSHA512 com 64 bytes e armazena a chave como salt. A atualização atual altera nome e e-mail, mas não aplica a senha recebida no DTO. As leituras não retornam credenciais.
- Os repositórios de curso, turma, usuário e nota buscam a entidade existente durante a atualização e alteram somente os campos permitidos; retornam null quando ausente. Isso preserva Deleted, perfil do usuário, data e aprovação da nota. O EnrollmentService e sua atualização de referência foram preservados; seu repositório ainda usa Update da entidade recebida, que grava também os valores padrão dos campos omitidos.

### Exclusão lógica e limitações

Os repositórios marcam Deleted como true. Seguindo o serviço de referência, os serviços retornam os registros fornecidos pelo repositório, sem ocultar excluídos nem bloquear sua atualização. Uma política uniforme de filtragem e validação permanece como evolução futura.

ExceptionMiddleware serializa globalmente as exceções; ainda fixa o status HTTP em 500, mesmo quando identifica uma exceção de negócio como 404 ou 400. Não há autenticação, autorização nem política completa de exclusão de relacionamentos.

## Injeção de dependência e pipeline

`AddScoped<ICourseService, CourseService>()` informa ao ASP.NET Core qual implementação criar quando alguém solicitar a interface. O tempo de vida scoped compartilha a instância dentro de um escopo, normalmente uma requisição HTTP. Os repositórios e o `DbContext` seguem esse tempo de vida.

Em `Program.cs`, `AddControllers` registra os controllers, `AddInfrastructure` registra serviços/repositórios/banco e `AddSwaggerGen` gera o contrato OpenAPI. Em desenvolvimento, `UseSwagger` publica `/swagger/v1/swagger.json` e `UseSwaggerUI` disponibiliza `/swagger`. `UseHttpsRedirection` redireciona para HTTPS e `MapControllers` mapeia as rotas. `UseAuthorization` e a definição Bearer do Swagger não implementam login nem protegem os endpoints.

## Endpoints implementados

Os controllers usam `[ApiController]`, `[Route("api/[controller]")]` e injeção da interface de serviço no construtor. O token `[controller]` é substituído pelo nome da classe sem o sufixo Controller.

| Recurso | Rota base | DTO de criação | DTO de atualização |
| --- | --- | --- | --- |
| Cursos | `/api/Course` | `CoursePostDto` | `CoursePutDto` |
| Turmas | `/api/SchoolClass` | `SchoolClassPostDto` | `SchoolClassPutDto` |
| Usuários | `/api/User` | `UserPostDto` | `UserPutDto` |
| Matrículas | `/api/Enrollment` | `EnrollmentPostDto` | `EnrollmentPutDto` |
| Notas | `/api/Grade` | `GradePostDto` | `GradePutDto` |

| Operação | Método e caminho relativo | Comportamento |
| --- | --- | --- |
| Listar | `GET` na rota base | `200 OK` com lista, inclusive vazia. |
| Consultar | `GET /{id}` | `200 OK` com DTO; ausência lança NotFoundException (ver limitação do middleware). |
| Criar | `POST` na rota base | DTO no corpo; `200 OK` com o resultado ou `400 Bad Request` se o serviço retornar null. |
| Atualizar | `PUT` na rota base; usuários usam `PUT /api/User/{id}` | DTO no corpo; `200 OK` com o resultado; ausência lança NotFoundException (ver limitação do middleware). |
| Excluir | `DELETE /{id}` | `200 OK` com o DTO da exclusão lógica; ausência lança NotFoundException (ver limitação do middleware). |

Nas atualizações de cursos, turmas, matrículas e notas, o ID vem do DTO. Em usuários, o ID vem da rota e é passado separadamente ao serviço. Consultas de matrículas e a consulta individual de turma retornam DTOs detalhados. Os DTOs de usuário não expõem senha, salt ou hash.

`[ApiController]` retorna `400 Bad Request` automaticamente quando o corpo viola as anotações dos DTOs. Exceções de serviço e persistência passam pelo middleware global, com a limitação de status HTTP descrita na seção de exceções. A padronização das respostas de ausência, o uso de `201 Created` com `CreatedAtAction`, o tratamento global com `ProblemDetails` e a autenticação/autorização permanecem como melhorias futuras.

Para adicionar uma funcionalidade: modele a entidade e seus relacionamentos; crie/ajuste a configuração EF; gere uma migration se o esquema mudar; defina DTOs e contrato do serviço; implemente regras e persistência; registre as dependências; exponha o controller; teste regras e respostas; atualize este README no mesmo commit.

## Executar localmente

Pré-requisitos: SDK .NET 10, PostgreSQL e a ferramenta `dotnet-ef` compatível com EF Core 10 para migrations.

```powershell
dotnet restore SchoolApp.sln
dotnet build SchoolApp.sln
# Exemplo: substitua os valores apenas no ambiente local.
$env:ConnectionStrings__DefaultConnection = "Host=localhost;Database=school;Username=postgres;Password=SUA_SENHA"
dotnet ef database update --project School.Infra.Data --startup-project School.API
dotnet run --project School.API
```

O ASP.NET Core combina configurações de `appsettings.json`, ambiente e outras fontes. Também é possível usar User Secrets no projeto API. Não versione senhas reais. Consulte a URL exibida no terminal; em desenvolvimento o documento OpenAPI fica em `/swagger/v1/swagger.json`.

Migrations são alterações versionadas do esquema, não alterações automáticas causadas por editar uma entidade:

```powershell
dotnet ef migrations add NomeDaAlteracao --project School.Infra.Data --startup-project School.API
# Revise o código gerado antes de aplicar.
dotnet ef database update --project School.Infra.Data --startup-project School.API
```

## Verificar os serviços

```powershell
dotnet run --project School.Application.Tests
```

O executável retorna erro se uma verificação falhar. ServiceChecks.cs verifica mapeamentos, consultas detalhadas, exceções para registros ausentes, delegação ao repositório e geração de hash com repositórios em memória. Não acessa PostgreSQL e não substitui testes de integração ou HTTP.

## Manutenção deste guia

Este é um documento vivo, atualizado junto das mudanças do projeto; não existe geração automática deste arquivo. Cada alteração de arquitetura, pasta, contrato, regra, endpoint ou configuração deve incluir a revisão da seção correspondente. Descreva como implementado apenas o que o código já faz e mantenha propostas claramente identificadas.

Na validação desta implementação, a compilação apontou avisos já presentes: conflito entre versões transitivas de `Microsoft.EntityFrameworkCore.Relational` e vulnerabilidade reportada pelo NuGet em `Microsoft.OpenApi` 2.0.0. A atualização/alinhamento dessas dependências permanece pendente. A compilação não substitui testes de integração com PostgreSQL.

A migration RenameClassToSchoolClass renomeia a tabela Classes para SchoolClasses e a coluna ClassId para SchoolClassId, com seus índices e restrições, preservando os registros. As migrations anteriores mantêm os nomes históricos. Para atualizar um banco existente, execute dotnet ef database update --project School.Infra.Data --startup-project School.API; as migrations de renomeação foram aplicadas com sucesso ao banco configurado nesta execução.

O nome final da tabela é SchoolClass (singular), definido explicitamente por ToTable em SchoolClassConfiguration. A migration UseSchoolClassTableName renomeia SchoolClasses para SchoolClass e ajusta as restrições e o índice; o DbSet e as coleções em C# permanecem SchoolClasses. As duas migrations de renomeação foram aplicadas com dotnet ef database update --project School.Infra.Data --startup-project School.API, preservando os registros existentes.

Na validação atual, dotnet run --project School.Application.Tests passou. Não foram executados testes HTTP com PostgreSQL.

### Exceções dos serviços

Os cinco serviços usam `School.Application.Exceptions.NotFoundException` nas consultas individuais, atualizações e exclusões de registros ausentes. As interfaces em `School.Application/Interfaces` documentam essa condição com comentários XML; as classes de exceção permanecem na pasta `Exceptions`. As assinaturas anuláveis foram preservadas por compatibilidade, embora esses casos agora lancem exceção em vez de retornar null. Listagens vazias continuam válidas.

SchoolClassService também verifica o curso na criação e atualização; GradeService usa a mesma exceção quando não encontra matrícula ou turma ao resolver o curso na atualização. EnrollmentService verifica a existência da matrícula antes de atualizar. BadRequestException permanece disponível para regras de entrada inválida, sem introdução de novas regras nesta alteração.

O pipeline atual já registra ExceptionMiddleware, que identifica NotFoundException como 404 e BadRequestException como 400 no corpo. Há uma limitação na implementação HTTP: o middleware ainda fixa o status da resposta em 500. Os caminhos de ausência agora lançam exceção antes dos testes de null nos controllers; o status HTTP efetivo depende da correção desse middleware.

As verificações de serviços foram ajustadas ao comportamento existente de senha (hash HMACSHA512 de 64 bytes e preservação do salt na atualização de nome/e-mail) e ao contrato de exceções para recursos ausentes. Isso substitui a divergência de testes de senha relatada anteriormente.