---
title: Roles Separate Host And Tenant Operations
type: story
status: proposed
tags:
  - planning
  - story
created: 2026-04-01
updated: 2026-04-01
slug: roles-separate-host-and-tenant-operations
feature: oidc-role-based-access
epic: tenantguard-secure-multitenant-foundation
priority: P0
---

# Roles Separate Host And Tenant Operations

## Story
As a platform owner, I want host and tenant operations to be separated by roles, so that privileged access is explicit and limited.

## Acceptance Criteria

- `HostAdmin`, `TenantAdmin`, and `TenantUser` roles are defined for MVP.
- Host-only operations require explicit host privileges.
- Tenant-level operations reject users with insufficient roles using `403 Forbidden`.
- Host access to tenant data is allowed only through explicitly bounded flows.

## Notes

- Policy-based and resource-based authorization are deferred to post-MVP.

## Tasks

- [[backlog/tasks/implement-role-based-authorization-rules]]
- [[backlog/tasks/add-role-boundary-authorization-tests]]

## Related Notes

- [[backlog/features/oidc-role-based-access]]
- [[backlog/epics/tenantguard-secure-multitenant-foundation]]