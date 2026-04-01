---
title: Configure Structured Logging And Correlation
type: task
status: todo
tags:
  - planning
  - task
created: 2026-04-01
updated: 2026-04-01
slug: configure-structured-logging-and-correlation
story: structured-logging-and-local-bootstrap-support-mvp
feature: operational-basics-and-critical-audit
---

# Configure Structured Logging And Correlation

## Goal

Provide structured logs with basic correlation and tenant-aware context for MVP troubleshooting.

## Implementation Notes

- Include `TraceId`, `TenantId`, and `UserId` where available.
- Keep OpenTelemetry out of MVP beyond future compatibility hooks.

## Done When

- Structured logging is configured for the API.
- Correlation and tenant metadata are present in logs where available.
- Logging remains simple enough for MVP operations.

## Related Notes

- [[backlog/stories/structured-logging-and-local-bootstrap-support-mvp]]
- [[backlog/features/operational-basics-and-critical-audit]]