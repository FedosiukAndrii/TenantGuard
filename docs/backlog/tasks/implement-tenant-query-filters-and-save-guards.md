---
title: Implement Tenant Query Filters And Save Guards
type: task
status: todo
tags:
  - planning
  - task
created: 2026-04-01
updated: 2026-04-01
slug: implement-tenant-query-filters-and-save-guards
story: persistence-enforces-tenant-isolation
feature: tenant-context-and-isolation
---

# Implement Tenant Query Filters And Save Guards

## Goal

Apply shared-schema tenant isolation consistently in persistence using query filters and write guards.

## Implementation Notes

- Ensure tenant-scoped entities carry `TenantId`.
- Apply automatic query filtering by tenant.
- Reject or guard writes without tenant context.

## Done When

- Tenant filters are active for tenant-scoped entities.
- Save operations assign or validate `TenantId`.
- Missing tenant context causes persistence to fail fast.

## Related Notes

- [[backlog/stories/persistence-enforces-tenant-isolation]]
- [[backlog/features/tenant-context-and-isolation]]