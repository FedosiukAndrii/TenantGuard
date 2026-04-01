---
title: Add Host Tenant Lifecycle Integration Tests
type: task
status: todo
tags:
  - planning
  - task
created: 2026-04-01
updated: 2026-04-01
slug: add-host-tenant-lifecycle-integration-tests
story: host-can-provision-and-suspend-tenants
feature: host-tenant-management
---

# Add Host Tenant Lifecycle Integration Tests

## Goal

Verify host tenant lifecycle behavior end to end, including host-only access and suspended tenant blocking.

## Implementation Notes

- Cover create, activate, suspend, and access boundary cases.
- Assert tenant endpoints reject suspended tenants after lifecycle changes.

## Done When

- Integration tests cover host-only lifecycle routes.
- Suspended tenant behavior is validated.
- Failure responses are asserted for invalid access paths.

## Related Notes

- [[backlog/stories/host-can-provision-and-suspend-tenants]]
- [[backlog/features/host-tenant-management]]