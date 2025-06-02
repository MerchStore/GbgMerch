# GbgMerch

## Beskrivning
GbgMerch är en webbaserad merchbutik där användare kan se produkter, lägga till recensioner, och administratörer kan hantera produktkatalogen. Projektet är byggt med Clean Architecture och använder MongoDB som databas. Externa recensioner hämtas via ett separat Review API.

## Funktioner
- Visa produkter med information och recensioner
- Lägg till produkter via olika lägen (produkt-ID, med URL eller detaljer)
- Lägg till egna produktrecensioner
- Autentisering via token (JWT) för API-anrop
- Resilient integration med Review API med hjälp av Circuit Breaker-mönster
- Adminfunktioner för produkt- och recensionshantering (via API)

## Teknologier
- .NET 8 / C#
- MongoDB
- Polly (för Circuit Breaker)
- REST API
- JWT Autentisering

## Arkitektur
Projektet följer Clean Architecture med tydliga lager:
- **Domain:** Domänmodeller och gränssnitt
- **Application:** Affärslogik och serviceklasser
- **Infrastructure:** Implementeringar, externa tjänster, databaskopplingar
- **API:** Webb-API och controllers

## Installation och körning

### Förutsättningar
- .NET 8 SDK installerad
- MongoDB-anslutning (Atlas eller lokal)
- API-nyckel eller JWT-token för Review API
 