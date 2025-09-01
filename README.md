# \# RavenTalesSolutions

# 

# RavenTalesSolutions is a collection of microservices and applications that together form the \*\*RavenTales\*\* language learning platform.

# 

# \## 📂 Repository Structure



RavenTalesSolutions/

├── StoryService/ # Generates language learning stories

├── TranslatorService/ # Handles word/sentence translations

├── UserAuthService/ # Authentication and user management

├── VocabularyService/ # Flashcards \& spaced repetition

├── DictionaryService/ # (Optional) dictionary / lexicon service

├── FrontendApp/ # React frontend for end users

├── Shared/ # Shared DTOs, contracts, and utilities

└── Infra/ # Docker/K8s/Terraform/CI/CD configs

\## 🚀 Services



\- \*\*StoryService\*\*  

&nbsp; Generates short stories tailored to user language level and topic using AI.



\- \*\*TranslatorService\*\*  

&nbsp; Provides translation APIs (Google/DeepL/LibreTranslate, configurable).



\- \*\*UserAuthService\*\*  

&nbsp; Handles registration, login, and JWT authentication.



\- \*\*VocabularyService\*\*  

&nbsp; Manages user flashcards and spaced repetition for vocabulary practice.



\- \*\*DictionaryService\*\* \*(planned)\*  

&nbsp; Provides definitions, synonyms, and lexicon functionality.



\- \*\*FrontendApp\*\*  

&nbsp; Web interface (React) for users to interact with the platform.



\## ⚙️ Infrastructure



\- \*\*Shared/\*\* contains code common to multiple services.  

\- \*\*Infra/\*\* contains deployment and orchestration files:

&nbsp; - `docker-compose.yml`

&nbsp; - `k8s-manifests/`

&nbsp; - `terraform/`



\## 📝 License



This project is licensed under the terms described in \[LICENSE](LICENSE).

