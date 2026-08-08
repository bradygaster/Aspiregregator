# 🧑‍🤝‍🧑 Meet Your Squad

> Your AI development team, powered by [Squad](https://github.com/bradygaster/squad).

## The Team

This squad was cast to modernize Aspiregregator: finish the Orleans-based
backend, upgrade the .NET/Aspire/Orleans platform stack, and replace the
Blazor UI with a modern React experience. Per the casting brief in
[issue #8](../../issues/8), members use **descriptive functional role names**
— no fictional personas.

| Name | Role | Specialty | How to Talk to Them |
|------|------|-----------|---------------------|
| 🏗️ Modernization Lead | Modernization Lead | Sequencing, architecture, cross-agent coordination, migration safety | `squad:modernization-lead` label or mention in issue |
| ⚙️ Aspire Platform Engineer | Aspire Platform Engineer | .NET/Aspire upgrades, AppHost/ServiceDefaults, orchestration, deployment manifests | `squad:aspire-platform-engineer` label or mention in issue |
| 🔧 Orleans Distributed Systems Engineer | Orleans Distributed Systems Engineer | Orleans grains, clustering, persistence, lifecycle, concurrency | `squad:orleans-distributed-systems-engineer` label or mention in issue |
| 🔧 Application API Engineer | Application API Engineer | Typed HTTP API / BFF boundary between React and Orleans, contracts, pagination | `squad:application-api-engineer` label or mention in issue |
| 🔧 Feed Pipeline Reliability Engineer | Feed Pipeline Reliability Engineer | Feed discovery/parsing, scheduling, retries, dedup, failure isolation | `squad:feed-pipeline-reliability-engineer` label or mention in issue |
| ⚛️ React Product Experience Engineer | React Product Experience Engineer | React/TypeScript rebuild, design system, accessibility, reader workflows | `squad:react-product-experience-engineer` label or mention in issue |
| 🔒 Application Security Engineer | Application Security Engineer | Threat modeling, SSRF/resource-exhaustion mitigation, auth and network policy | `squad:application-security-engineer` label or mention in issue |
| 🧪 Quality and Delivery Engineer | Quality and Delivery Engineer | Test strategy, CI/CD modernization, observability, release checks, docs | `squad:quality-and-delivery-engineer` label or mention in issue |

### Always-On Support

| Name | Role | Specialty | How to Talk to Them |
|------|------|-----------|---------------------|
| 📋 Scribe | Session Logger | Tracking all agent sessions | Automatic — never needs explicit routing |
| 🔄 Ralph | Work Monitor | Backlog health and stale work alerts | Automatic — watches for idle work |
| 🛡️ Rai | RAI Reviewer | Responsible AI and safety review | Automatic — reviews high-risk output |

## How to Work With Your Squad

### Label-Based Assignment

Apply a `squad:{name}` label to any issue or PR to route it directly to that
specialist. For example, `squad:react-product-experience-engineer` sends work
to the React rebuild engineer.

### Iteration Commands

| Command | What It Does |
|---------|--------------|
| `/squad cast` | Re-cast the full team (replaces current squad) |
| `/squad cast-member <spec>` | Add or modify a single team member |
| `/squad retire <name>` | Remove a team member from the roster |
| `/squad status` | Check current team composition and health |

### Routing

Work is routed automatically via `.squad/routing.md`. Each member has a
**charter** (`.squad/agents/{lowercase-name}/charter.md`) defining their
expertise, boundaries, and scope.

## What Happened Here

This team was assembled directly from the explicit team specification in
[issue #8](../../issues/8), which is the source of truth for this cast:

- **Repo baseline:** .NET 9 Blazor Interactive Server app orchestrated by
  Aspire, with Orleans grains backed by Azurite tables/blobs and a background
  feed updater. Aspire and Orleans packages are significantly out of date
  (Aspire pinned at `9.0.0-rc.1`, current is `13.4.6`; Orleans pinned at
  `8.2.0`, current is `10.2.2`), CI drift exists between the SDK version and
  installed tooling, there are no test projects, and the frontend has
  unused SQL Server/EF Core references with no implemented `DbContext`.
- **Rationale:** The issue explicitly required 8 named roles — Modernization
  Lead, Aspire Platform Engineer, Orleans Distributed Systems Engineer,
  Application API Engineer, Feed Pipeline Reliability Engineer, React Product
  Experience Engineer, Application Security Engineer, and Quality and
  Delivery Engineer — with descriptive names only (no fictional personas).
  This squad follows that spec exactly, replacing the generic preset team
  scaffolded during activation.

---

*Cast on 2026-08-08 for bradygaster/Aspiregregator*
