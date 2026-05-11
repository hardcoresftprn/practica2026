# Diagrame UML - Aplicație Management Service Auto (sdsAUTO)

Acest document conține diagramele UML actualizate, unde **Administratorul** deține toate drepturile sistemului, iar **Utilizatorul** are acces limitat la operațiunile de bază.

---

## 1. Descriere Comportamentală

### 1.1 Diagrama Use-Case (Imagine Generală)
Administratorul moștenește toate funcționalitățile Utilizatorului, având în plus acces la gestionarea sistemului și a rapoartelor avansate.

```mermaid
useCaseDiagram
    actor "User" as BaseUser
    actor "Utilizator" as UserActor
    actor "Administrator" as Admin

    BaseUser --> UserActor
    BaseUser --> Admin

    package "Sistem Management Service Auto (sdsAUTO)" {
        usecase "Autentificare & Autorizare" as UC1
        usecase "Gestionare Clienți" as UC3
        usecase "Gestionare Vehicule" as UC4
        usecase "Creare Comandă Service" as UC5
        usecase "Actualizare Status & Diagnostic" as UC7
        usecase "Configurare Profil Personal" as UC13
        usecase "Gestionare Utilizatori" as UC2
        usecase "Generare Factură & Plăți" as UC9
        usecase "Vizualizare Rapoarte" as UC11
        usecase "Configurare Parametri Sistem" as UC12
        usecase "Vizualizare Loguri Sistem (Logs)" as UC14
    }

    UserActor --> UC1
    UserActor --> UC3
    UserActor --> UC4
    UserActor --> UC5
    UserActor --> UC7
    UserActor --> UC13

    Admin --> UC2
    Admin --> UC9
    Admin --> UC11
    Admin --> UC12
    Admin --> UC14
```

### 1.2 Diagrama de Activitate (Flux Procesare Comandă)
Fluxul de lucru rămâne neschimbat, fiind procesul operațional standard al service-ului.

```mermaid
stateDiagram-v2
    [*] --> ReceptieClient
    ReceptieClient --> InregistrareComanda
    InregistrareComanda --> Diagnosticare
    
    state Diagnosticare {
        [*] --> IdentificareProbleme
        IdentificareProbleme --> Reparatie
    }

    Reparatie --> FinalizareReparatie
    FinalizareReparatie --> GenerareFactura
    GenerareFactura --> ProcesarePlata
    ProcesarePlata --> InchidereComanda
    InchidereComanda --> [*]
```

---

## 2. Descriere Structurală

### 2.1 Diagrama de Clase
Administratorul și Utilizatorul sunt diferențiați prin câmpul `Role`.

```mermaid
classDiagram
    class User {
        +int Id
        +string Username
        +string Role
        +Login()
        +Logout()
        +UpdateProfile()
    }

    class Client {
        +int ClientId
        +string FirstName
        +string LastName
        +string Phone
        +RegisterVehicle(Vehicle v)
        +UpdateContactInfo()
        +GetOrderHistory()
    }

    class Vehicle {
        +int VehicleId
        +string VIN
        +string Brand
        +string Model
        +int ClientId
        +UpdateMileage()
        +GetMaintenanceRecords()
    }

    class ServiceOrder {
        +int OrderId
        +string OrderNumber
        +string Complaint
        +OrderStatus Status
        +int ClientId
        +int VehicleId
        +UpdateStatus(OrderStatus s)
        +AddDiagnosis(string d)
        +AssignMechanic(User u)
        +CloseOrder()
    }

    class Invoice {
        +int InvoiceId
        +int OrderId
        +decimal TotalAmount
        +CalculateTotal()
        +GeneratePdf()
        +SendToClient()
    }

    class Payment {
        +int PaymentId
        +int InvoiceId
        +decimal Amount
        +ProcessPayment()
        +ValidatePayment()
        +Refund()
    }

    Client "1" -- "0..*" Vehicle
    Vehicle "1" -- "0..*" ServiceOrder
    User "1" -- "0..*" ServiceOrder
    ServiceOrder "1" -- "0..1" Invoice
    Invoice "1" -- "1" Payment

```
