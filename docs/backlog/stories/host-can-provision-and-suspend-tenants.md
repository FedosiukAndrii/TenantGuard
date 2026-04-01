---
title: Host Can Provision And Suspend Tenants
type: story
status: proposed
tags:
  - planning
  - story
created: 2026-04-01
updated: 2026-04-01
slug: host-can-provision-and-suspend-tenants
feature: host-tenant-management
epic: tenantguard-secure-multitenant-foundation
priority: P0
---

# Host Can Provision And Suspend Tenants

## Story
As a host administrator, I want to create, activate, and suspend tenants, so that I can onboard and control tenant lifecycle safely.

## Acceptance Criteria

- Host-only endpoints support create, activate, and suspend actions.
- Host endpoints do not require `X-Tenant-Id`.
- Tenant lifecycle changes persist status and actor metadata.
- Suspended tenants cannot access tenant-scoped endpoints.

## Notes

- This story defines the bounded host-only API surface for tenant lifecycle.

## Tasks

- [[backlog/tasks/implement-host-tenant-lifecycle-api]]
- [[backlog/tasks/add-host-tenant-lifecycle-integration-tests]]

## Related Notes

- [[backlog/features/host-tenant-management]]
- [[backlog/epics/tenantguard-secure-multitenant-foundation]]