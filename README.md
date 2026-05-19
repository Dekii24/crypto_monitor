# crypto_monitor

A modern, containerized .NET application with a fully automated DevOps infrastructure. This project demonstrates industry-standard practices in Continuous Integration, Continuous Deployment, and Cloud Infrastructure management.

## Architecture & CI/CD Workflow

Every time a code change is pushed to the `main` branch, the entire build-and-deploy pipeline triggers automatically with zero human intervention:

1. **Continuous Integration (GitHub Actions):** * Triggers on `git push`.
   * Spins up an ephemeral Ubuntu runner.
   * Builds the .NET application inside a Docker container using a multi-stage `Dockerfile`.
2. **Container Registry (GHCR):**
   * Upon a successful build, the production-ready Docker image is tagged and securely pushed to the **GitHub Container Registry (GHCR)**.
3. **Continuous Deployment (AWS EC2):**
   * The runner safely connects to an **Amazon Web Services EC2** instance via automated SSH.
   * Inside the AWS server, it pulls the latest Docker image from GHCR.
   * Seamlessly stops and removes the old container, and spins up the new version on port `80`, minimizing downtime.


## Tech Stack & Skills Demonstrated

* **Backend:** .NET / C#
* **Containerization:** Docker (Multi-stage builds, image optimization, port mapping)
* **CI/CD Automation:** GitHub Actions (Workflows, Environment Secrets, Automated SSH deployment)
* **Cloud Infrastructure:** Amazon Web Services (AWS EC2, Security Groups/Firewall management, EBS Storage scaling)
* **Artifact Management:** GitHub Container Registry
* **OS:** Linux (Ubuntu Server administration via SSH/CLI)
  

## Repository Structure

* `CryptoMonitor/` - The C# source code and application logic.
* `CryptoMonitor/Dockerfile` - Blueprint for containerizing the application.
* `.github/workflows/` - The CI/CD pipeline definitions.

**Live Demo:** http://13.53.131.85/
