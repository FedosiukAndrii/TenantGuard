---
title: Host Tenant Management
type: feature
status: proposed
tags:
  - planning
  - feature
created: 2026-04-01
updated: 2026-04-01
slug: host-tenant-management
epic: tenantguard-secure-multitenant-foundation
priority: P0
---

# Host Tenant Management

## Summary

Provide bounded host-only capabilities for creating, viewing, activating, and suspending tenants without requiring tenant context on host endpoints.

## User Value

Host administrators can onboard customers and control tenant lifecycle safely without crossing tenant boundaries implicitly.

## Acceptance Criteria

- Host-only endpoints exist for create, view, activate, and suspend tenant flows.
- Host endpoints do not require `X-Tenant-Id`.
- Tenant lifecycle changes capture actor and timestamp metadata.
- Suspended tenants are blocked from tenant-scoped usage.

## Stories

- [[backlog/stories/host-can-provision-and-suspend-tenants]]
- [[backlog/stories/system-seeds-initial-tenant-admin]]

## Tasks

- [[backlog/tasks/implement-host-tenant-lifecycle-api]]
- [[backlog/tasks/add-host-tenant-lifecycle-integration-tests]]
- [[backlog/tasks/implement-tenant-bootstrap-seeding]]
- [[backlog/tasks/validate-tenant-bootstrap-flow]]

## Related Notes

- [[backlog/epics/tenantguard-secure-multitenant-foundation]]
- [[prd/tenantguard-secure-multitenant-saas]]