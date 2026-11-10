# Orientações para alterações neste projeto

- Mantenha o README.md em português e com finalidade teórica e explicativa.
- Ao alterar arquitetura, pastas, contratos, regras, endpoints, execução ou validação, atualize as seções correspondentes do README.md no mesmo conjunto de alterações.
- Diferencie funcionalidades implementadas de propostas futuras. Não documente serviços como endpoints enquanto não houver controllers correspondentes.
- Preserve a separação entre domínio, aplicação, persistência, composição de dependências e HTTP.
- Valide alterações de serviços com `dotnet run --project School.Application.Tests` e compile a solução com `dotnet build SchoolApp.sln`.
