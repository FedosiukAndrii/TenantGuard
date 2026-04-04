# TenantGuard

TenantGuard is an API-first, secure-by-design multi-tenant SaaS foundation built with .NET.

## Project Status

This project is currently **in progress**. The repository contains the foundation and planning work for the MVP, but the full intended result is still being implemented.

## Expected Project Result

The expected outcome of TenantGuard is a realistic MVP that demonstrates:

- strict tenant isolation across shared infrastructure
- clear separation between host and tenant operations
- OIDC-based authentication and role-based authorization
- a tenant-scoped Projects module with CRUD and search capabilities
- auditing, structured logging, migrations, seed data, and automated tests

The main goal is to provide a secure baseline for a multi-tenant SaaS platform where tenant data cannot leak across boundaries and core platform behavior is production-oriented from the start.

## MVP Scope

The planned MVP is expected to include:

- host tenant management and tenant lifecycle control
- tenant context resolution and enforcement through request processing
- authorization rules for host and tenant users
- project management endpoints scoped to the active tenant
- local development setup that is reproducible and demo-ready

## Current Solution Structure

- `TenantGuard.API` - ASP.NET Core Web API entry point, middleware, controllers, and Swagger setup
- `TenantGuard.Application` - application layer and use-case orchestration
- `TenantGuard.Domain` - core domain entities and enums
- `TenantGuard.Infrastructure` - persistence, dependency injection, and infrastructure concerns
- `TenantGuard.API.Tests` - automated tests
- `docs` - product, planning, epic, feature, and story documentation

## Notes

The top-level planning in `docs/` describes the intended product direction and delivery scope. Implementation is ongoing, so some planned capabilities may not yet be complete in code.
