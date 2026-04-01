---
title: TenantGuard Secure Multi-Tenant SaaS PRD
type: prd
status: draft
tags:
  - planning
  - prd
  - saas
  - multitenancy
  - security
created: 2026-04-01
updated: 2026-04-01
slug: tenantguard-secure-multitenant-saas
---

# TenantGuard Secure Multi-Tenant SaaS PRD

## Summary

TenantGuard is a secure-by-design foundation for a multi-tenant SaaS platform built around strong tenant isolation, role-based access control, focused auditing, and production-ready delivery practices. The current codebase already establishes a Clean Architecture ASP.NET Core API, tenant resolution through the `X-Tenant-Id` header, Swagger support, and a base tenant domain model.

This PRD narrows the initial product scope to a realistic MVP: secure tenant-aware API operations, tenant management, basic OIDC authentication, role-based authorization, Projects as the required tenant-scoped domain module, and test-backed tenant isolation. Tasks, broader observability, advanced authorization, UI expansion, and enterprise hardening remain explicitly phased after MVP.

## Problem

Teams building SaaS products often start with weak multi-tenancy boundaries, incomplete authorization, and limited observability, which creates a high risk of tenant data leakage and difficult production incidents. At the same time, fully enterprise-grade isolation from day one can overcomplicate a pet project and delay usable outcomes.

TenantGuard should solve this by providing a practical but disciplined SaaS foundation where tenant context is mandatory, authorization is explicit, data access is constrained by design, and core operational practices are present from the start.

## Goals

### Product Goals

- Deliver a usable multi-tenant SaaS foundation for future product modules.
- Enforce tenant isolation consistently across API, application, and data layers.
- Provide host and tenant administration boundaries with clear roles and permissions.
- Establish essential auditing, structured logging, migrations, and automated tests as first-class requirements.
- Keep MVP narrow enough to deliver and validate the security model before broader platform features are added.

### MVP Goals

- Tenant resolution through `X-Tenant-Id` on tenant-scoped endpoints.
- Tenant management for host administrators.
- Shared database plus shared schema tenancy model with mandatory `TenantId` filtering.
- Authentication through OAuth2/OIDC and authorization via roles only.
- Projects as the required tenant-scoped domain module to prove end-to-end isolation.
- Audit trail for critical actions only.
- Basic structured logging for operational visibility.
- Local-first developer workflow with migrations, seeded demo data, tests, and containerized dependencies.

## Non-Goals

- Billing, subscriptions, invoicing, or payment integration in MVP.
- Schema-per-tenant or database-per-tenant isolation in MVP.
- Dedicated customer-facing portal or administrative UI in MVP.
- Fine-grained feature flag platform in MVP.
- Enterprise controls such as SQL Server Row-Level Security in MVP.
- Full policy-based or resource-based authorization system in MVP.
- Service accounts or machine-to-machine integration flows in MVP.
- Full OpenTelemetry implementation, dashboards, or advanced tracing in MVP.
- Full-blown SIEM or compliance certification scope in MVP.

## Users

### Primary Personas

- SaaS platform owner: defines platform rules, provisions tenants, reviews security posture, and manages host-level controls.
- Host administrator: manages tenants, activation state, limits, and support operations across the platform.
- Tenant administrator: manages users, roles, and tenant-owned resources within one tenant.
- Tenant user: consumes tenant-scoped functionality with limited permissions.

## Requirements

## 1. Executive Summary

### Problem Statement

Multi-tenant SaaS systems frequently fail at the boring but critical parts: tenant isolation, authorization boundaries, auditability, and production readiness. TenantGuard should offer a practical reference implementation where these concerns are built in from the start instead of retrofitted later.

### Proposed Solution

Build TenantGuard as a secure-by-design multi-tenant SaaS foundation centered on an ASP.NET Core API with Clean Architecture, shared-schema tenancy for MVP, mandatory tenant context resolution, basic OAuth2/OIDC authentication, role-based authorization, focused auditing for critical actions, structured logging, and Projects as the first required tenant-scoped domain module.

### Success Criteria

- 100% of tenant-scoped API endpoints reject requests that lack valid tenant context.
- 100% of requests with mismatched `X-Tenant-Id` header and tenant claim are rejected with `403 Forbidden`.
- 0 verified cross-tenant data leakage cases in unit and integration test suites.
- 100% of privileged operations require explicit role checks.
- A new local environment can be started from scratch, seeded, and used for demo flows in under 15 minutes.
- A tenant administrator can authenticate and complete one tenant-scoped workflow through the API.

## 2. User Experience & Functionality

### User Personas

- Host administrator needs a way to create, activate, suspend, and review tenants.
- Tenant administrator needs a secure way to sign in, manage tenant members, and operate tenant-owned resources.
- Tenant user needs access only to their tenant's data and only to actions explicitly granted.

### Core Product Flows

