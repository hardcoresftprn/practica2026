# Cod PlantUML - Diagrame sdsAUTO (Ierarhie Admin/User)

În această variantă, **Administratorul** deține controlul total, moștenind toate permisiunile **Utilizatorului**, plus funcționalități exclusive de management.

---

## 1. Diagrama Use-Case

```plantuml
@startuml
left to right direction
skinparam packageStyle rectangle
skinparam monochrome true
skinparam shadowing false

actor "User" as BaseUser
actor "Utilizator (Read-Only)" as UserActor
actor "Administrator (Full Access)" as Admin

' Structura ierarhică conform cerinței
BaseUser -right-> UserActor
BaseUser -right-> Admin

rectangle "Sistem Management Service Auto (sdsAUTO)" {
    ' Funcționalități de vizualizare (Accesibile ambilor, dar singurele pentru Utilizator)
    usecase "Autentificare în Sistem" as UC1
    usecase "Vizualizare Listă Clienți" as UC_V1
    usecase "Vizualizare Date Vehicule" as UC_V2
    usecase "Vizualizare Status Comenzi" as UC_V3
    usecase "Vizualizare Istoric Service" as UC_V4
    
    ' Funcționalități de gestionare (Exclusiv Administrator)
    usecase "Gestionare Clienți (Adăugare/Editare)" as UC_G1
    usecase "Gestionare Vehicule (Adăugare/Editare)" as UC_G2
    usecase "Creare & Editare Comenzi Service" as UC_G3
    usecase "Actualizare Diagnostic & Status Reparație" as UC_G4
    usecase "Generare Facturi & Procesare Plăți" as UC_G5
    usecase "Gestionare Utilizatori & Permisiuni" as UC_G6
    usecase "Generare Rapoarte & Vizualizare Logs" as UC_G7
}

' Utilizatorul obișnuit (Mecanic/Vizitator) poate DOAR vizualiza
UserActor --> UC1
UserActor --> UC_V1
UserActor --> UC_V2
UserActor --> UC_V3
UserActor --> UC_V4

' Administratorul are control TOTAL asupra sistemului
Admin --> UC1
Admin --> UC_V1
Admin --> UC_V2
Admin --> UC_V3
Admin --> UC_V4
Admin --> UC_G1
Admin --> UC_G2
Admin --> UC_G3
Admin --> UC_G4
Admin --> UC_G5
Admin --> UC_G6
Admin --> UC_G7

@enduml
```

---

## 2. Diagrama de Clase

```plantuml
@startuml
skinparam monochrome true
skinparam shadowing false
hide circle
skinparam classAttributeIconSize 0

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
@enduml
```


---

## 3. Diagrama de Secvență (Generare Factură de către Admin)

```plantuml
@startuml
actor Administrator
participant "Interfață Management" as UI
participant "InvoicesController" as Ctrl
database "Baza de Date" as DB

Administrator -> UI : Accesează Comenzi Finalizate
UI -> Ctrl : CreateInvoice(orderId)
activate Ctrl

Ctrl -> DB : Salvează Factură
DB --> Ctrl : Confirmare

Ctrl --> UI : Afișează Factura
deactivate Ctrl
@enduml
```
