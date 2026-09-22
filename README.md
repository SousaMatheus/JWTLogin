# JWTLogin

Sistema de autenticação com **JWT (JSON Web Token)** em **ASP.NET Core**, aplicando arquitetura em camadas para separar API, domínio e infraestrutura.

## 🎯 O que demonstra

- Geração e validação de **tokens JWT** para autenticação stateless
- **Login e autorização** protegendo endpoints da API com `[Authorize]` e roles
- **Arquitetura em camadas** — separação clara de responsabilidades
- Hash de senha e boas práticas de segurança em autenticação

## 🏗️ Estrutura da solução

```
JWTLogin.sln
├── JWTLogin.Api/    # Endpoints, configuração de autenticação e middleware
├── JWTLogin.Core/   # Modelos de domínio e regras de negócio
└── JWTLogin.Infra/  # Acesso a dados e serviços de infraestrutura
```

## 🚀 Como executar

Pré-requisito: [.NET SDK](https://dotnet.microsoft.com/download)

```bash
git clone https://github.com/SousaMatheus/JWTLogin.git
cd JWTLogin
dotnet run --project JWTLogin.Api
```

Faça login no endpoint de autenticação para obter o token e envie-o nas demais requisições via header `Authorization: Bearer {token}`.

## 📄 Licença

Projeto de estudo — sinta-se livre para usar como referência.
