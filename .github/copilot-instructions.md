# TenantGuard – Copilot Instructions

## Project Type
- Multi-tenant SaaS API built with ASP.NET Core using Clean Architecture.

## Solution Structure
- TenantGuard.Domain – entities, value objects, domain rules.
- TenantGuard.Application – use cases, DTOs, interfaces (contracts).
- TenantGuard.Infrastructure – EF Core, external integrations, implementations of Application interfaces.
- TenantGuard.Api – controllers, middleware, DI configuration (composition root).

## General Rules
- Prefer best-practice, production-ready approaches when they’re clearly better.
- If details are missing, make reasonable assumptions and proceed; document them briefly.
- Keep changes consistent across the codebase (API/DTOs/validation/errors/logging/config/DI).


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

## Startup Configuration
- Local startup should open Swagger UI instead of raw OpenAPI JSON, which renders the OpenAPI spec.
- Prefer Swagger setup implementation in a separate file instead of inline in Program.cs when adding features like tenant header support.

## Planning Documentation Workflow
- Use the `obsidian-planning-docs` skill when creating or updating PRDs, plans, epics, features, user stories, or tasks.
- Store those artifacts as Obsidian-friendly markdown under `docs/`.
- If the required `docs/` structure is missing, create it before writing artifacts.
- Maintain stable filenames, frontmatter, and `[[wikilinks]]` between related planning notes.