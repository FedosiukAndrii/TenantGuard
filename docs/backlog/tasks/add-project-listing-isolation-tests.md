---
title: Add Project Listing Isolation Tests
type: task
status: todo
tags:
  - planning
  - task
created: 2026-04-01
updated: 2026-04-01
slug: add-project-listing-isolation-tests
story: project-listing-and-search-remain-tenant-scoped
feature: projects-tenant-module
---

# Add Project Listing Isolation Tests

## Goal

Verify that project listing and search never expose records across tenant boundaries.

## Implementation Notes

- Cover listing, filtering, search, and suspended tenant behavior.
- Use integration tests that operate with multiple tenants.

## Done When

- Integration tests prove listing and search isolation.
- Suspended tenant access is rejected.
- Cross-tenant leaks are asserted as failures.

## Related Notes

- [[backlog/stories/project-listing-and-search-remain-tenant-scoped]]
- [[backlog/features/projects-tenant-module]]