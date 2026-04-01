---
title: Critical Actions Are Audited
type: story
status: proposed
tags:
  - planning
  - story
created: 2026-04-01
updated: 2026-04-01
slug: critical-actions-are-audited
feature: operational-basics-and-critical-audit
epic: tenantguard-secure-multitenant-foundation
priority: P1
---

# Critical Actions Are Audited

## Story
As an operator, I want critical actions to be audited, so that security-relevant changes can be investigated later.

## Acceptance Criteria

- Audit coverage includes tenant lifecycle events.
- Audit coverage includes role changes.
- Audit coverage includes key domain actions in Projects.
- Audit storage excludes broader enterprise-grade scope in MVP.

## Notes

- This story intentionally keeps audit scope narrow for MVP.

## Tasks

- [[backlog/tasks/implement-critical-audit-persistence]]
- [[backlog/tasks/audit-tenant-and-project-critical-events]]

## Related Notes

- [[backlog/features/operational-basics-and-critical-audit]]
- [[backlog/epics/tenantguard-secure-multitenant-foundation]]