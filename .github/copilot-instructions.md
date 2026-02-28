# TenantGuard – Copilot Instructions

## Project Type
- Multi-tenant SaaS API built with ASP.NET Core using Clean Architecture.

## Solution Structure
- TenantGuard.Domain – entities, value objects, domain rules.
- TenantGuard.Application – use cases, DTOs, interfaces (contracts).
- TenantGuard.Infrastructure – EF Core, external integrations, implementations of Application interfaces.
- TenantGuard.Api – controllers, middleware, DI configuration (composition root).

## Architecture Rules
- Dependencies flow inward only.
- Interfaces live in Application.
- Implementations live in Infrastructure.
- Domain must not depend on ASP.NET or EF Core.
- Do not reference HttpContext outside Api layer.

## Multi-Tenancy
- Tenant is resolved via X-Tenant-Id header (MVP).
- ITenantContext is a scoped abstraction (defined in Application).
- Fail fast if tenant is missing on tenant endpoints.

## Coding Style
- Use controllers (not minimal APIs).
- Keep methods small and single responsibility.
- Use clear naming and avoid static state.
- Use best practices.