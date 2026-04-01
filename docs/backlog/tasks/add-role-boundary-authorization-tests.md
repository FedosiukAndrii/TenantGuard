---
title: Add Role Boundary Authorization Tests
type: task
status: todo
tags:
  - planning
  - task
created: 2026-04-01
updated: 2026-04-01
slug: add-role-boundary-authorization-tests
story: roles-separate-host-and-tenant-operations
feature: oidc-role-based-access
---

# Add Role Boundary Authorization Tests

## Goal

Validate that role boundaries correctly separate host and tenant operations.

## Implementation Notes

- Cover host-only operations, tenant-only operations, and insufficient role cases.
- Assert `403 Forbidden` for unauthorized access.

## Done When

- Integration tests verify role separation across key endpoints.
- Host and tenant misuse cases are covered.
- Authorization test suite is stable in local execution.

## Related Notes

- [[backlog/stories/roles-separate-host-and-tenant-operations]]
- [[backlog/features/oidc-role-based-access]]