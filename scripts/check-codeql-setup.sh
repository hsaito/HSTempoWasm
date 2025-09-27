#!/bin/bash
# CodeQL Setup Detection Script
# This script helps identify CodeQL configuration issues

echo "=== CodeQL Configuration Checker ==="
echo ""

# Check if we're in a GitHub Actions environment
if [ "$GITHUB_ACTIONS" = "true" ]; then
    echo "✓ Running in GitHub Actions"
    echo "Repository: $GITHUB_REPOSITORY"
    echo "Event: $GITHUB_EVENT_NAME"
    echo "Ref: $GITHUB_REF"
    echo ""
else
    echo "ℹ  Not running in GitHub Actions"
    echo ""
fi

# Check for CodeQL workflow files
echo "=== CodeQL Workflow Files ==="
if [ -f ".github/workflows/codeql-analysis.yml" ]; then
    echo "✓ Found: .github/workflows/codeql-analysis.yml"
else
    echo "✗ Missing: .github/workflows/codeql-analysis.yml"
fi

if [ -f ".github/workflows/codeql.yml" ]; then
    echo "✓ Found: .github/workflows/codeql.yml"
fi

if [ -f ".github/workflows/github-code-scanning.yml" ]; then
    echo "⚠  Found: .github/workflows/github-code-scanning.yml (may indicate default setup)"
fi

echo ""

# Check for CodeQL configuration files
echo "=== CodeQL Configuration Files ==="
if [ -f ".github/codeql-config.yml" ]; then
    echo "✓ Found: .github/codeql-config.yml"
else
    echo "ℹ  No custom CodeQL config found (this is normal)"
fi

echo ""

# Instructions
echo "=== Next Steps ==="
echo "1. Go to: https://github.com/$GITHUB_REPOSITORY/settings/security_analysis"
echo "2. Find 'CodeQL analysis' section"
echo "3. If it shows 'Default setup: Active', click 'Configure'"
echo "4. Select 'Advanced' instead of 'Default'"
echo "5. Confirm the change"
echo ""
echo "After making this change, re-run your workflow."