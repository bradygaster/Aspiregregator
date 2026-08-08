# Squad Routing

Work is routed to the specialist whose domain best matches the request.
Named-routing signals below are intent patterns, not exact strings.

| Domain / Signal | Route To |
|-----------------|----------|
| Architecture, sequencing, migration planning, cross-agent coordination, PR boundaries | Modernization Lead |
| .NET SDK/runtime upgrades, Aspire AppHost/ServiceDefaults, orchestration wiring, deployment manifests | Aspire Platform Engineer |
| Orleans grains, clustering, persistence providers, silo/client lifecycle, concurrency | Orleans Distributed Systems Engineer |
| HTTP API design, contracts, pagination, backend-for-frontend boundary | Application API Engineer |
| Feed discovery/parsing, refresh scheduling, retries, dedup, malformed-feed handling | Feed Pipeline Reliability Engineer |
| React/TypeScript UI, design system, accessibility, client state, reader workflows | React Product Experience Engineer |
| Threat modeling, SSRF, resource exhaustion, auth policy, outbound-network policy | Application Security Engineer |
| Test strategy/implementation, CI/CD, observability, dependency automation, release checks, contributor docs | Quality and Delivery Engineer |
| Session logging (automatic) | Scribe |
| Backlog health / stale work (automatic) | Ralph |
| Responsible AI / safety review (automatic) | Rai |
| "Team" or multi-domain question | Spawn Modernization Lead + 1-2 relevant specialists |
| Ambiguous | Modernization Lead triages and reassigns |

## Sequencing Notes

The Modernization Lead owns cross-cutting sequencing to avoid overlapping
edits:

1. Platform modernization (.NET/Aspire/Orleans package upgrades) should land
   before or alongside feature work that depends on the upgraded APIs.
2. The Application API Engineer's contracts should be defined early so the
   React Product Experience Engineer and Orleans engineer can build against a
   stable boundary.
3. Security and Quality work run continuously alongside all workstreams, not
   as a final pass.
