---
title: Add Tenant Context Boundary Tests
type: task
status: completed
tags:
  - planning
  - task
created: 2026-04-01
updated: 2026-04-01
slug: add-tenant-context-boundary-tests
story: tenant-endpoints-require-validated-tenant-context
feature: tenant-context-and-isolation
---

# Add Tenant Context Boundary Tests

## Goal

Prove that tenant context enforcement cannot be bypassed on tenant-scoped endpoints.

## Implementation Notes

- Cover missing header, invalid header, and mismatched token/header cases.
- Cover anonymous tenant requests and malformed tenant claims.
- Cover authenticated requests that omit the tenant claim.
- Assert host endpoint exclusions remain intentional.

## Done When

- Integration tests cover tenant boundary enforcement.
- `401 Unauthorized` behavior is asserted for unauthenticated tenant requests.
- `403 Forbidden` behavior is asserted for mismatches.
- `403 Forbidden` behavior is asserted for authenticated requests missing a tenant claim.
- `403 Forbidden` behavior is asserted for malformed tenant claims.
- ProblemDetails responses are asserted for invalid requests.

## Related Notes

- [[backlog/stories/tenant-endpoints-require-validated-tenant-context]]
- [[backlog/features/tenant-context-and-isolation]]