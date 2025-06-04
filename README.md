SkyBooker - Flugbuchungssystem
==============================

Projektübersicht
-------------------

SkyBooker ist ein modernes, mikroservicebasiertes Flugbuchungssystem, das eine skalierbare und wartbare Architektur verwendet. Das System ermöglicht Benutzern, Flüge zu suchen, zu buchen und zu verwalten.

<br>

Systemarchitektur
--------------------

### Microservices

Das System besteht aus vier Hauptservices:

1.  Gateway Service (Port 5075)

-   Zentraler Eingangspunkt für alle Client-Anfragen

-   Routing und Load Balancing

-   API-Gateway-Funktionalität

-   Reverse Proxy

1.  Auth Service (Port 5058)

-   Benutzerauthentifizierung und -autorisierung

-   JWT-Token-Verwaltung

-   Benutzerverwaltung

-   Sicherheitsfeatures

1.  Flight Service (Port 5221)

-   Flugverwaltung und -suche

-   Verfügbarkeitsprüfung

-   Preisberechnung

-   MongoDB als Datenbank

1.  Book Service (Port 5128)

-   Buchungsverwaltung

-   Reservierungssystem

-   Buchungsbestätigungen

-   SQL Server als Datenbank

### Datenbanken

-   MongoDB (Port 27017)

-   Speichert Flugdaten

-   Optimiert für Leseoperationen und Suchanfragen

-   Flexible Dokumentenstruktur

-   SQL Server (Port 1433)

-   Speichert Buchungsdaten

-   ACID-Konformität für Buchungstransaktionen

-   Relationale Datenstruktur

<br>

Technologie-Stack
--------------------

### Backend

-   .NET Core - Hauptentwicklungsframework

-   Entity Framework Core - ORM für SQL Server

-   MongoDB Driver - für MongoDB-Interaktionen

-   JWT Authentication - für Sicherheit

-   Docker - Containerisierung

-   Docker Compose - Container-Orchestrierung

### Testing

-   xUnit - Testframework

-   Moq - Mocking-Framework

-   InMemory Database - für Integrationstests


<br>

Entwicklungsumgebung einrichten
----------------------------------

### Voraussetzungen

-   Docker Desktop

-   .NET 6.0 SDK oder höher

-   Visual Studio 2022 oder VS Code

-   Git

### Installation

1.  Repository klonen:

    bash

    Apply to AuthServiceT...

    Run

    git clone [repository-url]

    cd SkyBooker

1.  Docker-Container starten:

    bash

    Apply to AuthServiceT...

    Run

    docker-compose up -d

1.  Entwicklungsserver starten:

    bash

    Apply to AuthServiceT...

    Run

    dotnet run --project GatewayService

<br>

Konfiguration
----------------

### Umgebungsvariablen

Jeder Service hat seine eigenen Umgebungsvariablen:

Auth Service

-   JWT-Konfiguration

-   Datenbank-Verbindungsstring

Flight Service

-   MongoDB-Verbindungsstring

-   Service-Konfigurationen

Book Service

-   SQL Server-Verbindungsstring

-   Flight Service URL

Gateway

-   Service-URLs

-   Routing-Konfiguration

<br>

Tests
--------

### Test-Suites

-   AuthService.Tests

-   BookService.Tests

-   FlightService.Tests

-   GatewayService.Tests

### Testausführung

bash

Apply to AuthServiceT...

Run

dotnet test

<br>

Deployment
-------------

### Docker Deployment

1.  Images bauen:

    bash

    Apply to AuthServiceT...

    Run

    docker-compose build

1.  Services starten:

    bash

    Apply to AuthServiceT...

    Run

    docker-compose up -d

### Ports

-   Gateway: 5075

-   Auth Service: 5058

-   Flight Service: 5221

-   Book Service: 5128

-   MongoDB: 27017

-   SQL Server: 1433

<br>

Sicherheit
-------------

### Implementierte Sicherheitsmaßnahmen

-   JWT-basierte Authentifizierung

-   Verschlüsselte Passwörter

-   HTTPS-Unterstützung

-   API-Gateway-Sicherheit

-   Datenbank-Sicherheit

<br>

Monitoring und Logging
-------------------------

### Logging-System

-   Strukturiertes Logging

-   Service-spezifische Logs

-   Performance-Metriken

-   Fehler-Tracking

<br>

API-Endpunkte
----------------

### Gateway Service

-   /api/v1/* - Routing zu verschiedenen Services

### Auth Service

-   POST /api/auth/register - Benutzerregistrierung

-   POST /api/auth/login - Benutzeranmeldung

### Flight Service

-   GET /api/flights - Flüge auflisten

-   GET /api/flights/{id} - Flugdetails

-   POST /api/flights/search - Flugsuche

### Book Service

-   POST /api/bookings - Neue Buchung

-   GET /api/bookings/{id} - Buchungsdetails

-   GET /api/bookings/user/{userId} - Benutzerbuchungen

<br>

Workflow
-----------

### Typischer Buchungsablauf

1.  Benutzer authentifiziert sich

1.  Sucht nach Flügen

1.  Wählt einen Flug aus

1.  Erstellt eine Buchung

1.  Erhält Buchungsbestätigung

<br>

Dokumentation
----------------

### API-Dokumentation

-   Swagger/OpenAPI-Integration

-   Endpunkt-Beschreibungen

-   Beispielanfragen

### Code-Dokumentation

-   XML-Kommentare

-   Architektur-Dokumentation

-   Datenbankschemas

<br>

Beitragen
------------

### Entwicklungsrichtlinien

1.  Fork des Repositories

1.  Feature-Branch erstellen

1.  Änderungen committen

1.  Pull Request erstellen

### Code-Standards

-   C# Coding Conventions

-   Clean Code-Prinzipien

-   SOLID-Prinzipien

<br>

Support
----------

### Kontakt

-   Technischer Support: [Kontakt]

-   Projektmanagement: [Kontakt]

-   Entwicklerteam: [Kontakt]

<br>

Roadmap
----------

### Geplante Features

1.  Mehrsprachige Unterstützung

1.  Erweiterte Buchungsoptionen

1.  Mobile App-Integration

1.  Erweiterte Reporting-Funktionen
