---
title: Tenant Endpoints Require Validated Tenant Context
type: story
status: completed
tags:
  - planning
  - story
created: 2026-04-01
updated: 2026-04-01
slug: tenant-endpoints-require-validated-tenant-context
feature: tenant-context-and-isolation
epic: tenantguard-secure-multitenant-foundation
priority: P0
---

# Tenant Endpoints Require Validated Tenant Context

## Story
As a platform owner, I want every tenant-scoped endpoint to require validated tenant context, so that tenant context cannot be bypassed.

## Acceptance Criteria

- Tenant endpoints require a valid `X-Tenant-Id`.
- Unauthenticated tenant-scoped requests return `401 Unauthorized`.
- Missing or invalid tenant headers return ProblemDetails responses.
- Header tenant and token tenant mismatches return `403 Forbidden`.
- Authenticated tenant requests without a valid tenant claim return `403 Forbidden`.
- Host endpoints are excluded from tenant context requirements.

## Notes

- This story covers request-time tenant validation and boundary handling.
- Implemented in API middleware with integration coverage for anonymous tenant access, invalid header, missing header, invalid tenant claim, tenant-claim mismatch, missing tenant claim, and host-path bypass behavior.

## Tasks

- [[backlog/tasks/implement-tenant-context-request-validation]]
- [[backlog/tasks/add-tenant-context-boundary-tests]]

## Related Notes

- [[backlog/features/tenant-context-and-isolation]]
- [[backlog/epics/tenantguard-secure-multitenant-foundation]]