1. Host admin provisions a new tenant.
2. Platform seeds baseline roles and an initial tenant admin identity.
3. Tenant admin signs in through OAuth2/OIDC.
4. Tenant admin interacts with the Projects module through the API.
5. Every tenant request is resolved against tenant context and role-based authorization.
6. The system records critical audit events and emits structured application logs.

### User Stories

#### Story 1: Host tenant provisioning

As a host administrator, I want to create and manage tenants so that I can onboard and control customers safely.

Acceptance Criteria:

- Host-only endpoints exist for creating, viewing, activating, and suspending tenants.
- Host endpoints must not require `X-Tenant-Id`.
- Tenant creation stores slug, name, status, timestamps, and actor metadata.
- Provisioning creates baseline tenant metadata and initial administrative access.
- Suspended tenants are blocked from tenant-scoped operations.

#### Story 2: Tenant context enforcement

As a platform owner, I want every tenant-scoped request to resolve tenant context consistently so that data cannot leak across tenants.

Acceptance Criteria:

- Tenant-scoped endpoints require a valid `X-Tenant-Id` header in MVP.
- Invalid or missing tenant headers return RFC7807 ProblemDetails responses.
- `X-Tenant-Id` must match the tenant claim in the access token.
- Header and token tenant mismatch returns `403 Forbidden`.
- Tenant context cannot be bypassed on tenant-scoped endpoints.
- Tenant context is available through an application abstraction rather than direct `HttpContext` usage outside the API layer.
- Tenant-scoped entities include `TenantId` and are filtered automatically.
- Save operations assign `TenantId` automatically for new tenant-scoped entities.

#### Story 3: Authentication and authorization

As a tenant administrator, I want to authenticate and receive only the permissions I should have so that access is secure and predictable.

Acceptance Criteria:

- OAuth2/OIDC authentication is supported for interactive users.
- Access tokens include user identity and tenant-related claims.
- Authorization uses role-based checks only in MVP.
- Privileged operations fail with `403 Forbidden` when roles are insufficient.
- Administrative boundaries separate host-level and tenant-level operations.

#### Story 4: Tenant-scoped domain module

As a tenant user, I want to use a real tenant-scoped resource so that the platform proves isolation in practical workflows.

Acceptance Criteria:

- MVP requires Projects as the core tenant-scoped domain module.
- Tasks are optional and may be added only if they do not expand MVP scope materially.
- CRUD operations are tenant-scoped by default.
- Search and filtering never return data from another tenant.
- API responses and tests verify that cross-tenant access is impossible through normal application paths.

#### Story 5: Auditing and observability

As an operator, I want to know who did what and when so that incidents and access problems can be investigated.

Acceptance Criteria:

- Audit records capture tenant, actor, action, target, and timestamp for critical actions only.
- MVP audit scope is limited to tenant lifecycle events, role changes, and key domain actions.
- Application logs are structured and include `TraceId`, `TenantId`, and `UserId` where available.
- Health checks exist for API readiness and database connectivity.
- OpenTelemetry support is limited to future integration hooks and is not required for MVP delivery.

### Simplification Decisions

- MVP will support header-based tenant resolution first; subdomain-based tenancy is deferred to a later phase.
- MVP will use shared database plus shared schema with strong application-level isolation.
- MVP has no required dedicated UI scope.
- Role-based authorization is the only required authorization model in MVP.
- Policy-based and resource-based authorization are deferred to v1.1.
- Projects are required for MVP; Tasks remain optional.
- OpenTelemetry is deferred beyond MVP apart from compatibility hooks.

## 3. Technical Specifications

### Architecture Overview

TenantGuard follows Clean Architecture:

- API layer: controllers, middleware, Swagger, authentication setup, request pipeline, ProblemDetails, tenant resolution.
- Application layer: use cases, DTOs, validation, abstractions such as `ITenantContext`, authorization contracts, and business orchestration.
- Domain layer: tenant and core business entities, value objects, rules, and enumerations.
- Infrastructure layer: EF Core persistence, identity provider integration, structured logging, focused audit storage, migrations, and concrete implementations.

Current codebase alignment:

- `ITenantContext` already exists in the Application layer.
- API middleware already resolves tenant context from `X-Tenant-Id` and returns ProblemDetails for invalid requests.
- Swagger setup already documents the tenant header requirement.
- A base `Tenant` entity already exists in the Domain layer.

### Functional Requirements

#### Tenancy Model

- Use shared database plus shared schema for MVP.
- Host endpoints must not require tenant context.
- Tenant endpoints must always enforce tenant context.
- All multi-tenant entities must carry `TenantId`.
- Global query filters must be applied to tenant-scoped entities.
- Persistence must reject or fail fast on tenant-scoped operations when tenant context is absent.
- `X-Tenant-Id` must match the tenant identifier carried in the authenticated access token.
- Mismatch between request header tenant and token tenant must return `403 Forbidden`.
- Host administrator access to tenant data must be explicit and implemented only through bounded administrative flows.
- Tenant statuses must include at least `Active` and `Suspended`, with suspended tenants blocked from normal usage.

#### Identity and Access Management

