# Security Policy

Thank you for helping keep HSTempoWasm secure.

## Community & Best-Effort

HSTempoWasm is a community-maintained project. Response times, fixes, and
remediation are provided on a best-effort basis by volunteer maintainers.
The timelines and processes below are guidance rather than guarantees and may
vary based on severity, complexity, and maintainer availability.

## Reporting a Vulnerability

- Please report privately via GitHub Security Advisories: https://github.com/hsaito/HSTempoWasm/security/advisories/new
- Include a clear description, impact, reproducible steps, affected version(s)/commit(s), and environment details.
- Do not create public issues for security vulnerabilities.
- We aim to acknowledge reports within 3 business days (best effort) and
  provide periodic status updates subject to maintainer availability.
- We will coordinate disclosure and can credit reporters upon request.

## Supported Versions

- Security fixes target the `main` branch and the latest stable release. Older releases may receive fixes at the maintainers’ discretion based on severity and feasibility.

## Disclosure & Remediation Timeline (Best-Effort Guidance)

- Triage & initial assessment: within 7 days.
- Severity based on CVSS and practical exploitability.
- Targeted remediation timelines (best effort):
  - Critical/High: aim for fix or mitigation within 30 days.
  - Medium: aim within 60 days.
  - Low: aim within 90 days.
- Timelines may adjust for complexity, upstream dependencies, or broader coordination needs.

## Testing Guidelines (Responsible Research)

- Only non-destructive testing; do not perform DDoS, spam, or social engineering.
- Do not access or exfiltrate data you do not own.
- Avoid automated scanning of third-party or production infrastructure.
- If you believe testing could impact availability, please coordinate privately first via the advisory form.

## Dependencies

- If the vulnerability is in a third-party dependency, please include the package name, version, and any relevant upstream advisories.
- This project uses Dependabot to help keep dependencies up-to-date.

## Security Tooling

- We aim to use GitHub code scanning (e.g., CodeQL) and SAST tools to detect issues early in development.
- Contributors are encouraged to run local static analysis and address findings before submitting PRs.

## Contact

- Preferred: GitHub Security Advisory form — https://github.com/hsaito/HSTempoWasm/security/advisories/new
- If you cannot access the form, please open a minimal issue that states you have a security report and request maintainer contact; we will convert to a private discussion.

## Confidentiality

- Details shared privately are treated as confidential until a coordinated public disclosure is agreed.
