# 🎵 MyMusicManager

Application de gestion de collection musicale (CD/Vinyles) développée dans le cadre du **LOTJ : Développer une application informatique**.

Ce projet est une application lourde (Desktop) permettant de gérer des albums, des artistes, de rechercher des informations via une API externe et de générer des rapports PDF.

## 🚀 Fonctionnalités Principales

1.  **Authentification Sécurisée :**
    * Connexion avec identifiant/mot de passe.
    * *Compétence validée :* Utilisation de **SQL Pur (ADO.NET)** pour la vérification des accès.

2.  **Gestion de la Collection (CRUD) :**
    * Affichage de la liste des albums avec images, prix, artistes.
    * Données persistantes dans une base de données **MySQL (WampServer)**.
    * *Compétence validée :* Utilisation d'un **ORM (Entity Framework Core)** pour la gestion des données.

3.  **Enrichissement via API (Service Tiers) :**
    * Recherche d'albums en temps réel via l'**API iTunes**.
    * Récupération automatique des métadonnées (Titre, Artiste, Année, Image de la pochette).

4.  **Exportation :**
    * Génération d'un catalogue au format **PDF**.
    * *Compétence validée :* Intégration de la bibliothèque tierce **PdfSharp**.

## 🛠️ Stack Technique

* **Langage :** C# (.NET 9.0)
* **Interface :** WPF (Windows Presentation Foundation)
* **Base de Données :** MySQL (via WampServer)
* **ORM :** Entity Framework Core (Pomelo.MySQL)
* **Bibliothèques :**
    * `Newtonsoft.Json` (Traitement API)
    * `PdfSharp` (Génération PDF)
    * `System.Drawing.Common` (Support graphique)

## ⚙️ Installation et Lancement

**Prérequis :**
* .NET SDK 9.0
* WampServer (ou un serveur MySQL local)

**Configuration Base de données :**
1.  Lancer WampServer.
2.  Créer une base de données nommée `mymusicmanager`.
3.  Importer le script SQL (disponible dans le dossier `docs` ou à la racine).
4.  Identifiants par défaut : `user: root`, `password: root`.

**Lancer l'application :**
```bash
git clone [https://github.com/TON_PSEUDO/MyMusicManager.git](https://github.com/TON_PSEUDO/MyMusicManager.git)
cd MyMusicManager
dotnet restore
dotnet run
---

### Étape 2 : Envoyer la version finale sur GitHub

Maintenant que le code marche et que le README est prêt, on envoie tout.

Ouvre ton terminal VS Code et tape :

1.  **Ajouter les nouveaux fichiers (le PDF, le Readme, les correctifs) :**
    ```powershell
    git add .
    ```

2.  **Enregistrer la version (Commit) :**
    ```powershell
    git commit -m "Version Finale : Ajout Export PDF et Documentation README"
    ```

3.  **Envoyer vers GitHub (Push) :**
    ```powershell
    git push
    ```

---

### Étape 3 : Vérification

Va sur la page de ton dépôt GitHub dans ton navigateur.
Tu devrais voir :
1.  Ton code à jour.
2.  En dessous, le beau texte explicatif (le README) qui s'affiche automatiquement.

---

### 🎁 Bonus : Préparation pour l'Oral (20 min)

Puisque c'est un LOTJ avec soutenance, voici le plan idéal pour ta présentation en t'appuyant sur ce projet :

**1. Introduction (2 min)**
* Présente-toi.
* Explique le but de l'application : "Aider les collectionneurs à gérer leurs albums".
* Montre le schéma de la base de données (Artiste 1---n Album).

**2. Démonstration Technique (8 min) - *Le moment clé***
* Lance l'appli devant eux.
* **Login :** "Ici, j'utilise du SQL pur pour vérifier le mot de passe admin."
* **Dashboard :** Montre la liste.
* **API (L'effet Wow) :** "Je vais ajouter un album. Je tape 'Daft Punk'. Regardez, ça vient d'iTunes." -> Tu l'enregistres.
* **PDF :** "Pour finir, j'imprime ma collection." -> Clique sur le bouton et montre le PDF ouvert.

**3. Analyse du Code (5 min)**
* Ouvre VS Code.
* Montre la classe `AddAlbumWindow.xaml.cs` pour expliquer comment tu appelles l'API (le `HttpClient`).
* Montre le fichier `Models` pour expliquer l'ORM (Entity Framework).

**4. Gestion de Projet (3 min)**
* Montre ton GitHub (l'historique des commits).
* Explique les difficultés (ex: "J'ai eu des soucis de version avec PdfSharp, j'ai dû passer en 1.50").

**5. Questions/Réponses (2 min)**

Tu es paré ! C'est un projet solide, propre et fonctionnel. **Bonne chance pour la soutenance !** 🚀