- Support OAuth2/OIDC through a provider such as Keycloak for local development and a production-capable provider later.
- Support interactive user authentication only in MVP.
- Define at least these access boundaries: `HostAdmin`, `TenantAdmin`, `TenantUser`.
- Use role-based authorization as the enforcement model for MVP.
- Move policy-based and resource-based authorization to v1.1.

#### Domain Scope

- Tenant management is mandatory.
- User membership and role assignment within a tenant are part of MVP scope.
- One tenant-scoped business module must exist to validate the platform end to end.
- Projects are the required domain module for MVP because they are simple, expressive, and easy to test for isolation.
- Tasks are optional and may be introduced after Projects only if they do not expand the MVP timeline.

#### Audit and Operations

- Separate application logging from persistent audit records.
- Persist audit records only for tenant lifecycle events, role changes, and key domain actions.
- Include structured logging with basic correlation identifiers and tenant metadata where available.
- Keep OpenTelemetry limited to future hooks and compatibility points, not an MVP requirement.
- Provide EF Core migrations and seeded demo data for host admin, demo tenant, and tenant admin.

### Integration Points

- ASP.NET Core Web API with controller-based endpoints.
- EF Core with SQL Server.
- OAuth2/OIDC provider.
- Swagger/OpenAPI with auth and tenant requirements.
- Docker Compose for local orchestration of API, database, and identity provider.

### Security & Privacy

- HTTPS is required outside local development.
- Secrets must stay outside the repository and use environment-specific configuration.
- Validation must be explicit on incoming commands and requests.
- Error responses must avoid leaking internal details.
- Tenant-scoped endpoints must reject requests that omit tenant context.
- Tenant-scoped endpoints must reject requests where `X-Tenant-Id` does not match tenant claims in the token.
- Host endpoints must not silently inherit tenant context requirements.
- Authorization failures and tenant mismatches must be test-covered.
- Tenant data must not be retrievable without matching tenant context and permission checks.

### Non-Functional Requirements

- API should provide deterministic RFC7807 error responses for validation, tenant, and authorization failures.
- Seeded local environments should be reproducible from a clean state.
- Security-critical logic must be covered by unit and integration tests.
- Core list endpoints should be designed with tenant-aware indexing and pagination in mind.

### Testing Strategy

- Unit tests for tenant resolution, role-based authorization checks, validators, and domain rules.
- Integration tests for tenant isolation, suspended tenant behavior, tenant claim and header matching, and authorization enforcement.

## 4. Risks & Roadmap

### Technical Risks

- Shared-schema tenancy is fast to build but depends on strict discipline in filters and authorization.
- OAuth2/OIDC integration adds setup complexity for local development and test environments.
- Adding UI too early can dilute focus from the platform security core.
- Audit scope can expand quickly unless privileged and business-critical events are defined explicitly.

### Product Risks

- The project can become an overbuilt reference platform if MVP is not kept narrow.
- Too many infrastructure concerns in the first release can delay visible user value.
- If the first domain module is too complex, it will slow validation of core tenancy guarantees.

### Phased Rollout

#### MVP

- Host tenant management.
- Header-based tenant resolution.
- Shared-schema tenant isolation.
- OAuth2/OIDC sign-in.
- Role-based authorization.
- Projects as the required tenant-scoped domain module.
- Critical-action auditing, structured logging, migrations, seed data, tests, Swagger, and Docker Compose.

#### v1.1

- Subdomain-based tenant resolution.
- Service accounts and machine-to-machine auth.
- More complete tenant user management and invitations.
- Policy-based and resource-based authorization.
- Optional Tasks module expansion.
- Broader OpenTelemetry adoption, dashboards, and alerting.

#### v2.0

- Optional stronger isolation model such as schema-per-tenant or database-per-tenant.
- SQL Server Row-Level Security for defense in depth.
- Feature limits, plan enforcement, and optional billing foundation.
- Broader compliance and security posture documentation.

## Success Metrics

- Unit and integration suites pass with 100% success on protected tenant-isolation flows.
- Host admin can provision a tenant and seed a tenant admin in one guided flow.
- Tenant admin can authenticate and complete at least one business workflow.
- All tenant-scoped endpoints are covered by tenant enforcement tests.
- Local onboarding from clean clone to running demo environment takes less than 15 minutes.

## Dependencies

- OAuth2/OIDC provider selection and local setup strategy.
- SQL Server persistence and migration strategy.
- Definition of the Projects domain module and its minimal scope.
- Testcontainers or equivalent strategy for reliable integration testing.

## Assumptions

- The current project remains API-first, with any UI work deferred until after the core MVP is validated.
- The first release should optimize for secure architecture and demonstrable flows rather than wide business scope.
- Subdomain tenancy and enterprise-grade isolation are desirable but not mandatory for initial delivery.

## Related Notes

- [[plans/tenantguard-secure-multitenant-foundation/project-plan]]
- [[backlog/epics/tenantguard-secure-multitenant-foundation]]