---
title: Implement Host Tenant Lifecycle API
type: task
status: todo
tags:
  - planning
  - task
created: 2026-04-01
updated: 2026-04-01
slug: implement-host-tenant-lifecycle-api
story: host-can-provision-and-suspend-tenants
feature: host-tenant-management
---

# Implement Host Tenant Lifecycle API

## Goal

Implement host-only endpoints and application flow for creating, activating, viewing, and suspending tenants.

## Implementation Notes

- Keep host endpoints outside tenant-context enforcement.
- Persist status transitions and actor metadata.
- Align responses with ProblemDetails conventions where applicable.

## Done When

- Host lifecycle endpoints exist and are wired through application and infrastructure layers.
- Host operations do not require `X-Tenant-Id`.
- Tenant status transitions are persisted correctly.

## Related Notes

- [[backlog/stories/host-can-provision-and-suspend-tenants]]
- [[backlog/features/host-tenant-management]]