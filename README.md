# RavenTalesSolutions

RavenTalesSolutions is a collection of microservices and a frontend application that together form **RavenTales**, a language learning platform.  
The goal of the system is to help learners improve their language skills by reading AI-generated stories, instantly translating words, saving vocabulary, and practicing flashcards with spaced repetition.

This repository is structured as a **mono-repo**: each service lives in its own folder but shares infrastructure and deployment tooling.

---

## 📂 Repository Structure

RavenTalesSolutions/

├── UserAuthService/ # Authentication and user management
|
├── ProfileService/ # Stores user learning preferences (languages, CEFR level)
|
├── StoryService/ # Generates stories, tracks saved stories and reading history
|
├── TranslatorService/ # Provides word/sentence translations
|
├── VocabularyService/ # Flashcards & spaced repetition
|
├── DictionaryService/ # (Optional) dictionary / lexicon service
|
├── FrontendApp/ # React frontend for end users
|
├── Shared/ # Shared DTOs, contracts, utilities
|
└── Infra/ # Docker/K8s/Terraform/CI/CD configs

## 🚀 Services

### UserAuthService

Handles **identity and authentication** across the system.

- Provides user registration, login, and logout.
- Issues **JWT tokens** that secure communication between services.
- Other services validate tokens to associate actions with a specific user.
- Future extensions: password reset, role-based access, integration with OAuth providers.

---

### ProfileService

Handles **user preferences only**.

- Stores learning preferences such as:
  - Preferred target language(s).
  - CEFR level (A1–C2).
  - Optional UI/display settings.
- Lightweight by design: does not manage stories, vocabulary, or history.
- Used by other services to adapt generated content to each learner.

---

### StoryService

Owns **all story content and user interactions with stories**.

- Generates AI-powered stories based on topic, CEFR level, and target language.
- Persists story data including metadata (creator, createdAt, topic, language level).
- Manages user interactions:
  - **Saved Stories** → tracks which users bookmarked which stories.
  - **Reading History** → logs which users viewed which stories and when.
- Provides the foundation for analytics and personalized recommendations.

---

### TranslatorService

Dedicated to **translations**.

- Returns word-by-word and sentence-level translations.
- Configurable backends: Google Translate, DeepL, LibreTranslate (self-hosted).
- Standalone service so both frontend and backend services (e.g., VocabularyService) can request translations without depending directly on third-party APIs.

---

### VocabularyService

Handles **vocabulary management and practice**.

- Users can save words/phrases (often from StoryService).
- Stores translations, example sentences, and review metadata.
- Implements **spaced repetition scheduling (SRS)** to optimize long-term retention.
- Allows structured review sessions, tracking progress over time.

---

### DictionaryService _(optional / future)_

Provides **dictionary/lexicon capabilities**.

- Complements TranslatorService with richer data: definitions, synonyms, grammatical info.
- Could integrate with external dictionary APIs or maintain a local lexicon.
- Useful for advanced learners seeking more depth than basic translations.

---

### FrontendApp

Learner-facing **web application built with React**.

- Provides login/registration (via UserAuthService).
- Allows reading and generating stories (from StoryService).
- Enables clicking words for instant translations (TranslatorService).
- Lets learners save words to flashcards (VocabularyService).
- Displays vocabulary practice sessions and user preferences (ProfileService).
- Ties all backend services into a single, cohesive experience.

---

## ⚙️ Shared and Infrastructure

### Shared/

Contains libraries and code used across multiple services:

- Common DTOs and contracts (only if used across services).
- Utility helpers (logging, error handling, constants).

### Infra/

Holds deployment and orchestration configurations:

- **docker-compose.yml** → run all services locally with one command.
- **k8s-manifests/** → Kubernetes YAML manifests for production deployment.
- **terraform/** → Infrastructure-as-Code for cloud provisioning.
- **CI/CD pipelines** → automated builds and deployments.

---

## 📝 License

**All Rights Reserved** – this code may not be copied, used, or distributed without explicit permission from the author.
