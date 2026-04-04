---
title: TenantGuard Secure Multi-Tenant Foundation Project Plan
type: project-plan
status: draft
tags:
  - planning
  - plan
created: 2026-04-01
updated: 2026-04-01
slug: tenantguard-secure-multitenant-foundation
related_prd: tenantguard-secure-multitenant-saas
---

# TenantGuard Secure Multi-Tenant Foundation Project Plan

## Scope

- Deliver the MVP defined by [[prd/tenantguard-secure-multitenant-saas]] as an API-first secure SaaS foundation.
- Complete all P0 features required to enforce tenant isolation, host and tenant boundaries, OIDC authentication, role-based authorization, and the Projects module.
- Complete P1 stories that are still required before MVP sign-off, especially critical audit coverage.
- Exclude v1.1 concerns such as policy-based authorization, service accounts, UI scope, and expanded observability.

## Milestones

### Milestone 1: Security Foundation

- [[backlog/features/tenant-context-and-isolation]]
- [[backlog/features/oidc-role-based-access]]

Exit criteria:

- Tenant endpoints enforce validated tenant context.
- Token tenant and header tenant mismatches return `403 Forbidden`.
- OIDC authentication and role-based authorization work for host and tenant roles.

### Milestone 2: Host And Tenant Core Flows

- [[backlog/features/host-tenant-management]]
- [[backlog/features/projects-tenant-module]]

Exit criteria:

- Host administrators can provision and suspend tenants.
- New tenants receive usable bootstrap setup.
- Tenant users can manage Projects within their own tenant.
- Listing and search remain tenant-scoped.

### Milestone 3: MVP Operability And Release Readiness

- [[backlog/features/operational-basics-and-critical-audit]]

Exit criteria:

- Structured logging and correlation support troubleshooting.
- Local bootstrap works from a clean environment.
- Critical actions are audited at MVP scope.
- Unit and integration tests cover isolation and authorization paths.

## Work Breakdown

### P0 Features

- [[backlog/features/tenant-context-and-isolation]]
- [[backlog/features/oidc-role-based-access]]
- [[backlog/features/host-tenant-management]]
- [[backlog/features/projects-tenant-module]]
- [[backlog/features/operational-basics-and-critical-audit]]

### P1 Stories

- [[backlog/stories/system-seeds-initial-tenant-admin]]
- [[backlog/stories/project-listing-and-search-remain-tenant-scoped]]
- [[backlog/stories/critical-actions-are-audited]]

### Dependency Order

1. [[backlog/stories/tenant-endpoints-require-validated-tenant-context]]
2. [[backlog/stories/persistence-enforces-tenant-isolation]]
3. [[backlog/stories/tenant-admin-authenticates-via-oidc]]
4. [[backlog/stories/roles-separate-host-and-tenant-operations]]
5. [[backlog/stories/host-can-provision-and-suspend-tenants]]
6. [[backlog/stories/system-seeds-initial-tenant-admin]]
7. [[backlog/stories/tenant-user-manages-projects-within-own-tenant]]
8. [[backlog/stories/project-listing-and-search-remain-tenant-scoped]]
9. [[backlog/stories/structured-logging-and-local-bootstrap-support-mvp]]
10. [[backlog/stories/critical-actions-are-audited]]

### Status Snapshot

- [[backlog/stories/tenant-endpoints-require-validated-tenant-context]] is completed: tenant-scoped requests now fail closed for anonymous access, enforce `X-Tenant-Id`, reject invalid or mismatched tenant claims, and are covered by integration tests.
- [[backlog/stories/persistence-enforces-tenant-isolation]] is completed: `ITenantScoped` interface marks tenant-scoped entities; `AppDbContext` applies a global query filter per tenant and a save guard that auto-assigns `TenantId` on new entities and rejects cross-tenant writes; absent tenant context on writes throws `InvalidOperationException`. Migration `AddProjectsTable` and full isolation test suite added.
- [[backlog/features/tenant-context-and-isolation]] is now completed: both the request-level validation and persistence-level isolation are in place.
- [[backlog/stories/structured-logging-and-local-bootstrap-support-mvp]] is partially implemented: request logging includes `TraceId` and `TenantId`, and development startup applies migrations automatically, but local seed/bootstrap data and the required test coverage are still missing.

### Task Breakdown

- [[backlog/tasks/implement-tenant-context-request-validation]]
- [[backlog/tasks/add-tenant-context-boundary-tests]]
- [[backlog/tasks/implement-tenant-query-filters-and-save-guards]]
- [[backlog/tasks/add-persistence-tenant-isolation-tests]]
- [[backlog/tasks/configure-oidc-authentication]]
- [[backlog/tasks/verify-tenant-admin-authentication-flow]]
- [[backlog/tasks/implement-role-based-authorization-rules]]
- [[backlog/tasks/add-role-boundary-authorization-tests]]
- [[backlog/tasks/implement-host-tenant-lifecycle-api]]
- [[backlog/tasks/add-host-tenant-lifecycle-integration-tests]]
- [[backlog/tasks/implement-tenant-bootstrap-seeding]]
- [[backlog/tasks/validate-tenant-bootstrap-flow]]
- [[backlog/tasks/implement-projects-crud-endpoints]]
- [[backlog/tasks/enforce-project-tenant-ownership-rules]]
- [[backlog/tasks/implement-project-listing-and-search]]
- [[backlog/tasks/add-project-listing-isolation-tests]]
- [[backlog/tasks/configure-structured-logging-and-correlation]]
- [[backlog/tasks/automate-local-bootstrap-migrations-and-seed-data]]
- [[backlog/tasks/implement-critical-audit-persistence]]
- [[backlog/tasks/audit-tenant-and-project-critical-events]]

## Risks

- Shared-schema isolation remains the highest technical risk and must be validated early with tests.
- OIDC setup may expand if provider-specific details leak into core delivery.
- Audit and operational work can grow beyond MVP if not kept tied to explicit acceptance criteria.
- Bootstrap and seeded data can become fragile unless exercised in integration flows.

## Related Notes

- [[prd/tenantguard-secure-multitenant-saas]]
- [[backlog/epics/tenantguard-secure-multitenant-foundation]]
- [[backlog/features/host-tenant-management]]
- [[backlog/features/tenant-context-and-isolation]]
- [[backlog/features/oidc-role-based-access]]
- [[backlog/features/projects-tenant-module]]
- [[backlog/features/operational-basics-and-critical-audit]]